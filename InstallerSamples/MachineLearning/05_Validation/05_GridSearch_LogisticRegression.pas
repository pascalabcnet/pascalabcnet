// В этом примере подбирается коэффициент регуляризации
// для LogisticRegression с помощью GridSearch.

uses MLABC;

begin
  var (X, y) := Datasets.MakeClassification(
    n := 400,
    nFeatures := 6,
    nInformative := 3,
    nRedundant := 1,
    noise := 0.2,
    classSep := 1.0,
    flipProb := 0.06,
    seed := 42
  );

  var scaler := new StandardScaler;
  scaler.Fit(X);
  X := scaler.Transform(X);

  var lambdaValues := [0.0, 0.01, 0.1, 0.5, 1.0, 2.0, 5.0];

  Println('Подбор параметра lambda для LogisticRegression');
  Println;
  Println('Проверяемые значения lambda:');

  foreach var lambda in lambdaValues do
  begin
    var score := Validation.StratifiedCrossValidate(
      new LogisticRegression(lambda := lambda, learningRate := 0.05, epochs := 1000),
      X, y,
      5,
      ClassificationMetrics.Accuracy,
      seed := 42
    );

    Println($'  lambda = {lambda,4:F2}  ->  средняя Accuracy = {score:F3}');
  end;

  Println;

  var (bestLambda, bestScore, bestModel) := GridSearch.Search(
    lambda -> new LogisticRegression(lambda := lambda, learningRate := 0.05, epochs := 1000),
    lambdaValues,
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    maximize := True,
    stratified := True,
    seed := 42
  );

  Println($'Лучшее значение lambda:   {bestLambda:F2}');
  Println($'Лучшая средняя Accuracy:  {bestScore:F3}');
end.
