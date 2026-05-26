uses MLABC, DataFrameABCCore;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('Age', Arr(20, 0, 40), Arr(true, false, true));

  var imp := new Imputer(25, ['Age']);
  var res := imp.FitTransform(df);

  Check(res.Schema.ColumnCount = 1, 'Unexpected column count');
  Check(res.Schema.NameAt(0) = 'Age', 'Column name mismatch');
  Check(res.Schema.ColumnTypeAt(0) = ColumnType.ctInt, 'Constant int imputation must preserve int type');
  Check(res.GetColumn(0).Info.ColType = ColumnType.ctInt, 'Physical column type must stay int');

  var age := res.GetIntColumn('Age');
  Check(age[0] = 20, 'First value mismatch');
  Check(age[1] = 25, 'Imputed value mismatch');
  Check(age[2] = 40, 'Third value mismatch');
end.
