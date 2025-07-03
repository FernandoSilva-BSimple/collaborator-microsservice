using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public interface ICollaboratorTempFactory
{
    CollaboratorTemp Create(Guid id, string names, string surnames, string email, DateTime finalDate, PeriodDateTime periodDateTime);
    public CollaboratorTemp Create(ICollaboratorTempVisitor visitor);

}
