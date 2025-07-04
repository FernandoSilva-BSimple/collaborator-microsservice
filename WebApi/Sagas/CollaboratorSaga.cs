using Application.Interfaces;
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

    private readonly ICollaboratorFactory _collaboratorFactory;
    private readonly ICollaboratorTempService _collaboratorTempService;
    private readonly ICollaboratorService _collaboratorService;

    public CollaboratorSaga(
        ICollaboratorFactory collaboratorFactory,
        ICollaboratorTempService collaboratorTempService,
        ICollaboratorService collaboratorService
    )
    {
        _collaboratorFactory = collaboratorFactory;
        _collaboratorTempService = collaboratorTempService;
        _collaboratorService = collaboratorService;

        InstanceState(x => x.CurrentState);

        Event(() => CreateCollaboratorRequested, x => x.CorrelateBy((saga, context) => saga.Email == context.Message.Email));
        Event(() => UserCreated, x => x.CorrelateById(context => context.Message.Id));

        Initially(
            When(CreateCollaboratorRequested)
                .ThenAsync(async ctx =>
                {
                    await _collaboratorTempService.CreateCollaboratorTempAsync(ctx.Message);
                })
                .Send(ctx => new CollaboratorWithoutUserCreatedMessage(
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
                    var temp = await _collaboratorTempService.GetByEmailAsync(ctx.Message.Email);

                    if (temp is null) throw new InvalidOperationException("CollaboratorTemp not found.");

                    var collaborator = _collaboratorFactory.ConvertFromTemp(temp, ctx.Message.Id);

                    await _collaboratorService.AddCollaboratorAsync(collaborator);

                    await _collaboratorTempService.DeleteCollaboratorTempAsync(temp);

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
