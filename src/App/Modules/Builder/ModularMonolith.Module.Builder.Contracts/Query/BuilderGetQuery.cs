using MediatR;
using ModularMonolith.Module.Builder.Contracts.Dto;

namespace ModularMonolith.Module.Builder.Contracts.Query;

public class BuilderGetQuery : IRequest<IList<BuilderDto>>;
