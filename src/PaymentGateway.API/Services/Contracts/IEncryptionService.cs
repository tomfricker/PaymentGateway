namespace PaymentGateway.API.Services.Contracts
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string credential);

        string Decrypt(byte[] encryptedCredential);
    }
}
