public class ViewModel
{
    //бизнес задачи 
    public AddDocumentUseCase addDocument { get; set; }
    public ChangeDocumentUC changeDocument { get; set; }
    public DeleteDocumentUseCase deleteDocument { get; set; }

    public AddSpecificationUseCase addSpecification { get; set; }
    public ChangeSpecificationUseCase changeSpecification { get; set; }
    public DeleteSpecificationUseCase deleteSpecification { get; set; }

    public ViewModel(PostgresDb db)
    {
        if (db == null)
        {
            throw new ArgumentNullException(nameof(db), "PostgresDb instance cannot be null");
        }

        //пока тестово - прокидываем в них бд 
        addDocument = new AddDocumentUseCase(db);
        changeDocument = new ChangeDocumentUC(db);
        deleteDocument = new DeleteDocumentUseCase(db);

        addSpecification = new AddSpecificationUseCase(db);
        changeSpecification = new ChangeSpecificationUseCase(db);
        deleteSpecification = new DeleteSpecificationUseCase(db);
    }
}