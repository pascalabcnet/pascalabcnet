// В этом примере показана ручная цепочка предобработки таблицы.
//
// Сначала заполняем пропуск в числовом столбце,
// затем превращаем категориальный столбец в набор бинарных признаков.

uses MLABC;

begin
  var df := DataFrame.FromCsvText('''
city,population,region
Ростов-на-Дону,1142,Юг
Таганрог,NA,Юг
Воронеж,1058,Центр
Курск,452,Центр
Хабаровск,616,Дальний Восток
''');

  Println('Исходные данные:');
  df.Print;
  Println;

  var imputer := new Imputer(['population']);
  df := imputer.FitTransform(df);

  var encoder := new OneHotEncoder('region');
  df := encoder.FitTransform(df);

  Println('После цепочки Imputer -> OneHotEncoder:');
  df.Print;
end.
