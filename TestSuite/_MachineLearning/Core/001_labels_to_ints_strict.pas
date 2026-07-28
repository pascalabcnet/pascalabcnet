uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddIntColumn('x', Arr(1, 2, 3));
  df.AddStrColumn('Target', Arr('cat', 'dog', 'cat'));

  var ds := Dataset.FromData(df, TaskType.Classification, [$'x'], 'Target');

  var enc := new LabelEncoder;
  var y := enc.FitTransform(ds);

  Check(y.Length = 3, 'Length mismatch');
  Check(y[0] = 0, 'First label mismatch');
  Check(y[1] = 1, 'Second label mismatch');
  Check(y[2] = 0, 'Third label mismatch');
  Check(enc.Classes.Length = 2, 'Class count mismatch');
  Check(enc.Classes[0] = 'cat', 'First class mismatch');
  Check(enc.Classes[1] = 'dog', 'Second class mismatch');
end.
