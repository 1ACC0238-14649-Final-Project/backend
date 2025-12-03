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
        IJwtEncryptService jwtEncryptService,
        IGoogleTokenValidationService googleTokenValidationService) : IUserCommandService
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

        public async Task<string> Handle(GoogleLoginCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.IdToken))
                throw new InvalidCredentialsException("Google ID token is required");

            // Validar el token de Google
            var validationResult = await googleTokenValidationService.ValidateTokenAsync(command.IdToken);
            
            if (!validationResult.IsValid)
                throw new InvalidCredentialsException(validationResult.ErrorMessage ?? "Invalid Google token");

            // Usar email del token validado (más seguro) o del comando como fallback
            var email = validationResult.Email ?? command.Email;
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidCredentialsException("Email is required for Google login");

            // Usar información del token validado o del comando como fallback
            var name = validationResult.Name ?? command.Name;
            var image = validationResult.ImageUrl ?? command.Image;

            // Buscar usuario existente por email
            var user = await userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                // Usuario no existe, crear uno nuevo
                // Separar name en Name y Lastname si es necesario
                var nameParts = name?.Split(' ', 2) ?? new[] { "", "" };
                var firstName = nameParts[0] ?? "";
                var lastName = nameParts.Length > 1 ? nameParts[1] : "";

                // Crear usuario sin password (autenticado por Google)
                // Generar una contraseña aleatoria que nunca se usará
                var randomPassword = Guid.NewGuid().ToString();
                
                user = new Domain.Models.Entities.User
                {
                    Email = email,
                    Password = hashService.HashPassword(randomPassword), // Password dummy, nunca se usará
                    Role = "buyer", // Rol por defecto para usuarios de Google
                    Name = firstName,
                    Lastname = lastName,
                    Image = image ?? "",
                    IsActive = true
                };

                await userRepository.AddAsync(user);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                // Usuario existe, actualizar información si es necesario
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var nameParts = name.Split(' ', 2);
                    user.Name = nameParts[0] ?? user.Name;
                    if (nameParts.Length > 1)
                        user.Lastname = nameParts[1];
                }
                if (!string.IsNullOrWhiteSpace(image))
                    user.Image = image;

                userRepository.Update(user);
                await unitOfWork.CompleteAsync();
            }

            // Generar y retornar JWT token
            var jwtToken = jwtEncryptService.Encrypt(user);
            return jwtToken;
        }
    }

}
