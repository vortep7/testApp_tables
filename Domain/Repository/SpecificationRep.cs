public interface SpecificationRepository
{
    Specification GetById(int specificationId);
    List<Specification> GetByDocumentId(int documentId);
    void Add(Specification specification);
    void Update(Specification specification);
    void Delete(int specificationId);

    bool ExistsByName(string specificationName);
}