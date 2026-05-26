// В этом примере SelectKBest оставляет
// только два самых полезных признака
// и показывает, как это влияет на качество модели.

uses MLABC;

begin
  var (X, y) := Datasets.MakeClassification(
    n := 200,
    nFeatures := 6,
    nInformative := 2,
    nRedundant := 0,
    noise := 0.1,
    classSep := 1.2,
    seed := 42
  );

  var featureNames := ['f1', 'f2', 'f3', 'f4', 'f5', 'f6'];

  Println('Размер до SelectKBest: ', X.RowCount, 'x', X.ColCount);

  var skb := new SelectKBest(2, FeatureScore.Correlation);
  skb.Fit(X, y);
  var X2 := skb.Transform(X);

  Println('Размер после SelectKBest: ', X2.RowCount, 'x', X2.ColCount);
  Println;
  Println('Отобранные признаки:');

  var selected := skb.SelectedFeatures;
  for var i := 0 to selected.Length - 1 do
    Println('  ', featureNames[selected[i]]);

  Println;

  var fullScore := Validation.StratifiedCrossValidate(
    new LogisticRegression(learningRate := 0.05, epochs := 1000),
    X, y,
    5,
    ClassificationMetrics.Accuracy,
    seed := 42
  );

  var reducedScore := Validation.StratifiedCrossValidate(
    new LogisticRegression(learningRate := 0.05, epochs := 1000),
    X2, y,
    5,
    ClassificationMetrics.Accuracy,
    seed := 42
  );

  Println('Сравнение качества LogisticRegression:');
  Println($'  Все признаки:      Accuracy = {fullScore:F3}');
  Println($'  После SelectKBest: Accuracy = {reducedScore:F3}');
end.
