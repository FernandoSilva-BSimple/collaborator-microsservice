
using Domain.Models;

namespace Domain.Messages
{

    public record CreateRequestedCollaboratorCommand(
        string Names,
        string Surnames,
        string Email,
        DateTime FinalDate,
        PeriodDateTime PeriodDateTime
    );

}