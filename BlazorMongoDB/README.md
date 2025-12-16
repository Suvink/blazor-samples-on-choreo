# BlazorMongoDB Example

Follow this guide to containerize and deploy the BlazorMongoDB application using Docker and Docker Compose.
https://wso2.com/choreo/docs/develop-components/deploy-a-containerized-application/

## Configurations

To add environment variables, use the following guide:
https://wso2.com/choreo/docs/devops-and-ci-cd/manage-configurations-and-secrets/

| Name | Example |
|----------|---------|
| `MONGODB_DATABASE_NAME` | `SchoolDB` |
| `MONGODB_COLLECTION_NAME` | `Students` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ASPNETCORE_URLS` | `http://+:80` |

## Secrets

| Name | Example |
|----------|---------|
| `MongoDB__ConnectionString` | `mongodb://mongodb:27017/` |
