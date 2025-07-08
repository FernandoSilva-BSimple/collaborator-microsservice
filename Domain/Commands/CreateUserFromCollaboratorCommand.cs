namespace Domain.Commands;

public record CreateUserFromCollaboratorCommand(string InstanceId, string Names, string Surnames, string Email, DateTime DeactivationDate);