using FluentAssertions;
using Moq;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.UseCases.CreateForum;
using System.Net;

namespace OldButGold.Domain.Tests.Authorization
{
    public class IntentionManagerShould
    {
        private Mock<IIdentityProvider> identityProvider;

        public IntentionManagerShould()
        {
            identityProvider = new Mock<IIdentityProvider>();
        }
        [Fact]
        public void ReturnFalse_WhenNoMatchingResolver()
        {
            var resolvers = new IIntentionResolver[]
            {
                new Mock<IIntentionResolver<DomainErrorCode>>().Object,
                new Mock<IIntentionResolver<HttpStatusCode>>().Object,

            };
                
            var sut = new IntentionManager(resolvers, identityProvider.Object);
            sut.IsAllowed(ForumIntention.Create).Should().BeFalse();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void ReturnMatchingResolverResult(bool expectedResolverResult, bool expected)
        {
            var resolver = new Mock<IIntentionResolver<ForumIntention>>();
            resolver
                .Setup(r => r.isAllowed(It.IsAny<IIdentity>(), It.IsAny<ForumIntention>()))
                .Returns(expectedResolverResult);

            identityProvider.Setup(p => p.Current)
                .Returns(new User(Guid.Parse("675359ce-239a-4f64-b041-6c491e520207"), Guid.Empty));

            var sut = new IntentionManager(
                new IIntentionResolver[] {resolver.Object}, 
                identityProvider.Object);

            sut.IsAllowed(ForumIntention.Create).Should().Be(expected);
        }
    }
}
