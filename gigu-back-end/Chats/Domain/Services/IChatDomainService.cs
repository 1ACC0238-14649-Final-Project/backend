using Chats.Domain.Models.Commands;
using Chats.Domain.Models.Entities;

namespace Chats.Domain.Services;

public interface IChatDomainService
{
    Task<Chat> Handle(CreateChatCommand command);
}