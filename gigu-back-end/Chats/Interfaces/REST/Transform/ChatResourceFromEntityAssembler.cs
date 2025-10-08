using Chats.Domain.Models.Entities;
using Chats.Interfaces.REST.Resources;

namespace Chats.Interfaces.REST.Transform;

public static class ChatResourceFromEntityAssembler
{
    public static ChatResource ToResourceFromEntity(Chat chat)
    {
        return new ChatResource(
            chat.Id,
            chat.SenderId,
            chat.ReceiverId,
            chat.Content,
            chat.SentAt,
            chat.IsRead,
            chat.CreatedDate,
            chat.ModifiedDate
        );
    }
}