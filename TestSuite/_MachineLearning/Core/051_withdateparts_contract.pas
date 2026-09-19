uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Id', Arr(1, 2));
  df.AddDateTimeColumn('CreatedAt', Arr(
    new System.DateTime(2024, 1, 15, 10, 20, 30),
    new System.DateTime(2025, 2, 16, 12, 30, 40)
  ));

  var res := df.WithDateParts('CreatedAt', [
    ('Year', dpYear),
    ('Month', dpMonth),
    ('OnlyDate', dpDate)
  ]);

  Check(res.ColumnCount = 5, 'WithDateParts column count mismatch');
  Check(res.GetColumnType('Year') = ColumnType.ctInt, 'Year type mismatch');
  Check(res.GetColumnType('Month') = ColumnType.ctInt, 'Month type mismatch');
  Check(res.GetColumnType('OnlyDate') = ColumnType.ctDateTime, 'OnlyDate type mismatch');
  Check(res.IntValues('Year')[0] = 2024, 'Year[0] mismatch');
  Check(res.IntValues('Month')[1] = 2, 'Month[1] mismatch');
  Check(res.DateTimeValues('OnlyDate')[0] = new System.DateTime(2024, 1, 15), 'OnlyDate[0] mismatch');
  CheckSchemaMatchesColumns(res);
end.
