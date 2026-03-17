namespace Application.Authentication.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; init; }

    public string RefreshToken { get; init; }

    public int RefreshTokenExpirationDays { get; set; }
}
