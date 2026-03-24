// В этом примере зависимость между признаками и целевой переменной
// специально сделана нелинейной. Линейная регрессия не умеет
// аппроксимировать такую зависимость, поэтому даёт большую ошибку
// и на обучении, и на тесте. Это не переобучение, а недообучение:
// модель слишком простая для задачи.

// RandomForest и GradientBoosting работают намного лучше,
// потому что деревья решений умеют моделировать сложные нелинейные связи.
// GradientBoosting показывает лучший результат,
// так как особенно хорошо приближает сложные зависимости.
uses MLABC;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 300,
    nFeatures := 2,
    nInformative := 2,
    noise := 0.1,
    coefScale := 1.0,
    bias := 0.0,
    nonlinearStrength := 5.0,  // КЛЮЧЕВОЕ !!!
    shuffle := True,
    seed := 42
  );

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(
    X, y, testRatio := 0.3, seed := 42
  );

  var lr := new LinearRegression;
  lr.Fit(XTrain, yTrain);

  var rf := new RandomForestRegressor(100);
  rf.Fit(XTrain, yTrain);

  var gb := new GradientBoostingRegressor(100);
  gb.Fit(XTrain, yTrain);

  var yTrainLR := lr.Predict(XTrain);
  var yTestLR  := lr.Predict(XTest);

  var yTrainRF := rf.Predict(XTrain);
  var yTestRF  := rf.Predict(XTest);

  var yTrainGB := gb.Predict(XTrain);
  var yTestGB  := gb.Predict(XTest);

  Println('nonlinearStrength = 5.0');
  Println;
  Println('--- Train ---');
  Println($'Linear  MSE: {RegressionMetrics.MSE(yTrain, yTrainLR),0:F4}');
  Println($'Forest  MSE: {RegressionMetrics.MSE(yTrain, yTrainRF),0:F4}');
  Println($'Boost   MSE: {RegressionMetrics.MSE(yTrain, yTrainGB),0:F4}');

  Println;
  Println('--- Test ---');
  Println($'Linear  MSE: {RegressionMetrics.MSE(yTest, yTestLR),0:F4}');
  Println($'Forest  MSE: {RegressionMetrics.MSE(yTest, yTestRF),0:F4}');
  Println($'Boost   MSE: {RegressionMetrics.MSE(yTest, yTestGB),0:F4}');
end.