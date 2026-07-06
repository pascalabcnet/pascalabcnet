uses MLABC;

begin
var df := DataFrame.FromCsvText('''
  Вес,ВысотаВХолке,Порода
  20,33,бульдог
  17,43,спаниель
  16,41,спаниель
  25,36,бульдог
  26,34,бульдог
  21,35,бульдог
  24,35,бульдог
  20,36,бульдог
  14,39,спаниель
  15,40,спаниель
  19,39,бульдог
  18,42,спаниель
  22,34,бульдог
  15,38,спаниель
  19,40,спаниель
  16,40,спаниель
  ''');

  var X := df.ToMatrix(['Вес', 'ВысотаВХолке']);
  var target := df.EncodeTarget('Порода');
  var y := target.Labels;

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, testRatio := 0.25, seed := 42);

  var model := new KNNClassifier(3);
  model.Fit(Xtrain, ytrain);

  var ypred := model.Predict(Xtest);

  var cm := new ConfusionMatrix(ytest, ypred, target.ClassNames);

  cm.Println;
  Println;
  Println('Точность: ', Metrics.Accuracy(ytest, ypred):0:3);
end.
