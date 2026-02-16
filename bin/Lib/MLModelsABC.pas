unit MLModelsABC;

interface

uses MLCoreABC;
uses LinearAlgebraML;

type
{$region Activations}
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
{$endregion Activations}
  
{$region Models}
  
  IModel = MLCoreABC.IModel;

/// Линейная регрессионная модель
/// Предсказывает числовое значение по линейной комбинации признаков
/// Используется в задачах регрессии при отсутствии сильной
/// мультиколлинеарности и когда число признаков существенно меньше числа объектов.
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
  /// Используется в задачах бинарной классификации,
  /// когда требуется вероятностный вывод и интерпретируемые коэффициенты.
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
    
/// Линейная регрессионная модель с L2-регуляризацией (Ridge).
/// Минимизирует функцию:
///     ||y - (Xβ + b)||² + λ ||β||².
/// Устойчива к мультиколлинеарности и плохо обусловленным данным.
/// Используется при коррелированных признаках
/// и в задачах, где важна численная стабильность решения.
  RidgeRegression = class(IRegressor)
  private
    fLambda: real;
    fCoef: Vector;
    fIntercept: real;
    fFitted: boolean;
  public
    /// Создаёт модель Ridge-регрессии.
    /// lambda — коэффициент L2-регуляризации (0 — обычная линейная регрессия).
    constructor Create(lambda: real := 1.0);
  
    /// Обучает модель на числовых данных.
    /// X — матрица m × n (m объектов, n признаков).
    /// y — вектор длины m с непрерывными значениями.
    /// Выполняется центрирование признаков и целевой переменной.
    function Fit(X: Matrix; y: Vector): IModel;
  
    /// Предсказывает непрерывные значения для объектов X.
    /// Результат — вектор длины m.
    function Predict(X: Matrix): Vector;
  
    /// Вектор коэффициентов модели (веса признаков).
    /// Длина равна числу признаков.
    /// Доступен после обучения (Fit).
    property Coefficients: Vector read fCoef;
  
    /// Свободный член модели (смещение, bias).
    /// Не подвергается регуляризации.
    property Intercept: real read fIntercept;
  
    /// Коэффициент L2-регуляризации.
    property Lambda: real read fLambda;
  
    /// Показывает, была ли модель обучена.
    /// После вызова Fit значение становится true.
    property IsFitted: boolean read fFitted;
  end;
  
/// Линейная регрессионная модель ElasticNet.
/// Минимизирует функцию:
///     ||y - (Xβ + b)||² + λ1 ||β||₁ + λ2 ||β||².
/// Объединяет L1-регуляризацию (разреженность, отбор признаков)
/// и L2-регуляризацию (численная устойчивость).
/// Используется при большом числе признаков, особенно если признаки коррелированы.
/// Обучение выполняется методом покоординатного спуска 
  ElasticNet = class(IRegressor)
  private
    fLambda1: real;   // L1
    fLambda2: real;   // L2
    fMaxIter: integer;
    fTol: real;

    fCoef: Vector;
    fIntercept: real;
    fFitted: boolean;
    /// Применяет оператор мягкого порога:
    /// soft(z, γ) = sign(z) * max(|z| - γ, 0).
    /// Используется для реализации L1-регуляризации.
    function SoftThreshold(z, gamma: real): real;
  public
    /// Создаёт модель ElasticNet.
    /// lambda1 — коэффициент L1-регуляризации (>= 0).
    /// lambda2 — коэффициент L2-регуляризации (>= 0).
    /// maxIter — максимальное число итераций coordinate descent.
    /// tol — критерий остановки по изменению коэффициентов.
    constructor Create(lambda1, lambda2: real; maxIter: integer := 1000; tol: real := 1e-6);
  
    /// Обучает модель на числовых данных.
    /// X — матрица m × n (m объектов, n признаков).
    /// y — вектор длины m с непрерывными значениями.
    /// Выполняется центрирование признаков и целевой переменной.
    function Fit(X: Matrix; y: Vector): IModel;
  
    /// Предсказывает непрерывные значения для объектов X.
    /// Результат — вектор длины m.
    function Predict(X: Matrix): Vector;
  
    /// Вектор коэффициентов модели (веса признаков).
    /// Длина равна числу признаков.
    /// Доступен после обучения (Fit).
    property Coefficients: Vector read fCoef;
  
    /// Свободный член модели (смещение, bias).
    /// Не подвергается регуляризации.
    property Intercept: real read fIntercept;
  
    /// Показывает, была ли модель обучена.
    /// После вызова Fit значение становится true.
    property IsFitted: boolean read fFitted;
    end;
    
