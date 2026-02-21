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

/// Линейная регрессионная модель (метод наименьших квадратов).
/// Предсказывает числовое значение по линейной комбинации признаков
/// Используется в задачах регрессии при отсутствии выраженной
/// мультиколлинеарности и когда число признаков существенно меньше числа объектов.
  LinearRegression = class(IRegressor)
  private
    fCoef: Vector;
    fIntercept: real;
    fFitted: boolean;
  public
    constructor Create();

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
    
    function ToString: string; override;
    
    function Clone: IModel;
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
    
    function ToString: string; override;
    
    function Clone: IModel;
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
    
    function ToString: string; override;
    
    function Clone: IModel;
  end;
    
/// Логистическая регрессия.
/// Поддерживает бинарную и многоклассовую классификацию.
/// Для нескольких классов используется softmax,
/// для двух классов — частный случай softmax.
/// Оптимизация выполняется по кросс-энтропийной функции потерь
/// с поддержкой L2-регуляризации.
  LogisticRegression = class(IProbabilisticClassifier)
  private
    fW: Matrix;      // p x k
    fIntercept: Vector; // k
    fLambda: real;
    fLearningRate: real;
    fEpochs: integer;
    fFitted: boolean;
    fClassCount: integer;
    fClassToIndex: Dictionary<integer, integer>;
    fIndexToClass: array of integer;
  public
    /// lambda — коэффициент L2-регуляризации.
    /// lr — шаг градиентного спуска.
    /// epochs — число итераций обучения.
    constructor Create(lambda: real := 0.0; lr: real := 0.1; epochs: integer := 1000);
  
    function Fit(X: Matrix; y: Vector): IModel;
  
    /// Возвращает матрицу вероятностей (m x k).
    function PredictProba(X: Matrix): Matrix;
  
    /// Возвращает вектор предсказанных классов.
    function Predict(X: Matrix): Vector;
  
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
  end;
  
  DecisionTreeNode = class
  public
    IsLeaf: boolean;
    FeatureIndex: integer;
    Threshold: real;
    Left: DecisionTreeNode;
    Right: DecisionTreeNode;
    LeafValue: real;
    
    function Clone: DecisionTreeNode;
  end;
  
  SplitResult = record
    Found: boolean;
    Feature: integer;
    Threshold: real;
  end;
  
  ISplitCriterion = interface
    function Impurity(y: Vector): real;
  end;
  
  GiniCriterion = class(ISplitCriterion)
  public
    function Impurity(y: Vector): real;
  end;

  VarianceCriterion = class(ISplitCriterion)
  public
    function Impurity(y: Vector): real;
  end;
  
//============================  
//    DecisionTreeBase
//============================  
  DecisionTreeBase = abstract class(ITreeModel)
  protected
    fRoot: DecisionTreeNode;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fFitted: boolean;
    fCriterion: ISplitCriterion;
    fFeatureImportances: Vector;
    fRandomSeed: integer;
    fMaxFeatures := 0;
    fRowIndices: array of integer := nil;
  
    function BuildTree(X: Matrix; y: Vector; indices: array of integer; depth: integer): DecisionTreeNode;
  
    function FindBestSplit0(X: Matrix; y: Vector; indices: array of integer): SplitResult;
    function FindBestSplitReg(X: Matrix; y: Vector; indices: array of integer): SplitResult;
    function FindBestSplitCls(X: Matrix; y: Vector; indices: array of integer; classCount: integer): SplitResult;
    
    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; virtual; abstract;
    function IsPure(y: Vector; indices: array of integer): boolean; virtual;
// --------------------------    
  
    function LeafValue(y: Vector; indices: array of integer): real; virtual; abstract;
    function LeafNode(value: real): DecisionTreeNode;
    procedure CopyBaseState(dest: DecisionTreeBase);
    function GetFeatureSubset(nFeatures: integer): array of integer; virtual;
    
    procedure SetRowIndices(rows: array of integer);
  public
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1);
  
    function FeatureImportances: Vector;
    
    function Fit(X: Matrix; y: Vector): IModel; virtual; abstract;
    function Predict(X: Matrix): Vector; virtual; abstract;
    function Clone: IModel; virtual; abstract;

    function IsFitted: boolean;
  end;

//============================  
//   DecisionTreeClassifier  
//============================  
  DecisionTreeClassifier = class(DecisionTreeBase, IClassifier)
  private
    fClassToIndex: Dictionary<integer, integer>;
    fIndexToClass: array of integer;
    fClassCount: integer;

    function PredictOne(x: Vector): integer;
    function MajorityClass(y: Vector; indices: array of integer): integer;
    
  protected  
    function LeafValue(y: Vector; indices: array of integer): real; override;
    
    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; override;
    begin
      Result := FindBestSplitCls(X, y, indices, fClassCount);
    end;

  public
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1);

    function Fit(X: Matrix; y: Vector): IModel; override;
    function Predict(X: Matrix): Vector; override;
    function Clone: IModel; override;
  end;
  
//============================  
//   DecisionTreeRegressor  
//============================  
  DecisionTreeRegressor = class(DecisionTreeBase, IRegressor)
  private
    function PredictOne(x: Vector): real;
  
  protected
    function LeafValue(y: Vector; indices: array of integer): real; override;

    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; override;
    function IsPure(y: Vector; indices: array of integer): boolean; override;
    
  public
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1);
    
    function Fit(X: Matrix; y: Vector): IModel; override;
    function Predict(X: Matrix): Vector; override;
    function Clone: IModel; override;
  end;
  
  TMaxFeaturesMode = (
    /// m = p
    AllFeatures,      
    /// m = sqrt(p)
    SqrtFeatures,   
    /// m = log2(p)
    Log2Features,
    /// m = p/2
    HalfFeatures     
  );
  
//============================  
//   RandomForestRegressor  
//============================  
// RandomForest = bagging + feature-subset
//   bagging = Делаем bootstrap-выборку (случайно с возвращением)
//   feature-subset = в каждом дереве - случайные признаки
//   RandomForest снижает корреляцию между деревьями, что хорошо

  RandomForestBase = abstract class(IModel)
  protected
    fNTrees: integer;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fMaxFeaturesMode: TMaxFeaturesMode;
    fFitted: boolean;
  
    function ComputeMaxFeatures(p: integer): integer;
    procedure BootstrapSample(X: Matrix; y: Vector; var Xb: Matrix; var yb: Vector);
  public
    constructor Create(
      nTrees: integer;
      maxDepth: integer;
      minSamplesSplit: integer;
      minSamplesLeaf: integer;
      maxFeatures: TMaxFeaturesMode);
  
    function Fit(X: Matrix; y: Vector): IModel; virtual; abstract;
    function Predict(X: Matrix): Vector; virtual; abstract;
    function Clone: IModel; virtual; abstract;
    
    function FeatureImportances: Vector; virtual; abstract;
  end;  
  
  RandomForestRegressor = class(RandomForestBase, IModel)
  private
    fTrees: array of DecisionTreeRegressor;
  public
    constructor Create(nTrees: integer := 100; 
      maxDepth: integer := integer.MaxValue;
      minSamplesSplit: integer := 2; 
      minSamplesLeaf: integer := 1;
      maxFeaturesMode: TMaxFeaturesMode := TMaxFeaturesMode.HalfFeatures);

    function Fit(X: Matrix; y: Vector): IModel; override;
    function Predict(X: Matrix): Vector; override;
    function Clone: IModel; override;
    
    function FeatureImportances: Vector; override;
  end;

  RandomForestClassifier = class(RandomForestBase, IClassifier)
  private
    fTrees: array of DecisionTreeClassifier;
  public
    constructor Create(nTrees: integer := 100; 
      maxDepth: integer := integer.MaxValue;
      minSamplesSplit: integer := 2;
      minSamplesLeaf: integer := 1;
      maxFeaturesMode: TMaxFeaturesMode := TMaxFeaturesMode.SqrtFeatures);
  
    function Fit(X: Matrix; y: Vector): IModel; override;
    function Predict(X: Matrix): Vector; override;
    function Clone: IModel; override;
    
    function FeatureImportances: Vector; override;
  end;
  
  TGBLoss = (SquaredError, Huber);
  
  GradientBoostingRegressor = class(IRegressor)
  private
    fNEstimators: integer;
    fLearningRate: real;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fSubsample: real;
    fRandomSeed: integer;

    fEstimators: List<DecisionTreeRegressor>;
    fInitValue: real;
    fFitted: boolean;
    fFeatureCount: integer;
