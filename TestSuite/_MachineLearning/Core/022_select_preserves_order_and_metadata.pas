uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B'));
  df.AddIntColumn('Age', Arr(10, 20));
  df := df.SetCategorical(['City']);

  var res := df.Select(['Age', 'City']);

  Check(res.Schema.ColumnCount = 2, 'Column count mismatch');
  Check(res.Schema.NameAt(0) = 'Age', 'First selected column mismatch');
  Check(res.Schema.NameAt(1) = 'City', 'Second selected column mismatch');
  Check(res.Schema.ColumnTypeAt(0) = ColumnType.ctInt, 'Age type mismatch');
  Check(res.Schema.ColumnTypeAt(1) = ColumnType.ctStr, 'City type mismatch');
  Check(res.Schema.IsCategoricalAt(1), 'City categorical flag must be preserved');
  CheckSchemaMatchesColumns(res, Arr(false, true));
end.
