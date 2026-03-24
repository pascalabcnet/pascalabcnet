// Задача с концентрическими окружностями (MakeCircles).
// Классы вложены друг в друга, поэтому линейная разделяющая
// граница в принципе невозможна.
//
// LogisticRegression строит только линейную границу,
// поэтому полностью проваливается (сильное недообучение).
//
// DecisionTree способен моделировать нелинейную структуру,
// но делает это через разбиения по осям, из-за чего граница
// получается рваной и переобученной.
//
// RandomForest усредняет множество деревьев,
// сглаживает границу и даёт более устойчивый результат.
//
// KNNClassifier показывает лучший результат,
// так как является локальной моделью и хорошо восстанавливает
// форму вложенных областей.
//
// Визуализация наглядно показывает различия:
// • Logistic — почти прямая граница (не подходит)
// • Tree — "ступенчатая" структура (переобучение)
// • Forest — сглаженная версия дерева
// • KNN — наиболее близкая к истинной геометрии граница
uses MLABC, PlotML;

begin
  // --- данные
  var (X, y) := Datasets.MakeCircles(
    n := 400,
    noise := 0.2,
    factor := 0.5,
    classBalance := 0.5,
    flipProb := 0.05,
    scale := 3.0,
    shuffle := True,
    seed := 1
  );

  var (XTrain, XTest, yTrain, yTest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.3, seed := 1);

  // --- модели
  var logreg := new LogisticRegression;
  logreg.Fit(XTrain, yTrain);

  var tree := new DecisionTreeClassifier(seed := 1);
  tree.Fit(XTrain, yTrain);

  var rf := new RandomForestClassifier(100, seed := 1);
  rf.Fit(XTrain, yTrain);

  var knn := new KNNClassifier(5);
  knn.Fit(XTrain, yTrain);

  // --- метрики
  var accLR   := Metrics.Accuracy(yTest, logreg.PredictLabels(XTest));
  var accTree := Metrics.Accuracy(yTest, tree.PredictLabels(XTest));
  var accRF   := Metrics.Accuracy(yTest, rf.PredictLabels(XTest));
  var accKNN  := Metrics.Accuracy(yTest, knn.PredictLabels(XTest));

  Println('--- Circles ---');
  Println($'Logistic  Acc: {accLR,0:F4}');
  Println($'Tree      Acc: {accTree,0:F4}');
  Println($'Forest    Acc: {accRF,0:F4}');
  Println($'KNN       Acc: {accKNN,0:F4}');

  // --- данные для визуализации
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
  fig[0,1].Surface(x1, x2, nx, ny, G -> tree.PredictLabels(G), Palettes.Pastel);
  fig[0,1].Points(x1, x2, yArr, size := 6);
  fig[0,1].Title := $'Tree (acc={accTree,0:F3})';

  // Forest
  fig[1,0].Surface(x1, x2, nx, ny, G -> rf.PredictLabels(G), Palettes.Pastel);
  fig[1,0].Points(x1, x2, yArr, size := 6);
  fig[1,0].Title := $'Forest (acc={accRF,0:F3})';

  // KNN
  fig[1,1].Surface(x1, x2, nx, ny, G -> knn.PredictLabels(G), Palettes.Pastel);
  fig[1,1].Points(x1, x2, yArr, size := 6);
  fig[1,1].Title := $'KNN (acc={accKNN,0:F3})';
end.