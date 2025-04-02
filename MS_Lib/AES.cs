using System.Security.Cryptography;
using System.Text;

namespace MS_Lib;

public static class AES
{
    public static byte[] chiffre(byte[] original, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create()) 
            using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv)) 
                using (MemoryStream ms = new MemoryStream())
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(original, 0, original.Length);
                        cs.FlushFinalBlock();
                        return ms.ToArray();
                    }
    }
    
    public static byte[] dechiffre(byte[] encrypted, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create()) 
        using (ICryptoTransform decryptor = aes.CreateDecryptor(key,iv))
        using (MemoryStream ms = new MemoryStream(encrypted))
        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (MemoryStream output = new MemoryStream())
        {
            cs.CopyTo(output);
            return output.ToArray();
        }
    }

    public static byte[] getKey(string s)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
        }
    }

    public static byte[] getIV(int i)
    {
        using (MD5 md5 = MD5.Create())
        {
            return md5.ComputeHash(Encoding.UTF8.GetBytes(i.ToString()));
        }
    }   
    public static byte[] getIV(string i)
    {
        using (MD5 md5 = MD5.Create())
        {
            return md5.ComputeHash(Encoding.UTF8.GetBytes(i));
        }
    }
}
