# Récupération de l'image avec le SDK de base de Microsoft
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Exposition des ports pour le trafic de la Web API
ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS http://+:5000;https://+:5001
EXPOSE 5000
EXPOSE 5001

# Copie du CSPROJ et restauration des dépendances (Nuget packages)
COPY *.csproj ./
RUN dotnet restore

# Copie des fichiers du projet et build
COPY . ./
RUN dotnet publish -c Release -o out

# Génération de l'image docker
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "Bws.Bible.Api.dll" ]