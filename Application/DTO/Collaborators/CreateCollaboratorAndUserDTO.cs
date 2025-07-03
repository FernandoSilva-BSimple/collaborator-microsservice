using Domain.Models;

public class CreateCollaboratorAndUserDTO
{
    public string Names { get; set; } = default!;
    public string Surnames { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime FinalDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }
}
