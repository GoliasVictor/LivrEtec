FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV APP_SETTINGS_JSON_PATH="batatinha2"
WORKDIR /src
COPY LivrEtec.GIB.Servidor/LivrEtec.GIB.Servidor.csproj LivrEtec.GIB.Servidor/
COPY LivrEtec.GIB/LivrEtec.GIB.csproj LivrEtec.GIB/
COPY LivrEtec/LivrEtec.csproj LivrEtec/
COPY LivrEtec.Servidor/LivrEtec.Servidor.csproj LivrEtec.Servidor/
RUN dotnet restore "LivrEtec.GIB.Servidor/LivrEtec.GIB.Servidor.csproj"
COPY . .
WORKDIR "/src/LivrEtec.GIB.Servidor"
RUN dotnet build "LivrEtec.GIB.Servidor.csproj" -c Release

FROM build AS publish
RUN	dotnet publish "LivrEtec.GIB.Servidor.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build
RUN cp /src/Docker/GIB.Servidor/appsettings.json /app/publish/appsettings.json

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LivrEtec.GIB.Servidor.dll"]