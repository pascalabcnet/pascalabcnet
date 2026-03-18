uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms','area','kitchen_area', 'floor','floors_total','metro_minutes'];
  var target := 'price';

  var X := df.ToMatrix(features);
  var y := df.ToVector(target);

  var model := new RandomForestRegressor(seed := 42);
  model.Fit(X,y);

  var imp := model.FeatureImportances;

  Println('Feature importance:');
  for var i := 0 to features.Length-1 do
    Println(features[i]:15, ':', imp[i]:0:3);
end.