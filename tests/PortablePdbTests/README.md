# Portable PDB и позиционирование ошибок .NET 10

После подготовки bin-net10:

```powershell
dotnet run --project tests/PortablePdbTests/PortablePdbTests.csproj -c Release -- D:\PABC_GIT\bin-net10
dotnet build tests/CompileNet10.Tests/CompileNet10.Tests.csproj -c Release
tests/CompileNet10.Tests/bin/Release/net472/CompileNet10.Tests.exe --runtime-location D:\PABC_GIT\bin-net10
```

Обе проверки включены отдельным modern-этапом в generate-all-setups.ps1;
Framework-only выпуск этот этап не запускает.

Первый тест полностью декодирует точки строк каждого созданного PDB (включая
код стандартных модулей), проверяет возрастающие смещения инструкций,
исходные документы, связь EXE с PDB и реальные исключения ReadLines,
вложенной функции, ветвления, лямбды, отдельного модуля и Pascal DLL.
Проверяются пути с кириллицей/пробелами,
обычное выполнение debug-программы и отсутствие PDB в release-режиме.

Второй тест проводит исключение реального процесса через production-протокол
Net10ProgramRunner/RuntimeExceptionInfo и проверяет настоящий RuntimeErrorBookmark
редактора IDE: строка 2, сплошной OrangeRed-фон, белый текст и удаление маркера.

Причина регрессии: старый генератор может несколько раз вызвать MarkSequencePoint
без генерации инструкции между вызовами. PersistedAssemblyBuilder .NET 10
сохраняет обе точки, кодируя расстояние 0. В Portable PDB ноль после первой
точки обозначает смену документа, из-за чего стандартный MetadataReader
выбрасывает Invalid handle, а CoreCLR не сообщает Pascal-строки в стеке.
Modern-генератор резервирует NOP для каждой отметки и переводит старый hidden
sentinel 0xFFFFFF в Portable PDB HiddenLine 0xFEEFEE. Ветка Framework не меняется.

Второй дефект PersistedAssemblyBuilder: пропуск строк MethodDebugInformation
для внешних/абстрактных методов. PortablePdbAlignment вставляет пустые строки,
сохраняя остальные таблицы. Отдельный синтетический тест проверяет сохранность
документов, всех девяти видов imports, локальных переменных/констант, пустых
и непустых scopes, state machines и custom debug data. Если writer среды уже
возвращает выровненные строки, преобразование не выполняется.

Ручная проверка в оболочке .NET 10: открыть программу из пяти строк
`begin / foreach var s in ReadLines('missing-23.txt') do / begin / end; / end.`,
запустить с отладочной информацией. Ошибка должна появиться в списке ошибок
и подсветить строку 2. После перехода к пользовательскому модулю должна
подсвечиваться строка его файла, не служебный PABCSystem.
