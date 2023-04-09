#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Api/WebApi/BlazorDictionary.Api.WebApi/BlazorDictionary.Api.WebApi.csproj", "src/Api/WebApi/BlazorDictionary.Api.WebApi/"]
COPY ["src/Api/Infrastructure/BlazorDictionary.Infrastructure.Persistence/BlazorDictionary.Api.Infrastructure.Persistence.csproj", "src/Api/Infrastructure/BlazorDictionary.Infrastructure.Persistence/"]
COPY ["src/Api/Core/BlazorDictionary.Api.Application/BlazorDictionary.Api.Application.csproj", "src/Api/Core/BlazorDictionary.Api.Application/"]
COPY ["src/Api/Core/BlazorDictionary.Api.Domain/BlazorDictionary.Api.Domain.csproj", "src/Api/Core/BlazorDictionary.Api.Domain/"]
COPY ["src/Common/BlazorDictionary.Common/BlazorDictionary.Common.csproj", "src/Common/BlazorDictionary.Common/"]
RUN dotnet restore "src/Api/WebApi/BlazorDictionary.Api.WebApi/BlazorDictionary.Api.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Api/WebApi/BlazorDictionary.Api.WebApi"
RUN dotnet build "BlazorDictionary.Api.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorDictionary.Api.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorDictionary.Api.WebApi.dll"]