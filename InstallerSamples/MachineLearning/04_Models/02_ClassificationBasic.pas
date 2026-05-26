// Базовый пример логистической регрессии на датасете Iris.
//
// Iris - удобный учебный датасет для первого знакомства с классификацией:
// классы в нём разделяются достаточно хорошо, поэтому простая линейная модель
// уже даёт высокую точность.
uses MLABC;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target);

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.2, seed := 1);

  var model := new LogisticRegression;
  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);
  var proba := model.PredictProba(Xtest);

  Println($'Точность на тестовой выборке: {Metrics.Accuracy(ytest, pred):F3}');
  Println;
  Println('Вероятности для первого объекта:');

  var classes := model.GetClasses;
  for var j := 0 to classes.Length - 1 do
    Println($'Класс {classes[j]:F0}: {proba[0, j]:F3}');
end.
