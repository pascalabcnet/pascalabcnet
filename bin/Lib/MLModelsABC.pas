unit MLModelsABC;

interface

uses MLCoreABC;
uses LinearAlgebraML;

type
/// Активационные функции для моделей
  Activations = static class
  public  
    /// Функция активации Sigmoid (логистическая функция).
    /// Преобразует любое число в диапазон (0, 1).
    /// Используется в логистической регрессии
    /// и других моделях для получения вероятности.
    /// Формула: σ(x) = 1 / (1 + e^(−x)).  
    static function Sigmoid(v: Vector): Vector;
    
    /// Функция активации Tanh (гиперболический тангенс).
    /// Похожа на Sigmoid, но значения лежат в диапазоне (−1, 1).
    /// Применяется в моделях, где требуется
    /// симметричная нелинейность относительно нуля.
    /// Формула: tanh(x) = (e^x − e^(−x)) / (e^x + e^(−x)).
    static function Tanh(v: Vector): Vector;
    
    /// Функция активации ReLU (Rectified Linear Unit).
    /// Отрицательные числа заменяет на 0,
    /// положительные оставляет без изменений.
    /// Используется как простое нелинейное преобразование
    /// в моделях машинного обучения.
    /// Формула: ReLU(x) = max(0, x).
    static function ReLU(v: Vector): Vector;
    
    /// Функция активации Softmax.
    /// Преобразует набор значений в вероятностное распределение:
    /// все элементы неотрицательны,
    /// их сумма равна 1.
    /// Используется в задачах многоклассовой классификации.
    /// Каждый элемент интерпретируется как вероятность класса.
    /// Формула: softmax(x_i) = e^{x_i} / Σ e^{x_j}.
    static function Softmax(v: Vector): Vector;
  end; 
  
  IModel = MLCoreABC.IModel;
/// Линейная регрессионная модель
/// Предсказывает числовое значение по линейной комбинации признаков
  LinearRegression = class(IRegressor)
  private
    fCoef: Vector;
    fIntercept: real;
    fLambda: real;
    fFitted: boolean;
  public
    constructor Create(lambda: real := 0.0);

    /// Обучает модель на числовых данных
    /// X — матрица m × n (m объектов, n признаков)
    /// y — вектор длины m
    function Fit(X: Matrix; y: Vector): IModel;

    /// Предсказывает значения для матрицы признаков
    /// Возвращает вектор длины m
    function Predict(X: Matrix): Vector;
    
/// Вектор коэффициентов модели (веса признаков).
/// Каждый элемент показывает вклад соответствующего признака в линейную комбинацию.
/// Доступен после обучения (Fit).
    property Coefficients: Vector read fCoef;
    
/// Свободный член модели (смещение, bias).
/// Добавляется к линейной комбинации признаков.
/// Интерпретируется как значение предсказания, когда все признаки равны нулю.
    property Intercept: real read fIntercept;
    
