FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Create non-root user for security
RUN groupadd -g 10014 choreo && \
    useradd --no-create-home --uid 10014 --gid 10014 choreouser

# Copy project file and restore dependencies
COPY BlazorWebAppMovies.csproj .
RUN dotnet restore BlazorWebAppMovies.csproj

# Copy the rest of the application
COPY . .

# Build the application
RUN dotnet build BlazorWebAppMovies.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish BlazorWebAppMovies.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Copy published application
COPY --from=publish /app/publish .

# Switch to non-root user
USER 10014

# Set entrypoint
ENTRYPOINT ["dotnet", "BlazorWebAppMovies.dll"]
