﻿namespace OldButGold.Domain.UseCases.GetTopics
{
    public record GetTopicsQuery(Guid ForumId, int Skip, int Take);
}