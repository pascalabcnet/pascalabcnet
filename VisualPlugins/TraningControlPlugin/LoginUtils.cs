using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using System.IO;

namespace VisualPascalABCPlugins
{
    public static class LoginUtils
    {
        public static string ProcessorId()
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            var mbsList = mbs.Get();
            foreach (ManagementObject mo in mbsList)
            {
                return mo["ProcessorId"].ToString();
            }
            return "";
        }

        public static byte[] Encrypt(string src)
        {
            var ae = Aes.Create();
            var key = Encoding.UTF8.GetBytes(ProcessorId());
            var crypt = ae.CreateEncryptor(key, key);
            byte[] encrypted;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, crypt, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(src);
                }
                encrypted = ms.ToArray();
            }
            return encrypted;
        }

        public static string Decrypt(byte[] data)
        {
            if (data == null)
                return "";
            var ae = Aes.Create();
            var key = Encoding.UTF8.GetBytes(ProcessorId());
            var crypt = ae.CreateDecryptor(key, key);
            string text;
            using (var ms = new MemoryStream(data))
            using (var cs = new CryptoStream(ms, crypt, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                text = sr.ReadToEnd();
            }
            return text;
        }
    }
}
