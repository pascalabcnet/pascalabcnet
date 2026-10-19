# NuGet для PascalABC.NET

`PascalABCNet.PackageServices` — самостоятельная библиотека под .NET 10,
будущий мост между официальными библиотеками NuGet и PascalABC.NET.
Она не зависит от компилятора, IntelliSense, LSP или графической оболочки.

Подключены `NuGet.Commands`, `NuGet.ProjectModel` и `NuGet.Protocol` версии 7.9.0.
Реализован первый restore через `PackageRestoreService.RestoreAsync`:
точные прямые версии, транзитивные зависимости, отдельные compile/runtime DLL,
сохранённые assets, повторный no-op restore и диагностические коды NuGet.

Текущий вход: путь однофайловой программы, пакеты с точными версиями и `net10.0`.
По умолчанию используется общий кеш NuGet и стандартные настройки NuGet.Config.
Публичный результат не содержит типов NuGet. Resource assets,
проверка авторизации закрытых источников и аудит уязвимостей относятся
к следующим этапам.

## Нативные зависимости: первый поддержанный вариант

Реализованы восстановление, запуск из кеша и публикация нативных файлов для
Windows x64. Проверенные реальные пакеты — Microsoft.Data.Sqlite 10.0.13 и
MathNet.Numerics.MKL.Win-x64 3.0.0 (Math.NET Numerics и MKL provider 5.0.0).
`PackageRestoreRequest.RuntimeIdentifier` можно явно задать как `win-x64`;
по умолчанию он выбирается в Windows x64 процессе. На других платформах
сохраняется прежнее платформонезависимое восстановление; поддержка их нативных
зависимостей пока не заявляется.

NuGet использует штатные правила совместимости RID. Portable RID graph берётся
из SDK только при сборке библиотеки и встраивается в неё; пользователю SDK не
нужен. При restore данные извлекаются в служебный каталог программы, не рядом
с исходником. Управляемая реализация выбирается для RID, а нативные файлы
передаются отдельно в `ResolvedPackage.NativeLibraries` и не загружаются
семантикой/IntelliSense. `ResolvedPackageGraph.RuntimeIdentifier` обозначает
платформенную привязку результата; обычные управляемые графы сохраняют
переносимость, если выбранные зависимости и runtime-файлы не отличаются от portable.

Для запуска native-файлы описываются в `.deps.json`, их находит обычный .NET
host в общем кеше. Глобальный PATH не меняется. Публикация копирует выбранные
управляемые и нативные файлы рядом с программой без зависимости от кеша.
Это пока не обещание поддержки произвольных MSBuild-скриптов пакетов, всех
платформ и системных библиотек, которые могут требоваться конкретному пакету.
Проверки: `tests/NuGetRestoreTests/README.md`, режимы `--native-assets` и `--sqlite-native`.
Готовый пример для оболочки и консольного компилятора: `NuGet/Samples/SqliteNative.pas`.
Пример нативного умножения матриц: `NuGet/Samples/MathNetMkl.pas`.
Он принудительно включает MKL, проверяет тип активного провайдера и результат;
управляемый fallback не считается успехом. Тест `--mathnet-mkl` проверяет также
вторичную нативную зависимость OpenMP, выполнение без SDK и публикацию без кеша.

Первый пример ML.NET: `NuGet/Samples/MLNetRegression.pas` с Microsoft.ML 5.0.0.
Обучает SDCA-регрессию на `y = 2*x + 1`, делает три прогноза на других входах.
Данные имеют тип `single`; `[VectorType(1)]` описывает длину вектора признаков.
Фиксированы seed и один поток; регуляризация свободного члена обеспечивает
устойчивость на маленьком учебном наборе. `--save <путь.zip>` сохраняет модель,
`--load <путь.zip>` загружает её без обучения. Без аргументов файлы модели не создаются.
Сетевой тест `--mlnet-regression` проверяет также IntelliSense, запуск без SDK,
сохранение/чтение в новом процессе и публикацию без кеша.
Модель является данными пользователя: в папку публикации её копируют явно.
Это проверка SDCA, не всех алгоритмов/дополнительных пакетов ML.NET.

## Пакетные копии базовых сборок .NET

