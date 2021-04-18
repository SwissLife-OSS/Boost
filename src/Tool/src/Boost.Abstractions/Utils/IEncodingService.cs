namespace Boost.Utils
{
    public interface IEncodingService
    {
        string Decode(string value, EncodingType type);
        string Encode(string value, EncodingType type);
    }

    public enum EncodingType
    {
        Base64
    }
}
