unit ValidationML;

interface

uses LinearAlgebraML, MLCoreABC;

type
/// Результат построения валидационной кривой.
/// Хранит значения параметра, оценки по фолдам
/// и их агрегированные характеристики
  ValidationCurveResult = class
  public
    /// Проверенные значения гиперпараметра.
    ParameterValues: Vector;
    /// Значения метрики на обучающих подвыборках.
    /// Строки соответствуют параметрам, столбцы — фолдам.
    TrainScores: Matrix;
    /// Значения метрики на валидационных подвыборках.
    /// Строки соответствуют параметрам, столбцы — фолдам.
    ValidationScores: Matrix;
    /// Средние значения метрики на обучающих подвыборках.
    TrainMean: Vector;
    /// Средние значения метрики на валидационных подвыборках.
    ValidationMean: Vector;
    /// Стандартные отклонения метрики на обучающих подвыборках.
    TrainStd: Vector;
    /// Стандартные отклонения метрики на валидационных подвыборках.
    ValidationStd: Vector;
    
    /// Возвращает число проверенных значений параметра
    function Count: integer;
    /// Возвращает индекс лучшего значения параметра
    /// по средней валидационной метрике
    function BestIndex(maximize: boolean := True): integer;
    /// Возвращает лучшее значение параметра
    function BestParam(maximize: boolean := True): real;
    /// Возвращает лучшее среднее значение валидационной метрики
    function BestScore(maximize: boolean := True): real;
  end;

/// Результат построения кривой обучения.
/// Хранит размеры обучающей выборки, оценки по фолдам
/// и их агрегированные характеристики
  LearningCurveResult = class
  public
    /// Проверенные размеры обучающей выборки
    /// или их эквиваленты в числовом виде.
    TrainSizes: Vector;
    /// Значения метрики на обучающих подвыборках.
    /// Строки соответствуют размерам, столбцы — фолдам.
    TrainScores: Matrix;
    /// Значения метрики на валидационных подвыборках.
    /// Строки соответствуют размерам, столбцы — фолдам.
    ValidationScores: Matrix;
    /// Средние значения метрики на обучающих подвыборках.
    TrainMean: Vector;
    /// Средние значения метрики на валидационных подвыборках.
    ValidationMean: Vector;
    /// Стандартные отклонения метрики на обучающих подвыборках.
    TrainStd: Vector;
    /// Стандартные отклонения метрики на валидационных подвыборках.
    ValidationStd: Vector;
    
    /// Возвращает число проверенных размеров обучающей выборки
    function Count: integer;
  end;

/// Методы для разбиения данных и оценки моделей.
///
/// Содержит утилиты для:
/// • разделения выборки на обучающую и тестовую (TrainTestSplit)
/// • k-fold кросс-валидации (KFold)
/// • стратифицированной кросс-валидации (StratifiedKFold)
/// • оценки моделей через кросс-валидацию (CrossValidate, StratifiedCrossValidate)
///
/// Методы возвращают индексы или подвыборки без изменения исходных данных.
///
/// • KFold — простое разбиение без учёта распределения классов
/// • StratifiedKFold — сохраняет пропорции классов в каждом fold (только для классификации)
///
/// Для стратифицированных методов требуется:
/// • целочисленные метки классов
/// • число объектов каждого класса ≥ числа фолдов
///
/// Все методы используют генератор случайных чисел (seed) для воспроизводимости
  Validation = static class
  private  
    static function CrossValidateCore(model: IRegressor; 
      X: Matrix; y: Vector;
      folds: sequence of (array of integer, array of integer);
      metric: (Vector, Vector) -> real): real;
    static function CrossValidateCore(model: IClassifier;
      X: Matrix; y: array of integer;
      folds: sequence of (array of integer, array of integer);
      metric: (array of integer, array of integer) -> real): real;
  public
    /// Делит данные на обучающую и тестовую выборки.
    /// testRatio — доля объектов, попадающих в тестовую выборку (по умолчанию 0.2).
    /// Перед разбиением объекты перемешиваются.
    /// Возвращает кортеж (X_train, X_test, y_train, y_test).
    static function TrainTestSplit(X: Matrix; y: Vector;
      testRatio: real := 0.2; seed: integer := -1): (Matrix, Matrix, Vector, Vector);
    static function TrainTestSplit(X: Matrix; y: array of integer;
      testRatio: real := 0.2; seed: integer := -1): (Matrix, Matrix, array of integer, array of integer);

    /// Разбивает индексы объектов на k непересекающихся частей (fold).
    /// На каждом шаге одна часть используется как тестовая, остальные — как обучающая выборка.
    /// Используется для k-fold кросс-валидации.
    /// Возвращает последовательность пар (trainIdx, testIdx).
    static function KFold(n, k: integer; seed: integer := -1):
      sequence of (array of integer, array of integer);
    
