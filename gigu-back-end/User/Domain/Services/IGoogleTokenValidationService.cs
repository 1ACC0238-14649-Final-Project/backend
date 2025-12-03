namespace gigu_back_end.User.Domain.Services;

public interface IGoogleTokenValidationService
{
    Task<GoogleTokenValidationResult> ValidateTokenAsync(string idToken, string? clientId = null);
}

public class GoogleTokenValidationResult
{
    public bool IsValid { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? ErrorMessage { get; set; }
}

