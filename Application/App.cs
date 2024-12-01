public class App
{
    private readonly DocumentRepositoryService _documentRepository;

    public App(DocumentRepositoryService documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task RunAsync()
    {
        try
        {
            Console.WriteLine("Подключение к базе данных...");
            await _documentRepository.CreateTablesAsync();
            Console.WriteLine("Таблицы 'master' и 'detail' созданы.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}