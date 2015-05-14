echo Compile: GRM,TEMPLATE to CS...
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler PascalABC.grm PascalABC.tmpl PascalABC.cs
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler PascalABC.grm PascalABC_part.tmpl PascalABC_part.cs
echo.

echo Compile: CGT to RES...
..\..\utils\ResMaker\bin\ResXMaker.exe PascalABC.CGT PascalABCLanguage PascalABCLang.resources
echo.

copy PascalABCLang.resources ..\..\Parsers\PascalABCParser\PascalABCLang.resources
copy PascalABCLang.resources ..\..\Parsers\PascalABCPartParser\PascalABCLang.resources
copy PascalABC.cs ..\..\Parsers\PascalABCParser\pascalabc_lrparser_rules.cs
copy PascalABC_part.cs ..\..\Parsers\PascalABCPartParser\_COPY\pascalabc_lrparser_rules.cs