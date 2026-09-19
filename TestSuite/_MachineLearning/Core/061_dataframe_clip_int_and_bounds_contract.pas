uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
age
-5
10
30
''');

  var clipped := df.Clip('age', 0, 20);
  Check(clipped.RowCount = 3, 'Clip int row count mismatch');
  Check(clipped.IntValues('age')[0] = 0, 'Clip int first value mismatch');
  Check(clipped.IntValues('age')[1] = 10, 'Clip int second value mismatch');
  Check(clipped.IntValues('age')[2] = 20, 'Clip int third value mismatch');
  CheckSchemaMatchesColumns(clipped);

  CheckRaises(procedure -> begin
    var tmp := df.Clip('age', 0.5, 20);
  end, 'Clip must reject non-integer bounds for int column');

  CheckRaises(procedure -> begin
    var tmp := df.Clip('age', 10, 0);
  end, 'Clip must reject lower > upper');
end.
