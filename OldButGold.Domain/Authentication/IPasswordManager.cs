namespace OldButGold.Domain.Authentication
{
    internal interface IPasswordManager
    {
        (byte[] Salt, byte[] Hash) GeneratePasswordParts(string password);
        bool ComparePassword(string password, byte[] salt, byte[] hash);

    }
}
