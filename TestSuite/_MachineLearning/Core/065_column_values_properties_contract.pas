uses MLABC, DataFrameABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
age,score,name,active,created_at
14,91.5,Anna,True,2024-09-01
15,84.0,Boris,False,2024-09-02
''');

  Check(df['age'].IntValues[1] = 15, 'Column IntValues mismatch');
  Check(Abs(df['score'].FloatValues[0] - 91.5) < 1e-9, 'Column FloatValues mismatch');
  Check(df['name'].StrValues[1] = 'Boris', 'Column StrValues mismatch');
  Check(not df['active'].BoolValues[1], 'Column BoolValues mismatch');
  Check(df['created_at'].DateTimeValues[0] = new System.DateTime(2024, 9, 1),
    'Column DateTimeValues mismatch');

  CheckRaises(procedure -> begin
    var values := df['name'].IntValues;
  end, 'IntValues must reject a string column');

  var numericDf := DataFrame.FromCsvText('''
1,2
3,4
''', hasHeader := False);
  var allColumns := numericDf.ToMatrix;
  Check((allColumns.RowCount = 2) and (allColumns.ColCount = 2),
    'ToMatrix without columns shape mismatch');
  Check(allColumns[1, 0] = 3, 'ToMatrix without columns value mismatch');

  var selectedColumn := numericDf.ToMatrix([1]);
  Check((selectedColumn.RowCount = 2) and (selectedColumn.ColCount = 1),
    'ToMatrix by indices shape mismatch');
  Check(selectedColumn[0, 0] = 2, 'ToMatrix by indices value mismatch');
end.
