using FluentAssertions;
using OldButGold.Domain.UseCases.SignOn;

namespace OldButGold.Domain.Tests.SignOn
{
    public class SignOnCommandValidatorShould
    {
        private readonly SignOnCommandValidator sut = new();

        [Fact]
        public void ReturnSucces_WhenCommandValid() 
        { 
            var validCommand = new SignOnCommand("Login", "Password");

            sut.Validate(validCommand).IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new SignOnCommand("Login", "Password");

            yield return new object[] { validCommand with { Password = string.Empty} };
            yield return new object[] { validCommand with { Password = "          "} };
            yield return new object[] { validCommand with { Login =  string.Empty} };
            yield return new object[] { validCommand with { Login = "          " } };
            yield return new object[] { validCommand with { Login =  string.Join("a", Enumerable.Range(0, 30))} };
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public void ReturnFailure_WhenCommandInvalid(SignOnCommand command)
        {
            sut.Validate(command).IsValid.Should().BeFalse();
        }
    }
}
