# Use .NET 8 SDK to build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore dependencies
COPY DotnetKafka.csproj ./
RUN dotnet restore

# Copy all project files and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "DotnetKafka.dll"]
