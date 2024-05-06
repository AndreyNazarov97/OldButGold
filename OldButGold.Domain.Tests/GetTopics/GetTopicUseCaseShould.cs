using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.GetForums;
using OldButGold.Domain.UseCases.GetTopics;

namespace OldButGold.Domain.Tests.GetTopics
{
    public class GetTopicUseCaseShould
    {
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> getForumsSetup;
        private readonly Mock<IGetTopicsStorage> storage;
        private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> getTopicsSetup;
        private readonly GetTopicsUseCase sut;

        public GetTopicUseCaseShould()
        {
            var validator = new Mock<IValidator<GetTopicsQuery>>();
            validator
                .Setup(x => x.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var getForumsStorage = new Mock<IGetForumsStorage>();
            getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            storage = new Mock<IGetTopicsStorage>();
            getTopicsSetup = storage.Setup(s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

            sut = new GetTopicsUseCase(validator.Object, getForumsStorage.Object, storage.Object);
        }

        [Fact]
        public async Task ThrowForumNotFounException_WhenNoForum()
        {
            var forumId = Guid.Parse("e93762dc-6b55-4ffc-a3f4-bfc2f18cb429");

            getForumsSetup.ReturnsAsync(new Forum[] { new(){ Id = Guid.Parse("3d18459f-4ffe-44b6-8bd5-899c4fd600b8"),} });

            var query = new GetTopicsQuery(forumId, 0, 1);
            await sut.Invoking(s => s.Execute(query, CancellationToken.None))
                .Should().ThrowAsync<ForumNotFoundException>();
        }


        [Fact]
        public async Task ReturnTopics_ExtractedFromStorage_WhenForumExist()
        {
            var forumId = Guid.Parse("bd1aff8e-4351-4d3f-8444-737687ef7bd7");

            getForumsSetup.ReturnsAsync(new Forum[] { new() { Id = forumId } });
            var expectedResources = new Topic[] { new() };
            var expectedTotalCount = 6;
            getTopicsSetup.ReturnsAsync((expectedResources ,expectedTotalCount));

            var(actualResources, actualTotalCount) = await sut.Execute(
                new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

            actualResources.Should().BeEquivalentTo(expectedResources);
            actualTotalCount.Should().Be(expectedTotalCount);

            storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
