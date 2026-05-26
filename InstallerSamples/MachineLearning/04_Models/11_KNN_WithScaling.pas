// KNN чувствителен к масштабу признаков.
//
// В этом примере мы специально делаем часть признаков
// очень большими по масштабу. Без StandardScaler расстояние
// между объектами начинает определяться в основном этими
// признаками, и качество KNN ухудшается.
//
// После масштабирования все признаки снова становятся
// сопоставимыми, и модель работает лучше.
uses MLABC;

begin
  var (X, y) := Datasets.MakeClassification(
    n := 500,
    nFeatures := 4,
    nInformative := 2,
    nRedundant := 0,
    noise := 0.15,
    classSep := 2.2,
    flipProb := 0.02,
    classBalance := 0.5,
    shuffle := True,
    seed := 42
  );

  // Искусственно увеличиваем масштаб двух последних признаков.
  // Они начинают слишком сильно влиять на расстояние в KNN.
  for var i := 0 to X.RowCount - 1 do
  begin
    X[i, 2] *= 1000;
    X[i, 3] *= 1000;
  end;

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.25, seed := 42);

  // --- KNN без масштабирования
  var knnRaw := new KNNClassifier(7);
  knnRaw.Fit(Xtrain, ytrain);

  var predRaw := knnRaw.Predict(Xtest);
  var accRaw := Metrics.Accuracy(ytest, predRaw);

  // --- Масштабируем признаки
  var scaler := new StandardScaler;
  scaler.Fit(Xtrain);

  var XtrainScaled := scaler.Transform(Xtrain);
  var XtestScaled := scaler.Transform(Xtest);

  // --- KNN после масштабирования
  var knnScaled := new KNNClassifier(7);
  knnScaled.Fit(XtrainScaled, ytrain);

  var predScaled := knnScaled.Predict(XtestScaled);
  var accScaled := Metrics.Accuracy(ytest, predScaled);

  Println($'Точность KNN без масштабирования: {accRaw:F3}');
  Println($'Точность KNN после StandardScaler: {accScaled:F3}');
end.
