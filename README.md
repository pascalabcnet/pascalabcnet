## Сборка проекта в Windows
_ReBuildRelease.bat собирает проект в Release-режиме  
_ReBuildDebug.bat собирает проект в Debug-режиме  
_GenerateAllSetups.bat собирает инсталлят (запускать с правами администратора).  

Разработка ведется в Visual Studio Community 2015.

## Сборка проекта в Linux (Ubuntu)
$ sudo apt-get install mono-complete
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ MONO_IOMAP=case xbuild pabcnetc.sln
### Запуск
$ cd bin
$ mono pabcnetc.exe
или $ mono --debug pabcnetc.exe

## Тесты
Тесты расположены в папке TestSuite. Прогон тестов осуществляется программой bin/TestRunner.exe
