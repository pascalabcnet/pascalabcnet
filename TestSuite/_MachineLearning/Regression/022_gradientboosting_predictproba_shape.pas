uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  var ds := Datasets.Iris;
  var X := ds.Data.ToMatrix(ds.Features);

  var classes: array of string;
  var y := ds.Data.EncodeLabels(ds.Target, classes);

  var model := new GradientBoostingClassifier(20, learningRate := 0.1, maxDepth := 3, seed := 1);
  model.Fit(X, y);

  var proba := model.PredictProba(X);

  Check(proba.RowCount = X.RowCount, 'Probability row count mismatch');
  Check(proba.ColCount = classes.Length, 'Probability class count mismatch');
  CheckProbabilityRowsSumToOne(proba);
end.
