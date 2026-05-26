uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var trainDf := new DataFrame;
  trainDf.AddFloatColumn('X', Arr(0.0, 1.0, 0.0, 1.0));
  trainDf.AddStrColumn('Target', Arr('cat', 'dog', 'cat', 'dog'));
  trainDf := trainDf.SetCategorical(['Target']);

  var testDf := new DataFrame;
  testDf.AddFloatColumn('X', Arr(0.5, 0.7));
  testDf.AddStrColumn('Target', Arr('cat', 'fox'));
  testDf := testDf.SetCategorical(['Target']);

  var pipe := DataPipeline.Build(
    TaskKind.tkClassification,
    'Target',
    Arr($'X'),
    new LogisticRegression
  );

  pipe.Fit(trainDf);

  CheckRaises(procedure -> begin var y := pipe.GetEncodedLabels(testDf); end,
    'GetEncodedLabels must reject unseen target classes');
end.
