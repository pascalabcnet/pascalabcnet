uses MLABC;

procedure TestModel(model: ISupervisedModel; X: Matrix; y: Vector);
begin
  var acc :=
    Validation.StratifiedCrossValidate(
      model,
      X,
      y,
      5,
      Metrics.Accuracy,
      seed := 42);

  Println(model.Name:26, ':', acc:0:3);
end;

begin
  var ds := Datasets.Iris;
  var df := ds.Data;

  var X := df.ToMatrix(ds.Features);
  var y := df.EncodeLabels(ds.Target);

  var models: array of ISupervisedModel := (
    new LogisticRegression,
    new DecisionTreeClassifier(seed := -1),
    new RandomForestClassifier(seed := -1),
    new GradientBoostingClassifier(seed := -1)
  );

  foreach var model in models do
    TestModel(model, X, y);
end.