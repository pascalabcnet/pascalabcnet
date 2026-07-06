uses MLABC, PlotML;

begin
  var df := DataFrame.FromCsvText('''
  Вес,ВысотаВХолке,Порода
  20,33,бульдог
  22,34,бульдог
  24,35,бульдог
  25,36,бульдог
  19,35,бульдог
  21,35,бульдог
  26,34,бульдог
  20,36,бульдог
  14,39,спаниель
  15,40,спаниель
  16,41,спаниель
  18,42,спаниель
  17,43,спаниель
  15,38,спаниель
  19,40,спаниель
  16,40,спаниель
  ''');

  var X := df.ToMatrix(['Вес', 'ВысотаВХолке']);
  var target := df.EncodeTarget('Порода');
  var y := target.Labels;

  var model := new KNNClassifier(5);
  model.Fit(X, y);

  var example := Vector([20.0, 38.0]);
  var pred := model.PredictOne(example);

  Println('Предсказанная порода:', target.ClassNames[pred]);
  Println('Ближайшие соседи:');

  var neigh := model.GetNearestNeighbors(example);
  foreach var n in neigh do
    Println(target.ClassName(n.Index):10, n.Distance:0:2);
  
  Plot.XLabel := df.ColumnNames[0];
  Plot.YLabel := df.ColumnNames[1];
  Plot.Title := 'Бульдоги и спаниели';

  Plot.Points(X.Col(0), X.Col(1), y);

  var (x0,y0) := (example[0], example[1]);
  
  foreach var n in neigh do
  begin
    var (x1,y1) := (X[n.Index,0],X[n.Index,1]);
    var color := Plot.PaletteColor(y[n.Index]);
    Plot.Point(x1,y1,color);
    Plot.LineGraph([x0,x1],[y0,y1],color);
  end;  
  Plot.Point(x0,y0);
end.
