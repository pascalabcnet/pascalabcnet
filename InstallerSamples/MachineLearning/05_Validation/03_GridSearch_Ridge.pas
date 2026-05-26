// В этом примере подбирается параметр регуляризации для RidgeRegression.
//
// Используются линейные данные с сильно коррелированными признаками.
// В такой ситуации Ridge часто работает устойчивее обычной линейной регрессии.
//
// Для подбора используется метрика MSE:
// чем меньше значение, тем лучше модель.

uses MLABC;
uses System;

function Normal(rnd: Random): real;
begin
  var u1 := rnd.NextDouble;
  var u2 := rnd.NextDouble;
  if u1 < 1e-12 then
    u1 := 1e-12;
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

  // Несколько признаков делаем почти копиями первого,
  // чтобы усилить корреляцию и сделать Ridge полезным.
  var rnd := new Random(1);
  for var i := 0 to X.RowCount - 1 do
  begin
    X[i, 1] := X[i, 0] + 0.01 * Normal(rnd);
    X[i, 2] := X[i, 0] - 0.01 * Normal(rnd);
  end;

  var lambdaValues := [0.0, 1.0, 5.0, 10.0, 20.0, 50.0, 100.0, 200.0, 500.0];

  Println('Подбор параметра для RidgeRegression');
  Println;
  Println('Проверяемые значения lambda:');

  foreach var lambda in lambdaValues do
  begin
    var score := Validation.CrossValidate(
      new RidgeRegression(lambda),
      X, y,
      5,
      RegressionMetrics.MSE,
      1
    );
    Println($'  lambda = {lambda,6:F2}  ->  средняя MSE = {score:F3}');
  end;

  Println;

  var (bestLambda, bestScore, bestModel) := GridSearch.Search(
    lambda -> new RidgeRegression(lambda),
    lambdaValues,
    X, y,
    5,
    RegressionMetrics.MSE,
    maximize := False,
    stratified := False,
    seed := 1
  );

  Println($'Лучшее значение lambda:   {bestLambda:F2}');
  Println($'Лучшая средняя MSE:       {bestScore:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- GridSearch перебирает несколько значений параметра lambda.');
  Println('- Для каждого значения качество оценивается по кросс-валидации.');
  Println('- Лучшим считается параметр с наименьшей средней ошибкой MSE.');
end.
