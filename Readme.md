# Руководство по запуску

## Веб-приложение

Переходим в основную директорию (modsen-events)

```
docker-compose up --build
```

В докере запустятся 
- EventsAPI :8080
- EventsReact :3000
- PostgreSQL :5432

По пути http://localhost:3000 будет доступно приложение

## Тесты

По пути /modsen-event/EventsAPI.Tests расположены тесты

В командной строке прописываем

```
dotnet test EventsAPI.Tests.csproj
```