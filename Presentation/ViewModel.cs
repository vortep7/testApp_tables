using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

public class ViewModel
{

    public AddDocumentUseCase addDocument { get; set; }
    public ChangeDocumentUC changeDocument { get; set; }
    public DeleteDocumentUseCase deleteDocument { get; set; }

    public AddSpecificationUseCase addSpecification { get; set; }
    public ChangeSpecificationUseCase changeSpecification { get; set; }
    public DeleteSpecificationUseCase deleteSpecification { get; set; }

    public GetDocumentsUseCase getDocumentsUseCase { get; set; }
    public GetSpecificationsUseCase getSpecificationsUseCase { get; set; }

    public LogErrorUseCase logErrorUseCase { get; set; }

    public ViewModel(PostgresDb db)
    {
        if (db == null)
        {
            throw new ArgumentNullException(nameof(db), "PostgresDb instance cannot be null");
        }

        addDocument = new AddDocumentUseCase(db);
        changeDocument = new ChangeDocumentUC(db);
        deleteDocument = new DeleteDocumentUseCase(db);

        addSpecification = new AddSpecificationUseCase(db);
        changeSpecification = new ChangeSpecificationUseCase(db);
        deleteSpecification = new DeleteSpecificationUseCase(db);

        getDocumentsUseCase = new GetDocumentsUseCase(db);
        getSpecificationsUseCase = new GetSpecificationsUseCase(db);
        
        logErrorUseCase = new LogErrorUseCase(db);
    }

    public async Task<List<Document>> GetDocumentsFromDatabaseAsync()
    {
        return await getDocumentsUseCase.GetDocumentsAsync();
    }

    public async Task<List<Specification>> GetSpecificationsFromDatabaseAsync()
    {
        return await getSpecificationsUseCase.GetSpecificationsAsync();
    }
}