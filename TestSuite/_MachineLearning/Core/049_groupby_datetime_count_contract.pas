uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Id', Arr(1, 2, 3, 4));
  df.AddDateTimeColumn('CreatedAt', Arr(
    new System.DateTime(2024, 1, 15),
    new System.DateTime(2024, 1, 16, 12, 30, 0),
    new System.DateTime(2024, 1, 15),
    new System.DateTime(2024, 1, 16, 12, 30, 0)
  ));

  var g := df.GroupBy('CreatedAt').Count;

  Check(g.Schema.ColumnCount = 2, 'Unexpected GroupBy(DateTime) column count');
  Check(g.Schema.NameAt(0) = 'CreatedAt', 'DateTime key column name mismatch');
  Check(g.Schema.ColumnTypeAt(0) = ColumnType.ctDateTime, 'DateTime key column type must stay DateTime');
  Check(g.GetColumn(0).Info.Name = 'CreatedAt', 'Physical DateTime key column name mismatch');
  Check(g.GetColumn(0).Info.ColType = ColumnType.ctDateTime, 'Physical DateTime key column type must stay DateTime');
  Check(g.RowCount = 2, 'GroupBy(DateTime) distinct key count mismatch');
  Check(g.DateTimeValues('CreatedAt')[0] = new System.DateTime(2024, 1, 15), 'First grouped DateTime key mismatch');
  Check(g.IntValues('count')[0] = 2, 'First grouped count mismatch');
  Check(g.IntValues('count')[1] = 2, 'Second grouped count mismatch');
end.
