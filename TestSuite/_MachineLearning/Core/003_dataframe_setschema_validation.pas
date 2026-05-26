uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb'));
  df.AddIntColumn('Age', Arr(20, 30));
  df := df.SetCategorical(['City']);

  var badName := new DataFrameSchema(Arr('Town', 'Age'), Arr(ColumnType.ctStr, ColumnType.ctInt), Arr(true, false));
  CheckRaises(procedure -> begin df.SetSchema(badName); end, 'SetSchema must reject name mismatch');

  var badType := new DataFrameSchema(Arr('City', 'Age'), Arr(ColumnType.ctBool, ColumnType.ctInt), Arr(true, false));
  CheckRaises(procedure -> begin df.SetSchema(badType); end, 'SetSchema must reject type mismatch');
end.
