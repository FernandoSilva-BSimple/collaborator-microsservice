using Application.Interfaces;
using Application.IPublishers;
using Domain.Models;
using MassTransit;
using Moq;
using WebApi.Consumers;
using Xunit;
using Domain.Messages;

namespace WebApi.IntegrationTests.ConsumerTests
{
    public class UserCreatedConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldCallAddUserReferenceAsync_WithCorrectData()
        {
            // arrange
            var serviceDouble = new Mock<IUserService>();
            var consumer = new UserCreatedConsumer(serviceDouble.Object);

            var initDate = DateTime.Now;
            var finalDate = DateTime.Now.AddYears(1);
            var periodDateTime = new PeriodDateTime(initDate, finalDate);

            var message = new UserCreatedMessage(Guid.NewGuid(), "Teste", "Test", "test@email.com", periodDateTime);

            var context = Mock.Of<ConsumeContext<UserCreatedMessage>>(c => c.Message == message);

            // act
            await consumer.Consume(context);

            // asset
            serviceDouble.Verify(s => s.AddUserReferenceAsync(message.Id), Times.Once);
        }

    }
}