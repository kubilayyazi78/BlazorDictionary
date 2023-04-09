#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Projections/BlazorDictionary.Projections.UserService/BlazorDictionary.Projections.UserService.csproj", "src/Projections/BlazorDictionary.Projections.UserService/"]
COPY ["src/Common/BlazorDictionary.Common/BlazorDictionary.Common.csproj", "src/Common/BlazorDictionary.Common/"]
RUN dotnet restore "src/Projections/BlazorDictionary.Projections.UserService/BlazorDictionary.Projections.UserService.csproj"
COPY . .
WORKDIR "/src/src/Projections/BlazorDictionary.Projections.UserService"
RUN dotnet build "BlazorDictionary.Projections.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorDictionary.Projections.UserService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorDictionary.Projections.UserService.dll"]