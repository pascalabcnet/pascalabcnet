unit ValidationML;

interface

uses LinearAlgebraML, MLCoreABC;

type
  Validation = static class
  public
    /// Делит данные на обучающую и тестовую выборки.
    /// testRatio — доля объектов, попадающих в тестовую выборку (по умолчанию 0.2).
    /// Перед разбиением объекты перемешиваются.
    /// Возвращает кортеж (X_train, X_test, y_train, y_test).
    static function TrainTestSplit(X: Matrix; y: Vector;
      testRatio: real := 0.2; seed: integer := -1): (Matrix, Matrix, Vector, Vector);

    /// Разбивает индексы объектов на k непересекающихся частей (fold).
    /// На каждом шаге одна часть используется как тестовая, остальные — как обучающая выборка.
    /// Используется для k-fold кросс-валидации.
    /// Возвращает последовательность пар (trainIdx, testIdx).
    static function KFold(n, k: integer; seed: integer := -1):
      sequence of (array of integer, array of integer);
    
/// Разбивает данные на k частей (k-fold) с сохранением пропорций классов
/// (стратифицированная k-fold кросс-валидация).
/// В каждой части доля объектов каждого класса
/// максимально близка к их доле во всей выборке
/// (разница не превышает одного объекта на класс).
/// Рекомендуется для задач классификации,
/// особенно при несбалансированных классах.
/// Возвращает последовательность пар (trainIdx, testIdx)
    static function StratifiedKFold(y: Vector; k: integer;
      seed: integer := -1): sequence of (array of integer, array of integer);
  
    /// Выполняет k-fold кросс-валидацию модели с учителем.
    /// На каждом шаге модель обучается на обучающей части и оценивается на соответствующей тестовой части.
    /// metric — функция качества, принимающая (y_true, y_pred)
    ///   и возвращающая значение метрики (например, Accuracy или MSE).
    /// Возвращает среднее значение метрики по всем частям.
    static function CrossValidate(model: ISupervisedModel; X: Matrix; y: Vector;
      k: integer; metric: (Vector,Vector) -> real; seed: integer := -1): real;
    
    /// Выполняет стратифицированную k-fold кросс-валидацию модели с учителем.
    /// Разбиение данных выполняется методом StratifiedKFold
    ///     с сохранением пропорций классов в каждой части.
    /// Рекомендуется для задач классификации, особенно при несбалансированных классах.
    /// Возвращает среднее значение метрики по k разбиениям.
    static function StratifiedCrossValidate(model: ISupervisedModel; X: Matrix; y: Vector;
      k: integer; metric: (Vector,Vector) -> real; seed: integer := -1): real;  
  end;
  
/// Класс для подбора гиперпараметров методом перебора по сетке (Grid Search).
/// Для каждого значения параметра выполняется k-кратная кросс-валидация.
/// Выбирается параметр, дающий наилучшее среднее значение метрики.
/// Используется для настройки регуляризации и других гиперпараметров моделей.
  GridSearch = static class
  public
    /// Выполняет подбор гиперпараметра по заданной сетке значений.
    /// modelFactory — функция создания модели по значению параметра.
    /// paramValues — набор тестируемых значений гиперпараметра.
    /// X, y — обучающие данные.
    /// k — число фолдов в кросс-валидации.
    /// metric — функция оценки качества (yTrue, yPred) → real.
    /// Возвращает кортеж (лучший параметр, лучшее среднее значение метрики,
    ///   модель, обученная на всём датасете с лучшим параметром).
  class function Search<T>(
    modelFactory: real -> T;
    paramValues: array of real;
    X: Matrix; y: Vector;
    k: integer;
    metric: (Vector, Vector) -> real;
    maximize: boolean := True
  ): (real, real, T); where T: class,ISupervisedModel;
  end;

implementation

uses MLExceptions;

const
  ER_DIM_MISMATCH_TRAIN_TEST =
    'Несоответствие размерностей в TrainTestSplit: X.RowCount={0}, y.Length={1}!!' +
    'Dimension mismatch in TrainTestSplit: X.RowCount={0}, y.Length={1}';
  ER_TEST_RATIO_INVALID =
    'Параметр testRatio должен быть в интервале (0,1), получено {0}!!' +
    'Parameter testRatio must be in (0,1), got {0}';  
  ER_K_INVALID =
    'Некорректное значение k в KFold: k={0}, n={1}!!' +
    'Invalid k in KFold: k={0}, n={1}';  
  ER_K_INVALID_STRATIFIED =
    'Некорректное значение k в StratifiedKFold: k={0}, n={1}!!' +
    'Invalid k in StratifiedKFold: k={0}, n={1}';  
  ER_STRATIFIED_LABELS_INVALID =
    'StratifiedKFold поддерживает только целочисленные метки классов!!' +
    'StratifiedKFold supports only integer class labels';
  ER_INVALID_VALUE =
    'Некорректное значение параметра {0}!!Invalid value for parameter {0}';
  ER_DATASET_TOO_SMALL =
    'Для {0} требуется как минимум 2 объекта!!' +
    'At least 2 samples are required for {0}';    
    
