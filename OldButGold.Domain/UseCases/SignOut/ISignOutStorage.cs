﻿namespace OldButGold.Domain.UseCases.SignOut
{
    public interface ISignOutStorage
    {
        Task RemoveSession(Guid sessionId, CancellationToken cancellationToken);
    }
}
