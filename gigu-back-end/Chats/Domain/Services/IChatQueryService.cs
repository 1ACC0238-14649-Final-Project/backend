using Chats.Domain.Models.Commands;
using Chats.Domain.Models.Entities;

namespace Chats.Domain.Services;

public interface IChatQueryService
{
    Task<IEnumerable<Chat>> Handle(GetChatsByUserIdQuery query);
    Task<IEnumerable<Chat>> Handle(GetChatsBetweenUsersQuery query);

}
