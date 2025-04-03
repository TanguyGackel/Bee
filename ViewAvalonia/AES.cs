using System.Security.Cryptography;
using System.Text;

namespace ViewAvalonia;

public static class AES
{
    public static void trans(byte[] original)
    {
        int i = 0;
        if(original.Length%2 == 0)
            while (i < original.Length)
            {
                original[i] ^= original[i + 1];
                original[i + 1] ^= original[i];
                original[i] ^= original[i+1];

                i +=2 ;
            }
        else
            while (i < original.Length - 1)
            {
                original[i] ^= original[i + 1];
                original[i + 1] ^= original[i];
                original[i] ^= original[i + 1];

                i += 2;
            }
    }
    public static void subs1(byte[] original)
    {
        for(int i = 0; i < original.Length; i++) 
            original[i] ^= 0x01;
    }
    public static void subs2(byte[] original)
    {
        for(int i = 0; i < original.Length; i++) 
            original[i] ^= 0x02;
    }
    
    public static byte[] chiffre(byte[] original, byte[] key, byte[] iv)
    {
        trans(original);
        subs1(original);
        subs2(original);
        
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

        byte[] original;
        
        
        using (Aes aes = Aes.Create()) 
        using (ICryptoTransform decryptor = aes.CreateDecryptor(key,iv))
        using (MemoryStream ms = new MemoryStream(encrypted))
        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (MemoryStream output = new MemoryStream())
        {
            cs.CopyTo(output);
            original = output.ToArray();
        }
        
        trans(original);
        subs1(original);
        subs2(original);
        
        return original;
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
