using System.IO;

namespace PascalABCCompiler.CoreUtils
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
}
