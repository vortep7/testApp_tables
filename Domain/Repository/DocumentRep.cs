public interface DocumentRepository
{
    void Add(Document document);
    void Update(Document document);
    void Delete(int documentId);
    bool ExistsByNumber(string documentNumber);

    bool IsDocumentNumberUnique(string documentNumber);
}