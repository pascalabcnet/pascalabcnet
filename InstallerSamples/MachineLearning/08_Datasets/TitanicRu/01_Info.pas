uses MLABC;

begin
  Datasets.Language := 'ru';

  var ds := Datasets.TitanicRu;
  ds.Info;
  Println;

  ds.ClassCounts.PrintLines(kv -> ds.ClassName(kv.Key) + ' → ' + kv.Value);
  Println;

  ds.Data.Print(20);
end.
