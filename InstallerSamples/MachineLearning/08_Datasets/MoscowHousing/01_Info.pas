uses MLABC;

begin
  Datasets.Language := 'ru';
  
  var ds := Datasets.MoscowHousing;
  ds.Info;
  var df := ds.Data;
  
  Println;
  Println('Первые строки:');
  ds.Head.Println(1);
  
  df.Schema.Println
end.