uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('id', Arr(1, 2));
  left.AddFloatColumn('feature', Arr(1.0, 2.0));

  var right := new DataFrame;
  right.AddIntColumn('id', Arr(1, 2));
  right.AddFloatColumn('feature', Arr(10.0, 20.0));

  var res := left.Join(right, 'id');

  Check(res.Schema.ColumnCount = 3, 'Join column count mismatch');
  Check(res.Schema.NameAt(0) = 'id', 'First join column mismatch');
  Check(res.Schema.NameAt(1) = 'feature', 'Left feature name mismatch');
  Check(res.Schema.NameAt(2) = 'right_feature', 'Right conflicting column must get prefix');
  Check(res.GetColumn(2).Info.Name = 'right_feature', 'Physical conflicting right column name mismatch');
  CheckSchemaMatchesColumns(res, Arr(false, false, false));
end.
