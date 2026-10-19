{$package Microsoft.ML.TimeSeries 5.0.0}
uses Microsoft.ML, Microsoft.ML.Data, Microsoft.ML.Transforms.TimeSeries;

type
  Observation = class
  public
    Value: single;
  end;

  ForecastResult = class
  public
    Forecast: array of single;
  end;

function SeasonalValue(i: integer): single := single(10 + 3*Sin(2*Pi*i/12));

procedure PrintMklModules;
begin
  var process := System.Diagnostics.Process.GetCurrentProcess;
  try
    foreach var m: System.Diagnostics.ProcessModule in process.Modules do
      if System.String.Equals(m.ModuleName, 'MklImports.dll', System.StringComparison.OrdinalIgnoreCase) or
          System.String.Equals(m.ModuleName, 'MklProxyNative.dll', System.StringComparison.OrdinalIgnoreCase) or
          System.String.Equals(m.ModuleName, 'libiomp5md.dll', System.StringComparison.OrdinalIgnoreCase) then
        Println('NATIVE=', m.FileName);
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
    // Ten periods for training; the next six observations are held out.
    var rows := new List<Observation>;
    for var i := 0 to 119 do
    begin
      var row := new Observation;
      row.Value := SeasonalValue(i);
      rows.Add(row);
    end;
    var data := ml.Data.LoadFromEnumerable(rows);
    var trainer := ml.Forecasting.ForecastBySsa('Forecast', 'Value', 24, 48, 120, 6);
    model := trainer.Fit(data);
    Println('TRAINED');
  end;

  var engine := model.CreateTimeSeriesEngine&<Observation, ForecastResult>(ml);
  // Save the time-series engine state, not only the initial transformer.
  if (ParamCount = 2) and (ParamStr(1) = '--save') then
    engine.CheckPoint(ml, ParamStr(2));
  var forecast := engine.Predict;
  if forecast.Forecast.Length <> 6 then
    raise new System.Exception('Unexpected forecast length');
  for var i := 0 to 5 do
  begin
    var predicted := forecast.Forecast[i];
    if System.Single.IsNaN(predicted) or System.Single.IsInfinity(predicted) or
        (Abs(predicted - SeasonalValue(120+i)) >= 0.2) then
      raise new System.Exception('Seasonal forecast is outside tolerance');
    Println('FORECAST=', predicted.ToString('F3', System.Globalization.CultureInfo.InvariantCulture));
  end;
  engine.Dispose;
  PrintMklModules;
end.
