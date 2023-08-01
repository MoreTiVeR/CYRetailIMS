
using System.Security.Cryptography;
using System.Text;
using CYRetailIMS.Application.Common.Cryptography;

namespace CYRetailIMS.Infrastructure.Common.Cryptography;
public class EncryptionString : IEncryptionString
{
    public string HashWithSHA256(string key)
    {
        StringBuilder Sb = new StringBuilder();
        using var hash = SHA256.Create();
        Encoding enc = Encoding.UTF8;
        byte[] result = hash.ComputeHash(enc.GetBytes(key));
        foreach (byte b in result)
        {
            Sb.Append(b.ToString("x2"));
        }
        return Sb.ToString();
    }
}
