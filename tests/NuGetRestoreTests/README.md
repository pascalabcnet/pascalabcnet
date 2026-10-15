# NuGetRestoreTests

Интеграционный тест самостоятельного NuGet-моста; не зависит от Pascal-компилятора.

Из корня репозитория:

```bat
dotnet run --project tests\NuGetRestoreTests\NuGetRestoreTests.csproj -c Release
```

Fixtures/TestRoot и Fixtures/TestDependency собираются автоматически.
Сам тест через NuGet.Packaging создаёт локальный feed с пакетами и восстанавливает
их через API PascalABCNet.PackageServices. Для root используется настоящая
reference assembly, созданная C#-компилятором, и отдельная runtime implementation.
Выполнение теста не обращается к nuget.org и не запускает restore через CLI.

Повторная проверка выполняется в самостоятельно созданной копии текущего
.NET runtime без sdk/, с пустым PATH. Она проверяет, что --list-sdks пуст,
и повторяет весь набор сценариев без установленного в этом окружении SDK.

Проверяются также стандартная иерархия NuGet.Config, локальный источник
с именем intranet, отключённые источники, `<clear/>`, NUGET_PACKAGES,
приоритет явного API-каталога и отдельный скрытый кеш на программу.
Настройки теста создаются в его временном каталоге; пользовательские конфиги
не меняются. Переменная NUGET_PACKAGES восстанавливается после проверки.

Явный дополнительный тест общего кеша и скачивания с nuget.org:

```bat
dotnet run --project tests\NuGetRestoreTests\NuGetRestoreTests.csproj -c Release -- --shared-cache
```

Для него нужен установленный Newtonsoft.Json 13.0.3. Dapper 2.1.66
скачивается при отсутствии либо используется из кеша. Источники берутся из
настоящих настроек NuGet; nuget.org должен быть включён. Существующие пакеты
не удаляются и не изменяются. Этот режим требует сеть при первом скачивании.

В том же режиме восстанавливается реальный пакет
Microsoft.Extensions.Configuration.Json 10.0.0: проверяются зависимость второго
уровня через Configuration.FileExtensions к FileProviders.Physical, наличие всех
выбранных DLL и повторный no-op restore. Скачанные пакеты остаются в общем кеше.

Ожидаемый результат обычного режима: три строки PASS и код завершения 0.
Версии зависимостей тестового приложения зафиксированы в packages.lock.json.
