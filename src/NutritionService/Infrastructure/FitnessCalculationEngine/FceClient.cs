using Microsoft.Extensions.Options;

namespace NutritionService.Infrastructure.FitnessCalculationEngine
{
    public class FceClient : IFceClient
    {
        private readonly HttpClient _httpClient;
        private readonly FceOptions _options;

        public FceClient(HttpClient httpClient, IOptions<FceOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        public async Task<UserNutritionTargetsDto?> GetUserTargetsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var path = _options.TargetPath.Replace("{userId}", userId.ToString());

            var response = await _httpClient.GetAsync(path, cancellationToken);

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<UserNutritionTargetsDto>(cancellationToken: cancellationToken);
        }
    }
}
