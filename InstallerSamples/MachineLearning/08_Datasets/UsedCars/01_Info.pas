uses MLABC;

begin
  Datasets.Language := 'ru';

  var ds := Datasets.UsedCarsPrice;
  ds.Info;
  Println;

  ds.Data.Print(20);
end.
