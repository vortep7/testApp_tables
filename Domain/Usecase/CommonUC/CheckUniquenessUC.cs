public class CheckUniquenessUseCase
{
    private readonly DocumentRepository _documentRepository;
    private readonly SpecificationRepository _specificationRepository;

    public CheckUniquenessUseCase(DocumentRepository documentRepository, SpecificationRepository specificationRepository)
    {
        _documentRepository = documentRepository;
        _specificationRepository = specificationRepository;
    }

    public bool IsDocumentNumberUnique(string number)
    {
        return !_documentRepository.ExistsByNumber(number);
    }

    public bool IsSpecificationNameUnique(string name)
    {
        return !_specificationRepository.ExistsByName(name);
    }
}