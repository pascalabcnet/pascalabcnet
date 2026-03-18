uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms','area','kitchen_area', 'floor','floors_total','metro_minutes'];
  var target := 'price';

  var X := df.ToMatrix(features);
  var y := df.ToVector(target);

  var model := new GradientBoostingRegressor;
  model.Fit(X,y);

  // квартира: 2 комнаты, 55 м²
  var flat: Matrix := [
    [2,55.0,10.0,7,16,8]
  ];

  var pred := model.Predict(flat);

  Println('Predicted price:', pred[0]:0:0, 'RUB');
end.