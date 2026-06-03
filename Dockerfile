FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем csproj файлы (пути без src/)
COPY ["WarehouseMonitor.API/WarehouseMonitor.API.csproj", "WarehouseMonitor.API/"]
COPY ["WarehouseMonitor.Application/WarehouseMonitor.Application.csproj", "WarehouseMonitor.Application/"]
COPY ["WarehouseMonitor.Domain/WarehouseMonitor.Domain.csproj", "WarehouseMonitor.Domain/"]
COPY ["WarehouseMonitor.Infrastructure/WarehouseMonitor.Infrastructure.csproj", "WarehouseMonitor.Infrastructure/"]

RUN dotnet restore "WarehouseMonitor.API/WarehouseMonitor.API.csproj"

# Копируем весь остальной код (текущая директория, где лежат папки проектов)
COPY . .

WORKDIR "/app/WarehouseMonitor.API"
RUN dotnet build "WarehouseMonitor.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WarehouseMonitor.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseMonitor.API.dll"]