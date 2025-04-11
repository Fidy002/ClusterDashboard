# ---- Build Stage ----
    FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /app
    
    # Copy everything and restore
    COPY . ./
    RUN dotnet restore
    
    # Build and publish
    RUN dotnet publish -c Release -o out
    
    # ---- Runtime Stage ----
    FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
    WORKDIR /app
    
    # SSH-Client hinzufügen
    RUN apk add --no-cache openssh
    
    # Copy published output
    COPY --from=build /app/out ./
    
    # Configure ASP.NET Core
    ENV ASPNETCORE_URLS=http://+:7123
    EXPOSE 7123
    
    ENTRYPOINT ["dotnet", "ClusterDashboard.dll"]
    