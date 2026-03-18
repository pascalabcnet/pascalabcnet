uses MLABC;

procedure TestModel(model: IModel; Xtrain, Xtest: Matrix; ytrain, ytest: Vector);
begin
  model.Fit(Xtrain, ytrain);

  var pred := model.Predict(Xtest);

  var acc := Metrics.Accuracy(ytest, pred);

  Println(model.Name:26, ':', acc:0:3);
end;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target);

  var (Xtrain, Xtest, ytrain, ytest) := Validation.TrainTestSplit(X, y, 0.2, seed := 5);

  TestModel(new LogisticRegression, Xtrain, Xtest, ytrain, ytest);
  TestModel(new DecisionTreeClassifier(seed := -1), Xtrain, Xtest, ytrain, ytest);
  TestModel(new RandomForestClassifier(seed := -1), Xtrain, Xtest, ytrain, ytest);
  TestModel(new GradientBoostingClassifier(seed := -1), Xtrain, Xtest, ytrain, ytest);
end.