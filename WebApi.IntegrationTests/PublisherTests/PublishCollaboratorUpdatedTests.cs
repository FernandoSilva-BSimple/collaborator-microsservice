using Application.IPublishers;
using Domain.Interfaces;
using Domain.Models;
using MassTransit;
using Moq;
using Domain.Messages;
using WebApi.Publishers;
using Xunit;

namespace WebApi.IntegrationTests.PublisherTests
{
    public class PublishCollaboratorUpdatedTests
    {
        [Fact]
        public async Task PublishCollaboratorUpdatedAsync_ShouldPublishEventWithCorrectData()
        {
            // Arrange 
            var publishEndpointDouble = new Mock<IPublishEndpoint>();
            var sendEndpointDouble = new Mock<ISendEndpointProvider>();

            var publisher = new MassTransitPublisher(publishEndpointDouble.Object, sendEndpointDouble.Object);

            var collaboratorDouble = new Mock<ICollaborator>();
            var collaboratorId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var period = new PeriodDateTime(DateTime.Now, DateTime.Now.AddYears(1));

            collaboratorDouble.Setup(c => c.Id).Returns(collaboratorId);
            collaboratorDouble.Setup(c => c.UserId).Returns(userId);
            collaboratorDouble.Setup(c => c.PeriodDateTime).Returns(period);

            // Act 
            await publisher.PublishCollaboratorUpdatedAsync(collaboratorDouble.Object);

            // Assert
            publishEndpointDouble.Verify(
                p => p.Publish(
                    It.Is<CollaboratorUpdatedMessage>(e =>
                        e.Id == collaboratorId &&
                        e.UserId == userId &&
                        e.PeriodDateTime == period
                    ),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}