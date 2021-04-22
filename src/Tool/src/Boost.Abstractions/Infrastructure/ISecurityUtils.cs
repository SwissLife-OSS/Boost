namespace Boost.Utils
{
    public interface ISecurityUtils
    {
        string? CreateHash(string value, HashAlg alg);
    }

    public enum HashAlg
    {
        Sha256,
        Sha512
    }
}
