using MediatR;
using ModularMonolith.Module.Template.Contract;
using ModularMonolith.Module.Template.Contract.Dto;
using ModularMonolith.Module.Template.Contract.Query;

namespace ModularMonolith.Module.Template.UseCases.Template.Handles;

public class TemplateGetQueryHandler(ITemplateContract templateContract) : IRequestHandler<TemplateGetQuery, IList<TemplateDto>>
{
    public Task<IList<TemplateDto>> Handle(TemplateGetQuery request, CancellationToken cancellationToken)
        => templateContract.GetAsync(cancellationToken);
}