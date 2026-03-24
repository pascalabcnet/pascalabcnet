// В датасете MakeMoons классы разделяются нелинейной границей.
// Поэтому LogisticRegression, строящая линейную разделяющую поверхность,
// показывает худший результат.

// DecisionTreeClassifier идеально подгоняет обучающую выборку,
// но на тесте заметно уступает, что указывает на переобучение.

// RandomForest и GradientBoosting значительно лучше работают
// с нелинейной структурой данных за счёт ансамблей деревьев.

// KNNClassifier показывает лучший результат на тесте,
// потому что это локальная модель, хорошо улавливающая
// сложную геометрию классов.
uses MLABC;

begin
  var (X, y) := Datasets.MakeMoons(
    n := 300,
    noise := 0.2,
    shuffle := True,
    seed := 42
  );

  var (XTrain, XTest, yTrain, yTest) := Validation.TrainTestSplit(
    X, y, testRatio := 0.3, seed := 42
  );

  var logreg := new LogisticRegression;
  logreg.Fit(XTrain, yTrain);

  var tree := new DecisionTreeClassifier;
  tree.Fit(XTrain, yTrain);

  var rf := new RandomForestClassifier(100);
  rf.Fit(XTrain, yTrain);

  var gb := new GradientBoostingClassifier(100);
  gb.Fit(XTrain, yTrain);

  var knn := new KNNClassifier(5);
  knn.Fit(XTrain, yTrain);

  // --- предсказания
  var yTrainLR := logreg.Predict(XTrain);
  var yTestLR  := logreg.Predict(XTest);

  var yTrainTree := tree.Predict(XTrain);
  var yTestTree  := tree.Predict(XTest);

  var yTrainRF := rf.Predict(XTrain);
  var yTestRF  := rf.Predict(XTest);

  var yTrainGB := gb.Predict(XTrain);
  var yTestGB  := gb.Predict(XTest);

  var yTrainKNN := knn.Predict(XTrain);
  var yTestKNN  := knn.Predict(XTest);

  // --- вывод accuracy
  Println('--- Train ---');
  Println($'Logistic  Acc: {Metrics.Accuracy(yTrain, yTrainLR),0:F4}');
  Println($'Tree      Acc: {Metrics.Accuracy(yTrain, yTrainTree),0:F4}');
  Println($'Forest    Acc: {Metrics.Accuracy(yTrain, yTrainRF),0:F4}');
  Println($'Boost     Acc: {Metrics.Accuracy(yTrain, yTrainGB),0:F4}');
  Println($'KNN       Acc: {Metrics.Accuracy(yTrain, yTrainKNN),0:F4}');

  Println;
  Println('--- Test ---');
  Println($'Logistic  Acc: {Metrics.Accuracy(yTest, yTestLR),0:F4}');
  Println($'Tree      Acc: {Metrics.Accuracy(yTest, yTestTree),0:F4}');
  Println($'Forest    Acc: {Metrics.Accuracy(yTest, yTestRF),0:F4}');
  Println($'Boost     Acc: {Metrics.Accuracy(yTest, yTestGB),0:F4}');
  Println($'KNN       Acc: {Metrics.Accuracy(yTest, yTestKNN),0:F4}');
end.