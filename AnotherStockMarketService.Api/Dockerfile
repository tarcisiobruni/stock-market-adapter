#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy everything
COPY ./ ./

# Restore as distinct layers
RUN dotnet restore "./AnotherStockMarketService.Api/AnotherStockMarketService.Api.csproj" 

RUN dotnet publish "./AnotherStockMarketService.Api/AnotherStockMarketService.Api.csproj" -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

ENTRYPOINT ["dotnet", "AnotherStockMarketService.Api.dll"]