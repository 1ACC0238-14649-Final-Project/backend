using gigu_back_end.Shared.Domain.Model.Entities;

namespace Chats.Domain.Models.Entities;

public class Chat : BaseEntity
    {
        public Chat() { }

        public Chat(int senderId, int receiverId, string content)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            SentAt = DateTime.UtcNow;
            IsRead = false;
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
        }

        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }