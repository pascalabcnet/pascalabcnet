unit ValidationML;

interface

uses LinearAlgebraML, MLModelsABC;

type
  Validation = static class
  public
    /// Делит данные на обучающую и тестовую выборки.
    /// testRatio — доля объектов, попадающих в тестовую выборку (по умолчанию 0.2).
    /// Перемешивает объекты перед разбиением.
    /// Возвращает (X_train, X_test, y_train, y_test).
    static function TrainTestSplit(X: Matrix; y: Vector;
      testRatio: real := 0.2; seed: integer := 0): (Matrix, Matrix, Vector, Vector);

    /// Разбивает индексы объектов на k непересекающихся частей (fold).
    /// На каждом шаге одна часть используется как тестовая, остальные — как обучающая.
    /// Применяется для k-fold кросс-валидации.
    /// Возвращает последовательность пар (trainIdx, testIdx).
    static function KFold(n, k: integer; seed: integer := 0):
      sequence of (array of integer, array of integer);
    
    /// Разбивает данные на k частей с сохранением пропорций классов.
    /// В каждой части сохраняется примерно то же соотношение
    /// объектов разных классов, что и во всей выборке.
    /// Рекомендуется для задач классификации,
    /// особенно при неравномерном распределении классов.
    /// Возвращает последовательность пар (trainIdx, testIdx).
    static function StratifiedKFold(y: Vector; k: integer;
      seed: integer := 0): sequence of (array of integer, array of integer);
  
    /// Выполняет k-fold кросс-валидацию модели.
    /// На каждом шаге модель обучается на обучающей части и оценивается на тестовой.
    /// metric — функция качества (например, Accuracy или MSE).
    /// Возвращает среднее значение метрики по всем частям.
    static function CrossValidate(model: IModel; X: Matrix; y: Vector;
      k: integer; metric: (Vector,Vector) -> real; seed: integer := 0): real;
    
    /// Выполняет k-fold кросс-валидацию с сохранением пропорций классов.
    /// Использует StratifiedKFold для разбиения данных.
    /// Подходит для задач классификации.
    /// Возвращает среднее значение метрики по всем частям.
    static function StratifiedCrossValidate(model: IModel; X: Matrix; y: Vector;
      k: integer; metric: (Vector,Vector) -> real; seed: integer := 0): real;  
  end;

implementation

static function Validation.TrainTestSplit(X: Matrix; y: Vector;
  testRatio: real; seed: integer): (Matrix, Matrix, Vector, Vector);
begin
  if X.RowCount <> y.Length then
    raise new Exception('Dimension mismatch in TrainTestSplit');

  if (testRatio <= 0) or (testRatio >= 1) then
    raise new Exception('testRatio must be in (0,1)');

  var n := X.RowCount;
  var p := X.ColCount;

  var rnd := new System.Random(seed);

  // Перемешанные индексы
  var idx := (0..n-1).ToArray;
  idx := idx.OrderBy(i -> rnd.Next).ToArray;

  var testSize := Round(n * testRatio);
  var trainSize := n - testSize;

  var X_train := new Matrix(trainSize, p);
  var X_test  := new Matrix(testSize, p);

  var y_train := new Vector(trainSize);
  var y_test  := new Vector(testSize);

  // Заполняем train
  for var i := 0 to trainSize - 1 do
  begin
    var row := idx[i];

    for var j := 0 to p - 1 do
      X_train[i,j] := X[row,j];

    y_train[i] := y[row];
  end;

  // Заполняем test
  for var i := 0 to testSize - 1 do
  begin
    var row := idx[trainSize + i];

    for var j := 0 to p - 1 do
      X_test[i,j] := X[row,j];

    y_test[i] := y[row];
  end;

  Result := (X_train, X_test, y_train, y_test);
end;

static function Validation.KFold(n, k: integer; seed: integer):
  sequence of (array of integer, array of integer);
begin
  if (k < 2) or (k > n) then
    raise new Exception('Invalid k in KFold');

  var rnd := new System.Random(seed);
  var idx := (0..n-1).OrderBy(i -> rnd.Next).ToArray;

  var baseSize := n div k;
  var extra := n mod k;
  var start := 0;

  for var fold := 0 to k - 1 do
  begin
    var size := baseSize + (if fold < extra then 1 else 0);

    var testIdx := idx.Skip(start).Take(size).ToArray;
    var trainIdx := idx.Take(start).Concat(idx.Skip(start + size)).ToArray;

    yield (trainIdx, testIdx);
    start += size;
  end;
