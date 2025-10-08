using Chats.Domain;
using Chats.Domain.Models.Entities;
using Chats.Domain.Models.Commands;
using Chats.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Chats.Application.QueryService;

public class ChatQueryService(IChatRepository chatRepository) : IChatQueryService
{
    private readonly IChatRepository _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));

    public async Task<IEnumerable<Chat>> Handle(GetChatsByUserIdQuery query)
    {
        var chats = await _chatRepository.GetChatsByUserIdAsync(query.UserId);
        return chats;
    }
    
    public async Task<IEnumerable<Chat>> Handle(GetChatsBetweenUsersQuery query)
    {
        return await _chatRepository.GetChatsBetweenUsersAsync(query.SenderId, query.ReceiverId);
    }

}