uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('k1', Arr(1, 1, 2));
  left.AddStrColumn('k2', Arr('A', 'B', 'A'));
  left.AddFloatColumn('x', Arr(10.0, 20.0, 30.0));
  left := left.SetCategorical(['k2']);

  var right := new DataFrame;
  right.AddIntColumn('k1', Arr(1, 2));
  right.AddStrColumn('k2', Arr('A', 'A'));
  right.AddFloatColumn('y', Arr(100.0, 200.0));
  right := right.SetCategorical(['k2']);

  var res := left.Join(right, ['k1', 'k2']);

  Check(res.HasColumn('x'), 'Left payload column missing');
  Check(res.HasColumn('y'), 'Right payload column missing');
  Check(res.RowCount = 2, 'Multi-key join row count mismatch');
  CheckSchemaMatchesColumns(res);
end.
