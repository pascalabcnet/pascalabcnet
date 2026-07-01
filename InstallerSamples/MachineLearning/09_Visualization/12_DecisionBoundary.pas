uses MLABC,PlotML;

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
  var classNames := target.ClassNames;

  var model := new LogisticRegression;
  model.Fit(X, y);

  var example := [20.0, 38.0];
  var pred := model.PredictOne(example);
  var prob := model.PredictProbaOne(example);

  Println('Предсказанная порода:', classNames[pred]);
  Println('Предсказанные вероятности:');
  Println(classnames[0]:10,'-',prob[0]:0:2);
  Println(classnames[1]:10,'-',prob[1]:0:2);
  
  Plot.XLabel := df.ColumnNames[0];
  Plot.YLabel := df.ColumnNames[1];
  Plot.Title := 'Бульдоги и спаниели';

  Plot.Points(X.Col(0),X.Col(1),y);
  Plot.Points([example[0]],[example[1]],Colors.Black,size := 8);

  var (A,B,C) := model.DecisionBoundary.Coefficients;
  Plot.Line(A,B,C);  
end.