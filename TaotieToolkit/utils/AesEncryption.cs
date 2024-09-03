using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class AesEncryption
{
    private Aes aesAlgorithm;

    public AesEncryption(string key)
    {
        aesAlgorithm = Aes.Create();
        aesAlgorithm.Key = Encoding.UTF8.GetBytes(key);
    }

    public byte[][] EncryptStringArray(string[] plainTextArray)
    {
        byte[][] cipherTextWithIvArray = new byte[plainTextArray.Length][];
        for (int i = 0; i < plainTextArray.Length; i++)
        {
            cipherTextWithIvArray[i] = EncryptStringToBytes(plainTextArray[i]);
        }
        return cipherTextWithIvArray;
    }

    public string[] DecryptStringArray(byte[][] cipherTextWithIvArray)
    {
        string[] plainTextArray = new string[cipherTextWithIvArray.Length];
        for (int i = 0; i < cipherTextWithIvArray.Length; i++)
        {
            plainTextArray[i] = DecryptStringFromBytes(cipherTextWithIvArray[i]);
        }
        return plainTextArray;
    }

    private byte[] EncryptStringToBytes(string plainText)
    {
        aesAlgorithm.GenerateIV(); // 生成一个新的随机IV
        ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
            }
            // 将IV添加到密文的开头
            byte[] encrypted = msEncrypt.ToArray();
            byte[] combinedIvCiphertext = new byte[aesAlgorithm.IV.Length + encrypted.Length];
            Array.Copy(aesAlgorithm.IV, 0, combinedIvCiphertext, 0, aesAlgorithm.IV.Length);
            Array.Copy(encrypted, 0, combinedIvCiphertext, aesAlgorithm.IV.Length, encrypted.Length);

            return combinedIvCiphertext;
        }
    }

    private string DecryptStringFromBytes(byte[] combinedIvCiphertext)
    {
        // 提取IV
        byte[] iv = new byte[aesAlgorithm.IV.Length];
        Array.Copy(combinedIvCiphertext, 0, iv, 0, iv.Length);

        // 提取密文
        byte[] cipherText = new byte[combinedIvCiphertext.Length - iv.Length];
        Array.Copy(combinedIvCiphertext, iv.Length, cipherText, 0, cipherText.Length);

        ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, iv);

        using (MemoryStream msDecrypt = new MemoryStream(cipherText))
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
