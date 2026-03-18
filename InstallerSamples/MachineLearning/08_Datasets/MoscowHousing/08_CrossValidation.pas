uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms','area','kitchen_area','floor','floors_total','metro_minutes'];
  var target := 'price';

  var X := df.ToMatrix(features);
  var y := df.ToVector(target);

  var models: array of IRegressor := (
    new LinearRegression,
    new RandomForestRegressor,
    new GradientBoostingRegressor
  );
  
  foreach var model in models do
  begin
    var score :=
      Validation.CrossValidate(
        model,
        X,
        y,
        5,
        Metrics.R2,
        42
      );
  
    Println(model.Name:25, ':', score:0:3);
  end;
end.