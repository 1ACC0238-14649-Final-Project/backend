namespace Chats.Interfaces.REST.Resources;

public record ChatResource(
    int Id,
    int SenderId,
    int ReceiverId,
    string Content,
    DateTime SentAt,
    bool IsRead,
    DateTime CreatedDate,
    DateTime? ModifiedDate
);