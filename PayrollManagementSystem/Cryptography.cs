using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities;

public class Cryptography
{
    private const string EncryptDecryptKey = "fixed29894(";

    public static string Encrypt(string input)
    {
        var encryptString = string.Empty;
        try
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(input);

            var hashMd5 = MD5.Create();
            var keyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(EncryptDecryptKey));
            hashMd5.Clear();
            var tDes = TripleDES.Create();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;

            var cTransform = tDes.CreateEncryptor();
            var resultArray =
                cTransform.TransformFinalBlock(toEncryptArray, 0,
                    toEncryptArray.Length);
            tDes.Clear();

            encryptString = Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        catch (Exception)
        {
            // Do Something
        }

        return encryptString;
    }

    public static string Decrypt(string input)
    {
        var decryptString = string.Empty;
        try
        {
            var toEncryptArray = Convert.FromBase64String(input);

            var hashMd5 = MD5.Create();
            var keyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(EncryptDecryptKey));
            hashMd5.Clear();


            var tDes = TripleDES.Create();
            tDes.Key = keyArray;
            tDes.Mode = CipherMode.ECB;
            tDes.Padding = PaddingMode.PKCS7;

            var cTransform = tDes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tDes.Clear();

            decryptString = Encoding.UTF8.GetString(resultArray);
        }
        catch (Exception)
        {
            // Do Something
        }

        return decryptString;
    }
}