using Microsoft.AspNetCore.Mvc;
using OldButGold.API.Models;
using OldButGold.Domain.UseCases.GetForums;


namespace OldButGold.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForumController : ControllerBase
    {
        /// <summary>
        /// Get list of every forum
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        [HttpGet]
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

    }
}
