namespace SmartCoachService.Common
{
    public sealed class AIProviderSettings
    {
        public OpenAISettings OpenAI { get; set; } = new();
        public ClaudeSettings Claude { get; set; } = new();
    }

    public sealed class OpenAISettings
    {
        public bool Enabled { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }

    public sealed class ClaudeSettings
    {
        public bool Enabled { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }
}
