uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  // Берём только числовые признаки, чтобы дерево было компактным и понятным.
  var features := ['rooms', 'area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes'];
  var target := 'price';

  var (trainDf, testDf) := df.TrainTestSplit(0.2, seed := 42);

  var Xtrain := trainDf.ToMatrix(features);
  var ytrain := trainDf.ToVector(target);
  var Xtest := testDf.ToMatrix(features);
  var ytest := testDf.ToVector(target);

  // Небольшая глубина даёт дерево, которое ещё можно разобрать глазами.
  var model := new DecisionTreeRegressor(
    maxDepth := 3,
    minSamplesSplit := 20,
    minSamplesLeaf := 10,
    seed := 42
  );

  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);
  var r2 := Metrics.R2(ytest, pred);

  Println('Дерево решений для прогноза цены квартиры');
  Println($'R² на тестовой выборке = {r2:F3}');
  Println;
  Println('Текстовое представление дерева:');
  Println(model.Tree(features, nil).ToString);
end.
