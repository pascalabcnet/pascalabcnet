// Первый пример работы с pipeline.
//
// В этом примере:
// 1) загружаем готовый датасет Iris;
// 2) делим данные на обучающую и тестовую выборки;
// 3) собираем pipeline из масштабирования и модели;
// 4) обучаем pipeline;
// 5) оцениваем точность и смотрим вероятности классов.
//
// Pipeline удобен тем, что он сам выполняет все шаги по порядку:
// подготовку признаков, обучение модели и предсказание.
uses MLABC;

begin
  // Загружаем учебный датасет Iris
  var ds := Datasets.Iris;

  // Берём таблицу данных из датасета
  var df := ds.Data;

  // Делим данные на обучающую и тестовую выборки
  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 3);

  // Создаём pipeline:
  // сначала масштабируем признаки,
  // затем обучаем логистическую регрессию
  var pipe :=
    DataPipeline.Build(
      TaskKind.tkClassification,
      ds.Target,
      ds.Features,
      new StandardScaler,
      new LogisticRegression
    );

  // Обучаем весь pipeline на обучающей выборке
  pipe.Fit(trainDf);

  // Получаем предсказанные классы для тестовой выборки
  var pred := pipe.Predict(testDf);

  // Получаем правильные метки классов в кодировке pipeline
  var y := pipe.GetEncodedLabels(testDf);

  // Считаем долю правильных ответов
  Println($'Точность: {Metrics.Accuracy(y, pred):F3}');

  // Получаем вероятности классов для объектов тестовой выборки
  var proba := pipe.PredictProba(testDf);

  // Получаем названия классов в правильном порядке
  var classes := pipe.GetClassLabels;

  Println($'Классы: {classes.JoinToString('', '')}');
  Println('Вероятности для первого объекта:');

  for var j := 0 to classes.Length - 1 do
    Println($'{classes[j]}: {proba[0, j]:F3}');
end.
