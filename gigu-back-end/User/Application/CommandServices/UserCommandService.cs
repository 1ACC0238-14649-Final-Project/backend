using System.Data;
using FluentValidation;
using gigu_back_end.Shared.Domain;
using gigu_back_end.Shared.Domain.Models.Commands;
using gigu_back_end.User.Domain.Services;
using gigu_back_end.User.Domain;
using gigu_back_end.User.Domain.Models.Commands;
using gigu_back_end.User.Domain.Models.Validadors;
using gigu_back_end.User.Domain.Models.Entities;
using gigu_back_end.User.Domain.Models.Exceptions;
using NuGet.Packaging.Licenses;

namespace gigu_back_end.User.Application.CommandServices
{
    public class UserCommandService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateUserCommand> validator, 
        IHashService hashService,
        IJwtEncryptService jwtEncryptService) : IUserCommandService
    {
        public async Task<Domain.Models.Entities.User> Handle(CreateUserCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid)
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var existingUser = await userRepository.GetByEmailAsync(command.Email);
            if (existingUser != null)
                throw new DuplicateNameException($"A user with the email '{command.Email}' already exists.");

            var user = new Domain.Models.Entities.User(command.Name, command.Lastname, command.Email, command.Password, command.Role, command.Image);
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
            return user;
        }

        public async Task<bool> Handle(DeleteUserCommand command)
        {
            var user = await userRepository.FindByIdAsync(command.Id);
            if (user is null) return false;
            user.IsActive = false;
            user.ModifiedDate = DateTime.UtcNow;
            user.UpdatedUserId = 87;
            userRepository.Update(user);
            await unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> Handle(UpdateUserCommand command, int id)
        {
            var user = await userRepository.FindByIdAsync(id);
            if (user is null) throw new DataException("User not found.");

            user.Name = command.Name;
            user.Lastname = command.Lastname;
            user.Email = command.Email;
    
            user.ModifiedDate = DateTime.UtcNow;
            user.UpdatedUserId = 87;

            userRepository.Update(user);
            await unitOfWork.CompleteAsync();
            return true;
        }

        
        public async Task<Domain.Models.Entities.User> Handle(SignUpCommand command)
        {
            var existingUser = await userRepository.GetByEmailAsync(command.Email);
            if (existingUser != null)
                throw new EmailAlreadyTakenException();

            var user = new Domain.Models.Entities.User
            {
                Email = command.Email,
                Password = hashService.HashPassword(command.Password),
                Role = command.Role, 
                Name = command.Name,
                Lastname = command.Lastname,
                Image = command.Image,
                IsActive = true
            };

            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();

            return user;
        }
        
        public async Task<string> Handle(LoginCommand command)
        {
            var user = await userRepository.GetByEmailAsync(command.Email);
            if (user == null || !hashService.VerifyPassword(command.Password, user.Password))
                throw new InvalidCredentialsException();

            var jwtToken = jwtEncryptService.Encrypt(user);


            return jwtToken;
        }
    }

}
