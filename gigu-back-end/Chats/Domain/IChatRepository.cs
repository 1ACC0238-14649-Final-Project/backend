using Chats.Domain.Models.Entities;
using gigu_back_end.Shared.Domain;

namespace Chats.Domain;

public interface IChatRepository : IBaseRepository<Chat>
{
    Task<IEnumerable<Chat>> GetChatsByUserIdAsync(int userId);
    
    Task<IEnumerable<Chat>> GetChatsBetweenUsersAsync(int senderId, int receiverId);
}