// -----------  
    fLoss: TGBLoss;
    fHuberDelta: real;
  
    fEarlyStoppingPatience: integer;
  
    fTrainLossHistory: List<real>;
    fBestTrainLoss: real;
    fBestIteration: integer;
    
    fValLossHistory: List<real>;
    fBestValLoss: real;
    
    function ComputeTrainLoss(y, yPred: Vector): real;
    procedure ComputePseudoResiduals(y, yPred: Vector; r: Vector);
    
    function FitInternal(XTrain: Matrix; yTrain: Vector;
      XVal: Matrix; yVal: Vector; useValidation: boolean): IModel;
    
  public
    constructor Create(
      nEstimators: integer := 100;
      learningRate: real := 0.1;
      maxDepth: integer := 3;
      minSamplesSplit: integer := 2;
      minSamplesLeaf: integer := 1;
      subsample: real := 1.0;
      randomSeed: integer := 42;
      loss: TGBLoss := TGBLoss.SquaredError;
      huberDelta: real := 1.0;
      earlyStoppingPatience: integer := 0      
      );

    function Fit(X: Matrix; y: Vector): IModel;
    function Predict(X: Matrix): Vector;
    function Clone: IModel;
    
    function FitWithValidation(XTrain: Matrix; yTrain: Vector;
      XVal: Matrix; yVal: Vector): IModel;
    
    property TrainLossHistory: List<real> read fTrainLossHistory;
    property ValLossHistory: List<real> read fValLossHistory;
    property BestIteration: integer read fBestIteration;
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

    /// Строит конвейер машинного обучения из последовательности шагов.
    /// Шаги указываются в порядке выполнения:
    ///   сначала преобразователи, затем модель.
    /// Последний шаг обязан быть моделью (IModel).
    /// Возвращает сконструированный конвейер.
    static function Build(params steps: array of IPipeStep): Pipeline;
    
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
  
    /// Возвращает матрицу вероятностей (m x k)
    /// для вероятностной модели в конце пайплайна.
    function PredictProba(X: Matrix): Matrix;
  
    /// Показывает, был ли пайплайн обучен (вызван метод Fit).
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
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

    function ToString: string; override;
    
    function Clone: ITransformer;
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

    function ToString: string; override;
    
    function Clone: ITransformer;
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

    function ToString: string; override;
    function Clone: ITransformer;    
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

    function ToString: string; override;
    function Clone: ITransformer;
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
  SelectKBest = class(ISupervisedTransformer)
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

    function ToString: string; override;
    function Clone: ITransformer;
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

    function ToString: string; override;
    function Clone: ITransformer;
  end;
  

{$endregion Transformers}
  
  
implementation  

uses MLExceptions;

{$region ErrConstants}
const
  ER_PIPELINE_NO_STEPS =
    'Pipeline должен содержать хотя бы один шаг!!Pipeline requires at least one step';
  ER_PIPELINE_LAST_NOT_MODEL =
    'Последний шаг Pipeline должен быть моделью (IModel)!!Last step must be a model (IModel)';
  ER_PIPELINE_INVALID_STEP_ORDER =
    'Все шаги, кроме последнего, должны быть трансформерами!!All steps except the last must be transformers';
  ER_TRANSFORMER_NULL =
    'Трансформер не может быть nil!!Transformer cannot be nil';
  ER_PROBA_NOT_SUPPORTED =
    'Модель не поддерживает предсказание вероятностей!!Model does not support probability prediction';
  ER_RANGE_INVALID =
    'rangeMax должен быть больше rangeMin!!rangeMax must be greater than rangeMin';
  ER_K_MUST_BE_POSITIVE =
    'Параметр k должен быть > 0!!Parameter k must be > 0';
  ER_K_EXCEEDS_FEATURES =
    'k превышает число признаков!!k exceeds feature count';  
  ER_THRESHOLD_NEGATIVE =
    'Порог threshold должен быть >= 0!!threshold must be >= 0';  
  ER_CHI_SQUARE_NEGATIVE =
    'ChiSquare требует неотрицательные признаки!!ChiSquare requires non-negative features';
  ER_UNKNOWN_FEATURE_SCORE =
    'Неизвестный тип FeatureScore!!Unknown FeatureScore type';
  ER_SELECTKBEST_FIT_INVALID =
    'Для SelectKBest необходимо вызывать Fit(X, y)!!SelectKBest requires Fit(X, y)';
  ER_FIT_NOT_CALLED =
    'Необходимо вызвать Fit перед Predict!!Fit must be called before Predict';
  ER_X_NULL =
    'X не может быть nil!!X cannot be nil';
  ER_Y_NULL =
    'y не может быть nil!!y cannot be nil';
  ER_XY_SIZE_MISMATCH =
    'Размерности X и y не согласованы!!X and y size mismatch';
  ER_FEATURE_COUNT_MISMATCH =
    'Число признаков не совпадает!!Feature count mismatch';
  ER_N_ESTIMATORS_NOT_POSITIVE =
    'Параметр nEstimators должен быть > 0!!nEstimators must be > 0';
  ER_LEARNING_RATE_NOT_POSITIVE =
    'Параметр learningRate должен быть > 0!!learningRate must be > 0';
  ER_SUBSAMPLE_OUT_OF_RANGE =
    'Параметр subsample должен быть в диапазоне (0, 1]!!subsample must be in (0, 1]';  
    
{$endregion ErrConstants}  
  
//-----------------------------
//       LinearRegression
//-----------------------------

constructor LinearRegression.Create();
begin
  ffitted := false;
end;

function LinearRegression.Fit(X: Matrix; y: Vector): IModel;
begin
  var n := X.RowCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATASET);
  if y.Length <> n then
    DimensionError(ER_DIM_MISMATCH, y.Length, n);

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
 
  // 4. Solve
  fcoef := Solve(XtX, XtY);

  // 5. Intercept
  fintercept := meanY - meanX.Dot(fcoef);

  ffitted := true;
  Result := Self;
end;

function LinearRegression.Predict(X: Matrix): Vector;
begin
  if not ffitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X.ColCount <> fcoef.Length then
    DimensionError(ER_DIM_MISMATCH, X.ColCount, fCoef.Length);

  Result := X * fcoef + intercept;
end;

function LinearRegression.ToString: string;
begin
  Result := 'LinearRegression'
end;

function LinearRegression.Clone: IModel;
begin
  var m := new LinearRegression;

  m.fFitted := fFitted;

  if fCoef <> nil then
    m.fCoef := fCoef.Clone;

  m.fIntercept := fIntercept;

  Result := m;
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

(*constructor LogisticRegression.Create(lambda: real; lr: real; epochs: integer);
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
    DimensionError(ER_DIM_MISMATCH, y.Length, n);

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
    NotFittedError(ER_FIT_NOT_CALLED);

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

function LogisticRegression.ToString: string;
begin
  Result := 'LogisticRegression(lambda=' + fLambda +
    ', lr=' + fLearningRate + ', epochs=' + fEpochs + ')';
end;

function LogisticRegression.Clone: IModel;
begin
  Result := new LogisticRegression(fLambda, fLearningRate, fEpochs);
end;*)


