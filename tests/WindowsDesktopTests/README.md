# WPF и WinForms в modern-режиме

Запуск из PowerShell в корне репозитория:

```powershell
./scripts/test-windows-desktop.ps1
./scripts/test-windows-desktop.ps1 -Graphics
```

Нужны Windows, SDK .NET 10 для сборки тестов и Windows Desktop Runtime 10.0.x.
Сборка изолирована в `.codex-build/windows-desktop/<run>/bin-net10`, рабочий `bin-net10` не изменяется.

Проверки: подключение только по `{$platform WinForms/WPF}` или явной ссылке;
консольные программы без Desktop; правильные runtimeconfig и STA;
отсутствие копирования системных DLL к программе; совместимые servicing-патчи;
GraphABC, FormsABC и GraphWPF из исходников и PCU; IntelliSense;
чередование графических и консольных программ в одном компиляторе.

`-Graphics` дополнительно запускает GraphABC, FormsABC и GraphWPF:
окна кратковременно появляются и автоматически закрываются. Все дочерние
процессы ограничены по времени. Путь к временным результатам печатается в начале.