/// Разбивает данные на k частей (k-fold) с сохранением пропорций классов.
/// В каждой части доля объектов каждого класса
/// максимально близка к их доле во всей выборке
/// (разница не превышает одного объекта на класс).
/// Это стратифицированная k-fold кросс-валидация.
/// Рекомендуется только для задач классификации,
/// особенно при несбалансированных классах.
/// Возвращает последовательность пар (trainIdx, testIdx)
    static function StratifiedKFold(y: Vector; k: integer;
      seed: integer := -1): sequence of (array of integer, array of integer);
    static function StratifiedKFold(y: array of integer; k: integer;
      seed: integer := -1): sequence of (array of integer, array of integer);
  
    /// Выполняет k-fold кросс-валидацию модели с учителем.
    /// На каждом шаге модель обучается на обучающей части и оценивается на соответствующей тестовой части.
    /// metric — функция качества, принимающая (y_true, y_pred)
    ///   и возвращающая значение метрики (например, Accuracy или MSE).
    /// Возвращает среднее значение метрики по всем частям.
    /// 
    /// Перегрузка для регрессионных моделей.
    /// DataPipeline сюда передавать нельзя, так как он работает с DataFrame.
    static function CrossValidate(model: IRegressor; X: Matrix; y: Vector;
      k: integer; metric: (Vector,Vector) -> real; seed: integer := -1): real;
    /// Перегрузка для классификационных моделей.
    static function CrossValidate(model: IClassifier; X: Matrix; y: array of integer;
      k: integer; metric: (array of integer, array of integer) -> real; seed: integer := -1): real;
    
    /// Выполняет стратифицированную k-fold кросс-валидацию модели с учителем.
    /// Разбиение данных выполняется методом StratifiedKFold
    ///     с сохранением пропорций классов в каждой части.
    /// Рекомендуется для задач классификации, особенно при несбалансированных классах.
    /// Возвращает среднее значение метрики по k разбиениям.
    /// Перегрузка для классификационных моделей.
    static function StratifiedCrossValidate(model: IClassifier; X: Matrix; y: array of integer;
      k: integer; metric: (array of integer, array of integer) -> real; seed: integer := -1): real;
    
    /// Строит валидационную кривую для регрессионной модели
    /// по целочисленной сетке параметра.
    /// paramValues — набор проверяемых значений гиперпараметра.
    /// modelFactory — функция создания модели по значению параметра.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function ValidationCurve(
      paramValues: array of integer;
      modelFactory: integer -> IRegressor;
      X: Matrix; y: Vector;
      metric: (Vector, Vector) -> real;
      cv: sequence of (array of integer, array of integer)
    ): ValidationCurveResult;
    /// Строит валидационную кривую для регрессионной модели
    /// по вещественной сетке параметра.
    /// paramValues — набор проверяемых значений гиперпараметра.
    /// modelFactory — функция создания модели по значению параметра.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function ValidationCurve(
      paramValues: array of real;
      modelFactory: real -> IRegressor;
      X: Matrix; y: Vector;
      metric: (Vector, Vector) -> real;
      cv: sequence of (array of integer, array of integer)
    ): ValidationCurveResult;
    /// Строит валидационную кривую для классификационной модели
    /// по целочисленной сетке параметра.
    /// paramValues — набор проверяемых значений гиперпараметра.
    /// modelFactory — функция создания модели по значению параметра.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function ValidationCurve(
      paramValues: array of integer;
      modelFactory: integer -> IClassifier;
      X: Matrix; y: array of integer;
      metric: (array of integer, array of integer) -> real;
      cv: sequence of (array of integer, array of integer)
    ): ValidationCurveResult;
    /// Строит валидационную кривую для классификационной модели
    /// по вещественной сетке параметра.
    /// paramValues — набор проверяемых значений гиперпараметра.
    /// modelFactory — функция создания модели по значению параметра.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function ValidationCurve(
      paramValues: array of real;
      modelFactory: real -> IClassifier;
      X: Matrix; y: array of integer;
      metric: (array of integer, array of integer) -> real;
      cv: sequence of (array of integer, array of integer)
    ): ValidationCurveResult;
    
    /// Строит кривую обучения для регрессионной модели
    /// по целочисленной сетке размеров обучающей выборки.
    /// trainSizes — набор проверяемых размеров обучающей выборки.
    /// modelFactory — функция создания модели.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function LearningCurve(
      trainSizes: array of integer;
      modelFactory: () -> IRegressor;
      X: Matrix; y: Vector;
      metric: (Vector, Vector) -> real;
      cv: sequence of (array of integer, array of integer)
    ): LearningCurveResult;
    /// Строит кривую обучения для регрессионной модели
    /// по вещественной сетке долей обучающей выборки.
    /// trainFractions — набор проверяемых долей обучающей выборки.
    /// modelFactory — функция создания модели.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function LearningCurve(
      trainFractions: array of real;
      modelFactory: () -> IRegressor;
      X: Matrix; y: Vector;
      metric: (Vector, Vector) -> real;
      cv: sequence of (array of integer, array of integer)
    ): LearningCurveResult;
    /// Строит кривую обучения для классификационной модели
    /// по целочисленной сетке размеров обучающей выборки.
    /// trainSizes — набор проверяемых размеров обучающей выборки.
    /// modelFactory — функция создания модели.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function LearningCurve(
      trainSizes: array of integer;
      modelFactory: () -> IClassifier;
      X: Matrix; y: array of integer;
      metric: (array of integer, array of integer) -> real;
      cv: sequence of (array of integer, array of integer)
    ): LearningCurveResult;
    /// Строит кривую обучения для классификационной модели
    /// по вещественной сетке долей обучающей выборки.
    /// trainFractions — набор проверяемых долей обучающей выборки.
    /// modelFactory — функция создания модели.
    /// X, y — обучающие данные.
    /// metric — функция качества (yTrue, yPred) -> real.
    /// cv — готовая последовательность фолдов в виде пар (trainIdx, testIdx).
    /// Возвращает объект, содержащий оценки по фолдам,
    /// а также средние значения и стандартные отклонения
    /// на обучающих и валидационных подвыборках.
    static function LearningCurve(
      trainFractions: array of real;
      modelFactory: () -> IClassifier;
      X: Matrix; y: array of integer;
      metric: (array of integer, array of integer) -> real;
      cv: sequence of (array of integer, array of integer)
    ): LearningCurveResult;
  end;
  
