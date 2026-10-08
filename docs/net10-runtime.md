# Единый комплект .NET 10

`scripts/build-net10-runtime.ps1 -Configuration Release` собирает
`Net10Runtime.slnx`: pabcnetc, pabcnetcclear, PABCCompilerController,
PABCCompilerWorker, LanguageServer/LanguageServices и языковые компоненты. Общие проекты собираются одним графом;
готовый комплект находится в `bin-net10`. Старую solution и net472 IDE не меняем.

CompileNet10Plugin запускает Controller напрямую из этого каталога.
Для IDE в `bin` настройка: `RuntimeDirectory=..\bin-net10`.
Путь считается относительно EXE IDE, не рабочего каталога Visual Studio.
Для другой IDE можно указать абсолютный путь к тому же комплекту.
После смены INI перезапустите IDE, чтобы завершился прежний Controller.

Сборка проходит в свежем staging-каталоге `.codex-build/net10-runtime`.
Lib/Lng берутся только из версионируемых исходников bin,
без legacy PCU и бинарников. Ресурсы IDE Highlighting/Ico не копируются.
Посторонние пользовательские файлы в bin-net10 не удаляются и в комплект
не включаются. `net10-runtime.manifest.json` содержит состав комплекта и SHA256;
это список для упаковки, а не перечень всех файлов рабочего каталога.

`scripts/export-net10-runtime.ps1 -Destination <папка>` копирует готовые файлы
по манифесту, проверяя хеши, без повторной сборки. `-IncludeCompiledUnits`
добавляет PCU соответствующих стандартных исходников после их пересборки.
Используйте свежий каталог назначения: export не удаляет старые файлы.
Экспорт нужен для дистрибутива или изолированных тестов, но не для плагина.

`build-compiler-host.ps1 -Target net10` — совместимый вход: вызывает единую
сборку и экспортирует её в OutputRoot/net10. net-framework остаётся прежним.
`_RebuildStandartModules_net10.bat` использует ту же сборку и пересоздаёт PCU.
`_BuildConsoleNet10Distribution.bat` упаковывает этот комплект по манифесту.
Windows-инсталлятор, Tooling и VS Code extension на этом этапе не меняются.

LanguageServer работает отдельным процессом, но находится в этом же каталоге:
`dotnet bin-net10/PascalABCNet.LanguageServer.dll --stdio --documentation-language ru`.
Он использует общие Compiler/TreeConverter/NETGenerator и CodeCompletion DLL,
а не второй комплект компилятора. LSP через stdio не меняет JSONL-протокол
Controller. CompileNet10Plugin теперь запускает LanguageServer из того же
RuntimeDirectory и синхронизирует открытые/несохранённые документы в режиме .NET 10.
Список членов после точки, описание при наведении и параметры после скобки/запятой
показаны в штатных окнах IDE. Hover/signature используют тот же LanguageServer,
очередь актуальных несохранённых документов и отдельные пользовательские флаги подсказок.
Ctrl+щелчок ищет реализацию/определение через этот же сервер. Для Pascal открывается
исходник штатным переходом IDE, для CLR — штатная вкладка метаданных, без файлов
на диске. Новых процессов или комплектов DLL для навигации нет.
Дополнительный комплект DLL не создаётся. Подробности жизненного цикла:
[уведомления документов и синхронизация](ide-document-events.md).

Проверка анализа и LSP: `scripts/test-language-server.ps1 -SkipBuild`
(без флага сначала собирает единый runtime). Тестовые entrypoints размещаются
рядом с runtime для использования тех же DLL, но не включаются в manifest.
LSP проверяется и в bin-net10, и в отдельно экспортированном дистрибутиве.

При проверке первого этапа переноса обнаружен отдельный эффект свежего комплекта:
первый прогон CompileNet10.Tests после сброса стандартных PCU может остановиться
на предупреждении для ReadInteger. Повторный прогон после создания PCU проходит.
Тот же результат воспроизведён на контрольной сборке прежней Net10Runtime.slnx
без LanguageServer/LanguageServices; в рамках переноса ядро не исправлялось.

При проверке completion дополнительный `CompilerControllerSmokeTest` выявил ещё
одну независимую проблему: `CheckSourceSnapshotsAsync`, запрос 13 после двух
несохранённых вариантов модуля и restart Worker, возвращает «Неизвестное имя
DiskValue» при возврате к дисковому модулю. Это воспроизведено отдельно на текущем
runtime и на сборке до подключения completion
(`.codex-build/net10-runtime/41e868043cba4f24ad5b4df1717d1456`). Полный
CompileNet10.Tests и проверки LSP проходят; данный сценарий Controller требует
отдельного исследования PCU несохранённых модулей. Ядро, SavePCU и тест не менялись.

Проверка состава/хешей экспорта: `scripts/test-net10-runtime.ps1 -SkipBuild`.
Полный транспорт: `scripts/test-compiler-host.ps1 -Target all`.
Плагин: `CompileNet10.Tests.exe <полный путь к bin-net10>`.
Сборка сбрасывает PCU только соответствующих стандартных исходников;
эти кэши восстанавливаются компилятором либо скриптом пересборки модулей.
