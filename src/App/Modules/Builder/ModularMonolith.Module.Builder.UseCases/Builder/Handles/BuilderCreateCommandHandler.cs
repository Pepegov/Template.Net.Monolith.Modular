using MediatR;
using ModularMonolith.Module.Builder.Contracts;
using ModularMonolith.Module.Builder.Contracts.Commands;

namespace ModularMonolith.Module.Builder.UseCases.Builder.Handles;

public class BuilderCreateCommandHandler(IBuilderContract builderContract) : IRequestHandler<BuilderCreateCommand, Unit>
{
    public async Task<Unit> Handle(BuilderCreateCommand request, CancellationToken cancellationToken)
    {
        await builderContract.CreateAsync(request.Model, cancellationToken);
        await builderContract.DbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
