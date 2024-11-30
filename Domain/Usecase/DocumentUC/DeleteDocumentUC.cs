public class DeleteDocumentUseCase
{
    private readonly DocumentRepository _documentRepository;
    private readonly SpecificationRepository _specificationRepository;

    public DeleteDocumentUseCase(DocumentRepository documentRepository, SpecificationRepository specificationRepository){
        _documentRepository = documentRepository;
        _specificationRepository = specificationRepository;
    }

    public void Execute(int documentId){
        var document = _documentRepository.GetById(documentId);
        if (document == null)
        {
            throw new InvalidOperationException("Документ не найден.");
        }

        var specifications = _specificationRepository.GetByDocumentId(documentId);
        foreach (var specification in specifications)
        {
            _specificationRepository.Delete(specification.Id);
        }

        _documentRepository.Delete(documentId);
    }
}