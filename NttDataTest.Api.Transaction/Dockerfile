#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NttDataTest.Api.Transaction/NttDataTest.Api.Transaction.csproj", "NttDataTest.Api.Transaction/"]
COPY ["NttDataTest.Persistence.Transaction/NttDataTest.Persistence.Transaction.csproj", "NttDataTest.Persistence.Transaction/"]
COPY ["NttDataTest.Domain.Transaction/NttDataTest.Domain.Transaction.csproj", "NttDataTest.Domain.Transaction/"]
COPY ["NttDataTest.Common.Transaction/NttDataTest.Common.Transaction.csproj", "NttDataTest.Common.Transaction/"]
COPY ["NttDataTest.Services.Transaction/NttDataTest.Services.Transaction.csproj", "NttDataTest.Services.Transaction/"]
COPY ["NttDataTest.Services.Proxies.Transaction/NttDataTest.Services.Proxies.Transaction.csproj", "NttDataTest.Services.Proxies.Transaction/"]
RUN dotnet restore "NttDataTest.Api.Transaction/NttDataTest.Api.Transaction.csproj"
COPY . .
WORKDIR "/src/NttDataTest.Api.Transaction"
RUN dotnet build "NttDataTest.Api.Transaction.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NttDataTest.Api.Transaction.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NttDataTest.Api.Transaction.dll"]