// Pipeline для регрессии на DataFrame с категориальным признаком.
//
// В этом примере:
// • числовые признаки подаются в модель напрямую;
// • категориальный признак renovation кодируется через OneHotEncoder;
// • затем все признаки масштабируются;
// • после этого обучается линейная регрессия.
uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms', 'area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes', 'renovation'];
  var target := 'price';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 42);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkRegression,
      target,
      features,
      new OneHotEncoder('renovation'),
      new StandardScaler,
      new LinearRegression
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := testDf.ToVector(target);

  Println($'R²: {Metrics.R2(y, pred):F3}');
end.
