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

# Microsoft.Data.SqlClient requires ICU to be installed
ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

RUN apk add --no-cache \
    icu-data-full \
    icu-libs

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "GlobalExceptionHandler.Api.dll"]