using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public class CollaboratorTempFactory : ICollaboratorTempFactory
{
    public CollaboratorTemp Create(Guid id, string names, string surnames, string email, DateTime finalDate, PeriodDateTime periodDateTime)
    {
        return new CollaboratorTemp(
            id,
            names,
            surnames,
            email,
            finalDate,
            periodDateTime,
            userId: null
        );
    }

    public CollaboratorTemp Create(ICollaboratorTempVisitor visitor)
    {
        return new CollaboratorTemp(visitor.Id, visitor.Names, visitor.Surnames, visitor.Email, visitor.FinalDate, visitor.PeriodDateTime, visitor.UserId);
    }
}
