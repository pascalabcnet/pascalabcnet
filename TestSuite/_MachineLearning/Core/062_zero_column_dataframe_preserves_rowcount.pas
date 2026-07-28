uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
a,b,c
1,10,x
2,20,y
3,30,z
''');

  var selected := df.Select(new integer[0]);
  Check(selected.RowCount = 3, 'Select([]) must preserve row count');
  Check(selected.ColumnCount = 0, 'Select([]) must produce 0 columns');

  var dropped := df.Drop(df.ColumnNames);
  Check(dropped.RowCount = 3, 'Drop(all columns) must preserve row count');
  Check(dropped.ColumnCount = 0, 'Drop(all columns) must produce 0 columns');

  var sliced := df.TakeRows([0, 2]).Select(new integer[0]);
  Check(sliced.RowCount = 2, 'TakeRows(...).Select([]) must preserve row count');
  Check(sliced.ColumnCount = 0, 'TakeRows(...).Select([]) must produce 0 columns');
end.
