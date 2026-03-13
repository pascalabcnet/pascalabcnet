namespace PascalABCCompiler.CoreUtils
{
    public static class FormatTools
    {

        public static string LanguageAndExtensionsFormatted(string languageName, string[] extensions)
        {
            return string.Format("{0} ({1})", languageName, ExtensionsToString(extensions, "*", ";"));
        }

        public static string ExtensionsToString(string[] Extensions, string Mask, string Delimer)
        {
            string res = "";
            for (int i = 0; i < Extensions.Length; i++)
            {
                res += Mask + Extensions[i];
                if (i != Extensions.Length - 1)
                    res += Delimer;
            }
            return res;
        }
    }
}
