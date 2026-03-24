uses MLABC, PlotML;

begin
  var ds := Datasets.RussianCities;
  var df := ds.Data;

  // --- признаки
  df := df.WithColumnFloat('density', row -> row.Float('population') / row.Float('area'));
  df := df.WithColumnInt('age', row -> 2026 - row.Int('foundation_year'));
  df := df.WithColumnFloat('log_population', row -> Ln(row.Float('population')));

  var features := ['log_population', 'density', 'lat', 'lon'{, 'age'}];

  // --- кластеризация
  var X := df.ToMatrix(features);

  var km := new KMeans(5, seed := 42);
  km.Fit(X);

  var labels: array of integer := km.PredictLabels(X);
  df.AddIntColumn('cluster', labels, nil);

  // --- координаты
  var lon := df.ToVector('lon').ToArray;
  var lat := df.ToVector('lat').ToArray;

  Plot.Points(
    lon,
    lat,
    labels,
    size := 5,
    marker := MarkerType.Circle
  );

  Plot.XLabel('Долгота');
  Plot.YLabel('Широта');
  Plot.Title := 'Кластеры городов России';
end.