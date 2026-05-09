# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Bws.Bible.Api/*.csproj Bws.Bible.Api/
COPY Bws.Bible.Core/*.csproj Bws.Bible.Core/
COPY Bws.Bible.Infrastructure/*.csproj Bws.Bible.Infrastructure/
COPY Bws.Bible.Api.Tests/*.csproj Bws.Bible.Api.Tests/
COPY Bws.Bible.Api.sln .

# Restore NuGet packages
RUN dotnet restore Bws.Bible.Api.sln

# Copy remaining source code
COPY . .

# Build and publish the application
RUN dotnet publish Bws.Bible.Api/Bws.Bible.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install runtime dependencies (curl for health checks)
RUN apt-get update && apt-get install -y curl --no-install-recommends && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN useradd -m appuser && chown -R appuser:appuser /app
USER appuser

# Copy published application
COPY --from=build --chown=appuser:appuser /app/publish .

# Create Storage directory for bible files
RUN mkdir -p /app/Storage

# Expose ports
EXPOSE 5000 5001

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:5000/health/ready || exit 1

# Run the application
ENTRYPOINT ["dotnet", "Bws.Bible.Api.dll"]