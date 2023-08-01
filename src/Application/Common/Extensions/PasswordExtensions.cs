using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Application.Common.Extensions;
public static class PasswordExtensions
{
    public static string EncryptPassword(this string password, string keyString)
    {
        var key = Encoding.UTF8.GetBytes(keyString);

        using (var aesAlg = Aes.Create())
        {
            using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
            {
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(password);
                    }

                    var iv = aesAlg.IV;

                    var decryptedContent = msEncrypt.ToArray();

                    var result = new byte[iv.Length + decryptedContent.Length];

                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }
    }

    public static string DecryptPassword(this string password, string keyString)
    {
        var fullCipher = Convert.FromBase64String(password);

        var iv = new byte[16];
        var cipher = new byte[16];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
        var key = Encoding.UTF8.GetBytes(keyString);

        using (var aesAlg = Aes.Create())
        {
            using (var decryptor = aesAlg.CreateDecryptor(key, iv))
            {
                string result;
                using (var msDecrypt = new MemoryStream(cipher))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return result;
            }
        }
    }

    /// <summary>
    /// Hasing Pattern
    /// </summary>
    /// <param name="pwd">string: UserName + string: Password</param>
    /// <returns></returns>
    public static byte[] ToMD5Password(this string pwd)
    {
        System.Security.Cryptography.MD5 md5 = MD5.Create();
        byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(pwd));
        return dataMd5;
    }

    /// <summary>
    /// Function for Enc string data to MD5 for querystring, param and etc.
    /// </summary>
    /// <param name="strContent">Any string type</param>
    /// <returns></returns>
    public static string ToMD5Hash(this string strContent)
    {
        System.Security.Cryptography.MD5 md5 = MD5.Create();
        byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(strContent));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < dataMd5.Length; i++)
            sb.AppendFormat("{0:x2}", dataMd5[i]);
        return sb.ToString().ToUpper();
    }

    public static string GenerateTempPassword(int length = 12)
    {
        Random random = new Random();
        //string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@&#?".ToLower();
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToLower();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
