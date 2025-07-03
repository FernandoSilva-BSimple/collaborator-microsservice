
using Domain.Models;

namespace Domain.Messages
{

    public record CreateCollaboratorRequested(
        Guid CorrelationId,
        string Names,
        string Surnames,
        string Email,
        DateTime FinalDate,
        PeriodDateTime PeriodDateTime
    );

}