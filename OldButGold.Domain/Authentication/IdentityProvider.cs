﻿namespace OldButGold.Forums.Domain.Authentication
{
    internal class IdentityProvider : IIdentityProvider
    {
        public IIdentity Current { get; set; }
    }
}
