using Npgsql;

public class ChangeSpecificationUseCase
{
    private readonly PostgresDb _db;

    public ChangeSpecificationUseCase(PostgresDb db)
    {
        _db = db;
    }

    public async Task ChangeSpecificationAsync(int specificationId, string newName, decimal newAmount)
    {
        string updateQuery = @"
            UPDATE detail
            SET name = @name, amount = @amount
            WHERE id = @id;
        ";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(updateQuery, connection);
        command.Parameters.AddWithValue("id", specificationId);
        command.Parameters.AddWithValue("name", newName);
        command.Parameters.AddWithValue("amount", newAmount);

        await command.ExecuteNonQueryAsync();
    }
}