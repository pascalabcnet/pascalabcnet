# PascalABC.NET

PascalABC.NET is a modern Pascal programming language and an integrated
development environment for Microsoft .NET. The repository currently supports
two compiler targets:

- the complete system for .NET Framework 4.0/4.7.1;
- the console compiler for .NET 10.

## Building on Windows

PascalABC.NET is developed with Visual Studio 2026. Install the .NET desktop
development tools and the .NET 10 SDK.

Before the first complete .NET Framework build, run `_RegisterHelixNUnit.bat` as
Administrator. It installs the required HelixToolkit and NUnit assemblies into
the GAC.

### .NET Framework version

```bat
_ReBuildDebug.bat
_ReBuildRelease.bat
```

Build the Release configuration, rebuild Pascal units, and run the complete
.NET Framework test suite (Administrator privileges are required):

```bat
_RebuildReleaseAndRunTests.bat
```

Build and test the complete system, then create the Windows installers:

```bat
_GenerateAllSetups.bat
```

The generated installers are written to the `Release` directory.

### .NET 10 console compiler

Build the console compiler and rebuild its standard units:

```bat
_RebuildStandartModules_net10.bat Release
```

Run the console test suites independently:

```bat
_RunConsoleTests_net40.bat
_RunConsoleTests_net10_clean.bat
```

Create the ready-to-publish .NET 10 console distribution:

```bat
_BuildConsoleNet10Distribution.bat
```

The resulting archive is written to
`Release\PascalABCNET-Console-net10.zip`.

## Linux and Mono

The existing Linux IDE and .NET Framework compiler run on Mono. A short source-build
command is:

```bash
git clone https://github.com/pascalabcnet/pascalabcnet
cd pascalabcnet
sh _RebuildReleaseAndRunTests.sh
```

Run the command-line compiler with:

```bash
cd bin
mono pabcnetc.exe
```

The complete installation guide, including distribution-specific Mono setup,
CP1251 support, and Mono registry configuration, is currently available in
Russian: [Installing PascalABC.NET on Linux](https://pascalabcnet.github.io/mydoc_linux1.html).

---

# PascalABC.NET: сборка проекта

PascalABC.NET — современный язык программирования Паскаль и интегрированная
среда разработки для платформы Microsoft .NET.

Репозиторий поддерживает две цели:

- полную систему для .NET Framework 4.0/4.7.1;
- консольный компилятор для .NET 10.

## Сборка в Windows

Разработка ведётся в Visual Studio 2026. Необходимо установить средства
разработки классических приложений .NET и .NET 10 SDK.

Перед первой полной сборкой версии для .NET Framework запустите `_RegisterHelixNUnit.bat` с правами
администратора. Сценарий устанавливает необходимые сборки HelixToolkit и NUnit
в GAC.

### Версия для .NET Framework

```bat
_ReBuildDebug.bat
_ReBuildRelease.bat
```

Сборка Release, перекомпиляция Pascal-модулей и полный прогон старых тестов
(требуются права администратора):

```bat
_RebuildReleaseAndRunTests.bat
```

Полная сборка, тестирование и создание Windows-установщиков:

```bat
_GenerateAllSetups.bat
```

Готовые установщики сохраняются в каталоге `Release`.

### Консольный компилятор .NET 10

Сборка компилятора и перекомпиляция стандартных модулей:

```bat
_RebuildStandartModules_net10.bat Release
```

Раздельный запуск консольных тестов:

```bat
_RunConsoleTests_net40.bat
_RunConsoleTests_net10_clean.bat
```

Создание готового дистрибутива консольного компилятора .NET 10:

```bat
_BuildConsoleNet10Distribution.bat
```

Результат сохраняется в
`Release\PascalABCNET-Console-net10.zip`.

## Linux и Mono

Существующая Linux IDE и legacy-компилятор работают под Mono. Полная актуальная
инструкция по установке Mono, поддержке кодировки CP1251 и настройке Mono
находится в документе
[«Установка PascalABC.NET под Linux»](https://pascalabcnet.github.io/mydoc_linux1.html).

Сборка исходного проекта и запуск тестов:

```bash
git clone https://github.com/pascalabcnet/pascalabcnet
cd pascalabcnet
sh _RebuildReleaseAndRunTests.sh
```

Запуск компилятора из командной строки:

```bash
cd bin
mono pabcnetc.exe
```

## Тесты

Основной набор тестов находится в каталоге `TestSuite`. Полный старый TestRunner
используется сценариями сборки .NET Framework. Для независимой проверки консольных
компиляторов используйте `_RunConsoleTests_net40.bat` и
`_RunConsoleTests_net10_clean.bat`.