/// Многоклассовая логистическая регрессия (Softmax).
/// Предсказывает вероятности принадлежности к каждому классу
/// на основе линейной комбинации признаков и softmax-функции.
/// Использует кросс-энтропийную функцию потерь.
/// Поддерживает L2-регуляризацию.
  MulticlassLogisticRegression = class(IProbabilisticClassifier)
  private
    fW: Matrix;      // p x k
    fIntercept: Vector; // k
    fLambda: real;
    fLearningRate: real;
    fEpochs: integer;
    fFitted: boolean;
    fClassCount: integer;
  public
    /// lambda — коэффициент L2-регуляризации.
    /// lr — шаг градиентного спуска.
    /// epochs — число итераций обучения.
    constructor Create(lambda: real := 0.0; lr: real := 0.1; epochs: integer := 1000);
  
    function Fit(X: Matrix; y: Vector): IModel;
  
    function PredictProba(X: Matrix): Vector;

    /// Возвращает матрицу вероятностей (m x k).
    function PredictProbaMatrix(X: Matrix): Matrix;
  
    /// Возвращает вектор предсказанных классов.
    function Predict(X: Matrix): Vector;
  
    property IsFitted: boolean read fFitted;
  end;


{$endregion Models}

{$region Pipeline}
/// Последовательный конвейер машинного обучения.
/// Гарантирует строгий порядок выполнения шагов:
///     [преобразователи] → [модель].
/// 
/// Поддерживает:
/// - преобразователи без учёта целевой переменной;
/// - преобразователи с учётом целевой переменной;
/// - одну финальную модель.
///
/// Обеспечивает единый интерфейс Fit / Predict
/// и воспроизводимость полного процесса обучения.
  Pipeline = class(IModel)
  private
    fTransformers: List<ITransformer>;
    fModel: IModel;
    fFitted: boolean;
  public
    /// Создаёт конвейер машинного обучения для заданной модели.
    /// model — модель, которая будет обучена
    /// после последовательного применения всех преобразователей.
    constructor Create(model: IModel);
    
    /// Создаёт пустой пайплайн (конвейер машинного обучения).
    /// Модель должна быть установлена через SetModel.
    constructor Create;

    /// Строит конвейер машинного обучения.
    ///   model — модель, обучаемая после применения всех преобразователей.
    ///   transformers — последовательность преобразователей признаков,
    /// применяемых к данным перед обучением модели.
    /// Возвращает сконструированный конвейер.
    static function Build(model: IModel; params transformers: array of ITransformer): Pipeline;
    
    /// Устанавливает или заменяет модель.
    function SetModel(m: IModel): Pipeline;
  
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
{$endregion Pipeline}
  
