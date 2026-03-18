uses MLABC;

begin
  // Откомментируйте - всё станет по-английски
  //Datasets.Language := 'en';

  var ds := Datasets.Iris;
  ds.Info;
  Println;

  ds.Classes.Println;
  Println;

  ds.ClassCounts.PrintLines;
  Println;
  
  ds.Classes.PrintLines(cvalue -> ds.ClassName(cvalue));
  Println;
  
  ds.ClassCounts.PrintLines(kv -> ds.ClassName(kv.Key) + ' → ' + kv.Value);
end.