using FluentAssertions;
using Moq;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.UseCases.CreateForum;

namespace OldButGold.Domain.Tests.Authorization
{
    public class ForumIntentionResolverShould
    {
        private readonly ForumIntentionResolver sut = new();

        [Fact]
        public void ReturnFalse_WhenIntentionNotInEnum()
        {
            var intention = (ForumIntention)(-1);
            sut.isAllowed(new Mock<IIdentity>().Object, intention).Should().BeFalse();
        }

        [Fact]
        public void ReturnFalse_WhenChekingForumCreateIntention_AndUserIsGuest()
        {
            var identity = User.Guest;

            sut.isAllowed(identity, ForumIntention.Create).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenChekingForumCreateIntention_AndUserIsAuthentivated()
        {
            var identity = new User(Guid.Parse("0b8682c0-0ac1-41c7-91aa-b44b1ec23038"));

            sut.isAllowed(identity, ForumIntention.Create).Should().BeTrue();
        }
    }
}
