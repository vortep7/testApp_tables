using Npgsql;

public class ChangeSpecificationUseCase
{
    private readonly PostgresDb _db;

    public ChangeSpecificationUseCase(PostgresDb db)
    {
        _db = db;
    }

    // Асинхронный метод для изменения спецификации
    public async Task ChangeSpecificationAsync(int specificationId, string newName, decimal newAmount)
    {
        string updateQuery = @"
            UPDATE detail
            SET name = @name, amount = @amount
            WHERE id = @id;
        ";

        // Открываем соединение с базой данных
        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();

        // Создаем команду для обновления спецификации
        using var command = new NpgsqlCommand(updateQuery, connection);
        command.Parameters.AddWithValue("id", specificationId);
        command.Parameters.AddWithValue("name", newName);
        command.Parameters.AddWithValue("amount", newAmount);

        // Выполняем запрос
        await command.ExecuteNonQueryAsync();
    }
}