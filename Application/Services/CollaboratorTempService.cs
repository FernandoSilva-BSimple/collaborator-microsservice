using Application.DTO;
using Application.Interfaces;
using Application.IPublishers;
using Domain.Factory;
using Domain.Interfaces;
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

        public async Task CreateCollaboratorTempAsync(CreateCollaboratorRequested message)
        {
            var collaboratorTemp = _factory.Create(message.Names, message.Surnames, message.Email, message.FinalDate, message.PeriodDateTime);
            await _repository.AddAsync(collaboratorTemp);
            await _repository.SaveChangesAsync();
        }

        public async Task StartSagaAsync(CreateCollaboratorAndUserDTO dto)
        {
            CreateCollaboratorRequested message = new(dto.Names, dto.Surnames, dto.Email, dto.FinalDate, dto.PeriodDateTime);
            await _publisher.PublishForCollaboratorSagaAsync(message);
        }

        public async Task<ICollaboratorTemp> GetByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email) ?? throw new InvalidOperationException("Collaborator not found");
        }

        public async Task DeleteCollaboratorTempAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id) ?? throw new InvalidOperationException("Collaborator not found");
            await _repository.RemoveAsync(existing);
            await _repository.SaveChangesAsync();
        }
    }
}
