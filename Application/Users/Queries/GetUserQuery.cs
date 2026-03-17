using Application.Users.DTOs;
using Application.Utils.Interfaces.MediatR;

public record GetUserQuery(Guid UserGuid) : IRequest<GetUserDto>;
