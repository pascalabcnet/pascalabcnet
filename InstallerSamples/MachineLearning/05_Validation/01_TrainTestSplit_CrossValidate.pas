// В этом примере одна и та же модель оценивается двумя способами:
// 1. по одной обучающей и тестовой выборке;
// 2. по стратифицированной k-fold кросс-валидации.
//
// Один train/test split даёт быструю, но более случайную оценку.
// Кросс-валидация обычно надёжнее, потому что усредняет результат
// по нескольким разбиениям данных.

uses MLABC;

begin
  var ds := Datasets.Iris;
  var (trainDs, testDs) := ds.StratifiedTrainTestSplit(testRatio := 0.2, seed := 3);

  var XTrain := trainDs.Data.ToMatrix(trainDs.Features);
  var yTrain := trainDs.Data.EncodeLabels(trainDs.Target);

  var XTest := testDs.Data.ToMatrix(testDs.Features);
  var yTest := testDs.Data.EncodeLabels(testDs.Target);

  var X := ds.Data.ToMatrix(ds.Features);
  var y := ds.Data.EncodeLabels(ds.Target);

  var model := new LogisticRegression;
  model.Fit(XTrain, yTrain);

  var yPred := model.Predict(XTest);
  var testAccuracy := Metrics.Accuracy(yTest, yPred);

  var cvAccuracy := Validation.StratifiedCrossValidate(
    new LogisticRegression,
    X, y,
    5,
    Metrics.Accuracy,
    seed := 1
  );

  Println('Оценка одной и той же модели двумя способами');
  Println;
  Println($'Точность на стратифицированной тестовой выборке:  {testAccuracy:F3}');
  Println($'Средняя точность по кросс-валидации:    {cvAccuracy:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Одна тестовая выборка даёт быстрый, но более случайный результат.');
  Println('- Кросс-валидация усредняет качество по нескольким разбиениям.');
  Println('- Поэтому её часто используют для более надёжной оценки модели.');
end.
