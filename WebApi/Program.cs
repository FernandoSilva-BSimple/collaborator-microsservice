using Application.DTO.Collaborators;
using Application.Interfaces;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using WebApi.Consumers;
using WebApi.Publishers;
using Application.IPublishers;
using Domain.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AbsanteeContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ICollaboratorTempService, CollaboratorTempService>();

builder.Services.AddTransient<IMessagePublisher, MassTransitPublisher>();

//Repositories
builder.Services.AddTransient<IUserRepository, UserRepositoryEF>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepositoryEF>();
builder.Services.AddTransient<ICollaboratorTempRepository, CollaboratorTempRepositoryEF>();


//Factories
builder.Services.AddTransient<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddTransient<IUserFactory, UserFactory>();
builder.Services.AddTransient<ICollaboratorTempFactory, CollaboratorTempFactory>();


//Mappers
builder.Services.AddTransient<UserDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<CollaboratorTempDataModelConverter>();
builder.Services.AddAutoMapper(cfg =>
{
    //DataModels
    cfg.AddProfile<DataModelMappingProfile>();

    //DTO
    cfg.CreateMap<Collaborator, CollaboratorDTO>();
});


// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedConsumer>();
    x.AddConsumer<CollaboratorConsumer>();
    x.AddConsumer<CollaboratorUpdatedConsumer>();

    x.AddSagaStateMachine<CollaboratorSaga, CollaboratorSagaState>().InMemoryRepository();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        var instance = InstanceInfo.InstanceId;
        cfg.ReceiveEndpoint($"collaborators-cmd-{instance}", e =>
        {
            e.ConfigureConsumer<CollaboratorConsumer>(context);
            e.ConfigureConsumer<CollaboratorUpdatedConsumer>(context);
            e.ConfigureConsumer<UserCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("collaborator-saga-queue", e =>
{
    e.ConfigureSaga<CollaboratorSagaState>(context);
});
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
