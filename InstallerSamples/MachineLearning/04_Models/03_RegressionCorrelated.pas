// В этом примере сравниваются три модели: LinearRegression, Ridge и ElasticNet.
// На обучении Linear даёт наименьшую ошибку, так как максимально подгоняет данные,
// но на тесте его ошибка резко растёт — это признак переобучения. Ridge слегка
// ухудшает качество на train, зато заметно снижает ошибку на test за счёт
// регуляризации (штрафа на величину коэффициентов). ElasticNet ведёт себя как
// компромисс: он тоже борется с переобучением и дополнительно может занулять
// часть коэффициентов, выполняя отбор признаков.

// По коэффициентам это видно особенно хорошо: у Linear они большие и нестабильные
// (из-за коррелированных признаков модель «раздувает» веса и компенсирует одни
// признаки другими). Ridge сглаживает веса — они становятся меньше и ближе друг
// к другу, что делает решение устойчивым. ElasticNet помимо сглаживания ещё и
// зануляет часть коэффициентов, оставляя только наиболее значимые признаки.

// Итог: Linear лучше подгоняет обучение, но переобучается; Ridge даёт лучший
// баланс и результат на тесте; ElasticNet — промежуточный вариант с эффектом
// отбора признаков.
uses MLABC;
uses System;

function Normal(rnd: Random): real;
begin
  var u1 := rnd.NextDouble;
  var u2 := rnd.NextDouble;
  if u1 < 1e-12 then u1 := 1e-12;
  Result := Sqrt(-2 * Ln(u1)) * Cos(2 * Pi * u2);
end;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 100,
    nFeatures := 20,
    nInformative := 3,
    noise := 2.0,
    coefScale := 1.0,
    bias := 0.0,
    nonlinearStrength := 0.0,
    shuffle := True,
    seed := 42
  );

  // --- корреляция
  var rnd := new Random(1);
  for var i := 0 to X.RowCount - 1 do
  begin
    X[i, 1] := X[i, 0] + 0.01 * Normal(rnd);
    X[i, 2] := X[i, 0] - 0.01 * Normal(rnd);
  end;

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(
    X, y, testRatio := 0.3, seed := 42
  );

  var lr := new LinearRegression;
  lr.Fit(XTrain, yTrain);

  var ridge := new RidgeRegression(50.0);
  ridge.Fit(XTrain, yTrain);

  var en := new ElasticNet(5.0, 0.2); // alpha, l1_ratio
  en.Fit(XTrain, yTrain);

  // --- предсказания
  var yTrainLR := lr.Predict(XTrain);
  var yTestLR  := lr.Predict(XTest);

  var yTrainRidge := ridge.Predict(XTrain);
  var yTestRidge  := ridge.Predict(XTest);

  var yTrainEN := en.Predict(XTrain);
  var yTestEN  := en.Predict(XTest);

  // --- вывод
  Println('--- Train ---');
  Println($'Linear     MSE: {Metrics.MSE(yTrain, yTrainLR),0:F4}');
  Println($'Ridge      MSE: {Metrics.MSE(yTrain, yTrainRidge),0:F4}');
  Println($'ElasticNet MSE: {Metrics.MSE(yTrain, yTrainEN),0:F4}');

  Println;
  Println('--- Test ---');
  Println($'Linear     MSE: {Metrics.MSE(yTest, yTestLR),0:F4}');
  Println($'Ridge      MSE: {Metrics.MSE(yTest, yTestRidge),0:F4}');
  Println($'ElasticNet MSE: {Metrics.MSE(yTest, yTestEN),0:F4}');
  
  {Println;
  Println('Коэффициенты (первые 10):');
  lr.Coefficients.ToArray.Take(10).Println;
  ridge.Coefficients.ToArray.Take(10).Println;
  en.Coefficients.ToArray.Take(10).Println;}
end.