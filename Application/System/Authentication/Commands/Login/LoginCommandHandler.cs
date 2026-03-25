using Application.Common.Base;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Application.Interfaces;
using Application.Utils.Interfaces.Transaction;
using Application.Utils.Logger;
using Application.Utils.Mediator.Interfaces;

using AutoMapper;

using Domain.Common.Exceptions;

using Microsoft.Extensions.Options;

using SLAIS.Domain.Users;

namespace Application.Authentication.Commands.Login;

public class LoginCommandHandler :
    BaseHandler<LoginCommandHandler>,
    IRequestHandler<LoginCommand, GeneratedTokenResult>,
    INoTransaction
{
    private readonly IUserRepository _userRepository;
    private readonly RefreshTokenOptions _refreshTokenOptions;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    protected readonly CommonOptions _commonOptions;


    public LoginCommandHandler(
        IUserRepository userRepository,
        IOptions<RefreshTokenOptions> refreshTokenOptions,
        ITokenService tokenService,
        ISlaisLogger<LoginCommandHandler> logger,
        IPasswordHasher passwordHasher,
        IOptions<CommonOptions> commonOptions,
        IUnitOfWork unitOfWork)
        : base(logger)
    {
        _userRepository = userRepository;
        _refreshTokenOptions = refreshTokenOptions.Value;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _commonOptions = commonOptions.Value;
    }

    public async Task<GeneratedTokenResult> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await CheckUser(request.LoginName);

        await CheckPassword(user, request.Password, cancellationToken);

        return GenerateToken(
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
            return;
        }

        user.IncrementWrongLoginAttempts(_commonOptions.MaxLoginAttempts);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        throw new SlaisException(AuthErrorCodes.WrongPassword);
    }

    private GeneratedTokenResult GenerateToken(
        UserEntity user,
        LoginCommand request)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);

        var refreshToken = user.CreateRefreshToken(
            _refreshTokenOptions.ExpiresInDays,
            deviceName: request.DeviceName,
            ipAddress: request.IpAddress,
            deviceGuid: request.DeviceGuid);

        return new GeneratedTokenResult
        {
            GeneratedAccessToken = accessToken,
            RefreshToken = new GeneratedRefreshTokenResult
            {
                RefreshToken = refreshToken.Guid,
                RefreshTokenExpiresInDays = _refreshTokenOptions.ExpiresInDays
            }
        };
    }
}
