## Сборка проекта в Windows
_RebuildReleaseAndRunTests.bat собирает проект в Release-режиме, перекомпилирует pas-модули и прогоняет все тесты (запускать с правами администратора).

_GenerateAllSetups.bat собирает инсталлят (запускать с правами администратора).

_ReBuildRelease.bat собирает проект в Release-режиме.

_ReBuildDebug.bat собирает проект в Debug-режиме.


Разработка ведется в Visual Studio Community 2015.

## Сборка проекта в Linux (Ubuntu)
Установка Mono (http://www.mono-project.com/docs/getting-started/install/linux/)
```bash
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
sudo apt-get update
sudo apt-get install mono-devel
sudo apt-get install mono-complete
```

Сборка проекта и выполение тестов
```bash
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ sudo sh _RebuildReleaseAndRunTests.sh
```

## Сборка проекта в MacOS
Скачайте и установите Mono с официального сайта. При необходимости установите git-клиент. Далее выполните команды
```bash
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ sudo sh _RebuildReleaseAndRunTests.sh
```

### Запуск
```bash
$ cd bin
$ mono pabcnetc.exe
или $ mono --debug pabcnetc.exe
```

## Тесты
Тесты расположены в папке TestSuite. Прогон тестов осуществляется программой bin/TestRunner.exe
