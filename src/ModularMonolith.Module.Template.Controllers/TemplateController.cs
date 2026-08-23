using MediatR;
using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Module.Template.Contract.Commands;
using ModularMonolith.Module.Template.Contract.Dto;
using ModularMonolith.Module.Template.Contract.Query;

namespace ModularMonolith.Module.Template.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemplateController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public Task<IList<TemplateDto>> Get(CancellationToken cancellationToken)
        => mediator.Send(new TemplateGetQuery(), cancellationToken);
    
    [HttpPost]
    public Task Create([FromBody] TemplateDto model, CancellationToken cancellationToken)
        => mediator.Send(new TemplateCreateCommand(model), cancellationToken);
}