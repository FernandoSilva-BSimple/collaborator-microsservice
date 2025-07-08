using Domain.Models;

namespace Domain.Messages;

public record UserCreatedSagaMessage(Guid Id, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);