using System.Security.Cryptography.X509Certificates;

namespace Boost.Certificates
{
    public interface ICertificateManager
    {
        void AddToStore(X509Certificate2 certificate, StoreLocation location = StoreLocation.CurrentUser, StoreName storeName = StoreName.My);
        X509Certificate2 CreateSelfSignedCertificate(string subject);
        X509Certificate2? GetFromStore(string thumbprint, StoreLocation location = StoreLocation.CurrentUser);
    }
}