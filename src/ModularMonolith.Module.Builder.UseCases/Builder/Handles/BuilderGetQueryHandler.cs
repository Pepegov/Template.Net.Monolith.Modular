using MediatR;
using ModularMonolith.Module.Builder.Contracts;
using ModularMonolith.Module.Builder.Contracts.Dto;
using ModularMonolith.Module.Builder.Contracts.Query;

namespace ModularMonolith.Module.Builder.UseCases.Builder.Handles;

public class BuilderGetQueryHandler(IBuilderContract builderContract) : IRequestHandler<BuilderGetQuery, IList<BuilderDto>>
{
    public Task<IList<BuilderDto>> Handle(BuilderGetQuery request, CancellationToken cancellationToken)
        => builderContract.GetAsync(cancellationToken);
}
