# BlazorWebAppMovies - Multi-Database Support

This application supports **SQL Server** and **PostgreSQL** databases through Entity Framework Core.

## 🗄️ Database Options

### Option 1: PostgreSQL (Recommended for All Platforms)

PostgreSQL is open-source, feature-rich, and works great on all platforms including ARM64 Macs.

**Start with PostgreSQL:**
```bash
docker-compose -f docker-compose.postgres.yml up -d
```

**Connection String:**
```
Host=postgres;Database=BlazorWebAppMovies;Username=postgres;Password=YourStrong@Passw0rd
```

**Environment Variable:**
```bash
DATABASE_PROVIDER=PostgreSQL
```

### Option 2: SQL Server / Azure SQL Edge

Azure SQL Edge is used for ARM64 compatibility (Apple Silicon Macs). For other platforms, standard SQL Server can be used.

**Start with SQL Server:**
```bash
docker-compose up -d
```

**Connection String:**
```
Server=sqlserver;Database=BlazorWebAppMovies;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true
```

**Environment Variable:**
```bash
DATABASE_PROVIDER=SqlServer
```

## 🚀 Quick Start

### 1. Choose Your Database

Pick one of the docker-compose files based on your preference:
- `docker-compose.yml` - SQL Server/Azure SQL Edge (default)
- `docker-compose.postgres.yml` - PostgreSQL

### 2. Start the Application

```bash
# PostgreSQL
docker-compose -f docker-compose.postgres.yml up -d

# SQL Server
docker-compose up -d
```

### 3. Access the Application

Open your browser to: **http://localhost:8080**

## 📋 Database Schema Management

### PostgreSQL

PostgreSQL uses `EnsureCreated()` which automatically creates the database schema based on your models. The application will:
1. Connect to PostgreSQL
2. Create the database if it doesn't exist
3. Create all tables based on the Entity Framework models
4. Seed initial data

**No manual migrations needed!**

### SQL Server

SQL Server uses Entity Framework migrations. The existing migrations in the `Migrations` folder will be applied automatically when the container starts.

# Update database
dotnet ef database update --context BlazorWebAppMoviesContext
```

## 🔧 Configuration

The database provider is configured via the `DATABASE_PROVIDER` environment variable:

```yaml
environment:
  - DATABASE_PROVIDER=PostgreSQL  # Options: SqlServer, PostgreSQL
  - ConnectionStrings__BlazorWebAppMoviesContext=<your-connection-string>
```

## 📊 Comparing Database Options

| Feature | SQL Server | PostgreSQL |
|---------|-----------|-----------|
| **License** | Commercial | Open Source |
| **ARM64 Support** | Azure SQL Edge | ✅ Native |
| **JSON Support** | ✅ | ✅ Excellent |
| **Full-Text Search** | ✅ | ✅ |
| **Performance** | Excellent | Excellent |
| **Schema Management** | Migrations | EnsureCreated |
| **Best For** | Enterprise, Windows | Cross-platform, Advanced features |

## 🐳 Docker Commands

### View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f blazor-app
docker-compose logs -f postgres  # or sqlserver
```

### Stop Services
```bash
docker-compose down

# Or for specific compose file
docker-compose -f docker-compose.postgres.yml down
```

### Clean Restart (Remove Volumes)
```bash
# PostgreSQL
docker-compose -f docker-compose.postgres.yml down -v
docker-compose -f docker-compose.postgres.yml up -d

# SQL Server
docker-compose down -v
docker-compose up -d
```

## 🔐 Security Notes

**⚠️ Important:** Change the default passwords in production!

Update these in your docker-compose file or .env file:
- PostgreSQL: `POSTGRES_PASSWORD`
- SQL Server: `SA_PASSWORD` or `MSSQL_SA_PASSWORD`

## 🛠️ Troubleshooting

### Application won't start
1. Check if the database container is healthy:
   ```bash
   docker-compose ps
   ```

2. View application logs:
   ```bash
   docker-compose logs blazor-app
   ```

### Database connection errors
1. Ensure the database service is running and healthy
2. Verify the connection string matches your database
3. Check the `DATABASE_PROVIDER` environment variable is set correctly

### Reset everything
```bash
# Stop all containers and remove volumes
docker-compose down -v

# Remove all images
docker-compose down --rmi all

# Start fresh
docker-compose up --build -d
```

## 📦 NuGet Packages Used

- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server provider
- `Npgsql.EntityFrameworkCore.PostgreSQL` - PostgreSQL provider
- `Microsoft.EntityFrameworkCore.Tools` - EF Core tooling

## 🎯 Production Deployment

For production deployments:

1. **Use secrets management** for database credentials
2. **Enable SSL/TLS** for database connections
3. **Configure backups** for your database
4. **Use connection pooling** (enabled by default)
5. **Monitor performance** and adjust connection strings as needed

### PostgreSQL Production Connection String
```
Host=your-host;Database=BlazorWebAppMovies;Username=app_user;Password=<secret>;SSL Mode=Require;Trust Server Certificate=true
```

### SQL Server Production Connection String
```
Server=your-host;Database=BlazorWebAppMovies;User Id=app_user;Password=<secret>;Encrypt=True;TrustServerCertificate=False
```

## 📚 Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL EF Core Provider](https://www.npgsql.org/efcore/)
- [SQL Server EF Core Provider](https://docs.microsoft.com/en-us/ef/core/providers/sql-server/)

---

**Built with ❤️ using .NET 10.0 and Blazor**
