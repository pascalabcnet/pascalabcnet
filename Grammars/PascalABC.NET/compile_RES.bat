echo Compile: CGT to RES...
..\..\utils\ResXMaker\ResXMaker.exe PascalABC.CGT PascalABCLanguage PascalABCLang.resources
echo.

copy PascalABCLang.resources ..\..\Parsers\PascalABCParser\PascalABCLang.resources
copy PascalABCLang.resources ..\..\Parsers\PascalABCPartParser\PascalABCLang.resources
copy PascalABC.cs ..\..\Parsers\PascalABCParser\pascalabc_lrparser_rules.cs
copy PascalABC_part.cs ..\..\Parsers\PascalABCPartParser\_COPY\pascalabc_lrparser_rules.cs