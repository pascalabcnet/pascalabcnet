uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B', 'C'));
  df.AddIntColumn('Age', Arr(10, 20, 30));
  df.AddFloatColumn('Score', Arr(1.0, 2.0, 3.0));
  df := df.SetCategorical(['City']);

  var res := df.Filter(r -> r.Int('Age') >= 20);

  Check(res.RowCount = 2, 'Filtered row count mismatch');
  CheckSchemaMatchesColumns(res, Arr(true, false, false));
end.
