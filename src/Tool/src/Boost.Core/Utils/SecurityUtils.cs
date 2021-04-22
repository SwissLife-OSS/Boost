using IdentityModel;

namespace Boost.Utils
{
    public class SecurityUtils : ISecurityUtils
    {
        public string? CreateHash(string value, HashAlg alg)
        {
            string? result = null;

            switch (alg)
            {
                case HashAlg.Sha256:
                    result = value.ToSha256();
                    break;
                case HashAlg.Sha512:
                    result = value.ToSha512();
                    break;
            }

            return result;
        }
    }


}
