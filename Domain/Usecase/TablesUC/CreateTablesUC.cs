using Npgsql;

public class CreateTableUseCase
{
    private readonly PostgresDb _db;

    public CreateTableUseCase(PostgresDb db)
    {
        _db = db;
    }

    public async Task CreateTablesAsync()
    {
        string createMasterTableQuery = @"
            CREATE TABLE IF NOT EXISTS master (
                id SERIAL PRIMARY KEY,
                number VARCHAR(50) UNIQUE NOT NULL,
                date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                amount DECIMAL(10, 2) NOT NULL,
                remarks TEXT
            );
        ";

        string createDetailTableQuery = @"
            CREATE TABLE IF NOT EXISTS detail (
                id SERIAL PRIMARY KEY,
                master_id INT REFERENCES master(id) ON DELETE CASCADE,
                name VARCHAR(100) NOT NULL,
                amount DECIMAL(10, 2) NOT NULL
            );
        ";

        string createLogTableQuery = @"
            CREATE TABLE IF NOT EXISTS logs (
                id SERIAL PRIMARY KEY,
                error_message TEXT NOT NULL,
                error_details TEXT,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );
        ";

        await _db.ExecuteQueryAsync(createMasterTableQuery);
        await _db.ExecuteQueryAsync(createDetailTableQuery);
        await _db.ExecuteQueryAsync(createLogTableQuery);
    }
}