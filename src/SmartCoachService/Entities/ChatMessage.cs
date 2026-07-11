using SmartCoachService.Entities.enums;

namespace SmartCoachService.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; private set; }
        public SenderType Sender { get; private set; }
        public string Message { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }

        //////////////////////////////////////////////////////////////////////

        public Guid ChatSessionId { get; private set; }
        public ChatSession ChatSession { get; private set; } = default!;

        //////////////////////////////////////////////////////////////////////

        public static ChatMessage Create(Guid sessionId,
                                         SenderType sender,
                                         string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be empty.", nameof(message));

            return new ChatMessage
            {
                Id = Guid.CreateVersion7(),
                ChatSessionId = sessionId,
                Sender = sender,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
