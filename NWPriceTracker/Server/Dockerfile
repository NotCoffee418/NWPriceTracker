#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NWPriceTracker/Server/NWPriceTracker.Server.csproj", "NWPriceTracker/Server/"]
COPY ["NWPriceTracker/Shared/NWPriceTracker.Shared.csproj", "NWPriceTracker/Shared/"]
COPY ["NWPriceTracker/Client/NWPriceTracker.Client.csproj", "NWPriceTracker/Client/"]
RUN dotnet restore "NWPriceTracker/Server/NWPriceTracker.Server.csproj"
COPY . .
WORKDIR "/src/NWPriceTracker/Server"
RUN dotnet build "NWPriceTracker.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NWPriceTracker.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NWPriceTracker.Server.dll"]
