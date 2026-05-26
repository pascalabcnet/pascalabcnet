// Пример табличной предобработки:
// сначала заполняем пропуски в числовом столбце,
// затем кодируем категориальный столбец числами.
uses MLABC;

begin
  var df := DataFrame.FromCsvText('''
city,population,region
Ростов-на-Дону,1142,Юг
Таганрог,NA,Юг
Воронеж,1058,Центр
Курск,452,Центр
Белгород,392,Центр
''');

  Println('Исходные данные:');
  df.Print;
  Println;

  // Заполняем пропуск в числовом столбце средним значением
  var imp := new Imputer(['population']);
  df := imp.FitTransform(df);

  // Кодируем названия регионов числами 0, 1, 2, ...
  var encoder := new OrdinalEncoder('region');
  df := encoder.FitTransform(df);

  Println('После Imputer и OrdinalEncoder:');
  df.Print;
end.
