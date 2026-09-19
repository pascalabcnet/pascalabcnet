uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
name,age,score
Alice,10,1.5
Bob,25,4.8
Alice,10,1.5
Clara,30,8.2
''');

  var dedup := df.DropDuplicates;
  Check(dedup.RowCount = 3, 'DropDuplicates row count mismatch');
  Check(dedup.StrValues('name')[0] = 'Alice', 'DropDuplicates first row mismatch');
  Check(dedup.StrValues('name')[1] = 'Bob', 'DropDuplicates second row mismatch');
  Check(dedup.StrValues('name')[2] = 'Clara', 'DropDuplicates third row mismatch');
  CheckSchemaMatchesColumns(dedup);

  var clipped := df.Clip('score', 0, 5);
  Check(clipped.RowCount = 4, 'Clip row count mismatch');
  Check(Abs(clipped.FloatValues('score')[0] - 1.5) < 1e-9, 'Clip first value mismatch');
  Check(Abs(clipped.FloatValues('score')[1] - 4.8) < 1e-9, 'Clip second value mismatch');
  Check(Abs(clipped.FloatValues('score')[3] - 5.0) < 1e-9, 'Clip clipped value mismatch');
  CheckSchemaMatchesColumns(clipped);
end.
