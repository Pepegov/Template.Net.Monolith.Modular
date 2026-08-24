using Microsoft.EntityFrameworkCore;
using ModularMonolith.Framework.DataAccess;
using ModularMonolith.Framework.DataAccess.UnitOfWork;
using ModularMonolith.Module.Template.Contract.Dto;
using ModularMonolith.Module.Template.DataAccess.Interface;
using Mapster;

namespace ModularMonolith.Module.Template.Contract.Implementation;

public class TemplateContract(ITemplateDbContext templateDbContext) : ITemplateContract, IContract
{
    public IDbContext DbContext => templateDbContext;
    
    public async Task<Guid> CreateAsync(TemplateDto dto, CancellationToken cancellationToken)
    {
        var entity = dto.Adapt<Domain.Agreggates.Template>();
        await templateDbContext.Templates.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task<IList<TemplateDto>> GetAsync(CancellationToken cancellationToken)
    {
        var entities = await templateDbContext.Templates.ToListAsync(cancellationToken: cancellationToken);
        return entities.Adapt<List<TemplateDto>>();
    }
}