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

            var encryptor = new Mock<ISymmetricEncryptor>();
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

            (await sut.Invoking(s => s.Execute(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
                .Which.Errors.Should().Contain(e => e.PropertyName == "Login");
        }

        [Fact]
        public async Task ThrowValidationException_WhenPasswordDoesntMatch()
        {
            findUserSetup.ReturnsAsync(new RecognisedUser());
            comparePasswordsSetup.Returns(false);

            (await sut.Invoking(s => s.Execute(new SignInCommand("Test", "qwerty"), CancellationToken.None))
                .Should().ThrowAsync<ValidationException>())
                .Which.Errors.Should().Contain(e => e.PropertyName == "Password");
        }

        [Fact]
        public async Task ReturnToken()
        {
            var userId = Guid.Parse("336f2aa9-4ae4-4ee4-b908-e29a5211a4be");
            findUserSetup.ReturnsAsync(new RecognisedUser
            {
                UserId = userId,
                PasswordHash = new byte[] {1},
                Salt = new byte[] {2},
            });
            comparePasswordsSetup.Returns(true);
            encryptorSetup.ReturnsAsync("token");

            var (identity, token) = await sut.Execute(new SignInCommand("Test", "qwerty"), CancellationToken.None);
            identity.UserId.Should().Be(userId);
            token.Should().Be("token");
        }
    }
}
