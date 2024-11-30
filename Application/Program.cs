using Spectre.Console;
using System;

class Program
{
    static void Main(string[] args)
    {
        var view = new DocumentView();

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