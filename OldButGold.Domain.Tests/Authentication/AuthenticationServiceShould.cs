using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using OldButGold.Forums.Domain.Authentication;
using System.Security.Cryptography;

namespace OldButGold.Forums.Domain.Tests.Authentication
{
    public class AuthenticationServiceShould
    {
        private readonly AuthenticationService sut;
        private readonly ISetup<ISymmetricDecryptor, Task<string>> setupDecryptor;
        private readonly ISetup<IAuthenticationStorage, Task<Session?>> findSessionSetup;


        public AuthenticationServiceShould()
        {
            var decryptor = new Mock<ISymmetricDecryptor>();
            setupDecryptor = decryptor.Setup(d => d.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

            var storage = new Mock<IAuthenticationStorage>();
            findSessionSetup = storage.Setup(s => s.FindSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

            var logger = new Mock<ILogger<AuthenticationService>>();

            var options = new Mock<IOptions<AuthenticationConfiguration>>();
            options
                .Setup(o => o.Value)
                .Returns(new AuthenticationConfiguration()
                {
                    Base64Key = "XtDotH86WLjaEoFev6uZFN/3C0EQIApoD+5iqqmPtpg=",

                });

            sut = new AuthenticationService(decryptor.Object, storage.Object, logger.Object, options.Object);
        }

        [Fact]
        public async Task ReturnGuestIdentity_WhenTokenCannotBeDecrypted()
        {
            setupDecryptor.Throws<CryptographicException>();
            var actual = await sut.Authenticate("bad-token", CancellationToken.None);

            actual.Should().BeEquivalentTo(User.Guest);
        }

        [Fact]
        public async Task ReturnGuestIdentity_WhenTokenIsInvalid()
        {
            setupDecryptor.ReturnsAsync("not-a-guid");
            var actual = await sut.Authenticate("bad-token", CancellationToken.None);

            actual.Should().BeEquivalentTo(User.Guest);
        }

        [Fact]
        public async Task ReturnGuestIdentity_WhenSessionNotFound()
        {
            setupDecryptor.ReturnsAsync("358aeb37-0ada-45e0-9999-132fb981157b");
            findSessionSetup.ReturnsAsync(() => null);

            var actual = await sut.Authenticate("good-token", CancellationToken.None);

            actual.Should().BeEquivalentTo(User.Guest);
        }

        [Fact]
        public async Task ReturnGuestIdentity_WhenSessionIsExpired()
        {
            setupDecryptor.ReturnsAsync("358aeb37-0ada-45e0-9999-132fb981157b");
            findSessionSetup.ReturnsAsync(new Session()
            {
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(-1)
            });

            var actual = await sut.Authenticate("good-token", CancellationToken.None);

            actual.Should().BeEquivalentTo(User.Guest);
        }

        [Fact]
        public async Task ReturnIdentity_WhenSessionIsValid()
        {
            var userId = Guid.Parse("459e4574-1be5-4ba2-83ef-a90b99cea996");
            var sessionId = Guid.Parse("9b917718-5a65-49cd-846a-19738781d9c4");

            setupDecryptor.ReturnsAsync("9b917718-5a65-49cd-846a-19738781d9c4");
            findSessionSetup.ReturnsAsync(new Session()
            {
                UserId = userId,
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(1)
            });

            var actualIdentity = await sut.Authenticate("good-token", CancellationToken.None);


            actualIdentity.Should().BeEquivalentTo(new User(userId, sessionId));
        }
    }

}
