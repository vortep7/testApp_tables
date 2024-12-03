using Npgsql;

public class LogErrorUseCase {

    private readonly PostgresDb _db;

    public LogErrorUseCase(PostgresDb db)
    {
        _db = db;
    }

    public async Task LogErrorAsync(string errorMessage, string errorDetails = null, DateTime? timestamp = null)
    {
        string query = @"
            INSERT INTO logs (error_message, error_details, created_at) 
            VALUES (@errorMessage, @errorDetails, @createdAt)";

        using var connection = new NpgsqlConnection(_db.ConnectionString);
        await connection.OpenAsync();
        
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@errorMessage", errorMessage);
        command.Parameters.AddWithValue("@errorDetails", (object)errorDetails ?? DBNull.Value);
        command.Parameters.AddWithValue("@createdAt", timestamp ?? DateTime.UtcNow);

        await command.ExecuteNonQueryAsync();
    }
}