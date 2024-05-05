using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.GetTopics;

namespace OldButGold.Domain.Tests.GetTopics
{
    public class GetTopicUseCaseShould
    {
        private readonly Mock<IGetTopicsStorage> storage;
        private readonly ISetup<IGetTopicsStorage, Task<(IEnumerable<Topic> resources, int totalCount)>> getTopicsSetup;
        private readonly GetTopicsUseCase sut;

        public GetTopicUseCaseShould()
        {
            var validator = new Mock<IValidator<GetTopicsQuery>>();
            validator
                .Setup(x => x.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            storage = new Mock<IGetTopicsStorage>();
            getTopicsSetup = storage.Setup(s => s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

            sut = new GetTopicsUseCase(validator.Object, storage.Object);
        }

        [Fact]
        public async Task ReturnTopics_ExtractedFromStorage()
        {
            var foumId = Guid.Parse("bd1aff8e-4351-4d3f-8444-737687ef7bd7");

            var expectedResources = new Topic[] { new() };
            var expectedTotalCount = 6;
            getTopicsSetup.ReturnsAsync((expectedResources ,expectedTotalCount));

            var(actualResources, actualTotalCount) = await sut.Execute(
                new GetTopicsQuery(foumId, 5, 10), CancellationToken.None);

            actualResources.Should().BeEquivalentTo(expectedResources);
            actualTotalCount.Should().Be(expectedTotalCount);

            storage.Verify(s => s.GetTopics(foumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
