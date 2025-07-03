using Domain.Interfaces;
using Domain.Messages;

namespace Application.IPublishers
{
    public interface IMessagePublisher
    {
        Task PublishCollaboratorCreatedAsync(ICollaborator collaborator);
        Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator);
        Task PublishAsync(CreateCollaboratorRequested message);
        Task PublishAsync(CollaboratorWithoutUserCreatedMessage message);
    }
}