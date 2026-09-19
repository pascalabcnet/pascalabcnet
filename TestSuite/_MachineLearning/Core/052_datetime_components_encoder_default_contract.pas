uses MLABC, DataFrameABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := CsvLoader.LoadFromLines('''
id,created_at,name
1,15.01.2024 10:20:30,Alice
2,16.02.2025 12:30:40,Bob
'''.ToLines, inferTypes := True);

  var enc := new DateTimeComponentsEncoder('created_at');
  var res := enc.FitTransform(df);

  Check(not res.HasColumn('created_at'), 'Source DateTime column must be replaced');
  Check(res.HasColumn('created_at_year'), 'Year column missing');
  Check(res.HasColumn('created_at_month'), 'Month column missing');
  Check(res.HasColumn('created_at_day'), 'Day column missing');
  Check(res.GetColumnType('created_at_year') = ColumnType.ctInt, 'Year column type mismatch');
  Check(res.IntValues('created_at_year')[0] = 2024, 'First year mismatch');
  Check(res.IntValues('created_at_month')[1] = 2, 'Second month mismatch');
  Check(res.IntValues('created_at_day')[1] = 16, 'Second day mismatch');
  CheckSchemaMatchesColumns(res);
end.
