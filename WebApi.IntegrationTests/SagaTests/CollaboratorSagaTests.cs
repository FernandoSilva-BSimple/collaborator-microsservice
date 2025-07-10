/*using Application.Interfaces;
using Domain.Commands;
using Domain.Factory;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using System.Linq;

public class CollaboratorSagaTests
{
    [Fact]
    public async Task Should_CompleteSaga_When_UserCreated_IsReceived()
    {
        // Arrange
        var collaboratorTempServiceMock = new Mock<ICollaboratorTempService>();
        var collaboratorFactoryMock = new Mock<ICollaboratorFactory>();
        var collaboratorServiceMock = new Mock<ICollaboratorService>();

        var userId = Guid.NewGuid(); // Este será usado na mensagem UserCreated e no Collaborator
        var expectedPeriod = new PeriodDateTime(DateTime.Today, DateTime.Today.AddMonths(1));

        var fakeTemp = new CollaboratorTemp(
            "Test", "User", "test@example.com", DateTime.Today, expectedPeriod
        );

        // Mock do Collaborator criado pela factory com um Id previsível
        var collaboratorCreated = new Collaborator(Guid.NewGuid(), userId, expectedPeriod);

        collaboratorTempServiceMock
            .Setup(x => x.CreateCollaboratorTempAsync(It.IsAny<CreateRequestedCollaboratorCommand>()))
            .Returns(Task.CompletedTask);

        collaboratorTempServiceMock
            .Setup(x => x.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(fakeTemp);

        collaboratorTempServiceMock
            .Setup(x => x.DeleteCollaboratorTempAsync(fakeTemp.Id))
            .Returns(Task.CompletedTask);

        collaboratorFactoryMock
            .Setup(x => x.ConvertFromTemp(fakeTemp, userId))
            .Returns(collaboratorCreated);

        collaboratorServiceMock
            .Setup(x => x.AddCollaboratorAsync(collaboratorCreated))
            .ReturnsAsync(collaboratorCreated);

        await using var provider = new ServiceCollection()
            .AddSingleton(collaboratorTempServiceMock.Object)
            .AddSingleton(collaboratorFactoryMock.Object)
            .AddSingleton(collaboratorServiceMock.Object)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSagaStateMachine<CollaboratorSaga, CollaboratorSagaState>()
                   .InMemoryRepository();
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        try
        {
            // Act 1 – envia o comando que inicia a saga
            var period = new PeriodDateTime(DateTime.Today, DateTime.Today.AddMonths(1));

            await harness.Bus.Publish(new CreateRequestedCollaboratorCommand(
                "Test",
                "User",
                "test@example.com",
                DateTime.Today,
                period));

            var sagaHarness = harness.GetSagaStateMachineHarness<CollaboratorSaga, CollaboratorSagaState>();

            // confirma que alguma saga foi criada
            Assert.True(await sagaHarness.Created.Any());

            // 1) obtém todas as instâncias criadas
            var allCreated = sagaHarness.Created.Select().ToList();  // agora tens IList<ISagaInstance<CollaboratorSagaState>>

            // 2) encontra a que tem o email desejado
            var instance = allCreated.FirstOrDefault(s => s.Saga.Email == "test@example.com");
            Assert.NotNull(instance);

            // 3) verifica se essa instância está no estado WaitingForUserCreation
            var isInState = await sagaHarness.Created.ContainsInState(
                instance.Saga.CorrelationId,
                sagaHarness.StateMachine,
                sagaHarness.StateMachine.WaitingForUserCreation);

            Assert.True(isInState);        // a saga existe e está no estado certo


            // Act 2: Simula a criação de utilizador
            await harness.Bus.Publish(new UserCreatedMessage
            {
                Id = userId,
                Email = "test@example.com"
            });

            // Assert: Mensagem consumida
            Assert.True(await sagaHarness.Consumed.Any<UserCreatedMessage>());
            Assert.True(await harness.Published.Any<CollaboratorCreatedMessage>());

            var published = harness.Published.Select<CollaboratorCreatedMessage>().FirstOrDefault();
            Assert.NotNull(published);
            Assert.Equal(userId, published.Context.Message.UserId);

            // Verifica que a saga completou
            var final = await sagaHarness.Created.ContainsInState(
                instance.CorrelationId,
                sagaHarness.StateMachine,
                sagaHarness.StateMachine.Completed
            );
            Assert.NotNull(final);

            // Verificações de chamadas
            collaboratorTempServiceMock.Verify(x => x.CreateCollaboratorTempAsync(It.IsAny<CreateRequestedCollaboratorCommand>()), Times.Once);
            collaboratorTempServiceMock.Verify(x => x.GetByEmailAsync("test@example.com"), Times.Once);
            collaboratorTempServiceMock.Verify(x => x.DeleteCollaboratorTempAsync(fakeTemp.Id), Times.Once);
            collaboratorFactoryMock.Verify(x => x.ConvertFromTemp(fakeTemp, userId), Times.Once);
            collaboratorServiceMock.Verify(x => x.AddCollaboratorAsync(collaboratorCreated), Times.Once);
        }
        finally
        {
            await harness.Stop();
        }
    }
}
*/