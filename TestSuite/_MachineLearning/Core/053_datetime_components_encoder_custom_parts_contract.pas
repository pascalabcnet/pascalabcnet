uses MLABC, DataFrameABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := CsvLoader.LoadFromLines('''
id,created_at,name
1,15.01.2024 10:20:30,Alice
2,16.02.2025 12:30:40,Bob
'''.ToLines, inferTypes := True);

  var enc := new DateTimeComponentsEncoder('created_at', [dpDate, dpHour, dpDayOfWeek]);
  var res := enc.FitTransform(df);

  Check(not res.HasColumn('created_at'), 'Source DateTime column must be replaced');
  Check(res.HasColumn('created_at_date'), 'Date component column missing');
  Check(res.HasColumn('created_at_hour'), 'Hour component column missing');
  Check(res.HasColumn('created_at_dayofweek'), 'DayOfWeek component column missing');
  Check(res.GetColumnType('created_at_date') = ColumnType.ctDateTime, 'Date component type mismatch');
  Check(res.GetColumnType('created_at_hour') = ColumnType.ctInt, 'Hour component type mismatch');
  Check(res.DateTimeValues('created_at_date')[0] = new System.DateTime(2024, 1, 15), 'Date component value mismatch');
  Check(res.IntValues('created_at_hour')[1] = 12, 'Hour component value mismatch');
  CheckSchemaMatchesColumns(res);
end.
