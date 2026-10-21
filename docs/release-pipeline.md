# Подготовка и упаковка выпуска

Общий вход — `_GenerateAllSetups.bat`. Он вызывает
`scripts/generate-all-setups.ps1` и возвращает ненулевой код при ошибке.
Запуск не зависит от текущего каталога.

Отдельный вход для обычного выпуска — `_GenerateNetFrameworkSetup.bat`.
Он вызывает тот же сценарий с `-Target NetFramework`: сохраняет подготовку
версии, legacy IDE, оба прохода модулей и подпись/GAC PABCRtl, Pascal/SPython
тесты и упаковку прежних установщиков/консольного ZIP. Не собирает и не проверяет
`bin-net10`, не требует modern-плагина, не запускает modern-тесты и упаковку.
Cross-target runner выполняется только для `net472`. Проверки редактора
выполняются на legacy DLL и не запускают runtime .NET 10.
Параметры `-Stage`, `-NoVersionIncrement` и `-Plan` доступны в обоих входах.

```powershell
.\_GenerateNetFrameworkSetup.bat -Plan
.\_GenerateNetFrameworkSetup.bat -NoVersionIncrement
.\_GenerateNetFrameworkSetup.bat -Stage Package
```

## Этапы

- `Prepare`: примеры и версия; legacy IDE/компилятор; legacy-модули,
  создание и подпись PABCRtl, финальные legacy-модули; единый modern-комплект;
  плагин IDE без повторной сборки runtime; modern-PCU без повторной сборки runtime.
- `Test`: проверка подготовленного комплекта; сборка только тестовых entrypoint;
  короткая проверка Enter и авто-вставки `begin/end` в обоих режимах на DLL из `bin`;
  прежние шесть проходов Pascal/SPython и cross-target проверки обоих target.
  Legacy TestRunner получает флаг без GUI-диалогов.
- `Package`: проверка подготовленного комплекта, существующие NSIS-установщики
  и console ZIP, modern console ZIP по манифесту с подготовленными PCU,
  двуцелевой `PascalABCNETSetupNet10.exe`.
  Производственные DLL и модули не пересобираются.
- `All` (по умолчанию): все три этапа в указанном порядке; при ошибке
  последующие этапы не выполняются.

Расширение VS Code не собирается и не включается в эти пакеты.
Net10Pack дополнительно содержит полную копию `ReleaseGenerators\PT4` в `Net10\PT4`:
модуль .NET 10 ищет нативные DLL относительно своего системного каталога.
Обычная копия `$INSTDIR\PT4` остаётся на месте.

```powershell
# Только показать порядок, без записи файлов, сборки, изменения версии и GAC:
.\_GenerateAllSetups.bat -Plan

# Полный выпуск с однократным увеличением REVISION:
.\_GenerateAllSetups.bat

# Пробный выпуск без увеличения REVISION:
.\_GenerateAllSetups.bat -NoVersionIncrement

# Выполнить этапы отдельно:
.\_GenerateAllSetups.bat -Stage Prepare -NoVersionIncrement
.\_GenerateAllSetups.bat -Stage Test
.\_GenerateAllSetups.bat -Stage Package
```

`Prepare` сохраняет прежнюю регистрацию PABCRtl в GAC: нужны соответствующие
права, IDE перед пересборкой нужно закрыть. Два legacy-прохода до и после
PABCRtl сохранены намеренно; их сокращение — отдельное исследование.
`NoVersionIncrement` не увеличивает REVISION, но обновляет файлы из шаблонов.
IDE, PluginsSupport и CodeTemplatesPlugin используют готовую DLL редактора из
`Libraries`. После изменения его исходников DLL обновляется явно Release-сборкой;
обычная сборка IDE редактор не пересобирает. Тесты выпуска проверяют Enter,
загрузку файлов и движение курсора с подсветкой в обоих режимах на DLL из `bin`.

При раздельном запуске `Package` пользователь должен предварительно выполнить
`Test`: режим упаковки сам тесты не запускает. После изменения исходников
нужно снова выполнять `Prepare`; проверка манифеста подтверждает целостность
готовых modern-файлов, а не актуальность всех исходников репозитория.

## Отдельные команды и совместимость

Без новых флагов самостоятельные команды по-прежнему подготавливают свои
зависимости. Общий сценарий явно отключает повторную подготовку:

```powershell
.\_RebuildStandartModules_net10.bat Release --skip-build
.\_BuildConsoleNet10Distribution.bat --skip-build
.\_RunCrossTargetTests.bat all all --skip-build
.\_RunCrossTargetTests.bat all core arithm --skip-build
.\scripts\build-compile-net10-plugin.ps1 -Configuration Release -SkipRuntimeBuild
```

В режиме `--skip-build` cross-target runner компилирует только свою тестовую
программу; CodeCompletion и другие производственные DLL не пересобираются.
Для modern-тестов этого режима требуется подготовленный Release-комплект.

`assert-net10-runtime.ps1` проверяет конфигурацию, обязательные entrypoint,
пути и SHA256 по манифесту. При `RequireCompiledUnits` проверяет непустые PCU
базовых модулей, перечисленных в `RebuildStandartModulesNet10.pas`.
Новые имена модулей не нужно дублировать в BAT-файле.
Экспорт и упаковка не подхватывают посторонние файлы из `bin-net10`.

