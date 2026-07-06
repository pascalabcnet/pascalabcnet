uses MLABC;

begin
  var df := DataFrame.FromCsvText('''
Возраст,Зарплата,Одобрить кредит
17,25,Отказать
64,80,Одобрить
18,22,Отказать
20,36,Одобрить
38,37,Отказать
49,59,Одобрить
55,74,Одобрить
25,70,Отказать
29,33,Отказать
31,102,Одобрить
33,88,Отказать
''');

  var featureNames := ['Возраст', 'Зарплата'];
  var X := df.ToMatrix(featureNames);
  var target := df.EncodeTarget('Одобрить кредит');
  
  var model := new DecisionTreeClassifier;
  model.Fit(X, target.Labels);
  
  var view := model.Tree(featureNames, target.ClassNames);
  
  // Вывод дерева решений
  Println(view);

  // Новый клиент
  var example := [20.0, 100.0];

  var pred := model.PredictOne(example);

  Println('Возраст:', example[0]);
  Println('Зарплата:', example[1]);
  Println('Одобрить кредит:', target.ClassNames[pred]);
end.