using Domain.Commands;
using Domain.Messages;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Publishers;
using Xunit;

namespace WebApi.IntegrationTests.PublisherTests;

public class SendCreateCollaboratorSagaCommandTests
{
    [Fact]
    public async Task SendCreateCollaboratorSagaCommandAsync_ShouldSendToCorrectQueueWithMessage()
    {
        // Arrange
        var sendEndpointMock = new Mock<ISendEndpoint>();
        var endpointProvider = new Mock<ISendEndpointProvider>();
        var publishEndpoint = new Mock<IPublishEndpoint>();

        var expectedUri = new Uri($"queue:collaborators-saga-{InstanceInfo.InstanceId}");

        endpointProvider
            .Setup(p => p.GetSendEndpoint(expectedUri))
            .ReturnsAsync(sendEndpointMock.Object);

        var publisher = new MassTransitPublisher(publishEndpoint.Object, endpointProvider.Object);

        var cmd = new CreateRequestedCollaboratorCommand(
            "Test", "User", "test@example.com", DateTime.Today,
            new PeriodDateTime(DateTime.Today, DateTime.Today.AddMonths(1)));

        // Act
        await publisher.SendCreateCollaboratorSagaCommandAsync(cmd);

        // Assert
        endpointProvider.Verify(p => p.GetSendEndpoint(expectedUri), Times.Once);

        sendEndpointMock.Verify(ep =>
            ep.Send(
                It.Is<CreateRequestedCollaboratorCommand>(m =>
                    m.Names == cmd.Names &&
                    m.Surnames == cmd.Surnames &&
                    m.Email == cmd.Email &&
                    m.FinalDate == cmd.FinalDate &&
                    m.PeriodDateTime == cmd.PeriodDateTime),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
