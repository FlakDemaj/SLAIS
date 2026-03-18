using Application.Common.Base;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Application.Interfaces;
using Application.Utils.Exceptions;
using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;

using AutoMapper;

using Microsoft.Extensions.Options;

using SLAIS.Domain.Users;

namespace Application.Authentication.Commands.Login;

public class LoginCommandHandler :
    BaseHandler<LoginCommandHandler>,
    IRequestHandler<LoginCommand, GeneratedTokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        ISlaisLogger<LoginCommandHandler> logger,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IOptions<CommonOptions> commonOptions)
        : base(logger, mapper, commonOptions)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<GeneratedTokenResult> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await CheckUser(request.LoginName);

        await CheckPassword(user, request.Password);

        return await GenerateTokenAsync(
                user,
                request);
    }

    private async Task<UserEntity> CheckUser(
        string loginName)
    {
        var user = await _userRepository
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
        var checkPassword = _passwordHasher.Verify(password, user.HashedPassword);

        if (checkPassword)
        {
            user.SetLoginAttemptsToZero();
            await _userRepository.UpdateAndSaveChangesAsync(user);
            return;
        }

        user.IncrementWrongLoginAttempts(CommonOptions.MaxLoginAttempts);

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
        var refreshToken = await _tokenService.GenerateRefreshToken(
            request,
            user.Guid);

        await _refreshTokenRepository.CreateAsync(refreshToken);

        return new GeneratedTokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.RefreshToken,
            ExpiresIn = refreshToken.GetExpirationInDays()
        };
    }
}
