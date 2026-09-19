uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Id', Arr(1, 2));
  df.AddDateTimeColumn('CreatedAt', Arr(
    new System.DateTime(2024, 1, 15, 10, 20, 30),
    new System.DateTime(2025, 2, 16, 12, 30, 40)
  ));

  var resYear := df.WithDatePart('CreatedAt', 'Year', dpYear);
  Check(resYear.GetColumnType('Year') = ColumnType.ctInt, 'Year part type mismatch');
  Check(resYear.IntValues('Year')[0] = 2024, 'First year mismatch');
  Check(resYear.IntValues('Year')[1] = 2025, 'Second year mismatch');

  var resDate := df.WithDatePart('CreatedAt', 'OnlyDate', dpDate);
  Check(resDate.GetColumnType('OnlyDate') = ColumnType.ctDateTime, 'Date part type mismatch');
  Check(resDate.DateTimeValues('OnlyDate')[0] = new System.DateTime(2024, 1, 15), 'First date part mismatch');
  Check(resDate.DateTimeValues('OnlyDate')[1] = new System.DateTime(2025, 2, 16), 'Second date part mismatch');
  CheckSchemaMatchesColumns(resDate);
end.
