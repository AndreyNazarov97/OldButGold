namespace OldButGold.Forums.Domain.Authentication
{
    public interface ISymmetricEncryptor
    {
        Task<string> Encrypt(string plainText, byte[] key, CancellationToken cancellationToken);
    }
}
