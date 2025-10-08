namespace gigu_back_end.User.Domain.Services;

public interface IHashService
{
    string HashPassword(string password);
    
    bool VerifyPassword(string password, string passwordHashed); 
    
}