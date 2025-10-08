using gigu_back_end.User.Domain.Models.Commands;
using gigu_back_end.User.Domain.Models.Entities;

namespace gigu_back_end.User.Domain.Services;

public interface IUserQueryService
{
    Task<IEnumerable<Models.Entities.User>> Handle(GetAllUsersQuery query);
    Task<Models.Entities.User> Handle(GetUserByIdQuery query);
    Task<Models.Entities.User> Handle(GetCurrentUserQuery query);
}