{$region Transformers}
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
  
  /// Трансформер главных компонент (PCA).
  /// Выполняет уменьшение размерности путём проекции данных
  /// на первые k главных компонент.
  /// На этапе Fit вычисляет главные компоненты ковариационной матрицы.
  /// На этапе Transform проецирует данные:
  ///     Z = (X - μ) · W
  PCATransformer = class(ITransformer)
  private
    fK: integer;
    fComponents: Matrix;   // W
    fMean: Vector;         // μ
    fFitted: boolean;
  public
    /// Создаёт PCA-трансформер.
    /// k — число главных компонент (k > 0).
    constructor Create(k: integer);
  
    /// Обучает трансформер на матрице признаков X.
    /// X — матрица m × n.
    function Fit(X: Matrix): ITransformer;
  
    /// Преобразует матрицу X в пространство главных компонент.
    /// Возвращает матрицу m × k.
    function Transform(X: Matrix): Matrix;
  
    property Components: Matrix read fComponents;
    property Mean: Vector read fMean;
    property IsFitted: boolean read fFitted;
  end;
  
/// Трансформер, удаляющий признаки с малой дисперсией.
/// Удаляет столбцы X_j, для которых Var(X_j) < threshold.
/// Не использует целевую переменную (unsupervised).
  VarianceThreshold = class(ITransformer)
  private
    fThreshold: real;
    fSelected: array of integer;
    fFitted: boolean;
  
  public
    /// threshold — минимальная допустимая дисперсия (>= 0).
    constructor Create(threshold: real := 0.0);
  
    /// Вычисляет дисперсии признаков и запоминает индексы
    /// признаков, удовлетворяющих порогу.
    function Fit(X: Matrix): ITransformer;
  
    /// Возвращает матрицу, содержащую только отобранные признаки.
    function Transform(X: Matrix): Matrix;
  
    /// Индексы выбранных признаков.
    property SelectedFeatures: array of integer read fSelected;
  
    /// Показывает, был ли выполнен Fit.
    property IsFitted: boolean read fFitted;
  end;
  
  /// Тип критерия оценки признаков для SelectKBest.
  /// Определяет способ вычисления значимости признака
  /// относительно целевой переменной.
  FeatureScore = (
    /// Абсолютное значение коэффициента корреляции Пирсона
    /// между признаком и целевой переменной.
    /// Подходит для задач регрессии и бинарной классификации при линейной зависимости.
    Correlation,
    
    /// F-статистика линейной регрессии (FRegression).
    /// Оценивает статистическую значимость линейной связи
    /// между признаком и целевой переменной.
    /// Основан на коэффициенте детерминации (R²) и F-статистике.
    /// Более строгий критерий, чем простая корреляция.
    FRegression,
    
    /// ANOVA F-критерий.
    /// Используется в задачах классификации.
    /// Оценивает различие средних значений признака между различными классами.
    AnovaF,
    
    /// Хи-квадрат (Chi-Square) критерий независимости.
    /// Используется в задачах классификации.
    /// Оценивает зависимость между признаком и классом
    /// на основе различия наблюдаемых и ожидаемых частот.
    /// Предполагает, что значения признака неотрицательны.
    /// Часто применяется для текстовых данных и частотных представлений (bag-of-words).
    ChiSquare
  );
  
