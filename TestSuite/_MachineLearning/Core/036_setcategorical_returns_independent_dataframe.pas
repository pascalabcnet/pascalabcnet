uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb'));
  df.AddIntColumn('Age', Arr(20, 30));

  var df2 := df.SetCategorical(['City']);
  df.AddIntColumn('X', Arr(1, 2));

  Check(df.ColumnCount = 3, 'Original DataFrame must have the new column');
  Check(df2.ColumnCount = 2, 'SetCategorical result must keep its own column list');
  Check(not df2.HasColumn('X'), 'SetCategorical result must not see later columns from source DataFrame');
  Check(df2.Schema.NameAt(0) = 'City', 'First schema name must stay intact');
  Check(df2.Schema.NameAt(1) = 'Age', 'Second schema name must stay intact');
  CheckSchemaMatchesColumns(df2, Arr(true, false));
end.
