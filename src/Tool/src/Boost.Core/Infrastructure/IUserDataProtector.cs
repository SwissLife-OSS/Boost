namespace Boost.Infrastructure
{
    public interface IUserDataProtector
    {
        byte[] Descypt(byte[] data);
        byte[] Encrypt(byte[] data);
    }
}
