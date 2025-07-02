# STAGE 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the full source code
COPY . .

# Set working directory to src
WORKDIR /app/src

# Restore dependencies using the solution file inside src
RUN dotnet restore DDDPlayGround.sln

# Build and publish the project
RUN dotnet publish DDDPlayGround.Host/DDDPlayGround.Host.csproj -c Release -o /app/publish --no-restore

# STAGE 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DDDPlayGround.Host.dll"]
