uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Id', Arr(1, 2, 3));
  df.AddDateTimeColumn('CreatedAt', Arr(
    new System.DateTime(2024, 1, 15),
    new System.DateTime(2024, 1, 16, 12, 30, 0),
    new System.DateTime(2024, 1, 17, 9, 15, 0)
  ));
  df.AddStrColumn('Name', Arr('Alice', 'Bob', 'Charlie'));

  var res := df.Filter(cur -> cur.DateTime('CreatedAt') >= new System.DateTime(2024, 1, 16));

  Check(res.RowCount = 2, 'Filter(DateTime) row count mismatch');
  Check(res.IntValues('Id')[0] = 2, 'First filtered row mismatch');
  Check(res.IntValues('Id')[1] = 3, 'Second filtered row mismatch');
  Check(res.DateTimeValues('CreatedAt')[0] = new System.DateTime(2024, 1, 16, 12, 30, 0), 'First filtered DateTime mismatch');
  CheckSchemaMatchesColumns(res);
end.
