using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;
namespace Infrastructure.DataModel;

public class CollaboratorTempDataModel : ICollaboratorTempVisitor
{
    public Guid Id { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; }
    public Guid? UserId { get; }

    public CollaboratorTempDataModel(ICollaboratorTemp collaboratorTemp)
    {
        Id = collaboratorTemp.Id;
        PeriodDateTime = collaboratorTemp.PeriodDateTime;
        Names = collaboratorTemp.Names;
        Surnames = collaboratorTemp.Surnames;
        Email = collaboratorTemp.Email;
        FinalDate = collaboratorTemp.FinalDate;
    }

    public CollaboratorTempDataModel()
    {
    }
}
