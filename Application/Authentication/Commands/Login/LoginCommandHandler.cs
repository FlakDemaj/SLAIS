using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Authentication.DTOs;
using Application.Common;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;
using Microsoft.AspNetCore.Identity;
using SAIS.Domain.Users;

namespace Application.Authentication.Handlers;

public class LoginCommandHandler : BaseHandler<LoginCommandHandler>, IRequestHandler<LoginCommand, LoginResponseDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        ISAISLogger<LoginCommandHandler> logger) : base(logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDTO> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository
            .GetUserByUsernameOrEmailAsync(request.Username);

        if (user == null)
        {
            throw new SAISException(AuthErrorCodes.NoUserWithThisName);
        }

        if (user.IsBlocked)
        {
            throw new SAISException(AuthErrorCodes.UserIsBlocked);
        }

        var hasher = new PasswordHasher<UserEntity>();

        var checkPassword = user.VerifyPassword(request.Password, hasher);

        if (!checkPassword)
        {
            user.IncrementWrongLoginAttempts();
            await _userRepository.UpdateAndSaveChangesAsync(user);
            throw new SAISException(
                user.IsBlocked 
                ? AuthErrorCodes.UserIsBlocked
                : AuthErrorCodes.WrongPassword);
        }

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
    }
}