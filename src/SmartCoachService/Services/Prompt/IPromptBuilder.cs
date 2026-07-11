namespace SmartCoachService.Services.Prompt
{
    public interface IPromptBuilder
    {
        string Build(string userMessage, string? userContext);
    }
}
