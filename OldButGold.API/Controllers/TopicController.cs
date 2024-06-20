using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OldButGold.Forums.API.Models.Comments;
using OldButGold.Forums.Domain.UseCases.CreateComment;

namespace OldButGold.Forums.API.Controllers
{
    [ApiController]
    [Route("topics")]
    public class TopicController(
        IMediator mediator,
        IMapper mapper) : ControllerBase
    {

        [HttpPost("{topicId:guid}/comments")]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(200, Type = typeof(Comment))]
        public async Task<IActionResult> CreateComment(
            Guid topicId,
            [FromBody] CreateComment request,
            CancellationToken cancellationToken)
        {
            var command = new CreateCommentCommand(topicId, request.Text);
            var comment = await mediator.Send(command, cancellationToken);

            return Ok(mapper.Map<Comment>(comment));
        }
    }
}