//-----------------------------
//          RidgeRegression 
//-----------------------------

constructor RidgeRegression.Create(lambda: real);
begin
  if lambda < 0 then
    ArgumentError(ER_LAMBDA_NEGATIVE, lambda);
  fLambda := lambda;
  fFitted := false;
end;

function RidgeRegression.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

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
    NotFittedError(ER_FIT_NOT_CALLED);

  Result := X * fCoef;
  for var i := 0 to Result.Length - 1 do
    Result[i] += fIntercept;
end;

function RidgeRegression.ToString: string;
begin
  Result := 'RidgeRegression(lambda=' + fLambda + ')';
end;

function RidgeRegression.Clone: IModel;
begin
  var m := new RidgeRegression(fLambda);

  m.fFitted := fFitted;

  if fCoef <> nil then
    m.fCoef := fCoef.Clone;

  m.fIntercept := fIntercept;

  Result := m;
end;


//-----------------------------
//          ElasticNet 
//-----------------------------

constructor ElasticNet.Create(lambda1, lambda2: real; maxIter: integer; tol: real);
begin
  if (lambda1 < 0) or (lambda2 < 0) then
    ArgumentError(ER_LAMBDA_NEGATIVE);

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
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

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
    NotFittedError(ER_FIT_NOT_CALLED);

  Result := X * fCoef;
  for var i := 0 to Result.Length - 1 do
    Result[i] += fIntercept;
end;

function ElasticNet.ToString: string;
begin
  Result :=
    'ElasticNet(lambda1=' + fLambda1 +
    ', lambda2=' + fLambda2 +
    ', maxIter=' + fMaxIter +
    ', tol=' + fTol + ')';
end;

function ElasticNet.Clone: IModel;
begin
  var m := new ElasticNet(fLambda1, fLambda2, fMaxIter, fTol);

  m.fFitted := fFitted;

  if fCoef <> nil then
    m.fCoef := fCoef.Clone;

  m.fIntercept := fIntercept;

  Result := m;
end;

//-----------------------------
// MulticlassLogisticRegression 
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
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  var m := X.RowCount;
  var p := X.ColCount;

  // --- internal encoding
  var unique := y.data.Select(v -> integer(v)).Distinct.ToArray;

  fClassCount := unique.Length;

  fClassToIndex := new Dictionary<integer, integer>;
  SetLength(fIndexToClass, fClassCount);

  for var i := 0 to fClassCount - 1 do
  begin
    fClassToIndex[unique[i]] := i;
    fIndexToClass[i] := unique[i];
  end;

  var yEncoded := new Vector(m);
  for var i := 0 to m - 1 do
    yEncoded[i] := fClassToIndex[integer(y[i])];

  // --- init
  fW := new Matrix(p, fClassCount);
  fIntercept := new Vector(fClassCount);

  // --- one-hot
  var YoneHot := new Matrix(m, fClassCount);
  for var i := 0 to m - 1 do
    YoneHot[i, integer(yEncoded[i])] := 1.0;

  for var epoch := 1 to fEpochs do
  begin
    var Z := X * fW;

    for var i := 0 to m - 1 do
      for var k := 0 to fClassCount - 1 do
        Z[i,k] += fIntercept[k];

    // softmax (оставляем твою версию)
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

    var diff := Z - YoneHot;

    var gradW := X.Transpose * diff;
    gradW *= 1.0 / m;                     // ВАЖНО

    if fLambda <> 0 then
      gradW += fLambda * fW;

    fW -= fLearningRate * gradW;

    var gradB := diff.ColumnSums;
    gradB *= 1.0 / m;                     // ВАЖНО

    fIntercept -= fLearningRate * gradB;
  end;

  fFitted := true;
  Result := Self;
end;

