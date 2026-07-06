// Пример может работать несколько секунд
uses MLABC;

begin
  var ds := Datasets.MnistSmall;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target);

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.2, seed := 42);

  // Для изображений полезно привести яркости пикселей к сопоставимому масштабу.
  var scaler := new MinMaxScaler;
  Xtrain := scaler.FitTransform(Xtrain);
  Xtest := scaler.Transform(Xtest);

  var model := new LogisticRegression(
    learningRate := 1.0,
    epochs := 50
  );

  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);
  var acc := Metrics.Accuracy(ytest, pred);

  Println('MnistSmall: классификация рукописных цифр');
  Println($'Accuracy на тестовой выборке: {acc:F3}');
end.
