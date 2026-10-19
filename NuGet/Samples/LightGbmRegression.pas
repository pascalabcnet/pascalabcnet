{$package Microsoft.ML.LightGbm 5.0.0}
uses Microsoft.ML, Microsoft.ML.Data, Microsoft.ML.Trainers.LightGbm;

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

function NativeLightGbmPath: string;
begin
  Result := '';
  var process := System.Diagnostics.Process.GetCurrentProcess;
  try
    foreach var m: System.Diagnostics.ProcessModule in process.Modules do
      if System.String.Equals(m.ModuleName, 'lib_lightgbm.dll', System.StringComparison.OrdinalIgnoreCase) then
        Result := m.FileName;
  finally
    process.Dispose;
  end;
end;

begin
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
    // Trees interpolate inside the training range; no extrapolation test here.
    var rows := new List<Observation>;
    for var i := -200 to 200 do
    begin
      var x := single(i / 100);
      var row := new Observation;
      row.Features := Arr(x);
      row.Target := x*x;
      rows.Add(row);
    end;
    var data := ml.Data.LoadFromEnumerable(rows);
    var options := new LightGbmRegressionTrainer.Options;
    options.LabelColumnName := 'Target';
    options.NumberOfThreads := 1;
    options.Seed := 1;
    options.NumberOfIterations := 100;
    options.NumberOfLeaves := 16;
    options.MinimumExampleCountPerLeaf := 3;
    var trainer := ml.Regression.Trainers.LightGbm(options);
    model := trainer.Fit(data);
    if NativeLightGbmPath = '' then
      raise new System.Exception('Native LightGBM was not loaded during training');
    if (ParamCount = 2) and (ParamStr(1) = '--save') then
      ml.Model.Save(model, data.Schema, ParamStr(2));
    Println('TRAINED');
  end;

  var engine := ml.Model.CreatePredictionEngine&<Observation, Prediction>(model);
  foreach var x in Arr(single(-1.255), single(0.505), single(1.755)) do
  begin
    var input := new Observation;
    input.Features := Arr(x);
    var predicted := engine.Predict(input).Score;
    if System.Single.IsNaN(predicted) or System.Single.IsInfinity(predicted) or
        (Abs(predicted - x*x) >= 0.15) then
      raise new System.Exception('LightGBM prediction is outside tolerance');
    Println('SCORE=', predicted.ToString('F3', System.Globalization.CultureInfo.InvariantCulture));
  end;
  engine.Dispose;
  Println('NATIVE=', NativeLightGbmPath);
end.
