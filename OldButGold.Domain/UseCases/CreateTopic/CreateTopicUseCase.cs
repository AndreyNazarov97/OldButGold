using Microsoft.EntityFrameworkCore;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.Models;
using OldButGold.Storage;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly ForumDbContext forumDbContext;
        private readonly IGuidFactory guidFactory;
        private readonly IMomentProvider momentProvider;

        public CreateTopicUseCase(
            ForumDbContext forumDbContext,
            IGuidFactory guidFactory,
            IMomentProvider momentProvider)
        {
            this.forumDbContext = forumDbContext;
            this.guidFactory = guidFactory;
            this.momentProvider = momentProvider;
        }
        public async Task<Topic> Execute(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
        {
            var forumExist = await forumDbContext.Forums.AnyAsync(f => f.ForumId == forumId, cancellationToken);

            if (!forumExist)
            {
                throw new ForumNotFoundException(forumId);
            }

            var topicId = guidFactory.Create();
            var topic = await forumDbContext.Topics.AddAsync(new Storage.Topic()
            {
                TopicId = topicId,
                ForumId = forumId,
                UserId = authorId,
                Title = title,
                CreatedAt = momentProvider.Now,
            }, cancellationToken);
            await forumDbContext.SaveChangesAsync(cancellationToken);

            return await forumDbContext.Topics
                .Where(t => t.TopicId == topicId)
                .Select(t => new Topic()
                {
                    Id = t.TopicId,
                    Title = t.Title,
                    CreatedAt = t.CreatedAt,
                    Author = t.Author.Login,
                })
                .FirstAsync(cancellationToken);
        }
    }
}
