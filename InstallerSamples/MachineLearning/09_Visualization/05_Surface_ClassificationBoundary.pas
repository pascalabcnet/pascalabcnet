// В этом примере Surface показывает,
// как модель делит плоскость на области классов.

uses MLABC, PlotML;

begin
  var (X, y) := Datasets.MakeMoons(
    n := 300,
    noise := 0.18,
    seed := 42
  );

  var model := new DecisionTreeClassifier(maxDepth := 5, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42);
  model.Fit(X, y);
  var acc := ClassificationMetrics.Accuracy(y, model.Predict(X));

  var x1 := X.Col(0);
  var x2 := X.Col(1);
  var labels := LabelsToInts(y);

  Println('Граница решений для DecisionTreeClassifier');
  Println;
  Println('Цветной фон показывает области, которые модель относит к разным классам.');
  Println($'Accuracy = {acc:F3}');

  Plot.Surface(x1, x2, 80, 80, G -> model.PredictLabels(G), Palettes.Pastel);
  Plot.Points(x1, x2, labels, size := 6);
  Plot.Title := $'DecisionTreeClassifier: decision boundary (Acc = {acc:F3})';
end.