ML.NET транзитивно требует System.Collections.Immutable 9, уже предоставляемую
платформой .NET 10. После восстановления `FrameworkAssemblyPolicy` исключает
из compile/runtime assets пакетные копии сборок, если в текущем
Microsoft.NETCore.App есть совпадающие имя, culture и непустой public key token,
а assembly version платформы выше (или равна при не меньшей file version).
Сборки читаются как метаданные, без загрузки в процесс.
Граф требований и версии пакетов сохраняются; manifest/публикация используют
тот же отфильтрованный набор. NuGet-кеш и `project.assets.json` не изменяются
этой фильтрацией. Новый SDK или `dotnet restore` для неё не нужен.

Это не правило «игнорировать все System.*»: более новые, чужие/unsigned DLL,
WindowsDesktop, пользовательские reference и native assets не подменяются.
Конфликты пользовательских DLL по-прежнему остаются ошибками.
Локальные проверки `--framework-assemblies` включены в обычный прогон и runtime-only.

Restore выполняется через API библиотек NuGet. Запуск `dotnet restore`,
создание служебного C#-проекта и установленный .NET SDK пользователю библиотеки
для этого не потребуются. SDK нужен разработчику для сборки самой библиотеки.

Открыть в Visual Studio 2026: `NuGet/PascalABCNet.NuGet.slnx`.

Сборка из корня репозитория:

```bat
dotnet build NuGet\PascalABCNet.NuGet.slnx -c Release
```

Результат: `NuGet/PascalABCNet.PackageServices/bin/Release/net10.0/`.
Помимо `PascalABCNet.PackageServices.dll` туда копируются зависимости NuGet.
Файл `packages.lock.json` фиксирует их версии и должен храниться в Git.

Проект включён в modern-сборку `Net10Runtime.slnx` как зависимость компилятора;
при сборке консольной поставки его DLL и зависимости попадают в общий каталог
`bin-net10`.

Modern-компилятор подключает библиотеку через `{$package}`. Для программы
под .NET 10 он записывает `.deps.json` и `runtimeconfig.json`, позволяя
обычному `dotnet` загружать управляемые DLL из общего кеша NuGet без
копирования рядом с программой. Проверка интеграции: см.
`tests/NuGetRestoreTests/README.md` и режим `--compiler`.

## Использование библиотеки

```csharp
using PascalABCNet.PackageServices;

var graph = await new PackageRestoreService().RestoreAsync(new PackageRestoreRequest
{
    Packages = new[] { new PackageRequirement("Newtonsoft.Json", "13.0.3") },
    ProgramFilePath = @"C:\PABCWork.NET\a.pas"
});

foreach (var package in graph.Packages)
{
    Console.WriteLine($"{package.Id} {package.Version}");
    // package.CompileAssemblies and package.RuntimeAssemblies can differ.
}
```

`PackagesDirectory` по умолчанию определяется NuGet.Configuration:
учитывается NUGET_PACKAGES, globalPackagesFolder в NuGet.Config и стандартный
общий кеш. В обычной Windows-конфигурации это `%USERPROFILE%\.nuget\packages`,
тот же каталог, который использует C#. Явное значение свойства переопределяет
автоматический выбор.

`RestoreDirectory` по умолчанию находится в скрытом пользовательском кеше:

```text
Windows: %LOCALAPPDATA%\PascalABC.NET\NuGet\Restore\<ключ пути программы>
Linux:   $XDG_CACHE_HOME/PascalABC.NET/NuGet/Restore/<ключ>
         либо ~/.cache/PascalABC.NET/NuGet/Restore/<ключ>
```

Ключ определяется полным путём ProgramFilePath. Одноимённые программы в разных
папках имеют разные служебные каталоги; рядом с .pas ничего не создаётся.
Это пока расположение только результатов restore, а не EXE/PDB программы.
Будущая оболочка сможет использовать его при организации скрытой сборки.

Если RestoreDirectory указан явно, ProgramFilePath необязателен. Это позволяет
консольным инструментам или будущей публикации самостоятельно выбирать output.
Используйте разные RestoreDirectory для независимых параллельных запросов.

