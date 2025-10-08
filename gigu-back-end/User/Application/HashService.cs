using gigu_back_end.User.Domain.Services;

namespace gigu_back_end.User.Application;

public class HashService : IHashService  
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHashed)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHashed);
    }
    
}