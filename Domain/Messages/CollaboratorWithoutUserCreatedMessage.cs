namespace Domain.Messages
{

    public record CollaboratorWithoutUserCreatedMessage(
        string Names,
        string Surnames,
        string Email,
        DateTime FinalDate
    );
}