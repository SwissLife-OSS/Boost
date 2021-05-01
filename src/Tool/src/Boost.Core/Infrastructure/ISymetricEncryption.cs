namespace Boost.Infrastructure
{
    public interface ISymetricEncryption
    {
        byte[] DecryptFile(EncryptedDataEnvelope data, byte[] key);
        EncryptedDataEnvelope EncryptData(byte[] data, byte[] key);
    }
}