{function LogisticRegression.PredictProba(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var P := PredictProbaMatrix(X);

  var m := P.RowCount;
  Result := new Vector(m);

  for var i := 0 to m - 1 do
  begin
    var k := P.RowArgMax(i);
    Result[i] := P[i, k];
  end;
end;}

function LogisticRegression.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

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

function LogisticRegression.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var P := PredictProba(X);

  var m := P.RowCount;
  Result := new Vector(m);

  for var i := 0 to m - 1 do
  begin
    var internalIdx := P.RowArgMax(i);
    Result[i] := fIndexToClass[internalIdx];
  end;
end;

function LogisticRegression.ToString: string;
begin
  Result :=
    'LogisticRegression(lambda=' + fLambda +
    ', lr=' + fLearningRate +
    ', epochs=' + fEpochs + ')';
end;

function LogisticRegression.Clone: IModel;
begin
  var m := new LogisticRegression(
    fLambda,
    fLearningRate,
    fEpochs
  );

  m.fFitted := fFitted;
  m.fClassCount := fClassCount;

  if fW <> nil then
    m.fW := fW.Clone;

  if fIntercept <> nil then
    m.fIntercept := fIntercept.Clone;

  if fClassToIndex <> nil then
  begin
    m.fClassToIndex := new Dictionary<integer, integer>;
    foreach var kv in fClassToIndex do
      m.fClassToIndex[kv.Key] := kv.Value;
  end;

  if fIndexToClass <> nil then
  begin
    SetLength(m.fIndexToClass, Length(fIndexToClass));
    for var i := 0 to Length(fIndexToClass) - 1 do
      m.fIndexToClass[i] := fIndexToClass[i];
  end;

  Result := m;
end;

function GiniCriterion.Impurity(y: Vector): real;
begin
  if y.Length = 0 then exit(0.0);

  var counts := Dict&<integer,integer>;
  foreach var v in y.data do
  begin
    var c := integer(v);
    if counts.ContainsKey(c) then
      counts[c] += 1
    else
      counts[c] := 1;
  end;

  var n := y.Length;
  var sumsq := 0.0;

  foreach var kv in counts do
  begin
    var p := kv.Value / n;
    sumsq += p * p;
  end;

  Result := 1.0 - sumsq;
end;

function VarianceCriterion.Impurity(y: Vector): real;
begin
  if y.Length = 0 then exit(0.0);

  var mean := y.Mean;
  var s := 0.0;

  foreach var v in y.data do
  begin
    var d := v - mean;
    s += d*d;
  end;

  Result := s / y.Length;
end;

function LeafClass(c: integer): DecisionTreeNode;
begin
  var n := new DecisionTreeNode;
  n.IsLeaf := true;
  n.LeafValue := c;
  Result := n;
end;

function LeafValue(v: real): DecisionTreeNode;
begin
  var n := new DecisionTreeNode;
  n.IsLeaf := true;
  n.LeafValue := v;
  Result := n;
end;

function SplitNode(feature: integer; threshold: real;
                   leftNode, rightNode: DecisionTreeNode): DecisionTreeNode;
begin
  var n := new DecisionTreeNode;
  n.IsLeaf := false;
  n.FeatureIndex := feature;
  n.Threshold := threshold;
  n.Left := leftNode;
  n.Right := rightNode;
  Result := n;
end;

function DecisionTreeNode.Clone: DecisionTreeNode;
begin
  var n := new DecisionTreeNode;

  n.IsLeaf := IsLeaf;
  n.FeatureIndex := FeatureIndex;
  n.Threshold := Threshold;
  n.LeafValue := LeafValue;

  if Left <> nil then
    n.Left := Left.Clone;

  if Right <> nil then
    n.Right := Right.Clone;

  Result := n;
end;

constructor DecisionTreeBase.Create(maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer);
begin
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fRandomSeed := 0;
end;

procedure DecisionTreeBase.CopyBaseState(dest: DecisionTreeBase);
begin
  dest.fMaxDepth := fMaxDepth;
  dest.fMinSamplesSplit := fMinSamplesSplit;
  dest.fMinSamplesLeaf := fMinSamplesLeaf;
  dest.fFitted := fFitted;

  if fRoot <> nil then
    dest.fRoot := fRoot.Clone;
end;

function DecisionTreeBase.GetFeatureSubset(nFeatures: integer): array of integer;
begin
  if (fMaxFeatures = 0) or (fMaxFeatures >= nFeatures) then
  begin
    Result := new integer[nFeatures];
    for var i := 0 to nFeatures-1 do
      Result[i] := i;
    exit;
  end;

  var all := new List<integer>;
  for var i := 0 to nFeatures-1 do
    all.Add(i);

  var subset := new integer[fMaxFeatures];

  for var k := 0 to fMaxFeatures-1 do
  begin
    var idx := Random(all.Count);
    subset[k] := all[idx];
    all.RemoveAt(idx);
  end;

  Result := subset;
end;

procedure DecisionTreeBase.SetRowIndices(rows: array of integer);
begin
  if Length(rows) = 0 then
    ArgumentError('Row subset cannot be empty!!Row subset cannot be empty');

  fRowIndices := Copy(rows);
end;

function DecisionTreeBase.FeatureImportances: Vector;
begin
  Result := fFeatureImportances.Clone;
end;

function DecisionTreeBase.IsFitted: boolean;
begin
  Result := fFitted;
end;

function DecisionTreeBase.LeafNode(value: real): DecisionTreeNode;
begin
  var n := new DecisionTreeNode;
  n.IsLeaf := true;
  n.LeafValue := value;
  Result := n;
end;

function DecisionTreeBase.BuildTree(X: Matrix; y: Vector;
  indices: array of integer; depth: integer): DecisionTreeNode;
begin
  // Ограничение глубины
  if depth >= fMaxDepth then
    exit(LeafNode(LeafValue(y, indices)));

  // Минимальное число объектов
  if indices.Length < fMinSamplesSplit then
    exit(LeafNode(LeafValue(y, indices)));

  // Если узел уже чистый
  if IsPure(y, indices) then
    exit(LeafNode(LeafValue(y, indices)));
  
  var parentImp := fCriterion.Impurity(y.SubvectorBy(indices));

  // Поиск лучшего разбиения
  var split := FindBestSplit(X, y, indices);

  if not split.Found then
    exit(LeafNode(LeafValue(y, indices)));

  // Разделение индексов
  var left := new List<integer>;
  var right := new List<integer>;

  foreach var i in indices do
    if X[i, split.Feature] <= split.Threshold then
      left.Add(i)
    else
      right.Add(i);

  // Проверка минимального размера листа
  if (left.Count < fMinSamplesLeaf) or
     (right.Count < fMinSamplesLeaf) then
    exit(LeafNode(LeafValue(y, indices)));
     
  var leftArr := left.ToArray;
  var rightArr := right.ToArray;
  
  var leftImp := fCriterion.Impurity(y.SubvectorBy(leftArr));
  var rightImp := fCriterion.Impurity(y.SubvectorBy(rightArr));
  
  var n := indices.Length;
  
  var delta :=
    parentImp
    - (leftArr.Length / n) * leftImp
    - (rightArr.Length / n) * rightImp;
  
  if delta > 0 then
    fFeatureImportances[split.Feature] += delta;     

  // Рекурсия
  var leftNode := BuildTree(X, y, left.ToArray, depth + 1);
  var rightNode := BuildTree(X, y, right.ToArray, depth + 1);

  // Создание split-узла
  var node := new DecisionTreeNode;
  node.IsLeaf := false;
  node.FeatureIndex := split.Feature;
  node.Threshold := split.Threshold;
  node.Left := leftNode;
  node.Right := rightNode;

  Result := node;
end;

function DecisionTreeBase.FindBestSplit0(X: Matrix; y: Vector; indices: array of integer): SplitResult;
begin
  var bestScore := 1e308;
  var bestFeature := -1;
  var bestThreshold := 0.0;

  var n := indices.Length;
  if n < 2 then
  begin
    Result.Found := false;
    exit;
  end;

  // parent impurity (для feature importance)
  var sumAll := 0.0;
  var sumSqAll := 0.0;

  for var i := 0 to n-1 do
  begin
    var v := y[indices[i]];
    sumAll += v;
    sumSqAll += v*v;
  end;

  var parentMean := sumAll / n;
  var parentVar := (sumSqAll / n) - parentMean*parentMean;

  // --- перебор признаков
  var features := GetFeatureSubset(X.Cols);

  foreach var j in features do
  begin
    // --- собрать пары (value, rowIndex)
    var pairs: array of (real, integer);
    SetLength(pairs, n);

    for var k := 0 to n-1 do
    begin
      var idx := indices[k];
      pairs[k] := (X[idx,j], idx);
    end;

    // сортировка
    pairs.Sort(p -> p.Item1);

    // подготовить y в этом порядке
    var ySorted := new real[n];
    for var k := 0 to n-1 do
      ySorted[k] := y[pairs[k].Item2];

    var leftCount := 0;
    var leftSum := 0.0;
    var leftSumSq := 0.0;

    // идем по возможным split-позициям
    for var k := 1 to n-1 do
    begin
      // переносим элемент k-1 влево
      var v := ySorted[k-1];
      leftCount += 1;
      leftSum += v;
      leftSumSq += v*v;

      var rightCount := n - leftCount;

      if (leftCount < fMinSamplesLeaf) or
         (rightCount < fMinSamplesLeaf) then
        continue;

      var x1 := pairs[k-1].Item1;
      var x2 := pairs[k].Item1;

      if x1 = x2 then
        continue;

      var rightSum := sumAll - leftSum;
      var rightSumSq := sumSqAll - leftSumSq;

      var leftMean := leftSum / leftCount;
      var rightMean := rightSum / rightCount;

      var leftVar := (leftSumSq / leftCount) - leftMean*leftMean;
      var rightVar := (rightSumSq / rightCount) - rightMean*rightMean;

      var weighted :=
        (leftCount / n) * leftVar +
        (rightCount / n) * rightVar;

      if weighted < bestScore then
      begin
        bestScore := weighted;
        bestFeature := j;
        bestThreshold := (x1 + x2) / 2.0;

        if bestScore = 0.0 then
          break;
      end;
    end;
  end;

  Result.Found := bestFeature <> -1;
  Result.Feature := bestFeature;
  Result.Threshold := bestThreshold;

  // --- feature importance (optional)
  if bestFeature <> -1 then
  begin
    var gain := parentVar - bestScore;
    if gain > 0 then
      fFeatureImportances[bestFeature] += gain;
  end;
end;

function DecisionTreeBase.FindBestSplitCls(X: Matrix; y: Vector; indices: array of integer; classCount: integer): SplitResult;
begin
  Result := FindBestSplit0(X, y, indices);
end;

function DecisionTreeBase.IsPure(y: Vector; indices: array of integer): boolean;
begin
  Result := fCriterion.Impurity(y.SubvectorBy(indices)) = 0.0;
end;

function DecisionTreeBase.FindBestSplitReg(X: Matrix; y: Vector; indices: array of integer): SplitResult;
begin
  var bestScore := 1e308;
  var bestFeature := -1;
  var bestThreshold := 0.0;

  var n := indices.Length;
  if n < 2 then
  begin
    Result.Found := false;
    exit;
  end;

  // --- parent sums
  var sumAll := 0.0;
  var sumSqAll := 0.0;

  for var i := 0 to n-1 do
  begin
    var v := y[indices[i]];
    sumAll += v;
    sumSqAll += v*v;
  end;

  // ===============================
  // ЛЯМБДА ОБРАБОТКИ ПРИЗНАКА
  // ===============================

  var ProcessFeature: integer -> () := j ->
  begin
    var pairs: array of (real, integer);
    SetLength(pairs, n);

    for var i := 0 to n-1 do
    begin
      var idx := indices[i];
      pairs[i] := (X[idx,j], idx);
    end;

    pairs.Sort(p -> p.Item1);

    var leftCount := 0;
    var leftSum := 0.0;
    var leftSumSq := 0.0;

    for var i := 1 to n-1 do
    begin
      var v := y[pairs[i-1].Item2];

      leftCount += 1;
      leftSum += v;
      leftSumSq += v*v;

      var rightCount := n - leftCount;

      if (leftCount < fMinSamplesLeaf) or
         (rightCount < fMinSamplesLeaf) then
        continue;

      var x1 := pairs[i-1].Item1;
      var x2 := pairs[i].Item1;

      if x1 = x2 then
        continue;

      var rightSum := sumAll - leftSum;
      var rightSumSq := sumSqAll - leftSumSq;

      var leftMean := leftSum / leftCount;
      var rightMean := rightSum / rightCount;

      var leftVar := (leftSumSq / leftCount) - leftMean*leftMean;
      var rightVar := (rightSumSq / rightCount) - rightMean*rightMean;

      var weighted :=
        (leftCount / n) * leftVar +
        (rightCount / n) * rightVar;

      if weighted < bestScore then
      begin
        bestScore := weighted;
        bestFeature := j;
        bestThreshold := (x1 + x2) / 2.0;
      end;
    end;
  end;

  var p := X.Cols;

  // ===============================
  // FEATURE LOOP WITH SUBSET
  // ===============================

  if (fMaxFeatures <= 0) or (fMaxFeatures >= p) then
  begin
    for var j := 0 to p-1 do
      ProcessFeature(j);
  end
  else
  begin
    var feat := new integer[p];
    for var i := 0 to p-1 do
      feat[i] := i;

    for var i := 0 to fMaxFeatures-1 do
    begin
      var r := i + Random(p - i);
      var tmp := feat[i];
      feat[i] := feat[r];
      feat[r] := tmp;
    end;

    for var k := 0 to fMaxFeatures-1 do
      ProcessFeature(feat[k]);
  end;

  Result.Found := bestFeature <> -1;
  Result.Feature := bestFeature;
  Result.Threshold := bestThreshold;
end;

function DecisionTreeClassifier.PredictOne(x: Vector): integer;
begin
  var node := fRoot;

  while not node.IsLeaf do
  begin
    if x[node.FeatureIndex] <= node.Threshold then
      node := node.Left
    else
      node := node.Right;
  end;

  Result := fIndexToClass[integer(node.LeafValue)]; // internal class index
end;


function DecisionTreeClassifier.MajorityClass(y: Vector; indices: array of integer): integer;
begin
  var counts := new integer[fClassCount];

  // Подсчёт частот
  foreach var i in indices do
  begin
    var c := integer(y[i]);
    counts[c] += 1;
  end;

  // Поиск максимума
  var bestClass := 0;
  var bestCount := -1;

  for var c := 0 to fClassCount - 1 do
    if counts[c] > bestCount then
    begin
      bestCount := counts[c];
      bestClass := c;
    end;

  Result := bestClass;
end;

{function DecisionTreeClassifier.Gini(y: Vector; indices: array of integer): real;
begin
  var counts := new integer[fClassCount];

  foreach var i in indices do
    counts[integer(y[i])] += 1;

  var n := indices.Length;
  var sum := 0.0;

  for var c := 0 to fClassCount - 1 do
  begin
    var p := counts[c] / n;
    sum += p * p;
  end;

  Result := 1.0 - sum;
end;}

constructor DecisionTreeClassifier.Create(maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer);
begin
  inherited Create(maxDepth, minSamplesSplit, minSamplesLeaf);
  fCriterion := new GiniCriterion;
end;

function DecisionTreeClassifier.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.Rows <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.Rows, y.Length);

  if X.Rows = 0 then
    ArgumentError(ER_EMPTY_DATASET);
  
  if fRandomSeed <> 0 then
    Randomize(fRandomSeed);
  
  fFeatureImportances := new Vector(X.Cols);

  // --- 1. Найти уникальные классы
  var unique := new HashSet<integer>;

  for var i := 0 to y.Length - 1 do
    unique.Add(integer(y[i]));

  fClassCount := unique.Count;

  // --- 2. Создать массив index -> original class
  SetLength(fIndexToClass, fClassCount);
  fClassToIndex := new Dictionary<integer, integer>;

  var idx := 0;
  foreach var c in unique do
  begin
    fIndexToClass[idx] := c;
    fClassToIndex[c] := idx;
    idx += 1;
  end;

  // --- 3. Создать закодированный y
  var yEncoded := new Vector(y.Length);

  for var i := 0 to y.Length - 1 do
    yEncoded[i] := fClassToIndex[integer(y[i])];

  // --- 4. Создать массив индексов строк
  var indices := new integer[X.Rows];
  for var i := 0 to X.Rows - 1 do
    indices[i] := i;

  // --- 5. Построить дерево
  fRoot := BuildTree(X, yEncoded, indices, 0);
  
  var s := fFeatureImportances.Sum;
  if s > 0 then
    for var i := 0 to fFeatureImportances.Length-1 do
      fFeatureImportances[i] /= s;

  fFitted := true;

  Result := Self;
