using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

public class CollaboratorTempDataModel : ICollaboratorTempVisitor
{
    public Guid Id { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public string Names { get; private set; } = default!;
    public string Surnames { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public DateTime FinalDate { get; private set; }

    public CollaboratorTempDataModel(ICollaboratorTemp collaboratorTemp)
    {
        Id = collaboratorTemp.Id;
        PeriodDateTime = collaboratorTemp.PeriodDateTime;
        Names = collaboratorTemp.Names;
        Surnames = collaboratorTemp.Surnames;
        Email = collaboratorTemp.Email;
        FinalDate = collaboratorTemp.FinalDate;
    }

    public CollaboratorTempDataModel() { }
}
