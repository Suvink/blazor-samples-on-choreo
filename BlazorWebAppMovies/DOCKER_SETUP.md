# BlazorWebAppMovies - Docker Setup Guide

## ✅ What's Included

This Dockerized setup includes:

- **Blazor Server Application** (.NET 10.0)
- **PostgreSQL Database** (Version 16)
- **Automatic Database Setup**: Schema creation and data seeding
- **Health Checks**: Ensures database is ready before app starts
- **Non-root Security**: Application runs as user `choreouser` (UID 10014)

## 🚀 Quick Start

```bash
docker compose up -d
```

**Access the app**: http://localhost:8080

## 📁 Files Overview

```
BlazorWebAppMovies/
├── Dockerfile                      # Multi-stage build for .NET app
├── docker-compose.yml              # PostgreSQL setup
├── .dockerignore                   # Files excluded from Docker build
├── .env.sample                     # Environment variable template
└── README.Database.md             # Detailed database documentation
```

## 🔧 Configuration

### Environment Variables

Create a `.env` file from `.env.sample`:

```bash
cp .env.sample .env
```

Key variable:
- `ConnectionStrings__BlazorWebAppMoviesContext`: PostgreSQL connection string

### Default Credentials

**⚠️ Change these in production!**

**PostgreSQL:**
- Username: `postgres`
- Password: `YourStrong@Passw0rd`
- Port: `5432`

## 🐳 Common Docker Commands

### View Logs
```bash
# All services
docker compose logs -f

# Just the app
docker compose logs -f blazor-app

# Just the database
docker compose logs -f postgres
```

### Check Status
```bash
docker compose ps
```

### Stop Everything
```bash
docker compose down
```

### Clean Restart (Delete Data)
```bash
docker compose down -v
docker compose up -d
```

### Rebuild After Code Changes
```bash
docker compose up --build -d
```

## 🛠️ How It Works

### Database Initialization

PostgreSQL uses `EnsureCreated()` to automatically:
- Create the database if it doesn't exist
- Generate schema from Entity Framework models
- Seed initial movie data
- Include retry logic (10 attempts with 3-second delays)

### Docker Multi-Stage Build

The Dockerfile uses a multi-stage build:

1. **Build Stage**: 
   - Uses `mcr.microsoft.com/dotnet/sdk:10.0`
   - Restores NuGet packages
   - Builds the application

2. **Publish Stage**:
   - Publishes optimized release build

3. **Runtime Stage**:
   - Uses lightweight `mcr.microsoft.com/dotnet/aspnet:10.0`
   - Copies published application
   - Runs as non-root user `choreouser`

### Health Checks

Docker Compose includes health checks to ensure:
- Database is fully started
- Database accepts connections
- App waits for healthy database before starting

## 🔍 Troubleshooting

### App won't start

1. Check database health:
   ```bash
   docker compose ps
   ```

2. View detailed logs:
   ```bash
   docker compose logs
   ```

### Connection errors

- Check connection string in docker-compose.yml
- Ensure database container is healthy

### Port already in use

Change the port mapping in docker-compose.yml:
```yaml
ports:
  - "8081:8080"  # Use 8081 instead of 8080
```

## 📦 What Gets Built

The Docker image includes:
- .NET 10.0 Runtime
- Published Blazor application
- All dependencies from NuGet packages
- Database migrations (for SQL Server)
- Static assets (wwwroot)

**Image size**: ~250-300 MB (optimized)

## 🔐 Security Notes

1. **Non-root execution**: App runs as UID 10014
2. **TrustServerCertificate**: Enabled for development (disable in production)
3. **Change default passwords** before deploying to production
4. **Use secrets management** in production (Azure Key Vault, AWS Secrets Manager, etc.)

## 📚 Additional Resources

- [README.Database.md](./README.Database.md) - Detailed database documentation
- [.env.sample](./.env.sample) - Environment variable reference
- [Dockerfile](./Dockerfile) - Container build configuration

---

**Questions?** Check [README.Database.md](./README.Database.md) for more details!
