// Линейно разделимая задача с шумом.
// Классы в целом разделяются почти прямой линией, но из-за шума
// и случайных выбросов граница получается неидеальной.
//
// LogisticRegression показывает наилучший результат,
// так как её линейная модель хорошо соответствует структуре данных.
//
// DecisionTree строит кусочно-постоянную (рваную) границу,
// переобучается на шуме и даёт худшее качество на тесте.
//
// RandomForest сглаживает переобучение дерева за счёт ансамбля,
// но всё равно уступает линейной модели на этой задаче.
//
// KNN хорошо улавливает локальную структуру,
// но чувствителен к шуму и поэтому даёт промежуточный результат.
//
// Визуализация показывает:
// • Logistic — почти прямая граница
// • Tree — рваная, переобученная
// • Forest — сглаженная версия дерева
// • KNN — локально адаптивная граница
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

  // --- accuracy на test
  var accLR := Metrics.Accuracy(yTest, logreg.PredictLabels(XTest));
  var accTree := Metrics.Accuracy(yTest, tree.PredictLabels(XTest));
  var accRF := Metrics.Accuracy(yTest, rf.PredictLabels(XTest));
  var accKNN := Metrics.Accuracy(yTest, knn.PredictLabels(XTest));

  // --- область рисования
  var x1 := X.Col(0);
  var x2 := X.Col(1);
  var yArr := y.ToIntArray;

  var nx := 80;
  var ny := 80;

  // --- визуализация
  var fig := Plot.Grid(2, 2);

  // Logistic
  fig[0,0].Surface(x1, x2, nx, ny, G -> logreg.PredictLabels(G),Palettes.Pastel);
  fig[0,0].Points(x1, x2, yArr, size := 6);
  fig[0,0].Title := $'Logistic (acc={accLR,0:F3})';

  // Tree
  fig[0,1].Surface(x1, x2, nx, ny, G -> tree.PredictLabels(G),Palettes.Pastel);
  fig[0,1].Points(x1, x2, yArr, size := 6);
  fig[0,1].Title := $'Tree (acc={accTree,0:F3})';

  // Forest
  fig[1,0].Surface(x1, x2, nx, ny, G -> rf.PredictLabels(G),Palettes.Pastel);
  fig[1,0].Points(x1, x2, yArr, size := 6);
  fig[1,0].Title := $'Forest (acc={accRF,0:F3})';

  // KNN
  fig[1,1].Surface(x1, x2, nx, ny, G -> knn.PredictLabels(G),Palettes.Pastel);
  fig[1,1].Points(x1, x2, yArr, size := 6);
  fig[1,1].Title := $'KNN (acc={accKNN,0:F3})';
end.