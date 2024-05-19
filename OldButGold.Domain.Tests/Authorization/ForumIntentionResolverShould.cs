using FluentAssertions;
using Moq;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.UseCases.CreateForum;

namespace OldButGold.Forums.Domain.Tests.Authorization
{
    public class ForumIntentionResolverShould
    {
        private readonly ForumIntentionResolver sut = new();

        [Fact]
        public void ReturnFalse_WhenIntentionNotInEnum()
        {
            var intention = (ForumIntention)(-1);
            sut.IsAllowed(new Mock<IIdentity>().Object, intention).Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_WhenChekingForumCreateIntention_AndUserIsGuest()
        {
            var identity = User.Guest;

            sut.IsAllowed(identity, ForumIntention.Create).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenChekingForumCreateIntention_AndUserIsAuthentivated()
        {
            var identity = new User(Guid.Parse("0b8682c0-0ac1-41c7-91aa-b44b1ec23038"), Guid.Empty);

            sut.IsAllowed(identity, ForumIntention.Create).Should().BeTrue();
        }
    }
}
