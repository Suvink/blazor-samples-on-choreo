FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN groupadd -g 10014 choreo && \
    useradd --no-create-home --uid 10014 --gid 10014 choreouser

COPY HelloWorldBlazorApp/HelloWorldBlazorApp.csproj HelloWorldBlazorApp/
RUN dotnet restore HelloWorldBlazorApp/HelloWorldBlazorApp.csproj

COPY . .
WORKDIR /src/HelloWorldBlazorApp
RUN dotnet build HelloWorldBlazorApp.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish HelloWorldBlazorApp.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
USER 10014
ENTRYPOINT ["dotnet", "HelloWorldBlazorApp.dll"]