end;

static function Validation.StratifiedKFold(y: Vector; k: integer;
  seed: integer): sequence of (array of integer, array of integer);
begin
  var n := y.Length;
  if (k < 2) or (k > n) then
    raise new Exception('Invalid k in StratifiedKFold');

  var rnd := new System.Random(seed);

  var idx0 := (0..n-1)
    .Where(i -> y[i] = 0)
    .OrderBy(i -> rnd.Next)
    .ToArray;

  var idx1 := (0..n-1)
    .Where(i -> y[i] = 1)
    .OrderBy(i -> rnd.Next)
    .ToArray;

  if idx0.Length + idx1.Length <> n then
    raise new Exception('StratifiedKFold supports only 0/1 labels');

  var base0 := idx0.Length div k;
  var extra0 := idx0.Length mod k;

  var base1 := idx1.Length div k;
  var extra1 := idx1.Length mod k;

  for var fold := 0 to k - 1 do
  begin
    var start0 := fold * base0 + Min(fold, extra0);
    var size0 := base0 + (if fold < extra0 then 1 else 0);

    var start1 := fold * base1 + Min(fold, extra1);
    var size1 := base1 + (if fold < extra1 then 1 else 0);

    var testIdx :=
      idx0.Skip(start0).Take(size0)
          .Concat(idx1.Skip(start1).Take(size1))
          .ToArray;

    var trainIdx :=
      (0..n-1)
        .Where(i -> not testIdx.Contains(i))
        .ToArray;

    yield (trainIdx, testIdx);
  end;
end;


static function Validation.CrossValidate(model: IModel; X: Matrix; y: Vector;
  k: integer; metric: (Vector,Vector) -> real; seed: integer): real;
begin
  if X.RowCount <> y.Length then
    raise new Exception('Dimension mismatch in CrossValidate');

  var total := 0.0;
  var folds := 0;
  var p := X.ColCount;

  foreach var (trainIdx, testIdx) in KFold(X.RowCount, k, seed) do
  begin
    var Xtr := new Matrix(trainIdx.Length, p);
    var ytr := new Vector(trainIdx.Length);

    for var i := 0 to trainIdx.Length - 1 do
    begin
      var r := trainIdx[i];
      for var j := 0 to p - 1 do
        Xtr[i,j] := X[r,j];
      ytr[i] := y[r];
    end;

    var Xte := new Matrix(testIdx.Length, p);
    var yte := new Vector(testIdx.Length);

    for var i := 0 to testIdx.Length - 1 do
    begin
      var r := testIdx[i];
      for var j := 0 to p - 1 do
        Xte[i,j] := X[r,j];
      yte[i] := y[r];
    end;

    var m := model.Fit(Xtr, ytr);
    var pred := m.Predict(Xte);

    total += metric(yte, pred);
    folds += 1;
  end;

  Result := total / folds;
end;

static function Validation.StratifiedCrossValidate(
  model: IModel; X: Matrix; y: Vector;
  k: integer; metric: (Vector,Vector) -> real; seed: integer
): real;
begin
  if X.RowCount <> y.Length then
    raise new Exception('Dimension mismatch in StratifiedCrossValidate');

  var total := 0.0;
  var folds := 0;
  var p := X.ColCount;

  foreach var (trainIdx, testIdx) in StratifiedKFold(y, k, seed) do
  begin
    var Xtr := new Matrix(trainIdx.Length, p);
    var ytr := new Vector(trainIdx.Length);

    for var i := 0 to trainIdx.Length - 1 do
    begin
      var r := trainIdx[i];
      for var j := 0 to p - 1 do
        Xtr[i,j] := X[r,j];
      ytr[i] := y[r];
    end;

    var Xte := new Matrix(testIdx.Length, p);
    var yte := new Vector(testIdx.Length);

    for var i := 0 to testIdx.Length - 1 do
    begin
      var r := testIdx[i];
      for var j := 0 to p - 1 do
        Xte[i,j] := X[r,j];
      yte[i] := y[r];
    end;

    var m := model.Fit(Xtr, ytr);
    var pred := m.Predict(Xte);

    total += metric(yte, pred);
    folds += 1;
  end;

  Result := total / folds;
end;



end.
