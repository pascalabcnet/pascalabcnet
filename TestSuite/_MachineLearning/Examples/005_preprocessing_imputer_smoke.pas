uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B', 'C'));
  df.AddIntColumn('Population', Arr(10, 0, 30), Arr(true, false, true));

  var imp := new Imputer(['Population']);
  var res := imp.FitTransform(df);

  Check(res.RowCount = df.RowCount, 'Row count must stay the same');
  Check(res.HasColumn('City'), 'City must stay in result');
  Check(res.HasColumn('Population'), 'Population must stay in result');
  Check(res.GetColumn(res.ColumnIndex('Population')).Info.ColType = ColumnType.ctFloat, 'Mean-imputed population must become float');
end.
