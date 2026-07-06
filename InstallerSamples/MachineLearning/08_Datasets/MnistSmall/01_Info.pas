uses MLABC;

begin
  Datasets.Language := 'ru';

  var ds := Datasets.MnistSmall;
  ds.Info;
  Println;

  ds.ClassCounts
    .OrderBy(kv -> kv.Key) 
    .PrintLines(kv -> ds.ClassName(kv.Key) + ' → ' + kv.Value + ' шт');
  Println;

  ds.Data.Print(10);
end.
