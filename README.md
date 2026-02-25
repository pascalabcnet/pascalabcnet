# PascalABC.NET
[Russian](#Описание) | [English](#Description)

## Описание
Компилятор языка Pascal с множеством расширений и интеграцией с `.NET`

Проект также включает легковесную IDE

## Разработка
### Зависимости
1. `.NET SDK` \
https://dotnet.microsoft.com
2. `Mono` (требуется под Linux / MacOS / etc) \
https://www.mono-project.com

> [!IMPORTANT]
> Некоторые репозитории пакетов для Linux-дистрибутивов предоставляют урезанную версию `.NET SDK` с отсутствующим `Microsoft.NET.Sdk.WindowsDesktop`. В данном случае воспользуйтесь установкой SDK с помощью [официальных скриптов](https://learn.microsoft.com/dotnet/core/install/linux-scripted-manual) или обратитесь к дистрибьютору пакета

### Подготовка
На Windows следует запустить `./_RegisterHelixNUnit.bat` с правиами администратора (Не требуется, если Вы уже устанавливали PascalABC.NET)
### Сборка
Сборка компилятора и IDE для Windows
```
dotnet build PascalABCNET.sln
```
Сборка компилятора и IDE для Linux / MacOS / etc
```
dotnet build PascalABCNETLinux.sln
```
Сборка только компилятора
```
dotnet build pabcnetc.sln
```

### Тестирование
Тесты расположены в [TestSuite](./TestSuite). Выполнение тестов осуществляется утилитой `./bin/TestRunner.exe`

### Создание установочных пакетов
[_GenerateAllSetups.bat](./_GenerateAllSetups.bat) — Создаёт установочный пакет для Windows

[_GenerateLinuxVersion.bat](./_GenerateLinuxVersion.bat) — Создаёт архив для Linux

## Использование
На Windows
```
cd bin
./PascalABCNET.exe # Запуск IDE
./pabcnetcclear.exe # Консольный компилятор
./pabcnetc.exe # Консольный компилятор с интерактивным режимом
```
На Linux / MacOS / etc
```
cd bin
mono PascalABCNETLinux.exe # Запуск IDE
mono pabcnetcclear.exe # Консольный компилятор
mono pabcnetc.exe # Консольный компилятор с интерактивным режимом
```
___

## Description
Pascal compiler with many extensions and integration with `.NET`

The project also includes a lightweight IDE

## Development
### Dependencies
1. `.NET SDK` \
https://dotnet.microsoft.com
2. `Mono` (required on Linux / MacOS / etc) \
https://www.mono-project.com

> [!IMPORTANT] 
> Some package repositories for Linux distributions provide a stripped-down version `.NET SDK` with missing `Microsoft.NET.Sdk.WindowsDesktop`. In this case, use the [official scripts](https://learn.microsoft.com/dotnet/core/install/linux-scripted-manual)  to install the SDK or contact the package distributor

### Prepare
On Windows, run `./_RegisterHelixNUnit.bat` with admin permissions (Not required if you have already installed PascalABC.NET)
### Building
Build compiler and IDE for Windows
```
dotnet build PascalABCNET.sln
```
Build compiler and IDE for Linux
```
dotnet build PascalABCNETLinux.sln
```
Build only compiler
```
dotnet build pabcnetc.sln
```
### Testing
The tests are located in [TestSuite](./TestSuite). Tests are executed by the utility `./bin/TestRunner.exe`

### Creating installation packages
[_GenerateAllSetups.bat](./_GenerateAllSetups.bat) — Creates an installation package for Windows

[_GenerateLinuxVersion.bat](./_GenerateLinuxVersion.bat) — Creates an archive for Linux

## Using
On Windows
```
cd bin
./PascalABCNET.exe # Run IDE
./pabcnetcclear.exe # Console compiler
./pabcnetc.exe # Interactive console compiler
```
On Linux / MacOS / etc
```
cd bin
mono PascalABCNETLinux.exe # Run IDE
mono pabcnetcclear.exe # Console compiler
mono pabcnetc.exe # Interactive console compiler
```