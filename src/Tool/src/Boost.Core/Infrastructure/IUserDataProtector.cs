namespace Boost.Infrastructure
{
    public interface IUserDataProtector
    {
        byte[] UnProtect(byte[] data);
        byte[] Protect(byte[] data);
        string Protect(string value);
        string UnProtect(string value);
    }
}
