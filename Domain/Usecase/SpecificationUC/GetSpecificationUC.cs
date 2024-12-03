using Npgsql;

public class GetSpecificationsUseCase {
    private readonly PostgresDb _db;

    public GetSpecificationsUseCase(PostgresDb db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<List<Specification>> GetSpecificationsAsync()
    {
        var sqlQuery = "SELECT * FROM detail"; 

        var specifications = new List<Specification>();
        
        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync(); 

        using var command = new NpgsqlCommand(sqlQuery, connection);
        
        using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            var specification = new Specification
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")), 
                DocumentId = reader.GetInt32(reader.GetOrdinal("master_id")),
                Name = reader.GetString(reader.GetOrdinal("name")),  
                Amount = reader.GetDecimal(reader.GetOrdinal("amount"))  
            };

            specifications.Add(specification);
        }

        return specifications;
    }
}