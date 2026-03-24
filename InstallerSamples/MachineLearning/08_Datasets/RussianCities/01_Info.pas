uses MLABC;
 
begin
  var ds := Datasets.RussianCities;
  
  var df := ds.Data;

  df.Schema.Println;
  df.PrintlnInfo;
  
  df.PrintlnPreview(20);
end.