using Npgsql;

public class AddDocumentUseCase
{
    private readonly PostgresDb _db;

    public AddDocumentUseCase(PostgresDb db)
    {
        _db = db;
    }
    
   public async Task AddDocumentAsync(string number, decimal amount, string remarks)
    {
        string insertQuery = @"
            INSERT INTO master (number, amount, remarks)
            VALUES (@number, @amount, @remarks);
        ";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();
        
        using var command = new NpgsqlCommand(insertQuery, connection);
        command.Parameters.AddWithValue("number", number);
        command.Parameters.AddWithValue("amount", amount);
        command.Parameters.AddWithValue("remarks", remarks);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<bool> IsNumberUniqueAsync(string number)
    {
        var query = "SELECT COUNT(*) FROM master WHERE number = @number";
        using (var connection = new NpgsqlConnection(_db.ConnectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@number", number);
                var count = (long)await command.ExecuteScalarAsync();
                return count == 0;
            }
        }
    }
}