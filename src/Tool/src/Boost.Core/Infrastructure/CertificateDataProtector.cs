using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Boost.Certificates;
using Boost.Settings;

namespace Boost.Infrastructure
{
    public class CertificateDataProtector : IUserDataProtector
    {
        private readonly ICertificateManager _certificateManager;
        private readonly IUserSettingsManager _userSettingsManager;

        public CertificateDataProtector(
            ICertificateManager certificateManager,
            IUserSettingsManager userSettingsManager)
        {
            _certificateManager = certificateManager;
            _userSettingsManager = userSettingsManager;

            _key = Initialize().GetAwaiter().GetResult();
        }



        private async Task<RSA> Initialize()
        {
            UserSettings? settings = _userSettingsManager.GetAsync(default)
                .GetAwaiter()
                .GetResult();

            if (settings.Encryption.Parameters.ContainsKey("Thumbprint"))
            {
                return GetKeyFromThumbprint(settings);
            }
            else
            {
                X509Certificate2 cert = _certificateManager.CreateSelfSignedCertificate("boost");



                return null;
            }
        }


        private RSA GetKeyFromThumbprint(UserSettings settings)
        {
            var thumb = settings.Encryption.Parameters["Thumbprint"];

            X509Certificate2? cert = _certificateManager.GetFromStore(thumb);

            if (cert is null)
            {
                throw new ApplicationException(
                    $"No certificate found in UserStore with thumbprint: {thumb}");
            }

            RSA? rsa = cert.GetRSAPrivateKey() as RSA;

            if (rsa is null)
            {
                throw new ApplicationException(
                   $"Could not get RSA private key from certificate");
            }

            return rsa;
        }

        private RSA _key;

        public byte[] Protect(byte[] data)
        {
            return _key.Encrypt(
                data,
                RSAEncryptionPadding.OaepSHA256);
        }

        public string Protect(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var cipherData = Protect(data);

            return Encoding.UTF8.GetString(cipherData);
        }

        public byte[] UnProtect(byte[] data)
        {
            return _key.Decrypt(
                data,
                RSAEncryptionPadding.OaepSHA256);
        }

        public string UnProtect(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var plainData = UnProtect(data);

            return Encoding.UTF8.GetString(plainData);
        }
    }
}
