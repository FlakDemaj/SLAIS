using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Authentication.DTOs;
using Application.Common;
using Application.Interfaces;
using Application.Utils;
using Application.Utils.Logger;
using Application.Utils.MediatR.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SAIS.Domain.Users;

namespace Application.Authentication.Handlers;

public class LoginCommandHandler :
    BaseHandler<LoginCommandHandler>,
    IRequestHandler<LoginCommand, LoginResponseDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        ISAISLogger<LoginCommandHandler> logger,
        IPasswordHasher<UserEntity> passwordHasher,
        IMapper mapper) 
        : base(logger,mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDTO> HandleAsync(
        LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = await CheckUser(request.LoginName);

        await CheckPassword(user, request.Password);
        
        return _mapper.Map<LoginResponseDTO>(
            await GenerateTokenAsync(
                user,
                request));
    }

    private async Task<UserEntity> CheckUser(
        string loginName)
    {
        var user = await _userRepository
            .GetUserByUsernameOrEmailAsync(loginName);

        if (user == null)
        {
            throw new SAISException(AuthErrorCodes.NoUserWithThisName);
        }

        if (user.IsBlocked)
        {
            throw new SAISException(AuthErrorCodes.UserIsBlocked);
        }
        
        return user;
    }

    private async Task CheckPassword(
        UserEntity user,
        string password)
    {
        var checkPassword = user.VerifyPassword(password, _passwordHasher);

        if (checkPassword)
        {
            return;
        }
        
        user.IncrementWrongLoginAttempts();
        
        await _userRepository.UpdateAndSaveChangesAsync(user);
        
        throw new SAISException(
            user.IsBlocked 
                ? AuthErrorCodes.UserIsBlocked
                : AuthErrorCodes.WrongPassword);
    }

    private async Task<(string, (Guid, int))> GenerateTokenAsync(
        UserEntity user,
        LoginCommand request)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = await _tokenService.GenerateRefreshToken(
            request,
            user.Guid);
        
        return (accessToken, refreshToken);
    }
}