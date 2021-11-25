using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace LabCsMongoDB.Data.Encryption
{
     public class Encryptor : IEncryptor
     {
          private readonly SecureString _password;

          public Encryptor(SecureString password)
          {
               _password = password;
          }

          public string Encrypt(string text)
          {
               return Encrypt(text, GetDefaultSalt());
          }

          public string Encrypt(string text, string salt)
          {
               if (text == null)
               {
                    return null;
               }

               RijndaelManaged rijndaelCipher;
               byte[] textData;
               ICryptoTransform encryptor;

               using (rijndaelCipher = new RijndaelManaged())
               {
                    var secretKey = GetSecretKey(salt);
                    textData = Encoding.Unicode.GetBytes(text);
                    encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
               }

               MemoryStream memoryStream;
               byte[] encryptedData;

               using (memoryStream = new MemoryStream())
               {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {

                         cryptoStream.Write(textData, 0, textData.Length);
                         cryptoStream.FlushFinalBlock();
                         encryptedData = memoryStream.ToArray();
                         memoryStream.Close();
                         cryptoStream.Close();
                    }
               }
               var encryptedText = Convert.ToBase64String(encryptedData);
               return encryptedText;
          }

          public string Decrypt(string encryptedText)
          {
               return Decrypt(encryptedText, GetDefaultSalt());
          }

          public string Decrypt(string encryptedText, string salt)
          {
               if (encryptedText == null)
               {
                    return null;
               }

               RijndaelManaged rijndaelCipher;
               byte[] encryptedData;
               ICryptoTransform decryptor;

               using (rijndaelCipher = new RijndaelManaged())
               {
                    var secretKey = GetSecretKey(salt);

                    encryptedData = Convert.FromBase64String(encryptedText);

                    decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
               }

               MemoryStream memoryStream;
               byte[] unencryptedData;
               int decryptedDataLength;

               using (memoryStream = new MemoryStream(encryptedData))
               {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {

                         unencryptedData = new byte[encryptedData.Length];
                         try
                         {
                              decryptedDataLength = cryptoStream.Read(unencryptedData, 0, unencryptedData.Length);
                         }
                         catch
                         {
                              throw new CryptographicException("Unable to decrypt string");
                         }

                         cryptoStream.Close();
                         memoryStream.Close();
                    }
               }
               var decryptedText = Encoding.Unicode.GetString(unencryptedData, 0, decryptedDataLength);
               return decryptedText;
          }

          private PasswordDeriveBytes GetSecretKey(string salt)
          {
               var encodedSalt = Encoding.ASCII.GetBytes(salt);

               var valuePointer = IntPtr.Zero;
               try
               {
                    valuePointer = Marshal.SecureStringToGlobalAllocUnicode(_password);
                    return new PasswordDeriveBytes(Marshal.PtrToStringUni(valuePointer), encodedSalt);
               }
               finally
               {
                    Marshal.ZeroFreeGlobalAllocUnicode(valuePointer);
               }
          }

          private string GetDefaultSalt()
          {
               return _password.Length.ToString(CultureInfo.InvariantCulture);
          }
     }
}
