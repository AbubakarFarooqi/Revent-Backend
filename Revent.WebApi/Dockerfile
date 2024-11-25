#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app # set to app instead of root for security purpose
WORKDIR /app # it specifies the directory where the final aapplication will reside
EXPOSE 8080
#EXPOSE 8081



#build stage:
#Purpose:
#Compiles the application into an intermediate build output.
#Generates files like DLLs, PDBs (debugging information), and temporary assets used for debugging and running the application locally.
#Output Location:
#By default, outputs to a bin/<configuration> directory (e.g., bin/Release or bin/Debug).
#With -o (e.g., -o /app/build), the output location can be customized.
#Output Content:
#Includes compiled DLLs of your application and any referenced libraries.
#Not optimized for deployment; it's geared for local testing or further processing.
#Does not include runtime dependencies, only the application and its direct dependencies
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Revent.WebApi/Revent.WebApi.csproj", "Revent.WebApi/"]
COPY ["Revent.Common/Revent.Common.csproj", "Revent.Common/"]
COPY ["Revent.DataAccess.Implementation/Revent.DataAccess.Implementation.csproj", "Revent.DataAccess.Implementation/"]
COPY ["Revent.Services/Revent.Services.csproj", "Revent.Services/"]
RUN dotnet restore "./Revent.WebApi/Revent.WebApi.csproj"
# the above copy commands save time in building and caching as those will only execute if csproj changes not the code base
# and dotnet restore only runs on dependicies change not on code change
COPY . .
WORKDIR "/src/Revent.WebApi"
RUN dotnet build "./Revent.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build
# the /app is not the one that is used in base stage as each stage of multistage docker file runs in isolated file system
# this is the output folder where DLLs and binaries will be stored


#publish Stage:
#Purpose:
#Prepares the application for deployment.
#Packages the application into a self-contained format, optimized for running in production.
#Output Location:
#Defaults to a publish folder (e.g., bin/Release/net8.0/publish).
#Can be customized with -o (e.g., -o /app/publish).
#Output Content:
#Includes the application's compiled DLLs and runtime dependencies.
#Optionally, if specified, it can produce a self-contained deployment:
#Bundles the .NET runtime with the application, meaning the target system doesn’t need to have the .NET runtime installed.
#This is achieved by adding --self-contained or similar options.
#Removes unnecessary files to reduce the size of the final deployment package.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Revent.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false



#Publish Stage:
#
#The publish stage is responsible for building and preparing the application for deployment.
#It uses the SDK image, which includes development tools like dotnet publish, compilers, and other utilities.
#However, these tools are not needed to run the application in production and can unnecessarily bloat the final image.
#Final Stage:
#
#The final stage uses the runtime image (e.g., mcr.microsoft.com/dotnet/aspnet:8.0), which is smaller and optimized for running applications.
#By copying only the published output from the publish stage, the final image is stripped of unnecessary build tools, reducing its size and attack surface.
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Revent.WebApi.dll"]

#one more question in this multistage docker file the final image that will be created is the final stage, and the other two build and publish will be discarded?
#ChatGPT said:
#ChatGPT
#Yes, you are absolutely correct! In a multi-stage Dockerfile, the final image that gets created and retained is based solely on the last stage (in this case, the final stage). The intermediate stages (build and publish) are used only temporarily during the build process and are discarded once the final image is created.