using MediatR;
using Microsoft.AspNetCore.Mvc;
using OldButGold.Search.Domain.Models;
using OldButGold.Search.Domain.UseCases.Index;
using OldButGold.Search.Domain.UseCases.Search;

namespace OldButGold.Search.API.Controllers
{
    public class SearchController(
        IMediator mediator) : ControllerBase
    {
        [HttpPost("index")]
        public async Task<IActionResult> Index(
            [FromBody] SearchEntity searchEntity,
            CancellationToken cancellationToken)
        {
            var command = new IndexCommand(searchEntity.EntityId, searchEntity.EntityType, searchEntity.Title, searchEntity.Text);
            await mediator.Send(command,cancellationToken);

            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            string query,
            CancellationToken cancellationToken)
        {
            var (resources, totalCount) = await mediator.Send(new SearchQuery(query),cancellationToken);
            return Ok(new {resources, totalCount});
        }

    }
}
