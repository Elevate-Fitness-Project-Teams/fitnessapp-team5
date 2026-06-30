namespace NutritionService.Common.DTOs
{
    public record ApiResponse<TResult>(TResult Data, bool IsSuccess, string errorMessage)
    {
        public static ApiResponse<TResult> Success(TResult data) => new(data, true, string.Empty);
        public static ApiResponse<TResult> Failure(string errorMessage) => new(default!, false, errorMessage);

    }
}
