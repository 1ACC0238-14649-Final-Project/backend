namespace Chats.Domain.Models.Commands;

public record GetChatsBetweenUsersQuery(int SenderId, int ReceiverId);