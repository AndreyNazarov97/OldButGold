using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.UseCases.SignOn;

namespace OldButGold.Forums.Domain.Tests.SignOn
{
    public class SignOnUseCaseShould
    {
        private readonly ISetup<IPasswordManager, bool> comparePasswordsSetup;
        private readonly ISetup<IPasswordManager, (byte[] Salt, byte[] Hash)> generatePasswordPartsSetup;
        private readonly Mock<ISignOnStorage> storage;
        private readonly ISetup<ISignOnStorage, Task<Guid>> createUserSetup;
        private readonly SignOnUseCase sut;

        public SignOnUseCaseShould()
        {
            var passwordManager = new Mock<IPasswordManager>();
            comparePasswordsSetup = passwordManager.Setup(m => m.ComparePassword(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));
            generatePasswordPartsSetup = passwordManager.Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));

            storage = new Mock<ISignOnStorage>();
            createUserSetup = storage.Setup(s => s.CreateUser(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

            sut = new SignOnUseCase(passwordManager.Object, storage.Object);
        }

        [Fact]
        public async Task CreateUser_WithGeneratedPasswordParts()
        {
            byte[] salt = [1];
            byte[] hash = [2];
            generatePasswordPartsSetup.Returns((Salt: salt, Hash: hash));

            var actual = await sut.Handle(new SignOnCommand("Login", "Password"), CancellationToken.None);

            storage.Verify(s => s.CreateUser("Login", salt, hash, It.IsAny<CancellationToken>()), Times.Once());
            storage.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReturnIdentityOfNewlyCreatedUser()
        {
            byte[] salt = [1];
            byte[] hash = [2];
            generatePasswordPartsSetup.Returns((Salt: salt, Hash: hash));
            createUserSetup.ReturnsAsync(Guid.Parse("162e20bf-ca2b-4695-8db5-810e0320d5dc"));


            var actual = await sut.Handle(new SignOnCommand("Login", "Password"), CancellationToken.None);
            actual.UserId.Should().Be(Guid.Parse("162e20bf-ca2b-4695-8db5-810e0320d5dc"));
        }

    }
}
