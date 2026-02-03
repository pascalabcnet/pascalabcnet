// Новый стандартный модуль для работы с датасетами
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