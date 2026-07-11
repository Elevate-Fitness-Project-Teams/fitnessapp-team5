using System.Text;

namespace SmartCoachService.Services.Prompt
{
    public sealed class PromptBuilder : IPromptBuilder
    {
        public string Build(string userMessage, string? userContext)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("""
                                 You are a professional AI Fitness Coach.
                                 
                                 Your responsibilities are:
                                 - Help users achieve their fitness goals.
                                 - Recommend workouts and nutrition.
                                 - Use the provided user context.
                                 - Never invent user data.
                                 - Keep responses concise and actionable.
                             """);

            if (!string.IsNullOrWhiteSpace(userContext))
            {
                prompt.AppendLine("User Context:");
                prompt.AppendLine(userContext);
                prompt.AppendLine();
            }

            prompt.AppendLine("User Question:");
            prompt.AppendLine(userMessage);

            return prompt.ToString();
        }
    }
}