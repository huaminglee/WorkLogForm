using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace OnLineUpdate
{
    public class Securit
    {
        private static string key = "iti@kjqb";
        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DES(string sourceString)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.ASCII.GetBytes(sourceString);
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            crypStream.Write(inputByteArray, 0, inputByteArray.Length);
            crypStream.FlushFinalBlock();
            byte[] result = memStream.ToArray();
            crypStream.Close();
            string resultString = "";
            for (int i = 0; i < result.Length; i++)
            {
                string a = result[i].ToString("X2");
                resultString += a;
            }
            return resultString;
        }

        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DeDES(string encryptString)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = strToToHexByte(encryptString);
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream crypStream = new CryptoStream(memStream, desProvider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write))
                    {
                        crypStream.Write(inputByteArray, 0, inputByteArray.Length);
                        crypStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 字符串转换为byte数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString) 
        { 
            hexString = hexString.Replace(" ", ""); 
            if ((hexString.Length % 2) != 0)     
                hexString += " "; 
            byte[] returnBytes = new byte[hexString.Length / 2]; 
            for (int i = 0; i < returnBytes.Length; i++)        
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); 
            return returnBytes; 
        } 
    }
}
