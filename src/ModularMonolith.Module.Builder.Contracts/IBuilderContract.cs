using ModularMonolith.Framework.DataAccess.UnitOfWork;
using ModularMonolith.Module.Builder.Contracts.Dto;

namespace ModularMonolith.Module.Builder.Contracts;

public interface IBuilderContract : IContract
{
    Task CreateAsync(BuilderDto dto, CancellationToken cancellationToken = default);
    Task<IList<BuilderDto>> GetAsync(CancellationToken cancellationToken = default);
}
