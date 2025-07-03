using AutoMapper;
using Domain.Factory;
using Domain.Models;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class CollaboratorTempDataModelConverter : ITypeConverter<CollaboratorTempDataModel, CollaboratorTemp>
{
    private readonly ICollaboratorTempFactory _factory;

    public CollaboratorTempDataModelConverter(ICollaboratorTempFactory factory)
    {
        _factory = factory;
    }

    public CollaboratorTemp Convert(CollaboratorTempDataModel source, CollaboratorTemp destination, ResolutionContext context)
    {
        return _factory.Create(source);
    }
}