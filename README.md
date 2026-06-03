
## 🛠️ Технологический стек

- **Backend**: .NET 9, ASP.NET Core, Entity Framework Core (PostgreSQL)
- **Архитектура**: Clean Architecture, CQRS + MediatR, Repositories + UnitOfWork
- **База данных**: PostgreSQL (в Docker)
- **Контейнеризация**: Docker, Docker Compose
- **Аутентификация**: JWT Bearer
- **Фоновые задачи**: Hangfire (хранение в PostgreSQL)
- **Документация API**: Swagger / OpenAPI
- **Фронтенд**: HTML5 / Bootstrap / Vanilla JS (встроенный)

## 🚀 Быстрый старт (через Docker)

**Требования:** Docker Desktop (или Docker + Docker Compose) установлены и запущены.

### 1. Клонировать репозиторий
git clone https://github.com/k1der0/warehouse-monitor.git
cd warehouse-monitor
### 2. Запустить приложение
docker-compose up -d --build

Первый запуск может занять 2–3 минуты (скачивание образов, сборка). После появления сообщения о запуске контейнеров переходите к следующему шагу.

### 3. Проверить работоспособность
Веб-интерфейс: http://localhost:5000
Swagger API: http://localhost:5000/swagger
Hangfire Dashboard: http://localhost:5000/hangfire
Авторизация (в веб-интерфейсе)
  Username: любое (например, admin)
  
  Password: любое (например, 123)
  
Внутри интерфейса можно:

  Добавлять/удалять товары
  
  Записывать приходы и продажи
  
  Генерировать прогноз для выбранного товара
  
  Просматривать таблицу товаров с актуальными остатками
