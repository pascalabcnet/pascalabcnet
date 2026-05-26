uses MLABC;

begin
  Datasets.Language := 'ru';
  
  var ds := Datasets.MoscowHousing;
  ds.Info;
  var df := ds.Data;
  
  Println;
  Println('Первые строки:');
  ds.Head.Print;
  Println;
  df.Schema.Println
end.