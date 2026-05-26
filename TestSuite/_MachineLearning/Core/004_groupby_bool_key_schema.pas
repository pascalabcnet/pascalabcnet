uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddBoolColumn('Flag', Arr(true, false, true));
  df.AddFloatColumn('X', Arr(1.0, 2.0, 3.0));
  df := df.SetCategorical(['Flag']);

  var g := df.GroupBy('Flag').Sum('X');

  Check(g.Schema.ColumnCount = 2, 'Unexpected column count');
  Check(g.Schema.NameAt(0) = 'Flag', 'Key column name mismatch');
  Check(g.Schema.ColumnTypeAt(0) = ColumnType.ctBool, 'Key column type must stay bool');
  Check(g.GetColumn(0).Info.Name = 'Flag', 'Physical key column name mismatch');
  Check(g.GetColumn(0).Info.ColType = ColumnType.ctBool, 'Physical key column type must stay bool');
end.
