// Загрузка и просмотр данных
uses DataFrameABC;

// Выбор столбцов (Select)
begin
  var df := DataFrame.FromCsvText('''
  name,age,score
  Alice,20,85
  Bob,22,90
  Charlie,21,78
  ''');
  
  df.Select(['name', 'score']).Print;
end.