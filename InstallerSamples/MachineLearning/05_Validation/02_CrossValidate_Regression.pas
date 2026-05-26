// В этом примере линейная регрессия оценивается двумя способами:
// 1. по одной обучающей и тестовой выборке;
// 2. по k-fold кросс-валидации.
//
// Для регрессии в качестве метрики используется R²:
// чем ближе значение к 1, тем лучше модель объясняет данные.

uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms', 'area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes'];
  var target := 'price';

  var X := df.ToMatrix(features);
  var y := df.ToVector(target);

  var (XTrain, XTest, yTrain, yTest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.2, seed := 1);

  var model := new LinearRegression;
  model.Fit(XTrain, yTrain);

  var yPred := model.Predict(XTest);
  var testR2 := RegressionMetrics.R2(yTest, yPred);

  var cvR2 := Validation.CrossValidate(
    new LinearRegression,
    X, y,
    5,
    RegressionMetrics.R2,
    1
  );

  Println('Оценка линейной регрессии двумя способами');
  Println;
  Println($'R² на тестовой выборке:              {testR2:F3}');
  Println($'Средний R² по кросс-валидации:       {cvR2:F3}');
  Println;
  Println('Интерпретация результата:');
  Println('- Одна тестовая выборка даёт быструю оценку качества.');
  Println('- Кросс-валидация усредняет результат по нескольким разбиениям.');
  Println('- Если значения близки, модель ведёт себя достаточно стабильно.');
end.
