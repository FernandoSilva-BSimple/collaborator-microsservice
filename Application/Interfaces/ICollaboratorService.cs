using Application.DTO;
using Application.DTO.Collaborators;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorService
    {
        Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);
        Task<Result<CreatedCollaboratorDTO>> Create(CreateCollaboratorDTO collabDto);
        Task<Result<CollabUpdatedDTO>?> EditCollaborator(CollabData dto);
        Task<ICollaborator?> UpdateCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);
        Task<ICollaborator> AddCollaboratorAsync(ICollaborator collaborator);

    }
}