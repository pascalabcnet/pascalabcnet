// Удаление столбцов (Drop)
uses DataFrameABC;

begin
  var df := DataFrame.FromCsvText('''
  name,age,score
  Alice,20,85
  Bob,22,90
  Charlie,21,78
  ''');
  
  df.Drop(['score']).Print;
end.