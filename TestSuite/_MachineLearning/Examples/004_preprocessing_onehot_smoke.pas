uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('A', 'B', 'C'));
  df.AddStrColumn('Region', Arr('North', 'South', 'North'));
  df := df.SetCategorical(['City', 'Region']);

  var enc := new OneHotEncoder('Region');
  var res := enc.FitTransform(df);

  Check(res.HasColumn('City'), 'City must stay in result');
  Check(res.HasColumn('Region_North'), 'Region_North must exist');
  Check(res.HasColumn('Region_South'), 'Region_South must exist');
  Check(not res.HasColumn('Region'), 'Source region column must be removed');
  Check(res.RowCount = df.RowCount, 'Row count must stay the same');
end.
