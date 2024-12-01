using Spectre.Console;
using System;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Подключение к базе данных
        var connectionString = "Host=localhost;Port=5432;Database=my_database;Username=my_user;Password=my_password";
        var postgresDb = new PostgresDb(connectionString);
        var documentRepository = new DocumentRepositoryService(postgresDb);

        var app = new App(documentRepository);

        await app.RunAsync();

        // Запуск вьюшки, инжектим бд во вью 
        await RunMainMenuAsync(documentRepository,postgresDb);
    }

    private static async Task RunMainMenuAsync(DocumentRepositoryService documentRepository, PostgresDb postgresDb)
    {
        var view = new DocumentView(postgresDb);

        while (true)
        {
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Что вы хотите сделать?")
                    .AddChoices("Добавить новый документ", "Просмотр и редактирование документа", "Выход"));

            if (action == "Добавить новый документ")
            {
                view.AddNewDocument();
            }
            else if (action == "Просмотр и редактирование документа")
            {
                view.ManageDocuments();
            }
            else
            {
                break;
            }
        }
    }
}