uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
city
Moscow
Kazan
Moscow
Omsk
Kazan
Moscow
''');

  var values := df.Unique('city');
  Check(values.Length = 3, 'Unique length mismatch');
  Check(values[0].Str = 'Moscow', 'Unique first value mismatch');
  Check(values[1].Str = 'Kazan', 'Unique second value mismatch');
  Check(values[2].Str = 'Omsk', 'Unique third value mismatch');

  Check(df.NUnique('city') = 3, 'NUnique mismatch');

  var vc := df.ValueCounts('city');
  Check(vc.RowCount = 3, 'ValueCounts row count mismatch');
  Check(vc.StrValues('city')[0] = 'Moscow', 'ValueCounts first key mismatch');
  Check(vc.IntValues('Count')[0] = 3, 'ValueCounts first count mismatch');
  Check(vc.StrValues('city')[1] = 'Kazan', 'ValueCounts second key mismatch');
  Check(vc.IntValues('Count')[1] = 2, 'ValueCounts second count mismatch');
  Check(vc.StrValues('city')[2] = 'Omsk', 'ValueCounts third key mismatch');
  Check(vc.IntValues('Count')[2] = 1, 'ValueCounts third count mismatch');
  CheckSchemaMatchesColumns(vc);
end.
