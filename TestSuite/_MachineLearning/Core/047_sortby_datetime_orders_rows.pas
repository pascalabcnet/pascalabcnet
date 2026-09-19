uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Id', Arr(1, 2, 3));
  df.AddDateTimeColumn('CreatedAt', Arr(
    new System.DateTime(2024, 1, 16, 12, 30, 0),
    new System.DateTime(2024, 1, 15),
    new System.DateTime(2024, 1, 17, 9, 15, 0)
  ));

  var res := df.SortBy('CreatedAt');

  Check(res.RowCount = 3, 'SortBy(DateTime) row count mismatch');
  Check(res.IntValues('Id')[0] = 2, 'First sorted row mismatch');
  Check(res.IntValues('Id')[1] = 1, 'Second sorted row mismatch');
  Check(res.IntValues('Id')[2] = 3, 'Third sorted row mismatch');
  Check(res.DateTimeValues('CreatedAt')[0] = new System.DateTime(2024, 1, 15), 'Sorted DateTime value mismatch');
  CheckSchemaMatchesColumns(res);
end.
