using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MediCloudDrive.Biz
{
    public class MediAES
    {
        private const string version = "20200102";
        private const string cryptKey = "mediprt1";
        private const string cryptIV = "1234567890123456";

        private byte[] AESKEY { get; set; }
        private byte[] AESIV { get; set; }

        public MediAES()
        {
            SetAESKey(Encoding.UTF8.GetBytes(cryptKey + version));
            SetAESIV(Encoding.UTF8.GetBytes(cryptIV));
        }

        public void SetAESKey(string arg_aesKey)
        {
            Debug.Assert(false == string.IsNullOrEmpty(arg_aesKey));

            SetAESKey(Encoding.UTF8.GetBytes(arg_aesKey));
        }

        public void SetAESKey(byte[] arg_bytes)
        {
            Debug.Assert(null != arg_bytes);
            Debug.Assert(16 == arg_bytes.Length);

            AESKEY = arg_bytes;
        }

        public void SetAESIV(string arg_aesiv)
        {
            Debug.Assert(false == string.IsNullOrEmpty(arg_aesiv));

            SetAESIV(Encoding.UTF8.GetBytes(arg_aesiv));
        }

        public void SetAESIV(byte[] arg_bytes)
        {
            Debug.Assert(null != arg_bytes);
            Debug.Assert(16 == arg_bytes.Length);

            AESIV = arg_bytes;
        }

        /// <summary>
        /// 암호화 하기
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string GetEncryptString(string plainText)
        {
            Debug.Assert(null != AESKEY);
            Debug.Assert(null != AESIV);
            Debug.Assert(false == string.IsNullOrEmpty(plainText));

            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = AESKEY,
                IV = AESIV
            };
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        /// <summary>
        /// 암호화 풀기
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string GetDeCryptString(string plainText)
        {
            Debug.Assert(null != AESKEY);
            Debug.Assert(null != AESIV);
            Debug.Assert(false == string.IsNullOrEmpty(plainText));

            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = AESKEY,
                IV = AESIV
            };

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(plainText);

                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Encoding.UTF8.GetString(xBuff);

            return Output;
        }
    }
}