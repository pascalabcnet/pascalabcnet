uses MLABC;

begin
  var X: Matrix := 
    [[1.0,1.0],[1.2,1.3],[1.5,1.2],[1.8,1.6],[2.0,1.9],[2.2,2.1],
    [2.4,2.3],[2.6,2.5],[2.8,2.7],[3.0,3.0],[3.2,3.1],[3.4,3.3]];
  
  var y: Vector := 
    [0.0,0.0,0.0,0.0,0.0,0.0, 1.0,1.0,1.0,1.0,1.0,1.0];

  var (Xtrain, Xtest, ytrain, ytest) :=
    Validation.TrainTestSplit(X, y, 0.3, seed := 4);
  
  var model := new LogisticRegression;
  model.Fit(Xtrain, ytrain);
  
  var pred := model.Predict(Xtest);
  
  Println('Accuracy:', Metrics.Accuracy(ytest, pred):0:3);
end.