Настройки NuGet.Config читаются начиная от папки программы вверх по дереву,
с учётом пользовательских и машинных настроек. Поддерживаются `<clear/>`,
отключённые источники и относительный globalPackagesFolder. В ConfigurationDirectory
можно явно задать начало поиска, а NuGetConfigFile выбирает один конкретный
конфигурационный файл вместо всей иерархии. Пользовательские настройки не переписываются.

Если источники нигде не настроены, используется `nuget.org`
(`https://api.nuget.org/v3/index.json`). Visual Studio, SDK и заранее созданный
NuGet.Config для этого не нужны. Явная секция `packageSources`, включая `<clear/>`
или пустую секцию, и отключение nuget.org не обходятся: если включённых источников
нет, выдаётся ошибка. Локальный/интранет-источник или явный список `Sources`
не дополняется nuget.org автоматически.

Sources = null использует включённые источники из конфигурации. Явный список
переопределяет выбор; пустой список или конфигурация без включённых источников
дают понятную ошибку, не добавляя nuget.org вопреки `<clear/>`.
Для интранета достаточно NuGet.Config в папке с программами, например:

```xml
<configuration>
  <packageSources>
    <clear />
    <add key="mechmath" value="https://packages.example.edu/v3/index.json" />
  </packageSources>
</configuration>
```

Для просмотра эффективных путей, источников и файлов конфигурации без restore
доступен `PackageRestoreService.GetConfiguration(request)`. Результат restore
также содержит эту информацию в `graph.Configuration`.

`ForceRestore = true` заставляет NuGet пересчитать результат, сохраняя возможность
использовать уже установленные пакеты. `PackageRestoreException.Diagnostics`
содержит ошибки с кодами `NU...`. Поддерживается CancellationToken.

Reference assemblies из `ref/` библиотека возвращает как compile assets,
а implementation assemblies из `lib/` — как runtime assets. Библиотека не
загружает эти сборки через Reflection. Их интеграция с семантикой PascalABC.NET
будет выполняться отдельно.

## Изолированные тесты

```bat
dotnet run --project tests\NuGetRestoreTests\NuGetRestoreTests.csproj -c Release
```

Тест создаёт локальные .nupkg из двух собственных fixture-проектов.
Выполнение restore не требует сети или внешних процессов SDK.
Проверяются:

- корневой пакет и его транзитивная зависимость;
- точная версия при наличии более новой;
- выбор net10.0 среди нескольких TFM;
- отдельные реальные `ref/` и `lib/` DLL и fallback на lib;
- выполнение выбранного runtime-графа с результатом 43;
- no-op restore без перезаписи assets и пакетных файлов;
- новый restore из полного кеша с недоступным исходным feed;
- изменение версии, отсутствующая версия, диагностика и восстановление после ошибки;
- принудительный restore и отмена;
- отсутствие создаваемого .csproj;
- повтор всего теста в отдельном runtime-only окружении без SDK и с пустым PATH.
- иерархия NuGet.Config, интранет-источник, отключённые источники и `<clear/>`;
- приоритет NUGET_PACKAGES и явного переопределения API;
- стабильный скрытый кеш на программу и отсутствие файлов рядом с исходником.

Для последней проверки тест копирует текущий host/fxr и Microsoft.NETCore.App
в собственный временный каталог, не копируя sdk/. Вначале проверяет пустой вывод
`--list-sdks`, затем запускает собранный тест через этот host.
Временные пакеты и runtime удаляются после проверки. SDK используется только
для первоначальной сборки тестового приложения и fixture-DLL.

Дополнительная явная проверка общего кеша и nuget.org:

```bat
dotnet run --project tests\NuGetRestoreTests\NuGetRestoreTests.csproj -c Release -- --shared-cache
```

Она требует уже установленный Newtonsoft.Json 13.0.3: проверяет восстановление
из общего кеша с недоступным источником, без изменения существующих DLL.
Затем восстанавливает Dapper 2.1.66 через настроенные источники, среди которых
должен быть nuget.org, и проверяет no-op повтор. Если Dapper отсутствует, он
скачивается в общий кеш; если уже установлен, используется кеш. Пакеты не
удаляются и не заменяются ради теста. Сетевой режим не входит в обычный offline-тест.
