#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Revent.WebApi/Revent.WebApi.csproj", "Revent.WebApi/"]
COPY ["Revent.Common/Revent.Common.csproj", "Revent.Common/"]
COPY ["Revent.DataAccess.Implementation/Revent.DataAccess.Implementation.csproj", "Revent.DataAccess.Implementation/"]
COPY ["Revent.Services/Revent.Services.csproj", "Revent.Services/"]
RUN dotnet restore "./Revent.WebApi/Revent.WebApi.csproj"
COPY . .
WORKDIR "/src/Revent.WebApi"
RUN dotnet build "./Revent.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Revent.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Revent.WebApi.dll"]