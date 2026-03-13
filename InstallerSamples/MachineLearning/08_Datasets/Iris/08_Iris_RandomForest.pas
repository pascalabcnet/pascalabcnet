uses MLABC;

begin
  var ds := Datasets.Iris;

  var (X, y) := ds.ToXY;

  //var model := new LogisticRegression;
  //var model := new RandomForestClassifier();
  //var model := new DecisionTreeClassifier();
  var model := new GradientBoostingClassifier();
  
  model.Fit(X, y);

  var pred := model.Predict(X);

  var acc := Metrics.Accuracy(y, pred);

  Println('Accuracy:', acc:0:3);
end.