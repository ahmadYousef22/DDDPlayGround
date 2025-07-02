# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and all project folders
COPY *.sln ./
COPY DDDPlayGround.Host ./DDDPlayGround.Host/
COPY DDDPlayGround.Infrastructure ./DDDPlayGround.Infrastructure/
COPY DDDPlayGround.Domain ./DDDPlayGround.Domain/
COPY DDDPlayGround.Application ./DDDPlayGround.Application/

# Restore dependencies
RUN dotnet restore DDDPlayGround.sln

# Build all projects
RUN dotnet build DDDPlayGround.sln --configuration Release --no-restore

# Publish only the Host project
RUN dotnet publish DDDPlayGround.Host/DDDPlayGround.Host.csproj -c Release -o /app/publish --no-build

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

EXPOSE 80

ENTRYPOINT ["dotnet", "DDDPlayGround.Host.dll"]
