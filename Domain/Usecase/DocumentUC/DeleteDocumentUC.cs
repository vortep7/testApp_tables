

using Npgsql;

public class DeleteDocumentUseCase
{
    private readonly PostgresDb _db;

    public DeleteDocumentUseCase(PostgresDb db)
    {
        _db = db;
    }
    
    public async Task DeleteDocumentAsync(int id)
    {
        string deleteQuery = @"
            DELETE FROM master WHERE id = @id;
        ";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(deleteQuery, connection);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }
}