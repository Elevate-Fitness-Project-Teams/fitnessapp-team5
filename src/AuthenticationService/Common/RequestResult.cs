using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuthenticationService.Common
{
    public record RequestResult<TResult>(bool IsSuccess,
                                  string Message,
                                  TResult? Data,
                                  IEnumerable<string> Errors,
                                  int StatusCode,
                                  DateTime Timestamp)
        where TResult : class
    {
        public static RequestResult<TResult> Success(
            TResult data,
            string message = "Operation completed successfully.")
            => new(true,
                   message,
                   data,
                   [],
                   200,
                   DateTime.UtcNow);

        public static RequestResult<TResult> Failure(
            string message,
            IEnumerable<string>? errors = null,
            int statusCode = 400)
            => new(false,
                   message,
                   default,
                   errors ?? [],
                   statusCode,
                   DateTime.UtcNow);
    }
}
