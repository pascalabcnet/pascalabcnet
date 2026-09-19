uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
name,age,income
Alice,20,100
Bob,,120
Clara,25,
Dmitry,30,150
''');

  Check(df.MissingCount('age') = 1, 'MissingCount(age) mismatch');
  Check(df.MissingCount('income') = 1, 'MissingCount(income) mismatch');

  var mc := df.MissingCounts;
  Check(mc.RowCount = 3, 'MissingCounts row count mismatch');
  Check(mc.StrValues('Column')[0] = 'name', 'MissingCounts first column mismatch');
  Check(mc.IntValues('MissingCount')[0] = 0, 'MissingCounts first count mismatch');
  Check(mc.StrValues('Column')[1] = 'age', 'MissingCounts second column mismatch');
  Check(mc.IntValues('MissingCount')[1] = 1, 'MissingCounts second count mismatch');
  Check(mc.StrValues('Column')[2] = 'income', 'MissingCounts third column mismatch');
  Check(mc.IntValues('MissingCount')[2] = 1, 'MissingCounts third count mismatch');

  var dropped := df.DropMissing(['age', 'income']);
  Check(dropped.RowCount = 2, 'DropMissing selected row count mismatch');
  Check(dropped.StrValues('name')[0] = 'Alice', 'DropMissing first row mismatch');
  Check(dropped.StrValues('name')[1] = 'Dmitry', 'DropMissing second row mismatch');
  CheckSchemaMatchesColumns(dropped);
end.
