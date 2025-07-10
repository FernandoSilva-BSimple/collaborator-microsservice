using Domain.Interfaces;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IUser?> AddUserReferenceAsync(Guid userId);

    }
}
