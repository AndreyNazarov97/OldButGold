using Microsoft.AspNetCore.Mvc;
using OldButGold.API.Models;
using OldButGold.API.Models.Topic;
using OldButGold.Domain.UseCases.CreateForum;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Domain.UseCases.GetTopics;
using Forum = OldButGold.API.Models.Forum;
using Topic = OldButGold.API.Models.Topic.Topic;


namespace OldButGold.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(Forum))]
        public async Task<IActionResult> CreateForum(
        [FromBody] CreateForum request,
        [FromServices] ICreateForumUseCase useCase,
        CancellationToken cancellationToken)
        {
            var command = new CreateForumCommand(request.Title);
            var forum = await useCase.Execute(command, cancellationToken);


            return CreatedAtRoute(nameof(GetForums), new Forum()
            {
                Id = forum.Id,
                Title = forum.Title,
            });
        }

        /// <summary>
        /// Get list of every forum
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(200, Type = typeof(Forum))]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase useCase,
            CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);

            return Ok(forums.Select(f => new Forum()
            {
                Id = f.Id,
                Title = f.Title,
            }));
        }

        [HttpPost("{forumId}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(Topic))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopic request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken cancellationToken)
        {
            var command = new CreateTopicCommand(forumId, request.Title);
            var topic = await useCase.Execute(command, cancellationToken);
            return CreatedAtRoute(nameof(GetForums), new Topic
            {
                Title = topic.Title,
                Id = topic.Id,
                CreatedAt = topic.CreatedAt,
            });
        }


        [HttpGet("{forumId:guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(410)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTopics(
            [FromRoute] Guid forumId,
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromServices] IGetTopicsUseCase useCase,
            CancellationToken cancellationToken)
        {
            var query = new GetTopicsQuery(forumId, skip, take);
            var (resources, totalCount) = await useCase.Execute(query, cancellationToken);
            return Ok(new { resources = resources.Select(r => new Topic()
            {
                Id = r.Id,
                Title = r.Title,
                CreatedAt = r.CreatedAt,
            }), totalCount });
        }

    }
}
