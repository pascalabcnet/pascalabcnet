// В этом примере сравниваются границы решений
// дерева решений и случайного леса.

uses MLABC, PlotML;

begin
  var (X, y) := Datasets.MakeCircles(
    n := 300,
    noise := 0.3,
    factor := 0.5,
    flipProb := 0.08,
    scale := 3.0,
    seed := 42
  );

  var tree := new DecisionTreeClassifier(maxDepth := 6, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42);
  tree.Fit(X, y);
  var accTree := ClassificationMetrics.Accuracy(y, tree.Predict(X));

  var forest := new RandomForestClassifier(nTrees := 100, maxDepth := 6, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42);
  forest.Fit(X, y);
  var accForest := ClassificationMetrics.Accuracy(y, forest.Predict(X));

  var x1 := X.Col(0);
  var x2 := X.Col(1);
  var labels := LabelsToInts(y);

  var fig := Plot.Grid(1, 2);

  fig[0,0].Surface(x1, x2, 80, 80, G -> tree.PredictLabels(G), Palettes.Pastel);
  fig[0,0].Points(x1, x2, labels, size := 6);
  fig[0,0].Title := $'DecisionTree (Acc = {accTree:F3})';

  fig[0,1].Surface(x1, x2, 80, 80, G -> forest.PredictLabels(G), Palettes.Pastel);
  fig[0,1].Points(x1, x2, labels, size := 6);
  fig[0,1].Title := $'RandomForest (Acc = {accForest:F3})';
end.
