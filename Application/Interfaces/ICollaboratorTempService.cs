using Application.DTO;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorTempService
    {
        Task CreateCollaboratorTempAndRequestUserAsync(Guid correlationId, CreateCollaboratorAndUserDTO dto);

    }
}
