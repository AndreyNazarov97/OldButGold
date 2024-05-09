using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.Tests.Authentication
{
    public class AuthenticationServiceShould
    {
        private readonly AuthenticationService sut;
        private readonly ISetup<ISymmetricDecryptor, Task<string>> setupDecryptor;

        public AuthenticationServiceShould()
        {
            var decryptor = new Mock<ISymmetricDecryptor>();
            setupDecryptor = decryptor.Setup(d => d.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

            var options = new Mock<IOptions<AuthenticationConfiguration>>();
            options
                .Setup(o => o.Value)
                .Returns(new AuthenticationConfiguration()
                {
                    Base64Key = "XtDotH86WLjaEoFev6uZFN/3C0EQIApoD+5iqqmPtpg=",

                });

            sut = new AuthenticationService(decryptor.Object, options.Object);
        }

        [Fact]
        public async Task ExtractIdentityFromToken()
        {
            setupDecryptor.ReturnsAsync("459e4574-1be5-4ba2-83ef-a90b99cea996");

            var actualIdentity  = await sut.Authenticate("some token", CancellationToken.None);

            actualIdentity.Should().BeEquivalentTo( new User(Guid.Parse("459e4574-1be5-4ba2-83ef-a90b99cea996")));
        }
    }

}
