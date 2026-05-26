uses MLABC;

begin
  var ds := Datasets.UsedCarsPrice;
  var df := ds.Data;

  // В этом примере смотрим не только на R²,
  // но и на конкретные ошибки модели для отдельных автомобилей.
  // Остаток = истинная цена - предсказанная цена.
  // Если остаток положительный, модель занизила цену.
  // Если отрицательный, модель завысила цену.

  var features := [
    'model',
    'year',
    'transmission',
    'mileage_km',
    'fuelType',
    'l_100km',
    'engineSize',
    'Make'
  ];

  var target := 'price_k_rub';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 42);

  var pipe :=
    DataPipeline.Build(
      TaskKind.tkRegression,
      target,
      features,
      new OrdinalEncoder('model'),
      new OneHotEncoder('transmission'),
      new OneHotEncoder('fuelType'),
      new OneHotEncoder('Make'),
      new StandardScaler,
      new RandomForestRegressor(12, 50)
    );

  pipe.Fit(trainDf);

  var pred := pipe.Predict(testDf);
  var y := testDf.ToVector(target);

  var models := testDf.Str('model');
  var makes := testDf.Str('Make');
  var years := testDf.Int('year');

  var idx := Arr(0..pred.Length - 1);
  idx := idx.OrderByDescending(i -> Abs(y[i] - pred[i])).ToArray;

  Println('UsedCars: самые большие ошибки модели');
  Println;
  Println('Остаток = истинная цена - предсказанная цена.');
  Println('Положительный остаток означает, что модель занизила цену.');
  Println('Отрицательный остаток означает, что модель завысила цену.');
  Println;

  for var k := 0 to Min(9, idx.Length - 1) do
  begin
    var i := idx[k];
    var residual := y[i] - pred[i];

    Println($'{makes[i]} {models[i]}, {years[i]}');
    Println($'  Истинная цена: {y[i]:F0} тыс. руб.');
    Println($'  Прогноз:       {pred[i]:F0} тыс. руб.');
    Println($'  Остаток:       {residual:F0} тыс. руб.');
    Println;
  end;
end.
