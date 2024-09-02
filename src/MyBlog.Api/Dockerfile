#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyBlog.Api/MyBlog.Api.csproj", "MyBlog.Api/"]
COPY ["MyBlog.Service/MyBlog.Service.csproj", "MyBlog.Service/"]
COPY ["MyBlog.Repository/MyBlog.Repository.csproj", "MyBlog.Repository/"]
COPY ["MyBlog.Domain/MyBlog.Domain.csproj", "MyBlog.Domain/"]
RUN dotnet restore "./MyBlog.Api/MyBlog.Api.csproj"
COPY . .
WORKDIR "/src/MyBlog.Api"
RUN dotnet build "./MyBlog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyBlog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyBlog.Api.dll"]