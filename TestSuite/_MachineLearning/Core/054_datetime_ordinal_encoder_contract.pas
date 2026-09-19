uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := CsvLoader.LoadFromLines('''
id,created_at,name
1,15.01.2024 00:00:00,Alice
2,16.01.2024 12:00:00,Bob
3,18.01.2024 06:00:00,Charlie
'''.ToLines, inferTypes := True);

  var encDays := new DateTimeOrdinalEncoder('created_at');
  var daysDf := encDays.FitTransform(df);

  Check(not daysDf.HasColumn('created_at'), 'Source DateTime column must be replaced');
  Check(daysDf.GetColumnType('created_at_days') = ColumnType.ctFloat, 'Ordinal DateTime column must become float');
  Check(Abs(daysDf.FloatValues('created_at_days')[0] - 0.0) < 1e-9, 'First day offset mismatch');
  Check(Abs(daysDf.FloatValues('created_at_days')[1] - 1.5) < 1e-9, 'Second day offset mismatch');
  Check(Abs(daysDf.FloatValues('created_at_days')[2] - 3.25) < 1e-9, 'Third day offset mismatch');

  var encHours := new DateTimeOrdinalEncoder('created_at', 'created_at_hours', dtuHours);
  var hoursDf := encHours.FitTransform(df);

  Check(hoursDf.GetColumnType('created_at_hours') = ColumnType.ctFloat, 'Hour ordinal column type mismatch');
  Check(Abs(hoursDf.FloatValues('created_at_hours')[0] - 0.0) < 1e-9, 'First hour offset mismatch');
  Check(Abs(hoursDf.FloatValues('created_at_hours')[1] - 36.0) < 1e-9, 'Second hour offset mismatch');
  Check(Abs(hoursDf.FloatValues('created_at_hours')[2] - 78.0) < 1e-9, 'Third hour offset mismatch');
  CheckSchemaMatchesColumns(hoursDf);
end.
