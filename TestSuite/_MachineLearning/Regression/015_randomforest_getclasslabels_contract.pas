uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;
  var X := ds.Data.ToMatrix(ds.Features);

  var classes: array of string;
  var y := ds.Data.EncodeLabels(ds.Target, classes);

  var model := new RandomForestClassifier(20, maxDepth := 6, seed := 1);
  model.Fit(X, y);

  var modelClasses := model.GetClassLabels;
  Check(modelClasses.Length = classes.Length, 'class count mismatch');

  for var i := 0 to classes.Length - 1 do
    Check(modelClasses[i] = i.ToString, $'RandomForestClassifier GetClassLabels mismatch at {i}');
end.