//-----------------------------
//         Validation
//-----------------------------

static function Validation.TrainTestSplit(X: Matrix; y: Vector;
  testRatio: real; seed: integer): (Matrix, Matrix, Vector, Vector);
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH_TRAIN_TEST, X.RowCount, y.Length);

  if (testRatio <= 0.0) or (testRatio >= 1.0) then
    ArgumentError(ER_TEST_RATIO_INVALID, testRatio);

  var n := X.RowCount;
  var p := X.ColCount;

  if n < 2 then
    ArgumentError(ER_DATASET_TOO_SMALL, 'TrainTestSplit');

  var actualSeed := if seed >= 0 then seed else System.Environment.TickCount and integer.MaxValue;
  var rnd := new System.Random(actualSeed);

  var idx := Arr(0..n-1);

  // --- 2. Перемешивание через стандартный Shuffle
  idx.Shuffle(rnd);

  var rawSize := Round(n * testRatio);
  var testSize := rawSize.Clamp(1, n - 1);
  var trainSize := n - testSize;

  var X_train := new Matrix(trainSize, p);
  var X_test  := new Matrix(testSize, p);

  var y_train := new Vector(trainSize);
  var y_test  := new Vector(testSize);

  for var i := 0 to trainSize - 1 do
  begin
    var row := idx[i];
    for var j := 0 to p - 1 do
      X_train[i,j] := X[row,j];
    y_train[i] := y[row];
  end;

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
  if n <= 0 then
    ArgumentError(ER_EMPTY_DATA, 'KFold');

  if (k < 2) or (k > n) then
    ArgumentError(ER_K_INVALID, k, n);

  var actualSeed := if seed >= 0 then seed
                    else System.Environment.TickCount and integer.MaxValue;

  var rnd := new System.Random(actualSeed);

  // --- 1. Индексы 0..n-1
  var idx := Arr(0..n-1);

  // --- 2. Перемешивание через стандартный Shuffle
  idx.Shuffle(rnd);

  var baseSize := n div k;
  var extra := n mod k;
  var start := 0;

  // --- 3. Формируем фолды
  for var fold := 0 to k - 1 do
  begin
    var size := baseSize + Ord(fold < extra);

    var testIdx := new integer[size];
    System.Array.Copy(idx, start, testIdx, 0, size);

    var trainSize := n - size;
    var trainIdx := new integer[trainSize];

    if start > 0 then
      System.Array.Copy(idx, 0, trainIdx, 0, start);

    var tailCount := n - (start + size);
    if tailCount > 0 then
      System.Array.Copy(idx, start + size, trainIdx, start, tailCount);

    yield (trainIdx, testIdx);

    start += size;
  end;
end;

static function Validation.StratifiedKFold(y: Vector; k: integer; seed: integer):
  sequence of (array of integer, array of integer);
begin
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  var n := y.Length;

  if n <= 0 then
    ArgumentError(ER_EMPTY_DATA, 'StratifiedKFold');

  if (k < 2) or (k > n) then
    ArgumentError(ER_K_INVALID_STRATIFIED, k, n);

  var actualSeed := if seed >= 0 then seed
                    else System.Environment.TickCount and integer.MaxValue;

  var rnd := new System.Random(actualSeed);

  // --- 1. Индексы по классам
  var classMap := new Dictionary<integer, List<integer>>();

  for var i := 0 to n - 1 do
  begin
    var v := y[i];
    var cls := integer(v);

    if Abs(v - cls) > 1e-12 then
      ArgumentError(ER_STRATIFIED_LABELS_INVALID);

    var lst: List<integer>;
    if classMap.TryGetValue(cls, lst) then
      lst.Add(i)
    else
    begin
      lst := new List<integer>;
      lst.Add(i);
      classMap.Add(cls, lst);
    end;
  end;

  // --- 2. Контейнеры фолдов
  var folds := new List<integer>[k];
  for var f := 0 to k - 1 do
    folds[f] := new List<integer>;

  // --- 3. Для каждого класса: shuffle + равномерное распределение
  foreach var pair in classMap do
  begin
    var indices := pair.Value;
    indices.Shuffle(rnd);

    var m := indices.Count;
    var baseSize := m div k;
    var extra := m mod k;
    var start := 0;

    for var fold := 0 to k - 1 do
    begin
      var size := baseSize + Ord(fold < extra);
      for var t := 0 to size - 1 do
        folds[fold].Add(indices[start + t]);
      start += size;
    end;
  end;

  // --- 4. Формирование train/test
  for var fold := 0 to k - 1 do
  begin
    var testIdx := folds[fold].ToArray;

    var mask := new boolean[n];
    foreach var id in testIdx do
      mask[id] := true;

    var trainIdx := new integer[n - testIdx.Length];
    var p := 0;

    for var i := 0 to n - 1 do
      if not mask[i] then
      begin
        trainIdx[p] := i;
        p += 1;
      end;

    yield (trainIdx, testIdx);
  end;
