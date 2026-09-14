using MediatR;
using ModularMonolith.Module.Template.Contract.Dto;

namespace ModularMonolith.Module.Template.Contract.Commands;

public record TemplateCreateCommand(TemplateDto Model) : IRequest<Unit>;