using Npgsql; 

public class PostgresDb
{
    private readonly string _connectionString;

    public PostgresDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    public string ConnectionString => _connectionString;

    public async Task<NpgsqlConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task ExecuteQueryAsync(string query)
    {
        using var connection = await CreateConnectionAsync();
        using var command = new NpgsqlCommand(query, connection);
        await command.ExecuteNonQueryAsync();
    }
}