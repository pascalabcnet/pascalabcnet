uses MLABC;

begin
  var df := CsvLoader.Load('towns_russia.csv', inferCategorical := true);  
  df := df.Select(['population', 'lat', 'lon', 'region_name', 'federal_district']);
  
  var imputer := new Imputer('population', 'lat', 'lon');
  df := imputer.FitTransform(df);
  
  var le1 := new LabelEncoder('region_name');
  df := le1.FitTransform(df);
  
  var le2 := new LabelEncoder('federal_district');
  df := le2.FitTransform(df);
  
  var features := ['lat', 'lon', 'region_name', 'federal_district'];
  
  var X := df.ToMatrix(features);
  var y := df.ToVector('population');
  
  var scaler := new StandardScaler;
  scaler.Fit(X);
  var Xscaled := scaler.Transform(X);
  
  var model := new LinearRegression;
  model.Fit(Xscaled, y);
  
  var preds := model.Predict(Xscaled);
  
  Println('RMSE:', Metrics.RMSE(y, preds):0:3);
end.