/// Класс для подбора гиперпараметров методом перебора по сетке (Grid Search).
/// Для каждого значения параметра выполняется k-кратная кросс-валидация.
/// Выбирается параметр, дающий наилучшее среднее значение метрики.
/// Используется для настройки регуляризации и других гиперпараметров моделей.
  GridSearch = static class
  public
    /// Выполняет подбор гиперпараметра по заданной сетке значений.
    /// • modelFactory — функция создания модели по значению параметра (P -> T).
    /// • paramValues — набор тестируемых значений гиперпараметра типа P.
    /// • X, y — обучающие данные.
    /// • k — число фолдов в кросс-валидации.
    /// • metric — функция оценки качества (yTrue, yPred) → real.
    /// • maximize — если true, максимизируется метрика; иначе минимизируется.
    /// • stratified — для регрессии должен оставаться false.
    /// • seed — seed для разбиения на фолды (для воспроизводимости).
    /// Возвращает кортеж:
    /// • лучший параметр,
    /// • лучшее среднее значение метрики,
    /// • модель, обученная на всём датасете с лучшим параметром
    /// 
    /// Все параметры оцениваются на одном и том же разбиении данных
    /// (используется фиксированный seed), что обеспечивает корректное и сопоставимое сравнение моделей
    class function Search<T, P>(
      modelFactory: P -> T;
      paramValues: array of P;
      X: Matrix; y: Vector;
      k: integer;
      metric: (Vector, Vector) -> real;
      maximize: boolean := True;
      stratified: boolean := False;
      seed: integer := -1
    ): (P, real, T); where T: class, IRegressor;
    /// Перегрузка для классификационных моделей.
    /// Здесь stratified включает стратифицированную кросс-валидацию по меткам классов.
    class function Search<T, P>(
      modelFactory: P -> T;
      paramValues: array of P;
      X: Matrix; y: array of integer;
      k: integer;
      metric: (array of integer, array of integer) -> real;
      maximize: boolean := True;
      stratified: boolean := False;
      seed: integer := -1
    ): (P, real, T); where T: class, IClassifier;
  end;

implementation

uses MLExceptions;
uses MLUtilsABC;

const
  ER_VALIDATION_CURVE_EMPTY =
    'ValidationCurveResult не содержит значений параметра!!ValidationCurveResult has no parameter values';
  ER_VALIDATION_CURVE_FOLDS_EMPTY =
    'ValidationCurve: список фолдов пуст!!ValidationCurve: folds list is empty';
  ER_DIM_MISMATCH_TRAIN_TEST =
    'Несоответствие размерностей в TrainTestSplit: X.RowCount={0}, y.Length={1}!!' +
    'Dimension mismatch in TrainTestSplit: X.RowCount={0}, y.Length={1}';
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
  ER_STRATIFIED_CLASS_TOO_SMALL =
    'Класс {0} содержит {1} объектов, что меньше числа фолдов ({2}). Уменьшите k или объедините малочисленные классы.!!' +
    'Class {0} has {1} samples, which is less than the number of folds ({2}). Reduce k or merge very small classes.';
  ER_STRATIFIED_K_TOO_LARGE =
    'Stratified CV: число фолдов ({0}) превышает минимальный размер класса ({1})!!Stratified CV: number of folds ({0}) exceeds smallest class size ({1})';
  ER_STRATIFIED_REGRESSION_NOT_SUPPORTED =
    'Стратифицированная кросс-валидация не поддерживается для регрессии!!Stratified cross-validation is not supported for regression';

