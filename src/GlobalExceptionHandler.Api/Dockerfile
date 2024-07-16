FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build and publish a release of the application.
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
COPY . /source
WORKDIR /source
RUN dotnet publish -c Release -o /app

# Copy everything needed to run the app from the "build" stage.
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "GlobalExceptionHandler.Api.dll"]