# BlazorWebAppMovies - Docker Deployment

This guide explains how to deploy the BlazorWebAppMovies application using Docker and Docker Compose.

## Files Overview

- **Dockerfile**: Multi-stage build configuration for the Blazor application
- **docker-compose.yml**: Orchestrates both the Blazor app and Azure SQL Edge database
- **.env.sample**: Sample environment variables configuration file
- **.dockerignore**: Specifies files to exclude from the Docker build context

## Prerequisites

- Docker Desktop installed (for macOS with Apple Silicon/ARM64)
- Docker Compose installed (usually included with Docker Desktop)

## Quick Start

### 1. Environment Setup

Copy the sample environment file and customize it:

```bash
cp .env.sample .env
```

Edit `.env` and update the database connection string with your values.

### 2. Start the Application

For **development** (includes database):

```bash
docker compose up -d
```

This will:
- Start Azure SQL Edge database (compatible with ARM64/Apple Silicon)
- Build and start the Blazor application
- Apply database migrations automatically
- Seed initial data
- Expose the application on `http://localhost:8080`

### 3. Check Logs

To view application logs:

```bash
docker compose logs -f blazor-app
```

To view database logs:

```bash
docker compose logs -f sqlserver
```

### 4. Stop the Application

```bash
docker compose down
```

To stop and remove volumes (database data):

```bash
docker compose down -v
```

## Docker Commands

### Build Only

```bash
docker compose build
```

### Rebuild Without Cache

```bash
docker compose build --no-cache
```

### View Running Containers

```bash
docker compose ps
```

### Access Database

```bash
docker exec -it blazorwebappmovies-sqlserver-1 /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P YourStrong@Passw0rd
```

## Production Deployment

For production, use the Dockerfile directly:

```bash
# Build the image
docker build -t blazor-movies-app:latest .

# Run with environment variables
docker run -d \
  -p 8080:8080 \
  -e ConnectionStrings__BlazorWebAppMoviesContext="Server=your-sql-server;Database=BlazorWebAppMovies;User=sa;Password=your-password" \
  --name blazor-app \
  blazor-movies-app:latest
```

## Configuration

### Environment Variables

Key configuration options (set in `.env` or docker-compose.yml):

- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployment
- `ASPNETCORE_URLS`: Application URL binding (default: `http://+:8080`)
- `ConnectionStrings__BlazorWebAppMoviesContext`: Database connection string

### Database

The application uses **Azure SQL Edge** which is compatible with ARM64 architecture (Apple Silicon Macs). For Intel-based systems or production, you can switch to SQL Server 2022 in `docker-compose.yml`:

```yaml
sqlserver:
  image: mcr.microsoft.com/mssql/server:2022-latest
  environment:
    - ACCEPT_EULA=Y
    - SA_PASSWORD=YourStrong@Passw0rd
    - MSSQL_PID=Developer
```

## Troubleshooting

### Application won't start

1. Check if SQL Server is healthy:
   ```bash
   docker compose ps
   ```

2. View application logs:
   ```bash
   docker compose logs blazor-app
   ```

### Database connection errors

The application includes automatic retry logic (10 attempts with 3-second delays). If connections still fail:

1. Ensure SQL Server container is running and healthy
2. Check the connection string in `.env` or `docker-compose.yml`
3. Verify SQL Server is accepting connections:
   ```bash
   docker compose logs sqlserver
   ```

### Reset database

To start fresh with a clean database:

```bash
docker compose down
docker volume rm blazorwebappmovies_sqlserver-data
docker compose up -d
```

## Architecture

### Multi-Stage Build

The Dockerfile uses a multi-stage build process:

1. **Build Stage**: Uses .NET SDK 10.0 to build the application
2. **Publish Stage**: Publishes the application for deployment  
3. **Final Stage**: Uses lightweight ASP.NET runtime for the final image

### Security

- The application runs as a non-root user (UID: 10014)
- Database credentials should be changed in production
- Use secrets management for sensitive data in production

## Ports

- **8080**: HTTP (Application)
- **8081**: HTTPS (if configured)
- **1433**: SQL Server (Database)

## Data Persistence

Database data is persisted in a Docker volume named `blazorwebappmovies_sqlserver-data`.

## Health Checks

The SQL Server service includes a health check that ensures the database is ready before the application starts.

## Support

For issues or questions, please refer to the project documentation or create an issue in the repository.