## Проверки инфраструктуры

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/ReleasePipelineTests/Test-ReleasePipeline.ps1
# При наличии 7-Zip: проверить DLL редактора, извлечённую из готового установщика,
# без установки продукта и без изменения реестра/GAC:
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/InstallerTests/Test-Net10EditorPayload.ps1
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/InstallerTests/Test-Net10InstallerManifest.ps1
```

Тесты используют небольшой изолированный комплект в `.codex-build`:
проверяют упаковку без вызова сборки, состав ZIP, повторную упаковку,
отказы при повреждении манифеста/неподходящей конфигурации/отсутствии PCU,
план этапов и передачу ненулевого кода внешней команды.
Они не устанавливают продукт, не изменяют GAC и не заменяют полные тесты
компилятора и проверку настоящих установщиков.

При проверке переноса 15.10.2026 обычный установщик был реально собран в
`.codex-build` текущим NSIS-сценарием и контрольными NSIS-файлами из `HEAD`
до переноса (на тех же подготовленных бинарниках, примерах и версии).
После распаковки обоих EXE все 1560 вложенных файлов совпали по путям и
SHA256; modern-комплект и CompileNet10Plugin в обычный установщик не попали.
Это проверка совместимости упаковки, а не повторный полный выпуск:
увеличение версии, изменение GAC и все проходы компиляторных тестов
в рамках этой проверки не выполнялись.

## Новый установщик IDE

`scripts/build-net10-ide-installer.ps1` использует готовые Release-комплект,
PCU и плагин IDE. Пересборок нет. В отдельном staging-каталоге формируется
`Net10` по манифесту и явный NSIS include со списком файлов/директорий для
установки и журнала удаления. Стандартный и modern PCU не смешиваются.
Ни SDK, ни Microsoft runtime, ни расширение VS Code в EXE не включаются.

`ReleaseGenerators/PascalABCNETNet10.nsi` использует существующий
`PascalABCNETStandart.nsi` и общие секции; добавляется `sect_Net10.nsh`.
Состав ordinary-установщика не включает эту секцию.
Для новой установки конфигурация плагина содержит `RuntimeDirectory=Net10`.
В установщике `Net10\Lib` содержит PCU, а исходники `.pas`, `.pys` и `.vb`
размещаются в `Net10\LibSource` для навигации. В рабочем `bin-net10` и
консольном ZIP исходная структура сохраняется. `__RedirectIOMode` входит
в предварительную сборку: запуск из IDE не должен создавать его PCU в
защищённой папке установки.
Пользовательский INI сохраняется при повторной установке и удалении.

Проверяется стабильный Desktop Runtime 10.0.x **x64**: зарегистрированный
InstallLocation из 32-битного представления реестра (с fallback в Program Files),
наличие host, Core Runtime и Windows Desktop Runtime. SDK сам по себе, .NET 9,
.NET 11 и preview/RC не заменяют эту зависимость. На 32-битной Windows modern
режим этого первого варианта поставки не объявляется доступным.
Если runtime отсутствует, предлагается открыть страницу Microsoft; после
установки можно повторить проверку либо продолжить без modern-режима.
В silent-режиме браузер не открывается, отсутствие зависимости записывается
в детали установки.

При каждом запуске новой IDE плагин проверяет доступные стабильные Core и
Desktop Runtime 10.0.x через настроенный `dotnet --list-runtimes`.
Если runtime и полный комплект Controller/Worker/LanguageServer доступны,
начальный режим — .NET 10; иначе остаётся .NET Framework, без диалога при старте.
Ручное переключение не сохраняется между запусками. Стандартный установщик
не содержит modern-плагина, его начальный режим не изменён.

Двуцелевой установщик ведёт `uninstall-net10.log` в UTF-16LE, независимо от
системной кодировки. Обычный вариант сохраняет прежний ANSI `uninstall.log`.
Совместимость обновления/понижения между вариантами и отключение пункта меню
в самой IDE без runtime — последующие этапы, ещё не проверка готового релиза.
До их завершения установщики считаются пробными.

```powershell
# Только подготовить payload:
.\scripts\build-net10-ide-installer.ps1 -StageOnly

# Собрать новый EXE из готовых файлов:
.\scripts\build-net10-ide-installer.ps1

# Пробная сборка с отдельным именем:
.\scripts\build-net10-ide-installer.ps1 -OutputFile D:\PABC_GIT\.codex-build\trial\PascalABCNETSetupNet10.exe

# Проверки NSIS-кода (не устанавливают сам продукт):
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/InstallerTests/Test-Net10RuntimeChecker.ps1
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/InstallerTests/Test-Net10Payload.ps1 -PayloadDirectory <каталог из NET10_INSTALLER_PAYLOAD>
```

Последняя проверка распаковывает только новую секцию в `.codex-build`,
сверяет файлы и Unicode-журнал, повторную установку и удаление, проверяет
сохранность INI и постороннего пользовательского файла. GAC, ассоциации,
ярлыки и существующая установка IDE не затрагиваются.
