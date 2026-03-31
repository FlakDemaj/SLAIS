using Application.Common.DTOs.Base;
using Application.Public.Users;

using AutoMapper;

using SLAIS.Domain.Users;

namespace Application.Common.Mappers.Public;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserEntity, BaseAuditCreatedDto>()
            .ForMember(dto =>
                    dto.CreatedByFirstname,
                    opt =>
                    {
                        opt.MapFrom(src =>
                                                src.CreatedByUser!.FirstName);
                    })
            .ForMember(dto =>
                        dto.CreatedByLastname,
                    opt =>
                    {
                        opt.MapFrom(src =>
                                                src.CreatedByUser!.LastName);
                    })
            .ForMember(dto =>
                        dto.CreationDate,
                    opt =>
                    {
                        opt.MapFrom(src =>
                                                src.CreatedDate);
                    });

        CreateMap<UserEntity, BaseAuditUpdatedDto>()
            .ForMember(dto =>
                        dto.UpdatedByFirstname,
                    opt =>
                    {
                        opt.MapFrom(src =>
                                                src.UpdatedByUser != null ? src.UpdatedByUser.FirstName : null);
                    })
            .ForMember(dto =>
                        dto.UpdatedByLastname,
                    opt =>
                    {
                        opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.LastName : null);
                    })
            .ForMember(dto =>
                        dto.UpdatedDate,
                    opt =>
                    {
                        opt.MapFrom(src => src.UpdateDate);
                    });

        CreateMap<UserEntity, BaseAuditDeletedDto>()
            .ForMember(dto =>
                        dto.DeletedByFirstname,
                    opt =>
                    {
                        opt.MapFrom(src => src.DeletedByUser != null ? src.DeletedByUser.FirstName : null);
                    })
            .ForMember(dto =>
                        dto.DeletedByLastname,
                    opt =>
                    {
                        opt.MapFrom(src => src.DeletedByUser != null ? src.DeletedByUser.LastName : null);
                    })
            .ForMember(dto =>
                        dto.DeletedDate,
                    opt =>
                    {
                        opt.MapFrom(src => src.DeleteDate);
                    });

        CreateMap<UserEntity, GetUserResponseDto>()
            .ForMember(dto =>
                        dto.CreatedBy,
                    opt =>
                    {
                        opt.MapFrom(src => src);
                    })
            .ForMember(dto =>
                        dto.UpdatedBy,
                    opt =>
                    {
                        opt.MapFrom(src => src.UpdateDate == null ? null : src);
                    })
            .ForMember(dto =>
                        dto.DeletedBy,
                    opt =>
                    {
                        opt.MapFrom(src => src.DeleteDate == null ? null : src);
                    });
    }
}
