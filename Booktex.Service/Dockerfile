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
COPY ["Booktex.Service/Booktex.Service.csproj", "Booktex.Service/"]
COPY ["Booktex.Application/Booktex.Application.csproj", "Booktex.Application/"]
COPY ["Booktex.Domain/Booktex.Domain.csproj", "Booktex.Domain/"]
COPY ["Booktex.Infrastructure/Booktex.Infrastructure.csproj", "Booktex.Infrastructure/"]
COPY ["Booktex.Persistence/Booktex.Persistence.csproj", "Booktex.Persistence/"]
RUN dotnet restore "./Booktex.Service/Booktex.Service.csproj"
COPY . .
WORKDIR "/src/Booktex.Service"
RUN dotnet build "./Booktex.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Booktex.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booktex.Service.dll"]