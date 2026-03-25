uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms', 'area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes', 'renovation'];

  var target := 'price';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, 42);

  var pipe :=
    DataPipeline.Build( // сборка pipeline: target + features + шаги
      target,           // целевая переменная
      features,         // список признаков
      new OneHotEncoder('renovation'), // DataFrame-уровень: кодирование категории
                                       // Внутри - преобразование .ToMatrix .ToVector
      new StandardScaler,              // Matrix-уровень: масштабирование признаков
      new LinearRegression             // модель (всегда последний шаг)
    );

  pipe.Fit(trainDf); // обучение: последовательно encoding → scaling → model

  var pred := pipe.Predict(testDf); // предсказание: те же преобразования
  var y := testDf.ToVector(target);

  Println('R²:', Metrics.R2(y, pred):0:3);
end.