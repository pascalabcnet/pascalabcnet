uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B', 'C'));
  df.AddIntColumn('Age', Arr(10, 20, 30));
  df := df.SetCategorical(['City']);

  var res := df.TakeRows([2, 0]);

  Check(res.RowCount = 2, 'TakeRows row count mismatch');
  Check(res.GetStrColumn('City')[0] = 'C', 'First row order mismatch');
  Check(res.GetStrColumn('City')[1] = 'A', 'Second row order mismatch');
  CheckSchemaMatchesColumns(res, Arr(true, false));
end.
