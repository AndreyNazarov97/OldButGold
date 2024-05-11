﻿using FluentAssertions;
using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.Tests.Authentication
{
    public class PasswordManagerShould
    {
        private readonly PasswordManager sut = new();
        private static readonly byte[] EmptySalt = Enumerable.Repeat((byte)0, 100).ToArray();
        private static readonly byte[] EmptyHash = Enumerable.Repeat((byte)0, 32).ToArray();


        [Theory]
        [InlineData("password")]
        [InlineData("qwerty123")]
        public void GenerateMeaningfullSaltAndHash(string password)
        {
            var(salt, hash) = sut.GeneratePasswordParts(password);
            salt.Should().HaveCount(100).And.NotBeEquivalentTo(EmptySalt);
            hash.Should().HaveCount(32).And.NotBeEquivalentTo(EmptyHash);

        }

        [Fact]
        public void ReturnTrue_WhenPasswordMatch()
        {
            var password = "qwerty123";
            var (salt, hash) = sut.GeneratePasswordParts(password);

            sut.ComparePassword(password, salt, hash).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenPasswordDoesntMatch()
        {
            var (salt, hash) = sut.GeneratePasswordParts("qwerty123");

            sut.ComparePassword("password", salt, hash).Should().BeFalse();
        }
    }
}