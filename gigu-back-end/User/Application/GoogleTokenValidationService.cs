using Google.Apis.Auth;
using gigu_back_end.User.Domain.Services;

namespace gigu_back_end.User.Application;

public class GoogleTokenValidationService : IGoogleTokenValidationService
{
    private readonly IConfiguration _configuration;

    public GoogleTokenValidationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GoogleTokenValidationResult> ValidateTokenAsync(string idToken, string? clientId = null)
    {
        try
        {
            // Obtener el Web Client ID de la configuración o usar el proporcionado
            var webClientId = clientId ?? _configuration["Google:WebClientId"];
            
            if (string.IsNullOrWhiteSpace(webClientId))
            {
                // Si no hay configuración, intentar validar sin client ID específico
                // Esto funciona pero es menos seguro
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    // Sin client ID específico, valida contra cualquier cliente de Google
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                
                return new GoogleTokenValidationResult
                {
                    IsValid = true,
                    Email = payload.Email,
                    Name = payload.Name,
                    ImageUrl = payload.Picture
                };
            }
            else
            {
                // Validar con el Web Client ID específico (más seguro)
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { webClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                
                return new GoogleTokenValidationResult
                {
                    IsValid = true,
                    Email = payload.Email,
                    Name = payload.Name,
                    ImageUrl = payload.Picture
                };
            }
        }
        catch (InvalidJwtException ex)
        {
            return new GoogleTokenValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Invalid Google token: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new GoogleTokenValidationResult
            {
                IsValid = false,
                ErrorMessage = $"Error validating Google token: {ex.Message}"
            };
        }
    }
}

