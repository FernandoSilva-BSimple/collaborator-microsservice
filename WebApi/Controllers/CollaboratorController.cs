using Application.DTO;
using Application.DTO.Collaborators;
using Application.Interfaces;
using Domain.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/collaborators")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collabService;
    private readonly ICollaboratorTempService _collabTempService;

    public CollaboratorController(ICollaboratorService collabService)
    {
        _collabService = collabService;
    }

    [HttpPost]
    public async Task<ActionResult<CreatedCollaboratorDTO>> Create([FromBody] CreateCollabDTO collabDto)
    {
        var createCollabDto = new CreateCollaboratorDTO(collabDto.UserId, collabDto.PeriodDateTime);

        var collabCreated = await _collabService.Create(createCollabDto);

        return collabCreated.ToActionResult();
    }

    [HttpPut]
    public async Task<ActionResult<CollabUpdatedDTO>> updateCollaborator([FromBody] CollabDetailsDTO newCollabData)
    {
        var collabData = new CollabData(newCollabData.Id, newCollabData.PeriodDateTime);

        var result = await _collabService.EditCollaborator(collabData);
        if (result == null) return BadRequest("Invalid Arguments");
        return Ok(result);
    }


    [HttpPost("with-user")]
    public async Task<IActionResult> CreateWithUser(
    [FromBody] CreateCollaboratorAndUserDTO dto)
    {
        await _collabTempService.StartSagaAsync(dto);
        return Accepted();
    }
}