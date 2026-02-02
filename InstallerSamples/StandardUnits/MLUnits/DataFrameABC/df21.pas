// Сортировка по нескольким полям
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
  
  df.Filter(r -> r.IsValid('score'))
    .SortBy(['age','score'],[True,True])
    .Println;
end.