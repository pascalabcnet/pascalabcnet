// В этом примере сравниваются несколько моделей
// на одной задаче регрессии.

uses MLABC;

begin
  var (X, y) := Datasets.MakeRegression(
    n := 400,
    nFeatures := 8,
    nInformative := 3,
    noise := 0.35,
    nonlinearStrength := 1.5,
    seed := 42
  );

  Println('Сравнение моделей регрессии по кросс-валидации');
  Println;

  var linScore := Validation.CrossValidate(
    new LinearRegression,
    X, y,
    5,
    RegressionMetrics.R2,
    seed := 42
  );

  var treeScore := Validation.CrossValidate(
    new DecisionTreeRegressor(maxDepth := 6, minSamplesSplit := 10, minSamplesLeaf := 5, seed := 42),
    X, y,
    5,
    RegressionMetrics.R2,
    seed := 42
  );

  var forestScore := Validation.CrossValidate(
    new RandomForestRegressor(nTrees := 120, maxDepth := 8, minSamplesSplit := 10, minSamplesLeaf := 5, seed := 42),
    X, y,
    5,
    RegressionMetrics.R2,
    seed := 42
  );

  var gbScore := Validation.CrossValidate(
    new GradientBoostingRegressor(nEstimators := 100, learningRate := 0.1, maxDepth := 3, minSamplesSplit := 10, minSamplesLeaf := 5, seed := 42),
    X, y,
    5,
    RegressionMetrics.R2,
    seed := 42
  );

  Println($'LinearRegression:           R² = {linScore:F3}');
  Println($'DecisionTreeRegressor:      R² = {treeScore:F3}');
  Println($'RandomForestRegressor:      R² = {forestScore:F3}');
  Println($'GradientBoostingRegressor:  R² = {gbScore:F3}');
end.
