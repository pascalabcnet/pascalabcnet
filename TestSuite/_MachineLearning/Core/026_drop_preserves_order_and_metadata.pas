uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B'));
  df.AddIntColumn('Age', Arr(10, 20));
  df.AddFloatColumn('Score', Arr(1.0, 2.0));
  df := df.SetCategorical(['City']);

  var res := df.Drop(['Score']);

  Check(res.ColumnCount = 2, 'Drop column count mismatch');
  Check(res.RowCount = 2, 'Drop row count mismatch');
  Check(res.Schema.NameAt(0) = 'City', 'First remaining column mismatch');
  Check(res.Schema.NameAt(1) = 'Age', 'Second remaining column mismatch');
  CheckSchemaMatchesColumns(res, Arr(true, false));
end.
