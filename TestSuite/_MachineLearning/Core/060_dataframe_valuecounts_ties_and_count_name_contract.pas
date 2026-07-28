uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
city
Moscow
Kazan
Moscow
Kazan
Omsk
''');

  var vc := df.ValueCounts('city');
  Check(vc.RowCount = 3, 'ValueCounts tie row count mismatch');
  Check(vc.Str('city')[0] = 'Moscow', 'ValueCounts tie first key mismatch');
  Check(vc.Int('Count')[0] = 2, 'ValueCounts tie first count mismatch');
  Check(vc.Str('city')[1] = 'Kazan', 'ValueCounts tie second key mismatch');
  Check(vc.Int('Count')[1] = 2, 'ValueCounts tie second count mismatch');
  Check(vc.Str('city')[2] = 'Omsk', 'ValueCounts tie third key mismatch');
  Check(vc.Int('Count')[2] = 1, 'ValueCounts tie third count mismatch');

  var df2 := DataFrame.FromCsvText('''
Count
1
2
1
''');

  var vc2 := df2.ValueCounts('Count', 'Frequency');
  Check(vc2.HasColumn('Count'), 'ValueCounts must keep source column name');
  Check(vc2.HasColumn('Frequency'), 'ValueCounts must use explicit counter column name');
  Check(vc2.Int('Count')[0] = 1, 'ValueCounts conflict first key mismatch');
  Check(vc2.Int('Frequency')[0] = 2, 'ValueCounts conflict first frequency mismatch');
  CheckSchemaMatchesColumns(vc2);
end.
