# BlazorWebAppMovies - Docker Deployment

This guide explains how to deploy the BlazorWebAppMovies application using Docker and Docker Compose with PostgreSQL.

## Files Overview

- **Dockerfile**: Multi-stage build configuration for the Blazor application
- **docker-compose.yml**: Orchestrates both the Blazor app and PostgreSQL database
- **.env.sample**: Sample environment variables configuration file
- **.dockerignore**: Specifies files to exclude from the Docker build context

## Prerequisites

- Docker Desktop installed
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
- Start PostgreSQL 16 database
- Build and start the Blazor application
- Create database schema automatically
- Seed initial data
- Expose the application on `http://localhost:8080`

### 3. Check Logs

To view application logs:

```bash
docker compose logs -f blazor-app
```

To view database logs:

```bash
docker compose logs -f postgres
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
docker exec -it blazorwebappmovies-postgres-1 psql -U postgres -d BlazorWebAppMovies
```

## Production Deployment

For production, use the Dockerfile directly:

```bash
# Build the image
docker build -t blazor-movies-app:latest .

# Run with environment variables
docker run -d \
  -p 8080:8080 \
  -e ConnectionStrings__BlazorWebAppMoviesContext="Host=your-postgres-host;Database=BlazorWebAppMovies;Username=postgres;Password=your-password" \
  --name blazor-app \
  blazor-movies-app:latest
```

## Configuration

### Environment Variables

Key configuration options (set in `.env` or docker-compose.yml):

- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployment
- `ASPNETCORE_URLS`: Application URL binding (default: `http://+:8080`)
- `ConnectionStrings__BlazorWebAppMoviesContext`: PostgreSQL connection string

### Database

The application uses **PostgreSQL 16** (Alpine variant for smaller image size). The database is configured with health checks to ensure it's ready before the application starts.

## Troubleshooting

### Application won't start

1. Check if PostgreSQL is healthy:
   ```bash
   docker compose ps
   ```

2. View application logs:
   ```bash
   docker compose logs blazor-app
   ```

### Database connection errors

The application includes automatic retry logic (10 attempts with 3-second delays). If connections still fail:

1. Ensure PostgreSQL container is running and healthy
2. Check the connection string in `.env` or `docker-compose.yml`
3. Verify PostgreSQL is accepting connections:
   ```bash
   docker compose logs postgres
   ```

### Reset database

To start fresh with a clean database:

```bash
docker compose down
docker volume rm blazorwebappmovies_postgres-data
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
- **5432**: PostgreSQL (Database)

## Data Persistence

Database data is persisted in a Docker volume named `blazorwebappmovies_postgres-data`.

## Health Checks

The PostgreSQL service includes a health check that ensures the database is ready before the application starts.

## Support

For issues or questions, please refer to the project documentation or create an issue in the repository.
