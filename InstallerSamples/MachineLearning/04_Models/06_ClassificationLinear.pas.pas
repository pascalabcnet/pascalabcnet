// Линейно разделимая задача с шумом.
// LogisticRegression показывает наилучший результат,
// так как модель соответствует природе данных.
//
// DecisionTree переобучается (рваная граница) → хуже на тесте.
// RandomForest снижает переобучение и приближается к Logistic.
// KNN чувствителен к шуму и даёт промежуточный результат.
//
// Logistic  Acc: 0.8733
// Tree      Acc: 0.7800
// Forest    Acc: 0.8467
// KNN       Acc: 0.8533

uses MLABC, PlotML;

begin
  // --- данные
  var (X, y) := Datasets.MakeClassification(
    n := 500,
    nFeatures := 2,
    nInformative := 2,
    nRedundant := 0,
    noise := 0.5,
    classSep := 2.0,
    flipProb := 0.1,
    classBalance := 0.5,
    shuffle := True,
    seed := 1
  );

  var (XTrain, XTest, yTrain, yTest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.3, seed := 1);

  // --- модели
  var logreg := new LogisticRegression;
  logreg.Fit(XTrain, yTrain);

  var tree := new DecisionTreeClassifier;
  tree.Fit(XTrain, yTrain);

  var rf := new RandomForestClassifier(100);
  rf.Fit(XTrain, yTrain);

  var knn := new KNNClassifier(5);
  knn.Fit(XTrain, yTrain);

  // --- предсказания (для визуализации)
  var yLR := logreg.PredictLabels(X);
  var yTree := tree.PredictLabels(X);
  var yRF := rf.PredictLabels(X);
  var yKNN := knn.PredictLabels(X);

  // --- test предсказания
  var yLR_test := logreg.PredictLabels(XTest);
  var yTree_test := tree.PredictLabels(XTest);
  var yRF_test := rf.PredictLabels(XTest);
  var yKNN_test := knn.PredictLabels(XTest);

  // --- метрики
  var accLR := Metrics.Accuracy(yTest, yLR_test);
  var accTree := Metrics.Accuracy(yTest, yTree_test);
  var accRF := Metrics.Accuracy(yTest, yRF_test);
  var accKNN := Metrics.Accuracy(yTest, yKNN_test);

  Println($'Logistic  Acc: {accLR,0:F4}');
  Println($'Tree      Acc: {accTree,0:F4}');
  Println($'Forest    Acc: {accRF,0:F4}');
  Println($'KNN       Acc: {accKNN,0:F4}');
  Println;

  // --- координаты
  var x1 := X.Col(0);
  var x2 := X.Col(1);

  // --- визуализация
  var fig := Plot.Grid(2, 2);

  fig[0,0].Points(x1, x2, yLR, size := 5);
  fig[0,0].Title := $'Logistic (acc={accLR,0:F3})';

  fig[0,1].Points(x1, x2, yTree, size := 5);
  fig[0,1].Title := $'Tree (acc={accTree,0:F3})';

  fig[1,0].Points(x1, x2, yRF, size := 5);
  fig[1,0].Title := $'Forest (acc={accRF,0:F3})';

  fig[1,1].Points(x1, x2, yKNN, size := 5);
  fig[1,1].Title := $'KNN (acc={accKNN,0:F3})';
end.