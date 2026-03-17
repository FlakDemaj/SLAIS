using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Common;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;

using AutoMapper;

using SLAIS.Domain.Users;

namespace Application.Authentication.Handlers;

public class LoginCommandHandler :
    BaseHandler<LoginCommandHandler>,
    IRequestHandler<LoginCommand, GeneratedTokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        ISlaisLogger<LoginCommandHandler> logger,
        IPasswordHasher passwordHasher,
        IMapper mapper)
        : base(logger, mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<GeneratedTokenResult> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        UserEntity user = await CheckUser(request.LoginName);

        await CheckPassword(user, request.Password);

        return await GenerateTokenAsync(
                user,
                request);
    }

    private async Task<UserEntity> CheckUser(
        string loginName)
    {
        UserEntity? user = await _userRepository
            .GetUserByUsernameOrEmailAsync(loginName);

        if (user == null)
        {
            throw new SlaisException(AuthErrorCodes.NoUserWithThisName);
        }

        if (user.IsBlocked)
        {
            throw new SlaisException(AuthErrorCodes.UserIsBlocked);
        }

        return user;
    }

    private async Task CheckPassword(
        UserEntity user,
        string password)
    {
        var checkPassword = _passwordHasher.Verify(password, user.PasswordHashed);

        if (checkPassword)
        {
            user.SetLoginAttemptsToZero();
            await _userRepository.UpdateAndSaveChangesAsync(user);
            return;
        }

        user.IncrementWrongLoginAttempts();

        await _userRepository.UpdateAndSaveChangesAsync(user);

        throw new SlaisException(
            user.IsBlocked
                ? AuthErrorCodes.UserIsBlocked
                : AuthErrorCodes.WrongPassword);
    }

    private async Task<GeneratedTokenResult> GenerateTokenAsync(
        UserEntity user,
        LoginCommand request)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        GeneratedRefreshTokenResult refreshToken = await _tokenService.GenerateRefreshToken(
            request,
            user.Guid);

        return new GeneratedTokenResult
        {
            AccessToken = accessToken,
            RefreshTokenResult = refreshToken
        };
    }
}
