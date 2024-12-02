public interface SpecificationRepository
{
    void AddSpecification(int specificationId);
    void DeleteSpecification(int documentId);
    void Add(Specification specification);
    void Update(Specification specification);
    void Delete(int specificationId);

    bool ExistsByName(string specificationName);
}