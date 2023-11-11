using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace App.Foundation.Common
{
    public class Encryption
    {
        static readonly string DataEncryptionKey = "V2VBcmVUaGVCZXN0";
        static readonly string EncryptionKey = "MAKV2SPBNI99212";
        static readonly string SaltValue = "3.14157284><;VeriDoc?+|-";
        static readonly string PassPhrase = "<Veri><Doc><WebApp>..!";
        static readonly string HashAlgorithm = "SHA1";
        static readonly string PasswordKey = "veriD@c*%(i7q1d!";
        private const int PasswordIterations = 2;
        private const int KeySize = 128;


        public static string EncryptData(string textData)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;
            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] passBytes = Encoding.UTF8.GetBytes(DataEncryptionKey);
            byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length) len = EncryptionkeyBytes.Length;
            Array.Copy(passBytes, EncryptionkeyBytes, len);

            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;

            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            string b64 = Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
            return HttpUtility.UrlEncode(b64);
        }

        public static string DecryptData(string EncryptedText)
        {
            var EncryptedDecoded = HttpUtility.UrlDecode(EncryptedText);
            EncryptedDecoded = EncryptedDecoded.Replace(" ", "+");
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;

            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] encryptedTextByte = Convert.FromBase64String(EncryptedDecoded);
            byte[] passBytes = Encoding.UTF8.GetBytes(DataEncryptionKey);
            byte[] EncryptionkeyBytes = new byte[0x10];
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(TextByte);
        }

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string EncryptASCII(string strText)
        {
            string tex = String.Empty;

            //First get ASCII Code of every letter in byte array
            byte[] asciiBytes = Encoding.ASCII.GetBytes(strText);

            //For every integer of letter's ASCII convert to own letterFormat by getMetaValue(), seperated by 'x' then concat in a single string
            foreach (byte b in asciiBytes) { foreach (char c in b.ToString()) { tex += GetMetaValue(c); } tex += "x"; }

            //Encode it to Base64
            byte[] EncodeAsBytes = Encoding.UTF8.GetBytes(tex.Substring(0, tex.Length - 1));
            tex = System.Convert.ToBase64String(EncodeAsBytes);

            //Finally return that string
            return tex;
        }

        public static string DecryptASCII(string strText)
        {
            string tex = String.Empty, ascii;
            //First decode string from Base64
            byte[] DecodeAsBytes = System.Convert.FromBase64String(strText);
            string stringData = Encoding.UTF8.GetString(DecodeAsBytes);

            //Split up string by seperator 'x'
            string[] splitedChar = stringData.Split('x');

            //For every char(ASCII) getting its integerFormat by getMetaValue() then concat in a single string
            foreach (string asc in splitedChar)
            {
                ascii = String.Empty;
                foreach (char c in asc) ascii += GetMetaValue(c);
                tex += (char)Convert.ToInt32(ascii);
            }

            //Finally return that string
            return tex;
        }

        private static char GetMetaValue(char data)
        {
            if (char.IsDigit(data))
            {
                if (data == '1') return 'm'; if (data == '7') return 'r'; if (data == '4') return 'i'; if (data == '0') return 'c'; if (data == '3') return 't';
                if (data == '2') return 'e'; if (data == '5') return 'a'; if (data == '9') return 'z'; if (data == '6') return 'b'; if (data == '8') return 'u'; else return '0';
            }
            else
            {
                if (data == 'm') return '1'; if (data == 'r') return '7'; if (data == 'i') return '4'; if (data == 'c') return '0'; if (data == 't') return '3';
                if (data == 'e') return '2'; if (data == 'a') return '5'; if (data == 'z') return '9'; if (data == 'b') return '6'; if (data == 'u') return '8'; else return '0';
            }
        }

        public static string EncryptPassword(string stringToEncrypt)
        {
            return Encrypt(stringToEncrypt, PasswordKey);
        }

        /// <summary>
        /// Encrypts specified plain text string using <see cref="Rijndael"/> symmetric key algorithm 
        /// </summary>
        /// <param name="stringToEncrypt">Plain text value to be Encrypted.</param>
        /// <param name="key">key for encryption</param>
        /// <returns>Returns Encryped string value.</returns>
        private static string Encrypt(string stringToEncrypt, string key)
        {
            ICryptoTransform encryptor;
            byte[] initVectorBytes;
            byte[] saltValueBytes;
            byte[] plainTextBytes;
            byte[] keyBytes;
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            byte[] byteEncryptedText;
            string strEncryptedText;

            try
            {
                //// Convert strings into byte arrays.
                //// Assumes that string only contain ASCII codes.
                //// If strings includes Unicode character, use Unicode, UTF7, or UTF8 encoding.
                initVectorBytes = Encoding.ASCII.GetBytes(key);

                saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);

                //// Convert plain text into a byte array.
                //// Assumes that plaintext contains UTF8-encoded characters.
                plainTextBytes = Encoding.UTF8.GetBytes(stringToEncrypt);

                //// First, We must create a password, from which the key will be derived.
                //// This password will be generated from the specified passphrase and 
                //// salt value. The password will be created using the specified hash
                //// algorithm. Password creation can be done in several iterations.

                using (var password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations))
                {
                    //// Use the password to generate pseudo-random bytes for the encryption
                    //// key. Specify the size of the key in bytes (instead of bits).
                    keyBytes = password.GetBytes(KeySize / 8);
                }

                //// Create uninitialized Rijndael encryption object
                using (var symmetricKey = new RijndaelManaged())
                {
                    //// It is reasonable to set encryption mode to Ciper Block Chaining
                    //// (CBC). Use default options for other symmetric key parameters.
                    symmetricKey.Mode = CipherMode.CBC;

                    //// Generate enryptor from the existing key bytes and initialization 
                    //// vector. Key size will be defined based on the number of the key
                    //// bytes.
                    encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                    //// Define memory stream which will be used to hold encrypted data.
                    memoryStream = new MemoryStream();

                    //// Define cryptography stream (always use Write mode for encryption).
                    cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                    //// start encryption.
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                    //// Finish encryption.
                    cryptoStream.FlushFinalBlock();

                    //// Convert our encrypted data from a memory stream into a byte array
                    byteEncryptedText = memoryStream.ToArray();

                    //// Close both streams.
                    memoryStream.Close();
                    cryptoStream.Close();

                    //// Convert encryped data into a base64-encoded string.
                    strEncryptedText = Convert.ToBase64String(byteEncryptedText);

                    //// Return encrypted string.
                    return strEncryptedText;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
