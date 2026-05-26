// Строго типизированный pipeline для классификации на DataFrame.
//
// Pipeline сам:
// • проверяет схему таблицы;
// • применяет preprocessing к признакам;
// • кодирует целевую переменную внутри себя;
// • передаёт матрицу признаков в модель.
//
// Здесь показан канонический сценарий:
// Iris -> Train/Test Split -> StandardScaler -> LogisticRegression.
uses MLABC;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 3);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      ds.Features,
      new StandardScaler,
      new LogisticRegression
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := pipe.GetEncodedLabels(testDf);

  Println('Точность:', Metrics.Accuracy(y, pred):0:3);

  var proba := pipe.PredictProba(testDf);
  var classes := pipe.GetClassLabels;

  Println('Классы:', classes.JoinToString(', '));
  Println('Вероятности для первого объекта:');

  for var j := 0 to classes.Length - 1 do
    Println(classes[j], ': ', proba[0, j]:0:3);
end.
