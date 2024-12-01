
using Npgsql;

public class DeleteSpecificationUseCase
{
    private readonly PostgresDb _db;

    public DeleteSpecificationUseCase(PostgresDb db)
    {
        _db = db;
    }
    
    public async Task DeleteSpecificationAsync(int id)
    {
        string deleteQuery = @"
            DELETE FROM detail WHERE id = @id;
        ";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(deleteQuery, connection);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync();
    }
}