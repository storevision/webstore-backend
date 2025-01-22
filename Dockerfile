# Build-Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopiere die Projektdateien und restore Dependencies
COPY . ./  
RUN dotnet restore

# Baue die Anwendung
RUN dotnet publish -c Release -o out

# Runtime-Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Kopiere die veröffentlichten Dateien aus der Build-Stage
COPY --from=build /app/out ./ 

# Kopiere die benötigten Umgebungsdateien und SQL-Skripte
COPY backend.env ./      
COPY ./sql ./             

# Setze den Port und sicherstelle, dass der richtige verwendet wird
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Startpunkt für die Anwendung
ENTRYPOINT ["dotnet", "Webshop.dll"]
