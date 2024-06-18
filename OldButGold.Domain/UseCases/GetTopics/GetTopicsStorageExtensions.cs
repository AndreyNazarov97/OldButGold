using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.GetTopics
{
    internal static class GetTopicsStorageExtensions
    {
        public static async Task<bool> TopicExists(this IGetTopicsStorage storage, Guid forumId, Guid topicId,
            CancellationToken cancellationToken)
        {
            var (topics, count) = await storage.GetTopics(forumId, 0, Int32.MaxValue , cancellationToken);
            return topics.Any(f => f.Id == topicId);
        }

        public static async Task ThrowIfTopicNotExist(this IGetTopicsStorage storage, Guid forumId, Guid topicId,
            CancellationToken cancellationToken)
        {
            if (!await storage.TopicExists(forumId, topicId, cancellationToken))
            {
                throw new TopicNotFoundException(topicId);
            }
        }

    }
}