/// Показывает, была ли модель обучена.
/// После вызова Fit значение становится true.
/// Используется для проверки корректности вызова Predict.
    property IsFitted: boolean read fFitted;
  end;  
  
  /// Логистическая регрессионная модель для бинарной классификации.
  /// Предсказывает вероятность принадлежности объекта к классу 1
  /// на основе линейной комбинации признаков и сигмоидной функции.
  /// Поддерживает L2-регуляризацию.
  LogisticRegression = class(IClassifier)
  private
    fCoef: Vector;
    fIntercept: real;
    fLambda: real;
    fLearningRate: real;
    fEpochs: integer;
    fFitted: boolean;
  public
    /// Создаёт модель логистической регрессии.
    /// lambda — коэффициент L2-регуляризации (0 — без регуляризации).
    /// lr — шаг градиентного спуска.
    /// epochs — число итераций обучения.
    constructor Create(lambda: real := 0.0; lr: real := 0.1; epochs: integer := 1000);
  
    /// Обучает модель на числовых данных.
    /// X — матрица m × n (m объектов, n признаков).
    /// y — вектор длины m, содержащий метки классов (0 или 1).
    function Fit(X: Matrix; y: Vector): IModel;
  
    /// Возвращает вероятность принадлежности к положительному классу (1)
    /// для каждого объекта.
    /// Результат — вектор длины m со значениями в диапазоне (0, 1).
    function PredictProba(X: Matrix): Vector;
  
    /// Выполняет бинарную классификацию с порогом 0.5.
    /// Возвращает вектор из 0 и 1.
    function Predict(X: Matrix): Vector;
  
    /// Выполняет бинарную классификацию с заданным порогом.
    /// threshold — значение в диапазоне (0, 1).
    /// Если вероятность ≥ threshold, возвращается 1, иначе 0.
    function Predict(X: Matrix; threshold: real): Vector;
  
    /// Вектор коэффициентов модели (веса признаков).
    /// Длина равна числу признаков.
    /// Доступен после обучения (Fit).
    property Coefficients: Vector read fCoef;
  
    /// Свободный член модели (смещение, bias).
    /// Добавляется к линейной комбинации признаков.
    property Intercept: real read fIntercept;
  
    /// Показывает, была ли модель обучена.
    /// После вызова Fit значение становится true.
    property IsFitted: boolean read fFitted;
  end;

  /// Конвейер машинного обучения (Pipeline).
  /// Объединяет несколько шагов подготовки данных и модель
  /// в единую последовательность обработки.
  /// Сначала данные проходят через все преобразования,
  /// затем итоговые признаки передаются в модель.
  /// Обеспечивает единый интерфейс Fit / Predict.
  Pipeline = class(IModel)
  private
    fTransformers: List<ITransformer>;
    fModel: IModel;
    fFitted: boolean;
  public
    /// Создаёт пайплайн (конвейер машинного обучения) для заданной модели.
    /// model — финальная модель (регрессор или классификатор),
    /// которая будет обучаться после всех преобразований.
    constructor Create(model: IModel);

    /// Строит пайплайн (конвейер машинного обучения).
    /// model — финальная модель, которая будет обучаться.
    /// transformers — последовательность преобразований признаков,
    ///   применяемых к данным перед обучением модели.
    /// Возвращает настроенный конвейер.
    static function Build(model: IModel; params transformers: array of ITransformer): Pipeline;
  
    /// Добавляет преобразование в конец пайплайна
    function Add(t: ITransformer): Pipeline;
  
    /// Обучает преобразования и модель
    function Fit(X: Matrix; y: Vector): IModel;
  
    /// Применяет только преобразования (без модели)
    function Transform(X: Matrix): Matrix;
  
    /// Делает предсказание
    function Predict(X: Matrix): Vector;
  
    /// Возвращает вероятности (если модель поддерживает)
    function PredictProba(X: Matrix): Vector;
  
    /// Показывает, был ли пайплайн обучен (вызван метод Fit).
    property IsFitted: boolean read fFitted;
  end;
  
  /// Стандартизирует признаки: вычитает среднее
  /// и делит на стандартное отклонение по каждому столбцу.
  /// Используется для приведения признаков к сопоставимому масштабу.
  StandardScaler = class(ITransformer)
  private
    fMean: Vector;
    fStd: Vector;
    fFitted: boolean;
  public
    /// Вычисляет среднее и стандартное отклонение по каждому признаку.
    function Fit(X: Matrix): ITransformer;
  
    /// Применяет стандартизацию к данным.
    function Transform(X: Matrix): Matrix;
  
    /// Средние значения признаков, вычисленные при обучении.
    property Mean: Vector read fMean;
  
    /// Стандартные отклонения признаков, вычисленные при обучении.
    property Std: Vector read fStd;
  
    /// Признак того, что преобразование обучено.
    property IsFitted: boolean read fFitted;
  end;
  
