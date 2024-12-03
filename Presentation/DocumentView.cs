using Spectre.Console;
using System;
using System.Collections.Generic;

public class DocumentView {

    private readonly ViewModel vm; 
    public List<Document> documents;
    public List<Specification> specifications;

    public DocumentView(PostgresDb db)
    {
        if (db == null)
        {
            throw new ArgumentNullException(nameof(db), "PostgresDb instance cannot be null.");
        }

        vm = new ViewModel(db);

        //надо бы отрефакторить (можем дедлокнуть)
        InitializeDocumentsAsync(db).Wait();  
        InitializeSpecificationsAsync(db).Wait();  
    }

    private async Task InitializeDocumentsAsync(PostgresDb db)
    {
        documents = await vm.GetDocumentsFromDatabaseAsync();  
    }

    private async Task InitializeSpecificationsAsync(PostgresDb db)
    {
        specifications = await vm.GetSpecificationsFromDatabaseAsync();  
    }


    public async Task AddNewDocument()
    {
        var documentTable = new Table();
        documentTable.AddColumn("Поле");
        documentTable.AddColumn("Значение");

        string number = AnsiConsole.Ask<string>("Введите номер документа:");
        string date = AnsiConsole.Ask<string>("Введите дату документа (в формате YYYY-MM-DD):");
        string note = AnsiConsole.Ask<string>("Введите примечание документа:");
        int amount = AnsiConsole.Ask<int>("Введите сумму документа:");

        // Проверка уникальности номера
        try
        {
            bool isNumberUnique = await vm.addDocument.IsNumberUniqueAsync(number); 
            if (!isNumberUnique)
            {
                AnsiConsole.MarkupLine("[red]Ошибка: Номер документа должен быть уникальным![/]");
                
                // Сохранение ошибки в базу данных логов
                await vm.logErrorUseCase.LogErrorAsync("Уникальность номера документа", 
                    $"Номер '{number}' уже существует в базе данных.", DateTime.Now);

                return;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Ошибка при проверке уникальности номера: {ex.Message}[/]");
            
            await vm.logErrorUseCase.LogErrorAsync("Проверка уникальности номера", ex.Message, DateTime.Now);

            return;
        }

        documentTable.AddRow("Номер", number);
        documentTable.AddRow("Дата", date);
        documentTable.AddRow("Примечание", note);
        documentTable.AddRow("Сумма", amount.ToString());

        AnsiConsole.Render(documentTable);

        try
        {
            await vm.addDocument.AddDocumentAsync(number, amount, note);
            AnsiConsole.MarkupLine("[green]Документ успешно добавлен в базу данных![/]");
            
            var documentId = documents.Count + 1;
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
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Ошибка при добавлении документа в базу данных: {ex.Message}[/]");

            await vm.logErrorUseCase.LogErrorAsync("Добавление документа", ex.Message, DateTime.Now);
        }
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
        if (document == null)
        {
            AnsiConsole.MarkupLine($"[red]Документ с ID {documentId} не найден![/]");
            return;
        }

        decimal newAmount = AnsiConsole.Ask<decimal>($"Введите новую сумму для документа (текущая сумма: {document.Amount}):");
        string newRemarks = AnsiConsole.Ask<string>($"Введите новые примечания для документа (текущие примечания: {document.Note}):");

        try
        {
            vm.changeDocument.UpdateDocumentAsync(documentId, newAmount, newRemarks).Wait();

            document.Amount = newAmount;
            document.Note = newRemarks;

            AnsiConsole.MarkupLine("[green]Документ успешно обновлен![/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Ошибка при обновлении документа: {ex.Message}[/]");
        }
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