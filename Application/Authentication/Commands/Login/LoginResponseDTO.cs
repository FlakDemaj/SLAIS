namespace Application.Authentication.DTOs;

public class LoginResponseDTO
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public int RefreshTokenExpirationDays { get; set; }
}