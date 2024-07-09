using Microsoft.EntityFrameworkCore;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.UseCases.GetTopics;
using OldButGold.Forums.Storage;
using OldButGold.Forums.Storage.Models;

namespace OldButGold.Forums.Storage.Storages;

internal class GetTopicsStorage(
    ForumDbContext dbContext) : IGetTopicsStorage
{
    public async Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {

        var query = dbContext.Topics.Where(t => t.ForumId == forumId);

        var totalCount = await query.CountAsync(cancellationToken);

        var topicReadModels = await dbContext.Database.SqlQuery<TopicListItemReadModel>($@"
            SELECT
                t.""TopicId"" AS ""TopcId"",
                t.""Title"" AS ""Title"",
                t.""CreatedAt"" AS ""CreatedAt"",
                COALESCE(c.TotalCommentsCount, 0) AS ""TotalCommentsCount"",
                c.""CreatedAt"" AS ""LastCommentCreatedAt"",
            	c.""Text""  AS ""CommentText"" 
            FROM ""Topics"" AS t    
            LEFT JOIN (
            	SELECT 
            		""TopicId"",
            		""CommentId"",
            		""CreatedAt"",
            		""Text"" ,
            		COUNT(*) OVER (PARTITION BY ""TopicId"") AS TotalCommentsCount,
            		ROW_NUMBER() OVER(PARTITION BY ""TopicId"" ORDER BY ""CreatedAt"" DESC) AS rn
            	FROM ""Comments""
            ) AS c ON t.""TopicId"" = c.""TopicId"" AND c.rn = 1
            WHERE t.""ForumId"" = {forumId}
            ORDER BY 
            	COALESCE(c.""CreatedAt"", t.""CreatedAt"") DESC
            LIMIT {take}
            OFFSET {skip};
        ").ToArrayAsync(cancellationToken);

        var resources = await query
            .Select(t => new Topic
            {
                Id = t.TopicId,
                ForumId = t.ForumId,
                CreatedAt = t.CreatedAt,
                Title = t.Title,
                UserId = t.UserId,
            })
            .Skip(skip)
            .Take(take)
            .ToArrayAsync(cancellationToken);

        return (resources, totalCount);
    }
}

