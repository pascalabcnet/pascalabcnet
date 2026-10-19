{$package Microsoft.ML 5.0.0}
uses Microsoft.ML, Microsoft.ML.Data, Microsoft.ML.Trainers;

type
  Observation = class
  public
    [VectorType(1)]
    Features: array of single;
    Target: single;
  end;

  Prediction = class
  public
    Score: single;
  end;

begin
  // ML.NET expects single, not the Pascal real (Double).
  var ml := new MLContext(1);
  var model: ITransformer;
  if (ParamCount = 2) and (ParamStr(1) = '--load') then
  begin
    var schema: DataViewSchema;
    model := ml.Model.Load(ParamStr(2), schema);
    Println('LOADED');
  end
  else
  begin
    // Train on y = 2*x + 1. Predictions below use other x values.
    var rows := new List<Observation>;
    for var i := -20 to 20 do
    begin
      var row := new Observation;
      var x := single(i / 10);
      row.Features := Arr(x);
      row.Target := 2*x + 1;
      rows.Add(row);
    end;
    var data := ml.Data.LoadFromEnumerable(rows);
    var options := new SdcaRegressionTrainer.Options;
    options.LabelColumnName := 'Target';
    options.NumberOfThreads := 1;
    options.MaximumNumberOfIterations := 1000;
    options.L2Regularization := single(0.01);
    options.L1Regularization := single(0);
    // Regularize the intercept too: stable SDCA on this tiny training set.
    options.BiasLearningRate := 0;
    var trainer := ml.Regression.Trainers.Sdca(options);
    model := trainer.Fit(data);
    if (ParamCount = 2) and (ParamStr(1) = '--save') then
      ml.Model.Save(model, data.Schema, ParamStr(2));
    Println('TRAINED');
  end;

  var engine := ml.Model.CreatePredictionEngine&<Observation, Prediction>(model);
  foreach var x in Arr(single(-1.25), single(0.5), single(1.75)) do
  begin
    var input := new Observation;
    input.Features := Arr(x);
    var predicted := engine.Predict(input).Score;
    if System.Single.IsNaN(predicted) or System.Single.IsInfinity(predicted) or
        (Abs(predicted - (2*x + 1)) >= 0.1) then
      raise new System.Exception('Regression prediction is outside tolerance');
    Println('SCORE=', predicted.ToString('F3', System.Globalization.CultureInfo.InvariantCulture));
  end;
  engine.Dispose;
end.
