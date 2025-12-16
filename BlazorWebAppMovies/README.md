# BlazorWebAppMovies Example

Follow this guide to containerize and deploy the BlazorWebAppMovies application using Docker and Docker Compose.
https://wso2.com/choreo/docs/develop-components/deploy-a-containerized-application/

## Configurations

To add environment variables, use the following guide:
https://wso2.com/choreo/docs/devops-and-ci-cd/manage-configurations-and-secrets/

| Name | Example |
|----------|---------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ASPNETCORE_URLS` | `http://+:80` |

## Secrets

| Name | Example |
|----------|---------|
| `ConnectionStrings__BlazorWebAppMoviesContext` | `Server=localhost;Port=27017;Database=BlazorWebAppMovies;User=admin;Password=admin;` |
