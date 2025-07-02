# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and all src folders
COPY *.sln ./
COPY src/ ./src/

# Restore dependencies
RUN dotnet restore DDDPlayGround.sln

# Build all projects
RUN dotnet build DDDPlayGround.sln --configuration Release --no-restore

# Publish only the Host project (adjust project name if needed)
RUN dotnet publish src/DDDPlayGround.Host/DDDPlayGround.Host.csproj -c Release -o /app/publish --no-build

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from build stage
COPY --from=build /app/publish ./

# Expose port (change if your app uses a different port)
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "DDDPlayGround.Host.dll"]