/// Преобразователь с учётом целевой переменной
/// Для каждого признака вычисляет score(X_j, y)
/// и оставляет k признаков с наибольшим значением score.
/// Может использовать встроенные критерии
/// или пользовательскую функцию оценки.
  SelectKBest = class(ITransformer)
  private
    fK: integer;
    fScoreType: FeatureScore;
    fScoreFunc: (Vector, Vector) -> real;
    fSelected: array of integer;
    fFitted: boolean;
    
    function ComputeScore(feature: Vector; y: Vector): real;
    function ComputeCorrelation(x: Vector; y: Vector): real;
    function ComputeFRegression(feature: Vector; y: Vector): real;
    function ComputeAnovaF(feature: Vector; y: Vector): real;
    function ComputeChiSquare(feature: Vector; y: Vector): real;
  public
    /// Создаёт трансформер с использованием встроенного критерия.
    /// k — число отбираемых признаков.
    /// score — тип критерия (например, Correlation).
    constructor Create(k: integer; score: FeatureScore := FeatureScore.Correlation);
  
    /// Создаёт трансформер с пользовательской функцией оценки.
    /// scoreFunc — функция (feature, y) → real.
    constructor Create(k: integer; scoreFunc: (Vector, Vector) -> real);

    /// Выбрасывает исключение
    /// Данный преобразователь требует целевую переменную.
    /// Используйте перегруженный метод Fit(X, y).
    function Fit(X: Matrix): ITransformer;
  
    /// Вычисляет оценки признаков и выбирает k лучших.
    function Fit(X: Matrix; y: Vector): ITransformer;
  
    /// Возвращает матрицу, содержащую только выбранные признаки.
    function Transform(X: Matrix): Matrix;
  
    /// Индексы выбранных признаков.
    property SelectedFeatures: array of integer read fSelected;
  
    /// Показывает, был ли выполнен Fit.
    property IsFitted: boolean read fFitted;
  end;
  
  /// Тип нормы для нормализации строк.
  NormType = (L1, L2);
  
  /// Преобразователь нормализации по строкам.
  /// Для каждой строки X_i выполняет нормализацию:
  ///   L1:  x := x / ||x||₁
  ///   L2:  x := x / ||x||₂
  /// Используется перед моделями, чувствительными к масштабу
  /// (LogisticRegression, SVM, L1-регуляризация).
  Normalizer = class(ITransformer)
  private
    fNormType: NormType;
    fFitted: boolean;
  public
    constructor Create(norm: NormType := NormType.L2);
  
    function Fit(X: Matrix): ITransformer;
    function Transform(X: Matrix): Matrix;
  
    property IsFitted: boolean read fFitted;
  end;
  

{$endregion Transformers}
  
  
implementation  

uses System;

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
    XtX := XtX + Matrix.Identity(p) * flambda;

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
//          RidgeRegression 
//-----------------------------

constructor RidgeRegression.Create(lambda: real);
begin
  if lambda < 0 then
    raise new ArgumentException('lambda must be >= 0');
  fLambda := lambda;
  fFitted := false;
end;

function RidgeRegression.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.RowCount <> y.Length then
    raise new ArgumentException('X and y size mismatch');

  var n := X.RowCount;
  var p := X.ColCount;

  // Means
  var muX := X.ColumnMeans;
  var muY := y.Mean;

  // Centered copies
  var Xc := X.Clone;
  var yc := y.Clone;

  for var j := 0 to p - 1 do
    for var i := 0 to n - 1 do
      Xc[i, j] -= muX[j];

  for var i := 0 to n - 1 do
    yc[i] -= muY;

  // Ridge solution
  fCoef := SolveRidge(Xc, yc, fLambda);

  // Intercept (NOT regularized)
  fIntercept := muY - muX.Dot(fCoef);

  fFitted := true;
  Result := Self;
end;

function RidgeRegression.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    raise new InvalidOperationException('Model is not fitted');

  Result := X * fCoef;
  for var i := 0 to Result.Length - 1 do
    Result[i] += fIntercept;
end;

constructor ElasticNet.Create(lambda1, lambda2: real; maxIter: integer; tol: real);
begin
  if (lambda1 < 0) or (lambda2 < 0) then
    raise new ArgumentException('lambda must be >= 0');

  fLambda1 := lambda1;
  fLambda2 := lambda2;
  fMaxIter := maxIter;
  fTol := tol;
  fFitted := false;
end;

function ElasticNet.SoftThreshold(z, gamma: real): real;
begin
  if z > gamma then
    exit(z - gamma)
  else if z < -gamma then
    exit(z + gamma)
  else
    exit(0.0);
end;

