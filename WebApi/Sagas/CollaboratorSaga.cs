using Domain.Factory;
using Domain.IRepository;
using Domain.Messages;
using Domain.Models;
using MassTransit;

public class CollaboratorSaga : MassTransitStateMachine<CollaboratorSagaState>
{
    public State WaitingForUserCreation { get; private set; }
    public State Completed { get; private set; }

    public Event<CreateCollaboratorRequested> CreateCollaboratorRequested { get; private set; } = default!;
    public Event<UserCreatedMessage> UserCreated { get; private set; } = default!;

    private readonly ICollaboratorTempFactory _collaboratorTempFactory;
    private readonly ICollaboratorFactory _collaboratorFactory;
    private readonly ICollaboratorTempRepository _collaboratorTempRepository;
    private readonly ICollaboratorRepository _collaboratorRepository;

    public CollaboratorSaga(
        ICollaboratorTempFactory collaboratorTempFactory,
        ICollaboratorFactory collaboratorFactory,
        ICollaboratorTempRepository collaboratorTempRepository,
        ICollaboratorRepository collaboratorRepository
    )
    {
        _collaboratorTempFactory = collaboratorTempFactory;
        _collaboratorFactory = collaboratorFactory;
        _collaboratorTempRepository = collaboratorTempRepository;
        _collaboratorRepository = collaboratorRepository;

        InstanceState(x => x.CurrentState);

        Event(() => CreateCollaboratorRequested, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => UserCreated, x => x.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(CreateCollaboratorRequested)
                .ThenAsync(async ctx =>
                {
                    var temp = _collaboratorTempFactory.Create(
                        ctx.Message.CorrelationId,
                        ctx.Message.Names,
                        ctx.Message.Surnames,
                        ctx.Message.Email,
                        ctx.Message.FinalDate,
                        ctx.Message.PeriodDateTime
                    );

                    await _collaboratorTempRepository.AddAsync(temp);
                    await _collaboratorTempRepository.SaveChangesAsync();
                })
                .Publish(ctx => new CollaboratorWithoutUserCreatedMessage(
                    ctx.Message.CorrelationId,
                    ctx.Message.Names,
                    ctx.Message.Surnames,
                    ctx.Message.Email,
                    ctx.Message.FinalDate
                ))
                .TransitionTo(WaitingForUserCreation)
        );

        During(WaitingForUserCreation,
            When(UserCreated)
                .ThenAsync(async ctx =>
                {
                    // Atualiza o CollaboratorTemp com o UserId
                    var temp = await _collaboratorTempRepository.GetByIdAsync(ctx.Saga.CorrelationId);
                    if (temp is null)
                        throw new InvalidOperationException("CollaboratorTemp not found.");

                    temp.UserId = ctx.Message.Id;
                    await _collaboratorTempRepository.SaveChangesAsync();

                    // Cria o Collaborator real
                    var collaborator = _collaboratorFactory.ConvertFromTemp(temp);

                    await _collaboratorRepository.AddAsync(collaborator);
                    await _collaboratorRepository.SaveChangesAsync();

                    // Elimina o temp
                    await _collaboratorTempRepository.RemoveAsync(temp);
                    await _collaboratorTempRepository.SaveChangesAsync();

                    // Publica evento final
                    await ctx.Publish(new CollaboratorCreatedMessage(
                        collaborator.Id,
                        collaborator.UserId,
                        collaborator.PeriodDateTime
                    ));
                })
                .TransitionTo(Completed)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
}
