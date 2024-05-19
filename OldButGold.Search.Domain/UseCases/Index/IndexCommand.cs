using MediatR;
using OldButGold.Search.Domain.Models;

namespace OldButGold.Search.Domain.UseCases.Index
{
    public record IndexCommand(Guid EntityId, SearchEntityType EntityType, string? Title, string? Text) : IRequest;
}
