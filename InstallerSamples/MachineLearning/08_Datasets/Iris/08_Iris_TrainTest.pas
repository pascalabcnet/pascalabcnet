uses MLABC;

  //var model := new LogisticRegression;
  //var model := new RandomForestClassifier();
  //var model := new DecisionTreeClassifier();
  //var model := new GradientBoostingClassifier();


begin
  var ds := Datasets.Iris;
  
  var (X, y) := ds.ToXY;
  
  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, 0.2);
  
  var model := new GradientBoostingClassifier(seed := -1);
  
  model.Fit(Xtrain, ytrain);
  
  var pred := model.Predict(Xtest);
  
  var acc := Metrics.Accuracy(ytest, pred);
  
  Println('Accuracy:', acc:0:3);  
end.