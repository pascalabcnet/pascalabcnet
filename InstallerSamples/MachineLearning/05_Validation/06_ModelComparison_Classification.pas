// В этом примере сравниваются несколько моделей
// на одной задаче классификации.

uses MLABC;

begin
  var (X, y) := Datasets.MakeMoons(
    n := 400,
    noise := 0.18,
    seed := 42
  );

  Println('Сравнение моделей классификации по кросс-валидации');
  Println;

  var logregScore := Validation.StratifiedCrossValidate(
    new LogisticRegression(learningRate := 0.05, epochs := 1000),
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    seed := 42
  );

  var treeScore := Validation.StratifiedCrossValidate(
    new DecisionTreeClassifier(maxDepth := 5, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42),
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    seed := 42
  );

  var forestScore := Validation.StratifiedCrossValidate(
    new RandomForestClassifier(nTrees := 100, maxDepth := 6, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42),
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    seed := 42
  );

  var gbScore := Validation.StratifiedCrossValidate(
    new GradientBoostingClassifier(nEstimators := 80, learningRate := 0.1, maxDepth := 3, minSamplesSplit := 6, minSamplesLeaf := 3, seed := 42),
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    seed := 42
  );

  Println($'LogisticRegression:         Accuracy = {logregScore:F3}');
  Println($'DecisionTreeClassifier:     Accuracy = {treeScore:F3}');
  Println($'RandomForestClassifier:     Accuracy = {forestScore:F3}');
  Println($'GradientBoostingClassifier: Accuracy = {gbScore:F3}');
end.
