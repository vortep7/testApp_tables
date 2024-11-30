public class ChangeDocumentUseCase
{
    private readonly DocumentRepository _documentRepository;
    private readonly SpecificationRepository _specificationRepository;

    public ChangeDocumentUseCase(DocumentRepository documentRepository, SpecificationRepository specificationRepository) {
        _documentRepository = documentRepository;
        _specificationRepository = specificationRepository;
    }

    public void Execute(int documentId, string newNumber, string newNotes) {
        var document = _documentRepository.GetById(documentId);
        if (document == null)
        {
            throw new InvalidOperationException("Документ не найден.");
        }

        if (!_documentRepository.IsDocumentNumberUnique(newNumber))
        {
            throw new InvalidOperationException("Документ с таким номером уже существует.");
        }

        document.Number = newNumber;
        document.Notes = newNotes;

        document.RecalculateTotal();

        _documentRepository.Update(document);
    }
}