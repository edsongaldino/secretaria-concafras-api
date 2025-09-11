using AutoMapper;
using MercadoPago.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecretariaConcafras.Application.Interfaces;
using SecretariaConcafras.Application.Interfaces.Services;
using SecretariaConcafras.Application.Mappings;
using SecretariaConcafras.Application.Options;
using SecretariaConcafras.Application.Services;
using SecretariaConcafras.Domain.Exceptions;
using SecretariaConcafras.Domain.Interfaces;
using SecretariaConcafras.Infrastructure;
using SecretariaConcafras.Infrastructure.Repositories;
using SecretariaConcafras.SharedKernel.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurações
var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

// Serviços
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// Repositório genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPagamentoService, PagamentoService>();

// Registro automático com Scrutor
builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(IUsuarioService).Assembly,     // Application
        typeof(ApplicationDbContext).Assembly, // Infrastructure
        typeof(ITokenService).Assembly        // SharedKernel
    )
    .AddClasses(c => c.Where(type => type.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    .AddClasses(c => c.Where(type => type.Name.EndsWith("Repository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);


// Configuração do MpOptions direto do appsettings
builder.Services.Configure<MpOptions>(builder.Configuration.GetSection("MercadoPago"));

// Define singleton que injeta as opções e seta o token global do SDK
builder.Services.AddSingleton(sp =>
{
    var opt = sp.GetRequiredService<IOptions<MpOptions>>().Value;
    MercadoPagoConfig.AccessToken = opt.AccessToken;
    return opt;
});

// Registro do seu gateway de pagamento
builder.Services.AddScoped<IGatewayPagamento, MercadoPagoCheckoutProGateway>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(CommonProfile).Assembly);

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kv => kv.Key,
                kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            message = "Model validation failed",
            errors
        });
    };
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SecretariaConcafras API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";              // UI fica em /swagger (a gente reescreve /api/swagger no Nginx)
    c.SwaggerEndpoint("v1/swagger.json",    // <-- RELATIVO! não use "/swagger/..."
                      "SecretariaConcafras API v1");
});


app.UseCors("AllowAngular");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feat = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = feat?.Error;

        ProblemDetails problem;

        if (ex is InscricaoException iex)
        {
            context.Response.StatusCode = (int)iex.Status;
            problem = new ProblemDetails
            {
                Status = (int)iex.Status,
                Title = iex.Message
            };
            if (iex.Errors is not null)
                problem.Extensions["errors"] = iex.Errors;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erro inesperado."
            };
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(problem);
    });
});

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));
app.Run();
