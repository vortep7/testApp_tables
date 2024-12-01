# Используем официальный образ PostgreSQL
FROM --platform=linux/amd64 postgres:latest

# Устанавливаем переменные окружения для PostgreSQL
ENV POSTGRES_USER=my_user
ENV POSTGRES_PASSWORD=my_password
ENV POSTGRES_DB=my_database

# Открываем порт 5432 для PostgreSQL
EXPOSE 5432