//-----------------------------
//         Validation
//-----------------------------

function TakeVectorByIndices(y: Vector; indices: array of integer): Vector;
begin
  Result := new Vector(indices.Length);
  for var i := 0 to indices.Length - 1 do
    Result[i] := y[indices[i]];
end;

function TakeLabelsByIndices(y: array of integer; indices: array of integer): array of integer;
begin
  SetLength(Result, indices.Length);
  for var i := 0 to indices.Length - 1 do
    Result[i] := y[indices[i]];
end;

procedure FillRowMeansAndStd(scores: Matrix; means, stds: Vector);
begin
  var rows := scores.RowCount;
  var cols := scores.ColCount;
  
  for var i := 0 to rows - 1 do
  begin
    var sum := 0.0;
    for var j := 0 to cols - 1 do
      sum += scores[i, j];
    
    var mean := sum / cols;
    means[i] := mean;
    
    var sq := 0.0;
    for var j := 0 to cols - 1 do
    begin
      var d := scores[i, j] - mean;
      sq += d * d;
    end;
    
    stds[i] := Sqrt(sq / cols);
  end;
end;

function BuildValidationCurveResult(
  paramValues: Vector;
  trainScores, validationScores: Matrix
): ValidationCurveResult;
begin
  Result := new ValidationCurveResult;
  Result.ParameterValues := paramValues;
  Result.TrainScores := trainScores;
  Result.ValidationScores := validationScores;
  Result.TrainMean := new Vector(trainScores.RowCount);
  Result.ValidationMean := new Vector(validationScores.RowCount);
  Result.TrainStd := new Vector(trainScores.RowCount);
  Result.ValidationStd := new Vector(validationScores.RowCount);
  
  FillRowMeansAndStd(trainScores, Result.TrainMean, Result.TrainStd);
  FillRowMeansAndStd(validationScores, Result.ValidationMean, Result.ValidationStd);
end;

function BuildLearningCurveResult(
  trainSizes: Vector;
  trainScores, validationScores: Matrix
): LearningCurveResult;
begin
  Result := new LearningCurveResult;
  Result.TrainSizes := trainSizes;
  Result.TrainScores := trainScores;
  Result.ValidationScores := validationScores;
  Result.TrainMean := new Vector(trainScores.RowCount);
  Result.ValidationMean := new Vector(validationScores.RowCount);
  Result.TrainStd := new Vector(trainScores.RowCount);
  Result.ValidationStd := new Vector(validationScores.RowCount);
  
  FillRowMeansAndStd(trainScores, Result.TrainMean, Result.TrainStd);
  FillRowMeansAndStd(validationScores, Result.ValidationMean, Result.ValidationStd);
end;

function MinTrainFoldSize(folds: array of (array of integer, array of integer)): integer;
begin
  Result := integer.MaxValue;
  
  foreach var (trainIdx, testIdx) in folds do
    if trainIdx.Length < Result then
      Result := trainIdx.Length;
end;

function ShuffledCopy(indices: array of integer; seed: integer): array of integer;
begin
  Result := Copy(indices);
  var rnd := new System.Random(seed);
  Result.Shuffle(rnd);
end;

function PrefixIndices(indices: array of integer; count: integer): array of integer;
begin
  Result := new integer[count];
  System.Array.Copy(indices, 0, Result, 0, count);
end;

function NormalizeTrainSizes(
  trainSizes: array of integer;
  minTrainSize: integer
): array of integer;
begin
  SetLength(Result, trainSizes.Length);
  
  for var i := 0 to trainSizes.Length - 1 do
  begin
    var sz := trainSizes[i];
    if (sz <= 0) or (sz > minTrainSize) then
      ArgumentError(ER_INVALID_VALUE, 'trainSizes');
    Result[i] := sz;
  end;
end;

function NormalizeTrainFractions(
  trainFractions: array of real;
  minTrainSize: integer
): array of integer;
begin
  SetLength(Result, trainFractions.Length);
  
  for var i := 0 to trainFractions.Length - 1 do
  begin
    var frac := trainFractions[i];
    if (frac <= 0.0) or (frac > 1.0) then
      ArgumentError(ER_INVALID_VALUE, 'trainFractions');
    
    var sz := Round(minTrainSize * frac);
    sz := sz.Clamp(1, minTrainSize);
    Result[i] := sz;
  end;
end;

function ValidationCurveResult.Count: integer;
begin
  if ParameterValues = nil then
    Result := 0
  else
    Result := ParameterValues.Length;
end;

function ValidationCurveResult.BestIndex(maximize: boolean): integer;
begin
  if (ValidationMean = nil) or (ValidationMean.Length = 0) then
    ArgumentError(ER_VALIDATION_CURVE_EMPTY);
  
  Result := 0;
  
  for var i := 1 to ValidationMean.Length - 1 do
    if maximize then
    begin
      if ValidationMean[i] > ValidationMean[Result] then
        Result := i;
    end
    else
    begin
      if ValidationMean[i] < ValidationMean[Result] then
        Result := i;
    end;
