using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.UseCases.CreateComment;

namespace OldButGold.Forums.Storage.Storages
{
    internal class CreateCommentStorage(
        IMapper mapper,
        ForumDbContext dbContext,
        IGuidFactory guidFactory,
        IMomentProvider momentProvider)
        : ICreateCommentStorage
    {
        public Task<Topic?> FindTopic(Guid topicId, CancellationToken cancellationToken)
        {
            return dbContext.Topics
                .Where(x => x.TopicId == topicId)
                .ProjectTo<Topic>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task<Comment> CreateComment(Guid topicId, Guid userId, string text, CancellationToken cancellationToken)
        {
            var commentId = guidFactory.Create();

            await dbContext.Comments.AddAsync(new Entities.Comment()
            {
                CommentId = commentId,
                TopicId = topicId,
                UserId = userId,
                CreatedAt = momentProvider.Now,
                Text = text,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return await dbContext.Comments
                .Where(c => c.CommentId == commentId)
                .ProjectTo<Comment>(mapper.ConfigurationProvider)
                .FirstAsync(cancellationToken);
        }
    }
}
