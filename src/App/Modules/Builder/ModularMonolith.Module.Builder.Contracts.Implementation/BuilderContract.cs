using Mapster;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Framework.DataAccess;
using ModularMonolith.Framework.DataAccess.UnitOfWork;
using ModularMonolith.Module.Builder.Contracts.Dto;
using ModularMonolith.Module.Builder.DataAccess.Interface;
using ModularMonolith.Module.Template.Contract;
using ModularMonolith.Module.Template.Contract.Dto;

namespace ModularMonolith.Module.Builder.Contracts.Implementation;

public class BuilderContract(IBuilderDbContext builderDbContext, IUnitOfWork unitOfWork, ITemplateContract diTemplateContract) : IBuilderContract
{
    public IDbContext DbContext => builderDbContext;
    
    public async Task CreateAsync(BuilderDto dto, CancellationToken cancellationToken)
    {
        var entity = dto.Adapt<Domain.Agreggates.Builder>();
        await builderDbContext.Builders.AddAsync(entity, cancellationToken);
    }
    
    public async Task CreateWithTemplateAsync(BuilderDto dto, TemplateDto templateDto, CancellationToken cancellationToken)
    {
        // Transaction and contract
        var transaction = await builderDbContext.BeginTransactionAsync(cancellationToken);
        await unitOfWork.UseTransactionAsync(transaction, cancellationToken: cancellationToken);
        var templateContract = unitOfWork.GetRequiredContract<ITemplateContract>();

        // Template
        var templateId = await templateContract.CreateAsync(templateDto, cancellationToken: cancellationToken);
        
        // Builder
        var entity = dto.Adapt<Domain.Agreggates.Builder>();
        entity.TemplateId = templateId;
        await builderDbContext.Builders.AddAsync(entity, cancellationToken);

        // Commit transaction
        await builderDbContext.SaveChangesAsync(cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await unitOfWork.CommitTransaction(cancellationToken);
    }
    
    public async Task CreateWithTemplate2Async(BuilderDto dto, TemplateDto templateDto, CancellationToken cancellationToken)
    {
        // Template
        var templateId = await diTemplateContract.CreateAsync(templateDto, cancellationToken: cancellationToken);
        await diTemplateContract.DbContext.SaveChangesAsync(cancellationToken);
        
        // Builder
        var entity = dto.Adapt<Domain.Agreggates.Builder>();
        entity.TemplateId = templateId;
        await builderDbContext.Builders.AddAsync(entity, cancellationToken);
        await builderDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<BuilderDto>> GetAsync(CancellationToken cancellationToken)
    {
        var entities = await builderDbContext.Builders.ToListAsync(cancellationToken: cancellationToken);
        return entities.Adapt<List<BuilderDto>>();
    }
}
