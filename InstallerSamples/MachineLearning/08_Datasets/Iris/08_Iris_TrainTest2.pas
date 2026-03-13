uses MLABC;

  //new LogisticRegression();
  //new RandomForestClassifier();
  //new DecisionTreeClassifier();
  //new GradientBoostingClassifier();
begin
  var ds := Datasets.Iris;
  
  var (X, y) := ds.ToXY;
  
  var acc :=
  Validation.StratifiedCrossValidate(
    new RandomForestClassifier(),
    X,
    y,
    5,
    Metrics.Accuracy,
    42);
  
  Println('CV accuracy:', acc:0:3);
end.