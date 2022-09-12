#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NttDataTest.Api.Client/NttDataTest.Api.Client.csproj", "NttDataTest.Api.Client/"]
COPY ["NttDataTest.Persistence.Client/NttDataTest.Persistence.Client.csproj", "NttDataTest.Persistence.Client/"]
COPY ["NttDataTest.Domain.Client/NttDataTest.Domain.Client.csproj", "NttDataTest.Domain.Client/"]
COPY ["NttDataTest.Common.Client/NttDataTest.Common.Client.csproj", "NttDataTest.Common.Client/"]
COPY ["NttDataTest.Services.Client/NttDataTest.Services.Client.csproj", "NttDataTest.Services.Client/"]
RUN dotnet restore "NttDataTest.Api.Client/NttDataTest.Api.Client.csproj"
COPY . .
WORKDIR "/src/NttDataTest.Api.Client"
RUN dotnet build "NttDataTest.Api.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NttDataTest.Api.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NttDataTest.Api.Client.dll"]