/// Масштабирует признаки в заданный диапазон
/// (по умолчанию [0, 1]) на основе минимального и максимального значения каждого столбца.
/// Используется для приведения признаков к единому масштабу без центрирования.
  MinMaxScaler = class(ITransformer)
  private
    fMin: Vector;
    fMax: Vector;
    fFitted: boolean;
    fRangeMin: real;
    fRangeMax: real;
  public
    /// Создаёт MinMaxScaler с диапазоном [rangeMin, rangeMax].
    /// По умолчанию масштабирование выполняется к [0, 1].
    constructor Create(rangeMin: real := 0.0; rangeMax: real := 1.0);
    /// Вычисляет минимальные и максимальные значения
    /// по каждому признаку.
    function Fit(X: Matrix): ITransformer;
  
    /// Применяет линейное масштабирование признаков
    /// к диапазону [0, 1].
    function Transform(X: Matrix): Matrix;
  
    /// Минимальные значения признаков,
    /// вычисленные при обучении.
    property Min: Vector read fMin;
  
    /// Максимальные значения признаков,
    /// вычисленные при обучении.
    property Max: Vector read fMax;
  
    /// Признак того, что преобразование обучено.
    property IsFitted: boolean read fFitted;
  end;
  
  
implementation  

//-----------------------------
//       LinearRegression
//-----------------------------

constructor LinearRegression.Create(lambda: real);
begin
  self.flambda := lambda;
  ffitted := false;
end;

function LinearRegression.Fit(X: Matrix; y: Vector): IModel;
begin
  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    raise new Exception('Empty dataset');
  if y.Length <> n then
    raise new Exception('Dimension mismatch in Fit');

  // 1. Means
  var meanX := X.ColumnMeans;
  var meanY := y.Mean;

  // 2. XtX and XtY
  var Xt := X.Transpose;
  var XtX := Xt * X;
  var XtY := Xt * y;

  // 3. Centering correction
  XtX := XtX - n * Matrix.OuterProduct(meanX, meanX);
  XtY := XtY - n * meanX * meanY;
 
  // 4. Ridge regularization
  if flambda > 0 then
    XtX := XtX + flambda * Matrix.Identity(p);

  // 5. Solve
  fcoef := Solve(XtX, XtY);

  // 6. Intercept
  fintercept := meanY - meanX.Dot(fcoef);

  ffitted := true;
  Result := Self;
end;

function LinearRegression.Predict(X: Matrix): Vector;
begin
  if not ffitted then
    raise new Exception('Model not fitted');

  if X.ColCount <> fcoef.Length then
    raise new Exception('Dimension mismatch in Predict');

  Result := X * fcoef + intercept;
end;

//-----------------------------
//       Activations
//-----------------------------

static function Activations.Sigmoid(v: Vector): Vector := v.Apply(x -> 1.0 / (1.0 + Exp(-x)));

static function Activations.Tanh(v: Vector): Vector := v.Apply(System.Math.Tanh);

static function Activations.ReLU(v: Vector): Vector := v.Apply(x -> (if x > 0 then x else 0.0));

static function Activations.Softmax(v: Vector): Vector;
begin
  var maxVal := v.Max;
  var shifted := v.Apply(x -> Exp(x - maxVal));
  var sumExp := shifted.Sum;
  if sumExp = 0 then
    exit(new Vector(v.Length));
  Result := shifted / sumExp;
end;

//-----------------------------
//       LogisticRegression
//-----------------------------

constructor LogisticRegression.Create(lambda: real; lr: real; epochs: integer);
begin
  fLambda := lambda;
  fLearningRate := lr;
  fEpochs := epochs;
  fFitted := false;
end;

function LogisticRegression.Fit(X: Matrix; y: Vector): IModel;
begin
  var n := X.RowCount;
  var p := X.ColCount;

  if y.Length <> n then
    raise new Exception('Dimension mismatch in Fit');

  fCoef := new Vector(p);
  fIntercept := 0.0;

  for var epoch := 1 to fEpochs do
  begin
    var z := X * fCoef + fIntercept;
    var yHat := Activations.Sigmoid(z);

    var error := yHat - y;

    var gradW := (X.Transpose * error) / n;

    if fLambda > 0 then
      gradW := gradW + fLambda * fCoef;

    var gradB := error.Sum / n;

    fCoef := fCoef - fLearningRate * gradW;
    fIntercept -= fLearningRate * gradB;
  end;

  fFitted := true;
  Result := Self;
end;

