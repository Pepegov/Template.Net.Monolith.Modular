using MediatR;
using ModularMonolith.Module.Template.Contract.Dto;

namespace ModularMonolith.Module.Template.Contract.Query;

public class TemplateGetQuery : IRequest<IList<TemplateDto>>;