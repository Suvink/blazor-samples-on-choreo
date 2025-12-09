# BlazorMongoDB Docker Deployment

This guide explains how to deploy the BlazorMongoDB application using Docker.

## Prerequisites

- Docker installed on your system
- Docker Compose installed on your system

## Files Created

1. **Dockerfile** - Multi-stage Docker build file for the Blazor application
2. **.env.sample** - Sample environment variables file
3. **docker-compose.yml** - Docker Compose configuration for running the app with MongoDB
4. **.dockerignore** - Files to exclude from Docker build context

## Quick Start

### Option 1: Using Docker Compose (Recommended)

1. Build and run the application with MongoDB:
   ```bash
   docker-compose up -d
   ```

2. Access the application at `http://localhost:8080`

3. Stop the application:
   ```bash
   docker-compose down
   ```

### Option 2: Using Dockerfile Only

1. Build the Docker image:
   ```bash
   docker build -t blazor-mongodb-app .
   ```

2. Run a MongoDB container:
   ```bash
   docker run -d --name mongodb -p 27017:27017 mongo:7.0
   ```

3. Run the application container:
   ```bash
   docker run -d -p 8080:80 \
     -e MONGODB_CONNECTION_STRING=mongodb://mongodb:27017/ \
     -e MONGODB_DATABASE_NAME=SchoolDB \
     -e MONGODB_COLLECTION_NAME=Students \
     --link mongodb:mongodb \
     blazor-mongodb-app
   ```

## Environment Variables

The following environment variables can be configured:

| Variable | Description | Default |
|----------|-------------|---------|
| `MONGODB_CONNECTION_STRING` | MongoDB connection string | `mongodb://mongodb:27017/` |
| `MONGODB_DATABASE_NAME` | MongoDB database name | `SchoolDB` |
| `MONGODB_COLLECTION_NAME` | MongoDB collection name | `Students` |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET Core environment | `Production` |
| `ASPNETCORE_URLS` | URLs for the application | `http://+:80` |

## Configuration

### Using .env file

1. Copy the sample environment file:
   ```bash
   cp .env.sample .env
   ```

2. Edit `.env` with your configuration values

3. The docker-compose.yml will automatically use these values

### Custom Configuration

Edit the `docker-compose.yml` file to customize:
- Port mappings
- MongoDB version
- Environment variables
- Volume mounts
- Network configuration

## Data Persistence

MongoDB data is persisted using Docker volumes. The volume `mongodb_data` is created automatically and stores the database files.

To remove all data:
```bash
docker-compose down -v
```

## Troubleshooting

### View application logs:
```bash
docker-compose logs blazor-app
```

### View MongoDB logs:
```bash
docker-compose logs mongodb
```

### Connect to MongoDB shell:
```bash
docker exec -it blazor-mongodb mongosh
```

### Rebuild after code changes:
```bash
docker-compose up -d --build
```

## Production Considerations

For production deployment, consider:

1. Use secrets management instead of environment variables
2. Enable MongoDB authentication
3. Use HTTPS with proper certificates
4. Configure MongoDB replica sets for high availability
5. Implement proper backup strategies
6. Use Docker secrets or external configuration management
7. Set resource limits in docker-compose.yml
