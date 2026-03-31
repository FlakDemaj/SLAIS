namespace Application.Common.DTOs.Base;

public class BaseAuditUpdatedDto
{
    public string? UpdatedByFirstname { get; set; }

    public string? UpdatedByLastname { get; set; }

    public DateTime UpdatedDate { get; set; }
}
