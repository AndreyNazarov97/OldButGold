using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using OldButGold.Domain.Models;
using OldButGold.Domain.Monitoring;
using OldButGold.Domain.UseCases.GetForums;

namespace OldButGold.Domain.Tests.GetForums
{
    public class GetForumsUseCaseShould
    {
        private readonly Mock<IGetForumsStorage> storage;
        private readonly ISetup<IGetForumsStorage, Task<IEnumerable<Forum>>> getforumsSetup;
        private readonly GetForumsUseCase sut;

        public GetForumsUseCaseShould()
        {
            storage = new Mock<IGetForumsStorage>();
            getforumsSetup = storage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

            sut = new GetForumsUseCase(storage.Object, new DomainMetrics());
        }

        [Fact]
        public async Task ReturnForums_FromStorage()
        {
            var forums = new Forum[]
            {
                new() { Id = Guid.Parse("1013b491-7fb1-4cb9-a8c4-0729f847218f"), Title = "Test Forum 1"  },
                new() { Id = Guid.Parse("2d61a43b-f927-455a-9706-1c757cf144a9"), Title = "Test Forum 2"  },
            };

            getforumsSetup.ReturnsAsync(forums);

            var actual = await sut.Execute(CancellationToken.None);
            actual.Should().BeSameAs(forums);
            storage.Verify(s => s.GetForums(CancellationToken.None), Times.Once());
            storage.VerifyNoOtherCalls();
        }
    }
}
