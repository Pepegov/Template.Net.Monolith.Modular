using Mapster;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Framework.DataAccess;
using ModularMonolith.Framework.DataAccess.UnitOfWork;
using ModularMonolith.Module.Builder.Contracts.Dto;
using ModularMonolith.Module.Builder.DataAccess.Interface;

namespace ModularMonolith.Module.Builder.Contracts.Implementation;

public class BuilderContract(IBuilderDbContext builderDbContext) : IBuilderContract, IContract
{
    public IDbContext DbContext => builderDbContext;
    
    public async Task CreateAsync(BuilderDto dto, CancellationToken cancellationToken)
    {
        var entity = dto.Adapt<Domain.Agreggates.Builder>();
        await builderDbContext.Builders.AddAsync(entity, cancellationToken);
    }

    public async Task<IList<BuilderDto>> GetAsync(CancellationToken cancellationToken)
    {
        var entities = await builderDbContext.Builders.ToListAsync(cancellationToken: cancellationToken);
        return entities.Adapt<List<BuilderDto>>();
    }
}
