
using Npgsql;

public class AddSpecificationUseCase
{
    private readonly PostgresDb _db;

    public AddSpecificationUseCase(PostgresDb db)
    {
        _db = db;
    }
    
    public async Task<int> AddSpecificationAsync(int masterId, string name, decimal amount)
    {
        string insertQuery = @"
            INSERT INTO detail (master_id, name, amount)
            VALUES (@master_id, @name, @amount)
            RETURNING id;
        ";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(insertQuery, connection);
        command.Parameters.AddWithValue("master_id", masterId);
        command.Parameters.AddWithValue("name", name);
        command.Parameters.AddWithValue("amount", amount);

        // Получаем сгенерированный ID
        var newId = (int)await command.ExecuteScalarAsync();
        return newId;
    }

}