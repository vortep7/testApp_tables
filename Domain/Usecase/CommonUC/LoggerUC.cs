// public class LogErrorUseCase
// {
//     private readonly IErrorLogRepository _errorLogRepository;

//     public LogErrorUseCase(IErrorLogRepository errorLogRepository)
//     {
//         _errorLogRepository = errorLogRepository;
//     }

//     public void LogError(string message, Exception exception)
//     {
//         _errorLogRepository.Log(new ErrorLog
//         {
//             Message = message,
//             ExceptionDetails = exception.ToString(),
//             Timestamp = DateTime.Now
//         });
//     }
// }