end;

function ValidationCurveResult.BestParam(maximize: boolean): real;
begin
  var i := BestIndex(maximize);
  
  if (ParameterValues = nil) or (i >= ParameterValues.Length) then
    ArgumentError(ER_VALIDATION_CURVE_EMPTY);
  
  Result := ParameterValues[i];
end;

function ValidationCurveResult.BestScore(maximize: boolean): real;
begin
  Result := ValidationMean[BestIndex(maximize)];
end;

function LearningCurveResult.Count: integer;
begin
  if TrainSizes = nil then
    Result := 0
  else
    Result := TrainSizes.Length;
end;

static function Validation.CrossValidateCore(
  model: IRegressor; 
  X: Matrix; 
  y: Vector;
  folds: sequence of (array of integer, array of integer);
  metric: (Vector, Vector) -> real
): real;
begin
  var total := 0.0;
  var foldsCount := 0;
  var p := X.ColCount;

  foreach var (trainIdx, testIdx) in folds do
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

    var m := model.Clone() as IRegressor;
    m := m.Fit(Xtr, ytr) as IRegressor;

    var pred := m.Predict(Xte);

    total += metric(yte, pred);
    foldsCount += 1;
  end;

  if foldsCount = 0 then
    ArgumentError(ER_EMPTY_DATA, 'CrossValidate');

  Result := total / foldsCount;
end;

static function Validation.CrossValidateCore(
  model: IClassifier;
  X: Matrix;
  y: array of integer;
  folds: sequence of (array of integer, array of integer);
  metric: (array of integer, array of integer) -> real
): real;
begin
  var total := 0.0;
  var foldsCount := 0;
  var p := X.ColCount;

  foreach var (trainIdx, testIdx) in folds do
  begin
    var Xtr := new Matrix(trainIdx.Length, p);
    var ytr := new integer[trainIdx.Length];

    for var i := 0 to trainIdx.Length - 1 do
    begin
      var r := trainIdx[i];
      for var j := 0 to p - 1 do
        Xtr[i,j] := X[r,j];
      ytr[i] := y[r];
    end;

    var Xte := new Matrix(testIdx.Length, p);
    var yte := new integer[testIdx.Length];

    for var i := 0 to testIdx.Length - 1 do
    begin
      var r := testIdx[i];
      for var j := 0 to p - 1 do
        Xte[i,j] := X[r,j];
      yte[i] := y[r];
    end;

    var m := model.Clone() as IClassifier;
    m := m.Fit(Xtr, ytr) as IClassifier;

    var pred := m.Predict(Xte);

    total += metric(yte, pred);
    foldsCount += 1;
  end;

  if foldsCount = 0 then
    ArgumentError(ER_EMPTY_DATA, 'CrossValidate');

  Result := total / foldsCount;
end;

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

static function Validation.TrainTestSplit(X: Matrix; y: array of integer;
  testRatio: real; seed: integer): (Matrix, Matrix, array of integer, array of integer);
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
  idx.Shuffle(rnd);

  var rawSize := Round(n * testRatio);
  var testSize := rawSize.Clamp(1, n - 1);
  var trainSize := n - testSize;

  var X_train := new Matrix(trainSize, p);
  var X_test  := new Matrix(testSize, p);

  var y_train := new integer[trainSize];
  var y_test  := new integer[testSize];

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

  // --- RNG (без дублирования логики seed)
  var rnd :=
    if seed >= 0 then new System.Random(seed)
    else new System.Random;

  // --- 1. Индексы 0..n-1
  var idx := Arr(0..n-1);

  // --- 2. Перемешивание
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

    System.Array.Copy(idx, 0, trainIdx, 0, start);
    System.Array.Copy(idx, start + size, trainIdx, start, n - (start + size));

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

  var rnd :=
    if seed >= 0 then new System.Random(seed)
    else new System.Random;

  // --- 1. Индексы по классам
  var classMap := new Dictionary<integer, List<integer>>();

  for var i := 0 to n - 1 do
  begin
    var v := y[i];
    var cls := Round(v);

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

  // --- 1.1 ПРОВЕРКА НА МИНИМАЛЬНЫЙ РАЗМЕР КЛАССА
  foreach var pair in classMap do
  begin
    var cls := pair.Key;
    var cnt := pair.Value.Count;

    // Класс может иметь меньше объектов, чем число фолдов.
    // В библиотеке принята строгая политика: такие случаи считаются ошибкой,
    // так как не гарантируется присутствие класса во всех train-fold.
    // Поэтому выполняется fail-fast проверка (см. ниже).
     if cnt < k then
      ArgumentError(ER_STRATIFIED_CLASS_TOO_SMALL, cls, cnt, k);
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

static function Validation.StratifiedKFold(y: array of integer; k: integer; seed: integer):
  sequence of (array of integer, array of integer);
