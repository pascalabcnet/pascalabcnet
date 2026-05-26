// OneHotEncoder заменяет категориальный столбец
// несколькими бинарными столбцами - по одному на каждую категорию.
uses MLABC;

begin
  var df := DataFrame.FromCsvText('''
city,region
Ростов-на-Дону,Юг
Таганрог,Юг
Воронеж,Центр
Курск,Центр
Хабаровск,Дальний Восток
''');

  Println('Исходные данные:');
  df.Print;
  Println;

  var encoder := new OneHotEncoder('region');
  df := encoder.FitTransform(df);

  Println('После OneHotEncoder:');
  df.Print;
end.
