using MediatR;
using Microsoft.AspNetCore.Mvc;
using OldButGold.Forums.Domain.UseCases.SignIn;
using OldButGold.Forums.Domain.UseCases.SignOn;
using OldButGold.Forums.API.Authentication;
using OldButGold.Forums.API.Models;

namespace OldButGold.Forums.API.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SignOn(
            [FromBody] SignOn request,
            CancellationToken cancellationToken)
        {
            var identity = await mediator.Send(new SignOnCommand(request.Login, request.Password), cancellationToken);

            return Ok(identity);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(
            [FromBody] SignIn request,
            [FromServices] IAuthTokenStorage authTokenStorage,
            CancellationToken cancellationToken)
        {
            var (identity, token) = await mediator.Send(new SignInCommand(request.Login, request.Password), cancellationToken);

            authTokenStorage.Store(HttpContext, token);

            return Ok(identity);
        }
    }
}
