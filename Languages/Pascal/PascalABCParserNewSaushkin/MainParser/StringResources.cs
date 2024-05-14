using PascalABCCompiler.ParserTools;

namespace Languages.Pascal.Frontend
{
    internal static class StringResources
    {
        private const string prefix = "PASCALABCPARSER_";
        public static string Get(string id)
        {
            return BaseStringResources.Get(id, prefix);
        }
    }
}
