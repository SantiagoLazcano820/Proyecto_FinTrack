# ─── Etapa 1: Build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto para restaurar dependencias (cache de capas)
COPY FinTrack/FinTrack.Core.csproj             FinTrack/
COPY FinTrack.Services/FinTrack.Services.csproj     FinTrack.Services/
COPY FinTrack.Infraestructure/FinTrack.Infraestructure.csproj FinTrack.Infraestructure/
COPY FinTrack.Api/FinTrack.Api.csproj               FinTrack.Api/

RUN dotnet restore FinTrack.Api/FinTrack.Api.csproj

# Copiar todo el código fuente y publicar
COPY . .
RUN dotnet publish FinTrack.Api/FinTrack.Api.csproj \
    -c Release -o /app/publish --no-restore

# ─── Etapa 2: Runtime ────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5065

ENTRYPOINT ["dotnet", "FinTrack.Api.dll"]
