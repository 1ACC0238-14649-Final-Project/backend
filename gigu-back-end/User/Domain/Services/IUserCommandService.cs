using gigu_back_end.Shared.Domain.Models.Commands;
using gigu_back_end.User.Domain.Models.Commands;
using gigu_back_end.User.Domain.Models.Entities;

namespace gigu_back_end.User.Domain.Services;

public interface IUserCommandService
{
    Task<Models.Entities.User> Handle(CreateUserCommand command);
    Task<bool> Handle(DeleteUserCommand command);
    Task<bool> Handle(UpdateUserCommand command, int id); 
    Task<Models.Entities.User> Handle(SignUpCommand command);
    Task<string> Handle(LoginCommand loginCommand);
}
