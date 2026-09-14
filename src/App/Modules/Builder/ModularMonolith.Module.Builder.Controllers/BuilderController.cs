using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Module.Builder.Contracts;
using ModularMonolith.Module.Builder.Contracts.Commands;
using ModularMonolith.Module.Builder.Contracts.Dto;
using ModularMonolith.Module.Builder.Contracts.Query;
using ModularMonolith.Module.Template.Contract.Dto;

namespace ModularMonolith.Module.Builder.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuilderController(IMediator mediator, IBuilderContract builderContract) : ControllerBase
{
    [HttpGet]
    public Task<IList<BuilderDto>> Get(CancellationToken cancellationToken)
        => mediator.Send(new BuilderGetQuery(), cancellationToken);
    
    [HttpPost]
    public Task Create([FromBody] BuilderDto model, CancellationToken cancellationToken)
        => mediator.Send(new BuilderCreateCommand(model), cancellationToken);
    
    [HttpPost("CreateWithTemplate")]
    public Task CreateWithTemplate([FromBody] CreateWithTemplateRequest model, CancellationToken cancellationToken)
        => builderContract.CreateWithTemplateAsync(model.Builder, model.Template, cancellationToken);
}

public class CreateWithTemplateRequest()
{
    public BuilderDto Builder { get; set; }
    public TemplateDto Template { get; set; }
}
