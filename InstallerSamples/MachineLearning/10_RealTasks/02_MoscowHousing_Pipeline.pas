// Полный пример задачи регрессии:
// загружаем датасет, делим его на выборки,
// обучаем pipeline и оцениваем качество.

uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms', 'area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes', 'renovation'];
  var target := 'price';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 42);

  var pipe := DataPipeline.Build(
    TaskKind.tkRegression,
    target,
    features,
    new OneHotEncoder('renovation'),
    new StandardScaler,
    new LinearRegression
  );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var yTest := testDf.ToVector(target);
  var r2 := Metrics.R2(yTest, pred);

  Println('Прогноз цен на жильё: полный пример');
  Println;
  Println($'R² = {r2:F3}');
end.
