using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;
using UnityEngine;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace GameNetwork
{
    public class UEncrypt
    {
        //private static String RSApublickey = ;
        private static char[] constant =
        {
            //'0','1','2','3','4','5','6','7','8','9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };

        public static string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(52);
            System.Random rd = new System.Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(52)]);
            }

            return newRandom.ToString();
        }

        public static byte[] ByteRandom(int length)
        {
            byte[] by = new byte[length];
            System.Random random = new System.Random();

            random.NextBytes(by);

            return by;
        }

        /// <summary>
        /// DES 的 私密
        /// </summary>
        public static string DESprivateKey = "";

        /// <summary>
        /// DES 的 公密
        /// </summary>
        private string DESpublicKey = "";

        /// <summary>
        /// 生成RSA私钥 公钥
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        public static void RSAGenerateKey(ref string privateKey, ref string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            privateKey = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
        }

        /// <summary>
        /// pem公钥内容转xml
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string Convert2XMLPublicKey(string pem)
        {
            RsaKeyParameters publicKeyParam =
                (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(pem));
            string XML = string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
            return XML;
        }

        /// <summary>
        /// 用RSA公钥 加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(string key) //, string publicKey)
        {
            string pem =
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAzIRWa+XxDQMtiDvveWR9pKVBiSLFo6rdyFrH7TcwcGNoz+D7pULfbCs9WJp3Oke6pwE7xGPAbQ5qmOyIBK23EKZLYYVl/bXIXQILjfem0LL9ameIWhewdfQb3QDeEMmL+KiI3DFSf+ofrzKSJIyWswnFqQXKPCF4Qs0Qpwaq+0XAlHkeIqwpqVNK3mWRcoby0JsbuH1S5E+SigCh5zksokjAqnuGKhF7+3HCaWhTrmEhiMJpYF4Bk6WS2WdihORg5lbLK+w3WtpWdYBmdVxTFqEih1eXmA51fOptLy4Le6i8aOiIgUvtdoQLMbQraietmRUfzMIho/cxN6dpPy6gTwIDAQAB";
            byte[] data = Encoding.UTF8.GetBytes(key);
            //string publicKey = "<RSAKeyValue><Modulus>3lfr8yPiGs+EAeH5Oprx5idhbXJ9Due4+cF9bxNJSQL3fv6pMdCmcQaUc4mNxljHfst8rQfpKCJq1ZYylizBfiQ6Z5HrAQMmByKSOkg8qJFjeLM1PvL8NDRW63OPecrN9ODV38Dy9oG3UoQO+LH0MVxKGtdVKZOezJdszxdQXnPifrXVqa1Xkr3CyvwShHwnk2OkE0avZhc29tjaewm27RxosIj4C5O8MZkfammbpukp2TWeI+4ZHoKNk8AedsOHmxqnCN9NC6dYkIqZTM8sH0PVS8Mpo0AS/6oaPhEr8qicX07cXI+FZB+yV36lDKMcWYH6lbmumoBSMac1Z/akow==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string publicKey = Convert2XMLPublicKey(pem);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            byte[] encryptData = rsa.Encrypt(data, false);
            return encryptData;
        }

        /// <summary>
        /// 用RSA私钥 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte[] RSADecrypt(byte[] data, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            byte[] decryptData = rsa.Decrypt(data, false);
            return decryptData;
        }

        private static string desKey;
        private static DESCryptoServiceProvider desProvider;

        public static void SetDesKey(string key)
        {
            desKey = key;
            CipherMode cipher = CipherMode.ECB;
            PaddingMode padding = PaddingMode.PKCS7;
            desProvider = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(desKey),
                //IV = Encoding.UTF8.GetBytes(desrgbKey),
                Mode = cipher,
                Padding = padding
            };
        }

        public static byte[] DESEncryptByBytes(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream =
                new CryptoStream(memoryStream, desProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        public static byte[] DESDecryptByBytes(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream =
                new CryptoStream(memoryStream, desProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="desrgbKey"></param>
        /// <param name="desrgbIV"></param>
        /// <returns></returns>
        public static byte[] DESEncrypt(byte[] data, string desrgbKey)
        {
            CipherMode cipher = CipherMode.ECB;
            PaddingMode padding = PaddingMode.PKCS7;
            var des = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(desrgbKey),
                //IV = Encoding.UTF8.GetBytes(desrgbKey),
                Mode = cipher,
                Padding = padding
            };

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptStr">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] encryptStr, byte[] key)
        {
            //byte[] keyArray = key;// UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = encryptStr; // UTF8Encoding.UTF8.GetBytes(encryptStr);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = key; // keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray; // Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] decryptStr, byte[] key)
        {
            //byte[] keyArray = key;// UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = decryptStr; // Convert.FromBase64String(decryptStr);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = key; // keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray; //UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string MD5Encrypt(string str)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string EncodeLuaPath(string fileName)
        {
            if (!fileName.EndsWith(".lua"))
            {
                fileName += ".lua";
            }

            return UEncrypt.MD5Encrypt(fileName) + ".lua";
        }

        public static string DecodeLuaPath(string fileName)
        {
            int num = fileName.LastIndexOf("/");
            string fileName2 = (num <= 0) ? fileName : fileName.Substring(num + 1);
            string str = UEncrypt.EncodeLuaPath(fileName2);
            string str2 = (num <= 0) ? string.Empty : fileName.Substring(0, num + 1);
            return str2 + str;
        }
    }
}