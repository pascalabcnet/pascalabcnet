uses MLABC;

begin
  Datasets.Language := 'ru';
  
  var ds := Datasets.MoscowHousing;
  ds.Info;
  var df := ds.Data;
  
  Println;
  df.PrintInfo;

  Println;
  Println('Первые строки:');
  ds.Head.Print;
end.