end;

function DecisionTreeClassifier.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);;

  var n := X.Rows;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    var row := X.GetRow(i);
    var internalClass := PredictOne(row);

    // вернуть оригинальную метку
    Result[i] := fIndexToClass[internalClass];
  end;
end;

function DecisionTreeClassifier.Clone: IModel;
begin
  var m := new DecisionTreeClassifier(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf
  );

  CopyBaseState(m);

  m.fClassCount := fClassCount;

  if fClassToIndex <> nil then
  begin
    m.fClassToIndex := new Dictionary<integer, integer>;
    foreach var kv in fClassToIndex do
      m.fClassToIndex[kv.Key] := kv.Value;
  end;

  if fIndexToClass <> nil then
  begin
    SetLength(m.fIndexToClass, Length(fIndexToClass));
    for var i := 0 to Length(fIndexToClass) - 1 do
      m.fIndexToClass[i] := fIndexToClass[i];
  end;

  Result := m;
end;

function DecisionTreeClassifier.LeafValue(y: Vector; indices: array of integer): real;
begin
  Result := MajorityClass(y, indices);
end;

// DecisionTreeRegressor

constructor DecisionTreeRegressor.Create(maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer);
begin
  inherited Create(maxDepth, minSamplesSplit, minSamplesLeaf);
  fCriterion := new VarianceCriterion;
end;

function DecisionTreeRegressor.LeafValue(y: Vector; indices: array of integer): real;
begin
  var sum := 0.0;
  foreach var i in indices do
    sum += y[i];

  Result := sum / indices.Length;
end;

function DecisionTreeRegressor.FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; 
begin
  Result := FindBestSplitReg(X, y, indices);
end;

function DecisionTreeRegressor.IsPure(y: Vector; indices: array of integer): boolean;
begin
  var first := y[indices[0]];
  for var i := 1 to indices.Length-1 do
    if y[indices[i]] <> first then
      exit(false);
  Result := true;
end;

