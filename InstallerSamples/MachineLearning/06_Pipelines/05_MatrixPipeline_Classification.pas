// В этом примере показан supervised pipeline для матрицы признаков.
//
// Данные сначала преобразуются в матрицу X и вектор меток y,
// а затем pipeline выполняет масштабирование и обучение модели.
//
// Такой вариант удобен, когда признаки уже подготовлены
// и не требуется отдельная обработка DataFrame.

uses MLABC;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  // Извлекаем числовую матрицу признаков
  // и кодируем названия классов в числа.
  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target);

  // Делим данные на обучающую и тестовую выборки.
  var (trainDs, testDs) := ds.StratifiedTrainTestSplit(testRatio := 0.3, seed := 42);

  // Строим pipeline:
  // StandardScaler -> LogisticRegression.
  var pipe := MatrixPipeline.Build(
    new StandardScaler,
    new LogisticRegression(learningRate := 0.05, epochs := 1000)
  );

  // Обучаем pipeline и проверяем качество на тестовой выборке.
  var XTrain := trainDs.Data.ToMatrix(trainDs.Features);
  var classes: array of string;
  var yTrain := trainDs.Data.EncodeLabels(trainDs.Target, classes);

  var XTest := testDs.Data.ToMatrix(testDs.Features);
  var yTest := testDs.Data.TransformLabels(testDs.Target, classes);

  pipe.Fit(XTrain, yTrain);
  var pred := pipe.Predict(XTest);
  var acc := ClassificationMetrics.Accuracy(yTest, pred);

  Println('Классификация Iris с помощью MatrixPipeline');
  Println;
  Println($'Точность на тестовой выборке: {acc:F3}');
end.
