uses MLABC;

begin
  Datasets.Language := 'ru';
  
  var ds := Datasets.MoscowHousing;
  ds.Info;
  
  Println;
  Println('Первые строки:');
  ds.Head.Println(1);
  
  ds.Data.Schema.Print
end.