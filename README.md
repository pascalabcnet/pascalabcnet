## Building PascalABC.NET in Windows
_RebuildReleaseAndRunTests.bat builds the project in Release-mode, rebuilds the pas-units and runs tests (run with administrative privileges!).

_GenerateAllSetups.bat builds the project in Release-mode, rebuilds the pas-units, runs tests and creates the install package (run with administrative privileges!).

_ReBuildRelease.bat builds the project in Release-mode.

_ReBuildDebug.bat builds the project in Debug-mode.


PascalABC is being developed in Visual Studio Community 2015.

## Building PascalABC.NET in Linux (Ubuntu)
Install Mono (http://www.mono-project.com/docs/getting-started/install/linux/)
```bash
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
sudo apt-get update
sudo apt-get install mono-devel
sudo apt-get install mono-complete
```

Build the project and run tests.
```bash
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ sudo sh _RebuildReleaseAndRunTests.sh
```

## Building PascalABC.NET in MacOS
Download and install Mono from official page. Install the git-client. Run the commands:
```bash
$ git clone https://github.com/pascalabcnet/pascalabcnet
$ cd pascalabcnet
$ sudo sh _RebuildReleaseAndRunTests.sh
```

### Run the PascalABC.NET command line compiler
```bash
$ cd bin
$ mono pabcnetc.exe
или $ mono --debug pabcnetc.exe
```

## Tests
Tests are located in the directory "TestSuite". To run the tests execute the file bin/TestRunner.exe

-----------------------------------------------------------------------------------------------------

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

