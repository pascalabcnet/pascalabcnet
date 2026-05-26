// В этом примере сравниваются границы решений
// логистической регрессии и дерева решений.

uses MLABC, PlotML;

begin
  var (X, y) := Datasets.MakeMoons(
    n := 300,
    noise := 0.18,
    seed := 42
  );

  var logreg := new LogisticRegression(learningRate := 0.05, epochs := 1000);
  logreg.Fit(X, y);
  var accLR := ClassificationMetrics.Accuracy(y, logreg.Predict(X));

  var tree := new DecisionTreeClassifier(maxDepth := 5, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42);
  tree.Fit(X, y);
  var accTree := ClassificationMetrics.Accuracy(y, tree.Predict(X));

  var x1 := X.Col(0);
  var x2 := X.Col(1);
  var labels := LabelsToInts(y);

  var fig := Plot.Grid(1, 2);

  fig[0,0].Surface(x1, x2, 80, 80, G -> logreg.PredictLabels(G), Palettes.Pastel);
  fig[0,0].Points(x1, x2, labels, size := 6);
  fig[0,0].Title := $'LogisticRegression (Acc = {accLR:F3})';

  fig[0,1].Surface(x1, x2, 80, 80, G -> tree.PredictLabels(G), Palettes.Pastel);
  fig[0,1].Points(x1, x2, labels, size := 6);
  fig[0,1].Title := $'DecisionTreeClassifier (Acc = {accTree:F3})';
end.
