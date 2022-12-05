FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src
COPY LivrEtec/*.csproj ./LivrEtec/
COPY LivrEtec.Testes/*.csproj ./LivrEtec.Testes/
COPY LivrEtec.GIB/*.csproj ./LivrEtec.GIB/
COPY LivrEtec.Servidor/*.csproj ./LivrEtec.Servidor/
COPY LivrEtec.GIB.Servidor/*.csproj ./LivrEtec.GIB.Servidor/
COPY *.sln .
RUN  dotnet restore
ENV APP_SETTINGS_JSON_PATH="/src/Docker/Testes/appsettings.json"
COPY . . 
ENTRYPOINT [ "dotnet" ]
CMD [ "test" ] 