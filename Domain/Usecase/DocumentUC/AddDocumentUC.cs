public class AddDocumentUseCase
{
    private readonly DocumentRepository _documentRepository;
    private readonly SpecificationRepository _specificationRepository;

    public AddDocumentUseCase(DocumentRepository documentRepository, SpecificationRepository specificationRepository)  {
        _documentRepository = documentRepository;
        _specificationRepository = specificationRepository;
    }

    public void Execute(Document document, List<Specification> specifications)  {
        if (!_documentRepository.IsDocumentNumberUnique(document.Number))
        {
            throw new InvalidOperationException("Документ с таким номером уже существует.");
        }

        _documentRepository.Add(document);

        foreach (var specification in specifications)
        {
            specification.DocumentId = document.Id;
            _specificationRepository.Add(specification);
        }

        document.RecalculateTotal();

        _documentRepository.Update(document);
    }
}