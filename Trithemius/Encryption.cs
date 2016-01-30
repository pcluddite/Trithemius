/*
 * Authors: Brett, Tim Baxendale
 * License: Creative Commons 3.0 (http://creativecommons.org/licenses/by-sa/3.0/legalcode)
 * Original: http://www.codeproject.com/Tips/240428/Work-with-bitmap-faster-with-Csharp
 */

using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

public class Crypto
{
    private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

    /// <summary>
    /// Encrypt the given string using AES.  The string can be decrypted using 
    /// DecryptStringAES().  The sharedSecret parameters must match.
    /// </summary>
    /// <param name="plainText">The text to encrypt.</param>
    /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
    public static byte[] EncryptStringAES(byte[] data, string sharedSecret)
    {
        byte[] outStr = null;                       // Encrypted string to return
        RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

        try
        {
            // generate the key from the shared secret and the salt
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

            // Create a RijndaelManaged object
            aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

            // Create a decrytor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                // prepend the IV
                msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(data, 0, data.Length);
                }
                outStr = msEncrypt.ToArray();
            }
        }
        finally
        {
            // Clear the RijndaelManaged object.
            if (aesAlg != null)
                aesAlg.Clear();
        }

        // Return the encrypted bytes from the memory stream.
        return outStr;
    }

    /// <summary>
    /// Decrypt the given string.  Assumes the string was encrypted using 
    /// EncryptStringAES(), using an identical sharedSecret.
    /// </summary>
    /// <param name="cipherText">The text to decrypt.</param>
    /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
    public static byte[] DecryptStringAES(byte[] bytes, string sharedSecret)
    {
        // Declare the RijndaelManaged object
        // used to decrypt the data.
        RijndaelManaged aesAlg = null;

        // Declare the string used to hold
        // the decrypted text.
        byte[] plaintext = null;

        try
        {
            // generate the key from the shared secret and the salt
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(bytes))
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                // Get the initialization vector from the encrypted stream
                aesAlg.IV = ReadByteArray(msDecrypt);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    plaintext = ReadStream(csDecrypt, bytes.Length);
                }
            }
        }
        finally
        {
            // Clear the RijndaelManaged object.
            if (aesAlg != null)
                aesAlg.Clear();
        }

        return plaintext;
    }

    private static byte[] ReadByteArray(Stream s)
    {
        byte[] rawLength = new byte[sizeof(int)];
        if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
        {
            throw new SystemException("Stream did not contain properly formatted byte array");
        }

        byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
        if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
        {
            throw new SystemException("Did not read byte array properly");
        }

        return buffer;
    }

    private static byte[] ReadStream(Stream stream, int size)
    {
        byte[] data = new byte[size];
        stream.Read(data, 0, data.Length);
        return data;
    }
}