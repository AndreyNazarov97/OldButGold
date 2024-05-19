using FluentAssertions;
using OldButGold.Forums.Domain.Authentication;
using System.Security.Cryptography;

namespace OldButGold.Forums.Domain.Tests.Authentication
{
    public class AesSymmetricEncryptorDecryptorShould
    {
        private readonly AesSymmetricEncryptorDecryptor sut = new();

        [Fact]
        public async Task ReturnMeaningfulEncryptedString()
        {
            var key = RandomNumberGenerator.GetBytes(32);
            var actual = await sut.Encrypt("Hello world!", key, CancellationToken.None);

            actual.Should().NotBeEmpty();
        }

        [Fact]
        public async Task DecryptEncryptedString_WhenKeyIsSame()
        {
            var key = RandomNumberGenerator.GetBytes(32);
            var password = "qwerty123";
            var encrypted = await sut.Encrypt(password, key, CancellationToken.None);

            var decrypted = await sut.Decrypt(encrypted, key, CancellationToken.None);
            decrypted.Should().Be(password);
        }

        [Fact]
        public async Task ThrowException_WhenDecryptingWithDifferentKey()
        {
            var encrypted = await sut.Encrypt("Hello world!", RandomNumberGenerator.GetBytes(32), CancellationToken.None);
            await sut.Invoking(s => s.Decrypt(encrypted, RandomNumberGenerator.GetBytes(32), CancellationToken.None))
                .Should().ThrowAsync<CryptographicException>();


        }
    }
}
