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

  var dropped := df.DropMissing;
  Check(dropped.RowCount = 2, 'DropMissing all row count mismatch');
  Check(dropped.StrValues('name')[0] = 'Alice', 'DropMissing all first row mismatch');
  Check(dropped.StrValues('name')[1] = 'Dmitry', 'DropMissing all second row mismatch');
  CheckSchemaMatchesColumns(dropped);
end.
