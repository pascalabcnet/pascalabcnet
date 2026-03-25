uses MLABC;
begin
  var df := CsvLoader.Load('towns_russia.csv', inferCategorical := true);
  
  df := df.Select(['city','population','lat','lon','region_name','federal_district']);

  df.Schema.Println;
  
  // Заполняем пропуски в числовых столбцах средним значением
  var imputer := new Imputer('population', 'lat', 'lon');
  df := imputer.FitTransform(df);
  
  // Кодируем категориальные признаки
  var encoder := new LabelEncoder('region_name');
  df := encoder.FitTransform(df);
  
  var encoder2 := new LabelEncoder('federal_district');
  df := encoder2.FitTransform(df);
  
  df.PrintPreview(6);
end.