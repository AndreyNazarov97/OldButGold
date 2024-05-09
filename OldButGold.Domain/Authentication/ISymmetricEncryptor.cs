namespace OldButGold.Domain.Authentication
{
    public interface ISymmetricEncryptor
    {
        Task<string> Encrypt(string plainText, byte[] key, CancellationToken cancellationToken);
    }
}
