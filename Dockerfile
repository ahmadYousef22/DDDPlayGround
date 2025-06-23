# Use official .NET 8 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy sln and csproj files, restore dependencies
COPY *.sln .
COPY DDDPlayGround.*/*.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish DDDPlayGround.Host/DDDPlayGround.Host.csproj -c Release -o /out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "DDDPlayGround.Host.dll"]
