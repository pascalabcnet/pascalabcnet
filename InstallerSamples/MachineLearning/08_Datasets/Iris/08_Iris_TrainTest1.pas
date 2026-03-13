uses MLABC;

  //var model := new LogisticRegression;
  //var model := new RandomForestClassifier();
  //var model := new DecisionTreeClassifier();
  //var model := new GradientBoostingClassifier();


begin
  var ds := Datasets.Iris;
  
  var (train, test) := ds.TrainTestSplit;
  
  var (Xtrain, ytrain) := train.ToXY;
  var (Xtest, ytest) := test.ToXY;
  
  var model := new RandomForestClassifier(seed := -1);
  
  model.Fit(Xtrain, ytrain);
  
  var pred := model.Predict(Xtest);
  
  var acc := Metrics.Accuracy(ytest, pred);
  
  Println('Accuracy:', acc:0:3);  
end.