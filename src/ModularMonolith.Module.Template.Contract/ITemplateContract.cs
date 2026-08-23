using ModularMonolith.Framework.DataAccess.UnitOfWork;
using ModularMonolith.Module.Template.Contract.Dto;

namespace ModularMonolith.Module.Template.Contract;

public interface ITemplateContract : IContract
{
    Task CreateAsync(TemplateDto dto, CancellationToken cancellationToken = default);
    Task<IList<TemplateDto>> GetAsync(CancellationToken cancellationToken = default);
}