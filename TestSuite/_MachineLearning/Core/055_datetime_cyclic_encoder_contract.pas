uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := CsvLoader.LoadFromLines('''
id,created_at,name
1,15.01.2024 00:00:00,Alice
2,15.01.2024 06:00:00,Bob
3,15.01.2024 12:00:00,Charlie
4,15.01.2024 18:00:00,Diana
5,15.01.2024 23:59:59,Eva
'''.ToLines, inferTypes := True);

  var enc := new DateTimeCyclicEncoder('created_at', dpTimeOfDay);
  var res := enc.FitTransform(df);

  Check(not res.HasColumn('created_at'), 'Source DateTime column must be replaced');
  Check(res.HasColumn('created_at_cyc_timeofday_sin'), 'TimeOfDay sin column missing');
  Check(res.HasColumn('created_at_cyc_timeofday_cos'), 'TimeOfDay cos column missing');
  Check(res.GetColumnType('created_at_cyc_timeofday_sin') = ColumnType.ctFloat, 'TimeOfDay sin type mismatch');
  Check(res.GetColumnType('created_at_cyc_timeofday_cos') = ColumnType.ctFloat, 'TimeOfDay cos type mismatch');

  Check(Abs(res.FloatValues('created_at_cyc_timeofday_sin')[0] - 0.0) < 1e-9, '00:00 sin mismatch');
  Check(Abs(res.FloatValues('created_at_cyc_timeofday_cos')[0] - 1.0) < 1e-9, '00:00 cos mismatch');

  Check(Abs(res.FloatValues('created_at_cyc_timeofday_sin')[1] - 1.0) < 1e-9, '06:00 sin mismatch');
  Check(Abs(res.FloatValues('created_at_cyc_timeofday_cos')[1] - 0.0) < 1e-9, '06:00 cos mismatch');

  Check(Abs(res.FloatValues('created_at_cyc_timeofday_sin')[2] - 0.0) < 1e-9, '12:00 sin mismatch');
  Check(Abs(res.FloatValues('created_at_cyc_timeofday_cos')[2] + 1.0) < 1e-9, '12:00 cos mismatch');

  Check(Abs(res.FloatValues('created_at_cyc_timeofday_sin')[3] + 1.0) < 1e-9, '18:00 sin mismatch');
  Check(Abs(res.FloatValues('created_at_cyc_timeofday_cos')[3] - 0.0) < 1e-9, '18:00 cos mismatch');

  Check(Abs(res.FloatValues('created_at_cyc_timeofday_sin')[4]) < 1e-3, '23:59:59 sin must be close to 0');
  Check(Abs(res.FloatValues('created_at_cyc_timeofday_cos')[4] - 1.0) < 1e-6, '23:59:59 cos must be close to 1');
  CheckSchemaMatchesColumns(res);
end.
