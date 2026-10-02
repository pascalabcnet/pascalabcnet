# Единый комплект .NET 10

`scripts/build-net10-runtime.ps1 -Configuration Release` собирает
`Net10Runtime.slnx`: pabcnetc, pabcnetcclear, PABCCompilerController,
ZMQServerPas и языковые компоненты. Общие проекты собираются одним графом;
готовый комплект находится в `bin-net10`. Старую solution и net472 IDE не меняем.

CompileNet10Plugin запускает Controller напрямую из этого каталога.
Для IDE в `bin` настройка: `RuntimeDirectory=..\bin-net10`.
Путь считается относительно EXE IDE, не рабочего каталога Visual Studio.
Для другой IDE можно указать абсолютный путь к тому же комплекту.
После смены INI перезапустите IDE, чтобы завершился прежний Controller.

Сборка проходит в свежем staging-каталоге `.codex-build/net10-runtime`.
Lib/Lng/Ico/Highlighting берутся только из версионируемых исходников bin,
без legacy PCU и бинарников. В bin-net10 пользовательские посторонние файлы
не удаляются. `net10-runtime.manifest.json` содержит состав комплекта и SHA256;
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

Проверка состава/хешей экспорта: `scripts/test-net10-runtime.ps1 -SkipBuild`.
Полный транспорт: `scripts/test-compiler-host.ps1 -Target all`.
Плагин: `CompileNet10.Tests.exe <полный путь к bin-net10>`.
Сборка сбрасывает PCU только соответствующих стандартных исходников;
эти кэши восстанавливаются компилятором либо скриптом пересборки модулей.
