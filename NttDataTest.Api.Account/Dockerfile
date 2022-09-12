#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NttDataTest.Api.Account/NttDataTest.Api.Account.csproj", "NttDataTest.Api.Account/"]
COPY ["NttDataTest.Persistence.Account/NttDataTest.Persistence.Account.csproj", "NttDataTest.Persistence.Account/"]
COPY ["NttDataTest.Domain.Account/NttDataTest.Domain.Account.csproj", "NttDataTest.Domain.Account/"]
COPY ["NttDataTest.Common.Account/NttDataTest.Common.Account.csproj", "NttDataTest.Common.Account/"]
COPY ["NttDataTest.Services.Account/NttDataTest.Services.Account.csproj", "NttDataTest.Services.Account/"]
COPY ["NttDataTest.Services.Proxies.Account/NttDataTest.Services.Proxies.Account.csproj", "NttDataTest.Services.Proxies.Account/"]
RUN dotnet restore "NttDataTest.Api.Account/NttDataTest.Api.Account.csproj"
COPY . .
WORKDIR "/src/NttDataTest.Api.Account"
RUN dotnet build "NttDataTest.Api.Account.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NttDataTest.Api.Account.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NttDataTest.Api.Account.dll"]