using Application.Authentication.DTOs;
using Application.Utils.MediatR.Interfaces;

namespace Application.Authentication.Commands;

public class LoginCommand : IRequest<LoginResponseDTO>
{
    public string Username { get; set; }
    public string Password { get; set; }
}