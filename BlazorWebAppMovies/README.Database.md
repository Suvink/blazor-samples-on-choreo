# BlazorWebAppMovies - PostgreSQL Database

This application uses **PostgreSQL** database through Entity Framework Core.

## 🗄️ Database Setup

PostgreSQL is open-source, feature-rich, and works great on all platforms including ARM64 Macs.

**Start with PostgreSQL:**
```bash
docker compose up -d
```

**Connection String Format:**
```
Host=postgres;Database=BlazorWebAppMovies;Username=postgres;Password=YourStrong@Passw0rd
```

## 🚀 Quick Start

### 1. Start the Application

```bash
docker compose up -d
```

### 2. Access the Application

Open your browser to: **http://localhost:8080**

## 📋 Database Schema Management

PostgreSQL uses `EnsureCreated()` which automatically creates the database schema based on your models. The application will:
1. Connect to PostgreSQL
2. Create the database if it doesn't exist
3. Create all tables based on the Entity Framework models
4. Seed initial data

**No manual migrations needed!**

## 🔧 Configuration

The database connection is configured via the `ConnectionStrings__BlazorWebAppMoviesContext` environment variable:

```yaml
environment:
  - ConnectionStrings__BlazorWebAppMoviesContext=Host=postgres;Database=BlazorWebAppMovies;Username=postgres;Password=YourStrong@Passw0rd
```
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

## 🔍 Troubleshooting

### App won't start
1. Check database health:
   ```bash
   docker compose ps
   ```

2. View application logs:
   ```bash
   docker compose logs blazor-app
   ```

### Database connection errors
1. Ensure the PostgreSQL service is running and healthy
2. Verify the connection string is correct
3. Check PostgreSQL logs: `docker compose logs postgres`

### Reset everything
```bash
# Stop all containers and remove volumes
docker compose down -v

# Remove all images
docker compose down --rmi all

# Start fresh
docker compose up --build -d
```

## 📦 NuGet Packages Used

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
Host=your-host;Database=BlazorWebAppMovies;Username=app_user;Password=<secret>;SslMode=Require;Trust Server Certificate=true
```

## 📚 Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [PostgreSQL EF Core Provider](https://www.npgsql.org/efcore/)

---

**Built with ❤️ using .NET 10.0 and Blazor**
