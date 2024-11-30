public class ChangeSpecificationUseCase
{
    private readonly SpecificationRepository _specificationRepository;
    private readonly DocumentRepository _documentRepository;

    public ChangeSpecificationUseCase(SpecificationRepository specificationRepository, DocumentRepository documentRepository)
    {
        _specificationRepository = specificationRepository;
        _documentRepository = documentRepository;
    }

    public void Execute(int specificationId, string newName, double newAmount)
    {
        var specification = _specificationRepository.GetById(specificationId);
        if (specification == null)
        {
            throw new InvalidOperationException("Спецификация не найдена.");
        }

        specification.Name = newName;
        specification.Amount = newAmount;

        var document = _documentRepository.GetById(specification.DocumentId);
        document.RecalculateTotal();

        _documentRepository.Update(document);
        _specificationRepository.Update(specification);
    }
}