function DecisionTreeRegressor.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.Rows <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.Rows, y.Length);

  if X.Rows = 0 then
    ArgumentError(ER_EMPTY_DATASET);
  
  if fRandomSeed <> 0 then
    Randomize(fRandomSeed);
  
  fFeatureImportances := new Vector(X.Cols);

  var indices: array of integer;

  // 🔹 Ключевое изменение
  if fRowIndices = nil then
  begin
    SetLength(indices, X.Rows);
    for var i := 0 to X.Rows - 1 do
      indices[i] := i;
  end
  else
    indices := fRowIndices;

  fRoot := BuildTree(X, y, indices, 0);
  
  var s := fFeatureImportances.Sum;
  if s > 0 then
    for var i := 0 to fFeatureImportances.Length - 1 do
      fFeatureImportances[i] /= s;
  
  fFitted := true;
  
  fRowIndices := nil;

  Result := Self;
end;

function DecisionTreeRegressor.PredictOne(x: Vector): real;
begin
  var node := fRoot;

  while not node.IsLeaf do
  begin
    if x[node.FeatureIndex] <= node.Threshold then
      node := node.Left
    else
      node := node.Right;
  end;

  Result := node.LeafValue;
end;

function DecisionTreeRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var n := X.Rows;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
    Result[i] := PredictOne(X.GetRow(i));
end;

function DecisionTreeRegressor.Clone: IModel;
begin
  var m := new DecisionTreeRegressor(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf
  );

  CopyBaseState(m);

  Result := m;
end;


//-----------------------------
//      RandomForestBase 
//-----------------------------

constructor RandomForestBase.Create(
  nTrees: integer;
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  maxFeatures: TMaxFeaturesMode);
begin
  fNTrees := nTrees;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fMaxFeaturesMode := maxFeatures;
  fFitted := false;
end;

function RandomForestBase.ComputeMaxFeatures(p: integer): integer;
begin
  case fMaxFeaturesMode of
    AllFeatures:  Result := p;
    SqrtFeatures: Result := integer(Sqrt(p));
    Log2Features: Result := integer(Log2(p));
    HalfFeatures: Result := p div 2;
  end;
end;

procedure RandomForestBase.BootstrapSample(X: Matrix; y: Vector;
  var Xb: Matrix; var yb: Vector);
begin
  var n := X.Rows;
  var p := X.Cols;

  Xb := new Matrix(n, p);
  yb := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    var idx := Random(n);

    for var j := 0 to p - 1 do
      Xb[i,j] := X[idx,j];

    yb[i] := y[idx];
  end;
end;

//-----------------------------
//     RandomForestRegressor 
//-----------------------------

constructor RandomForestRegressor.Create(nTrees: integer; maxDepth: integer;
  minSamplesSplit: integer; minSamplesLeaf: integer;
  maxFeaturesMode: TMaxFeaturesMode);
begin
  inherited Create(nTrees,maxDepth,minSamplesSplit,minSamplesLeaf,maxFeaturesMode)
end;

function RandomForestRegressor.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.Rows <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.Rows, y.Length);

  if X.Rows = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  SetLength(fTrees, fNTrees);

  for var t := 0 to fNTrees - 1 do
  begin
    var Xb: Matrix;
    var yb: Vector;

    BootstrapSample(X, y, Xb, yb);

    var tree := new DecisionTreeRegressor(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf
    );
    
    var p := X.Cols;
    var m := ComputeMaxFeatures(p);
        
    tree.fMaxFeatures := m; 
    tree.Fit(Xb, yb);
    fTrees[t] := tree;
  end;

  fFitted := true;
  Result := Self;
end;

function RandomForestRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var n := X.Rows;
  var resultVec := new Vector(n);

  for var t := 0 to fTrees.Length - 1 do
  begin
    var pred := fTrees[t].Predict(X);
    for var i := 0 to n - 1 do
      resultVec[i] += pred[i];
  end;

  for var i := 0 to n - 1 do
    resultVec[i] /= fTrees.Length;

  Result := resultVec;
end;

function RandomForestRegressor.Clone: IModel;
begin
  var rf := new RandomForestRegressor(
    fNTrees,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf
  );

  rf.fFitted := fFitted;

  SetLength(rf.fTrees, fTrees.Length);
  for var i := 0 to fTrees.Length - 1 do
    rf.fTrees[i] := DecisionTreeRegressor(fTrees[i].Clone);

  Result := rf;
end;

function RandomForestRegressor.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var p := fTrees[0].FeatureImportances.Length;
  var resultVec := new Vector(p);

  for var t := 0 to fTrees.Length - 1 do
    resultVec += fTrees[t].FeatureImportances;

  resultVec *= 1.0 / fTrees.Length;

  Result := resultVec;
end;

//-----------------------------
//     RandomForestClassifier 
//-----------------------------
constructor RandomForestClassifier.Create(nTrees: integer; 
  maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer;
  maxFeaturesMode: TMaxFeaturesMode);
begin
  inherited Create(nTrees,maxDepth,minSamplesSplit,minSamplesLeaf,maxFeaturesMode);
end;

function RandomForestClassifier.Fit(X: Matrix; y: Vector): IModel;
begin
  if X.Rows <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.Rows, y.Length);

  if X.Rows = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  SetLength(fTrees, fNTrees);

  var p := X.Cols;

  for var t := 0 to fNTrees-1 do
  begin
    var Xb: Matrix;
    var yb: Vector;
    
    BootstrapSample(X, y, Xb, yb);

    var tree := new DecisionTreeClassifier(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf
    );
    
    var m := ComputeMaxFeatures(p);
    
    // классический RF для классификации - Sqrt(p)
    tree.fMaxFeatures := m; 

    tree.Fit(Xb, yb);
    fTrees[t] := tree;
  end;

  fFitted := true;
  Result := Self;
end;

function RandomForestClassifier.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var n := X.Rows;
  var p := X.Cols;
  var resultVec := new Vector(n);

  for var i := 0 to n-1 do
  begin
    var votes := new Dictionary<integer, integer>;

    // сформировать вектор строки один раз
    var row := new Vector(p);
    for var j := 0 to p-1 do
      row[j] := X[i,j];

    for var t := 0 to fTrees.Length-1 do
    begin
      var cls := integer(fTrees[t].PredictOne(row));

      if not votes.ContainsKey(cls) then
        votes[cls] := 0;

      votes[cls] += 1;
    end;

    var bestClass := 0;
    var bestCount := -1;

    foreach var kv in votes do
      if kv.Value > bestCount then
      begin
        bestCount := kv.Value;
        bestClass := kv.Key;
      end;

    resultVec[i] := bestClass;
  end;

  Result := resultVec;
end;

function RandomForestClassifier.Clone: IModel;
begin
  var rf := new RandomForestClassifier(
    fNTrees,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf
  );

  rf.fFitted := fFitted;

  SetLength(rf.fTrees, fTrees.Length);

  for var i := 0 to fTrees.Length-1 do
    rf.fTrees[i] := DecisionTreeClassifier(fTrees[i].Clone);

  Result := rf;
end;

function RandomForestClassifier.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var p := fTrees[0].FeatureImportances.Length;
  var resultVec := new Vector(p);

  for var t := 0 to fTrees.Length - 1 do
    resultVec += fTrees[t].FeatureImportances;

  resultVec *= 1.0 / fTrees.Length;

  Result := resultVec;
end;

//-----------------------------
//  GradientBoostingRegressor 
//-----------------------------
constructor GradientBoostingRegressor.Create(
  nEstimators: integer;
  learningRate: real;
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  subsample: real;
  randomSeed: integer;
  loss: TGBLoss;
  huberDelta: real;
  earlyStoppingPatience: integer);
