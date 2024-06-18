using FluentAssertions;
using OldButGold.Forums.Domain.UseCases.SignIn;

namespace OldButGold.Forums.Domain.Tests.SignIn
{
    public class SignInCommandValidatorShould
    {
        private readonly SignInCommandValidator sut = new();

        [Fact]
        public void ReturnSucces_WhenCommandValid()
        {
            var validCommand = new SignInCommand("Login", "Password");

            sut.Validate(validCommand).IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new SignInCommand("Login", "Password");

            yield return [validCommand with { Password = string.Empty }];
            yield return [validCommand with { Password = "          " }];
            yield return [validCommand with { Login = string.Empty }];
            yield return [validCommand with { Login = "          " }];
            yield return [validCommand with { Login = string.Join("a", Enumerable.Range(0, 30)) }];
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public void ReturnFailure_WhenCommandInvalid(SignInCommand command)
        {
            sut.Validate(command).IsValid.Should().BeFalse();
        }
    }
}
