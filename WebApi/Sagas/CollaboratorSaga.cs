using Application.Interfaces;
using Domain.Commands;
using Domain.Factory;
using Domain.IRepository;
using Domain.Messages;
using Domain.Models;
using Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CollaboratorSaga : MassTransitStateMachine<CollaboratorSagaState>
{
    public State WaitingForUserCreation { get; private set; }
    public State Completed { get; private set; }

    public Event<CreateRequestedCollaboratorCommand> CreateCollaboratorRequested { get; private set; } = default!;
    public Event<UserCreatedMessage> UserCreated { get; private set; } = default!;

    public CollaboratorSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => CreateCollaboratorRequested, x =>
        {
            x.CorrelateBy((saga, context) => saga.Email == context.Message.Email);
            x.SelectId(context => NewId.NextGuid());
        });
        Event(() => UserCreated, x =>
        {
            x.CorrelateBy((saga, context) => saga.Email == context.Message.Email);
        });
        Initially(
            When(CreateCollaboratorRequested)
                .ThenAsync(async ctx =>
                {

                    Console.WriteLine("CreateCollaboratorRequested was CALLED");

                    var provider = ctx.GetPayload<IServiceProvider>();
                    using var scope = provider.CreateScope();

                    var collaboratorTempService = scope.ServiceProvider.GetRequiredService<ICollaboratorTempService>();

                    await collaboratorTempService.CreateCollaboratorTempAsync(ctx.Message);
                }).Then(ctx =>
                {
                    ctx.Saga.Email = ctx.Message.Email;
                })
                .Send(new Uri("queue:users-cmd-saga"), ctx => new CreateUserFromCollaboratorCommand(
                    InstanceInfo.InstanceId,
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

                    Console.WriteLine("UserCreaTED INSIDE SAGA was CALLED");

                    var provider = ctx.GetPayload<IServiceProvider>();
                    using var scope = provider.CreateScope();

                    var collaboratorTempService = scope.ServiceProvider.GetRequiredService<ICollaboratorTempService>();
                    var collaboratorFactory = scope.ServiceProvider.GetRequiredService<ICollaboratorFactory>();
                    var collaboratorService = scope.ServiceProvider.GetRequiredService<ICollaboratorService>();

                    var temp = await collaboratorTempService.GetByEmailAsync(ctx.Message.Email);
                    if (temp is null)
                        throw new InvalidOperationException("CollaboratorTemp not found.");

                    var collaborator = collaboratorFactory.ConvertFromTemp(temp, ctx.Message.Id);

                    await collaboratorService.AddCollaboratorAsync(collaborator);
                    await collaboratorTempService.DeleteCollaboratorTempAsync(temp.Id);

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
