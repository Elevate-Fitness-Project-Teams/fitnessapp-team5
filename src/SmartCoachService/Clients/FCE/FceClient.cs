namespace SmartCoachService.Clients.FCE
{
    public sealed class FceClient(HttpClient _httpClient)
               : IFceClient
    {
        public async Task<FceResponse?> GetUserContextAsync(Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<FceResponse>(
                $"api/v1/fce/stats/GetBiometrics/{userId}",
                cancellationToken);
        }
    }
}
