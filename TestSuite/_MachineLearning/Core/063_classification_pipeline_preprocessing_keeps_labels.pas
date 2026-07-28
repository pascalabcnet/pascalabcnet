uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := DataFrame.FromCsvText('''
x,target
1,cat
2,dog
3,cat
4,bird
''');

  var pipe := DataPipeline.BuildClassificationPreprocessing(
    'target',
    [$'x']
  );

  pipe.Fit(df);

  var classes := pipe.GetClassLabels;
  Check(classes.Length = 3, 'Class label count mismatch');
  Check(classes[0] = 'cat', 'First class label mismatch');
  Check(classes[1] = 'dog', 'Second class label mismatch');
  Check(classes[2] = 'bird', 'Third class label mismatch');

  var encoded := pipe.GetEncodedLabels(df);
  Check(encoded.Length = 4, 'Encoded label count mismatch');
  Check(encoded[0] = 0, 'First encoded label mismatch');
  Check(encoded[1] = 1, 'Second encoded label mismatch');
  Check(encoded[2] = 0, 'Third encoded label mismatch');
  Check(encoded[3] = 2, 'Fourth encoded label mismatch');
end.
