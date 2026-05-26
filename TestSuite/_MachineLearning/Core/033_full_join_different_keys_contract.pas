uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('id_left', Arr(1, 2));
  left.AddStrColumn('name', Arr('A', 'B'));

  var right := new DataFrame;
  right.AddIntColumn('id_right', Arr(2, 3));
  right.AddFloatColumn('score', Arr(20.0, 30.0));

  var res := left.Join(right, ['id_left'], ['id_right'], jkFull);

  Check(res.RowCount = 3, 'Full join with different keys row count mismatch');
  Check(res.HasColumn('name'), 'Left payload column missing');
  Check(res.HasColumn('score'), 'Right payload column missing');
  CheckSchemaMatchesColumns(res);
end.
