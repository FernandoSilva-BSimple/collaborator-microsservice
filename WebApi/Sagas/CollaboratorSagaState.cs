using MassTransit;

public class CollaboratorSagaState : SagaStateMachineInstance
{
    public string Email { get; set; } = default!;
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;

}
