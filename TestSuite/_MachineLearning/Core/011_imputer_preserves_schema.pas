uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('City', Arr('Msk', 'Spb', 'Kzn'));
  df.AddIntColumn('Age', Arr(20, 0, 40), Arr(true, false, true));
  df := df.SetCategorical(['City']);

  var imp := new Imputer(['Age']);
  var res := imp.FitTransform(df);

  Check(res.Schema.ColumnCount = df.Schema.ColumnCount, 'Schema column count must stay the same');
  Check(res.Schema.NameAt(0) = 'City', 'City name mismatch');
  Check(res.Schema.NameAt(1) = 'Age', 'Age name mismatch');
  Check(res.Schema.ColumnTypeAt(0) = ColumnType.ctStr, 'City type must stay string');
  Check(res.Schema.ColumnTypeAt(1) = ColumnType.ctFloat, 'Age type must become float for mean imputation');
  Check(res.Schema.IsCategoricalAt(0), 'City categorical flag must be preserved');
  Check(not res.Schema.IsCategoricalAt(1), 'Age categorical flag must stay false');
end.