end;

static function Validation.CrossValidate(
  model: ISupervisedModel; 
  X: Matrix; 
  y: Vector;
  k: integer; 
  metric: (Vector,Vector) -> real; 
  seed: integer): real;
begin
  if model = nil then
    ArgumentNullError(ER_ARG_NULL, 'model');

  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  if metric = nil then
    ArgumentNullError(ER_ARG_NULL, 'metric');

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if (k < 2) or (k > X.RowCount) then
    ArgumentError(ER_K_INVALID, k, X.RowCount);

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

    var m := model.Clone() as ISupervisedModel;
    m := m.Fit(Xtr, ytr);

    var pred := m.Predict(Xte);

    total += metric(yte, pred);
    folds += 1;
  end;

  if folds = 0 then
    ArgumentError(ER_EMPTY_DATA, 'CrossValidate');

  Result := total / folds;
end;

static function Validation.StratifiedCrossValidate(
  model: ISupervisedModel; 
  X: Matrix; 
  y: Vector;
  k: integer; 
  metric: (Vector,Vector) -> real; 
  seed: integer): real;
begin
  if model = nil then
    ArgumentNullError(ER_ARG_NULL, 'model');

  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  if metric = nil then
    ArgumentNullError(ER_ARG_NULL, 'metric');

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if (k < 2) or (k > X.RowCount) then
    ArgumentError(ER_K_INVALID_STRATIFIED, k, X.RowCount);

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

    var m := model.Clone() as ISupervisedModel;
    m := m.Fit(Xtr, ytr);

    var pred := m.Predict(Xte);

    total += metric(yte, pred);
    folds += 1;
  end;

  if folds = 0 then
    ArgumentError(ER_EMPTY_DATA, 'StratifiedCrossValidate');

  Result := total / folds;
end;

//-----------------------------
//         GridSearch
//-----------------------------

class function GridSearch.Search<T>(
  modelFactory: real -> T;
  paramValues: array of real;
  X: Matrix; 
  y: Vector;
  k: integer;
  metric: (Vector, Vector) -> real;
  maximize: boolean
): (real, real, T); where T: class,ISupervisedModel;
begin
  if modelFactory = nil then
    ArgumentNullError(ER_ARG_NULL, 'modelFactory');

  if paramValues = nil then
    ArgumentNullError(ER_ARG_NULL, 'paramValues');

  if paramValues.Length = 0 then
    ArgumentError(ER_PARAM_VALUES_EMPTY);

  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  if metric = nil then
    ArgumentNullError(ER_ARG_NULL, 'metric');

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  var bestParam := paramValues[0];
  var bestScore := 
    if maximize then -1e308 else 1e308;

  foreach var param in paramValues do
  begin
    var model := modelFactory(param);
    if model = nil then
      ArgumentError(ER_MODEL_NULL);
    var avgScore := Validation.CrossValidate(model, X, y, k, metric);

    if double.IsNaN(avgScore) or double.IsInfinity(avgScore) then
      ArgumentError(ER_INVALID_VALUE, 'avgScore');

    var better :=
      (maximize and (avgScore > bestScore)) or
      (not maximize and (avgScore < bestScore));

    if better then
    begin
      bestScore := avgScore;
      bestParam := param;
    end;
  end;

  var bestModel := modelFactory(bestParam);
  if bestModel = nil then
    ArgumentError(ER_MODEL_NULL);
  
  bestModel := bestModel.Fit(X, y) as T;

  Result := (bestParam, bestScore, bestModel);
end;



end.