function LogisticRegression.PredictProba(X: Matrix): Vector;
begin
  if not fFitted then
    raise new Exception('Model not fitted');

  Result := Activations.Sigmoid(X * fCoef + fIntercept);
end;

function LogisticRegression.Predict(X: Matrix; threshold: real): Vector;
begin
  var p := PredictProba(X);
  Result := new Vector(p.Length);

  for var i := 0 to p.Length - 1 do
    if p[i] >= threshold then
      Result[i] := 1.0
    else
      Result[i] := 0.0;
end;

function LogisticRegression.Predict(X: Matrix): Vector;
begin
  Result := Predict(X, 0.5);
end;

//-----------------------------
//          Pipeline 
//-----------------------------

constructor Pipeline.Create(model: IModel);
begin
  fModel := model;
  fTransformers := new List<ITransformer>;
  fFitted := false;
end;

static function Pipeline.Build(model: IModel; transformers: array of ITransformer): Pipeline;
begin
  var pipe := new Pipeline(model);

  foreach var t in transformers do
    pipe.Add(t);

  Result := pipe;
end;

function Pipeline.Add(t: ITransformer): Pipeline;
begin
  fTransformers.Add(t);
  Result := self;
end;

function Pipeline.Fit(X: Matrix; y: Vector): IModel;
begin
  var Xcur := X;

  foreach var t in fTransformers do
  begin
    t.Fit(Xcur);
    Xcur := t.Transform(Xcur);
  end;

  fModel.Fit(Xcur, y);
  fFitted := true;

  Result := self;
end;

function Pipeline.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new Exception('Pipeline is not fitted');

  var Xt := X;

  foreach var t in fTransformers do
    Xt := t.Transform(Xt);

  Result := Xt;
end;

function Pipeline.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    raise new Exception('Pipeline not fitted');

  var Xcur := X;

  foreach var t in fTransformers do
    Xcur := t.Transform(Xcur);

  Result := fModel.Predict(Xcur);
end;

function Pipeline.PredictProba(X: Matrix): Vector;
begin
  if not fFitted then
    raise new Exception('Pipeline is not fitted');

  var Xt := X;

  foreach var t in fTransformers do
    Xt := t.Transform(Xt);

  if fModel is IProbabilisticClassifier then
    exit(IProbabilisticClassifier(fModel).PredictProba(Xt));

  raise new Exception('Model does not support probability prediction');
end;

//-----------------------------
//        StandardScaler
//-----------------------------

function StandardScaler.Fit(X: Matrix): ITransformer;
begin
  fMean := X.ColumnMeans;
  fStd := X.ColumnStd;

  fFitted := true;
  Result := self;
end;

function StandardScaler.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new Exception('StandardScaler not fitted');

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);

  for var j := 0 to p - 1 do
  begin
    var mean := fMean[j];
    var std := fStd[j];

    for var i := 0 to n - 1 do
    begin
      if std <> 0 then
        Result[i,j] := (X[i,j] - mean) / std
      else Result[i,j] := 0.0;
    end;
  end;
end;

//-----------------------------
//       MinMaxScaler
//-----------------------------

constructor MinMaxScaler.Create(rangeMin: real; rangeMax: real);
begin
  if rangeMax <= rangeMin then
    raise new Exception('rangeMax must be greater than rangeMin');

  fRangeMin := rangeMin;
  fRangeMax := rangeMax;
  fFitted := false;
end;

function MinMaxScaler.Fit(X: Matrix): ITransformer;
begin
  fMin := X.ColumnMins;
  fMax := X.ColumnMaxs;

  fFitted := true;
  Result := self;
end;

function MinMaxScaler.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new Exception('MinMaxScaler is not fitted');

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);

  var scale := fRangeMax - fRangeMin;

  for var j := 0 to p - 1 do
  begin
    var minVal := fMin[j];
    var maxVal := fMax[j];
    var denom := maxVal - minVal;

    for var i := 0 to n - 1 do
    begin
      if denom <> 0 then
        Result[i,j] := fRangeMin + (X[i,j] - minVal) / denom * scale
      else Result[i,j] := fRangeMin;  // если столбец константный
    end;
  end;
end;


  
end.