begin
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  var n := y.Length;

  if n <= 0 then
    ArgumentError(ER_EMPTY_DATA, 'StratifiedKFold');

  if (k < 2) or (k > n) then
    ArgumentError(ER_K_INVALID_STRATIFIED, k, n);

  var rnd :=
    if seed >= 0 then new System.Random(seed)
    else new System.Random;

  var classMap := new Dictionary<integer, List<integer>>();

  for var i := 0 to n - 1 do
  begin
    var cls := y[i];

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

  foreach var pair in classMap do
  begin
    var cls := pair.Key;
    var cnt := pair.Value.Count;
     if cnt < k then
      ArgumentError(ER_STRATIFIED_CLASS_TOO_SMALL, cls, cnt, k);
  end;

  var folds := new List<integer>[k];
  for var f := 0 to k - 1 do
    folds[f] := new List<integer>;

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
  model: IRegressor; 
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

  var baseSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  Result := CrossValidateCore(
    model, 
    X, 
    y,
    KFold(X.RowCount, k, baseSeed),
    metric
  );
end;

static function Validation.CrossValidate(
  model: IClassifier;
  X: Matrix;
  y: array of integer;
  k: integer;
  metric: (array of integer, array of integer) -> real;
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

  var baseSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  Result := CrossValidateCore(
    model,
    X,
    y,
    KFold(X.RowCount, k, baseSeed),
    metric
  );
end;

static function Validation.StratifiedCrossValidate(
  model: IClassifier;
  X: Matrix;
  y: array of integer;
  k: integer;
  metric: (array of integer, array of integer) -> real;
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
 
  var baseSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  Result := CrossValidateCore(
    model,
    X,
    y,
    StratifiedKFold(y, k, baseSeed),
    metric
  );
end;

static function Validation.ValidationCurve(
  paramValues: array of integer;
  modelFactory: integer -> IRegressor;
  X: Matrix; y: Vector;
  metric: (Vector, Vector) -> real;
  cv: sequence of (array of integer, array of integer)
): ValidationCurveResult;
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
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var trainScores := new Matrix(paramValues.Length, folds.Length);
  var validationScores := new Matrix(paramValues.Length, folds.Length);
  
  for var pi := 0 to paramValues.Length - 1 do
  begin
    var model0 := modelFactory(paramValues[pi]);
    if model0 = nil then
      ArgumentNullError(ER_ARG_NULL, 'modelFactory(param)');
    
    for var fi := 0 to folds.Length - 1 do
    begin
      var (trainIdx, testIdx) := folds[fi];
      var Xtr := X.TakeRows(trainIdx);
      var Xte := X.TakeRows(testIdx);
      var ytr := TakeVectorByIndices(y, trainIdx);
      var yte := TakeVectorByIndices(y, testIdx);
      
      var model := model0.Clone() as IRegressor;
      model := model.Fit(Xtr, ytr) as IRegressor;
      
      trainScores[pi, fi] := metric(ytr, model.Predict(Xtr));
      validationScores[pi, fi] := metric(yte, model.Predict(Xte));
    end;
  end;
  
  Result := BuildValidationCurveResult(new Vector(paramValues), trainScores, validationScores);
end;

static function Validation.ValidationCurve(
  paramValues: array of real;
  modelFactory: real -> IRegressor;
  X: Matrix; y: Vector;
  metric: (Vector, Vector) -> real;
  cv: sequence of (array of integer, array of integer)
): ValidationCurveResult;
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
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var trainScores := new Matrix(paramValues.Length, folds.Length);
  var validationScores := new Matrix(paramValues.Length, folds.Length);
  
  for var pi := 0 to paramValues.Length - 1 do
  begin
    var model0 := modelFactory(paramValues[pi]);
    if model0 = nil then
      ArgumentNullError(ER_ARG_NULL, 'modelFactory(param)');
    
    for var fi := 0 to folds.Length - 1 do
    begin
      var (trainIdx, testIdx) := folds[fi];
      var Xtr := X.TakeRows(trainIdx);
      var Xte := X.TakeRows(testIdx);
      var ytr := TakeVectorByIndices(y, trainIdx);
      var yte := TakeVectorByIndices(y, testIdx);
      
      var model := model0.Clone() as IRegressor;
      model := model.Fit(Xtr, ytr) as IRegressor;
      
      trainScores[pi, fi] := metric(ytr, model.Predict(Xtr));
      validationScores[pi, fi] := metric(yte, model.Predict(Xte));
    end;
  end;
  
  Result := BuildValidationCurveResult(new Vector(paramValues), trainScores, validationScores);
end;

static function Validation.ValidationCurve(
  paramValues: array of integer;
  modelFactory: integer -> IClassifier;
  X: Matrix; y: array of integer;
  metric: (array of integer, array of integer) -> real;
  cv: sequence of (array of integer, array of integer)
): ValidationCurveResult;
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
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var trainScores := new Matrix(paramValues.Length, folds.Length);
  var validationScores := new Matrix(paramValues.Length, folds.Length);
  
  for var pi := 0 to paramValues.Length - 1 do
  begin
    var model0 := modelFactory(paramValues[pi]);
    if model0 = nil then
      ArgumentNullError(ER_ARG_NULL, 'modelFactory(param)');
    
    for var fi := 0 to folds.Length - 1 do
    begin
      var (trainIdx, testIdx) := folds[fi];
      var Xtr := X.TakeRows(trainIdx);
      var Xte := X.TakeRows(testIdx);
      var ytr := TakeLabelsByIndices(y, trainIdx);
      var yte := TakeLabelsByIndices(y, testIdx);
      
      var model := model0.Clone() as IClassifier;
      model := model.Fit(Xtr, ytr) as IClassifier;
      
      trainScores[pi, fi] := metric(ytr, model.Predict(Xtr));
      validationScores[pi, fi] := metric(yte, model.Predict(Xte));
    end;
  end;
  
  Result := BuildValidationCurveResult(new Vector(paramValues), trainScores, validationScores);
end;

static function Validation.ValidationCurve(
  paramValues: array of real;
  modelFactory: real -> IClassifier;
  X: Matrix; y: array of integer;
  metric: (array of integer, array of integer) -> real;
  cv: sequence of (array of integer, array of integer)
): ValidationCurveResult;
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
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var trainScores := new Matrix(paramValues.Length, folds.Length);
  var validationScores := new Matrix(paramValues.Length, folds.Length);
  
  for var pi := 0 to paramValues.Length - 1 do
  begin
    var model0 := modelFactory(paramValues[pi]);
    if model0 = nil then
      ArgumentNullError(ER_ARG_NULL, 'modelFactory(param)');
    
    for var fi := 0 to folds.Length - 1 do
    begin
      var (trainIdx, testIdx) := folds[fi];
      var Xtr := X.TakeRows(trainIdx);
      var Xte := X.TakeRows(testIdx);
      var ytr := TakeLabelsByIndices(y, trainIdx);
      var yte := TakeLabelsByIndices(y, testIdx);
      
      var model := model0.Clone() as IClassifier;
      model := model.Fit(Xtr, ytr) as IClassifier;
      
      trainScores[pi, fi] := metric(ytr, model.Predict(Xtr));
      validationScores[pi, fi] := metric(yte, model.Predict(Xte));
    end;
  end;
  
  Result := BuildValidationCurveResult(new Vector(paramValues), trainScores, validationScores);
end;

static function Validation.LearningCurve(
  trainSizes: array of integer;
  modelFactory: () -> IRegressor;
  X: Matrix; y: Vector;
  metric: (Vector, Vector) -> real;
  cv: sequence of (array of integer, array of integer)
): LearningCurveResult;
begin
  if modelFactory = nil then
    ArgumentNullError(ER_ARG_NULL, 'modelFactory');
  if trainSizes = nil then
    ArgumentNullError(ER_ARG_NULL, 'trainSizes');
  if trainSizes.Length = 0 then
    ArgumentError(ER_PARAM_VALUES_EMPTY);
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');
  if metric = nil then
    ArgumentNullError(ER_ARG_NULL, 'metric');
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var minTrainSize := MinTrainFoldSize(folds);
  var sizes := NormalizeTrainSizes(trainSizes, minTrainSize);
  var trainScores := new Matrix(sizes.Length, folds.Length);
  var validationScores := new Matrix(sizes.Length, folds.Length);
  
  for var fi := 0 to folds.Length - 1 do
  begin
    var (trainIdxFull, testIdx) := folds[fi];
    var trainIdxShuffled := ShuffledCopy(trainIdxFull, 12345 + fi);
    var Xte := X.TakeRows(testIdx);
    var yte := TakeVectorByIndices(y, testIdx);
    
    for var si := 0 to sizes.Length - 1 do
    begin
      var subIdx := PrefixIndices(trainIdxShuffled, sizes[si]);
      var Xtr := X.TakeRows(subIdx);
      var ytr := TakeVectorByIndices(y, subIdx);
      var model := modelFactory();
      if model = nil then
        ArgumentNullError(ER_ARG_NULL, 'modelFactory()');
      model := model.Fit(Xtr, ytr) as IRegressor;
      trainScores[si, fi] := metric(ytr, model.Predict(Xtr));
      validationScores[si, fi] := metric(yte, model.Predict(Xte));
    end;
  end;
  
  Result := BuildLearningCurveResult(new Vector(sizes), trainScores, validationScores);
end;

static function Validation.LearningCurve(
  trainFractions: array of real;
  modelFactory: () -> IRegressor;
  X: Matrix; y: Vector;
  metric: (Vector, Vector) -> real;
  cv: sequence of (array of integer, array of integer)
): LearningCurveResult;
begin
  if trainFractions = nil then
    ArgumentNullError(ER_ARG_NULL, 'trainFractions');
  if trainFractions.Length = 0 then
    ArgumentError(ER_PARAM_VALUES_EMPTY);
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var sizes := NormalizeTrainFractions(trainFractions, MinTrainFoldSize(folds));
  Result := LearningCurve(sizes, modelFactory, X, y, metric, folds);
end;

static function Validation.LearningCurve(
  trainSizes: array of integer;
  modelFactory: () -> IClassifier;
  X: Matrix; y: array of integer;
  metric: (array of integer, array of integer) -> real;
  cv: sequence of (array of integer, array of integer)
): LearningCurveResult;
begin
  if modelFactory = nil then
    ArgumentNullError(ER_ARG_NULL, 'modelFactory');
  if trainSizes = nil then
    ArgumentNullError(ER_ARG_NULL, 'trainSizes');
  if trainSizes.Length = 0 then
    ArgumentError(ER_PARAM_VALUES_EMPTY);
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');
  if metric = nil then
    ArgumentNullError(ER_ARG_NULL, 'metric');
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var minTrainSize := MinTrainFoldSize(folds);
  var sizes := NormalizeTrainSizes(trainSizes, minTrainSize);
  var trainScores := new Matrix(sizes.Length, folds.Length);
  var validationScores := new Matrix(sizes.Length, folds.Length);
  
  for var fi := 0 to folds.Length - 1 do
  begin
    var (trainIdxFull, testIdx) := folds[fi];
    var trainIdxShuffled := ShuffledCopy(trainIdxFull, 12345 + fi);
    var Xte := X.TakeRows(testIdx);
    var yte := TakeLabelsByIndices(y, testIdx);
    
    for var si := 0 to sizes.Length - 1 do
    begin
      var subIdx := PrefixIndices(trainIdxShuffled, sizes[si]);
      var Xtr := X.TakeRows(subIdx);
      var ytr := TakeLabelsByIndices(y, subIdx);
      var model := modelFactory();
      if model = nil then
        ArgumentNullError(ER_ARG_NULL, 'modelFactory()');
      model := model.Fit(Xtr, ytr) as IClassifier;
      trainScores[si, fi] := metric(ytr, model.Predict(Xtr));
      validationScores[si, fi] := metric(yte, model.Predict(Xte));
    end;
  end;
  
  Result := BuildLearningCurveResult(new Vector(sizes), trainScores, validationScores);
end;

static function Validation.LearningCurve(
  trainFractions: array of real;
  modelFactory: () -> IClassifier;
  X: Matrix; y: array of integer;
  metric: (array of integer, array of integer) -> real;
  cv: sequence of (array of integer, array of integer)
): LearningCurveResult;
begin
  if trainFractions = nil then
    ArgumentNullError(ER_ARG_NULL, 'trainFractions');
  if trainFractions.Length = 0 then
    ArgumentError(ER_PARAM_VALUES_EMPTY);
  if cv = nil then
    ArgumentNullError(ER_ARG_NULL, 'cv');
  
  var folds := cv.ToArray;
  if folds.Length = 0 then
    ArgumentError(ER_VALIDATION_CURVE_FOLDS_EMPTY);
  
  var sizes := NormalizeTrainFractions(trainFractions, MinTrainFoldSize(folds));
  Result := LearningCurve(sizes, modelFactory, X, y, metric, folds);
end;

//-----------------------------
//         GridSearch
//-----------------------------

class function GridSearch.Search<T, P>(
  modelFactory: P -> T;
  paramValues: array of P;
  X: Matrix; 
  y: Vector;
  k: integer;
  metric: (Vector, Vector) -> real;
  maximize: boolean;
  stratified: boolean;
  seed: integer
): (P, real, T); where T: class, IRegressor;
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

  var baseSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  foreach var param in paramValues do
  begin
    var model := modelFactory(param);
    if model = nil then
      ArgumentError(ER_MODEL_NULL);

    var avgScore :=
      Validation.CrossValidate(model as IRegressor, X, y, k, metric, baseSeed);

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

class function GridSearch.Search<T, P>(
  modelFactory: P -> T;
  paramValues: array of P;
  X: Matrix;
  y: array of integer;
  k: integer;
  metric: (array of integer, array of integer) -> real;
  maximize: boolean;
  stratified: boolean;
  seed: integer
): (P, real, T); where T: class, IClassifier;
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

  if stratified then
    ArgumentError(ER_STRATIFIED_REGRESSION_NOT_SUPPORTED);

  var bestParam := paramValues[0];
  var bestScore :=
    if maximize then -1e308 else 1e308;

  var baseSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  foreach var param in paramValues do
  begin
    var model := modelFactory(param);
    if model = nil then
      ArgumentError(ER_MODEL_NULL);

    var avgScore :=
      if stratified then
        Validation.StratifiedCrossValidate(model as IClassifier, X, y, k, metric, baseSeed)
      else
        Validation.CrossValidate(model as IClassifier, X, y, k, metric, baseSeed);

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
