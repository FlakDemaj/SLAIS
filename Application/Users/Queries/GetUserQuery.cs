using Application.Users.DTOs;
using Application.Utils.MediatR.Interfaces;

public record GetUserQuery(Guid UserGuid) : IRequest<GetUserDto>;
