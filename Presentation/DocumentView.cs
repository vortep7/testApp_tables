using Spectre.Console;
using System;
using System.Collections.Generic;

public class DocumentView {

    private readonly ViewModel vm; 
    private readonly List<Document> documents;

    public DocumentView(PostgresDb db)
    {
        if (db == null)
        {
            throw new ArgumentNullException(nameof(db), "PostgresDb instance cannot be null.");
        }

        vm = new ViewModel(db);

        documents = new List<Document>
        {
            //тестовые документы 
        };
    }

    private List<Specification> specifications = new List<Specification>
    {
        //тестовые спецификации 
    };

    public async Task AddNewDocument()
    {
        var documentTable = new Table();
        documentTable.AddColumn("Поле");
        documentTable.AddColumn("Значение");

        var documentId = documents.Count + 1;
        string number = AnsiConsole.Ask<string>("Введите номер документа:");
        string date = AnsiConsole.Ask<string>("Введите дату документа (в формате YYYY-MM-DD):");
        string note = AnsiConsole.Ask<string>("Введите примечание документа:");
        int amount = AnsiConsole.Ask<int>("Введите сумму документа:");  

        documentTable.AddRow("Номер", number);
        documentTable.AddRow("Дата", date);
        documentTable.AddRow("Примечание", note);
        documentTable.AddRow("Сумма", amount.ToString()); 

        AnsiConsole.Render(documentTable);

        try
        {
            await vm.addDocument.AddDocumentAsync(number, amount, note);
            AnsiConsole.MarkupLine("[green]Документ успешно добавлен в базу данных![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Ошибка при добавлении документа в базу данных: {ex.Message}[/]");
        }

        var newDocument = new Document
        {
            Id = documentId,
            Number = number,
            Date = date,
            Amount = amount,
            Note = note
        };

        documents.Add(newDocument);
    }

    public void ManageDocuments()
    {
        var masterTable = new Table();
        masterTable.AddColumn("ID");
        masterTable.AddColumn("Номер");
        masterTable.AddColumn("Дата");
        masterTable.AddColumn("Сумма");
        masterTable.AddColumn("Примечание");

        foreach (var doc in documents)
        {
            masterTable.AddRow(doc.Id.ToString(), doc.Number, doc.Date, doc.Amount.ToString(), doc.Note);
        }

        AnsiConsole.Render(masterTable);

        int documentId = AnsiConsole.Ask<int>("Выберите ID документа для просмотра спецификаций (0 для выхода):");
        if (documentId == 0) return;

        var selectedSpecifications = specifications.FindAll(s => s.DocumentId == documentId);

        var detailTable = new Table();
        detailTable.AddColumn("Id");
        detailTable.AddColumn("Наименование");
        detailTable.AddColumn("Сумма");
        

        foreach (var spec in selectedSpecifications)
        {
            detailTable.AddRow(spec.Id.ToString(),spec.Name,spec.Amount.ToString());
        }

        AnsiConsole.Render(detailTable);

        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Что вы хотите сделать?")
                .AddChoices("Редактировать документ", "Добавить спецификацию", "Удалить документ", "Удалить спецификацию", "Выход"));

        if (action == "Редактировать документ")
        {
            EditDocument(documentId);
        }
        else if (action == "Добавить спецификацию")
        {
            AddSpecification(documentId);
        }
        else if (action == "Удалить документ")
        {
            DeleteDocument(documentId);
        }
        else if (action == "Удалить спецификацию")
        {
            DeleteSpecification(documentId);
        }
    }

    private void EditDocument(int documentId)
    {
        var document = documents.Find(d => d.Id == documentId);
        decimal newAmount = AnsiConsole.Ask<decimal>($"Введите новую сумму для документа (текущая сумма: {document.Amount}):");
        string newRemarks = AnsiConsole.Ask<string>($"Введите новые примечания для документа (текущие примечания: {document.Note}):");

        vm.changeDocument.UpdateDocumentAsync(documentId, newAmount, newRemarks).Wait();

        document.Amount = newAmount;
        document.Note = newRemarks;

        AnsiConsole.MarkupLine("[green]Документ успешно обновлен![/]");
    }

    private async Task AddSpecification(int documentId)
    {
        var newSpecName = AnsiConsole.Ask<string>("Введите наименование спецификации:");
        var newSpecAmount = AnsiConsole.Ask<decimal>("Введите сумму спецификации:");

        int newSpecId = await vm.addSpecification.AddSpecificationAsync(documentId, newSpecName, newSpecAmount);

        specifications.Add(new Specification
        {
            DocumentId = documentId,
            Name = newSpecName,
            Amount = newSpecAmount,
            Id = newSpecId  
        });

        var updatedDocument = documents.Find(d => d.Id == documentId);
        updatedDocument.Amount = 0;
        foreach (var spec in specifications.FindAll(s => s.DocumentId == documentId))
        {
            updatedDocument.Amount += spec.Amount;
        }

        AnsiConsole.MarkupLine("[green]Спецификация успешно добавлена![/]");
    }

    private async void DeleteDocument(int documentId)
    {
        var documentToDelete = documents.Find(d => d.Id == documentId);
        if (documentToDelete != null)
        {
        documents.Remove(documentToDelete);

        specifications.RemoveAll(s => s.DocumentId == documentId);

        await vm.deleteDocument.DeleteDocumentAsync(documentId);

        AnsiConsole.MarkupLine("[green]Документ удален успешно![/]");
        }
        else
        {
        AnsiConsole.MarkupLine("[red]Документ не найден.[/]");
        }
    }

    private async Task DeleteSpecification(int documentId)
    {
        var selectedSpecifications = specifications.FindAll(s => s.DocumentId == documentId);
        var specificationToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Выберите спецификацию для удаления")
                .AddChoices(selectedSpecifications.ConvertAll(s => s.Name)));

        var specToRemove = specifications.Find(s => s.Name == specificationToDelete && s.DocumentId == documentId);

        await vm.deleteSpecification.DeleteSpecificationAsync(specToRemove.Id);

        specifications.Remove(specToRemove);

        var updatedDoc = documents.Find(d => d.Id == documentId);
        updatedDoc.Amount = 0;
        foreach (var spec in specifications.FindAll(s => s.DocumentId == documentId))
        {
            updatedDoc.Amount += spec.Amount;
        }

        AnsiConsole.MarkupLine("[green]Спецификация удалена успешно![/]");
    }
}