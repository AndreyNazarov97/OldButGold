using FluentAssertions;
using OldButGold.Forums.Domain.UseCases.CreateForum;

namespace OldButGold.Forums.Domain.Tests.CreateForum
{
    public class CreateForumCommandValidatorShould
    {
        private readonly CreateForumCommandValidator sut = new();

        [Fact]
        public void ReturnsSucces_WhenCommandValid()
        {
            var validCommand = new CreateForumCommand("Title");
            sut.Validate(validCommand).IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new CreateForumCommand("Title");

            yield return new object[] { validCommand with { Title = string.Empty } };
            yield return new object[] { validCommand with { Title = string.Join("a", Enumerable.Range(0, 100)) } };
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public void ReturnsFailure_WhenCommandInvalid(CreateForumCommand command)
        {
            sut.Validate(command).IsValid.Should().BeFalse();
        }


    }
}
