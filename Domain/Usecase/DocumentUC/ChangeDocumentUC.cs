using Npgsql;

public class ChangeDocumentUC
{
    private readonly PostgresDb _db;

    public ChangeDocumentUC(PostgresDb db)
    {
        _db = db;
    }
    
    public async Task UpdateDocumentAsync(int id, decimal newAmount, string newRemarks)
    {
        
    }
}