using System.Text.Json;

namespace NutritionService.Common.Json
{
    public static class JsonArrayReader
    {
        public static IReadOnlyList<string> ReadStringArray(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<string>();
            }

            try
            {
                return JsonSerializer.Deserialize<IReadOnlyList<string>>(json) ?? Array.Empty<string>();
            }
            catch (JsonException)
            {
                return Array.Empty<string>();
            }
        }
    }
}
