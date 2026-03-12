using Application.Users.DTOs;
using MediatR;

public record GetUserQuery(Guid userGuid) : IRequest<UserDto>;