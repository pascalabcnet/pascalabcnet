// StandardScaler приводит признаки к сопоставимому масштабу:
// после преобразования у каждого столбца среднее близко к 0,
// а стандартное отклонение - к 1.
uses MLABC;

begin
  var X := new Matrix(5, 2);

  // Первый признак имеет маленький масштаб и неравномерный шаг
  X[0,0] := 1;  X[1,0] := 2;  X[2,0] := 3;  X[3,0] := 10;  X[4,0] := 20;

  // Второй признак имеет гораздо больший масштаб
  X[0,1] := 100;  X[1,1] := 120;  X[2,1] := 150;  X[3,1] := 300;  X[4,1] := 500;

  Println('До масштабирования:');
  Println($'Средние: {X.ColumnMeans[0]:F3}, {X.ColumnMeans[1]:F3}');
  Println($'Стандартные отклонения: {X.ColumnStd(0):F3}, {X.ColumnStd(1):F3}');
  Println;

  var scaler := new StandardScaler;
  scaler.Fit(X);

  var Xscaled := scaler.Transform(X);

  Println('После StandardScaler:');
  Println($'Средние: {Xscaled.ColumnMeans[0]:F3}, {Xscaled.ColumnMeans[1]:F3}');
  Println($'Стандартные отклонения: {Xscaled.ColumnStd(0):F3}, {Xscaled.ColumnStd(1):F3}');
  Println;
  Println('Преобразованные значения:');
  Xscaled.Print;
end.
