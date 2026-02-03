// Простая фильтрация
uses DataFrameABC;

begin
  var df := DataFrame.FromCsvText('''
  name,age,score
  Alice,20,85
  Bob,22,90
  Charlie,21,78
  ''');
  
  df.Filter(row -> row.Int('age') >= 21).Print;
end.