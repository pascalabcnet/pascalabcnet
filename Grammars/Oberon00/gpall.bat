cls
..\..\Utils\GPLex_GPPG\gplex.exe /unicode oberon00.lex
..\..\Utils\GPLex_GPPG\gppg.exe /no-lines /gplex oberon00.y
copy oberon00.cs ..\..\Parsers\Oberon00Parser\oberon00.cs
copy oberon00yacc.cs ..\..\Parsers\Oberon00Parser\oberon00yacc.cs
