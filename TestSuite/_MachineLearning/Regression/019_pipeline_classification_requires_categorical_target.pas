uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddFloatColumn('X', Arr(1.0, 2.0, 3.0, 4.0));
  df.AddStrColumn('Target', Arr('cat', 'dog', 'cat', 'dog'));

  var pipe := DataPipeline.Build(
    TaskKind.tkClassification,
    'Target',
    Arr($'X'),
    new LogisticRegression
  );

  CheckRaises(procedure -> begin pipe.Fit(df); end,
    'Classification pipeline must require categorical target');
end.
