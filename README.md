## Сборка проекта в Windows
_RebuildReleaseAndRunTests.bat собирает проект в Release-режиме, перекомпилирует pas-модули и прогоняет все тесты (запускать с правами администратора).
_GenerateAllSetups.bat собирает инсталлят (запускать с правами администратора).
_ReBuildRelease.bat собирает проект в Release-режиме.
_ReBuildDebug.bat собирает проект в Debug-режиме.


Разработка ведется в Visual Studio Community 2015.

## Сборка проекта в Linux (Ubuntu)
```bash
$ sudo apt-get install mono-complete
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ MONO_IOMAP=case xbuild pabcnetc.sln
```

## Сборка проекта в MacOS
Скачайте и установите Mono с официального сайта. При необходимости установите git-клиент. Далее выполните команды
```bash
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ MONO_IOMAP=case xbuild pabcnetc.sln
```

### Запуск
```bash
$ cd bin
$ mono pabcnetc.exe
или $ mono --debug pabcnetc.exe
```

## Тесты
Тесты расположены в папке TestSuite. Прогон тестов осуществляется программой bin/TestRunner.exe
