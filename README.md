## Сборка проекта в Windows
_ReBuildRelease.bat собирает проект в Release-режиме  
_ReBuildDebug.bat собирает проект в Debug-режиме  
_GenerateAllSetups.bat собирает инсталлят (запускать с правами администратора).  

Разработка ведется в Visual Studio Community 2015.

## Сборка проекта в Linux (Ubuntu)
1. $ sudo apt-get install mono-complete
2. $ git clone https://github.com/pascalabcnet/pascalabcnet
4. $ cd pascalabcnet
5. $ MONO_IOMAP=case xbuild pabcnetc.sln
### Запуск
1. $ cd bin
2. $ mono pabcnetc.exe
3. или $ mono --debug pabcnetc.exe

## Тесты
Тесты расположены в папке TestSuite. Прогон тестов осуществляется программой bin/TestRunner.exe
