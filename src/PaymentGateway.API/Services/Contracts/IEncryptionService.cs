namespace PaymentGateway.API.Services.Contracts
{
    public interface IEncryptionService
    {
        byte[] Encrypt(string str);

        string Decrypt(byte[] encryptedString);
    }
}
