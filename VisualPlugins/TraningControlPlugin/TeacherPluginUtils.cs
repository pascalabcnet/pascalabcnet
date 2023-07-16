using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using System.IO;

namespace VisualPascalABCPlugins
{
    public static class TeacherPluginUtils
    {
        public static string ProcessorId()
        {
            /*var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            var mbsList = mbs.Get();
            foreach (ManagementObject mo in mbsList)
            {
                var pId = mo["ProcessorId"];
                if (pId != null)
                    return pId.ToString();
                else return "AAAAAAAAAAAAAAAA";
            }*/
            return "AAAAAAAAAAAAAAAA";
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

        public static string[] ReadLoginPassFromAuth(string AuthFileFullName)
        {
            // Исключения обрабатываются уровнем выше
            string[] lparr = new string[0];
            var n = 10000;
            var arr = new byte[n];
            int nbytes;
            // Здесь может быть исключение если с файлом или путем проблемы
            using (var fs = new FileStream(AuthFileFullName, FileMode.Open))
            {
                nbytes = fs.Read(arr, 0, n);
            }
            var data = new byte[nbytes];
            System.Array.Copy(arr, data, nbytes);
            // Здесь может быть исключение если ключ неправильный
            var lp = Decrypt(data);
            lparr = lp.Split((char)10);
            return lparr;
        }
    }
}
