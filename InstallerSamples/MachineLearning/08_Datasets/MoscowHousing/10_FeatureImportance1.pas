uses MLABC;

begin
  var ds := Datasets.MoscowHousing;
  var df := ds.Data;

  var features := ['rooms','area', 'kitchen_area', 'floor', 'floors_total', 'metro_minutes', 'renovation'];
  var target := 'price';

  var model := new RandomForestRegressor(seed := 42);

  var pipe :=
    DataPipeline.Build(
      target,
      features,
      new LabelEncoder('renovation'),
      model
    );

  pipe.Fit(df);

  var imp := model.FeatureImportances;

  Println('Feature importance:');
  for var i := 0 to features.Length-1 do
    Println(features[i]:15, ':', imp[i]:0:3);
end.