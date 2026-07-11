namespace Bulk.Shared.Settings;

public class RabbitMqOptions
{
    public readonly static string SectionName = "RabbitMQ";
    public string Host { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
} 