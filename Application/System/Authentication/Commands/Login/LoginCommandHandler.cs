using Application.Common.Base;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Application.Interfaces;
using Application.Utils.Exceptions;
using Application.Utils.Interfaces.Transaction;
using Application.Utils.Logger;
using Application.Utils.Mediator.Interfaces;

using AutoMapper;

using Infrastructure.Transaction;

using Microsoft.Extensions.Options;

using SLAIS.Domain.Users;

namespace Application.Authentication.Commands.Login;

public class LoginCommandHandler :
    BaseHandler<LoginCommandHandler>,
    IRequestHandler<LoginCommand, GeneratedTokenResult>
{
    private readonly IUserRepository _userRepository;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        ITokenService tokenService,
        ISlaisLogger<LoginCommandHandler> logger,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IOptions<CommonOptions> commonOptions,
        IUnitOfWork unitOfWork)
        : base(logger, mapper, commonOptions)
    {
        _userRepository = userRepository;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<GeneratedTokenResult> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await CheckUser(request.LoginName);

        await CheckPassword(user, request.Password, cancellationToken);

        return await GenerateTokenAsync(
                user,
                request);
    }

    private async Task<UserEntity> CheckUser(
        string loginName)
    {
        var user = await _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(loginName);

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
        string password,
        CancellationToken cancellationToken)
    {
        var checkPassword = _passwordHasher.Verify(password, user.HashedPassword);

        if (checkPassword)
        {
            user.SetLoginAttemptsToZero();
            await _userRepository.SaveChangesAsync(user);
            return;
        }

        user.IncrementWrongLoginAttempts(_commonOptions.MaxLoginAttempts);
        await _userRepository.SaveChangesAsync(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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

        var refreshToken = user.CreateRefreshToken(
            _refreshTokenOptions.ExpiresInDays,
            request.DeviceGuid,
            request.DeviceName,
            request.IpAddress);

        await _userRepository.SaveChangesAsync(user);

        return new GeneratedTokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.RefreshToken,
            ExpiresIn = refreshToken.GetExpirationInDays()
        };
    }
}
