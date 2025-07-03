using AutoMapper;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<User, UserDataModel>();
        CreateMap<Collaborator, CollaboratorDataModel>();
        CreateMap<CollaboratorDataModel, Collaborator>()
            .ConvertUsing<CollaboratorDataModelConverter>();
        CreateMap<UserDataModel, User>()
            .ConvertUsing<UserDataModelConverter>();
        CreateMap<CollaboratorTemp, CollaboratorTempDataModel>();
        CreateMap<CollaboratorTempDataModel, CollaboratorTemp>()
            .ConvertUsing<CollaboratorTempDataModelConverter>();
    }

}