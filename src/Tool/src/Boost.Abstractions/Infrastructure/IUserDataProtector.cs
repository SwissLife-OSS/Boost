using System;
using System.Collections.Generic;
using Boost.Settings;

namespace Boost.Infrastructure
{
    public interface IUserDataProtector
    {
        byte[] UnProtect(byte[] data);
        byte[] Protect(byte[] data);
        string Protect(string value);
        string UnProtect(string value);
    }

    public record KeyContext(Guid Id, IDictionary<string, string> Parameters);

    public class DataProtectorKeyRing
    {
        public Guid? ActiveKeyId { get; set; }

        public IList<EncryptionKeySetting> Protectors { get; set; } = new List<EncryptionKeySetting>();
    }

    public class EncryptionKeySetting
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
            = new Dictionary<string, string>();
    }

    public enum EnryptionType
    {
        None,
        X509Certificate
    }

    public record ProtectedData(byte[] Data, Guid KeyId);
}
