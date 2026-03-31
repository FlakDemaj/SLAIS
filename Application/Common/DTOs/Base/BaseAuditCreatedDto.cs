namespace Application.Common.DTOs.Base;

public class BaseAuditCreatedDto
{
    public string CreatedByFirstname { get; set; }


    public string CreatedByLastname { get; set; }

    public DateTime CreationDate { get; set; }
}
