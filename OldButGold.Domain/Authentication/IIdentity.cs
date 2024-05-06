﻿namespace OldButGold.Domain.Authentication
{
    public interface IIdentity
    {
        Guid UserId { get; }
    }

    internal static class IIdentityExtensions
    {
        public static bool isAuthenticated(this IIdentity identity)
        {
            return identity.UserId != Guid.Empty;
        }
    }
}