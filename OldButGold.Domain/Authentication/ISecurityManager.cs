﻿namespace OldButGold.Domain.Authentication
{
    internal interface ISecurityManager
    {
        bool ComparePassword(string password, string salt, string hash);

        (string Salt, string Hash) GeneratePasswordParts(string password);
    }
}
