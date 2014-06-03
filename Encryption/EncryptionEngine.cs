using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
    public class EncryptionEngine 
    {
        /// <summary>
        /// Encrypt the provided string with the public key of the supplied <see cref="X509Certificate2"/>.
        /// </summary>
        /// <param name="cert">A <see cref="X509Certificate2"/> used to obtain the public key for encryption.</param>
        /// <param name="plainText">The string to encrypt.</param>
        /// <param name="encryptedValue">A base 64 coded string of the encrypted bytes array.</param>
        /// <returns><b>true</b> if the encryption succeeded; otherwise, <b>false</b>.</returns>
        public bool TryEncrypt(X509Certificate2 cert, string plainText, out string encryptedValue)
        {
            encryptedValue = String.Empty;
            try
            {
                if (String.IsNullOrEmpty(plainText))
                {
                    // We cannot encrypt an empty string
                    return false;
                }
                RSACryptoServiceProvider rsaPublicKey = (RSACryptoServiceProvider)cert.PublicKey.Key;
                ASCIIEncoding asciiConverter = new ASCIIEncoding();
                byte[] plainData = asciiConverter.GetBytes(plainText);
                byte[] encryptedData = rsaPublicKey.Encrypt(plainData, true);
                encryptedValue = Convert.ToBase64String(encryptedData);
            }
            catch (CryptographicException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Decrypt the provided encrypted value string with the private key of the supplied <see cref="X509Certificate2"/>.
        /// </summary>
        /// <param name="cert">A <see cref="X509Certificate2"/> used to obtain the private key for decryption.</param>
        /// <param name="encryptedValue">A base 64 coded string of the encrypted bytes array.</param>
        /// <param name="plainText">The decrypted string.</param>
        /// <returns><b>true</b> if the decryption succeeded; otherwise, <b>false</b>.</returns>
        public bool TryDecrypt(X509Certificate2 cert, string encryptedValue, out string plainText)
        {
            plainText = String.Empty;
            try
            {
                if (cert.PrivateKey == null)
                {
                    // We need a PrivateKey on the certificate for decryption
                    return false;
                }
                if (String.IsNullOrEmpty(encryptedValue))
                {
                    return false;
                }
                RSACryptoServiceProvider rsaPrivateKey = (RSACryptoServiceProvider)cert.PrivateKey;
                ASCIIEncoding asciiConverter = new ASCIIEncoding();
                byte[] encryptedData = Convert.FromBase64String(encryptedValue);
                byte[] plainData = rsaPrivateKey.Decrypt(encryptedData, true);
                plainText = asciiConverter.GetString(plainData);
            }
            catch (CryptographicException)
            {
                return false;
            }
            return true;
        }
    }
}