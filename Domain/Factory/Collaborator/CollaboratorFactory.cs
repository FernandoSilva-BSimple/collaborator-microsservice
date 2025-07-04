using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory
{
    public class CollaboratorFactory : ICollaboratorFactory
    {
        private readonly ICollaboratorRepository _collabRepository;
        private readonly IUserRepository _userRepository;

        public CollaboratorFactory(ICollaboratorRepository collabRepository, IUserRepository userRepository)
        {
            _collabRepository = collabRepository;
            _userRepository = userRepository;
        }

        public async Task<Collaborator> Create(Guid userId, PeriodDateTime periodDateTime)
        {
            var userExists = await _userRepository.Exists(userId);
            if (!userExists)
            {
                throw new ArgumentException("User does not exist for the provided UserId.");
            }

            var collaboratorAlreadyExists = await _collabRepository.ExistsByUserIdAsync(userId);
            if (collaboratorAlreadyExists)
            {
                throw new InvalidOperationException("A collaborator with this UserId already exists.");
            }

            return new Collaborator(userId, periodDateTime);

        }

        public Collaborator Create(Guid collabId, Guid userId, PeriodDateTime periodDateTime)
        {
            return new Collaborator(collabId, userId, periodDateTime);
        }

        public Collaborator Create(ICollaboratorVisitor visitor)
        {
            return new Collaborator(visitor.Id, visitor.UserId, visitor.PeriodDateTime);
        }


        public Collaborator ConvertFromTemp(ICollaboratorTemp collaboratorTemp, Guid userId)
        {
            var periodDateTime = collaboratorTemp.PeriodDateTime;

            return new Collaborator(userId, periodDateTime);
        }
    }
}
