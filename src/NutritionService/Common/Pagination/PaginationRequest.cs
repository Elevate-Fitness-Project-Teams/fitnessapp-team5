namespace NutritionService.Common.Pagination
{
    public class PaginationRequest
    {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
