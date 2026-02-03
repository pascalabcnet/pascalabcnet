{ 3.11.1. Новый стандартный модуль для работы с датасетами
  Возможности DataFrame:

  – Постолбцовое хранение данных (column-store) для высокой производительности
  – Типизированные столбцы: int, float, string, bool с поддержкой NA
  – Cursor API для эффективного построчного обхода без лишних аллокаций
  – Фильтрация строк с пользовательским предикатом (Filter)
  – Изменение схемы таблицы: Select, Drop, Rename
  – Добавление вычисляемых столбцов: WithColumn*
  – Группировка данных и агрегаты (GroupBy)
  – Соединение таблиц: Inner / Left / Full Join (один и несколько ключей)
  – Описательная статистика: Count, Mean, Std, Min, Max, Describe
  – Загрузка данных из CSV и многострочного CSV-текста
  – Удобный вывод больших таблиц с предпросмотром (Print / PrintPreview)

  Семантика операций:

  – Select / Drop / Rename → view (без копирования данных)
  – Filter / Join / Sort / WithColumn → materialize (создаются новые массивы)
  – Count считает только валидные (non-NA) значения
}
uses DataFrameABC;

begin
  var df := DataFrame.FromCsvText('''
  name,age,score
  Alice,20,85
  Bob,22,90
  Charlie,21,78
  Bob,22,90
  Clara,,78
  Kat,21,NA
  ''');
  
  df.Filter(row -> row.Int('age') > 20).Println;
  
  df.GroupBy('age').Mean('score').Println;

  var stat := df.Describe('score');
  Println($'score: count={stat.Count}, min={stat.Min}, max={stat.Max}, mean={stat.Mean}, std={stat.Std.Round(3)}');
end.