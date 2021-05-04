using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Boost.Certificates;
using Boost.Core.Settings;
using HotChocolate;
using Serilog;

namespace Boost.Infrastructure
{
    public class CertificateDataProtector : IDataProtector
    {
        private readonly ICertificateManager _certificateManager;
        private readonly ISymetricEncryption _symetricEncryption;
        private byte[] _key;
        private const string CertificateSubjectName = "boost";
        private const string ThumbprintParameterName = "Thumbprint";

        public CertificateDataProtector(
            ICertificateManager certificateManager,
            ISymetricEncryption symetricEncryption)
        {
            _certificateManager = certificateManager;
            _symetricEncryption = symetricEncryption;
        }

        public string Name => typeof(CertificateDataProtector).FullName!;

        public EncryptionKeySetting SetupNew()
        {
            Guid id = Guid.NewGuid();
            var password = Guid.NewGuid().ToString("N");

            X509Certificate2 cert = _certificateManager
                .CreateSelfSignedCertificate(
                CertificateSubjectName + $"-{id.ToString("N").Substring(0, 6)}",
                password);

            var certPath = System.IO.Path.Combine(
                SettingsStore.GetUserDirectory("key"), $"{cert.Thumbprint}.pfx");

            File.WriteAllBytes(certPath, cert.Export(X509ContentType.Pfx, password));
            _certificateManager.AddToStore(cert, StoreLocation.CurrentUser);

            var settings = new EncryptionKeySetting
            {
                Id = id,
                Name = Name,
                Parameters = new()
                {
                    [ThumbprintParameterName] = cert.Thumbprint,
                    ["Pass"] = password
                }
            };

            (RSA publicKey, RSA privateKey) rsaKeys = GetRsaKeyFromThumbprint(settings);

            var fullKey = rsaKeys.publicKey.Encrypt(
                Guid.NewGuid().ToByteArray(),
                RSAEncryptionPadding.OaepSHA256);

            _key = fullKey.Take(32).ToArray();

            settings.Parameters.Add("Key", Convert.ToBase64String(fullKey));

            return settings;
        }

        public void Setup(EncryptionKeySetting settings)
        {
            if (_key is null)
            {
                (RSA publicKey, RSA privateKey) rsaKeys = GetRsaKeyFromThumbprint(settings);
                var configKey = settings.Parameters["Key"];
                var fullKey = rsaKeys.privateKey.Decrypt(
                    Convert.FromBase64String(configKey),
                    RSAEncryptionPadding.OaepSHA256);

                _key = fullKey.Take(32).ToArray();
            }
        }

        private (RSA publicKey, RSA privateKey) GetRsaKeyFromThumbprint(
            EncryptionKeySetting settings)
        {
            var thumb = settings.Parameters[ThumbprintParameterName];
            X509Certificate2? cert = null;

            if (settings.Parameters.ContainsKey("Pass"))
            {
                var certPath = System.IO.Path.Combine(
                     SettingsStore.GetUserDirectory("key"), $"{thumb}.pfx");

                cert = new X509Certificate2(
                    File.ReadAllBytes(certPath),
                    settings.Parameters["Pass"]);
            }
            else
            {
                cert = _certificateManager.GetFromStore(
                    thumb,
                    StoreLocation.CurrentUser);
            }


            if (cert is null)
            {
                throw new ApplicationException(
                    $"No certificate found in UserStore with thumbprint: {thumb}");
            }

            RSA? privateKey = cert.GetRSAPrivateKey();
            RSA? publicKey = cert.GetRSAPublicKey();

            if (privateKey is null)
            {
                throw new ApplicationException(
                   $"Could not get RSA private key from certificate");
            }

            if (publicKey is null)
            {
                throw new ApplicationException(
                   $"Could not get RSA public key from certificate");
            }

            return (publicKey, privateKey);
        }

        public byte[] Protect(byte[] data)
        {
            try
            {
                EncryptedDataEnvelope envelope = _symetricEncryption.EncryptData(data, _key);

                return envelope.Data;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CertificateDataProtector.Protect Error");
                throw;
            }
        }

        public byte[] UnProtect(byte[] data)
        {
            EncryptedDataEnvelope envelope = new EncryptedDataEnvelope(data, "AES256");
            var decrypted = _symetricEncryption.DecryptFile(envelope, _key);

            return decrypted;
        }
    }
}
