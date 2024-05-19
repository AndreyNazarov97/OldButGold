using FluentAssertions;
using OldButGold.Forums.Domain.UseCases.CreateTopic;

namespace OldButGold.Forums.Domain.Tests.CreateTopic
{
    public class CreateTopicCommandValidatorShould
    {
        private readonly CreateTopicCommandValidator sut;

        public CreateTopicCommandValidatorShould()
        {
            sut = new CreateTopicCommandValidator();
        }

        [Fact]
        public void ReturnSucces_WhenCommandIsValid()
        {
            var actual = sut.Validate(new CreateTopicCommand(Guid.Parse("462affc0-e684-4e61-ac34-bbf0c68f4ff4"), "Hello"));
            actual.IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidCommand()
        {
            var validCommand = new CreateTopicCommand(Guid.Parse("ba125fd6-08bd-4a0d-bc8f-64a163f2ac4f"), "Hello");

            yield return new[] { validCommand with { ForumId = Guid.Empty } };
            yield return new[] { validCommand with { Title = string.Empty } };
            yield return new[] { validCommand with { Title = "    " } };
            yield return new[] { validCommand with { Title = string.Join("a", Enumerable.Range(0, 100)) } };
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommand))]
        public void ReturnFailure_WhenCommandIsInvalid(CreateTopicCommand command)
        {
            var actual = sut.Validate(command);
            actual.IsValid.Should().BeFalse();
        }
    }
}
