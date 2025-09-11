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
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;

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
        typeof(IUsuarioService).Assembly,      // Application
        typeof(ApplicationDbContext).Assembly, // Infrastructure
        typeof(ITokenService).Assembly         // SharedKernel
    )
    .AddClasses(c => c.Where(type => type.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    .AddClasses(c => c.Where(type => type.Name.EndsWith("Repository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
);

// MpOptions do appsettings
builder.Services.Configure<MpOptions>(builder.Configuration.GetSection("MercadoPago"));
// Set explícito do AccessToken no SDK (não depende de DI)
MercadoPagoConfig.AccessToken = builder.Configuration["MercadoPago:AccessToken"];

// Registro do gateway de pagamento
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

// ModelState → 400 detalhado
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

// CORS (dev + produção)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200", "https://inscribo.com.br")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";           // UI em /swagger
    c.SwaggerEndpoint("v1/swagger.json", // relativo
                      "SecretariaConcafras API v1");
});

// CORS
app.UseCors("AllowAngular");

// Handler global de erros → envia para /error
app.UseExceptionHandler("/error");

// Endpoint central de erro (ProblemDetails + errorId)
app.Map("/error", (HttpContext http, ILogger<Program> logger) =>
{
    var feat = http.Features.Get<IExceptionHandlerFeature>();
    var ex = feat?.Error;
    var id = Activity.Current?.Id ?? http.TraceIdentifier;

    switch (ex)
    {
        case InscricaoException iex:
            return Results.Problem(
                statusCode: (int)iex.Status,
                title: iex.Message,
                extensions: new Dictionary<string, object?>
                {
                    ["errorId"] = id,
                    ["errors"] = iex.Errors
                });

        case InvalidOperationException ioe:
            return Results.Problem(
                statusCode: 422,
                title: "BUSINESS_RULE",
                detail: ioe.Message,
                extensions: new Dictionary<string, object?> { ["errorId"] = id });

        case ApplicationException aex:
            return Results.Problem(
                statusCode: 502,
                title: "GATEWAY_ERROR",
                detail: aex.Message,
                extensions: new Dictionary<string, object?> { ["errorId"] = id });

        default:
            logger.LogError(ex, "Unhandled {ErrorId}", id);
            return Results.Problem(
                statusCode: 500,
                title: "Erro interno",
                extensions: new Dictionary<string, object?> { ["errorId"] = id });
    }
});

//app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health básico
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

// Health do MP (confere se o token foi setado)
app.MapGet("/api/health/mp", () =>
{
    var at = MercadoPagoConfig.AccessToken;
    return Results.Ok(new
    {
        ok = !string.IsNullOrWhiteSpace(at),
        tokenPrefix = string.IsNullOrWhiteSpace(at) ? "" : at[..Math.Min(10, at.Length)]
    });
});

app.Run();