function ElasticNet.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.RowCount <> y.Length then
    raise new ArgumentException('X and y size mismatch');

  // Loss(β) = ||y - (Xβ + b)||² + λ1||β||1 + λ2||β||2²

  var n := X.RowCount;
  var p := X.ColCount;

  var muX := X.ColumnMeans;
  var muY := y.Mean;

  var Xc := X.Clone;
  var yc := y.Clone;

  for var j := 0 to p - 1 do
    for var i := 0 to n - 1 do
      Xc[i,j] -= muX[j];

  for var i := 0 to n - 1 do
    yc[i] -= muY;

  fCoef := new Vector(p);
  var residual := yc.Clone;   // initial residual = yc (since β=0)

  for var iter := 0 to fMaxIter - 1 do
  begin
    var maxChange := 0.0;

    for var j := 0 to p - 1 do
    begin
      var oldBeta := fCoef[j];

      var rho := 0.0;
      var zj := 0.0;

      for var i := 0 to n - 1 do
      begin
        rho += Xc[i,j] * (residual[i] + Xc[i,j] * oldBeta);
        zj += Xc[i,j] * Xc[i,j];
      end;

      var newBeta := SoftThreshold(rho, fLambda1) / (zj + fLambda2);
      var delta := newBeta - oldBeta;

      if Abs(delta) > 0 then
        for var i := 0 to n - 1 do
          residual[i] -= Xc[i,j] * delta;

      if Abs(delta) > maxChange then
        maxChange := Abs(delta);

      fCoef[j] := newBeta;
    end;

    if maxChange < fTol then
      break;
  end;

  fIntercept := muY - muX.Dot(fCoef);
  fFitted := true;

  Result := Self;
end;

function ElasticNet.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    raise new InvalidOperationException('Model is not fitted');

  Result := X * fCoef;
  for var i := 0 to Result.Length - 1 do
    Result[i] += fIntercept;
end;

constructor MulticlassLogisticRegression.Create(lambda: real; lr: real; epochs: integer);
begin
  fLambda := lambda;
  fLearningRate := lr;
  fEpochs := epochs;
  fFitted := false;
end;

function MulticlassLogisticRegression.Fit(X: Matrix; y: Vector): IModel;
begin
  var m := X.RowCount;
  var p := X.ColCount;

  // число классов
  fClassCount := Round(y.Max) + 1;

  // инициализация параметров
  fW := new Matrix(p, fClassCount);
  fIntercept := new Vector(fClassCount);

  // one-hot матрица
  var YoneHot := new Matrix(m, fClassCount);
  for var i := 0 to m - 1 do
    YoneHot[i, Round(y[i])] := 1.0;

  for var epoch := 1 to fEpochs do
  begin
    // Z = XW + b
    var Z := X * fW;

    for var i := 0 to m - 1 do
      for var k := 0 to fClassCount - 1 do
        Z[i,k] += fIntercept[k];

    // softmax
    for var i := 0 to m - 1 do
    begin
      var maxVal := Z.RowMax(i);

      var sumExp := 0.0;

      for var k := 0 to fClassCount - 1 do
      begin
        Z[i,k] := Exp(Z[i,k] - maxVal);
        sumExp += Z[i,k];
      end;

      for var k := 0 to fClassCount - 1 do
        Z[i,k] /= sumExp;
    end;

    var PP := Z;

    // градиенты
    var diff := PP - YoneHot;                // m x k
    var gradW := X.Transpose * diff;  // p x k

    // L2 регуляризация (только веса)
    if fLambda <> 0 then
      gradW += fLambda * fW;

    // обновление
    fW -= fLearningRate * gradW;

    // градиент по intercept
    var gradB := diff.ColumnSums;
    fIntercept -= fLearningRate * gradB;
  end;

  fFitted := true;
  Result := Self;
end;

function MulticlassLogisticRegression.PredictProba(X: Matrix): Vector;
begin
  var P := PredictProbaMatrix(X);

  var m := P.RowCount;
  Result := new Vector(m);

  for var i := 0 to m - 1 do
  begin
    var k := P.RowArgMax(i);
    Result[i] := P[i, k];
  end;
end;


