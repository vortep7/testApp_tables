using Spectre.Console;
using System;
using System.Collections.Generic;

public class DocumentView
{
    // Данные для документов
    private List<Document> documents = new List<Document>
    {
        new Document { Id = 1, Number = "123", Date = "01.01.2024", Amount = 1000, Note = "Примечание 1" },
        new Document { Id = 2, Number = "124", Date = "02.01.2024", Amount = 2000, Note = "Примечание 2" }
    };

    // Данные для спецификаций
    private List<Specification> specifications = new List<Specification>
    {
        new Specification { DocumentId = 1, Name = "Спецификация 1", Amount = 300 },
        new Specification { DocumentId = 1, Name = "Спецификация 2", Amount = 700 },
        new Specification { DocumentId = 2, Name = "Спецификация 1", Amount = 1500 }
    };

    // Метод для добавления нового документа
        // Метод для добавления нового документа
    public void AddNewDocument()
    {
    // Создаем таблицу для ввода данных документа
        var documentTable = new Table();
        documentTable.AddColumn("Поле");
        documentTable.AddColumn("Значение");

    // Заполняем таблицу данными
        var documentId = documents.Count + 1; // Новый ID для документа
        string number = AnsiConsole.Ask<string>("Введите номер документа:");
        string date = AnsiConsole.Ask<string>("Введите дату документа:");
        string note = AnsiConsole.Ask<string>("Введите примечание документа:");
        int amount = AnsiConsole.Ask<int>("Введите сумму документа:");  // Новый запрос для суммы

    // Вставляем данные в таблицу
        documentTable.AddRow("Номер", number);
        documentTable.AddRow("Дата", date);
        documentTable.AddRow("Примечание", note);
        documentTable.AddRow("Сумма", amount.ToString());  // Добавление суммы в таблицу

    // Отображаем таблицу
    AnsiConsole.Render(documentTable);

    // Сохраняем новый документ
    var newDocument = new Document
    {
        Id = documentId,
        Number = number,
        Date = date,
        Amount = amount, // Сохраняем введенную сумму
        Note = note
    };

    // Добавляем новый документ в список
    documents.Add(newDocument);

    AnsiConsole.MarkupLine("[green]Документ успешно добавлен в мастер-таблицу![/]");
    }

    // Метод для управления документами (редактирование, удаление)
    public void ManageDocuments()
    {
        // Создаем таблицу Master
        var masterTable = new Table();
        masterTable.AddColumn("ID");
        masterTable.AddColumn("Номер");
        masterTable.AddColumn("Дата");
        masterTable.AddColumn("Сумма");
        masterTable.AddColumn("Примечание");

        // Добавляем строки в таблицу Master
        foreach (var doc in documents)
        {
            masterTable.AddRow(doc.Id.ToString(), doc.Number, doc.Date, doc.Amount.ToString(), doc.Note);
        }

        // Отображаем таблицу Master
        AnsiConsole.Render(masterTable);

        // Выбираем документ из Master
        int documentId = AnsiConsole.Ask<int>("Выберите ID документа для просмотра спецификаций (0 для выхода):");
        if (documentId == 0) return;

        // Фильтруем спецификации по выбранному документу
        var selectedSpecifications = specifications.FindAll(s => s.DocumentId == documentId);

        // Создаем таблицу Detail
        var detailTable = new Table();
        detailTable.AddColumn("Наименование");
        detailTable.AddColumn("Сумма");

        // Добавляем строки в таблицу Detail
        foreach (var spec in selectedSpecifications)
        {
            detailTable.AddRow(spec.Name, spec.Amount.ToString());
        }

        // Отображаем таблицу Detail
        AnsiConsole.Render(detailTable);

        // Предлагаем пользователю выбрать действие
        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Что вы хотите сделать?")
                .AddChoices("Редактировать документ", "Добавить спецификацию", "Удалить документ", "Удалить спецификацию", "Выход"));

        if (action == "Редактировать документ")
        {
            // Редактирование документа
            EditDocument(documentId);
        }
        else if (action == "Добавить спецификацию")
        {
            // Добавление новой спецификации
            AddSpecification(documentId);
        }
        else if (action == "Удалить документ")
        {
            // Удаление документа
            DeleteDocument(documentId);
        }
        else if (action == "Удалить спецификацию")
        {
            // Удаление спецификации
            DeleteSpecification(documentId);
        }
    }

    // Метод для редактирования документа
    private void EditDocument(int documentId)
    {
        var documentToEdit = documents.Find(d => d.Id == documentId);
        documentToEdit.Number = AnsiConsole.Ask<string>("Введите новый номер документа:", documentToEdit.Number);
        documentToEdit.Date = AnsiConsole.Ask<string>("Введите новую дату:", documentToEdit.Date);
        documentToEdit.Amount = AnsiConsole.Ask<int>("Введите новую сумму:", documentToEdit.Amount);
        documentToEdit.Note = AnsiConsole.Ask<string>("Введите новое примечание:", documentToEdit.Note);

        // Обновляем сумму документа
        documentToEdit.Amount = 0;
        foreach (var spec in specifications.FindAll(s => s.DocumentId == documentId))
        {
            documentToEdit.Amount += spec.Amount;
        }
    }

    // Метод для добавления новой спецификации
    private void AddSpecification(int documentId)
    {
        var newSpecName = AnsiConsole.Ask<string>("Введите наименование спецификации:");
        var newSpecAmount = AnsiConsole.Ask<int>("Введите сумму спецификации:");
        specifications.Add(new Specification
        {
            DocumentId = documentId,
            Name = newSpecName,
            Amount = newSpecAmount
        });

        // Обновляем сумму документа
        var updatedDocument = documents.Find(d => d.Id == documentId);
        updatedDocument.Amount = 0;
        foreach (var spec in specifications.FindAll(s => s.DocumentId == documentId))
        {
            updatedDocument.Amount += spec.Amount;
        }
    }

    // Метод для удаления документа
    private void DeleteDocument(int documentId)
    {
        var documentToDelete = documents.Find(d => d.Id == documentId);
        documents.Remove(documentToDelete);

        // Удаляем все спецификации, связанные с документом
        specifications.RemoveAll(s => s.DocumentId == documentId);

        AnsiConsole.MarkupLine("[green]Документ удален успешно![/]");
    }

    // Метод для удаления спецификации
    private void DeleteSpecification(int documentId)
    {
        var selectedSpecifications = specifications.FindAll(s => s.DocumentId == documentId);
        var specificationToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите спецификацию для удаления")
                .AddChoices(selectedSpecifications.ConvertAll(s => s.Name)));

        var specToRemove = specifications.Find(s => s.Name == specificationToDelete && s.DocumentId == documentId);
        specifications.Remove(specToRemove);

        // Пересчитываем сумму документа
        var updatedDoc = documents.Find(d => d.Id == documentId);
        updatedDoc.Amount = 0;
        foreach (var spec in specifications.FindAll(s => s.DocumentId == documentId))
        {
            updatedDoc.Amount += spec.Amount;
        }

        AnsiConsole.MarkupLine("[green]Спецификация удалена успешно![/]");
    }

    // Класс для представления документа
    public class Document
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public string Note { get; set; }
    }

    public class Specification
    {
        public int DocumentId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}