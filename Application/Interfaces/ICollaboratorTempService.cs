using Application.DTO;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorTempService
    {
        Task StartSagaAsync(CreateCollaboratorAndUserDTO dto);
        Task CreateCollaboratorTempAsync(CreateCollaboratorRequested message);
        Task<ICollaboratorTemp> GetByEmailAsync(string email);
        Task DeleteCollaboratorTempAsync(Guid id);
    }
}
