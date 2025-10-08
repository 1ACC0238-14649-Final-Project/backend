namespace gigu_back_end.User.Domain.Services;

public interface IJwtEncryptService
{
    string Encrypt(Models.Entities.User user);
    
    Models.Entities.User Decrypt(string encrypted); 
}