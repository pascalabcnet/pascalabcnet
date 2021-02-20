Чтобы сгенерировать WebHelp надо воспользоваться программой WinCHM Pro

Для этого вначале запустить 01_decodemnemonics.pas чтобы переконвертировать
Table of Contents.hhc в Table of Contents New.hhc
Связано это с тем, что при пользовании HTMLHelp русские буквы в  Table of Contents.hhc
постоянно сбиваются. После этого проверить, что в Table of Contents New.hhc
всё в порядке и переименовать ее в Table of Contents.hhc

Сгенерировать chm, запустив PascalABCNET.hhp 

Запустить WinCHM Pro, открыть chm, создать проект в папке WinChmWebHelp 

Запустить 02_collectKeywords.pas для добавления к Table of Contents.wcp
ключевых слов

Переоткрыть проект в WinCHM Pro

Создать набор HTML

Закинуть по FTP на сайт PascalABC.NET