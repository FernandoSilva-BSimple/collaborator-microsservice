using Domain.Messages;
using MassTransit;
using Application.Interfaces;
using Application.DTO;

public class CreateCollaboratorRequestedConsumer : IConsumer<CreateCollaboratorRequested>
{
    private readonly ICollaboratorTempService _collaboratorTempService;

    public CreateCollaboratorRequestedConsumer(ICollaboratorTempService collaboratorTempService)
    {
        _collaboratorTempService = collaboratorTempService;
    }

    public async Task Consume(ConsumeContext<CreateCollaboratorRequested> context)
    {
        var msg = context.Message;

        var dto = new CreateCollaboratorAndUserDTO
        {
            Names = msg.Names,
            Surnames = msg.Surnames,
            Email = msg.Email,
            FinalDate = msg.FinalDate,
            PeriodDateTime = msg.PeriodDateTime
        };

        await _collaboratorTempService.CreateCollaboratorTempAndRequestUserAsync(msg.CorrelationId, dto);
    }
}
