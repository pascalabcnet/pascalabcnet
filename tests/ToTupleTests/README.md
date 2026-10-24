# Issue 3381: кортеж в выражении key to value

Исходное воспроизведение: `repro.pas`. До исправления компилировалось на обеих
платформах и падало с InvalidProgramException при запуске.

Причина: setter индексатора `to_expr[1]` присваивал параметр value самому себе,
а не свойству `this.value`. Замена кортежа на Tuple.Create терялась; позже
ToExprVisitor создавал Pair с непреобразованным кортежем. Ошибка исправлена
в Tree.cs и шаблоне NodesGeneratorNew. Такое же затенение устранено в enumerator.
Вложенные to обрабатываются до замены родительского узла, а не после неё.

Проверки AST (для второго запуска заменить net472/true на net10.0/false):

```powershell
dotnet run --project tests/ToTupleTests/ToTupleTests.csproj --framework net472 -p:PABCNET_LEGACY_ONLY=true
```

Полная компиляция и запуск на Framework и .NET 10:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/ToTupleTests/Test-ToTuple.ps1
```

Скрипт собирает компиляторы и языковые плагины в `.codex-build`, использует копии
подготовленных Lib/Lng и не заменяет рабочие bin/bin-net10 или их PCU.
Проверяет исходное воспроизведение, TestSuite/to_tuple_keyvaluepair.pas
и восемь соседних тестов кортежей. Регрессия включает кортежи в ключе/значении,
вложенные to и кортежи, типы KV/Pair и порядок однократного вычисления операндов.
Pascal-тест входит в обычный TestSuite; AST-проверка добавлена в CI.