function MulticlassLogisticRegression.PredictProbaMatrix(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new Exception('Model not fitted');

  var m := X.RowCount;

  var Z := X * fW;

  for var i := 0 to m - 1 do
    for var k := 0 to fClassCount - 1 do
      Z[i,k] += fIntercept[k];

  for var i := 0 to m - 1 do
  begin
    var maxVal := Z.RowMax(i);

    var sumExp := 0.0;

    for var k := 0 to fClassCount - 1 do
    begin
      Z[i,k] := Exp(Z[i,k] - maxVal);
      sumExp += Z[i,k];
    end;

    for var k := 0 to fClassCount - 1 do
      Z[i,k] /= sumExp;
  end;

  Result := Z;
end;

function MulticlassLogisticRegression.Predict(X: Matrix): Vector;
begin
  var P := PredictProbaMatrix(X);

  var m := P.RowCount;
  Result := new Vector(m);

  for var i := 0 to m - 1 do
    Result[i] := P.RowArgMax(i);
end;

//-----------------------------
//          Pipeline 
//-----------------------------


constructor Pipeline.Create;
begin
  fTransformers := new List<ITransformer>;
  fModel := nil;
  fFitted := false;
end;

constructor Pipeline.Create(model: IModel);
begin
  Create;
  if model = nil then
    raise new ArgumentException('Model cannot be nil');
  fModel := model;
end;

static function Pipeline.Build(model: IModel; transformers: array of ITransformer): Pipeline;
begin
  Result := new Pipeline(model);

  foreach var t in transformers do
    Result.Add(t);
end;

function Pipeline.Add(t: ITransformer): Pipeline;
begin
  if t = nil then
    raise new ArgumentException('Transformer cannot be nil');

  fTransformers.Add(t);
  Result := Self;
end;

function Pipeline.SetModel(m: IModel): Pipeline;
begin
  if m = nil then
    raise new ArgumentException('Model cannot be nil');

  fModel := m;
  Result := Self;
end;

function Pipeline.Fit(X: Matrix; y: Vector): IModel;
begin
  if fModel = nil then
    raise new Exception('Model is not set');

  var Xt := X;

  foreach var t in fTransformers do
  begin
    if t is ISupervisedTransformer (var sup) then
      sup.Fit(Xt, y)
    else t.Fit(Xt);

    Xt := t.Transform(Xt);
  end;

  fModel.Fit(Xt, y);

  fFitted := true;
  Result := Self;
end;

function Pipeline.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new Exception('Pipeline not fitted');

  var Xt := X;

  foreach var t in fTransformers do
    Xt := t.Transform(Xt);

  Result := Xt;
end;

function Pipeline.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    raise new Exception('Pipeline not fitted');

  var Xt := Transform(X);
  Result := fModel.Predict(Xt);
end;

function Pipeline.PredictProba(X: Matrix): Vector;
begin
  if not fFitted then
    raise new Exception('Pipeline not fitted');

  if not (fModel is IProbabilisticClassifier) then
    raise new Exception('Model does not support probability prediction');

  var Xt := Transform(X);

  Result := (fModel as IProbabilisticClassifier).PredictProba(Xt);
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

//-----------------------------
//        PCATransformer
//-----------------------------

constructor PCATransformer.Create(k: integer);
begin
  if k <= 0 then
    raise new ArgumentException('k must be > 0');

  fK := k;
  fFitted := false;
end;

function PCATransformer.Fit(X: Matrix): ITransformer;
begin
  if fK > X.ColCount then
    raise new ArgumentException('k exceeds feature count');

  fMean := X.ColumnMeans;

  var Xc := X.Clone;

  for var j := 0 to X.ColCount - 1 do
    for var i := 0 to X.RowCount - 1 do
      Xc[i,j] -= fMean[j];

  var (W, xxx) := Xc.PCA(fK);

  fComponents := W;
  fFitted := true;

  Result := Self;
end;

function PCATransformer.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new InvalidOperationException('PCA not fitted');

  var Xc := X.Clone;

  for var j := 0 to X.ColCount - 1 do
    for var i := 0 to X.RowCount - 1 do
      Xc[i,j] -= fMean[j];

  Result := Xc * fComponents;
end;

//-----------------------------
//      VarianceThreshold
//-----------------------------

constructor VarianceThreshold.Create(threshold: real);
begin
  if threshold < 0 then
    raise new ArgumentException('threshold must be >= 0');

  fThreshold := threshold;
  fFitted := false;
end;

function VarianceThreshold.Fit(X: Matrix): ITransformer;
begin
  var vars := X.ColumnVariances;

  var tmp := new List<integer>;

  for var j := 0 to X.ColCount - 1 do
    if vars[j] >= fThreshold then
      tmp.Add(j);

  fSelected := tmp.ToArray;
  fFitted := true;

  Result := Self;
end;

function VarianceThreshold.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new InvalidOperationException('VarianceThreshold not fitted');

  var n := X.RowCount;
  var k := fSelected.Length;

  var R := new Matrix(n, k);

  for var i := 0 to n - 1 do
    for var c := 0 to k - 1 do
      R[i,c] := X[i, fSelected[c]];

  Result := R;
end;

//-----------------------------
//         SelectKBest 
//-----------------------------

constructor SelectKBest.Create(k: integer; score: FeatureScore);
begin
  if k <= 0 then
    raise new ArgumentException('k must be > 0');

  fK := k;
  fScoreType := score;
  fScoreFunc := nil;
  fFitted := false;
end;

constructor SelectKBest.Create(k: integer; scoreFunc: (Vector, Vector) -> real);
begin
  if k <= 0 then
    raise new ArgumentException('k must be > 0');

  fK := k;
  fScoreFunc := scoreFunc;
  fFitted := false;
end;

function SelectKBest.ComputeCorrelation(x: Vector; y: Vector): real;
begin
  var mx := x.Mean;
  var my := y.Mean;

  var num := 0.0;
  var dx := 0.0;
  var dy := 0.0;

  for var i := 0 to x.Length - 1 do
  begin
    var vx := x[i] - mx;
    var vy := y[i] - my;

    num += vx * vy;
    dx += vx * vx;
    dy += vy * vy;
  end;

  if (dx = 0) or (dy = 0) then
    exit(0.0);

  Result := Abs(num / Sqrt(dx * dy));
end;

function SelectKBest.ComputeFRegression(feature: Vector; y: Vector): real;
begin
  var r := ComputeCorrelation(feature, y);
  var n := feature.Length;

  if Abs(r) >= 1 then
    exit(1e308);

  Result := (r*r / (1 - r*r)) * (n - 2);
end;

function SelectKBest.ComputeAnovaF(feature: Vector; y: Vector): real;
begin
  var mean0 := 0.0;
  var mean1 := 0.0;
  var n0 := 0;
  var n1 := 0;

  for var i := 0 to y.Length - 1 do
    if y[i] = 0 then
    begin
      mean0 += feature[i];
      n0 += 1;
    end
    else
    begin
      mean1 += feature[i];
      n1 += 1;
    end;

  if (n0 = 0) or (n1 = 0) then
    exit(0.0);

  mean0 /= n0;
  mean1 /= n1;

  var grandMean := feature.Mean;

  var ssBetween :=
    n0 * Sqr(mean0 - grandMean) +
    n1 * Sqr(mean1 - grandMean);

  var ssWithin := 0.0;

  for var i := 0 to y.Length - 1 do
    if y[i] = 0 then
      ssWithin += Sqr(feature[i] - mean0)
    else
      ssWithin += Sqr(feature[i] - mean1);

  if ssWithin = 0 then
    exit(1e308);

  Result := (ssBetween) / (ssWithin / (y.Length - 2));
end;

function SelectKBest.ComputeChiSquare(feature: Vector; y: Vector): real;
begin
  var n := y.Length;

  var sum0 := 0.0;
  var sum1 := 0.0;
  var total := 0.0;
  var n0 := 0;
  var n1 := 0;

  for var i := 0 to n - 1 do
  begin
    var v := feature[i];
    if v < 0 then
      raise new ArgumentException('ChiSquare requires non-negative features');

    total += v;

    if y[i] = 0 then
    begin
      sum0 += v;
      n0 += 1;
    end
    else
    begin
      sum1 += v;
      n1 += 1;
    end;
  end;

  if (n0 = 0) or (n1 = 0) or (total = 0) then
    exit(0.0);

  var expected0 := total * n0 / n;
  var expected1 := total * n1 / n;

  var chi := 0.0;

  if expected0 > 0 then
    chi += Sqr(sum0 - expected0) / expected0;

  if expected1 > 0 then
    chi += Sqr(sum1 - expected1) / expected1;

  Result := chi;
end;

function SelectKBest.ComputeScore(feature: Vector; y: Vector): real;
begin
  case fScoreType of
    FeatureScore.Correlation: Result := ComputeCorrelation(feature, y);
    FeatureScore.FRegression: Result := ComputeFRegression(feature, y);
    FeatureScore.AnovaF: Result := ComputeAnovaF(feature, y);
    FeatureScore.ChiSquare: Result := ComputeChiSquare(feature, y);
  else
    raise new InvalidOperationException('Unknown FeatureScore type');
  end;
end;

function SelectKBest.Fit(X: Matrix; y: Vector): ITransformer;
begin
  var p := X.ColCount;

  var scores: array of (real, integer);
  SetLength(scores, p);

  for var j := 0 to p - 1 do
  begin
    var col := new Vector(X.RowCount);
    for var i := 0 to X.RowCount - 1 do
      col[i] := X[i,j];

    var s :=
      if fScoreFunc <> nil then
        fScoreFunc(col, y)
      else
        ComputeCorrelation(col, y);

    scores[j] := (s, j);
  end;

  scores := scores.OrderByDescending(t -> t.Item1).ToArray;

  var k := Min(fK, p);
  SetLength(fSelected, k);

  for var i := 0 to k - 1 do
    fSelected[i] := scores[i].Item2;

  fFitted := true;
  Result := Self;
end;

function SelectKBest.Fit(X: Matrix): ITransformer;
begin
  Result := nil;
  raise new InvalidOperationException('Target variable is required');
end;

function SelectKBest.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new InvalidOperationException('SelectKBest not fitted');

  var n := X.RowCount;
  var k := fSelected.Length;

  var R := new Matrix(n, k);

  for var i := 0 to n - 1 do
    for var c := 0 to k - 1 do
      R[i,c] := X[i, fSelected[c]];

  Result := R;
end;

//-----------------------------
//         Normalizer 
//-----------------------------

constructor Normalizer.Create(norm: NormType);
begin
  fNormType := norm;
  fFitted := false;
end;

function Normalizer.Fit(X: Matrix): ITransformer;
begin
  // Нормализация не требует обучения
  fFitted := true;
  Result := Self;
end;

function Normalizer.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    raise new InvalidOperationException('Normalizer not fitted');

  var n := X.RowCount;
  var p := X.ColCount;

  var R := new Matrix(n, p);

  for var i := 0 to n - 1 do
  begin
    var norm := 0.0;

    case fNormType of
      NormType.L1:
        for var j := 0 to p - 1 do
          norm += Abs(X[i,j]);

      NormType.L2:
        for var j := 0 to p - 1 do
          norm += Sqr(X[i,j]);
    end;

    if fNormType = NormType.L2 then
      norm := Sqrt(norm);

    if norm = 0 then
      continue;

    for var j := 0 to p - 1 do
      R[i,j] := X[i,j] / norm;
  end;

  Result := R;
end;


  
end.