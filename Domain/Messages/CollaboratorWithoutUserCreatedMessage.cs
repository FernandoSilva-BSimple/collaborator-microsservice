namespace Domain.Messages
{

    public record CollaboratorWithoutUserCreatedMessage(
        Guid CorrelationId,
        string Names,
        string Surnames,
        string Email,
        DateTime FinalDate
    );
}