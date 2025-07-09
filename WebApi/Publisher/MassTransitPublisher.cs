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
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MassTransitPublisher(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
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

        public async Task SendCreateCollaboratorSagaCommandAsync(CreateRequestedCollaboratorCommand message)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:collaborators-saga-{InstanceInfo.InstanceId}"));
            await endpoint.Send(message);
        }
    }
}
