uses MLABC;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data;

  Println('До предобработки:');
  df.Select(['Id', 'Имя', 'Выжил', 'Класс', 'Пол', 'Возраст', 'ПортПосадки']).Head(10).Print;
  Println;

  // Убираем служебный номер и длинное текстовое поле.
  df := df.Drop(['Id', 'Имя']);

  // Заполняем пропуски в возрасте средним значением.
  var ageImputer := new Imputer(['Возраст']);
  df := ageImputer.FitTransform(df);

  // Заполняем пропуски в порту посадки константой.
  var portImputer := new Imputer('Саутгемптон', ['ПортПосадки']);
  df := portImputer.FitTransform(df);

  Println('После предобработки:');
  df.Select(['Выжил', 'Класс', 'Пол', 'Возраст', 'ПортПосадки']).Head(10).Print;
end.
