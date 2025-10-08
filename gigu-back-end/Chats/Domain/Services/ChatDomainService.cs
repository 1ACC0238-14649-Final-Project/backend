using FluentValidation;
using Chats.Domain.Models.Commands;
using Chats.Domain.Models.Entities;
using gigu_back_end.Shared.Domain;
using gigu_back_end.User.Domain;

namespace Chats.Domain.Services;

public class ChatDomainService(
    IChatRepository chatRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateChatCommand> validator
) : IChatDomainService
{
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<CreateChatCommand> _validator = validator;

    public async Task<Chat> Handle(CreateChatCommand command)
    {
        var result = await _validator.ValidateAsync(command);
        if (!result.IsValid)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));

        var sender = await _userRepository.FindByIdAsync(command.SenderId);
        var receiver = await _userRepository.FindByIdAsync(command.ReceiverId);

        if (sender is null || receiver is null)
            throw new KeyNotFoundException("Sender or Receiver not found.");

        var message = new Chat(command.SenderId, command.ReceiverId, command.Content);
        await _chatRepository.AddAsync(message);
        await _unitOfWork.CompleteAsync();
        return message;
    }
}
