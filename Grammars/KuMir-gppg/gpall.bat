cls
..\..\Utils\GPLex_GPPG\gplex.exe /unicode KuMir00.lex
..\..\Utils\GPLex_GPPG\gppg.exe /no-lines /gplex KuMir00.y
copy KuMir00.cs ..\..\Parsers\KuMir-gppg-Parser\KuMir00.cs
copy KuMir00yacc.cs ..\..\Parsers\KuMir-gppg-Parser\KuMir00yacc.cs
