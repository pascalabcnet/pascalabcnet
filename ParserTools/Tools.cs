// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System.IO;

namespace PascalABCCompiler
{

    public enum SourceFileOperation
    {
        GetText, GetLastWriteTime, Exists, FileEncoding
    }
    public delegate object SourceFilesProviderDelegate(string FileName, SourceFileOperation FileOperation);
    public static class SourceFilesProviders
    {
        public static object DefaultSourceFilesProvider(string FileName, SourceFileOperation FileOperation)
        {
            switch (FileOperation)
            {
                case SourceFileOperation.GetText:
                    if (!File.Exists(FileName)) return null;
                    /*TextReader tr = new StreamReader(file_name, System.Text.Encoding.GetEncoding(1251));
                    //TextReader tr = new StreamReader(file_name, System.Text.Encoding.);
                    string Text = tr.ReadToEnd();
                    tr.Close();*/
                    string Text = FileReader.ReadFileContent(FileName, null);
                    return Text;
                case SourceFileOperation.Exists:
                    return File.Exists(FileName);
                case SourceFileOperation.GetLastWriteTime:
                    return File.GetLastWriteTime(FileName);
            }
            return null;
        }
    }
        
    public class FormatTools
    {

        public static string LanguageAndExtensionsFormatted(string languageName, string[] extensions)
        {
            return string.Format("{0} ({1})", languageName, ExtensionsToString(extensions, "*", ";"));
        }

        public static string ExtensionsToString(string[] Extensions,string Mask,string Delimer)
        {
            string res = "";
            for (int i = 0; i < Extensions.Length; i++)
            {
                res += Mask+Extensions[i];
                if (i != Extensions.Length - 1)
                    res += Delimer;
            }
            return res;
        }
        public static string ObjectsToString(object[] Objects, string Delimer)
        {
            string res = "";
            for (int i = 0; i < Objects.Length; i++)
            {
                res += Objects[i].ToString();
                if (i != Objects.Length - 1)
                    res += Delimer;
            }
            return res;
        }
    }
}

