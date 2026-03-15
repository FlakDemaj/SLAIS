using Application.Users.DTOs;
using Application.Utils.MediatR.Interfaces;

public record GetUserQuery(Guid userGuid) : IRequest<GetUserDto>;