# Этап 1: Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Установка OpenMP для MKL (требуется ML.NET на Linux)
RUN apt-get update && apt-get install -y libomp-dev

# Копируем csproj файлы и восстанавливаем зависимости
COPY ["WarehouseMonitor.API/WarehouseMonitor.API.csproj", "WarehouseMonitor.API/"]
COPY ["WarehouseMonitor.Application/WarehouseMonitor.Application.csproj", "WarehouseMonitor.Application/"]
COPY ["WarehouseMonitor.Domain/WarehouseMonitor.Domain.csproj", "WarehouseMonitor.Domain/"]
COPY ["WarehouseMonitor.Infrastructure/WarehouseMonitor.Infrastructure.csproj", "WarehouseMonitor.Infrastructure/"]

RUN dotnet restore "WarehouseMonitor.API/WarehouseMonitor.API.csproj"

# Копируем исходный код
COPY . .

WORKDIR "/src/WarehouseMonitor.API"
RUN dotnet build "WarehouseMonitor.API.csproj" -c Release -o /app/build

# Этап 2: Публикация приложения
FROM build AS publish
RUN dotnet publish "WarehouseMonitor.API.csproj" -c Release -o /app/publish

# Этап 3: Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Установка OpenMP в финальном образе (критически важно для работы ML.NET)
RUN apt-get update && apt-get install -y libomp-dev && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseMonitor.API.dll"]