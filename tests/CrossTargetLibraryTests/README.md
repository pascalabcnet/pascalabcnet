# Вспомогательные библиотеки тестов для Framework и .NET 10

Запуск из корня репозитория с подготовленными компиляторами и стандартными PCU:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tests/CrossTargetLibraryTests/Test-CrossTargetLibraries.ps1
```

Проверяются тесты через штатный `CrossTargetTestRunner` для обеих платформ:

- `overloads11.pas`: библиотека собирается из `TestSuite/libs/l2.pas`;
- `foreach_resolver1.pas`: из `TestSuite/foreach_resolver_lib.pas`;
- `usesunits/uses_in3_derectives.pas`: из `usesunits/UsesInUnits3/uui3_lib.pas`.
- `records14.pas`, `units/u_lib1.pas`, `usesunits/use_lib1.pas`:
  C#-проект `TestSuite/libs/ClassLibrary1/ClassLibrary1.csproj`.

DLL создаются заново в `TestSuite/TestResults/<target>/work`, до компиляции
использующих их тестов. Готовые DLL в исходном `TestSuite` не изменяются.
Подготовка учитывает фильтр имени теста и работает в режимах `core`, `units`, `all`.
Ошибка сборки библиотеки считается ошибкой прогона.

Первые два теста компилируются и выполняются. Третий под Framework сохраняет
штатную проверку компиляции. Под .NET 10 он также запускается: это проверяет
чтение встроенного `1.txt` при инициализации модуля `UsesInUnits3/u.pas`.
Подробные проверки ресурсов EXE и DLL находятся в `tests/ManagedResourceTests`.

Исходник `ClassLibrary1.Rec1` восстановлен по метаданным и IL старой DLL:
публичная структура, два публичных поля `int a`, `int b`, конструктор `(int, int)`.
Операторов равенства и дополнительных методов в ней нет. Это важно: `records14`
проверяет сравнение внешнего CLR value type, а не операторов Pascal-записи.
Проверка `ClassLibraryChecks` сравнивает этот контракт исходной и новой DLL,
назначение полей конструктором, равенство CLR и правильную целевую платформу.

C#-проект собирается SDK один раз за процесс тестера, только если фильтр
включает один из трёх тестов. Отдельные `obj` и DLL находятся в рабочей папке
каждой платформы. Новая DLL копируется в `work/units` и `work/usesunits`;
три старые DLL в исходном `TestSuite` не заменяются. Ошибка или таймаут сборки
не позволяют продолжить соответствующую секцию с устаревшей DLL.
`use_lib1` также выполняется, чтобы проверить доступ к полям типа через модуль.
