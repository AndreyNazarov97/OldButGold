using OldButGold.Forums.API.Authentication;
using OldButGold.Forums.Domain.Authentication;

namespace OldButGold.Forums.API.Middleware
{
    public class AuthenticationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(
            HttpContext httpContext,
            IAuthTokenStorage authTokenStorage,
            IAuthenticationService authenticationService,
            IIdentityProvider identityProvider)
        {
            var identity = authTokenStorage.TryExtract(httpContext, out var authToken)
                ? await authenticationService.Authenticate(authToken, httpContext.RequestAborted)
                : User.Guest;

            identityProvider.Current = identity;

            await next(httpContext);
        }
    }
}
