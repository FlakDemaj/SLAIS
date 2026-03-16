using Application.Authentication.Commands;
using Application.Authentication.DTOs;
using AutoMapper;
using Domain.Systems.RefreshToken;

namespace Infrastructure.AutoMappers;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<(LoginCommand, string, int), RefreshTokenEntity>()
            .ForMember(rt => rt.RefreshToken, o
                => o.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(rt => rt.Revoked, o
                => o.MapFrom(src => false))
            .ForMember(rt => rt.CreatedDate, o
                => o.MapFrom(src => DateTime.Now))
            .ForMember(rt => rt.LastUsedDate, o
                => o.MapFrom(src => DateTime.Now))
            .ForMember(rt => rt.UserGuid, o
                => o.MapFrom(src => src.Item2))
            .ForMember(rt => rt.ExpirationDate, o
                => o.MapFrom(src => DateTime.Now.AddDays(src.Item3)));

        CreateMap<(string, (string, int)), LoginResponseDTO>()
            .ForMember(rt => rt.AccessToken, o
                => o.MapFrom(src => src.Item1))
            .ForMember(rt => rt.RefreshToken, o
                => o.MapFrom(src => src.Item2))
            .ForMember(rt => rt.RefreshTokenExpirationDays, o
                => o.MapFrom(src => src.Item2.Item2));
    }
}