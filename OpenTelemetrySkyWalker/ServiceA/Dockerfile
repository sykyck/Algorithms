# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy rest of the code and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
RUN apt-get update && apt-get install -y curl
COPY --from=build /app/out .

# Expose port
EXPOSE 5000
ENTRYPOINT ["dotnet", "ServiceA.dll"]

