using Application.Common.DTOs.Base;

using Domain.Common.Enums;

namespace Application.Public.Users;

public class GetUserResponseDto : BaseIdDto
{
    public required string Username { get; set; }

    public required string Email { get; set; }

    public required Roles Role { get; set; }

    public required BaseAuditCreatedDto CreatedBy { get; set; }

    public BaseAuditUpdatedDto? UpdatedBy { get; set; }

    public BaseAuditDeletedDto? DeletedBy { get; set; }

}
