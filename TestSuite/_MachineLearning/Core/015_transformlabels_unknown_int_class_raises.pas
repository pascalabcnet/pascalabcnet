uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var trainDf := new DataFrame;
  trainDf.AddIntColumn('Target', Arr(10, 20, 10));
  trainDf := trainDf.SetCategorical(['Target']);

  var testDf := new DataFrame;
  testDf.AddIntColumn('Target', Arr(10, 30));
  testDf := testDf.SetCategorical(['Target']);

  var classes: array of string;
  var yTrain := trainDf.EncodeLabels('Target', classes);

  Check(yTrain.Length = 3, 'Encoded training labels length mismatch');
  Check(classes.Length = 2, 'Unexpected class count');

  CheckRaises(procedure -> begin var tmp := testDf.TransformLabels('Target', classes); end,
    'TransformLabels must reject unseen integer target classes');
end.
