using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;

public interface ICollaboratorTempRepository : IGenericRepositoryEF<ICollaboratorTemp, CollaboratorTemp, ICollaboratorTempVisitor>
{
}