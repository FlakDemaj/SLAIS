namespace Application.Common.DTOs.Base;

public class BaseAuditDeletedDto
{
    public string? DeletedByFirstname { get; set; }

    public string? DeletedByLastname { get; set; }

    public DateTime DeletedDate { get; set; }
}
