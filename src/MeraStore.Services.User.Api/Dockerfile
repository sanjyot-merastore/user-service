# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MeraStore.Services.User.Api/MeraStore.Services.User.Api.csproj", "src/MeraStore.Services.User.Api/"]
COPY ["src/MeraStore.Services.User.Infrastructure/MeraStore.Services.User.Infrastructure.csproj", "src/MeraStore.Services.User.Infrastructure/"]
COPY ["src/MeraStore.Services.User.Persistence/MeraStore.Services.User.Persistence.csproj", "src/MeraStore.Services.User.Persistence/"]
COPY ["src/MeraStore.Services.User.Domain/MeraStore.Services.User.Domain.csproj", "src/MeraStore.Services.User.Domain/"]
COPY ["src/MeraStore.Services.User.Common/MeraStore.Services.User.Common.csproj", "src/MeraStore.Services.User.Common/"]
RUN dotnet restore "./src/MeraStore.Services.User.Api/MeraStore.Services.User.Api.csproj"
COPY . .
WORKDIR "/src/src/MeraStore.Services.User.Api"
RUN dotnet build "./MeraStore.Services.User.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MeraStore.Services.User.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeraStore.Services.User.Api.dll"]