begin
  if nEstimators <= 0 then
    ArgumentOutOfRangeError(ER_N_ESTIMATORS_NOT_POSITIVE);

  if learningRate <= 0 then
    ArgumentOutOfRangeError(ER_LEARNING_RATE_NOT_POSITIVE);

  if (subsample <= 0) or (subsample > 1) then
    ArgumentOutOfRangeError(ER_SUBSAMPLE_OUT_OF_RANGE);

  fNEstimators := nEstimators;
  fLearningRate := learningRate;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fSubsample := subsample;
  fRandomSeed := randomSeed;

  fEstimators := new List<DecisionTreeRegressor>;
  fFitted := false;
  
// -------------  
  if huberDelta <= 0 then
    ArgumentOutOfRangeError('huberDelta must be > 0!!huberDelta must be > 0');
  
  if earlyStoppingPatience < 0 then
    ArgumentOutOfRangeError('earlyStoppingPatience must be >= 0!!earlyStoppingPatience must be >= 0');
  
  fLoss := loss;
  fHuberDelta := huberDelta;
  fEarlyStoppingPatience := earlyStoppingPatience;
  
  fTrainLossHistory := new List<real>;
  fValLossHistory := new List<real>;
end;

function GradientBoostingRegressor.ComputeTrainLoss(y, yPred: Vector): real;
begin
  var n := y.Length;
  var loss := 0.0;

  if fLoss = TGBLoss.SquaredError then
  begin
    for var i := 0 to n - 1 do
    begin
      var d := y[i] - yPred[i];
      loss += d * d;
    end;
    Result := loss / n;
    exit;
  end;

  // Huber
  var delta := fHuberDelta;
  for var i := 0 to n - 1 do
  begin
    var d := y[i] - yPred[i];
    var ad := Abs(d);
    if ad <= delta then
      loss += 0.5 * d * d
    else
      loss += delta * (ad - 0.5 * delta);
  end;

  Result := loss / n;
end;

procedure GradientBoostingRegressor.ComputePseudoResiduals(y, yPred: Vector; r: Vector);
begin
  var n := y.Length;

  if fLoss = TGBLoss.SquaredError then
  begin
    for var i := 0 to n - 1 do
      r[i] := y[i] - yPred[i];
    exit;
  end;

  // Huber: псевдо-остатки = антиградиент
  var delta := fHuberDelta;
  for var i := 0 to n - 1 do
  begin
    var d := y[i] - yPred[i];
    var ad := Abs(d);
    if ad <= delta then
      r[i] := d
    else
      r[i] := delta * Sign(d);
  end;
end;

const MinImprovement = 1e-12;

function GradientBoostingRegressor.Fit(X: Matrix; y: Vector): IModel;
begin
  Result := FitInternal(X, y, nil, nil, false);
end;

function GradientBoostingRegressor.FitWithValidation(
  XTrain: Matrix; yTrain: Vector;
  XVal: Matrix; yVal: Vector): IModel;
begin
  Result := FitInternal(XTrain, yTrain, XVal, yVal, true);
end;

function GradientBoostingRegressor.FitInternal(
  XTrain: Matrix; yTrain: Vector;
  XVal: Matrix; yVal: Vector;
  useValidation: boolean): IModel;
begin
  // --- checks ---
  if XTrain = nil then ArgumentNullError(ER_X_NULL);
  if yTrain = nil then ArgumentNullError(ER_Y_NULL);

  if XTrain.Rows <> yTrain.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if XTrain.Rows = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if useValidation then
  begin
    if XVal = nil then ArgumentNullError(ER_X_NULL);
    if yVal = nil then ArgumentNullError(ER_Y_NULL);

    if XVal.Rows <> yVal.Length then
      DimensionError(ER_XY_SIZE_MISMATCH);

    if XVal.Cols <> XTrain.Cols then
      DimensionError(ER_FEATURE_COUNT_MISMATCH);
  end;

  // --- init state ---
  fEstimators.Clear;
  fFeatureCount := XTrain.Cols;

  fTrainLossHistory.Clear;
  fValLossHistory.Clear;

  fBestIteration := -1;
  fBestTrainLoss := real.PositiveInfinity;
  fBestValLoss := real.PositiveInfinity;

  var noImprove := 0;

  // --- F0 = mean(yTrain) ---
  var nTrain := yTrain.Length;
  var sum := 0.0;
  for var i := 0 to nTrain - 1 do
    sum += yTrain[i];
  fInitValue := sum / nTrain;

  var yPredTrain := new Vector(nTrain);
  for var i := 0 to nTrain - 1 do
    yPredTrain[i] := fInitValue;

  var yPredVal: Vector := nil;
  if useValidation then
  begin
    var nVal := yVal.Length;
    yPredVal := new Vector(nVal);
    for var i := 0 to nVal - 1 do
      yPredVal[i] := fInitValue;
  end;

  Randomize(fRandomSeed);

  // --- boosting loop ---
  for var m := 0 to fNEstimators - 1 do
  begin
    // pseudo-residuals on TRAIN only
    var r := new Vector(nTrain);
    ComputePseudoResiduals(yTrain, yPredTrain, r);

    var tree := new DecisionTreeRegressor(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf
    );

    if fSubsample < 1.0 then
    begin
      var k := Round(nTrain * fSubsample);
      if k < 1 then k := 1;

      var rows := new integer[k];
      for var i := 0 to k - 1 do
        rows[i] := Random(nTrain);

      tree.SetRowIndices(rows);
    end;

    tree.Fit(XTrain, r);
    fEstimators.Add(tree);

    // update TRAIN prediction
    var deltaTrain := tree.Predict(XTrain);
    for var i := 0 to nTrain - 1 do
      yPredTrain[i] += fLearningRate * deltaTrain[i];

    // update VAL prediction (if used)
    if useValidation then
    begin
      var deltaVal := tree.Predict(XVal);
      for var i := 0 to yPredVal.Length - 1 do
        yPredVal[i] += fLearningRate * deltaVal[i];
    end;

    // losses
    var trainLoss := ComputeTrainLoss(yTrain, yPredTrain);
    fTrainLossHistory.Add(trainLoss);

    var scoreLoss: real; // this is what we early-stop on
    if useValidation then
    begin
      var valLoss := ComputeTrainLoss(yVal, yPredVal);
      fValLossHistory.Add(valLoss);
      scoreLoss := valLoss;
    end
    else
      scoreLoss := trainLoss;

    // early stopping
    if fEarlyStoppingPatience > 0 then
    begin
      if (fBestValLoss - scoreLoss > MinImprovement) then
      begin
        fBestValLoss := scoreLoss;
        fBestIteration := m;
        noImprove := 0;
      end
      else
      begin
        noImprove += 1;
        if noImprove >= fEarlyStoppingPatience then
          break;
      end;
    end;
  end;

  // if stopping enabled: cut trees after best iteration
  if (fEarlyStoppingPatience > 0) and (fBestIteration >= 0) then
  begin
    var keep := fBestIteration + 1;
    if fEstimators.Count > keep then
      fEstimators.RemoveRange(keep, fEstimators.Count - keep);
  end;

  fFitted := true;
  Result := Self;
end;

function GradientBoostingRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.Cols <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var n := X.Rows;
  var yPred := new Vector(n);

  for var i := 0 to n - 1 do
    yPred[i] := fInitValue;

  foreach var tree in fEstimators do
  begin
    var delta := tree.Predict(X);
    for var i := 0 to n - 1 do
      yPred[i] += fLearningRate * delta[i];
  end;

  Result := yPred;
end;

