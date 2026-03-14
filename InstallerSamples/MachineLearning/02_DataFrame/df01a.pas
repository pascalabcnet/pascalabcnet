// Загрузка данных из CSV-файла
uses DataFrameABC;

begin
  var df := DataFrame.FromCsv('people.csv');
  df.Print;
end.