# Устанавливаем официальный образ SDK .NET для сборки приложения

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Указываем рабочую директорию внутри контейнера
WORKDIR /app

# Копируем все файлы проекта в рабочую директорию контейнера
COPY . .

# Восстанавливаем зависимости проекта
RUN dotnet restore

# Сборка приложения
RUN dotnet publish -c Release -o /app/publish

# Устанавливаем образ для запуска приложения
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Указываем рабочую директорию для приложения
WORKDIR /app

# Копируем скомпилированные файлы из стадии сборки
COPY --from=build /app/publish .

# Открываем порт, на котором будет работать приложение
EXPOSE 80

# Запускаем приложение
ENTRYPOINT ["dotnet", "myApp.dll"]