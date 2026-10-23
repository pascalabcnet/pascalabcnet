# Кеш поиска .NET-типов

```powershell
dotnet run --project tests/TypeLookupCacheTests/TypeLookupCacheTests.csproj --framework net472 -p:PABCNET_LEGACY_ONLY=true
dotnet run --project tests/TypeLookupCacheTests/TypeLookupCacheTests.csproj --framework net10.0 -p:PABCNET_LEGACY_ONLY=false
```

Промахи по короткому и полному имени сохраняются и повторно используются,
пока набор проиндексированных DLL не изменился. Новая DLL очищает только
отрицательные записи; повторное подключение известной DLL ничего не очищает.
После добавления новой сборки тип должен находиться без сброса положительного
кеша и таблиц членов. Проверяются совпадение коротких имён в разных пространствах,
границы uses, регистр Pascal-имён, AddType и прямое разрешение CLR-типа.
Индексированное неоднозначное короткое имя не считается глобально отсутствующим.
Сборки создаются в памяти, сеть и NuGet-пакеты для сценария не нужны.

Реальные холодные сборки RobotField/PT4MakerNetX без обходных полных имён
проверяются в `tests/MessageBoxLookupTests`.
