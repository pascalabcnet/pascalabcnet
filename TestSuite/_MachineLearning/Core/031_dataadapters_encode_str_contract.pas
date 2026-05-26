uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var df := new DataFrame;
  df.AddStrColumn('Target', Arr('cat', 'dog', 'cat', 'fox'));
  df := df.SetCategorical(['Target']);

  var classes: array of string;
  var y := df.EncodeLabels('Target', classes);

  Check(y.Length = 4, 'Encoded label length mismatch');
  Check(classes.Length = 3, 'Class count mismatch');
  Check(classes[0] = 'cat', 'First class must follow first appearance');
  Check(classes[1] = 'dog', 'Second class must follow first appearance');
  Check(classes[2] = 'fox', 'Third class must follow first appearance');
  Check(y[0] = 0, 'First encoded label mismatch');
  Check(y[1] = 1, 'Second encoded label mismatch');
  Check(y[2] = 0, 'Third encoded label mismatch');
  Check(y[3] = 2, 'Fourth encoded label mismatch');

  var y2 := df.TransformLabels('Target', classes);
  for var i := 0 to y.Length - 1 do
    Check(y2[i] = y[i], $'TransformLabels mismatch at {i}');
end.
