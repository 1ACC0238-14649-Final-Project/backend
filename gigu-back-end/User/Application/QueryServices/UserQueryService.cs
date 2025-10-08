using gigu_back_end.Shared.Domain;
using gigu_back_end.User.Domain.Services;
using gigu_back_end.User.Domain;
using gigu_back_end.User.Domain.Models.Commands;
using gigu_back_end.User.Domain.Models.Entities;

namespace gigu_back_end.User.Application.QueryServices
{
    public class UserQueryService(IUserRepository userRepository) : IUserQueryService
    {
        public async Task<IEnumerable<Domain.Models.Entities.User>> Handle(GetAllUsersQuery query)
        {
            var users = await userRepository.ListAsync();
            return users?.Where(user => user.IsActive) ?? Enumerable.Empty<Domain.Models.Entities.User>();
        }

        public async Task<Domain.Models.Entities.User> Handle(GetUserByIdQuery query)
        {
            var user = await userRepository.FindByIdAsync(query.UserId);
            return user?.IsActive == true ? user : null;
        }

        public async Task<Domain.Models.Entities.User> Handle(GetUserByEmailQuery query)
        {
            var user = await userRepository.GetByEmailAsync(query.UserEmail);
            return user?.IsActive == true ? user : null;
        }
        
        public async Task<Domain.Models.Entities.User> Handle(GetCurrentUserQuery query)
        {
            var user = await userRepository.FindByIdAsync(query.UserId);
            return user?.IsActive == true ? user : null;
        }
    }

}