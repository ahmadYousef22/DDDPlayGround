# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy solution file
COPY *.sln ./

# Copy all project files
COPY DDDPlayGround.*/*.csproj ./

# Restore using solution
RUN dotnet restore DDDPlayGround.sln

# Copy the full source
COPY . ./

# Build
RUN dotnet build DDDPlayGround.sln --configuration Release --no-restore

# Publish
RUN dotnet publish DDDPlayGround.sln --configuration Release --output /app/publish --no-restore

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish ./

ENTRYPOINT ["dotnet", "DDDPlayGround.Host.dll"]
