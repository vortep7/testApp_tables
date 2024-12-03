using Npgsql;

public class GetDocumentsUseCase
{
    private readonly PostgresDb _db;

    public GetDocumentsUseCase(PostgresDb db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<List<Document>> GetDocumentsAsync()
    {
        var sqlQuery = "SELECT * FROM master"; 

        var documents = new List<Document>();
        
        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();
        using var command = new NpgsqlCommand(sqlQuery, connection);
        
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            var document = new Document
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),  
                Number = reader.GetString(reader.GetOrdinal("number")), 
                Date = reader.GetDateTime(reader.GetOrdinal("date")).ToString("yyyy-MM-dd HH:mm:ss"),  
                Amount = reader.GetDecimal(reader.GetOrdinal("amount")),  
                Note = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks"))  
            };

            documents.Add(document);
        }
        return documents;
    }
}