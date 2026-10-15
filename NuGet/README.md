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
Публичный результат не содержит типов NuGet. RID, native/resource assets,
проверка авторизации закрытых источников и аудит уязвимостей относятся
к следующим этапам.

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
