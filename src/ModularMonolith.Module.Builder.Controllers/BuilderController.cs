using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Module.Builder.Contracts.Commands;
using ModularMonolith.Module.Builder.Contracts.Dto;
using ModularMonolith.Module.Builder.Contracts.Query;

namespace ModularMonolith.Module.Builder.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuilderController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<IList<BuilderDto>> Get(CancellationToken cancellationToken)
        => mediator.Send(new BuilderGetQuery(), cancellationToken);
    
    [HttpPost]
    public Task Create([FromBody] BuilderDto model, CancellationToken cancellationToken)
        => mediator.Send(new BuilderCreateCommand(model), cancellationToken);
}
