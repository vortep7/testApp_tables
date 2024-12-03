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
        string updateQuery = @"
            UPDATE master 
            SET amount = @newAmount, remarks = @newRemarks 
            WHERE id = @id;
        ";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();

        using var command = new NpgsqlCommand(updateQuery, connection);
        command.Parameters.AddWithValue("id", id);
        command.Parameters.AddWithValue("newAmount", newAmount);
        command.Parameters.AddWithValue("newRemarks", newRemarks);

        try
        {
            var rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                throw new Exception($"Документ с ID {id} не найден.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обновлении документа: {ex.Message}");
        }
    }
}