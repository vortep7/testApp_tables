FROM --platform=linux/amd64 postgres:latest

ENV POSTGRES_USER=my_user
ENV POSTGRES_PASSWORD=my_password
ENV POSTGRES_DB=my_database

EXPOSE 5432