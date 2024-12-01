using Spectre.Console;
using System;

public class App
{
    private readonly CreateTableUseCase _createTable;

    public App(CreateTableUseCase createTable)
    {
        _createTable = createTable;
    }

    public async Task RunAsync()
    {
        try
        {
            Console.WriteLine("Подключение к базе данных...");
            await _createTable.CreateTablesAsync();
            Console.WriteLine("Таблицы 'master' и 'detail' созданы.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public async Task RunMainMenuAsync(CreateTableUseCase documentRepository, PostgresDb postgresDb)
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