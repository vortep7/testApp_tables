public class AddSpecificationUseCase
{
    private readonly SpecificationRepository _specificationRepository;
    private readonly DocumentRepository _documentRepository;

    public AddSpecificationUseCase(SpecificationRepository specificationRepository, DocumentRepository documentRepository)
    {
        _specificationRepository = specificationRepository;
        _documentRepository = documentRepository;
    }

    public void Execute(int documentId, Specification specification)
    {
        var document = _documentRepository.GetById(documentId);
        if (document == null)
        {
            throw new InvalidOperationException("Документ не найден.");
        }

        specification.DocumentId = documentId;
        _specificationRepository.Add(specification);

        document.RecalculateTotal();

        _documentRepository.Update(document);
    }
}