using Application.Authentication.Commands;

using AutoMapper;

using Domain.Systems.RefreshToken;

namespace Application.Utils.AutoMapper;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<(LoginCommand, Guid, int), RefreshTokenEntity>()
            .ForMember(rt => rt.RefreshToken, o
                =>
            {
                o.MapFrom(src => Guid.CreateVersion7());
            })
            .ForMember(rt => rt.Revoked, o
                =>
            {
                o.MapFrom(src => false);
            })
            .ForMember(rt => rt.CreatedDate, o
                =>
            {
                o.MapFrom(src => DateTime.UtcNow);
            })
            .ForMember(rt => rt.LastUsedDate, o
                =>
            {
                o.MapFrom(src => DateTime.UtcNow);
            })
            .ForMember(rt => rt.UserGuid, o
                =>
            {
                o.MapFrom(src => src.Item2);
            })
            .ForMember(rt => rt.ExpirationDate, o
                =>
            {
                o.MapFrom(src => DateTime.UtcNow.AddDays(src.Item3));
            })
            .ForMember(rt => rt.IpAddress, o
                =>
            {
                o.MapFrom(src => src.Item1.IpAddress);
            })
            .ForMember(rt => rt.DeviceGuid, o
                =>
            {
                o.MapFrom(src => src.Item1.DeviceGuid);
            })
            .ForMember(rt => rt.DeviceName, o
                =>
            {
                o.MapFrom(src => src.Item1.DeviceName);
            });
    }
}
