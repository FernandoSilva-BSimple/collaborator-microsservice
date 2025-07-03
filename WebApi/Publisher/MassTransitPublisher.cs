using Application.IPublishers;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;
using MassTransit;

namespace WebApi.Publishers
{
    public class MassTransitPublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishCollaboratorCreatedAsync(ICollaborator collaborator)
        {
            var eventMessage = new CollaboratorCreatedMessage(
                collaborator.Id,
                collaborator.UserId,
                collaborator.PeriodDateTime
            );

            await _publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator)
        {
            var eventMessage = new CollaboratorUpdatedMessage(
                collaborator.Id,
                collaborator.UserId,
                collaborator.PeriodDateTime
            );

            await _publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishAsync(CreateCollaboratorRequested message)
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task PublishAsync(CollaboratorWithoutUserCreatedMessage message)
        {
            await _publishEndpoint.Publish(message);
        }

    }
}
