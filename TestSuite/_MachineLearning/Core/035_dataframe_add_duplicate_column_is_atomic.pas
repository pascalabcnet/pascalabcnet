uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Age', Arr(10, 20, 30));

  CheckRaises(procedure -> begin
    df.AddIntColumn('Age', Arr(1, 2, 3));
  end, 'Duplicate AddIntColumn must be rejected');

  Check(df.ColumnCount = 1, 'ColumnCount must stay unchanged after duplicate AddIntColumn');
  Check(df.HasColumn('Age'), 'Original column must stay present');
  CheckSchemaMatchesColumns(df, Arr(false));
end.
