# Вспомогательные библиотеки тестов для Framework и .NET 10

Запуск из корня репозитория с подготовленными компиляторами и стандартными PCU:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tests/CrossTargetLibraryTests/Test-CrossTargetLibraries.ps1
```

Проверяются три теста через штатный `CrossTargetTestRunner` для обеих платформ:

- `overloads11.pas`: библиотека собирается из `TestSuite/libs/l2.pas`;
- `foreach_resolver1.pas`: из `TestSuite/foreach_resolver_lib.pas`;
- `usesunits/uses_in3_derectives.pas`: из `usesunits/UsesInUnits3/uui3_lib.pas`.

DLL создаются заново в `TestSuite/TestResults/<target>/work`, до компиляции
использующих их тестов. Готовые DLL в исходном `TestSuite` не изменяются.
Подготовка учитывает фильтр имени теста и работает в режимах `core`, `units`, `all`.
Ошибка сборки библиотеки считается ошибкой прогона.

Первые два теста компилируются и выполняются. Третий под Framework сохраняет
штатную проверку компиляции. Под .NET 10 он также запускается: это проверяет
чтение встроенного `1.txt` при инициализации модуля `UsesInUnits3/u.pas`.
Подробные проверки ресурсов EXE и DLL находятся в `tests/ManagedResourceTests`.
