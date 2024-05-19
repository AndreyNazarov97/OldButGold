using FluentAssertions;
using OldButGold.Forums.Domain.UseCases.GetTopics;

namespace OldButGold.Forums.Domain.Tests.GetTopics
{
    public class GetTopicsQueryValidatorShould
    {
        private readonly GetTopicsQueryValidator sut = new();

        [Fact]
        public void ReturnSucces_WhenQueryIsValid()
        {
            var query = new GetTopicsQuery(
                Guid.Parse("186960ac-2f63-4549-ad10-3a94e7f8d7ce"),
                10, 5);
            sut.Validate(query).IsValid.Should().BeTrue();
        }

        public static IEnumerable<object[]> GetInvalidQuery()
        {
            var validQuery = new GetTopicsQuery(Guid.Parse("186960ac-2f63-4549-ad10-3a94e7f8d7ce"), 10, 5);

            yield return new object[] { validQuery with { ForumId = Guid.Empty } };
            yield return new object[] { validQuery with { Skip = -10 } };
            yield return new object[] { validQuery with { Take = -10 } };
        }

        [Theory]
        [MemberData(nameof(GetInvalidQuery))]
        public void ReturnFailure_WhenQueryIsInvalid(GetTopicsQuery query)
        {
            sut.Validate(query).IsValid.Should().BeFalse();
        }
    }
}
