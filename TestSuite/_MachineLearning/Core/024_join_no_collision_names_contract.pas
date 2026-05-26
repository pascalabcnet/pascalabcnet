uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var left := new DataFrame;
  left.AddIntColumn('id', Arr(1, 2));
  left.AddStrColumn('name', Arr('A', 'B'));

  var right := new DataFrame;
  right.AddIntColumn('id', Arr(1, 2));
  right.AddFloatColumn('score', Arr(10.0, 20.0));

  var res := left.Join(right, 'id');

  Check(res.Schema.ColumnCount = 3, 'Join column count mismatch');
  Check(res.Schema.NameAt(0) = 'id', 'First join column mismatch');
  Check(res.Schema.NameAt(1) = 'name', 'Second join column mismatch');
  Check(res.Schema.NameAt(2) = 'score', 'Right unique column must stay without prefix');
  Check(res.GetColumn(2).Info.Name = 'score', 'Physical right column name mismatch');
  CheckSchemaMatchesColumns(res, Arr(false, false, false));
end.
