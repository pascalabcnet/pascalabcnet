uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb'));
  df.AddIntColumn('Age', Arr(20, 30));
  df := df.SetCategorical(['City']);

  CheckRaises(procedure -> begin
    df.SetSchema(new DataFrameSchema(
      Arr('Town', 'Age'),
      Arr(ColumnType.ctStr, ColumnType.ctInt),
      Arr(true, false)
    ));
  end, 'SetSchema must reject mismatched schema');

  Check(df.ColumnCount = 2, 'ColumnCount must stay unchanged after failed SetSchema');
  Check(df.Schema.NameAt(0) = 'City', 'Schema must stay unchanged after failed SetSchema');
  CheckSchemaMatchesColumns(df, Arr(true, false));
end.
