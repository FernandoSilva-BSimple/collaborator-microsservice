using Domain.Interfaces;
using Domain.Messages;

namespace Application.IPublishers
{
    public interface IMessagePublisher
    {
        Task PublishCollaboratorCreatedAsync(ICollaborator collaborator);
        Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator);
        Task PublishForCollaboratorSagaAsync(CreateCollaboratorRequested message);
    }
}