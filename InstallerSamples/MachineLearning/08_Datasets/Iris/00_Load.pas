uses MLABC;

begin
  var ds := Datasets.Iris;

  Println('Dataset: ', ds.Name);
  Println('Task: ', ds.Task);
  Println;

  Println('Rows:', ds.RowCount);
  Println('Features:', ds.Features.Length);
  Println;

  Println('Head:');
  ds.Head.Print;
end.