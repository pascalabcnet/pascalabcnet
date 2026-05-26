uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb', 'Kzn'));
  df.AddStrColumn('Region', Arr('North', 'South', 'North'));
  df.AddIntColumn('Age', Arr(20, 30, 40));
  df := df.SetCategorical(['City', 'Region']);

  var enc := new OneHotEncoder('Region');
  var res := enc.FitTransform(df);

  Check(res.HasColumn('City'), 'Untouched categorical column must stay in result');
  Check(not res.HasColumn('Region'), 'Source column must be removed after one-hot encoding');
  Check(res.Schema.IsCategorical('City'), 'Untouched categorical metadata must be preserved');
  Check(res.Schema.ColumnTypeAt(res.ColumnIndex('City')) = ColumnType.ctStr, 'Untouched column type must stay string');
  Check(res.HasColumn('Region_North'), 'First one-hot column missing');
  Check(res.HasColumn('Region_South'), 'Second one-hot column missing');
  Check(res.Schema.ColumnTypeAt(res.ColumnIndex('Region_North')) = ColumnType.ctInt, 'One-hot columns must be int');
  Check(not res.Schema.IsCategorical('Region_North'), 'One-hot columns must not be categorical');
end.
