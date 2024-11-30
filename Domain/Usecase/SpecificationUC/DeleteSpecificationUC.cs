public class DeleteSpecificationUseCase
{
    private readonly SpecificationRepository _specificationRepository;
    private readonly DocumentRepository _documentRepository;

    public DeleteSpecificationUseCase(SpecificationRepository specificationRepository, DocumentRepository documentRepository)
    {
        _specificationRepository = specificationRepository;
        _documentRepository = documentRepository;
    }

    public void Execute(int specificationId)
    {
        var specification = _specificationRepository.GetById(specificationId);
        if (specification == null)
        {
            throw new InvalidOperationException("Спецификация не найдена.");
        }

        _specificationRepository.Delete(specificationId);

        var document = _documentRepository.GetById(specification.DocumentId);
        document.RecalculateTotal();

        _documentRepository.Update(document);
    }
}