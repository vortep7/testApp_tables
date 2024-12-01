using Spectre.Console;
using System;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Подключение к базе данных
        var connectionString = "Host=localhost;Port=5432;Database=my_database;Username=my_user;Password=my_password";
        var postgresDb = new PostgresDb(connectionString);
        var documentRepository = new CreateTableUseCase(postgresDb);

        var app = new App(documentRepository);

        await app.RunAsync();

        // Запуск вьюшки, инжектим бд во вью 
        await app.RunMainMenuAsync(documentRepository,postgresDb);
    }
}