uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb'));
  df.AddIntColumn('Age', Arr(20, 30));

  var z := Statistics.StandardizeAll(df);

  Check(z.Schema.ColumnCount = 2, 'Unexpected column count');
  Check(z.Schema.ColumnTypeAt(0) = ColumnType.ctStr, 'String column type must be preserved');
  Check(z.Schema.ColumnTypeAt(1) = ColumnType.ctFloat, 'Numeric column must become float');
  Check(z.GetColumn(0).Info.ColType = ColumnType.ctStr, 'Physical string column type must be preserved');
  Check(z.GetColumn(1).Info.ColType = ColumnType.ctFloat, 'Physical numeric column must become float');
end.
