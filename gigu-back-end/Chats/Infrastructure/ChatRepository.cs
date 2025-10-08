using Chats.Domain;
using Chats.Domain.Models.Entities;
using gigu_back_end.Shared.Infraestructure.Persistence.Repositories;
using gigu_back_end.Shared.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Chats.Infrastructure.Persistence.EFC.Repositories;

public class ChatRepository : BaseRepository<Chat>, IChatRepository
{
    public ChatRepository(GigUContext context) : base(context) { }

    public async Task<IEnumerable<Chat>> GetChatsByUserIdAsync(int userId)
    {
        return await Context.Set<Chat>()
            .Where(c => c.SenderId == userId || c.ReceiverId == userId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Chat>> GetChatsBetweenUsersAsync(int senderId, int receiverId)
    {
        return await Context.Set<Chat>()
            .Where(c => 
                (c.SenderId == senderId && c.ReceiverId == receiverId) ||
                (c.SenderId == receiverId && c.ReceiverId == senderId))
            .OrderBy(c => c.SentAt)
            .ToListAsync();
    }

}
