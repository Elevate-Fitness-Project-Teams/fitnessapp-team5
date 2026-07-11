namespace SmartCoachService.Clients.Progress
{
    public sealed class ProgressClient(HttpClient _httpClient) : IProgressClient
    {
        public async Task<ProgressResponse?> GetUserProgressAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<ProgressResponse>(
                $"api/v1/progress/{userId}",
                cancellationToken);
        }
    }
}
