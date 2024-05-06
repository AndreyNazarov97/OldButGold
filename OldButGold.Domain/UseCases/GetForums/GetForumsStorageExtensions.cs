﻿using OldButGold.Domain.Exceptions;

namespace OldButGold.Domain.UseCases.GetForums
{
    internal static class GetForumsStorageExtensions
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
            if(!await ForumExists(storage, forumId, cancellationToken))
            {
                throw new ForumNotFoundException(forumId);
            }
        }

    }
}
