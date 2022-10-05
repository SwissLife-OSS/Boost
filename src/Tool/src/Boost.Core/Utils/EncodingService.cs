using System;
using System.Text;

namespace Boost.Utils;

public class EncodingService : IEncodingService
{
    public string Encode(string value, EncodingType type)
    {
        string encoded;

        try
        {
            switch (type)
            {
                case EncodingType.Base64:
                    encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
                    break;
                default:
                    throw new ArgumentException("Invalid encoding type", nameof(type));
            }
        }
        catch (Exception ex)
        {
            encoded = ex.Message;
        }

        return encoded;
    }

    public string Decode(string value, EncodingType type)
    {
        string encoded;
        try
        {
            switch (type)
            {
                case EncodingType.Base64:
                    encoded = Encoding.UTF8.GetString(Convert.FromBase64String(value));
                    break;
                default:
                    throw new ArgumentException("Invalid encoding type", nameof(type));
            }
        }
        catch (Exception ex)
        {
            encoded = ex.Message;
        }

        return encoded;
    }
}
