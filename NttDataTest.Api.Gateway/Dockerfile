#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NttDataTest.Api.Gateway/NttDataTest.Api.Gateway.csproj", "NttDataTest.Api.Gateway/"]
RUN dotnet restore "NttDataTest.Api.Gateway/NttDataTest.Api.Gateway.csproj"
COPY . .
WORKDIR "/src/NttDataTest.Api.Gateway"
RUN dotnet build "NttDataTest.Api.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NttDataTest.Api.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NttDataTest.Api.Gateway.dll"]