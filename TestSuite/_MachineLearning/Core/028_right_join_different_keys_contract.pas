uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('id_left', Arr(1, 2));
  left.AddStrColumn('name', Arr('A', 'B'));

  var right := new DataFrame;
  right.AddIntColumn('id_right', Arr(2, 3));
  right.AddFloatColumn('score', Arr(20.0, 30.0));

  var res := left.Join(right, ['id_left'], ['id_right'], jkRight);

  Check(res.HasColumn('score'), 'Right payload column missing');
  Check(res.RowCount = 2, 'Right join row count mismatch');
  CheckSchemaMatchesColumns(res);
end.
