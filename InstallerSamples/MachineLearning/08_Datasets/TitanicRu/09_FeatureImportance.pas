uses MLABC;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data.Drop(['Id', 'Имя']);

  var ageImputer := new Imputer(['Возраст']);
  df := ageImputer.FitTransform(df);

  var portImputer := new Imputer('Саутгемптон', ['ПортПосадки']);
  df := portImputer.FitTransform(df);

  var sexEncoder := new OrdinalEncoder('Пол');
  df := sexEncoder.FitTransform(df);

  var portEncoder := new OrdinalEncoder('ПортПосадки');
  df := portEncoder.FitTransform(df);

  var features := ['Класс', 'Пол', 'Возраст', 'БратьяИСупруги', 'РодителиИДети', 'ЦенаБилета', 'ПортПосадки'];
  var X := df.ToMatrix(features);
  var y := df.Int('Выжил');

  var model := new RandomForestClassifier(nTrees := 100, maxDepth := 6, minSamplesLeaf := 3, minSamplesSplit := 6, seed := 42);
  model.Fit(X, y);

  var imp := model.FeatureImportances;

  Println('Важность признаков для RandomForestClassifier');
  for var i := 0 to features.Length - 1 do
    Println($'{features[i],-18}: {imp[i]:F3}');
end.
