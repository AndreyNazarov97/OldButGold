using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.UseCases.SignIn;
using OldButGold.Domain.UseCases.SignOn;

namespace OldButGold.Domain.Tests.SignIn
{
    public class SignInUseCaseShould
    {
        private readonly SignInUseCase sut;
        private readonly ISetup<IPasswordManager, bool> comparePasswordsSetup;
        private readonly ISetup<IPasswordManager, (byte[] Salt, byte[] Hash)> generatePasswordPartsSetup;
        private readonly Mock<ISignInStorage> storage;
        private readonly ISetup<ISignInStorage, Task<RecognisedUser>> findUserSetup;
        private readonly ISetup<ISignInStorage, Task<Guid>> createSessionSetup;
        private readonly Mock<ISymmetricEncryptor> encryptor;
        private readonly ISetup<ISymmetricEncryptor, Task<string>> encryptorSetup;

        public SignInUseCaseShould()
        {
            var validator = new Mock<IValidator<SignInCommand>>();
            validator
                .Setup(x => x.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var passwordManager = new Mock<IPasswordManager>();
            comparePasswordsSetup = passwordManager.Setup(m => m.ComparePassword(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));
            generatePasswordPartsSetup = passwordManager.Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));

            storage = new Mock<ISignInStorage>();
            findUserSetup = storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
            createSessionSetup = storage.Setup(s => s.CreateSession(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()));

            encryptor = new Mock<ISymmetricEncryptor>();
            encryptorSetup = encryptor.Setup(e => e.Encrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

            var configuration = new Mock<IOptions<AuthenticationConfiguration>>();
            configuration
                .Setup(c => c.Value)
                .Returns(new AuthenticationConfiguration()
                {
                    Base64Key = "XtDotH86WLjaEoFev6uZFN/3C0EQIApoD+5iqqmPtpg="
                });

            sut = new SignInUseCase(validator.Object, storage.Object, passwordManager.Object, encryptor.Object, configuration.Object);
        }

        [Fact]
        public async Task ThrowValidationException_WHenUserNotFound()
        {
            findUserSetup.ReturnsAsync(() => null);

            (await sut.Invoking(s => s.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
                .Which.Errors.Should().Contain(e => e.PropertyName == "Login");
        }

        [Fact]
        public async Task ThrowValidationException_WhenPasswordDoesntMatch()
        {
            findUserSetup.ReturnsAsync(new RecognisedUser());
            comparePasswordsSetup.Returns(false);

            (await sut.Invoking(s => s.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
                .Which.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public async Task CreateSession_WhenPasswordMathces()
        {
            var userId = Guid.Parse("1d2331ba-9850-4021-9706-0240e0d3b9f0");
            var sessionId = Guid.Parse("e1785f6f-0249-4624-800f-dcabc0f51e49");

            findUserSetup.ReturnsAsync(new RecognisedUser(){UserId = userId,});
            comparePasswordsSetup.Returns(true);
            createSessionSetup.ReturnsAsync(sessionId);

            await sut.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None);
            storage.Verify(s => s.CreateSession(userId, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReturnTokenAndIdentity()
        {
            var userId = Guid.Parse("336f2aa9-4ae4-4ee4-b908-e29a5211a4be");
            var sessionId = Guid.Parse("ec06bad3-0b00-488a-a71a-f37bb492a7ec");
            findUserSetup.ReturnsAsync(new RecognisedUser
            {
                UserId = userId,
                PasswordHash = new byte[] {1},
                Salt = new byte[] {2},
            });
            comparePasswordsSetup.Returns(true);
            createSessionSetup.ReturnsAsync(sessionId);
            encryptorSetup.ReturnsAsync("token");

            var (identity, token) = await sut.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None);
            identity.UserId.Should().Be(userId);
            identity.SessionId.Should().Be(sessionId);
            token.Should().Be("token");
        }

        [Fact]
        public async Task EncryptSessionIdIntoToken()
        {
            var userId = Guid.Parse("1d2331ba-9850-4021-9706-0240e0d3b9f0");
            var sessionId = Guid.Parse("e1785f6f-0249-4624-800f-dcabc0f51e49");

            findUserSetup.ReturnsAsync(new RecognisedUser() { UserId = userId, });
            comparePasswordsSetup.Returns(true);
            createSessionSetup.ReturnsAsync(sessionId);

            await sut.Handle(new SignInCommand("Test", "qwerty"), CancellationToken.None);
            encryptor.Verify(s => s
                .Encrypt("e1785f6f-0249-4624-800f-dcabc0f51e49", It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
        }
    }
}
