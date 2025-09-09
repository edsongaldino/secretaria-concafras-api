# ===== build =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# (opcional) se tiver .sln, copie-a
# COPY ./SecretariaConcafras.sln ./

# copie apenas os .csproj primeiro (cache do restore)
COPY ./SecretariaConcafras.API/SecretariaConcafras.API.csproj ./SecretariaConcafras.API/
COPY ./SecretariaConcafras.Application/SecretariaConcafras.Application.csproj ./SecretariaConcafras.Application/
COPY ./SecretariaConcafras.Domain/SecretariaConcafras.Domain.csproj ./SecretariaConcafras.Domain/
COPY ./SecretariaConcafras.Infrastructure/SecretariaConcafras.Infrastructure.csproj ./SecretariaConcafras.Infrastructure/
COPY ./SecretariaConcafras.SharedKernel/SecretariaConcafras.SharedKernel.csproj ./SecretariaConcafras.SharedKernel/

# restore (a partir do projeto da API puxa tudo que ela referencia)
RUN dotnet restore ./SecretariaConcafras.API/SecretariaConcafras.API.csproj

# agora copie o resto do código
COPY . .

# publish
RUN dotnet publish -c Release -o /out ./SecretariaConcafras.API/SecretariaConcafras.API.csproj -p:UseAppHost=false

# ===== runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
COPY --from=build /out ./
EXPOSE 8080
# use exatamente o AssemblyName do .csproj (case-sensitive)
ENTRYPOINT ["dotnet", "SecretariaConcafras.API.dll"]