# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Create non-root user for security
RUN groupadd -g 10014 choreo && \
    useradd --no-create-home --uid 10014 --gid 10014 choreouser

# Copy the project file and restore dependencies
COPY BlazorMongoDB/BlazorMongoDB.csproj BlazorMongoDB/
RUN dotnet restore BlazorMongoDB/BlazorMongoDB.csproj

# Copy the rest of the application code
COPY BlazorMongoDB/ BlazorMongoDB/

# Build the application
WORKDIR /src/BlazorMongoDB
RUN dotnet build BlazorMongoDB.csproj -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish BlazorMongoDB.csproj -c Release -o /app/publish /p:UseAppHost=false

# Use the official ASP.NET runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Expose the port the app runs on
EXPOSE 80
EXPOSE 443

# Copy the published application from the publish stage
COPY --from=publish /app/publish .

# Switch to non-root user
USER 10014

# Set the entry point for the application
ENTRYPOINT ["dotnet", "BlazorMongoDB.dll"]
