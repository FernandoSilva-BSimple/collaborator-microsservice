
using Domain.Models;

namespace Domain.Messages
{

    public record CreateCollaboratorRequested(
        string Names,
        string Surnames,
        string Email,
        DateTime FinalDate,
        PeriodDateTime PeriodDateTime
    );

}