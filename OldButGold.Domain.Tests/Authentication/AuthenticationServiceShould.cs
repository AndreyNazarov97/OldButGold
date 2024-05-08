using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.UseCases.SignIn;

namespace OldButGold.Domain.Tests.Authentication
{
    public class AuthenticationServiceShould
    {
        private readonly AuthenticationService sut;
        private Mock<IAuthenticationStorage> storage;
        private ISetup<IAuthenticationStorage, Task<RecognisedUser?>> findUserSetup;
        private Mock<IOptions<AuthenticationConfiguration>> options;

        public AuthenticationServiceShould()
        {
            storage = new Mock<IAuthenticationStorage>();
            findUserSetup = storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));

            var securityManager = new Mock<IPasswordManager>();
            securityManager
                .Setup(m => m.ComparePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            options = new Mock<IOptions<AuthenticationConfiguration>>();
            options
                .Setup(o => o.Value)
                .Returns(new AuthenticationConfiguration
                {
                    Key = "QkEeenXpHqgP6t0WwpUetAFvUUZiMb4f",
                    Iv = "dtEzMsz2ogg="
                });

            sut = new AuthenticationService(storage.Object, securityManager.Object, options.Object);
        }


        [Fact]
        public async Task ReturnSucces_WhenUserFound()
        {
            findUserSetup.ReturnsAsync(new RecognisedUser()
            {
                Salt = "lKhhmci5Aty/mw3Gqazg5A==",
                PasswordHash = "1xJt1aTh7/yD6kkJcBWbiqJ6+U/lWDH64cGkHJDOkDUtteQyqmH78J14g6bSuq3B/ipaFDbnYS0LAqzH0vyyuA==",
                UserId = Guid.Parse("dca4fda3-bef2-4037-b1a7-1d0397c6dcfd")
            });

            var (succes, authToken) = await sut.SignIn(new BasicSignInCredentials("User", "Password"), CancellationToken.None);
            succes.Should().BeTrue();
            authToken.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AuthenticateUser_AfterTheySignIn()
        {

            var userId = Guid.Parse("cd9c7261-f54f-46fe-962b-bff4e032e03d");
            findUserSetup.ReturnsAsync(new RecognisedUser()
            {
                UserId = userId
            });

            var (_, authToken) = await sut.SignIn(new BasicSignInCredentials("User", "Password"), CancellationToken.None);

            var identity = await sut.Authenticate(authToken, CancellationToken.None);
            identity.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task SignInUser_WhenPasswordMatch()
        {
            var password = "qwerty";
            var securityManager = new PasswordManager();
            var (salt, hash) = securityManager.GeneratePasswordParts(password);

            findUserSetup.ReturnsAsync(new RecognisedUser
            {
                UserId = Guid.Parse("2c0d5ef1-dd38-4c3c-b517-0218c7c19ca8"),
                Salt = salt,
                PasswordHash = hash
            });

            var localSut = new AuthenticationService(storage.Object, securityManager, options.Object );
            var (succes, _) = await localSut.SignIn( new BasicSignInCredentials("User", password), CancellationToken.None );
            succes.Should().BeTrue();





        }
    }
}