function GradientBoostingRegressor.Clone: IModel;
begin
  var copy := new GradientBoostingRegressor(
    fNEstimators,
    fLearningRate,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fSubsample,
    fRandomSeed);

  copy.fInitValue := fInitValue;
  copy.fFeatureCount := fFeatureCount;
  copy.fFitted := fFitted;

  foreach var tree in fEstimators do
    copy.fEstimators.Add(tree.Clone as DecisionTreeRegressor);

  Result := copy;
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
    ArgumentError(ER_MODEL_NULL);
  fModel := model;
end;

class function Pipeline.Build(params steps: array of IPipeStep): Pipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  // последний шаг должен быть моделью
  var last := steps[High(steps)];

  if not (last is IModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_MODEL);

  var pipe := new Pipeline(last as IModel);

  // все шаги кроме последнего должны быть преобразователями
  for var i := 0 to High(steps) - 1 do
  begin
    var step := steps[i];

    if not (step is ITransformer) then
      ArgumentError(ER_PIPELINE_INVALID_STEP_ORDER);

    pipe.Add(step as ITransformer);
  end;

  Result := pipe;
end;

function Pipeline.Add(t: ITransformer): Pipeline;
begin
  if t = nil then
    ArgumentError(ER_TRANSFORMER_NULL);

  fTransformers.Add(t);
  Result := Self;
end;

function Pipeline.SetModel(m: IModel): Pipeline;
begin
  if m = nil then
    ArgumentError(ER_MODEL_NULL);

  fModel := m;
  Result := Self;
end;

function Pipeline.Fit(X: Matrix; y: Vector): IModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

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
    NotFittedError(ER_FIT_NOT_CALLED);

  var Xt := X;

  foreach var t in fTransformers do
    Xt := t.Transform(Xt);

  Result := Xt;
end;

function Pipeline.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var Xt := Transform(X);
  Result := fModel.Predict(Xt);
end;

function Pipeline.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if not (fModel is IProbabilisticClassifier) then
    Error(ER_PROBA_NOT_SUPPORTED);

  var Xt := Transform(X);

  Result := (fModel as IProbabilisticClassifier)
              .PredictProba(Xt);
end;

function Pipeline.ToString: string;
begin
  var sb := 'Pipeline (' +
            (if fFitted then 'trained' else 'not trained') + '):' + NewLine;

  var idx := 1;

  foreach var t in fTransformers do
  begin
    sb += '  [' + idx + '] ' + t.ToString + NewLine;
    idx += 1;
  end;

  if fModel <> nil then
    sb += '  [' + idx + '] ' + fModel.ToString;

  Result := sb;
end;

function Pipeline.Clone: IModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var p := new Pipeline;

  foreach var t in fTransformers do
    p.Add(t.Clone);

  p.SetModel(fModel.Clone);

  Result := p;
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
    NotFittedError(ER_FIT_NOT_CALLED);

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

function StandardScaler.ToString: string;
begin
  Result := 'StandardScaler';
end;

function StandardScaler.Clone: ITransformer;
begin
  var s := new StandardScaler;
  s.fMean := fMean.Clone;
  s.fStd := fStd.Clone;
  s.fFitted := fFitted;
  Result := s;
end;

//-----------------------------
//       MinMaxScaler
//-----------------------------

constructor MinMaxScaler.Create(rangeMin: real; rangeMax: real);
begin
  if rangeMax <= rangeMin then
    ArgumentError(ER_RANGE_INVALID);

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
    NotFittedError(ER_FIT_NOT_CALLED);

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

function MinMaxScaler.ToString: string;
begin
  Result := 'MinMaxScaler(min=' + fRangeMin + ', max=' + fRangeMax + ')';
end;

function MinMaxScaler.Clone: ITransformer;
begin
  var s := new MinMaxScaler;
  s.fMin := fMin.Clone;
  s.fMax := fMax.Clone;
  s.fFitted := fFitted;
  s.fRangeMin := fRangeMin;
  s.fRangeMax := fRangeMax;
  Result := s;
end;

//-----------------------------
//        PCATransformer
//-----------------------------

constructor PCATransformer.Create(k: integer);
begin
  if k <= 0 then
    ArgumentError(ER_K_MUST_BE_POSITIVE);

  fK := k;
  fFitted := false;
end;

function PCATransformer.Fit(X: Matrix): ITransformer;
begin
  if fK > X.ColCount then
    ArgumentError(ER_K_EXCEEDS_FEATURES);

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
    NotFittedError(ER_FIT_NOT_CALLED);

  var Xc := X.Clone;

  for var j := 0 to X.ColCount - 1 do
    for var i := 0 to X.RowCount - 1 do
      Xc[i,j] -= fMean[j];

  Result := Xc * fComponents;
end;

function PCATransformer.ToString: string;
begin
  Result := 'PCATransformer(k=' + fK + ')';
end;

function PCATransformer.Clone: ITransformer;
begin
  var t := new PCATransformer(fK);
  t.fComponents := fComponents.Clone;
  t.fMean := fMean.Clone;
  t.fFitted := fFitted;
  Result := t;
end;

//-----------------------------
//      VarianceThreshold
//-----------------------------

constructor VarianceThreshold.Create(threshold: real);
begin
  if threshold < 0 then
    ArgumentError(ER_THRESHOLD_NEGATIVE);

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
    NotFittedError(ER_FIT_NOT_CALLED);

  var n := X.RowCount;
  var k := fSelected.Length;

  var R := new Matrix(n, k);

  for var i := 0 to n - 1 do
    for var c := 0 to k - 1 do
      R[i,c] := X[i, fSelected[c]];

  Result := R;
end;

function VarianceThreshold.ToString: string;
begin
  Result := 'VarianceThreshold(threshold=' + fThreshold + ')';
end;

function VarianceThreshold.Clone: ITransformer;
begin
  var t := new VarianceThreshold(fThreshold);
  t.fSelected := Copy(fSelected);
  t.fFitted := fFitted;
  Result := t;
end;

//-----------------------------
//         SelectKBest 
//-----------------------------

constructor SelectKBest.Create(k: integer; score: FeatureScore);
begin
  if k <= 0 then
    ArgumentError(ER_K_MUST_BE_POSITIVE);

  fK := k;
  fScoreType := score;
  fScoreFunc := nil;
  fFitted := false;
end;

constructor SelectKBest.Create(k: integer; scoreFunc: (Vector, Vector) -> real);
begin
  if k <= 0 then
    ArgumentError(ER_K_MUST_BE_POSITIVE);

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
      ArgumentError(ER_CHI_SQUARE_NEGATIVE);

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
    Error(ER_UNKNOWN_FEATURE_SCORE);
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
  ArgumentError(ER_SELECTKBEST_FIT_INVALID);
end;

function SelectKBest.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var n := X.RowCount;
  var k := fSelected.Length;

  var R := new Matrix(n, k);

  for var i := 0 to n - 1 do
    for var c := 0 to k - 1 do
      R[i,c] := X[i, fSelected[c]];

  Result := R;
end;

function SelectKBest.ToString: string;
begin
  var scoreStr :=
    if fScoreFunc <> nil then
      'custom'
    else
      fScoreType.ToString;

  Result := 'SelectKBest(k=' + fK + ', score=' + scoreStr + ')';
end;

function SelectKBest.Clone: ITransformer;
begin
  var t := new SelectKBest(fK, fScoreType);
  t.fScoreFunc := fScoreFunc;
  t.fSelected := Copy(fSelected);
  t.fFitted := fFitted;
  Result := t;
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
    NotFittedError(ER_FIT_NOT_CALLED);

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

function Normalizer.ToString: string;
begin
  Result :=
    'Normalizer(norm=' + fNormType.ToString + ')';
end;

function Normalizer.Clone: ITransformer;
begin
  var t := new Normalizer(fNormType);
  t.fFitted := fFitted;
  Result := t;
end;

    
end.