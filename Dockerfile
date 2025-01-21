# Build-Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopiere die Projektdateien und restore Dependencies
COPY *.csproj ./
RUN dotnet restore

# Kopiere den Rest des Codes und baue die Anwendung
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime-Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Kopiere die veröffentlichten Dateien aus der Build-Stage
COPY --from=build /app/out ./

COPY . ./

# Exponiere den Port, den die Anwendung nutzt
EXPOSE 8080

# Startpunkt für die Anwendung
ENTRYPOINT ["dotnet", "Webshop.dll"]
