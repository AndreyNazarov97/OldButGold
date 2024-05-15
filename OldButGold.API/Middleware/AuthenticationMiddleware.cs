using OldButGold.API.Authentication;
using OldButGold.Domain.Authentication;

namespace OldButGold.API.Middleware
{
    public class AuthenticationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(
            HttpContext httpContext,
            IAuthTokenStorage authTokenStorage,
            IAuthenticationService authenticationService,
            IIdentityProvider identityProvider,
            CancellationToken cancellationToken)
        {
            var identity = authTokenStorage.TryExtract(httpContext, out var authToken)
                ? await authenticationService.Authenticate(authToken, cancellationToken)
                : User.Guest;

            identityProvider.Current = identity;

            await next(httpContext);
        }
    }
}
