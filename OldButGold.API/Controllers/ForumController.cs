using Microsoft.AspNetCore.Mvc;
using OldButGold.API.Models.Topic;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.UseCases.CreateTopic;
using OldButGold.Domain.UseCases.GetForums;
using Forum = OldButGold.API.Models.Forum;
using Topic = OldButGold.API.Models.Topic.Topic;


namespace OldButGold.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {
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
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(201, Type = typeof(Topic))]
        public async Task<IActionResult> CreateTopic(
            Guid forumId,
            [FromBody] CreateTopic request,
            [FromServices] ICreateTopicUseCase useCase,
            CancellationToken cancellationToken)
        {
            try
            {
                var topic = await useCase.Execute(forumId, request.Title, cancellationToken);
                return CreatedAtRoute(nameof(GetForums), new Topic
                {
                    Title = topic.Title,
                    Id = topic.Id,
                    CreatedAt = topic.CreatedAt,
                });
            }
            catch (Exception exception)
            {
                return exception switch
                {
                    IntentionManagerException => Forbid(),
                    ForumNotFoundException => StatusCode(StatusCodes.Status410Gone),
                    _ => StatusCode(StatusCodes.Status500InternalServerError)
                };
            }
        }

    }
}
