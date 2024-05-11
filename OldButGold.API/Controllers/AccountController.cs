﻿using Microsoft.AspNetCore.Mvc;
using OldButGold.API.Authentication;
using OldButGold.API.Models;
using OldButGold.Domain.UseCases.SignIn;
using OldButGold.Domain.UseCases.SignOn;

namespace OldButGold.API.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SignOn(
            [FromBody] SignOn request,
            [FromServices] ISignOnUseCase useCase,
            CancellationToken cancellationToken)
        {
            var identity = await useCase.Execute(new SignOnCommand(request.Login, request.Password), cancellationToken);

            return Ok(identity);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(
            [FromBody] SignIn request,
            [FromServices] ISignInUseCase useCase,
            [FromServices] IAuthTokenStorage authTokenStorage,
            CancellationToken cancellationToken)
        {
            var (identity, token) = await useCase.Execute(new SignInCommand(request.Login, request.Password), cancellationToken);

            authTokenStorage.Store(HttpContext, token);

            return Ok(identity);
        }

    }
}