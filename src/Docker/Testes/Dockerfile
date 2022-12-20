FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY LivrEtec/*.csproj ./LivrEtec/
COPY LivrEtec.Testes/*.csproj ./LivrEtec.Testes/
COPY LivrEtec.GIB/*.csproj ./LivrEtec.GIB/
COPY LivrEtec.Servidor/*.csproj ./LivrEtec.Servidor/
COPY LivrEtec.GIB.Servidor/*.csproj ./LivrEtec.GIB.Servidor/
RUN dotnet restore "LivrEtec.Testes/LivrEtec.Testes.csproj"
COPY . .
WORKDIR /src/LivrEtec.Testes
RUN dotnet build "LivrEtec.Testes.csproj" -c Release -o /build --no-restore
RUN mv /src/Docker/Testes/appsettings.json /build/appsettings.json

FROM base AS final
ENV APP_SETTINGS_JSON_PATH="./appsettings.json"
COPY --from=build /build .
ENTRYPOINT ["dotnet", "test", "LivrEtec.Testes.dll"]
