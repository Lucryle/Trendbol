# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.sln .
COPY TrendbolAPI/*.csproj ./TrendbolAPI/
COPY UnitTests/*.csproj ./UnitTests/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "TrendbolAPI.dll"] 