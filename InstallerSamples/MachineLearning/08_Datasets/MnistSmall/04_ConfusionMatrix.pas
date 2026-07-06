// Программа работает несколько секунд - набор данных mnistSmall - достаточно большой (5000 записей)
uses MLABC, PlotML;

begin
  var ds := Datasets.MnistSmall;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeTarget(ds.Target);

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y.Labels, testRatio := 0.2, seed := 42);

  var model := new RandomForestClassifier(
    nTrees := 20,
    maxDepth := 20,
    seed := 42
  );

  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);
  var acc := Metrics.Accuracy(ytest, pred);
  
  var cm := new ConfusionMatrix(ytest, pred, y.ClassNames);
  cm.Println(normalize := MatrixNormalization.Rows, sortClassNames := True);  

  Plot.ConfusionMatrix(cm, 
    classNames := y.ClassNames,
    normalize := MatrixNormalization.Rows,
    sortClassNames := True);
  Plot.Title := $'MnistSmall Confusion Matrix: RandomForestClassifier, accuracy = {acc:F3}';
  Plot.XLabel := 'Предсказанная цифра';
  Plot.YLabel := 'Истинная цифра';
end.
