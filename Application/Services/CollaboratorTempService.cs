using Application.DTO;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Factory;
using Domain.IRepository;
using Domain.Messages;

namespace Application.Services
{
    public class CollaboratorTempService : ICollaboratorTempService
    {
        private readonly ICollaboratorTempRepository _repository;
        private readonly ICollaboratorTempFactory _factory;
        private readonly IMessagePublisher _publisher;

        public CollaboratorTempService(
            ICollaboratorTempRepository repository,
            ICollaboratorTempFactory factory,
            IMessagePublisher publisher)
        {
            _repository = repository;
            _factory = factory;
            _publisher = publisher;
        }

        public async Task CreateCollaboratorTempAndRequestUserAsync(Guid correlationId, CreateCollaboratorAndUserDTO dto)
        {
            var collaboratorTemp = _factory.Create(
                correlationId,
                dto.Names,
                dto.Surnames,
                dto.Email,
                dto.FinalDate,
                dto.PeriodDateTime
            );
            await _repository.AddAsync(collaboratorTemp);

            var message = new CollaboratorWithoutUserCreatedMessage(
                correlationId,
                dto.Names,
                dto.Surnames,
                dto.Email,
                dto.FinalDate
            );

            await _publisher.PublishAsync(message);
        }

    }
}
