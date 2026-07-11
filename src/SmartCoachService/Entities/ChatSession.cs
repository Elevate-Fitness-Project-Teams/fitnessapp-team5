namespace SmartCoachService.Entities
{
    public class ChatSession
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = default!;
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        ////////////////////////////////////////////////////////

        private readonly List<ChatMessage> _messages = [];
        public IReadOnlyCollection<ChatMessage> Messages => _messages.AsReadOnly();

        ////////////////////////////////////////////////////////

        public static ChatSession Create(Guid userId)
        {
            return new ChatSession
            {
                Id = Guid.CreateVersion7(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void AddMessage(ChatMessage message)
        {
            _messages.Add(message);
        }
    }
}
