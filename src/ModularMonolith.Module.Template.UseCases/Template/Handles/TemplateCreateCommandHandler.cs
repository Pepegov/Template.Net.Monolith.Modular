using MediatR;
using ModularMonolith.Module.Template.Contract;
using ModularMonolith.Module.Template.Contract.Commands;

namespace ModularMonolith.Module.Template.UseCases.Template.Handles;

public class TemplateCreateCommandHandler(ITemplateContract templateContract) : IRequestHandler<TemplateCreateCommand, Unit>
{
    public async Task<Unit> Handle(TemplateCreateCommand request, CancellationToken cancellationToken)
    {
        await templateContract.CreateAsync(request.Model, cancellationToken);
        await templateContract.DbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}