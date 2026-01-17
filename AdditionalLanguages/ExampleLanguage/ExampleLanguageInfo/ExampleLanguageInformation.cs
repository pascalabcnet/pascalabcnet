using PascalABCCompiler.Parsers;
using PascalABCCompiler;

namespace Languages.Example.Frontend.Data
{
    internal class ExampleLanguageInformation : SimpleLanguageInformation
    {
        public override string Name => "ExampleLang";

        public override string Version => "0.0.1";

        public override string Copyright => "Copyright © 2005-2025 by Example Programmer";

        public override string[] FilesExtensions => new string[] { ".exampleLang" };

        public override string[] SystemUnitNames => new string[] { StringConstants.pascalSystemUnitName };

        public override BaseKeywords KeywordsStorage { get; } = new Core.ExampleKeywords();

        public override bool CaseSensitive => true;
    }
}
