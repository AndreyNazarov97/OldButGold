using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.GetForums
{
    internal static class GetTopicsStorageExtensions
    {
        public static async Task<bool> ForumExists(this IGetForumsStorage storage, Guid forumId,
            CancellationToken cancellationToken)
        {
            var forums = await storage.GetForums(cancellationToken);
            return forums.Any(f => f.Id == forumId);
        }

        public static async Task ThrowIfFormNotExist(this IGetForumsStorage storage, Guid forumId,
            CancellationToken cancellationToken)
        {
            if (!await storage.ForumExists(forumId, cancellationToken))
            {
                throw new ForumNotFoundException(forumId);
            }
        }

    }
}
