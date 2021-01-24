#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Games.Api/Games.Api.csproj", "Games.Api/"]
COPY ["Games.Core/Games.Core.csproj", "Games.Core/"]
COPY ["Games.Infrastructure/Games.Infrastructure.csproj", "Games.Infrastructure/"]
COPY ["Games.Common/Games.Common.csproj", "Games.Common/"]
RUN dotnet restore "Games.Api/Games.Api.csproj"
COPY . .
WORKDIR "/src/Games.Api"
RUN dotnet build "Games.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Games.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Games.Api.dll"]