namespace OldButGold.Forums.Domain.Authentication
{
    public interface ISymmetricDecryptor
    {
        public Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken);
    }
}
