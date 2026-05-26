// MinMaxScaler переводит значения каждого признака в диапазон [0, 1].
//
// Это полезно, когда важно сохранить относительный порядок значений,
// но привести признаки к единому диапазону.
uses MLABC;

begin
  var X := new Matrix(5, 2);

  // Первый признак имеет маленький масштаб и неравномерный шаг
  X[0,0] := 1;  X[1,0] := 2;  X[2,0] := 3;  X[3,0] := 10;  X[4,0] := 20;

  // Второй признак имеет гораздо больший масштаб
  X[0,1] := 100;  X[1,1] := 120;  X[2,1] := 150;  X[3,1] := 300;  X[4,1] := 500;

  Println('До масштабирования:');
  Println($'Минимумы: {X.ColumnMin(0):F3}, {X.ColumnMin(1):F3}');
  Println($'Максимумы: {X.ColumnMax(0):F3}, {X.ColumnMax(1):F3}');
  Println;

  var scaler := new MinMaxScaler;
  scaler.Fit(X);

  var Xscaled := scaler.Transform(X);

  Println('После MinMaxScaler:');
  Println($'Минимумы: {Xscaled.ColumnMin(0):F3}, {Xscaled.ColumnMin(1):F3}');
  Println($'Максимумы: {Xscaled.ColumnMax(0):F3}, {Xscaled.ColumnMax(1):F3}');
  Println;
  Println('Преобразованные значения:');
  Xscaled.Print;
end.
