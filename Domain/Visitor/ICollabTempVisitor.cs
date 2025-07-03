using Domain.Models;

namespace Domain.Visitor;

public interface ICollaboratorTempVisitor
{
    Guid Id { get; }
    string Names { get; }
    string Surnames { get; }
    string Email { get; }
    DateTime FinalDate { get; }
    PeriodDateTime PeriodDateTime { get; }
    Guid? UserId { get; }
}