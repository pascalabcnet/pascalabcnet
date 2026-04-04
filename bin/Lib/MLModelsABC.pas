/// Модуль моделей машинного обучения и матричных преобразований.
///
/// Содержит:
/// • функции активации
/// • модели: LinearRegression, LogisticRegression, ElasticNet,
///   kNN, DecisionTree, RandomForest, GradientBoosting,
///   KMeans, DBSCAN
/// • трансформеры признаков (StandardScaler, PCA и др.)
/// • Pipeline для объединения преобразований и модели.
///
/// Все алгоритмы работают с числовыми данными:
/// X — Matrix (объекты × признаки), y — Vector (целевая переменная).
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
    /// Используется в логистической регрессии и других моделях для получения вероятности.
    /// Формула: σ(x) = 1 / (1 + e^(−x)).  
    static function Sigmoid(v: Vector): Vector;
    
    /// Функция активации Tanh (гиперболический тангенс).
    /// Похожа на Sigmoid, но значения лежат в диапазоне (−1, 1).
    /// Применяется в моделях, где требуется симметричная нелинейность относительно нуля.
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
    ///   все элементы неотрицательны, их сумма равна 1.
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
///   мультиколлинеарности и когда число признаков существенно меньше числа объектов
  LinearRegression = class(IRegressor)
  private
    fCoef: Vector;
    fIntercept: real;
    fFitted: boolean;
  public
    constructor Create();

    /// Обучает модель на числовых данных
    ///   X — матрица m × n (m объектов, n признаков)
    ///   y — вектор длины m
    function Fit(X: Matrix; y: Vector): ISupervisedModel;

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
/// После вызова Fit значение становится True.
/// Используется для проверки корректности вызова Predict.
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
    
    function Name: string := Self.GetType.Name;
  end;  
  
/// Линейная регрессионная модель с L2-регуляризацией (Ridge).
/// Минимизирует функцию:
///     ‖y - (Xβ + b)‖² + λ ‖β‖².
/// Устойчива к мультиколлинеарности и плохо обусловленным данным.
/// Используется при коррелированных признаках
/// и в задачах, где важна численная стабильность решения
  RidgeRegression = class(IRegressor)
  private
    fLambda: real;
    fCoef: Vector;
    fIntercept: real;
    fFitted: boolean;
  public
    /// Создаёт модель Ridge-регрессии:
    ///   • lambda — коэффициент L2-регуляризации (0 — обычная линейная регрессия)
    constructor Create(lambda: real := 1.0);
  
    /// Обучает модель на числовых данных.
    ///   X — матрица m × n (m объектов, n признаков).
    ///   y — вектор длины m с непрерывными значениями.
    /// Выполняется центрирование признаков и целевой переменной.
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  
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
    
    function Name: string := Self.GetType.Name;
  end;
  
/// Линейная регрессионная модель ElasticNet.
/// Минимизирует функцию:
///   ‖y - (Xβ + b)‖² + λ1 ‖β‖₁ + λ2 ‖β‖².
/// Объединяет L1-регуляризацию (разреженность, отбор признаков)
///   и L2-регуляризацию (численная устойчивость).
/// Используется при большом числе признаков, особенно если признаки коррелированы.
/// Обучение выполняется методом покоординатного спуска 
/// ВАЖНО: Модель чувствительна к масштабу признаков.
/// Всегда используйте StandardScaler в Pipeline перед ElasticNet.
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
    ///   soft(z, γ) = sign(z) * max(|z| - γ, 0).
    /// Используется для реализации L1-регуляризации.
    function SoftThreshold(z, gamma: real): real;
  public
    /// Создаёт модель ElasticNet:
    ///   • lambda1 — коэффициент L1-регуляризации (>= 0).
    ///   • lambda2 — коэффициент L2-регуляризации (>= 0).
    ///   • maxIter — максимальное число итераций coordinate descent.
    ///   • tol — критерий остановки по изменению коэффициентов.
    constructor Create(lambda1, lambda2: real; maxIter: integer := 1000; tol: real := 1e-6);
  
    /// Обучает модель на числовых данных.
    ///   X — матрица m × n (m объектов, n признаков).
    ///   y — вектор длины m с непрерывными значениями.
    /// Выполняется центрирование признаков и целевой переменной.
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  
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
    /// После вызова Fit значение становится True.
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
    
    function Name: string := Self.GetType.Name;
  end;
    
/// Логистическая регрессия.
/// Поддерживает бинарную и многоклассовую классификацию.
/// Для нескольких классов используется softmax,
///   для двух классов — частный случай softmax.
/// Оптимизация выполняется по кросс-энтропийной функции потерь
///   с поддержкой L2-регуляризации
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
    
    fTol: real := 1e-6;
    fCheckConvergence: boolean := true;
    fMinImprovement: real := 1e-8;
    
    fClassLabels: array of string; // В каждой модели классификации
    
    function GetWeights: Matrix;
    function GetIntercept: Vector;
  public
    /// lambda — коэффициент L2-регуляризации.
    /// lr — шаг градиентного спуска.
    /// epochs — число итераций обучения
    constructor Create(lambda: real := 0.0; lr: real := 0.1; epochs: integer := 1000);
  
/// Обучает модель логистической регрессии.
///   X — матрица признаков.
///   y — вектор меток классов (целочисленные значения).
/// Возвращает обученную модель.
/// После вызова IsFitted становится True.
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  
    /// Возвращает матрицу вероятностей (m x k).
    function PredictProba(X: Matrix): Matrix;
  
    /// Возвращает массив меток классов в порядке столбцов PredictProba.
    function GetClasses: array of real;

    /// Возвращает предсказанные классы для объектов из X.
    /// Результат — вектор значений, где каждый элемент соответствует классу объекта.
    /// Порядок элементов соответствует строкам матрицы X.
    /// Требует предварительного вызова Fit.
    function Predict(X: Matrix): Vector;
    
    /// Возвращает предсказанные метки классов для объектов из X.
    /// Каждый элемент результата — индекс класса (целое число).
    /// Порядок элементов соответствует строкам матрицы X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;
  
/// Показывает, была ли модель обучена.
/// Если false — вызов Predict или PredictProba приведет к ошибке.
    property IsFitted: boolean read fFitted;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
    
/// Создает глубокую копию модели.
    function Clone: IModel;
    
    /// Матрица коэффициентов модели (p × k).
    property Weights: Matrix read GetWeights;
    
    /// Вектор свободных членов (bias) для каждого класса.
    property Intercept: Vector read GetIntercept;
    
    function Name: string := Self.GetType.Name;
    
    procedure SetClassLabels(classes: array of string);
    
    function GetClassLabels: array of string;
  end;
  
  DecisionTreeNode = class
  public
    IsLeaf: boolean;
    FeatureIndex: integer;
    Threshold: real;
    Left: DecisionTreeNode;
    Right: DecisionTreeNode;
    LeafValue: real;
    
/// Создает глубокую копию узла вместе со всеми подузлами
    function Clone: DecisionTreeNode;
  end;
  
/// Результат поиска лучшего разбиения узла дерева
  SplitResult = record
/// Found = true, если допустимое разбиение найдено.
    Found: boolean;
/// Feature — индекс признака, по которому делается split.
    Feature: integer;
/// Threshold — пороговое значение признака.
    Threshold: real;
    
    static function Invalid: SplitResult;
    begin
      Result.Found := false;
      Result.Feature := -1;
      Result.Threshold := 0.0;
    end;
  
    static function Create(feature: integer; threshold: real): SplitResult;
    begin
      Result.Found := true;
      Result.Feature := feature;
      Result.Threshold := threshold;
    end;
  end;
  
/// Интерфейс критерия разбиения узла дерева.
/// Определяет функцию нечистоты (impurity), которая используется для оценки качества разбиения
  ISplitCriterion = interface
/// Вычисляет нечистоту для вектора целевых значений y.
/// Чем меньше значение — тем "чище" узел.
    function Impurity(y: Vector; indices: array of integer): real;
  end;
  
/// Критерий Джини.
/// Используется в классификации.
/// Минимизирует Gini-нечистоту, что приводит к более однородным по классам листьям
  GiniCriterion = class(ISplitCriterion)
  private
    fClassCount: integer;
  public
    constructor Create(classCount: integer);
    begin
      fClassCount := classCount;
    end;
    function Impurity(y: Vector; indices: array of integer): real;
  end;

/// Критерий дисперсии.
/// Используется в регрессии.
/// Минимизирует внутригрупповую дисперсию значений целевой переменной
  VarianceCriterion = class(ISplitCriterion)
  public
  /// Вычисляет дисперсию значений y.
  /// Чем меньше дисперсия — тем лучше узел.
    function Impurity(y: Vector; indices: array of integer): real;
  end;
  
  /// Критерий энтропии (Information Gain).
  /// Используется в классификации.
  /// Минимизирует энтропию распределения классов, стремясь к максимально "чистым" узлам.
  /// Основан на теории информации: чем меньше энтропия, тем менее неопределённо распределение классов.
  EntropyCriterion = class(ISplitCriterion)
  private
    fClassCount: integer;
  public
    constructor Create(classCount: integer);
    
    /// Вычисляет энтропию распределения классов в узле.
    /// Чем меньше значение — тем более однороден узел по классам.
    function Impurity(y: Vector; indices: array of integer): real;
  end;  
//============================  
//    DecisionTreeBase
//============================  
/// Базовый абстрактный класс дерева решений.
/// Реализует общую логику построения структуры дерева:
///   рекурсивное разбиение, контроль глубины,
///   минимального числа объектов и расчет важности признаков.
/// Конкретная логика вычисления значения листа и критерия разбиения задается в наследниках
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
    fMaxFeatures: integer := 0;
    fRowIndices: array of integer := nil;
    
    fRng: System.Random;
    fUserProvidedSeed: boolean;
  
    function BuildTree(X: Matrix; y: Vector; indices: array of integer; depth: integer): DecisionTreeNode;
  
    //function FindBestSplit0(X: Matrix; y: Vector; indices: array of integer): SplitResult;
    
    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; virtual; abstract;
    function IsPure(y: Vector; indices: array of integer): boolean; virtual;
// --------------------------    
  
    function LeafValue(y: Vector; indices: array of integer): real; virtual; abstract;
    function LeafNode(value: real): DecisionTreeNode;
    procedure CopyBaseState(dest: DecisionTreeBase);
    function GetFeatureSubset(nFeatures: integer): array of integer; virtual;
    
    procedure SetRowIndices(rows: array of integer);
  public
/// Создает дерево решений:
///   • maxDepth — максимальная глубина дерева.
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1; seed: integer := -1);
  
/// Возвращает вектор важности признаков.
/// Важность вычисляется как суммарное уменьшение
///   нечистоты (impurity reduction) по всем разбиениям.
/// Значения нормированы так, что сумма равна 1.
    function FeatureImportances: Vector;
    
/// Обучает дерево решений.
/// X — матрица признаков.
/// y — целевая переменная.
/// Реализация зависит от типа дерева (регрессия или классификация).
    function Fit(X: Matrix; y: Vector): ISupervisedModel; virtual; abstract;

/// Выполняет предсказание для матрицы X.
/// Возвращает вектор прогнозов.
/// Для регрессии — вещественные значения.
/// Для классификации — метки классов.
    function Predict(X: Matrix): Vector; virtual; abstract;

/// Создает глубокую копию дерева.
/// Копируется структура узлов, параметры и обученное состояние.    
    function Clone: IModel; virtual; abstract;

/// Возвращает true, если дерево обучено.
/// Если false — Predict вызовет ошибку.
    property IsFitted: boolean read fFitted;
    
    function Name: string := Self.GetType.Name;
  end;

//============================  
//   DecisionTreeClassifier  
//============================  
/// Дерево решений для задачи классификации.
/// Использует критерий нечистоты (обычно Gini) для выбора оптимальных разбиений.
/// В листьях хранится наиболее частый класс
  DecisionTreeClassifier = class(DecisionTreeBase, IClassifier)
  private
    fClassToIndex: Dictionary<integer, integer>;
    fIndexToClass: array of integer;
    fClassCount: integer;
    
    fClassLabels: array of string;

    function PredictOne(X: Matrix; rowIndex: integer): integer;
    function MajorityClass(y: Vector; indices: array of integer): integer;
    
  protected  
    function LeafValue(y: Vector; indices: array of integer): real; override;
    
    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; override;

  public
/// Создает классификационное дерево:
///   • maxDepth — максимальная глубина дерева (-1 означает без ограничения).
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1; seed: integer := -1);

/// Обучает классификационное дерево.
/// X — матрица признаков.
/// y — вектор целевых меток (целые значения).
/// Строит структуру дерева путем минимизации нечистоты в узлах.
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;
    
/// Выполняет предсказание меток классов для X.
/// Для каждого объекта возвращается класс, соответствующий листу дерева.
    function Predict(X: Matrix): Vector; override;
    
    /// Возвращает предсказанные метки классов для объектов из X.
    /// Каждый элемент результата — индекс класса (целое число).
    /// Порядок элементов соответствует строкам матрицы X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;
    
/// Создает глубокую копию дерева классификации.
/// Копируется структура узлов, параметры и обученное состояние.
    function Clone: IModel; override;

/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function ClassCount: integer := fClassCount;
    
    function IndexToClass: array of integer := Copy(fIndexToClass);
    
    function Name: string := Self.GetType.Name;
    
    procedure SetClassLabels(classes: array of string);
    
    function GetClassLabels: array of string;
  end;
  
//============================  
//   DecisionTreeRegressor  
//============================  
/// Дерево решений для задачи регрессии.
/// Наследуется от DecisionTreeBase.
/// Использует критерий дисперсии для выбора разбиений.
/// В листьях хранится среднее значение целевой переменной.
/// Поддерживает L2-регуляризацию значения листа (leafL2)
  DecisionTreeRegressor = class(DecisionTreeBase, IRegressor)
  private
    fLeafL2: real;
  
    function PredictOne(X: Matrix; rowIndex: integer): real;
  
  protected
/// Вычисляет значение листа для набора индексов.
/// В регрессии это среднее целевой переменной
/// с учетом L2-регуляризации (если leafL2 > 0).
    function LeafValue(y: Vector; indices: array of integer): real; override;
    
    function FindBestSplitReg(X: Matrix; y: Vector; indices: array of integer): SplitResult;
    
/// Ищет лучшее разбиение узла по всем признакам и возможным порогам.
/// Критерий — максимальное уменьшение дисперсии.
    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; override;
/// Проверяет, является ли узел "чистым".
/// Для регрессии это означает, что все значения y одинаковы
/// или разбиение больше не имеет смысла.
    function IsPure(y: Vector; indices: array of integer): boolean; override;
    
  public
/// Создает регрессионное дерево:
///   • maxDepth — максимальная глубина (-1 означает без ограничения).
///   • minSamplesSplit — минимальное число объектов для разбиения.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • leafL2 — коэффициент L2-регуляризации значения листа
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1;
      leafL2: real := 0.0; seed: integer := -1);
    
/// Обучает регрессионное дерево.
/// X — матрица признаков.
/// y — вещественная целевая переменная.
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;

/// Выполняет предсказание для всех объектов X.
/// Возвращает вектор вещественных значений.
    function Predict(X: Matrix): Vector; override;

/// Создает глубокую копию дерева регрессии.
/// Копируется структура узлов, параметры и обученное состояние.
    function Clone: IModel; override;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function Name: string := Self.GetType.Name;
  end;
  

/// Режим выбора числа признаков при поиске разбиения.
/// Определяет, сколько признаков m из общего числа p
/// будет случайно выбрано для рассмотрения в узле.
/// Используется в Random Forest и других ансамблях
/// для увеличения разнообразия деревьев
  TMaxFeaturesMode = (
/// Использовать все признаки: m = p
    AllFeatures,      
/// Использовать квадратный корень от числа признаков: m = sqrt(p)
/// Типичный выбор для классификации
    SqrtFeatures,
/// Использовать log2 от числа признаков: m = log2(p)
/// Более агрессивное ограничение признаков
    Log2Features,
/// Использовать половину признаков: m = p / 2
/// Компромисс между скоростью и разнообразием
    HalfFeatures     
  );
  
//============================  
//   RandomForestRegressor  
//============================  
// RandomForest = bagging + feature-subset
//   bagging = Делаем bootstrap-выборку (случайно с возвращением)
//   feature-subset = в каждом дереве - случайные признаки
//   RandomForest снижает корреляцию между деревьями, что хорошо

/// Базовый абстрактный класс случайного леса.
/// Реализует ансамбль из множества независимых деревьев,
///   обученных на случайных подвыборках данных и случайных подмножествах признаков.
/// Конкретная логика агрегирования предсказаний
///   определяется в наследниках (регрессия или классификация)
  RandomForestBase = abstract class(IModel)
  protected
    fNTrees: integer;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fMaxFeaturesMode: TMaxFeaturesMode;
    fFitted: boolean;
    
    fRandomSeed: integer;
    fRng: System.Random;
    fUserProvidedSeed: boolean;
    
    fFeatureCount: integer;
    
    fUseOOB: boolean;
    fOOBScore: real;
    fHasOOBScore: boolean;
  
    function ComputeMaxFeatures(p: integer): integer;
    
    procedure BootstrapRowIndices(n: integer; var rows: array of integer);

  public
/// Создает случайный лес.
///   • nTrees — число деревьев в ансамбле.
///   • maxDepth — максимальная глубина каждого дерева.
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • maxFeatures — режим выбора числа признаков, рассматриваемых при поиске разбиения
    constructor Create(
      nTrees: integer;
      maxDepth: integer;
      minSamplesSplit: integer;
      minSamplesLeaf: integer;
      maxFeatures: TMaxFeaturesMode;
      useOOB: boolean := False;
      seed: integer := -1);

/// Обучает ансамбль деревьев на данных X и y.
/// Для каждого дерева используется bootstrap-подвыборка
/// и случайное подмножество признаков.  
    function Fit(X: Matrix; y: Vector): ISupervisedModel; virtual; abstract;

/// Выполняет предсказание для матрицы X.
/// В регрессии — усреднение предсказаний деревьев.
/// В классификации — голосование (majority vote) или усреднение вероятностей.    
    function Predict(X: Matrix): Vector; virtual; abstract;

/// Создает глубокую копию случайного леса.
/// Копируются все деревья и параметры ансамбля.
    function Clone: IModel; virtual; abstract;
    
/// Возвращает вектор важности признаков.
/// Обычно вычисляется как средняя важность по всем деревьям ансамбля.
    function FeatureImportances: Vector; virtual; abstract;
    
/// Возвращает Out-Of-Bag (OOB) оценку качества модели.
/// OOB-оценка вычисляется на объектах, не вошедших в bootstrap-выборки деревьев.
/// • Для регрессии — среднеквадратичная ошибка (MSE).
/// • Для классификации — доля правильно классифицированных объектов (Accuracy).
/// Требует включённого режима computeOOB при создании модели.
/// Бросает исключение, если OOB не включён или модель не обучена
    function OOBScore: real;
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;  
  
/// Случайный лес для задачи регрессии.
/// Строит ансамбль регрессионных деревьев,
/// обученных на bootstrap-подвыборках данных и случайных подмножествах признаков.
/// Итоговое предсказание — среднее значение по всем деревьям ансамбля
  RandomForestRegressor = class(RandomForestBase, IRegressor)
  private
    fTrees: array of DecisionTreeRegressor;
  public
/// Создает регрессионный случайный лес:
///   • nTrees — число деревьев в ансамбле.
///   • maxDepth — максимальная глубина деревьев (-1 означает без ограничения).
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • maxFeaturesMode — режим выбора числа признаков, рассматриваемых при поиске разбиения
    constructor Create(nTrees: integer := 100; 
      maxDepth: integer := -1;
      minSamplesSplit: integer := 2; 
      minSamplesLeaf: integer := 1;
      maxFeaturesMode: TMaxFeaturesMode := TMaxFeaturesMode.HalfFeatures;
      computeOOB: boolean := False;
      seed: integer := -1);

/// Обучает случайный лес на данных X и y.
/// Для каждого дерева используется bootstrap-выборка.
/// Возвращает обученную модель.
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;

/// Выполняет предсказание для X.
/// Итоговое значение — среднее предсказаний всех деревьев ансамбля.
    function Predict(X: Matrix): Vector; override;

/// Создает глубокую копию случайного леса.
/// Копируются все деревья и параметры модели.
    function Clone: IModel; override;
    
/// Возвращает усредненную важность признаков по всем деревьям ансамбля.
/// Значения нормированы так, что сумма равна 1.
    function FeatureImportances: Vector; override;

/// Возвращает строковое представление модели.
    function ToString: string; override;
  end;

/// Случайный лес для задачи классификации.
/// Наследуется от RandomForestBase и реализует интерфейс IClassifier.
/// Строит ансамбль классификационных деревьев, обученных на  
///   bootstrap-подвыборках объектов и случайных подмножествах признаков.
/// Итоговое предсказание формируется голосованием деревьев или агрегацией вероятностей классов
  RandomForestClassifier = class(RandomForestBase, IProbabilisticClassifier)
  private
    fTrees: array of DecisionTreeClassifier;
    fIndexToClass: array of integer;
    fClassToIndex: Dictionary<integer, integer>;
    fClassCount: integer;
    fClassLabels: array of string;
  public
/// Создает классификационный случайный лес:
///   • nTrees — число деревьев в ансамбле.
///   • maxDepth — максимальная глубина каждого дерева (-1 означает без ограничения).
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • maxFeaturesMode — режим выбора числа признаков при поиске разбиения, по умолчанию используется sqrt(p), 
///     что является стандартом для классификации
    constructor Create(nTrees: integer := 100; 
      maxDepth: integer := -1;
      minSamplesSplit: integer := 2;
      minSamplesLeaf: integer := 1;
      maxFeaturesMode: TMaxFeaturesMode := TMaxFeaturesMode.SqrtFeatures;
      computeOOB: boolean := False;
      seed: integer := -1);
  
/// Обучает случайный лес на данных X и y.
/// Для каждого дерева используется bootstrap-выборка обучающих объектов.
/// В каждом узле рассматривается случайное подмножество признаков согласно maxFeaturesMode.
/// Возвращает обученную модель.
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;

/// Выполняет предсказание меток классов для матрицы X.
/// Для каждого объекта агрегируются предсказания всех деревьев.
/// Итоговый класс определяется большинством голосов или максимальной суммарной вероятностью.
    function Predict(X: Matrix): Vector; override;
    
    /// Возвращает предсказанные метки классов для объектов из X.
    /// Каждый элемент результата — индекс класса (целое число).
    /// Порядок элементов соответствует строкам матрицы X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;
    
    function PredictProba(X: Matrix): Matrix;

/// Создает глубокую копию случайного леса классификации.
/// Копируются все деревья, параметры ансамбля и обученное состояние модели.
    function Clone: IModel; override;
    
/// Возвращает усредненную важность признаков по всем деревьям ансамбля.
/// Важность рассчитывается как суммарное уменьшение нечистоты, нормированное так, что сумма равна 1.
    function FeatureImportances: Vector; override;

/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function GetClasses: array of real;
    
    function Name: string := Self.GetType.Name;
    
    procedure SetClassLabels(classes: array of string);
    
    function GetClassLabels: array of string;
  end;
  
{ Gradient Boosting v1.0 — Freeze Checklist
  GradientBoostingRegressor
✔ SquaredError
✔ Huber
✔ Quantile
✔ L2 regularization в листьях
✔ Subsample (stochastic boosting)
✔ Validation early stopping
✔ OOB early stopping
✔ Train / Val / OOB history
✔ Staged prediction
✔ Feature importance
✔ Clone

  GradientBoostingClassifier
✔ Multiclass softmax
✔ Корректное class mapping
✔ LogLoss
✔ Subsample
✔ Validation early stopping
✔ OOB early stopping
✔ History
✔ Staged prediction
✔ Feature importance
✔ Clone
}

/// Тип функции потерь для GradientBoostingRegressor.
/// SquaredError — классическая L2 (MSE).
/// Huber — робастная loss: квадратичная около нуля и линейная на хвостах.
/// Quantile — квантильная регрессия (асимметричная L1).    
  TGBLoss = (SquaredError, Huber, Quantile);
  
/// Gradient Boosting Regressor.
/// Реализует градиентный бустинг над деревьями решений.
/// Каждая новая модель обучается на псевдо-остатках предыдущей.
/// Поддерживает: 
///   • разные loss-функции, 
///   • subsample (stochastic boosting),
///   • early stopping (validation или OOB),
///   • L2-регуляризацию в листьях 
///   • staged prediction
  GradientBoostingRegressor = class(IRegressor)
  private
    fNEstimators: integer;
    fLearningRate: real;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fSubsample: real;

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
    fBestScoreLoss: real;
    
    fFeatureImportances: Vector;
    
    fQuantileAlpha: real;
    
    fLeafL2: real;
    fUseOOBEarlyStopping: boolean;
    
    fOOBLossHistory: List<real>;
    
    fRandomSeed: integer;
    fRng: System.Random;
    fUserProvidedSeed: boolean;
    
    function ComputeTrainLoss(y, yPred: Vector): real;
    procedure ComputePseudoResiduals(y, yPred: Vector; r: Vector);
    
    function FitInternal(XTrain: Matrix; yTrain: Vector;
      XVal: Matrix; yVal: Vector; useValidation: boolean): ISupervisedModel;
      
    function ComputeQuantile(y: Vector; alpha: real): real;

    function ComputeTrainLossMasked(yTrue, yPred: Vector; mask: array of boolean): real;
  
  public
/// Создает новый GradientBoostingRegressor:
///   • nEstimators — число деревьев (итераций бустинга).
///   • learningRate — коэффициент shrinkage.
///   • maxDepth — максимальная глубина дерева.
///   • minSamplesSplit — минимальное число объектов для split.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • subsample — доля выборки на каждой итерации (0..1].
///   • randomSeed — зерно генератора случайных чисел.
///   • loss — функция потерь.
///   • huberDelta — параметр Huber loss.
///   • earlyStoppingPatience — число итераций без улучшения до остановки.
///   • quantileAlpha — уровень квантили для Quantile loss.
///   • leafL2 — L2-регуляризация значения листа.
///   • useOOBEarlyStopping — использовать OOB loss для ранней остановки
    constructor Create(
      nEstimators: integer := 100;
      learningRate: real := 0.1;
      maxDepth: integer := 3;
      minSamplesSplit: integer := 2;
      minSamplesLeaf: integer := 1;
      subsample: real := 1.0;
      loss: TGBLoss := TGBLoss.SquaredError;
      huberDelta: real := 1.0;
      earlyStoppingPatience: integer := 0;
      quantileAlpha: real := 0.5;
      leafL2: real := 0.0;
      useOOBEarlyStopping: boolean := false; 
      seed: integer := -1
      );

/// Обучает модель на всей обучающей выборке.
/// Если включен early stopping и subsample < 1, может использоваться OOB loss.
/// Возвращает обученную модель.
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
/// Предсказывает значения целевой переменной.
/// Используются все обученные деревья.
    function Predict(X: Matrix): Vector;
/// Создает глубокую копию модели.
/// Копируются все деревья и внутреннее состояние.
    function Clone: IModel;
    
/// Обучает модель с использованием отдельной validation-выборки.
/// Early stopping (если включен) происходит по validation loss.
    function FitWithValidation(XTrain: Matrix; yTrain: Vector;
      XVal: Matrix; yVal: Vector): IModel;

/// История значения функции потерь на обучающей выборке.
/// Один элемент на итерацию бустинга.    
    property TrainLossHistory: List<real> read fTrainLossHistory;
/// История значения функции потерь на validation-выборке.
    property ValLossHistory: List<real> read fValLossHistory;
/// Индекс итерации с лучшим значением функции потерь
/// (validation или OOB в зависимости от режима).
    property BestIteration: integer read fBestIteration;
/// История OOB loss (если включен OOB early stopping).
    property OOBLossHistory: List<real> read fOOBLossHistory;
    
/// Предсказание по первым m итерациям бустинга.
/// m может быть от 0 до TreeCount.
/// Используется для анализа обучения и переобучения.
    function PredictStage(X: Matrix; m: integer): Vector;
/// Возвращает последовательность предсказаний
/// после каждой итерации бустинга.
/// Удобно для построения learning curve.
    function StagedPredict(X: Matrix): sequence of Vector;
    
/// Возвращает текущее количество деревьев в ансамбле.
    function TreeCount: integer := fEstimators.Count;

/// Возвращает нормированные importance признаков.
/// Значения суммируются по всем деревьям и нормируются к 1.    
    function FeatureImportances: Vector;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;

/// Тип функции потерь для классификатора.
/// В текущей версии используется только LogLoss
/// (многоклассовая кросс-энтропия).
  TGBCLoss = (LogLoss);
  
/// Gradient Boosting Classifier.
/// Реализует многоклассовый градиентный бустинг с использованием softmax и LogLoss.
/// На каждой итерации обучается по одному дереву для каждого класса (one-vs-all в логит-пространстве).
/// Поддерживает:
///   • subsample 
///   • validation early stopping 
///   • OOB early stopping
///   • staged prediction
  GradientBoostingClassifier = class(IProbabilisticClassifier)
  private
    // hyperparams
    fNEstimators: integer; 
    fLearningRate: real;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fSubsample: real;
    fEarlyStoppingPatience: integer;

    // fitted state
    fFitted: boolean;
    fFeatureCount: integer;

    fClassCount: integer;
    fClasses: array of integer;          // реальные метки классов (unique)
    fClassIndex: Dictionary<integer,integer>; // label -> 0..K-1

    // [m][k] дерево для класса k на итерации m
    fEstimators: List<array of DecisionTreeRegressor>;

    // init logits (как минимум нули; позже можно log-prior)
    fInitLogits: array of real; // длины K

    // diagnostics
    fTrainLossHistory: List<real>;
    fValLossHistory: List<real>;
    fBestIteration: integer;
    fBestScoreLoss: real;
    
    fFeatureImportances: Vector;
    
    fOOBLossHistory: List<real>;
    
    fRandomSeed: integer;
    fRng: System.Random;
    fUserProvidedSeed: boolean;
    
    fClassLabels: array of string;

  private
    function FitInternal(XTrain: Matrix; yTrain: Vector; XVal: Matrix; yVal: Vector; useValidation: boolean)
      : ISupervisedModel;

    procedure BuildClassMapping(y: Vector);
    function ApplyLabelEncoding(y: Vector): array of integer;

    procedure SoftmaxRow(var logits: array of real; var probs: array of real);
    procedure SoftmaxMatrix(logits: Matrix; probs: Matrix);

    function ComputeLogLoss(yEncoded: array of integer; probs: Matrix): real;
    
    function ComputeLogLossMasked(yEncoded: array of integer; logits: Matrix;
      mask: array of boolean): real;
  
  public
/// Создает новый GradientBoostingClassifier:
///   • nEstimators — число итераций бустинга.
///   • learningRate — коэффициент shrinkage.
///   • maxDepth — максимальная глубина деревьев.
///   • minSamplesSplit — минимальное число объектов для split.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • subsample — доля обучающей выборки на каждой итерации.
///   • seed — зерно генератора случайных чисел.
///   • earlyStoppingPatience — число итераций без улучшения до ранней остановки
    constructor Create(
      nEstimators: integer := 200;
      learningRate: real := 0.05;
      maxDepth: integer := 3;
      minSamplesSplit: integer := 2;
      minSamplesLeaf: integer := 1;
      subsample: real := 1.0;
      earlyStoppingPatience: integer := 20;
      seed: integer := -1);

/// Обучает классификатор на всей обучающей выборке.
/// Если включен early stopping и subsample < 1, то может использоваться OOB loss.
/// Возвращает обученную модель.
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
    
/// Обучает классификатор с использованием validation-набора.
/// Если включен early stopping, он основан на validation loss.
    function FitWithValidation(XTrain: Matrix; yTrain: Vector; XVal: Matrix; yVal: Vector): IModel;

/// Предсказывает метки классов.
/// Возвращает исходные значения классов, а не внутренние индексы.
    function Predict(X: Matrix): Vector;
    
    function PredictLabels(X: Matrix): array of integer;

/// Возвращает вероятности принадлежности к каждому классу.
/// Размер результата: [nSamples x nClasses].
/// Вероятности получаются через softmax.
    function PredictProba(X: Matrix): Matrix;
    
    function GetClasses: array of real;

/// Создает глубокую копию классификатора.
/// Копируются все деревья и внутреннее состояние.
    function Clone: IModel;

/// История значения LogLoss на обучающей выборке.
/// Один элемент на итерацию бустинга.
    property TrainLossHistory: List<real> read fTrainLossHistory;
/// История LogLoss на validation-наборе.
    property ValLossHistory: List<real> read fValLossHistory;
/// Индекс итерации с лучшим значением функции потерь (validation или OOB).
    property BestIteration: integer read fBestIteration;
/// История OOB LogLoss (если включен OOB early stopping).
    property OOBLossHistory: List<real> read fOOBLossHistory;
    
/// Возвращает вероятности после первых m итераций бустинга.
/// m может быть от 0 до TreeCount.
/// Используется для анализа обучения.
    function PredictStageProba(X: Matrix; m: integer): Matrix;
    
/// Предсказывает классы по первым m итерациям бустинга.
/// Удобно для построения learning curve.
    function PredictStage(X: Matrix; m: integer): Vector;
    
/// Текущее количество деревьев в ансамбле.
    function TreeCount: integer := fEstimators.Count;

/// Нормированные importance признаков.
/// Рассчитываются как суммарное уменьшение impurity
///   по всем деревьям и нормируются к 1.
    function FeatureImportances: Vector;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
    
    procedure SetClassLabels(classes: array of string);

    function GetClassLabels: array of string;
  end;

//-----------------------------
//             KNN
//-----------------------------
  
  Neighbor = record
    dist: double;
    idx: integer;
  end;
 
/// Режим взвешивания в алгоритме k ближайших соседей.
/// Uniform — равномерное голосование/усреднение.
/// Distance — веса обратно пропорциональны расстоянию (1 / dist)
  KNNWeighting = (Uniform, Distance);
 
/// Базовый абстрактный класс для алгоритма k ближайших соседей (kNN).
/// Реализует общий механизм поиска k ближайших объектов,
/// но не определяет способ агрегации (классификация или регрессия)
  KNNBase = abstract class(IPredictiveModel)
  protected
    // ==== train state ====
    fXTrain: Matrix;
    fK: integer;
    fFitted: boolean;
    
    fWeighting: KNNWeighting;

    // ==== work buffers ====
    fNeighbors: array of Neighbor;

    // ==== common methods ====
    procedure ValidatePredictInput(X: Matrix);

    function SquaredL2(trainRow: integer; XTest: Matrix; testRow: integer): double;

    procedure QuickSelect(k: integer);
    function Partition(left, right: integer): integer;

  public
    /// Создаёт модель kNN.
    /// k — число ближайших соседей (k > 0).
    /// weighting — режим взвешивания соседей
    constructor Create(k: integer; weighting: KNNWeighting := KNNWeighting.Uniform);
    
    /// Обучает модель на матрице признаков X и целевом векторе y.
    /// Возвращает текущий экземпляр модели
    function Fit(X: Matrix; y: Vector): ISupervisedModel; virtual; abstract;
    
    /// Выполняет предсказание для матрицы признаков X.
    /// Возвращает вектор предсказанных значений или меток
    function Predict(X: Matrix): Vector; virtual; abstract;
    
    /// Создаёт глубокую копию модели
    function Clone: IModel; virtual; abstract;
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;
  
  /// Классификатор на основе алгоритма k ближайших соседей (kNN).
  /// Поддерживает равномерное (Uniform) и взвешенное по расстоянию (Distance) голосование.
  /// Реализует вероятностные предсказания через PredictProba
  /// ВАЖНО: KNN чувствителен к масштабу признаков.
  /// Всегда используйте StandardScaler или MinMaxScaler в Pipeline перед KNN.
  KNNClassifier = class(KNNBase, IProbabilisticClassifier)
  private
    // ==== classification state ====
    fYEnc: array of integer;
    fClasses: array of double;
    fClassCount: integer;
    fClassLabels: array of string;

    // ==== voting buffers ====
    fVotes: array of double;
    fMark: array of integer;
    fTouched: array of integer;
    fEpoch: integer;

    procedure EncodeClasses(y: Vector);

  public
    /// Создаёт классификатор kNN.
    /// k — число ближайших соседей (k > 0).
    /// weighting — режим взвешивания голосов соседей
    constructor Create(k: integer; weighting: KNNWeighting := KNNWeighting.Uniform);
    
    /// Обучает модель на матрице признаков X и векторе меток y.
    /// Метки классов могут быть произвольными числами.
    /// Возвращает текущий экземпляр модели
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;
    
    /// Выполняет предсказание меток классов для объектов X.
    /// Возвращает вектор предсказанных меток
    function Predict(X: Matrix): Vector; override;
    
    function PredictLabels(X: Matrix): array of integer;
    
    /// Возвращает матрицу вероятностей размера (nSamples × nClasses).
    /// Столбцы соответствуют классам в порядке, возвращаемом GetClasses()
    function PredictProba(X: Matrix): Matrix;
    
    /// Возвращает массив меток классов в порядке столбцов PredictProba
    function GetClasses: array of real;
    
    /// Создаёт глубокую копию классификатора
    function Clone: IModel; override;
    
    function Name: string := Self.GetType.Name;
    
    procedure SetClassLabels(classes: array of string);

    function GetClassLabels: array of string;
  end;
  
  
  /// Регрессор на основе алгоритма k ближайших соседей (kNN).
  /// Поддерживает равномерное (Uniform) усреднение и взвешенное по расстоянию (Distance) усреднение.
  /// Предсказание вычисляется как среднее (или взвешенное среднее) значений целевой переменной
  /// по k ближайшим обучающим объектам
  KNNRegressor = class(KNNBase, IRegressor)
  private
    fYTrain: Vector;

  public
    /// Создаёт регрессор kNN.
    /// k — число ближайших соседей (k > 0).
    /// weighting — режим взвешивания вкладов соседей
    constructor Create(k: integer; weighting: KNNWeighting := KNNWeighting.Uniform);
    
    /// Обучает модель на матрице признаков X и целевом векторе y.
    /// Возвращает текущий экземпляр модели
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;
    
    /// Выполняет предсказание числовых значений для объектов X.
    /// Возвращает вектор предсказанных значений
    function Predict(X: Matrix): Vector; override;
    
    /// Создаёт глубокую копию регрессора
    function Clone: IModel; override;
    
    function Name: string := Self.GetType.Name;
  end;
  
  
  /// Модель кластеризации методом k-средних (KMeans).
  /// Разбивает объекты на k кластеров на основе евклидова расстояния.
  /// Реализует алгоритм без учителя
  KMeans = class(IPredictiveClusterer)
  private
    fNClusters: integer;
    fMaxIter: integer;
    fTol: real;
    fNInit: integer;
    fSeed: integer;

    fFitted: boolean;
    fFeatureCount: integer;

    fCenters: Matrix;
    fInertia: real;
    fIterations: integer;
    fHasConverged: boolean;
    
    fRandomSeed: integer;
    fUserProvidedSeed: boolean;
    fRng: System.Random;
    
    function RunSingle(X: Matrix; rnd: System.Random): (Matrix, real, integer, boolean);
  public
    /// Создаёт модель KMeans.
    /// nClusters — количество кластеров.
    /// maxIter — максимальное число итераций.
    /// tol — порог сходимости.
    /// nInit — количество независимых запусков.
    /// seed — значение генератора случайных чисел
    constructor Create(
      nClusters: integer;
      maxIter: integer := 300;
      tol: real := 1e-4;
      nInit: integer := 10;
      seed: integer := -1
    );

    /// Обучает модель по матрице признаков X.
    /// Выполняет nInit запусков и выбирает решение с минимальной инерцией.
    function Fit(X: Matrix): IUnsupervisedModel;
    
    /// Возвращает индекс кластера для каждого объекта из X.
    /// Требует предварительного вызова Fit.
    function Predict(X: Matrix): Vector;
    
    /// Возвращает индекс кластера для каждого объекта из X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;

    /// Выполняет обучение и сразу возвращает метки кластеров.
    function FitPredict(X: Matrix): array of integer;

    /// Создаёт глубокую копию модели.
    function Clone: IModel;
    
    /// Количество кластеров
    function ClustersCount: integer;

    /// Количество кластеров.
    property NClusters: integer read fNClusters;
    /// Максимальное число итераций.
    property MaxIter: integer read fMaxIter;
    /// Порог сходимости.
    property Tol: real read fTol;
    /// Количество запусков алгоритма.
    property NInit: integer read fNInit;
    /// Используемый seed.
    property Seed: integer read fSeed;
    /// Матрица центроидов (k × p).
    property ClusterCenters: Matrix read fCenters;
    /// Значение инерции (сумма квадратов расстояний до центроидов).
    property Inertia: real read fInertia;
    /// Число выполненных итераций.
    property Iterations: integer read fIterations;
    /// Признак сходимости алгоритма.
    property HasConverged: boolean read fHasConverged;
    /// Число признаков модели.
    property FeatureCount: integer read fFeatureCount;
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;
  

  DBSCAN = class(IClusterer)
  private
    fEps: real;
    fMinSamples: integer;
  
    fFitted: boolean;
    fFeatureCount: integer;
  
    fLabels: array of integer;
    fClusterCount: integer;
  
    function RegionQuery(X: Matrix; i: integer; neighbors: List<integer>): integer;
  
  public
    /// Создаёт модель DBSCAN.
    /// eps — радиус соседства.
    /// minSamples — минимальное число точек в eps-окрестности.
    /// seed — параметр для совместимости API.
    constructor Create(
      eps: real;
      minSamples: integer := 5
    );
  
    /// Обучает модель на матрице признаков.
    function Fit(X: Matrix): IUnsupervisedModel;
  
    /// Возвращает метки кластеров.
    function PredictLabels(X: Matrix): array of integer;

    /// Возвращает метки после обучения.
    function FitPredict(X: Matrix): array of integer;
    
    /// Копия модели.
    function Clone: IModel;
  
    property Labels: array of integer read fLabels;
    
    /// Количество кластеров, найденное моделью
    function ClustersCount: integer; 
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;  
  
{$endregion Models}

{$region Pipeline}
/// Последовательный конвейер машинного обучения (supervised).
/// Гарантирует строгий порядок выполнения шагов:
///   [преобразователи] → [модель].
///
/// Поддерживает:
///   • преобразователи без учёта целевой переменной (unsupervised);
///   • преобразователи с учётом целевой переменной (supervised);
///   • одну финальную модель с учителем.
///
/// Все преобразователи применяются последовательно к признакам X,
/// после чего финальная модель обучается на (X, y).
///
/// Обеспечивает единый интерфейс Fit(X, y) / Predict(X)
/// и воспроизводимость полного процесса обучения
  Pipeline = class(ISupervisedModel)
  private
    fTransformers: List<ITransformer>;
    fModel: ISupervisedModel;
    fFitted: boolean;
  public
    /// Создаёт конвейер машинного обучения для заданной модели:
    ///   • model — модель, которая будет обучена
    ///     после последовательного применения всех преобразователей.
    constructor Create(model: ISupervisedModel);
    
    /// Создаёт пустой пайплайн (конвейер машинного обучения).
    /// Модель должна быть установлена через SetModel.
    constructor Create;
    
{
    // Pipeline.Build используется, когда данные уже представлены
    // в виде числовой матрицы признаков X и вектора целевой переменной y.
    // В этом случае DataFrame и препроцессоры уровня таблицы не требуются.
    //
    // Типичные ситуации:
    //  • экспериментирование с ML-алгоритмами
    //  • сравнение моделей
    //  • кросс-валидация
    //  • подбор гиперпараметров
    //  • тестирование моделей
    //
    // Pipeline объединяет несколько матричных преобразований (ITransformer)
    // и модель (IModel) в единый объект, который можно обучать и использовать
    // для предсказаний.
    //
    // В отличие от этого, DataPipeline.Build используется,
    // когда исходные данные представлены в виде DataFrame
    // и требуется выполнить препроцессинг таблицы
    // (Imputer, OneHotEncoder, LabelEncoder и др.)
    // перед преобразованием данных в Matrix/Vector.
    
    var pipe1 :=
      Pipeline.Build(
        new StandardScaler,
        new LogisticRegression
      );
    
    var pipe2 :=
      Pipeline.Build(
        new StandardScaler,
        new PCATransformer(2),
        new LogisticRegression
      );
    
    var score1 :=
      Validation.CrossValidate(
        pipe1,
        X,
        y,
        5,
        Metrics.Accuracy
      );
    
    var score2 :=
      Validation.CrossValidate(
        pipe2,
        X,
        y,
        5,
        Metrics.Accuracy
      );
    
    Println('Scaler + LogisticRegression = ', score1);
    Println('Scaler + PCA + LogisticRegression = ', score2);
}    

    /// Строит конвейер машинного обучения из последовательности шагов.
    /// Шаги указываются в порядке выполнения:
    ///   сначала преобразователи, затем модель.
    /// Последний шаг обязан быть моделью (IModel).
    /// Возвращает сконструированный конвейер.
    static function Build(params steps: array of IPipelineStep): Pipeline;
    
    /// Устанавливает или заменяет модель.
    function SetModel(m: ISupervisedModel): Pipeline;
  
    /// Добавляет преобразование в конец пайплайна
    function Add(t: ITransformer): Pipeline;
  
    /// Обучает преобразования и модель
    /// Модель может быть только supervised а трансформеры - любыми
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  
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
    
    function Name: string := Self.GetType.Name;
  end;
  
  UPipeline = class(IUnsupervisedModel)
  private
    fTransformers: List<ITransformer>;
    fModel: IModel;
    fFitted: boolean;
  public
    constructor Create(model: IModel);
    constructor Create;
  
    static function Build(params steps: array of IPipelineStep): UPipeline;
  
    function SetModel(m: IModel): UPipeline;
    function Add(t: ITransformer): UPipeline;
  
    function Fit(X: Matrix): IUnsupervisedModel;
    function Transform(X: Matrix): Matrix;
    function Predict(X: Matrix): Vector;
  
    property IsFitted: boolean read fFitted;
  
    function ToString: string; override;
    function Clone: IModel;
    function Name: string := Self.GetType.Name;
  end;
  
{$endregion Pipeline}
  
{$region Transformers}

/// Стандартизирует признаки: вычитает среднее
///   и делит на стандартное отклонение по каждому столбцу.
/// Используется для приведения признаков к сопоставимому масштабу
  StandardScaler = class(IUnsupervisedTransformer)
  private
    fMean: Vector;
    fStd: Vector;
    fFeatureCount: integer;
    fFitted: boolean;
  public
    /// Создаёт StandardScaler.
    /// Параметры масштабирования (среднее и стандартное отклонение)
    ///   вычисляются при вызове Fit.
    constructor Create(); begin end;
    /// Вычисляет среднее и стандартное отклонение по каждому признаку.
    function Fit(X: Matrix): IUnsupervisedTransformer;
  
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
///   (по умолчанию [0, 1]) на основе минимального и максимального значения каждого столбца.
/// Используется для приведения признаков к единому масштабу без центрирования
  MinMaxScaler = class(IUnsupervisedTransformer)
  private
    fMin: Vector;
    fMax: Vector;
    fFeatureCount: integer;
    fFitted: boolean;
    fRangeMin: real;
    fRangeMax: real;
  public
    /// Создаёт MinMaxScaler с диапазоном [rangeMin, rangeMax].
    /// По умолчанию масштабирование выполняется к [0, 1]
    constructor Create(rangeMin: real := 0.0; rangeMax: real := 1.0);
    /// Вычисляет минимальные и максимальные значения по каждому признаку.
    function Fit(X: Matrix): IUnsupervisedTransformer;
  
    /// Применяет линейное масштабирование признаков к диапазону [0, 1].
    function Transform(X: Matrix): Matrix;
  
    /// Минимальные значения признаков, вычисленные при обучении.
    property Min: Vector read fMin;
  
    /// Максимальные значения признаков, вычисленные при обучении.
    property Max: Vector read fMax;
  
    /// Признак того, что преобразование обучено.
    property IsFitted: boolean read fFitted;

    function ToString: string; override;
    
    function Clone: ITransformer;
  end;
  
  /// Трансформер главных компонент (PCA).
  /// Выполняет уменьшение размерности путём проекции данных
  ///   на первые k главных компонент.
  /// На этапе Fit вычисляет главные компоненты ковариационной матрицы.
  /// На этапе Transform проецирует данные:
  ///     Z = (X - μ) · W
  PCATransformer = class(IUnsupervisedTransformer)
  private
    fK: integer;
    fComponents: Matrix;   // W
    fMean: Vector;         // μ
    fFeatureCount: integer;
    fFitted: boolean;
  public
    /// Создаёт PCA-трансформер:
    ///   • k — число главных компонент (k > 0).
    constructor Create(k: integer);
  
    /// Обучает трансформер на матрице признаков X.
    ///   X — матрица m × n.
    function Fit(X: Matrix): IUnsupervisedTransformer;
  
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
/// Не использует целевую переменную (unsupervised)
  VarianceThreshold = class(IUnsupervisedTransformer)
  private
    fThreshold: real;
    fSelected: array of integer;
    fFeatureCount: integer;
    fFitted: boolean;
  
  public
    /// Создаёт VarianceThreshold:
    ///   • threshold — минимально допустимая дисперсия признака.
    /// Столбцы с Var(X_j) < threshold удаляются.
    constructor Create(threshold: real := 0.0);
  
    /// Вычисляет дисперсии признаков и запоминает индексы
    /// признаков, удовлетворяющих порогу.
    function Fit(X: Matrix): IUnsupervisedTransformer;
  
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
  /// Определяет способ вычисления значимости признака относительно целевой переменной
  FeatureScore = (
    /// Абсолютное значение коэффициента корреляции Пирсона
    /// между признаком и целевой переменной.
    /// Подходит для задач регрессии и бинарной классификации при линейной зависимости
    Correlation,
    
    /// F-статистика линейной регрессии (FRegression).
    /// Оценивает статистическую значимость линейной связи
    ///   между признаком и целевой переменной.
    /// Основан на коэффициенте детерминации (R²) и F-статистике.
    /// Более строгий критерий, чем простая корреляция
    FRegression,
    
    /// ANOVA F-критерий.
    /// Используется в задачах классификации.
    /// Оценивает различие средних значений признака между различными классами
    AnovaF,
    
    /// Хи-квадрат (Chi-Square) критерий независимости.
    /// Используется в задачах классификации.
    /// Оценивает зависимость между признаком и классом
    ///   на основе различия наблюдаемых и ожидаемых частот.
    /// Предполагает, что значения признака неотрицательны.
    /// Часто применяется для текстовых данных и частотных представлений (bag-of-words)
    ChiSquare
  );
  
/// Преобразователь с учётом целевой переменной
/// Для каждого признака вычисляет score(X_j, y)
///   и оставляет k признаков с наибольшим значением score.
/// Может использовать встроенные критерии или пользовательскую функцию оценки
  SelectKBest = class(ISupervisedTransformer)
  private
    fK: integer;
    fScoreType: FeatureScore;
    fScoreFunc: (Vector, Vector) -> real;
    fSelected: array of integer;
    fFeatureCount: integer;
    fFitted: boolean;
    
    function ComputeScore(feature: Vector; y: Vector): real;
    function ComputeCorrelation(x: Vector; y: Vector): real;
    function ComputeFRegression(feature: Vector; y: Vector): real;
    // Multiclass version
    function ComputeAnovaF(feature: Vector; y: Vector): real;
    // Multiclass version
    function ComputeChiSquare(feature: Vector; y: Vector): real;
  public
    /// Создаёт трансформер SelectKBest с использованием встроенного критерия:
    ///   • k — число отбираемых признаков.
    ///   • score — тип критерия (например, Correlation)
    constructor Create(k: integer; score: FeatureScore := FeatureScore.Correlation);
  
    /// Создаёт трансформер SelectKBest с пользовательской функцией оценки:
    ///   • scoreFunc — функция (feature, y) → real
    constructor Create(k: integer; scoreFunc: (Vector, Vector) -> real);

    /// Вычисляет оценки признаков и выбирает k лучших.
    function Fit(X: Matrix; y: Vector): ISupervisedTransformer;
  
    /// Возвращает матрицу, содержащую только выбранные признаки.
    function Transform(X: Matrix): Matrix;
  
    /// Индексы выбранных признаков.
    property SelectedFeatures: array of integer read fSelected;
  
    /// Показывает, был ли выполнен Fit.
    property IsFitted: boolean read fFitted;

/// Возвращает строковое представление трансформера
    function ToString: string; override;

/// Создает глубокую копию.
/// Копируются параметры и внутреннее состояние.
    function Clone: ITransformer;
  end;
  
  /// Тип нормы для нормализации строк
  NormType = (L1, L2);
  
  /// Преобразователь нормализации по строкам.
  /// Для каждой строки X_i выполняет нормализацию:
  ///   L1:  x := x / ‖x‖₁
  ///   L2:  x := x / ‖x‖₂
  /// Используется перед моделями, чувствительными к масштабу
  /// (LogisticRegression, SVM, L1-регуляризация)
  Normalizer = class(IUnsupervisedTransformer)
  private
    fNormType: NormType;
    fFeatureCount: integer;
    fFitted: boolean;
  public
/// Создает нормализатор:
///   • norm — тип нормы, используемой для масштабирования строки признаков.
/// По умолчанию используется L2-норма.
    constructor Create(norm: NormType := NormType.L2);
  
/// Подготавливает трансформер к работе.
/// Для Normalizer этап обучения может быть формальным, так как параметры не накапливаются.
    function Fit(X: Matrix): IUnsupervisedTransformer;

/// Применяет нормализацию к матрице X.
/// Каждая строка масштабируется так, чтобы ее норма соответствовала выбранному типу.
/// Возвращает новую матрицу с нормализованными объектами.
    function Transform(X: Matrix): Matrix;
  
/// Показывает, был ли вызван метод Fit.
/// Если False, вызов Transform может привести к ошибке.
    property IsFitted: boolean read fFitted;

/// Возвращает строковое представление трансформера
    function ToString: string; override;
    
/// Создает глубокую копию нормализатора.
/// Копируются параметры и внутреннее состояние.
    function Clone: ITransformer;
  end;
  
{$endregion Transformers}


{$region Utility functions}
/// Преобразует вектор меток классов в массив целых.
/// Предполагается, что значения y являются целыми
/// (0,1,2,...) и могут содержать небольшие
/// численные ошибки, поэтому используется Round.
function LabelsToInts(y: Vector): array of integer;

{$endregion Utility functions}

  
var 
  /// Проверять ли входные данные моделей на NaN, Inf 
  ValidateFiniteInputs := True;  
  
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
  ER_LABELS_NOT_INTEGER =
    'Метки классов должны быть целыми!!Class labels must be integers'; 
  ER_N_ESTIMATORS_INVALID =
    'nEstimators должен быть > 0!!nEstimators must be > 0';
  ER_LEARNING_RATE_INVALID =
    'learningRate должен быть > 0!!learningRate must be > 0';
  ER_MAX_DEPTH_INVALID =
    'maxDepth должен быть > 0!!maxDepth must be > 0';
  ER_SUBSAMPLE_INVALID =
    'subsample должен быть > 0!!subsample must be > 0'; 
  ER_NAN_IN_Y =
    'y содержит NaN!!y contains NaN';  
  ER_NAN_IN_X =  
    'x содержит NaN!!x contains NaN';  
  ER_K_EXCEEDS_SAMPLES =
    'k превышает число обучающих объектов!!k exceeds number of training samples';
  ER_MAX_DEPTH_TOO_LARGE =
    'maxDepth слишком велик {0}. Возможен переполнение стека!!' +
    'maxDepth is too large {0}. It may cause stack overflow';
  ER_LOGISTIC_DIVERGED =
    'Обучение LogisticRegression расходится. Уменьшите learningRate.' +
    'LogisticRegression training diverged. Reduce learningRate.';
  ER_LOGISTIC_SOFTMAX_ZERO =
    'Softmax дал нулевую сумму экспонент. Уменьшите learningRate.' +
    'Softmax produced zero sum of exponentials. Reduce learningRate.';
  ER_LOGISTIC_INVALID_LOSS =
    'Функция потерь стала NaN или Infinity. Обучение прервано.' +
    'Loss became NaN or Infinity. Training stopped.';
  ER_ELASTICNET_DIVERGED =
    'ElasticNet расходится. Увеличьте регуляризацию или уменьшите tol.' +
    'ElasticNet diverged. Increase regularization or reduce tol.';
  ER_ELASTICNET_INVALID_LOSS =
    'Функция потерь ElasticNet стала NaN или Infinity.' +
    'ElasticNet loss became NaN or Infinity.';
  ER_TRAINING_DATA_CONTAINS_NAN =
    'Обучающие данные содержат NaN. Обработайте пропуски до вызова Fit.!!' +
    'Training data contains NaN. Please handle missing values before calling Fit.';
  ER_TRAINING_DATA_CONTAINS_INF =
    'Обучающие данные содержат Infinity. Проверьте данные на выбросы.!!' +
    'Training data contains Infinity. Please check for extreme values.';
  ER_PREDICTION_DATA_CONTAINS_NAN =
    'Данные для предсказания содержат NaN. Обработайте пропуски до вызова Predict.!!' +
    'Prediction data contains NaN. Please handle missing values before calling Predict.';
  ER_PREDICTION_DATA_CONTAINS_INF =
    'Данные для предсказания содержат Infinity. Проверьте данные на выбросы.!!' +
    'Prediction data contains Infinity. Please check for extreme values.';  
  ER_MAXITER_INVALID =
    'maxIter должен быть положительным. Получено {0}.!!' +
    'maxIter must be positive. Received {0}.';
  ER_TOL_INVALID =
    'tol должен быть положительным. Получено {0}.!!' +
    'tol must be positive. Received {0}.';
  ER_EPOCHS_INVALID =
    'epochs должен быть положительным. Получено {0}.!!' +
    'epochs must be positive. Received {0}.';
  ER_MINSAMPLESSPLIT_INVALID =
    'minSamplesSplit должен быть ≥ 2. Получено {0}.!!' +
    'minSamplesSplit must be ≥ 2. Received {0}.';
  ER_MINSAMPLESLEAF_INVALID =
    'minSamplesLeaf должен быть ≥ 1. Получено {0}.!!' +
    'minSamplesLeaf must be ≥ 1. Received {0}.';
  ER_L2_NEGATIVE =
    'L2-регуляризация листа не может быть отрицательной. Получено {0}.!!' +
    'Leaf L2 regularization cannot be negative. Received {0}.';
  ER_NTREES_INVALID =
    'Число деревьев должно быть положительным. Получено {0}.!!' +
    'Number of trees must be positive. Received {0}.';
  ER_MODEL_NOT_INITIALIZED =
    'Модель не инициализирована или не содержит деревьев.!!' +
    'Model is not initialized or contains no trees.';
  ER_NESTIMATORS_INVALID =
    'nEstimators должен быть положительным. Получено {0}.!!' +
    'nEstimators must be positive. Received {0}.';
  ER_STAGE_OUT_OF_RANGE =
    'Stage m должен быть в диапазоне [0, {1}]. Получено {0}.!!' +
    'Stage m must be in range [0, {1}]. Received {0}.';
  ER_PIPELINE_STEP_NULL =
    'Шаг Pipeline с индексом {0} равен nil.!!' +
    'Pipeline step at index {0} is nil.';
  ER_PIPELINE_TRANSFORM_RETURNED_NULL =
    'Шаг Pipeline вернул nil из Transform. Проверьте реализацию трансформера.!!' +
    'Pipeline step returned nil from Transform. Check transformer implementation.';
  ER_K_INVALID =
    'Параметр k должен быть положительным: {0}.!!' +
    'Parameter k must be positive: {0}.';  
  ER_THRESHOLD_INVALID =
    'Порог дисперсии должен быть >= 0: {0}.!!' +
    'Variance threshold must be >= 0: {0}.';
  ER_ALL_FEATURES_REMOVED =
    'Все признаки удалены VarianceThreshold. Уменьшите порог.!!' +
    'All features were removed by VarianceThreshold. Decrease the threshold.';    
  ER_TRAINING_DIVERGED =
    'Обучение разошлось. Попробуйте уменьшить learning rate.!!' +
    'Training diverged. Try reducing the learning rate.';  
  ER_LABEL_INDEX_INVALID =
    'Некорректный индекс класса при вычислении loss.!!' +
    'Invalid class index during loss computation.';
  ER_SOFTMAX_EMPTY =
    'Softmax получил пустой вектор.!!' +
    'Softmax received empty vector.';
  ER_LOGISTIC_NEED_AT_LEAST_TWO_CLASSES =
    'Для LogisticRegression нужно минимум 2 класса.!!' +
    'LogisticRegression requires at least 2 classes.';
  ER_MIN_SAMPLES_SPLIT_INVALID =
    'minSamplesSplit должно быть >= 2 ({0}).!!' +
    'minSamplesSplit must be >= 2 ({0}).';
  ER_MIN_SAMPLES_LEAF_INVALID =
    'minSamplesLeaf должно быть >= 1 ({0}).!!' +
    'minSamplesLeaf must be >= 1 ({0}).';
  ER_HUBER_DELTA_INVALID =
    'huberDelta должно быть > 0 ({0}).!!' +
    'huberDelta must be > 0 ({0}).';
  ER_EARLY_STOPPING_INVALID =
    'earlyStoppingPatience должно быть >= 0 ({0}).!!' +
    'earlyStoppingPatience must be >= 0 ({0}).';
  ER_QUANTILE_ALPHA_INVALID =
    'quantileAlpha должно быть в (0,1) ({0}).!!' +
    'quantileAlpha must be in (0,1) ({0}).';
  ER_LEAFL2_INVALID =
    'leafL2 должно быть >= 0 ({0}).!!' +
    'leafL2 must be >= 0 ({0}).'; 
  ER_MIN_LEAF_GE_SPLIT =
    'minSamplesLeaf ({0}) должно быть меньше minSamplesSplit ({1}).!!' +
    'minSamplesLeaf ({0}) must be less than minSamplesSplit ({1}).';
  ER_OOB_NOT_ENABLED =
    'OOB score не включен для этой модели. Установите computeOOB = true в конструкторе.!!' +
    'OOB score is not enabled for this model. Set computeOOB = true in the constructor.';
  ER_NINIT_INVALID =
    'Некорректное значение nInit: {0}. Должно быть не меньше 1!!Invalid nInit value: {0}. Must be at least 1';    
  ER_INVALID_VALUE_AT =
    'Некорректное значение в {0} на позиции {1}!!Invalid value in {0} at index {1}';
  ER_EPS_INVALID =
    'eps должен быть положительным!!eps must be positive';
  ER_MINSAMPLES_INVALID =
    'minSamples должен быть >= 1!!minSamples must be >= 1';
  ER_PIPELINE_TRANSFORMER_NO_FIT =
    'Трансформер с индексом {0} (тип: {1}) не поддерживает Fit!!Transformer at index {0} (type: {1}) does not support Fit';    
  ER_Model_NoFit =
    'Модель (тип: {0}) не поддерживает Fit!!Model (type: {0}) does not support Fit';    
  ER_MODEL_NOT_UNSUPERVISED =
    'Модель (тип: {0}) не является моделью без учителя!!' +
    'Model (type: {0}) is not an unsupervised model';
  ER_DBSCAN_PREDICT_ONLY_TRAIN_DATA =
    'DBSCAN: Predict поддерживается только для обучающей выборки!!DBSCAN: Predict is only supported for training data';  
  ER_MODEL_NOT_FITTED =
    'Модель "{0}" не обучена. Сначала вызовите Fit()|Model "{0}" is not fitted. Call Fit() first';
  ER_DBSCAN_PREDICT_NEW_DATA =
    'DBSCAN не поддерживает предсказание для новых данных!!DBSCAN does not support prediction for new data';
  ER_CLASSES_NOT_AVAILABLE =
    'Метки классов недоступны. Убедитесь, что модель обучена и метки установлены!!Class labels are not available. Ensure the model is fitted and class labels are set';  
  ER_PIPELINE_LAST_NOT_SUPERVISED_MODEL = 
    'Последний шаг Pipeline должен быть supervised-моделью!!' +
    'Last Pipeline step must be a supervised model';
  ER_INTERNAL_INVALID_MODEL_CLONE =
    'Внутренняя ошибка: Clone модели вернул несовместимый тип!!' +
    'Internal error: model Clone returned incompatible type';
  ER_PREDICT_NOT_SUPPORTED =
    'Модель не поддерживает Predict для данного типа алгоритма!!' +
    'Model does not support Predict for this type of algorithm';  
    
  
{$endregion ErrConstants}  

//-----------------------------
//     Проверка на NuN/Inf
//-----------------------------

procedure CheckXForFit(X: Matrix);
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  for var i := 0 to X.RowCount - 1 do
  for var j := 0 to X.ColCount - 1 do
  begin
    var v := X[i,j];

    if double.IsNaN(v) then
      ArgumentError(ER_TRAINING_DATA_CONTAINS_NAN);

    if double.IsInfinity(v) then
      ArgumentError(ER_TRAINING_DATA_CONTAINS_INF);
  end;
end;

procedure CheckXForPredict(X: Matrix);
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  for var i := 0 to X.RowCount - 1 do
    for var j := 0 to X.ColCount - 1 do
    begin
      var v := X[i,j];

      if double.IsNaN(v) then
        ArgumentError(ER_PREDICTION_DATA_CONTAINS_NAN);

      if double.IsInfinity(v) then
        ArgumentError(ER_PREDICTION_DATA_CONTAINS_INF);
    end;
end;

procedure CheckYForFit(y: Vector);
begin
  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  for var i := 0 to y.Length - 1 do
  begin
    var v := y[i];

    if double.IsNaN(v) then
      ArgumentError(ER_TRAINING_DATA_CONTAINS_NAN);

    if double.IsInfinity(v) then
      ArgumentError(ER_TRAINING_DATA_CONTAINS_INF);
  end;
end;
  
//-----------------------------
//       LinearRegression
//-----------------------------

constructor LinearRegression.Create();
begin
  ffitted := false;
end;

function LinearRegression.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;
  
  var m := X.RowCount;

  if m = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if y.Length <> m then
    DimensionError(ER_DIM_MISMATCH, y.Length, m);

  var n := X.ColCount;

  // --- means
  var meanX := X.ColumnMeans;
  var meanY := y.Mean;

  // --- center X
  var Xc := new Matrix(m, n);
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      Xc[i,j] := X[i,j] - meanX[j];

  // --- center y
  var yc := new Vector(m);
  for var i := 0 to m - 1 do
    yc[i] := y[i] - meanY;

  // --- solve via QR
  fcoef := SolveLeastSquaresQR(Xc, yc);

  // --- intercept
  fintercept := meanY - meanX.Dot(fcoef);

  ffitted := true;
  Result := Self;
end;

function LinearRegression.Predict(X: Matrix): Vector;
begin
  if not ffitted then
    NotFittedError(ER_FIT_NOT_CALLED);
  
  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fcoef.Length then
    DimensionError(ER_DIM_MISMATCH, X.ColCount, fCoef.Length);

  Result := X * fcoef + fIntercept;
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
  var n := v.Length;
  var resultVec := new Vector(n);

  if n = 0 then
    exit(resultVec);

  var maxVal := v.Max;

  var sumExp := 0.0;

  for var i := 0 to n - 1 do
  begin
    var e := Exp(v[i] - maxVal);
    resultVec[i] := e;
    sumExp += e;
  end;

  if sumExp <= 0 then
  begin
    var uniform := 1.0 / n;
    for var i := 0 to n - 1 do
      resultVec[i] := uniform;
    exit(resultVec);
  end;

  for var i := 0 to n - 1 do
    resultVec[i] /= sumExp;

  Result := resultVec;
end;

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

function RidgeRegression.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

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

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fCoef.Length then
    DimensionError(ER_DIM_MISMATCH, X.ColCount, fCoef.Length);

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
    ArgumentOutOfRangeError(ER_LAMBDA_NEGATIVE);

  if maxIter <= 0 then
    ArgumentOutOfRangeError(ER_MAXITER_INVALID, maxIter);

  if tol <= 0 then
    ArgumentOutOfRangeError(ER_TOL_INVALID, tol);

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

function ElasticNet.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if (fLambda1 < 0) or (fLambda2 < 0) then
    ArgumentOutOfRangeError(ER_LAMBDA_NEGATIVE);

  if fMaxIter <= 0 then
    ArgumentOutOfRangeError(ER_MAXITER_INVALID, fMaxIter);

  if fTol <= 0 then
    ArgumentOutOfRangeError(ER_TOL_INVALID, fTol);

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
  var residual := yc.Clone;   // since β = 0

  var prevLoss := real.PositiveInfinity;

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

      var denom := zj + fLambda2;

      if denom > 0 then
      begin
        var newBeta := SoftThreshold(rho, fLambda1) / denom;
        var delta := newBeta - oldBeta;
      
        if delta <> 0 then
          for var i := 0 to n - 1 do
            residual[i] -= Xc[i,j] * delta;
      
        if Abs(delta) > maxChange then
          maxChange := Abs(delta);
      
        fCoef[j] := newBeta;
      end;
    end;

    // --- cheap divergence guard
    if double.IsNaN(maxChange) or double.IsInfinity(maxChange) then
      ArgumentError(ER_ELASTICNET_DIVERGED);

    // --- каждые 10 итераций проверяем loss
    if iter mod 10 = 0 then
    begin
      var rss := 0.0;
      for var i := 0 to n - 1 do
        rss += residual[i] * residual[i];

      var l1 := 0.0;
      var l2 := 0.0;

      for var j := 0 to p - 1 do
      begin
        l1 += Abs(fCoef[j]);
        l2 += fCoef[j] * fCoef[j];
      end;

      var currentLoss := rss / n + fLambda1*l1 + fLambda2*l2;

      if double.IsNaN(currentLoss) or double.IsInfinity(currentLoss) then
        ArgumentError(ER_ELASTICNET_INVALID_LOSS);

      if Abs(prevLoss - currentLoss) < fTol then
        break;

      prevLoss := currentLoss;
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

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fCoef.Length then
    DimensionError(ER_DIM_MISMATCH, X.ColCount, fCoef.Length);

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
//     LogisticRegression 
//-----------------------------

constructor LogisticRegression.Create(lambda: real; lr: real; epochs: integer);
begin
  fLambda := lambda;
  fLearningRate := lr;
  fEpochs := epochs;
  fFitted := false;
end;

function LogisticRegression.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if fLearningRate <= 0 then
    ArgumentOutOfRangeError(ER_LEARNING_RATE_INVALID, fLearningRate);

  if fEpochs <= 0 then
    ArgumentOutOfRangeError(ER_EPOCHS_INVALID, fEpochs);

  if fLambda < 0 then
    ArgumentOutOfRangeError(ER_LAMBDA_NEGATIVE, fLambda);
  
  if fCheckConvergence and (fTol <= 0) then
    ArgumentOutOfRangeError(ER_TOL_INVALID, fTol);
  
  var m := X.RowCount;
  var p := X.ColCount;

  // --- internal encoding
  var classes := new HashSet<integer>;
  
  for var i := 0 to y.Length - 1 do
  begin
    var r := y[i];
    var ir := Round(r);
  
    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);
  
    classes.Add(ir);
  end;
  
  var unique := classes.ToArray;
  &Array.Sort(unique);

  fClassCount := unique.Length;

  if fClassCount < 2 then
    ArgumentError(ER_LOGISTIC_NEED_AT_LEAST_TWO_CLASSES);

  fClassToIndex := new Dictionary<integer, integer>;
  SetLength(fIndexToClass, fClassCount);

  for var i := 0 to fClassCount - 1 do
  begin
    fClassToIndex[unique[i]] := i;
    fIndexToClass[i] := unique[i];
  end;

  // --- encode labels
  var yEncoded := new integer[m];
  for var i := 0 to m - 1 do
  begin
    var r := y[i];
    var ir := Round(r);
  
    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);
  
    yEncoded[i] := fClassToIndex[ir];
  end;

  // --- init
  fW := new Matrix(p, fClassCount);
  fIntercept := new Vector(fClassCount);

  var prevLoss := real.PositiveInfinity;

  // --- training loop
  for var epoch := 1 to fEpochs do
  begin
    var Z := X * fW;

    // + intercept
    for var i := 0 to m - 1 do
      for var k := 0 to fClassCount - 1 do
        Z[i,k] += fIntercept[k];

    // --- stable softmax + loss accumulation
    var loss := 0.0;

    for var i := 0 to m - 1 do
    begin
      var maxVal := Z.RowMax(i);
      var sumExp := 0.0;

      for var k := 0 to fClassCount - 1 do
      begin
        Z[i,k] := Exp(Z[i,k] - maxVal);
        sumExp += Z[i,k];
      end;

      if sumExp <= 0 then
      begin
        // численно вырожденный случай → равномерное распределение
        for var k := 0 to fClassCount - 1 do
          Z[i,k] := 1.0 / fClassCount;
      end
      else
      begin
        for var k := 0 to fClassCount - 1 do
          Z[i,k] /= sumExp;
      end;
      
      var yi := yEncoded[i];
      var prob := Z[i, yi];
      
      // защита от log(0)
      if prob < 1e-300 then
        prob := 1e-300;
      
      loss -= Ln(prob);
    end;

    loss /= m;

    // --- L2 penalty
    if fLambda <> 0 then
    begin
      var l2 := 0.0;
      for var j := 0 to p - 1 do
        for var k := 0 to fClassCount - 1 do
          l2 += fW[j,k] * fW[j,k];

      loss += 0.5 * fLambda * l2;
    end;

    // --- divergence check
    if double.IsNaN(loss) or double.IsInfinity(loss) then
      ArgumentError(ER_LOGISTIC_INVALID_LOSS);

    // --- convergence check
    if fCheckConvergence then
    begin
      if Abs(prevLoss - loss) < fTol then
        break;

      prevLoss := loss;
    end;

    // --- gradient
    var gradW := new Matrix(p, fClassCount);
    var gradB := new Vector(fClassCount);

    for var i := 0 to m - 1 do
    begin
      var yi := yEncoded[i];

      for var k := 0 to fClassCount - 1 do
      begin
        var diff := Z[i,k];
        if k = yi then
          diff -= 1.0;

        gradB[k] += diff;

        for var j := 0 to p - 1 do
          gradW[j,k] += X[i,j] * diff;
      end;
    end;

    gradW *= 1.0 / m;
    gradB *= 1.0 / m;

    if fLambda <> 0 then
      gradW += fLambda * fW;

    fW -= fLearningRate * gradW;
    fIntercept -= fLearningRate * gradB;
  end;

  fFitted := true;
  Result := Self;
end;

function LogisticRegression.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fW.RowCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fW.RowCount);

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

function LogisticRegression.GetClasses: array of real;
begin
  SetLength(Result, fClassCount);
  for var i := 0 to fClassCount - 1 do
    Result[i] := fIndexToClass[i];
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

function LogisticRegression.PredictLabels(X: Matrix): array of integer;
begin
  var v := Predict(X);
  
  Result := new integer[v.Length];
  
  for var i := 0 to v.Length - 1 do
    Result[i] := integer(v[i]);
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

function LogisticRegression.GetWeights: Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  Result := fW;
end;

function LogisticRegression.GetIntercept: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  Result := fIntercept;
end;

procedure LogisticRegression.SetClassLabels(classes: array of string);
begin
  fClassLabels := Copy(classes);
end;

function LogisticRegression.GetClassLabels: array of string;
begin
  if fClassLabels = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := fClassLabels;
end;

function GiniCriterion.Impurity(y: Vector; indices: array of integer): real;
begin
  var n := indices.Length;
  if n = 0 then
    exit(0.0);

  var counts := new integer[fClassCount];

  foreach var idx in indices do
  begin
    var cls := Round(y[idx]);

    if (cls < 0) or (cls >= fClassCount) then
      ArgumentError(ER_LABEL_INDEX_INVALID);

    counts[cls] += 1;
  end;

  var sumsq := 0.0;

  for var c := 0 to fClassCount - 1 do
  begin
    if counts[c] > 0 then
    begin
      var p := counts[c] / n;
      sumsq += p * p;
    end;
  end;

  var g := 1.0 - sumsq;

  if g < 0 then
    g := 0.0;   // защита от отрицательных из-за округления

  Result := g;
end;

function VarianceCriterion.Impurity(y: Vector; indices: array of integer): real;
begin
  var n := indices.Length;
  if n = 0 then
    exit(0.0);

  var sum := 0.0;
  var sumsq := 0.0;

  foreach var idx in indices do
  begin
    var v := y[idx];
    sum += v;
    sumsq += v * v;
  end;

  var mean := sum / n;
  var varValue := (sumsq / n) - mean * mean;

  if varValue < 0 then
    varValue := 0.0;   // защита от FP-ошибок

  Result := varValue;
end;

constructor EntropyCriterion.Create(classCount: integer);
begin
  fclassCount := classCount;
end;

function EntropyCriterion.Impurity(y: Vector; indices: array of integer): real;
begin
  var counts := new integer[fClassCount];

  foreach var i in indices do
    counts[Round(y[i])] += 1;

  var total := indices.Length;
  if total = 0 then exit(0.0);

  var res := 0.0;

  foreach var c in counts do
    if c > 0 then
    begin
      var p := c / total;
      res -= p * Ln(p);
    end;

  Result := res;
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

const MAX_ALLOWED_TREE_DEPTH = 1000;

constructor DecisionTreeBase.Create(
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  seed: integer);
begin
  if maxDepth < -1 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, maxDepth);

  if maxDepth > MAX_ALLOWED_TREE_DEPTH then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_TOO_LARGE, maxDepth);

  if minSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_SPLIT_INVALID, minSamplesSplit);

  if minSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_LEAF_INVALID, minSamplesLeaf);

  if minSamplesLeaf >= minSamplesSplit then
    ArgumentOutOfRangeError(
      ER_MIN_LEAF_GE_SPLIT,
      minSamplesLeaf, minSamplesSplit
    );

  // --- parameters valid → assign

  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;

  if seed < 0 then
  begin
    fUserProvidedSeed := false;
    fRandomSeed := System.Environment.TickCount and integer.MaxValue;
  end
  else
  begin
    fUserProvidedSeed := true;
    fRandomSeed := seed;
  end;

  fRng := new System.Random(fRandomSeed);
end;

procedure DecisionTreeBase.CopyBaseState(dest: DecisionTreeBase);
begin
  dest.fMaxDepth := fMaxDepth;
  dest.fMinSamplesSplit := fMinSamplesSplit;
  dest.fMinSamplesLeaf := fMinSamplesLeaf;
  dest.fFitted := fFitted;
  dest.fRandomSeed := fRandomSeed;
  dest.fMaxFeatures := fMaxFeatures;
  
  dest.fUserProvidedSeed := fUserProvidedSeed;

  if fUserProvidedSeed then
    dest.fRng := new System.Random(fRandomSeed)
  else
    dest.fRng := new System.Random;

  // MUST be stateless
  if fCriterion <> nil then
    dest.fCriterion := fCriterion;

  if fFeatureImportances <> nil then
    dest.fFeatureImportances := fFeatureImportances.Clone;

  if fRoot <> nil then
    dest.fRoot := fRoot.Clone;

  if fRowIndices <> nil then
    dest.fRowIndices := Copy(fRowIndices);
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
    var idx := fRng.Next(all.Count);
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
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fFeatureImportances = nil then
    exit(new Vector(0));

  Result := fFeatureImportances.Clone;
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
  if (fMaxDepth >= 0) and (depth >= fMaxDepth) then
    exit(LeafNode(LeafValue(y, indices)));

  if indices.Length < fMinSamplesSplit then
    exit(LeafNode(LeafValue(y, indices)));

  if IsPure(y, indices) then
    exit(LeafNode(LeafValue(y, indices)));

  var parentImp := fCriterion.Impurity(y, indices);

  if double.IsNaN(parentImp) or double.IsInfinity(parentImp) then
    exit(LeafNode(LeafValue(y, indices)));

  var split := FindBestSplit(X, y, indices);

  if not split.Found then
    exit(LeafNode(LeafValue(y, indices)));

  var left := new List<integer>;
  var right := new List<integer>;

  foreach var i in indices do
    if X[i, split.Feature] <= split.Threshold then
      left.Add(i)
    else
      right.Add(i);

  if (left.Count < fMinSamplesLeaf) or
     (right.Count < fMinSamplesLeaf) then
    exit(LeafNode(LeafValue(y, indices)));

  var leftArr := left.ToArray;
  var rightArr := right.ToArray;

  var leftImp := fCriterion.Impurity(y, leftArr);
  var rightImp := fCriterion.Impurity(y, rightArr);

  var n := indices.Length;

  var weighted :=
    (real(leftArr.Length) / n) * leftImp +
    (real(rightArr.Length) / n) * rightImp;

  var delta := parentImp - weighted;

  if double.IsNaN(delta) or double.IsInfinity(delta) then
    exit(LeafNode(LeafValue(y, indices)));

  if delta < 0 then
    delta := 0.0;

  // КЛЮЧЕВОЙ PRODUCTION-ФИЛЬТР
  if delta <= 0 then
    exit(LeafNode(LeafValue(y, indices)));

  fFeatureImportances[split.Feature] += delta;

  var leftNode := BuildTree(X, y, leftArr, depth + 1);
  var rightNode := BuildTree(X, y, rightArr, depth + 1);

  var node := new DecisionTreeNode;
  node.IsLeaf := false;
  node.FeatureIndex := split.Feature;
  node.Threshold := split.Threshold;
  node.Left := leftNode;
  node.Right := rightNode;

  Result := node;
end;

const EPS = 1e-12;

function DecisionTreeBase.IsPure(y: Vector; indices: array of integer): boolean;
begin
  Result := fCriterion.Impurity(y, indices) < EPS;
end;

function DecisionTreeRegressor.FindBestSplitReg(X: Matrix; y: Vector; indices: array of integer): SplitResult;
begin
  var bestScore := real.PositiveInfinity;
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

      if leftVar < 0 then leftVar := 0.0;
      if rightVar < 0 then rightVar := 0.0;

      var weighted :=
        (real(leftCount) / n) * leftVar +
        (real(rightCount) / n) * rightVar;

      if double.IsNaN(weighted) or double.IsInfinity(weighted) then
        continue;

      if weighted < bestScore then
      begin
        bestScore := weighted;
        bestFeature := j;
        bestThreshold := (x1 + x2) * 0.5;
      end;
    end;
  end;

  var p := X.ColCount;

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
      var r := i + fRng.Next(p - i);
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

//==============================
//    DecisionTreeClassifier
//==============================

function DecisionTreeClassifier.FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult;
begin
  var n := indices.Length;
  if n <= 1 then
    exit(SplitResult.Invalid);

  var p := X.ColCount;

  var bestFeature := -1;
  var bestThreshold := 0.0;
  var bestScore := real.PositiveInfinity;

  var feat := new integer[p];
  for var i := 0 to p - 1 do
    feat[i] := i;

  var m := fMaxFeatures;
  if (m <= 0) or (m > p) then
    m := p;

  // partial Fisher–Yates
  for var i := 0 to m - 1 do
  begin
    var j := fRng.Next(i, p);
    var tmp := feat[i];
    feat[i] := feat[j];
    feat[j] := tmp;
  end;

  for var fi := 0 to m - 1 do
  begin
    var feature := feat[fi];

    var values := new real[n];
    var labels := new integer[n];

    for var i := 0 to n - 1 do
    begin
      var row := indices[i];
      values[i] := X[row, feature];

      var cls := Round(y[row]);

      if (cls < 0) or (cls >= fClassCount) then
        ArgumentError(ER_LABEL_INDEX_INVALID);

      labels[i] := cls;
    end;

    System.Array.Sort(values, labels);

    var rightCounts := new integer[fClassCount];
    for var i := 0 to n - 1 do
      rightCounts[labels[i]] += 1;

    var leftCounts := new integer[fClassCount];

    var leftSize := 0;
    var rightSize := n;

    for var i := 0 to n - 2 do
    begin
      var cls := labels[i];

      leftCounts[cls] += 1;
      rightCounts[cls] -= 1;

      leftSize += 1;
      rightSize -= 1;

      if values[i] = values[i + 1] then
        continue;

      if (leftSize < fMinSamplesLeaf) or (rightSize < fMinSamplesLeaf) then
        continue;

      // ----- GINI LEFT -----
      var giniLeft := 1.0;

      for var c := 0 to fClassCount - 1 do
      begin
        if leftCounts[c] > 0 then
        begin
          var q := leftCounts[c] / real(leftSize);
          giniLeft -= q * q;
        end;
      end;

      if giniLeft < 0 then
        giniLeft := 0.0;

      // ----- GINI RIGHT -----
      var giniRight := 1.0;

      for var c := 0 to fClassCount - 1 do
      begin
        if rightCounts[c] > 0 then
        begin
          var q := rightCounts[c] / real(rightSize);
          giniRight -= q * q;
        end;
      end;

      if giniRight < 0 then
        giniRight := 0.0;

      var weighted :=
        (real(leftSize) / n) * giniLeft +
        (real(rightSize) / n) * giniRight;

      if double.IsNaN(weighted) then
        continue;

      if weighted < bestScore then
      begin
        bestScore := weighted;
        bestFeature := feature;
        bestThreshold := (values[i] + values[i + 1]) * 0.5;
      end;
    end;
  end;

  if bestFeature = -1 then
    exit(SplitResult.Invalid);

  Result := SplitResult.Create(bestFeature, bestThreshold);
end;

function DecisionTreeClassifier.PredictOne(X: Matrix; rowIndex: integer): integer;
begin
  var node := fRoot;

  while not node.IsLeaf do
  begin
    if X[rowIndex, node.FeatureIndex] <= node.Threshold then
      node := node.Left
    else
      node := node.Right;
  end;

  Result := integer(node.LeafValue);  // внутренний индекс
end;

function DecisionTreeClassifier.MajorityClass(y: Vector; indices: array of integer): integer;
begin
  var counts := new integer[fClassCount];

  // Подсчёт частот
  foreach var i in indices do
  begin
    var c := Round(y[i]);
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

constructor DecisionTreeClassifier.Create(maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer; seed: integer);
begin
  inherited Create(maxDepth, minSamplesSplit, minSamplesLeaf, seed);
end;

function DecisionTreeClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  
  fFeatureImportances := new Vector(X.ColCount);

  // --- 1. Найти уникальные классы (с проверкой целочисленности)
  var classes := new HashSet<integer>;
  
  for var i := 0 to y.Length - 1 do
  begin
    var r := y[i];
    var ir := Round(r);
  
    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);
  
    classes.Add(ir);
  end;
  
  fClassCount := classes.Count;
  
  // --- 2. Отсортировать классы для детерминизма
  fIndexToClass := classes.ToArray;
  &Array.Sort(fIndexToClass);
  
  fClassToIndex := new Dictionary<integer, integer>;
  
  for var idx := 0 to fClassCount - 1 do
    fClassToIndex[fIndexToClass[idx]] := idx;
  
  fCriterion := new GiniCriterion(fClassCount);
  
  // --- 3. Создать закодированный y
  var yEncoded := new Vector(y.Length);
  
  for var i := 0 to y.Length - 1 do
  begin
    var ir := Round(y[i]); // безопасно после проверки
    yEncoded[i] := fClassToIndex[ir];
  end;

  // --- 4. Создать массив индексов строк
  var indices: array of integer;

  if fRowIndices <> nil then
    indices := fRowIndices
  else
  begin
    indices := new integer[X.RowCount];
    for var i := 0 to X.RowCount - 1 do
      indices[i] := i;
  end;

  // --- 5. Построить дерево
  fRoot := BuildTree(X, yEncoded, indices, 0);
  // Сбросить подмножество строк (не должно протекать на следующий Fit)
  fRowIndices := nil;
  
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
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureImportances.Length then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureImportances.Length);

  var n := X.RowCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    var internalClass := PredictOne(X, i);
    Result[i] := fIndexToClass[internalClass];
  end;
end;

function DecisionTreeClassifier.PredictLabels(X: Matrix): array of integer;
begin
  var v := Predict(X);
  
  Result := new integer[v.Length];
  
  for var i := 0 to v.Length - 1 do
    Result[i] := integer(v[i]);
end;

function DecisionTreeClassifier.Clone: IModel;
begin
  var m := new DecisionTreeClassifier(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fRandomSeed
  );

  CopyBaseState(m);

  // --- классы
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

function DecisionTreeClassifier.ToString: string;
begin
  Result :=
    $'DecisionTreeClassifier(' +
    $'maxDepth={fMaxDepth}, ' +
    $'minSamplesSplit={fMinSamplesSplit}, ' +
    $'minSamplesLeaf={fMinSamplesLeaf}' +
    ')';
end;

procedure DecisionTreeClassifier.SetClassLabels(classes: array of string);
begin
  fClassLabels := Copy(classes);
end;

function DecisionTreeClassifier.GetClassLabels: array of string;
begin
  if fClassLabels = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := fClassLabels;
end;

// DecisionTreeRegressor

constructor DecisionTreeRegressor.Create(maxDepth: integer; minSamplesSplit: integer; 
  minSamplesLeaf: integer; leafL2: real; seed: integer);
begin
  inherited Create(maxDepth, minSamplesSplit, minSamplesLeaf, seed);
  
  if leafL2 < 0 then
    ArgumentOutOfRangeError(ER_LEAFL2_INVALID, leafL2);
  
  fCriterion := new VarianceCriterion;
  fLeafL2 := leafL2;
end;

function DecisionTreeRegressor.LeafValue(y: Vector; indices: array of integer): real;
begin
  var n := indices.Length;

  if n = 0 then
    exit(0.0);  // безопасный fallback, не должен происходить

  if fLeafL2 < 0 then
    ArgumentOutOfRangeError(ER_LEAFL2_INVALID, fLeafL2);

  var sum := 0.0;

  foreach var idx in indices do
    sum += y[idx];

  var denom: real;

  if fLeafL2 > 0 then
    denom := n + fLeafL2
  else
    denom := n;

  var value := sum / denom;

  if double.IsNaN(value) or double.IsInfinity(value) then
    value := 0.0;  // защита от численного выброса

  Result := value;
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

function DecisionTreeRegressor.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if fLeafL2 < 0 then
    ArgumentOutOfRangeError(ER_L2_NEGATIVE, fLeafL2);

  fFeatureImportances := new Vector(X.ColCount);

  var indices: array of integer;

  // 🔹 Ключевое изменение
  if fRowIndices = nil then
  begin
    SetLength(indices, X.RowCount);
    for var i := 0 to X.RowCount - 1 do
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

function DecisionTreeRegressor.PredictOne(X: Matrix; rowIndex: integer): real;
begin
  var node := fRoot;

  while not node.IsLeaf do
  begin
    if X[rowIndex, node.FeatureIndex] <= node.Threshold then
      node := node.Left
    else
      node := node.Right;
  end;

  Result := node.LeafValue;  // для регрессии это уже real
end;

function DecisionTreeRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureImportances.Length then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,
                   X.ColCount,
                   fFeatureImportances.Length);

  var n := X.RowCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
    Result[i] := PredictOne(X, i);
end;

function DecisionTreeRegressor.Clone: IModel;
begin
  var m := new DecisionTreeRegressor(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fLeafL2,
    fRandomSeed
  );

  CopyBaseState(m);
  
  Result := m;
end;

function DecisionTreeRegressor.ToString: string;
begin
  var s :=
    $'DecisionTreeRegressor(maxDepth={fMaxDepth}, ' +
    $'minSamplesSplit={fMinSamplesSplit}, ' +
    $'minSamplesLeaf={fMinSamplesLeaf}';

  if fLeafL2 <> 0.0 then
    s += $', leafL2={fLeafL2}';

  s += ')';

  Result := s;
end;


//-----------------------------
//      RandomForestBase 
//-----------------------------

constructor RandomForestBase.Create(
  nTrees: integer;
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  maxFeatures: TMaxFeaturesMode;
  useOOB: boolean;
  seed: integer);
begin
  if nTrees <= 0 then 
    ArgumentOutOfRangeError(ER_NTREES_INVALID, nTrees);

  if maxDepth < -1 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, maxDepth);

  if minSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_SPLIT_INVALID, minSamplesSplit);

  if minSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_LEAF_INVALID, minSamplesLeaf);
  
  if minSamplesLeaf >= minSamplesSplit then
    ArgumentOutOfRangeError(ER_MIN_LEAF_GE_SPLIT, minSamplesLeaf, minSamplesSplit);
    
  fNTrees := nTrees;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fMaxFeaturesMode := maxFeatures;
  fFitted := false;
  
  fFeatureCount := 0;
  
  fUseOOB := useOOB;
  fOOBScore := real.NaN;
  
  if seed < 0 then
  begin
    fUserProvidedSeed := false;
    fRandomSeed := System.Environment.TickCount and integer.MaxValue;
  end
  else
  begin
    fUserProvidedSeed := true;
    fRandomSeed := seed;
  end;

  fRng := new System.Random(fRandomSeed);
end;

function RandomForestBase.OOBScore: real;
begin
  if not fUseOOB then
    Error(ER_OOB_NOT_ENABLED);

  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  Result := fOOBScore;
end;

function RandomForestBase.ComputeMaxFeatures(p: integer): integer;
begin
  case fMaxFeaturesMode of
    AllFeatures:  Result := p;
    SqrtFeatures: Result := integer(Sqrt(p));
    Log2Features: Result := integer(Log2(p));
    HalfFeatures: Result := p div 2;
  end;
  if Result < 1 then
    Result := 1;
end;

procedure RandomForestBase.BootstrapRowIndices(n: integer; var rows: array of integer);
begin
  SetLength(rows, n);
  for var i := 0 to n - 1 do
    rows[i] := fRng.Next(n);
end;

//-----------------------------
//     RandomForestRegressor 
//-----------------------------
constructor RandomForestRegressor.Create(nTrees: integer; maxDepth: integer;
  minSamplesSplit: integer; minSamplesLeaf: integer;
  maxFeaturesMode: TMaxFeaturesMode; computeOOB: boolean; seed: integer);
begin
  inherited Create(nTrees,maxDepth,minSamplesSplit,minSamplesLeaf,maxFeaturesMode,computeOOB,seed)
end;

function RandomForestRegressor.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  var n := X.RowCount;
  var p := X.ColCount;

  fFeatureCount := p;

  SetLength(fTrees, fNTrees);

  // --- OOB buffers (regression) ---
  var oobSum: Vector := nil;
  var oobCnt: array of integer := nil;

  if fUseOOB then
  begin
    oobSum := new Vector(n);
    oobCnt := new integer[n];
  end;

  for var t := 0 to fNTrees - 1 do
  begin
    var treeSeed := fRng.Next(integer.MaxValue);

    var tree := new DecisionTreeRegressor(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      0.0,          // leafL2 если нужно — подставь своё поле
      treeSeed
    );

    var mfeat := ComputeMaxFeatures(p);
    tree.fMaxFeatures := mfeat;

    var rows: array of integer;
    BootstrapRowIndices(n, rows);

    tree.SetRowIndices(rows);
    tree.Fit(X, y);

    // --- OOB accumulate ---
    if fUseOOB then
    begin
      var inBag := new boolean[n];
      for var i := 0 to rows.Length - 1 do
        inBag[rows[i]] := true;

      for var i := 0 to n - 1 do
        if not inBag[i] then
        begin
          oobSum[i] += tree.PredictOne(X, i);
          oobCnt[i] += 1;
        end;
    end;

    fTrees[t] := tree;
  end;

  // --- finalize OOB score: R^2 ---
  fHasOOBScore := false;
  fOOBScore := real.NaN;

  if fUseOOB then
  begin
    var meanY := y.Mean;

    var sse := 0.0;
    var sst := 0.0;
    var cnt := 0;

    for var i := 0 to n - 1 do
      if oobCnt[i] > 0 then
      begin
        var yhat := oobSum[i] / oobCnt[i];
        var e := y[i] - yhat;
        sse += e * e;

        var d := y[i] - meanY;
        sst += d * d;

        cnt += 1;
      end;

    // если OOB почти нет — просто помечаем как недоступно
    if cnt >= Max(1, n div 10) then
    begin
      if sst <= 0 then
        fOOBScore := 0.0
      else
        fOOBScore := 1.0 - (sse / sst);

      fHasOOBScore := true;
    end;
  end;

  fFitted := true;
  Result := Self;
end;

function RandomForestRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);
  
  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);

  var n := X.RowCount;
  var resultVec := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    var s := 0.0;

    for var t := 0 to fTrees.Length - 1 do
      s += fTrees[t].PredictOne(X, i);

    resultVec[i] := s / fTrees.Length;
  end;

  Result := resultVec;
end;

function RandomForestRegressor.Clone: IModel;
begin
  var rf := new RandomForestRegressor(
    fNTrees,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fMaxFeaturesMode,
    fUseOOB,
    fRandomSeed
  );

  // --- seed ---
  rf.fRandomSeed := fRandomSeed;
  rf.fUserProvidedSeed := fUserProvidedSeed;

  if fUserProvidedSeed then
    rf.fRng := new System.Random(fRandomSeed)
  else
    rf.fRng := new System.Random;

  rf.fFitted := fFitted;
  rf.fFeatureCount := fFeatureCount;

  rf.fOOBScore := fOOBScore;
  rf.fHasOOBScore := fHasOOBScore;

  // --- trees ---
  if fTrees <> nil then
  begin
    SetLength(rf.fTrees, fTrees.Length);
    for var i := 0 to fTrees.Length - 1 do
      rf.fTrees[i] := DecisionTreeRegressor(fTrees[i].Clone);
  end;

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

function RandomForestRegressor.ToString: string;
begin
  var depthStr :=
    if fMaxDepth = -1 then '∞'
    else fMaxDepth.ToString;

  var seedPart :=
    if fUserProvidedSeed then
      $', seed={fRandomSeed}'
    else
      '';

  Result :=
    $'RandomForestRegressor(' +
    $'nTrees={fNTrees}, ' +
    $'maxDepth={depthStr}, ' +
    $'minSamplesSplit={fMinSamplesSplit}, ' +
    $'minSamplesLeaf={fMinSamplesLeaf}, ' +
    $'maxFeatures={fMaxFeaturesMode}' +
    seedPart +
    ')';
end;

//-----------------------------
//     RandomForestClassifier 
//-----------------------------

constructor RandomForestClassifier.Create(nTrees: integer; 
  maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer;
  maxFeaturesMode: TMaxFeaturesMode; computeOOB: boolean; seed: integer);
begin
  inherited Create(nTrees,maxDepth,minSamplesSplit,minSamplesLeaf,maxFeaturesMode,computeOOB,seed);
end;

function RandomForestClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  var n := X.RowCount;
  var p := X.ColCount;

  fFeatureCount := p;

  SetLength(fTrees, fNTrees);

  // сброс
  fClassCount := 0;
  fIndexToClass := nil;
  fClassToIndex := nil;

  // --- OOB buffers (classification) ---
  var yEnc: array of integer := nil;
  var oobVotes: array[,] of integer := nil;
  var oobCnt: array of integer := nil;

  for var t := 0 to fNTrees - 1 do
  begin
    var treeSeed := fRng.Next(integer.MaxValue);

    var tree := new DecisionTreeClassifier(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      treeSeed
    );

    var mfeat := ComputeMaxFeatures(p);
    tree.fMaxFeatures := mfeat;

    var rows: array of integer;
    BootstrapRowIndices(n, rows);

    tree.SetRowIndices(rows);
    tree.Fit(X, y);

    if t = 0 then
    begin
      // classes фиксируем по первому дереву
      fClassCount := tree.ClassCount;
      fIndexToClass := tree.IndexToClass;

      fClassToIndex := new Dictionary<integer, integer>;
      for var k := 0 to fClassCount - 1 do
        fClassToIndex[fIndexToClass[k]] := k;

      // encode y в internal индексы (ОБОСНОВАННО: y должен быть целочисленным, как у дерева)
      SetLength(yEnc, n);
      for var i := 0 to n - 1 do
      begin
        var r := y[i];
        var ir := Round(r);
        if Abs(r - ir) > 1e-12 then
          ArgumentError(ER_LABELS_NOT_INTEGER);

        yEnc[i] := fClassToIndex[ir];
      end;

      if fUseOOB then
      begin
        oobVotes := new integer[n, fClassCount];
        oobCnt := new integer[n];
      end;
    end;

    // --- OOB accumulate ---
    if fUseOOB then
    begin
      var inBag := new boolean[n];
      for var i := 0 to rows.Length - 1 do
        inBag[rows[i]] := true;

      for var i := 0 to n - 1 do
        if not inBag[i] then
        begin
          var cls := tree.PredictOne(X, i); // internal index 0..K-1
          oobVotes[i, cls] += 1;
          oobCnt[i] += 1;
        end;
    end;

    fTrees[t] := tree;
  end;

  // --- finalize OOB score: accuracy ---
  fHasOOBScore := false;
  fOOBScore := real.NaN;

  if fUseOOB then
  begin
    var correct := 0;
    var cnt := 0;

    for var i := 0 to n - 1 do
      if oobCnt[i] > 0 then
      begin
        // argmax по oobVotes[i,*]
        var bestK := 0;
        var bestV := oobVotes[i, 0];
        for var k := 1 to fClassCount - 1 do
          if oobVotes[i, k] > bestV then
          begin
            bestV := oobVotes[i, k];
            bestK := k;
          end;

        if bestK = yEnc[i] then
          correct += 1;

        cnt += 1;
      end;

    if cnt >= Max(1, n div 10) then
    begin
      fOOBScore := correct / cnt;
      fHasOOBScore := true;
    end;
  end;

  fFitted := true;
  Result := Self;
end;

function RandomForestClassifier.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);

  var n := X.RowCount;
  var treeCount := fTrees.Length;

  if treeCount = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  if fClassCount <= 0 then
    Error(ER_MODEL_NOT_INITIALIZED);
  

  var resultVec := new Vector(n);
  var counts := new integer[fClassCount];

  for var i := 0 to n - 1 do
  begin
    for var c := 0 to fClassCount - 1 do
      counts[c] := 0;

    for var t := 0 to treeCount - 1 do
    begin
      var cls := fTrees[t].PredictOne(X, i);

      if (cls < 0) or (cls >= fClassCount) then
        ArgumentError(ER_LABEL_INDEX_INVALID);

      counts[cls] += 1;
    end;

    var bestClass := 0;
    var bestCount := counts[0];

    for var c := 1 to fClassCount - 1 do
      if counts[c] > bestCount then
      begin
        bestCount := counts[c];
        bestClass := c;
      end;

    resultVec[i] := fIndexToClass[bestClass];
  end;

  Result := resultVec;
end;

function RandomForestClassifier.PredictLabels(X: Matrix): array of integer;
begin
  var v := Predict(X);
  
  Result := new integer[v.Length];
  
  for var i := 0 to v.Length - 1 do
    Result[i] := integer(v[i]);
end;

function RandomForestClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);

  var n := X.RowCount;
  var treeCount := fTrees.Length;

  if treeCount = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  if fClassCount <= 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  var resultMat := new Matrix(n, fClassCount);
  var counts := new integer[fClassCount];

  for var i := 0 to n - 1 do
  begin
    for var c := 0 to fClassCount - 1 do
      counts[c] := 0;

    for var t := 0 to treeCount - 1 do
    begin
      var cls := fTrees[t].PredictOne(X, i);

      if (cls < 0) or (cls >= fClassCount) then
        ArgumentError(ER_LABEL_INDEX_INVALID);

      counts[cls] += 1;
    end;

    for var c := 0 to fClassCount - 1 do
      resultMat[i, c] := counts[c] / treeCount;
  end;

  Result := resultMat;
end;

function RandomForestClassifier.Clone: IModel;
begin
  var rf := new RandomForestClassifier(
    fNTrees,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fMaxFeaturesMode,
    fUseOOB,
    fRandomSeed
  );

  // --- seed ---
  rf.fRandomSeed := fRandomSeed;
  rf.fUserProvidedSeed := fUserProvidedSeed;

  if fUserProvidedSeed then
    rf.fRng := new System.Random(fRandomSeed)
  else
    rf.fRng := new System.Random;

  rf.fFitted := fFitted;
  rf.fFeatureCount := fFeatureCount;

  // --- classes ---
  rf.fClassCount := fClassCount;

  if fIndexToClass <> nil then
    rf.fIndexToClass := Copy(fIndexToClass);

  if fClassToIndex <> nil then
  begin
    rf.fClassToIndex := new Dictionary<integer, integer>;
    foreach var kv in fClassToIndex do
      rf.fClassToIndex.Add(kv.Key, kv.Value);
  end;

  // --- OOB ---
  rf.fOOBScore := fOOBScore;
  rf.fHasOOBScore := fHasOOBScore;

  // --- trees ---
  if fTrees <> nil then
  begin
    SetLength(rf.fTrees, fTrees.Length);
    for var i := 0 to fTrees.Length - 1 do
      rf.fTrees[i] := DecisionTreeClassifier(fTrees[i].Clone);
  end;

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

function RandomForestClassifier.ToString: string;
begin
  var depthStr :=
    if fMaxDepth = -1 then '∞'
    else fMaxDepth.ToString;

  var seedPart :=
    if fUserProvidedSeed then
      $', seed={fRandomSeed}'
    else
      '';

  Result :=
    $'RandomForestClassifier(' +
    $'nTrees={fNTrees}, ' +
    $'maxDepth={depthStr}, ' +
    $'minSamplesSplit={fMinSamplesSplit}, ' +
    $'minSamplesLeaf={fMinSamplesLeaf}, ' +
    $'maxFeatures={fMaxFeaturesMode}' +
    seedPart +
    ')';
end;

function RandomForestClassifier.GetClasses: array of real;
begin
  Result := new real[fClassCount];

  for var i := 0 to fClassCount - 1 do
    Result[i] := fIndexToClass[i];
end;

procedure RandomForestClassifier.SetClassLabels(classes: array of string);
begin
  fClassLabels := Copy(classes);
end;

function RandomForestClassifier.GetClassLabels: array of string;
begin
  if fClassLabels = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := fClassLabels;
end;

//-----------------------------
//      Helper GBC GBR
//-----------------------------

function BuildSubsampleIndices(nTrain: integer; subsample: real; rng: System.Random): array of integer;
begin
  if nTrain <= 0 then
    ArgumentOutOfRangeError(ER_EMPTY_DATASET);

  if (subsample <= 0.0) or (subsample > 1.0) then
    ArgumentOutOfRangeError(ER_SUBSAMPLE_INVALID, subsample);

  var k := Floor(nTrain * subsample);
  if k < 1 then
    k := 1;
  if k > nTrain then
    k := nTrain;

  // если берём всё — можно быстро вернуть [0..nTrain-1]
  if k = nTrain then
  begin
    Result := new integer[nTrain];
    for var i := 0 to nTrain - 1 do
      Result[i] := i;
    exit;
  end;

  var all := new integer[nTrain];
  for var i := 0 to nTrain - 1 do
    all[i] := i;

  // partial Fisher–Yates (без повторений)
  for var i := 0 to k - 1 do
  begin
    var j := i + rng.Next(nTrain - i);
    var tmp := all[i];
    all[i] := all[j];
    all[j] := tmp;
  end;

  Result := new integer[k];
  for var i := 0 to k - 1 do
    Result[i] := all[i];
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
  loss: TGBLoss;
  huberDelta: real;
  earlyStoppingPatience: integer;
  quantileAlpha: real;
  leafL2: real;
  useOOBEarlyStopping: boolean;
  seed: integer); 
begin
  if nEstimators <= 0 then
    ArgumentOutOfRangeError(ER_N_ESTIMATORS_NOT_POSITIVE);

  if learningRate <= 0 then
    ArgumentOutOfRangeError(ER_LEARNING_RATE_NOT_POSITIVE);

  if (subsample <= 0) or (subsample > 1) then
    ArgumentOutOfRangeError(ER_SUBSAMPLE_OUT_OF_RANGE);
  
  if maxDepth <= 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, maxDepth);
  
  if minSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_SPLIT_INVALID, minSamplesSplit);
  
  if minSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_LEAF_INVALID, minSamplesLeaf);
  
  if maxDepth > MAX_ALLOWED_TREE_DEPTH then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_TOO_LARGE, maxDepth);

  fNEstimators := nEstimators;
  fLearningRate := learningRate;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fSubsample := subsample;

  fEstimators := new List<DecisionTreeRegressor>;
  fFitted := false;
  
  if seed < 0 then
  begin
    fUserProvidedSeed := false;
    fRandomSeed := System.Environment.TickCount and integer.MaxValue;
  end
  else
  begin
    fUserProvidedSeed := true;
    fRandomSeed := seed;
  end;

  fRng := new System.Random(fRandomSeed);
  
// -------------  
  if huberDelta <= 0 then
    ArgumentOutOfRangeError(ER_HUBER_DELTA_INVALID, huberDelta);
  
  if earlyStoppingPatience < 0 then
    ArgumentOutOfRangeError(ER_EARLY_STOPPING_INVALID, earlyStoppingPatience);
  
  if (quantileAlpha <= 0) or (quantileAlpha >= 1) then
    ArgumentOutOfRangeError(ER_QUANTILE_ALPHA_INVALID, quantileAlpha);
  
  if leafL2 < 0 then
    ArgumentOutOfRangeError(ER_LEAFL2_INVALID, leafL2);
  
  fLoss := loss;
  fHuberDelta := huberDelta;
  fEarlyStoppingPatience := earlyStoppingPatience;
  
  fTrainLossHistory := new List<real>;
  fValLossHistory := new List<real>;
  
  fquantileAlpha := quantileAlpha;
  
  fLeafL2 := leafL2;
  fUseOOBEarlyStopping := useOOBEarlyStopping;
  
  fOOBLossHistory := new List<real>;
end;

function GradientBoostingRegressor.ComputeTrainLoss(
  y, yPred: Vector): real;
begin
  var n := y.Length;

  if n = 0 then
    exit(0.0);

  var sum := 0.0;

  case fLoss of

    TGBLoss.SquaredError:
      for var i := 0 to n - 1 do
      begin
        var e := y[i] - yPred[i];
        sum += 0.5 * e * e;
      end;

    TGBLoss.Huber:
      for var i := 0 to n - 1 do
      begin
        var e := y[i] - yPred[i];
        var ae := Abs(e);

        if ae <= fHuberDelta then
          sum += 0.5 * e * e
        else
          sum += fHuberDelta * (ae - 0.5 * fHuberDelta);
      end;

    TGBLoss.Quantile:
      for var i := 0 to n - 1 do
      begin
        var r := y[i] - yPred[i];

        if r >= 0 then
          sum += fQuantileAlpha * r
        else
          sum += (fQuantileAlpha - 1.0) * r;
      end;
  end;

  var loss := sum / n;

  if double.IsNaN(loss) or double.IsInfinity(loss) then
    ArgumentError(ER_TRAINING_DIVERGED);

  Result := loss;
end;

procedure GradientBoostingRegressor.ComputePseudoResiduals(
  y, yPred: Vector; r: Vector);
begin
  var n := y.Length;

  case fLoss of

    TGBLoss.SquaredError:
      for var i := 0 to n - 1 do
        r[i] := y[i] - yPred[i];

    TGBLoss.Huber:
      for var i := 0 to n - 1 do
      begin
        var e := y[i] - yPred[i];

        if Abs(e) <= fHuberDelta then
          r[i] := e
        else
          r[i] := fHuberDelta * Sign(e);
      end;

    TGBLoss.Quantile:
      for var i := 0 to n - 1 do
      begin
        var diff := y[i] - yPred[i];

        if diff > 0 then
          r[i] := fQuantileAlpha
        else
          r[i] := fQuantileAlpha - 1.0;
      end;

  end;
end;

function GradientBoostingRegressor.ComputeQuantile(y: Vector; alpha: real): real;
begin
  var n := y.Length;

  if (alpha <= 0) or (alpha >= 1) then
    ArgumentOutOfRangeError(
      'Quantile alpha must be in (0,1)!!Quantile alpha must be in (0,1)'
    );

  // копируем данные
  var data := new real[n];
  for var i := 0 to n - 1 do
    data[i] := y[i];

  // сортировка
  &Array.Sort(data);

  // индекс квантиля
  var k := Floor(alpha * (n - 1));

  Result := data[k];
end;

function GradientBoostingRegressor.ComputeTrainLossMasked(yTrue, yPred: Vector;
  mask: array of boolean): real;
begin
  var n := yTrue.Length;
  var sum := 0.0;
  var count := 0;

  case fLoss of

    TGBLoss.SquaredError:
      for var i := 0 to n - 1 do
        if mask[i] then
        begin
          var e := yTrue[i] - yPred[i];
          sum += 0.5 * e * e;
          count += 1;
        end;

    TGBLoss.Huber:
      for var i := 0 to n - 1 do
        if mask[i] then
        begin
          var e := yTrue[i] - yPred[i];
          var ae := Abs(e);

          if ae <= fHuberDelta then
            sum += 0.5 * e * e
          else
            sum += fHuberDelta * (ae - 0.5 * fHuberDelta);

          count += 1;
        end;

    TGBLoss.Quantile:
      for var i := 0 to n - 1 do
        if mask[i] then
        begin
          var e := yTrue[i] - yPred[i];
          if e > 0 then
            sum += fQuantileAlpha * e
          else
            sum += (fQuantileAlpha - 1.0) * e;
          count += 1;
        end;

  end;

  if count = 0 then
    exit(real.PositiveInfinity);

  Result := sum / count;
end;

const MinImprovement = 1e-12;

function GradientBoostingRegressor.Fit(X: Matrix; y: Vector): ISupervisedModel;
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
  useValidation: boolean): ISupervisedModel;
begin
  // --- null checks ---
  if XTrain = nil then
    ArgumentNullError(ER_X_NULL);

  if yTrain = nil then
    ArgumentNullError(ER_Y_NULL);

  // --- finite checks ---
  if ValidateFiniteInputs then
  begin
    CheckXForFit(XTrain);
    CheckYForFit(yTrain);
  end;

  // --- shape checks ---
  if XTrain.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if XTrain.RowCount <> yTrain.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if useValidation then
  begin
    if XVal = nil then
      ArgumentNullError(ER_X_NULL);

    if yVal = nil then
      ArgumentNullError(ER_Y_NULL);

    if ValidateFiniteInputs then
    begin
      CheckXForPredict(XVal);
      CheckYForFit(yVal);
    end;

    if XVal.RowCount <> yVal.Length then
      DimensionError(ER_XY_SIZE_MISMATCH);

    if XVal.ColCount <> XTrain.ColCount then
      DimensionError(ER_FEATURE_COUNT_MISMATCH);
  end;

  // --- init state ---
  fOOBLossHistory.Clear;
  fEstimators.Clear;
  fFeatureCount := XTrain.ColCount;

  fTrainLossHistory.Clear;
  fValLossHistory.Clear;

  fBestIteration := -1;
  fBestScoreLoss := real.PositiveInfinity;

  var noImprove := 0;

  // --- F0 ---
  case fLoss of
    TGBLoss.Quantile:
      fInitValue := ComputeQuantile(yTrain, fQuantileAlpha);
    else
      fInitValue := yTrain.Average;
  end;

  var nTrain := yTrain.Length;

  var yPredTrain := new Vector(nTrain);
  for var i := 0 to nTrain - 1 do
    yPredTrain[i] := fInitValue;

  // --- OOB logic ---
  var useSubsample := fSubsample < 1.0;
  var useOOB :=
      (not useValidation) and
      useSubsample and
      (fEarlyStoppingPatience > 0) and
      fUseOOBEarlyStopping;

  var oobSum := new Vector(nTrain);      // сумма вкладов OOB-деревьев
  var oobCount := new integer[nTrain];   // сколько раз объект был OOB

  var yPredVal: Vector := nil;
  if useValidation then
  begin
    var nVal := yVal.Length;
    yPredVal := new Vector(nVal);
    for var i := 0 to nVal - 1 do
      yPredVal[i] := fInitValue;
  end;

  // --- boosting loop ---
  for var m := 0 to fNEstimators - 1 do
  begin
    // 1. residuals
    var r := new Vector(nTrain);
    ComputePseudoResiduals(yTrain, yPredTrain, r);

    var stageSeed := fRng.Next(integer.MaxValue);

    var tree := new DecisionTreeRegressor(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      fLeafL2,
      stageSeed
    );

    // --- subsample ---
    var rows: array of integer := nil;
    var used: array of boolean := nil;
    
    if useSubsample then
    begin
      rows := BuildSubsampleIndices(nTrain, fSubsample, fRng);
      tree.SetRowIndices(rows);
    
      if useOOB then
      begin
        used := new boolean[nTrain];
        for var i := 0 to rows.Length - 1 do
          used[rows[i]] := true;
      end;
    end;
    
    tree.Fit(XTrain, r);
    fEstimators.Add(tree);

    var deltaTrain := tree.Predict(XTrain);

    // --- update TRAIN ---
    if useSubsample then
    begin
      for var i := 0 to rows.Length - 1 do
      begin
        var idx := rows[i];
        yPredTrain[idx] += fLearningRate * deltaTrain[idx];
      end;
    end
    else
    begin
      for var i := 0 to nTrain - 1 do
        yPredTrain[i] += fLearningRate * deltaTrain[i];
    end;

    // --- update OOB ---
    if useOOB then
    begin
      for var i := 0 to nTrain - 1 do
        if not used[i] then
        begin
          oobSum[i] += fLearningRate * deltaTrain[i];
          oobCount[i] += 1;
        end;
    end;

    // --- update VAL ---
    if useValidation then
    begin
      var deltaVal := tree.Predict(XVal);
      for var i := 0 to yPredVal.Length - 1 do
        yPredVal[i] += fLearningRate * deltaVal[i];
    end;

    // --- losses ---
    var trainLoss := ComputeTrainLoss(yTrain, yPredTrain);
    fTrainLossHistory.Add(trainLoss);

    var scoreLoss := trainLoss;

    if useValidation then
    begin
      var valLoss := ComputeTrainLoss(yVal, yPredVal);
      fValLossHistory.Add(valLoss);
      scoreLoss := valLoss;
    end
    else if useOOB then
    begin
      var mask := new boolean[nTrain];
      var yPredOOB := new Vector(nTrain);

      var cnt := 0;

      for var i := 0 to nTrain - 1 do
      begin
        if oobCount[i] > 0 then
        begin
          mask[i] := true;
          yPredOOB[i] :=
              fInitValue + oobSum[i] / oobCount[i];
          cnt += 1;
        end
        else
          mask[i] := false;
      end;

      if cnt >= Max(1, nTrain div 10) then
      begin
        scoreLoss := ComputeTrainLossMasked(yTrain, yPredOOB, mask);
        fOOBLossHistory.Add(scoreLoss);
      end
      else
      begin
        scoreLoss := trainLoss;
        fOOBLossHistory.Add(real.NaN);
      end;
    end;

    // --- early stopping ---
    if fEarlyStoppingPatience > 0 then
    begin
      if (fBestScoreLoss - scoreLoss > MinImprovement) then
      begin
        fBestScoreLoss := scoreLoss;
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

  // --- cut estimators ---
  if (fEarlyStoppingPatience > 0) and (fBestIteration >= 0) then
  begin
    var keep := fBestIteration + 1;

    if fEstimators.Count > keep then
      fEstimators.RemoveRange(keep, fEstimators.Count - keep);

    if fOOBLossHistory.Count > keep then
      fOOBLossHistory.RemoveRange(keep, fOOBLossHistory.Count - keep);

    if fTrainLossHistory.Count > keep then
      fTrainLossHistory.RemoveRange(keep, fTrainLossHistory.Count - keep);

    if fValLossHistory.Count > keep then
      fValLossHistory.RemoveRange(keep, fValLossHistory.Count - keep);
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

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var n := X.RowCount;
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

function GradientBoostingRegressor.PredictStage(X: Matrix; m: integer): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);

  var totalTrees := fEstimators.Count;

  if (m < 0) or (m > totalTrees) then
    ArgumentOutOfRangeError(ER_STAGE_OUT_OF_RANGE, m, totalTrees);

  var n := X.RowCount;
  var yPred := new Vector(n);

  for var i := 0 to n - 1 do
    yPred[i] := fInitValue;

  for var t := 0 to m - 1 do
  begin
    var delta := fEstimators[t].Predict(X);

    if delta.Length <> n then
      DimensionError(ER_XY_SIZE_MISMATCH, delta.Length, n);

    for var i := 0 to n - 1 do
    begin
      var v := yPred[i] + fLearningRate * delta[i];

      if double.IsNaN(v) or double.IsInfinity(v) then
        ArgumentError(ER_TRAINING_DIVERGED);

      yPred[i] := v;
    end;
  end;

  Result := yPred;
end;

function GradientBoostingRegressor.StagedPredict(X: Matrix): sequence of Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);

  var n := X.RowCount;
  var yPred := new Vector(n);

  for var i := 0 to n - 1 do
    yPred[i] := fInitValue;

  for var t := 0 to fEstimators.Count - 1 do
  begin
    var delta := fEstimators[t].Predict(X);

    if delta.Length <> n then
      DimensionError(ER_XY_SIZE_MISMATCH, delta.Length, n);

    for var i := 0 to n - 1 do
    begin
      var v := yPred[i] + fLearningRate * delta[i];

      if double.IsNaN(v) or double.IsInfinity(v) then
        ArgumentError(ER_TRAINING_DIVERGED);

      yPred[i] := v;
    end;

    yield yPred.Clone;   // ВАЖНО: clone!
  end;
end;

function GradientBoostingRegressor.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fFeatureImportances <> nil then
    exit(fFeatureImportances.Clone);   // НЕ отдаём внутренний кэш

  var importances := new Vector(fFeatureCount);

  foreach var tree in fEstimators do
  begin
    var imp := tree.FeatureImportances;

    if imp = nil then
      continue;

    if imp.Length <> fFeatureCount then
      DimensionError(ER_FEATURE_COUNT_MISMATCH, imp.Length, fFeatureCount);

    for var j := 0 to fFeatureCount - 1 do
    begin
      var v := imp[j];

      if double.IsNaN(v) or double.IsInfinity(v) then
        ArgumentError(ER_TRAINING_DIVERGED);

      importances[j] += v;
    end;
  end;

  var s := importances.Sum;

  if double.IsNaN(s) or double.IsInfinity(s) then
    ArgumentError(ER_TRAINING_DIVERGED);

  if Abs(s) > 1e-15 then
    for var j := 0 to fFeatureCount - 1 do
      importances[j] /= s;

  fFeatureImportances := importances;

  Result := fFeatureImportances.Clone;   // безопасный возврат
end;

function GradientBoostingRegressor.ToString: string;
begin
  var s :=
    $'GradientBoostingRegressor(' +
    $'n={fNEstimators}, ' +
    $'lr={fLearningRate}, ' +
    $'maxDepth={fMaxDepth}, ' +
    $'loss={fLoss}';

  if fSubsample <> 1.0 then
    s += $', subs={fSubsample}';

  if fEarlyStoppingPatience > 0 then
    s += $', earlyStop={fEarlyStoppingPatience}';

  if fLoss = TGBLoss.Huber then
    s += $', delta={fHuberDelta}';

  if fLoss = TGBLoss.Quantile then
    s += $', alpha={fQuantileAlpha}';

  if fLeafL2 <> 0.0 then
    s += $', leafL2={fLeafL2}';

  if fUseOOBEarlyStopping then
    s += ', OOB=true';

  if fUserProvidedSeed then
    s += $', seed={fRandomSeed}';

  s += ')';

  Result := s;
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
    fLoss,
    fHuberDelta,
    fEarlyStoppingPatience,
    fQuantileAlpha,
    fLeafL2,
    fUseOOBEarlyStopping,
    fRandomSeed
  );

  // --- seed ---
  copy.fRandomSeed := fRandomSeed;
  copy.fUserProvidedSeed := fUserProvidedSeed;

  if fUserProvidedSeed then
    copy.fRng := new System.Random(fRandomSeed)
  else
    copy.fRng := new System.Random;

  // --- basic state ---
  copy.fInitValue := fInitValue;
  copy.fFeatureCount := fFeatureCount;
  copy.fFitted := fFitted;

  // --- best iteration ---
  copy.fBestIteration := fBestIteration;
  copy.fBestTrainLoss := fBestTrainLoss;
  copy.fBestScoreLoss := fBestScoreLoss;

  // --- histories ---
  copy.fTrainLossHistory.Clear;
  copy.fTrainLossHistory.AddRange(fTrainLossHistory);

  copy.fValLossHistory.Clear;
  copy.fValLossHistory.AddRange(fValLossHistory);

  copy.fOOBLossHistory.Clear;
  copy.fOOBLossHistory.AddRange(fOOBLossHistory);

  // --- estimators ---
  copy.fEstimators.Clear;
  foreach var tree in fEstimators do
    copy.fEstimators.Add(tree.Clone as DecisionTreeRegressor);

  Result := copy;
end;


//-----------------------------
// GradientBoostingClassifier 
//-----------------------------

function GradientBoostingClassifier.FitInternal(
  XTrain: Matrix; yTrain: Vector;
  XVal: Matrix; yVal: Vector;
  useValidation: boolean): ISupervisedModel;
begin
  // --- null checks ---
  if XTrain = nil then
    ArgumentNullError(ER_X_NULL);

  if yTrain = nil then
    ArgumentNullError(ER_Y_NULL);

  // --- finite checks ---
  if ValidateFiniteInputs then
  begin
    CheckXForFit(XTrain);
    CheckYForFit(yTrain);
  end;

  // --- shape checks ---
  if XTrain.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if XTrain.RowCount <> yTrain.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if useValidation then
  begin
    if XVal = nil then
      ArgumentNullError(ER_X_NULL);

    if yVal = nil then
      ArgumentNullError(ER_Y_NULL);

    if ValidateFiniteInputs then
    begin
      CheckXForPredict(XVal);
      CheckYForFit(yVal);
    end;

    if XVal.RowCount <> yVal.Length then
      DimensionError(ER_XY_SIZE_MISMATCH);

    if XVal.ColCount <> XTrain.ColCount then
      DimensionError(ER_FEATURE_COUNT_MISMATCH);
  end;

  // --- reset state
  fOOBLossHistory.Clear;
  fEstimators.Clear;
  fTrainLossHistory.Clear;
  fValLossHistory.Clear;

  fFeatureCount := XTrain.ColCount;
  fBestIteration := -1;
  fBestScoreLoss := real.PositiveInfinity;
  fFitted := false;

  // --- mapping
  BuildClassMapping(yTrain);
  var yEncoded := ApplyLabelEncoding(yTrain);

  var nTrain := XTrain.RowCount;
  var classCount := fClassCount;
  
  // --- compute class priors
  fInitLogits := new real[classCount];
  
  var counts := new integer[classCount];
  
  for var i := 0 to nTrain - 1 do
    counts[yEncoded[i]] += 1;
  
  for var cls := 0 to classCount - 1 do
  begin
    var pi := counts[cls] / nTrain;
  
    if pi <= 0 then
      fInitLogits[cls] := -20.0   // защита от log(0)
    else
      fInitLogits[cls] := Ln(pi);
  end;
  
  // --- init logits
  var logitsTrain := new Matrix(nTrain, classCount);
  
  for var i := 0 to nTrain - 1 do
    for var cls := 0 to classCount - 1 do
      logitsTrain[i, cls] := fInitLogits[cls];
    
  // --- OOB init 
  var useOOB := (not useValidation) and (fSubsample < 1.0);
  
  var logitsOOB: Matrix := nil;
  var oobCount: array of integer;
  
  if useOOB then
  begin
    logitsOOB := new Matrix(nTrain, classCount);
    SetLength(oobCount, nTrain);
  
    for var i := 0 to nTrain - 1 do
    begin
      oobCount[i] := 0;
      for var cls := 0 to classCount - 1 do
        logitsOOB[i, cls] := fInitLogits[cls];
    end;
  end;

  var logitsVal: Matrix := nil;
  var yValEncoded: array of integer;

  if useValidation then
  begin
    logitsVal := new Matrix(XVal.RowCount, classCount);
    yValEncoded := ApplyLabelEncoding(yVal);
  
    // --- инициализация log-prior
    for var i := 0 to XVal.RowCount - 1 do
      for var cls := 0 to classCount - 1 do
        logitsVal[i, cls] := fInitLogits[cls];
  end;

  var noImprove := 0;

  // --- boosting loop
  for var iter := 0 to fNEstimators - 1 do
  begin
    // --- compute probabilities (train)
    var probsTrain := new Matrix(nTrain, classCount);
    SoftmaxMatrix(logitsTrain, probsTrain);

    // --- compute residuals
    var residuals := new Matrix(nTrain, classCount);

    for var i := 0 to nTrain - 1 do
      for var cls := 0 to classCount - 1 do
      begin
        var yik := 0.0;
        if yEncoded[i] = cls then
          yik := 1.0;

        residuals[i, cls] := yik - probsTrain[i, cls];
      end;
      
    // добавление
    // --- subsample rows
    var useSubsample := fSubsample < 1.0;
    var subIndices: array of integer := nil;
    
    // inBag
    var inBag: array of boolean := nil;
    
    if useSubsample then
    begin
      subIndices := BuildSubsampleIndices(nTrain, fSubsample, fRng);
    end;
    
    if useOOB then
    begin
      SetLength(inBag, nTrain);
    
      if useSubsample then
      begin
        for var i := 0 to subIndices.Length - 1 do
          inBag[subIndices[i]] := true;
      end
      else
      begin
        for var i := 0 to nTrain - 1 do
          inBag[i] := true;
      end;
    end;

    // --- train trees
    var trees := new DecisionTreeRegressor[classCount];

    for var cls := 0 to classCount - 1 do
    begin
      var rvec := new Vector(nTrain);
      for var i := 0 to nTrain - 1 do
        rvec[i] := residuals[i, cls];
      
      var stageSeed := fRng.Next(integer.MaxValue);

      var tree := new DecisionTreeRegressor(
        fMaxDepth,
        fMinSamplesSplit,
        fMinSamplesLeaf,
        seed := stageSeed
      );
      
      // добавление. Тренировка по идее будет происходить по строкам в subIndices
      if useSubsample then
        tree.SetRowIndices(subIndices);

      tree.Fit(XTrain, rvec);
      trees[cls] := tree;

      var deltaTrain := tree.Predict(XTrain);

      for var i := 0 to nTrain - 1 do
        logitsTrain[i, cls] += fLearningRate * deltaTrain[i];

      // --- OOB update ---
      if useOOB then
      begin
        for var i := 0 to nTrain - 1 do
          if not inBag[i] then
          begin
            logitsOOB[i, cls] += fLearningRate * deltaTrain[i];
            oobCount[i] += 1;
          end;
      end;
      
      // --- VALIDATION update ---
      if useValidation then
      begin
        var deltaVal := tree.Predict(XVal);
        for var i := 0 to XVal.RowCount - 1 do
          logitsVal[i, cls] += fLearningRate * deltaVal[i];
      end;
    end;

    fEstimators.Add(trees);

    // --- compute loss
    SoftmaxMatrix(logitsTrain, probsTrain);
    var trainLoss := ComputeLogLoss(yEncoded, probsTrain);
    fTrainLossHistory.Add(trainLoss);

    var scoreLoss := trainLoss;

    if useValidation then
    begin
      var probsVal := new Matrix(XVal.RowCount, classCount);
      SoftmaxMatrix(logitsVal, probsVal);
    
      var valLoss := ComputeLogLoss(yValEncoded, probsVal);
      fValLossHistory.Add(valLoss);
    
      scoreLoss := valLoss;
    end
    else if useOOB then
    begin
      var mask := new boolean[nTrain];
      var cnt := 0;
    
      for var i := 0 to nTrain - 1 do
      begin
        mask[i] := oobCount[i] > 0;
        if mask[i] then
          cnt += 1;
      end;
    
      if cnt >= Max(1, nTrain div 10) then
      begin
        scoreLoss := ComputeLogLossMasked(yEncoded, logitsOOB, mask);
        fOOBLossHistory.Add(scoreLoss);
      end
      else
        scoreLoss := trainLoss;
    end;

    // --- early stopping
    if fEarlyStoppingPatience > 0 then
    begin
      if fBestScoreLoss - scoreLoss > MinImprovement then
      begin
        fBestScoreLoss := scoreLoss;
        fBestIteration := iter;
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

  // --- cut trees after best iteration
  if (fEarlyStoppingPatience > 0) and (fBestIteration >= 0) then
  begin
    var keep := fBestIteration + 1;
    if fEstimators.Count > keep then
      fEstimators.RemoveRange(keep, fEstimators.Count - keep);
    if fOOBLossHistory.Count > keep then
      fOOBLossHistory.RemoveRange(keep, fOOBLossHistory.Count - keep);
  end;

  fFitted := true;
  Result := Self;
end;

procedure GradientBoostingClassifier.BuildClassMapping(y: Vector);
begin
  var classes := new HashSet<integer>;

  for var i := 0 to y.Length - 1 do
  begin
    var r := y[i];
    var ir := Round(r);

    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);

    classes.Add(ir);
  end;

  fClasses := classes.ToArray;
  &Array.Sort(fClasses);

  fClassCount := fClasses.Length;

  fClassIndex := new Dictionary<integer, integer>;
  for var cls := 0 to fClassCount - 1 do
    fClassIndex[fClasses[cls]] := cls;
end;

function GradientBoostingClassifier.ApplyLabelEncoding(y: Vector): array of integer;
begin
  var n := y.Length;
  Result := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var ir := Round(y[i]);
    Result[i] := fClassIndex[ir];
  end;
end;

procedure GradientBoostingClassifier.SoftmaxRow(
  var logits: array of real;
  var probs: array of real);
begin
  var classCount := Length(logits);

  if classCount = 0 then
    ArgumentError(ER_SOFTMAX_EMPTY);

  if Length(probs) <> classCount then
    DimensionError(ER_XY_SIZE_MISMATCH, Length(probs), classCount);

  // --- проверка входных логитов
  for var cls := 0 to classCount - 1 do
  begin
    var v := logits[cls];

    if double.IsNaN(v) or double.IsInfinity(v) then
      ArgumentError(ER_TRAINING_DIVERGED);
  end;

  // --- max trick
  var maxLogit := logits[0];
  for var cls := 1 to classCount - 1 do
    if logits[cls] > maxLogit then
      maxLogit := logits[cls];

  var sumExp := 0.0;

  for var cls := 0 to classCount - 1 do
  begin
    var shifted := logits[cls] - maxLogit;
    var e := Exp(shifted);

    if double.IsNaN(e) or double.IsInfinity(e) then
      ArgumentError(ER_TRAINING_DIVERGED);

    probs[cls] := e;
    sumExp += e;
  end;

  if double.IsNaN(sumExp) or double.IsInfinity(sumExp) then
    ArgumentError(ER_TRAINING_DIVERGED);

  if sumExp <= 0 then
  begin
    var uniform := 1.0 / classCount;
    for var cls := 0 to classCount - 1 do
      probs[cls] := uniform;
    exit;
  end;

  var inv := 1.0 / sumExp;

  for var cls := 0 to classCount - 1 do
  begin
    var p := probs[cls] * inv;

    // clamp для полной численной безопасности
    if p < 0 then
      p := 0.0
    else if p > 1 then
      p := 1.0;

    probs[cls] := p;
  end;
end;

procedure GradientBoostingClassifier.SoftmaxMatrix(
  logits: Matrix;
  probs: Matrix);
begin
  var nSamples := logits.RowCount;
  var classCount := logits.ColCount;

  for var i := 0 to nSamples - 1 do
  begin
    var rowLogits := new real[classCount];
    var rowProbs  := new real[classCount];

    for var cls := 0 to classCount - 1 do
      rowLogits[cls] := logits[i, cls];

    SoftmaxRow(rowLogits, rowProbs);

    for var cls := 0 to classCount - 1 do
      probs[i, cls] := rowProbs[cls];
  end;
end;

function GradientBoostingClassifier.ComputeLogLoss(
  yEncoded: array of integer;
  probs: Matrix): real;
begin
  var n := Length(yEncoded);

  if n = 0 then
    exit(0.0);

  if probs = nil then
    ArgumentNullError(ER_X_NULL);

  if probs.RowCount <> n then
    DimensionError(ER_XY_SIZE_MISMATCH, probs.RowCount, n);

  var eps := 1e-12;
  var loss := 0.0;

  for var i := 0 to n - 1 do
  begin
    var cls := yEncoded[i];

    if (cls < 0) or (cls >= probs.ColCount) then
      ArgumentError(ER_LABEL_INDEX_INVALID);

    var p := probs[i, cls];

    if double.IsNaN(p) or double.IsInfinity(p) then
      ArgumentError(ER_TRAINING_DIVERGED);

    if p < eps then
      p := eps
    else if p > 1 - eps then
      p := 1 - eps;

    loss -= Ln(p);
  end;

  var resultLoss := loss / n;

  if resultLoss < 0 then
    resultLoss := 0.0;

  if double.IsNaN(resultLoss) or double.IsInfinity(resultLoss) then
    ArgumentError(ER_TRAINING_DIVERGED);

  Result := resultLoss;
end;

function GradientBoostingClassifier.ComputeLogLossMasked(
  yEncoded: array of integer;
  logits: Matrix;
  mask: array of boolean): real;
begin
  if logits = nil then
    ArgumentNullError(ER_X_NULL);

  var n := logits.RowCount;
  var k := logits.ColCount;

  if k = 0 then
    ArgumentError(ER_SOFTMAX_EMPTY);

  if Length(yEncoded) <> n then
    DimensionError(ER_XY_SIZE_MISMATCH, Length(yEncoded), n);

  if Length(mask) <> n then
    DimensionError(ER_XY_SIZE_MISMATCH, Length(mask), n);

  var rowLogits := new real[k];
  var rowProbs := new real[k];

  var sum := 0.0;
  var cnt := 0;
  var eps := 1e-12;

  for var i := 0 to n - 1 do
  begin
    if not mask[i] then
      continue;

    for var cls := 0 to k - 1 do
      rowLogits[cls] := logits[i, cls];

    SoftmaxRow(rowLogits, rowProbs);

    var yi := yEncoded[i];

    if (yi < 0) or (yi >= k) then
      ArgumentError(ER_LABEL_INDEX_INVALID);

    var p := rowProbs[yi];

    if double.IsNaN(p) or double.IsInfinity(p) then
      ArgumentError(ER_TRAINING_DIVERGED);

    if p < eps then
      p := eps
    else if p > 1 - eps then
      p := 1 - eps;

    sum -= Ln(p);
    cnt += 1;
  end;

  if cnt = 0 then
    exit(real.PositiveInfinity);  // корректно для OOB

  var loss := sum / cnt;

  if loss < 0 then
    loss := 0.0;

  if double.IsNaN(loss) or double.IsInfinity(loss) then
    ArgumentError(ER_TRAINING_DIVERGED);

  Result := loss;
end;

constructor GradientBoostingClassifier.Create(
  nEstimators: integer;
  learningRate: real;
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  subsample: real;
  earlyStoppingPatience: integer;
  seed: integer);
begin
  if nEstimators <= 0 then
  ArgumentOutOfRangeError(ER_N_ESTIMATORS_INVALID, nEstimators);
  
  if learningRate <= 0 then
    ArgumentOutOfRangeError(ER_LEARNING_RATE_INVALID, learningRate);
  
  if maxDepth <= 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, maxDepth);
  
  if minSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_SPLIT_INVALID, minSamplesSplit);
  
  if minSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_LEAF_INVALID, minSamplesLeaf);
  
  if (subsample <= 0) or (subsample > 1) then
    ArgumentOutOfRangeError(ER_SUBSAMPLE_OUT_OF_RANGE, subsample);
  
  if earlyStoppingPatience < 0 then
    ArgumentOutOfRangeError(ER_EARLY_STOPPING_INVALID, earlyStoppingPatience);

  fNEstimators := nEstimators;
  fLearningRate := learningRate;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fSubsample := subsample;
  fEarlyStoppingPatience := earlyStoppingPatience;

  fEstimators := new List<array of DecisionTreeRegressor>;
  fTrainLossHistory := new List<real>;
  fValLossHistory := new List<real>;

  fFitted := false;
  fFeatureCount := 0;
  fClassCount := 0;

  fBestIteration := -1;
  fBestScoreLoss := real.PositiveInfinity;
  
  fOOBLossHistory := new List<real>;
  
  if seed < 0 then
  begin
    fUserProvidedSeed := False;
    fRandomSeed := System.Environment.TickCount and integer.MaxValue;
  end
  else
  begin
    fUserProvidedSeed := True;
    fRandomSeed := seed;
  end;

  fRng := new System.Random(fRandomSeed);
end;

function GradientBoostingClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var nSamples := X.RowCount;
  var classCount := fClassCount;

  var logits := new Matrix(nSamples, classCount);

  // --- F0
  for var i := 0 to nSamples - 1 do
    for var cls := 0 to classCount - 1 do
      logits[i, cls] := fInitLogits[cls];

  // --- накопление логитов
  foreach var trees in fEstimators do
    for var cls := 0 to classCount - 1 do
    begin
      var delta := trees[cls].Predict(X);

      for var i := 0 to nSamples - 1 do
        logits[i, cls] += fLearningRate * delta[i];
    end;

  var probs := new Matrix(nSamples, classCount);

  // --- устойчивый softmax
  for var i := 0 to nSamples - 1 do
  begin
    var maxVal := logits[i,0];
    for var cls := 1 to classCount - 1 do
      if logits[i,cls] > maxVal then
        maxVal := logits[i,cls];

    var sumExp := 0.0;

    for var cls := 0 to classCount - 1 do
    begin
      probs[i,cls] := Exp(logits[i,cls] - maxVal);
      sumExp += probs[i,cls];
    end;

    if Abs(sumExp) < 1e-12 then
    begin
      // численно вырожденный случай
      var uniform := 1.0 / classCount;
      for var cls := 0 to classCount - 1 do
        probs[i,cls] := uniform;
    end
    else
    begin
      for var cls := 0 to classCount - 1 do
        probs[i,cls] /= sumExp;
    end;
  end;

  Result := probs;
end;

function GradientBoostingClassifier.GetClasses: array of real;
begin
  SetLength(Result, fClassCount);
  for var i := 0 to fClassCount - 1 do
    Result[i] := fClasses[i];
end;

function GradientBoostingClassifier.PredictStageProba(
  X: Matrix; m: integer): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var total := fEstimators.Count;

  if (m < 0) or (m > total) then
    ArgumentOutOfRangeError(ER_STAGE_OUT_OF_RANGE, m, total);

  var n := X.RowCount;
  var k := fClassCount;

  var logits := new Matrix(n, k);

  // --- F0
  for var i := 0 to n - 1 do
    for var cls := 0 to k - 1 do
      logits[i, cls] := fInitLogits[cls];

  // --- add first m boosting stages
  for var t := 0 to m - 1 do
  begin
    var trees := fEstimators[t];

    for var cls := 0 to k - 1 do
    begin
      var delta := trees[cls].Predict(X);

      for var i := 0 to n - 1 do
        logits[i, cls] += fLearningRate * delta[i];
    end;
  end;

  var probs := new Matrix(n, k);
  SoftmaxMatrix(logits, probs);

  Result := probs;
end;

function GradientBoostingClassifier.PredictStage(X: Matrix; m: integer): Vector;
begin
  var probs := PredictStageProba(X, m);

  var n := probs.RowCount;
  var resultVec := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    var best := 0;
    var bestVal := probs[i,0];

    for var cls := 1 to probs.ColCount - 1 do
      if probs[i,cls] > bestVal then
      begin
        bestVal := probs[i,cls];
        best := cls;
      end;

    resultVec[i] := fClasses[best];
  end;

  Result := resultVec;
end;

function GradientBoostingClassifier.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fFeatureImportances <> nil then
    exit(fFeatureImportances);

  var importances := new Vector(fFeatureCount);

  foreach var trees in fEstimators do
  begin
    for var cls := 0 to fClassCount - 1 do
    begin
      var imp := trees[cls].FeatureImportances;

      for var j := 0 to fFeatureCount - 1 do
        importances[j] += imp[j];
    end;
  end;

  // нормализация
  var s := importances.Sum;
  if s > 0 then
    for var j := 0 to fFeatureCount - 1 do
      importances[j] /= s;

  fFeatureImportances := importances;

  Result := fFeatureImportances;
end;

function GradientBoostingClassifier.ToString: string;
begin
  var s :=
    $'GradientBoostingClassifier(' +
    $'n={fNEstimators}, ' +
    $'lr={fLearningRate}, ' +
    $'maxDepth={fMaxDepth}';

  if fSubsample <> 1.0 then
    s += $', subs={fSubsample}';

  if fEarlyStoppingPatience > 0 then
    s += $', earlyStop={fEarlyStoppingPatience}';

  if fUserProvidedSeed then
    s += $', seed={fRandomSeed}';

  s += ')';

  Result := s;
end;

function GradientBoostingClassifier.Clone: IModel;
begin
  var model := new GradientBoostingClassifier(
    fNEstimators,
    fLearningRate,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fSubsample,
    fEarlyStoppingPatience,
    fRandomSeed
  );

  // --- seed ---
  model.fRandomSeed := fRandomSeed;
  model.fUserProvidedSeed := fUserProvidedSeed;

  if fUserProvidedSeed then
    model.fRng := new System.Random(fRandomSeed)
  else
    model.fRng := new System.Random;

  // --- fitted state ---
  model.fFitted := fFitted;
  model.fFeatureCount := fFeatureCount;
  model.fClassCount := fClassCount;

  // --- classes ---
  if fClasses <> nil then
  begin
    SetLength(model.fClasses, Length(fClasses));
    for var i := 0 to Length(fClasses) - 1 do
      model.fClasses[i] := fClasses[i];
  end;

  if fClassIndex <> nil then
  begin
    model.fClassIndex := new Dictionary<integer, integer>;
    foreach var kv in fClassIndex do
      model.fClassIndex.Add(kv.Key, kv.Value);
  end;

  // --- estimators ---
  foreach var trees in fEstimators do
  begin
    var newTrees := new DecisionTreeRegressor[Length(trees)];
    for var cls := 0 to Length(trees) - 1 do
      newTrees[cls] := trees[cls].Clone as DecisionTreeRegressor;
    model.fEstimators.Add(newTrees);
  end;

  // --- histories ---
  foreach var v in fTrainLossHistory do model.fTrainLossHistory.Add(v);
  foreach var v in fValLossHistory do model.fValLossHistory.Add(v);
  foreach var v in fOOBLossHistory do model.fOOBLossHistory.Add(v);

  model.fBestIteration := fBestIteration;
  model.fBestScoreLoss := fBestScoreLoss;

  Result := model;
end;

function GradientBoostingClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  Result := FitInternal(X, y, nil, nil, false);
end;

function GradientBoostingClassifier.FitWithValidation(
  XTrain: Matrix; yTrain: Vector;
  XVal: Matrix; yVal: Vector): IModel;
begin
  Result := FitInternal(XTrain, yTrain, XVal, yVal, true);
end;

function GradientBoostingClassifier.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var probs := PredictProba(X);

  var nSamples := X.RowCount;
  var classCount := fClassCount;

  Result := new Vector(nSamples);

  for var i := 0 to nSamples - 1 do
  begin
    var bestClassIndex := 0;
    var bestValue := probs[i, 0];

    for var cls := 1 to classCount - 1 do
      if probs[i, cls] > bestValue then
      begin
        bestValue := probs[i, cls];
        bestClassIndex := cls;
      end;

    // возвращаем оригинальную метку
    Result[i] := fClasses[bestClassIndex];
  end;
end;

function GradientBoostingClassifier.PredictLabels(X: Matrix): array of integer;
begin
  var v := Predict(X);
  
  Result := new integer[v.Length];
  
  for var i := 0 to v.Length - 1 do
    Result[i] := integer(v[i]);
end;

procedure GradientBoostingClassifier.SetClassLabels(classes: array of string);
begin
  fClassLabels := Copy(classes);
end;

function GradientBoostingClassifier.GetClassLabels: array of string;
begin
  if fClassLabels = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := fClassLabels;
end;

//-----------------------------
//          KNNBase 
//-----------------------------

constructor KNNBase.Create(k: integer; weighting: KNNWeighting);
begin
  if k < 1 then
    ArgumentOutOfRangeError(ER_K_MUST_BE_POSITIVE);

  fK := k;
  fWeighting := weighting;
  fFitted := False;
end;

procedure KNNBase.ValidatePredictInput(X: Matrix);
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fXTrain.ColCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);
end;

function KNNBase.SquaredL2(trainRow: integer; XTest: Matrix; testRow: integer): double;
begin
  var sum := 0.0;
  var d := fXTrain.ColCount;

  for var j := 0 to d - 1 do
  begin
    var diff := fXTrain[trainRow, j] - XTest[testRow, j];
    sum += diff * diff;
  end;

  exit(sum);
end;

// QuickSelect(fK - 1); - так вызываем

procedure KNNBase.QuickSelect(k: integer);
begin
  var left := 0;
  var right := fNeighbors.Length - 1;

  while true do
  begin
    var pivotIndex := Partition(left, right);

    if pivotIndex = k then
      exit
    else if pivotIndex > k then
      right := pivotIndex - 1
    else
      left := pivotIndex + 1;
  end;
end;

function KNNBase.Partition(left, right: integer): integer;
begin
  var pivot := fNeighbors[(left + right) div 2].dist;

  var i := left;
  var j := right;

  while true do
  begin
    while fNeighbors[i].dist < pivot do i += 1;
    while fNeighbors[j].dist > pivot do j -= 1;

    if i >= j then
      exit(j);

    Swap(fNeighbors[i], fNeighbors[j]);
    
    i += 1;
    j -= 1;
  end;
end;

//-----------------------------
//        KNNClassifier 
//-----------------------------

const KNN_EPS = 1e-12;

constructor KNNClassifier.Create(k: integer; weighting: KNNWeighting);
begin
  inherited Create(k, weighting);
end;

procedure KNNClassifier.EncodeClasses(y: Vector);
begin
  var n := y.Length;

  // собрать уникальные значения
  var hs := new HashSet<double>;
  for var i := 0 to n - 1 do
  begin
    var v := y[i];
    if double.IsNaN(v) then
      ArgumentError(ER_NAN_IN_Y);
    hs.Add(v);
  end;

  fClasses := hs.ToArray;
  &Array.Sort(fClasses);

  fClassCount := fClasses.Length;

  // построить map label -> index
  var dict := new Dictionary<double, integer>;
  for var i := 0 to fClassCount - 1 do
    dict[fClasses[i]] := i;

  SetLength(fYEnc, n);

  for var i := 0 to n - 1 do
    fYEnc[i] := dict[y[i]];
end;

function KNNClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fK > X.RowCount then
    ArgumentOutOfRangeError(ER_K_EXCEEDS_SAMPLES);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  // копия train data
  fXTrain := X.Clone;  // предполагаем, что Clone делает глубокую копию

  // кодирование классов
  EncodeClasses(y);

  var n := fXTrain.RowCount;
  var C := fClassCount;

  // выделение буферов
  SetLength(fNeighbors, n);

  SetLength(fVotes, C);
  SetLength(fMark, C);
  SetLength(fTouched, C);
  fEpoch := 0;

  fFitted := true;

  exit(self);
end;

function KNNClassifier.GetClasses: array of real;
begin
  Result := fClasses;
end;

function KNNClassifier.Clone: IModel;
begin
  var clone := new KNNClassifier(fK, fWeighting);

  if fFitted then
  begin
    clone.fXTrain := fXTrain.Clone;
    clone.fClasses := fClasses.Clone as array of double;
    clone.fYEnc := fYEnc.Clone as array of integer;

    clone.fClassCount := fClassCount;

    SetLength(clone.fNeighbors, fNeighbors.Length);
    SetLength(clone.fVotes, fVotes.Length);
    SetLength(clone.fMark, fMark.Length);
    SetLength(clone.fTouched, fTouched.Length);

    clone.fEpoch := 0;
    clone.fFitted := true;
  end;

  Result := clone;
end;

function KNNClassifier.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  ValidatePredictInput(X);

  var m := X.RowCount;
  var n := fXTrain.RowCount;

  Result := new Vector(m);

  for var i := 0 to m - 1 do
  begin
    // заполнить расстояния
    for var t := 0 to n - 1 do
    begin
      fNeighbors[t].dist := SquaredL2(t, X, i);
      fNeighbors[t].idx := t;
    end;

    // выбрать k ближайших
    QuickSelect(fK - 1);

    // exact match: если среди k ближайших есть dist=0, возвращаем его класс
    var exactCls := -1;
    for var t := 0 to fK - 1 do
      if fNeighbors[t].dist < KNN_EPS then
      begin
        exactCls := fYEnc[fNeighbors[t].idx];
        break;
      end;

    if exactCls <> -1 then
    begin
      Result[i] := fClasses[exactCls];
      continue;
    end;

    // voting (stamping)
    fEpoch += 1;
    var touchCount := 0;

    if fWeighting = Uniform then
    begin
      for var t := 0 to fK - 1 do
      begin
        var trainIdx := fNeighbors[t].idx;
        var cls := fYEnc[trainIdx];

        if fMark[cls] <> fEpoch then
        begin
          fMark[cls] := fEpoch;
          fVotes[cls] := 0.0;
          fTouched[touchCount] := cls;
          touchCount += 1;
        end;

        fVotes[cls] += 1.0;
      end;
    end
    else
    begin
      // weighted: веса 1 / dist (dist = squared distance)
      for var t := 0 to fK - 1 do
      begin
        var trainIdx := fNeighbors[t].idx;
        var cls := fYEnc[trainIdx];
        var dist := fNeighbors[t].dist;

        if fMark[cls] <> fEpoch then
        begin
          fMark[cls] := fEpoch;
          fVotes[cls] := 0.0;
          fTouched[touchCount] := cls;
          touchCount += 1;
        end;

        if dist < KNN_EPS then
        begin
          // считаем как exact match
          exactCls := cls;
          break;
        end
        else
        begin
          var w := 1.0 / Sqrt(dist);
          fVotes[cls] += w;
        end;
      end;
    end;
    
    if exactCls <> -1 then
    begin
      Result[i] := fClasses[exactCls];
      continue;
    end;

    // argmax только по touched
    var bestCls := fTouched[0];
    var bestVotes := fVotes[bestCls];

    for var k2 := 1 to touchCount - 1 do
    begin
      var cls := fTouched[k2];
      var v := fVotes[cls];

      if (v > bestVotes) or ((v = bestVotes) and (cls < bestCls)) then
      begin
        bestCls := cls;
        bestVotes := v;
      end;
    end;

    Result[i] := fClasses[bestCls];
  end;
end;

function KNNClassifier.PredictLabels(X: Matrix): array of integer;
begin
  var v := Predict(X);
  
  Result := new integer[v.Length];
  
  for var i := 0 to v.Length - 1 do
    Result[i] := integer(v[i]);
end;

function KNNClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  ValidatePredictInput(X);

  var m := X.RowCount;
  var n := fXTrain.RowCount;

  Result := new Matrix(m, fClassCount); // предполагаем нулевую инициализацию

  for var i := 0 to m - 1 do
  begin
    // заполнить расстояния
    for var t := 0 to n - 1 do
    begin
      fNeighbors[t].dist := SquaredL2(t, X, i);
      fNeighbors[t].idx := t;
    end;

    // выбрать k ближайших
    QuickSelect(fK - 1);

    // exact match: если среди k ближайших есть dist=0, вероятность 1 у его класса
    var exactCls := -1;
    for var t := 0 to fK - 1 do
      if fNeighbors[t].dist < KNN_EPS then
      begin
        exactCls := fYEnc[fNeighbors[t].idx];
        break;
      end;

    if exactCls <> -1 then
    begin
      Result[i, exactCls] := 1.0;
      continue;
    end;

    // voting (stamping)
    fEpoch += 1;
    var touchCount := 0;

    if fWeighting = Uniform then
    begin
      for var t := 0 to fK - 1 do
      begin
        var trainIdx := fNeighbors[t].idx;
        var cls := fYEnc[trainIdx];

        if fMark[cls] <> fEpoch then
        begin
          fMark[cls] := fEpoch;
          fVotes[cls] := 0.0;
          fTouched[touchCount] := cls;
          touchCount += 1;
        end;

        fVotes[cls] += 1.0;
      end;

      // нормализация: сумма = k
      for var k2 := 0 to touchCount - 1 do
      begin
        var cls := fTouched[k2];
        Result[i, cls] := fVotes[cls] / fK;
      end;
    end
    else
    begin
      var sumW := 0.0;
    
      for var t := 0 to fK - 1 do
      begin
        var trainIdx := fNeighbors[t].idx;
        var cls := fYEnc[trainIdx];
        var dist := fNeighbors[t].dist;
    
        if dist < KNN_EPS then
        begin
          exactCls := cls;
          break;
        end;
    
        if fMark[cls] <> fEpoch then
        begin
          fMark[cls] := fEpoch;
          fVotes[cls] := 0.0;
          fTouched[touchCount] := cls;
          touchCount += 1;
        end;
    
        var w := 1.0 / Sqrt(dist);   // согласовано с Predict
        fVotes[cls] += w;
        sumW += w;
      end;
    
      // 🔴 ОБЯЗАТЕЛЬНО проверить exact снова
    
      if exactCls <> -1 then
      begin
        Result[i, exactCls] := 1.0;
        continue;
      end;
    
      // нормализация
      if sumW > 0 then
        for var k2 := 0 to touchCount - 1 do
        begin
          var cls := fTouched[k2];
          Result[i, cls] := fVotes[cls] / sumW;
        end
      else
      begin
        // fallback — равномерное распределение
        var uniform := 1.0 / fK;
        for var k2 := 0 to touchCount - 1 do
        begin
          var cls := fTouched[k2];
          Result[i, cls] := uniform;
        end;
      end;  
    end;
  end;
end;

procedure KNNClassifier.SetClassLabels(classes: array of string);
begin
  fClassLabels := Copy(classes);
end;

function KNNClassifier.GetClassLabels: array of string;
begin
  if fClassLabels = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := fClassLabels;
end;    


//-----------------------------
//        KNNRegressor 
//-----------------------------

constructor KNNRegressor.Create(k: integer; weighting: KNNWeighting);
begin
  inherited Create(k, weighting);
end;

function KNNRegressor.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fK > X.RowCount then
    ArgumentOutOfRangeError(ER_K_EXCEEDS_SAMPLES);

  if ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  fXTrain := X.Clone;
  fYTrain := y.Clone;

  var n := fXTrain.RowCount;
  SetLength(fNeighbors, n);

  fFitted := true;
  Result := Self;
end;

function KNNRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  ValidatePredictInput(X);

  var m := X.RowCount;
  var n := fXTrain.RowCount;

  Result := new Vector(m);

  for var i := 0 to m - 1 do
  begin
    // --- вычисление расстояний
    for var t := 0 to n - 1 do
    begin
      fNeighbors[t].dist := SquaredL2(t, X, i);
      fNeighbors[t].idx := t;
    end;

    // --- выбрать k ближайших
    QuickSelect(fK - 1);

    // --- exact match
    var exactIdx := -1;
    for var t := 0 to fK - 1 do
      if fNeighbors[t].dist < KNN_EPS then
      begin
        exactIdx := fNeighbors[t].idx;
        break;
      end;

    if exactIdx <> -1 then
    begin
      Result[i] := fYTrain[exactIdx];
      continue;
    end;

    // --- uniform weighting
    if fWeighting = KNNWeighting.Uniform then
    begin
      var sum := 0.0;

      for var t := 0 to fK - 1 do
        sum += fYTrain[fNeighbors[t].idx];

      Result[i] := sum / fK;
    end
    else
    begin
      // --- distance weighting
      var sumW := 0.0;
      var sumWY := 0.0;
      var exactFound := false;
      var exactValue := 0.0;

      for var t := 0 to fK - 1 do
      begin
        var idx := fNeighbors[t].idx;
        var dist := fNeighbors[t].dist;

        if dist < KNN_EPS then
        begin
          exactFound := true;
          exactValue := fYTrain[idx];
          break;
        end;

        var w := 1.0 / Sqrt(dist);
        sumW += w;
        sumWY += w * fYTrain[idx];
      end;

      if exactFound then
      begin
        Result[i] := exactValue;
        continue;
      end;

      if sumW > 0 then
        Result[i] := sumWY / sumW
      else
      begin
        // fallback — обычное среднее
        var sum := 0.0;
        for var t := 0 to fK - 1 do
          sum += fYTrain[fNeighbors[t].idx];

        Result[i] := sum / fK;
      end;
    end;
  end;
end;

function KNNRegressor.Clone: IModel;
begin
  var clone := new KNNRegressor(fK, fWeighting);

  if fFitted then
  begin
    clone.fXTrain := fXTrain.Clone;
    clone.fYTrain := fYTrain.Clone;

    SetLength(clone.fNeighbors, fNeighbors.Length);

    clone.fFitted := True;
  end;

  Result := clone;
end;

//-----------------------------
//           KMeans 
//-----------------------------

constructor KMeans.Create(
  nClusters: integer;
  maxIter: integer;
  tol: real;
  nInit: integer;
  seed: integer
);
begin
  if nClusters < 1 then
    ArgumentOutOfRangeError(ER_K_INVALID, nClusters);

  if maxIter < 1 then
    ArgumentOutOfRangeError(ER_MAXITER_INVALID, maxIter);

  if tol <= 0 then
    ArgumentOutOfRangeError(ER_TOL_INVALID, tol);

  if nInit < 1 then
    ArgumentOutOfRangeError(ER_NINIT_INVALID, nInit);

  fNClusters := nClusters;
  fMaxIter := maxIter;
  fTol := tol;
  fNInit := nInit;

  // --- seed (единый стиль)
  fRandomSeed := seed;
  fUserProvidedSeed := seed >= 0;

  if fUserProvidedSeed then
    fRng := new System.Random(seed)
  else
    fRng := new System.Random;

  // --- state
  fFitted := False;
  fFeatureCount := 0;

  fCenters := nil;
  fInertia := 0.0;
  fIterations := 0;
  fHasConverged := False;
end;

function KMeans.RunSingle(X: Matrix; rnd: System.Random): (Matrix, real, integer, boolean);
begin
  var n := X.RowCount;
  var p := X.ColCount;
  var k := fNClusters;

  // --- 1. Инициализация центроидов (случайные строки X)

  var idx := Arr(0..n-1);
  idx.Shuffle(rnd);

  var centers := new Matrix(k, p);

  for var c := 0 to k - 1 do
  begin
    var r := idx[c];
    for var j := 0 to p - 1 do
      centers[c,j] := X[r,j];
  end;

  var inertia := 0.0;
  var converged := False;
  var iterations := 0;

  // --- 2. Основной цикл
  for var iter := 1 to fMaxIter do
  begin
    iterations := iter;

    // Assignment + накопление сумм
    var counts := new integer[k];
    var sums := new Matrix(k, p);

    inertia := 0.0;

    for var i := 0 to n - 1 do
    begin
      var bestC := 0;
      var bestDist := double.MaxValue;

      for var c := 0 to k - 1 do
      begin
        var dist := 0.0;

        for var j := 0 to p - 1 do
        begin
          var xij := X[i,j];
          if double.IsNaN(xij) or double.IsInfinity(xij) then
            ArgumentError(ER_INVALID_VALUE_AT, 'X', i);
          
          var cj := centers[c,j];
          if double.IsNaN(cj) or double.IsInfinity(cj) then
            ArgumentError(ER_INVALID_VALUE_AT, 'Centers', c);
          
          var d := xij - cj;
          dist += d * d;
        end;

        if dist < bestDist then
        begin
          bestDist := dist;
          bestC := c;
        end;
      end;

      inertia += bestDist;

      counts[bestC] += 1;

      for var j := 0 to p - 1 do
        sums[bestC,j] += X[i,j];
    end;

    // --- 3. Пересчёт центроидов
    var maxShift := 0.0;

    for var c := 0 to k - 1 do
    begin
      if counts[c] = 0 then
      begin
        // Пустой кластер — переинициализация случайной точкой
        var r := rnd.Next(n);
        for var j := 0 to p - 1 do
          centers[c,j] := X[r,j];
        continue;
      end;

      for var j := 0 to p - 1 do
      begin
        var newVal := sums[c,j] / counts[c];
        var shift := Abs(newVal - centers[c,j]);

        if shift > maxShift then
          maxShift := shift;

        centers[c,j] := newVal;
      end;
    end;

    if maxShift < fTol then
    begin
      converged := True;
      break;
    end;
  end;

  Result := (centers, inertia, iterations, converged);
end;

function KMeans.Fit(X: Matrix): IUnsupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'KMeans');

  if p = 0 then
    ArgumentError(ER_EMPTY_DATA, 'KMeans');

  if fNClusters > n then
    ArgumentOutOfRangeError(ER_K_INVALID, fNClusters);

  fFeatureCount := p;

  // --- базовый RNG (уже создан в конструкторе)
  var rndBase := fRng;

  var bestInertia := 1e308;
  var bestCenters: Matrix := nil;
  var bestIterations := 0;
  var bestConverged := False;

  for var run := 1 to fNInit do
  begin
    // --- независимый RNG для каждого запуска
    var runSeed := rndBase.Next(integer.MaxValue);
    var rnd := new System.Random(runSeed);

    var (centers, inertia, iters, converged) := RunSingle(X, rnd);

    if inertia < bestInertia then
    begin
      bestInertia := inertia;
      bestCenters := centers;
      bestIterations := iters;
      bestConverged := converged;
    end;
  end;

  fCenters := bestCenters;
  fInertia := bestInertia;
  fIterations := bestIterations;
  fHasConverged := bestConverged;
  fFitted := True;

  Result := Self;
end;

function KMeans.PredictLabels(X: Matrix): array of integer;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');
  
  if X.ColCount <> fFeatureCount then
    DimensionError(ER_DIM_MISMATCH, X.ColCount, fFeatureCount);

  var n := X.RowCount;
  var p := X.ColCount;
  var k := fNClusters;

  SetLength(Result, n);

  for var i := 0 to n - 1 do
  begin
    var bestC := 0;
    var bestDist := 1e308;

    for var c := 0 to k - 1 do
    begin
      var dist := 0.0;

      for var j := 0 to p - 1 do
      begin
        var xij := X[i,j];
        if double.IsNaN(xij) or double.IsInfinity(xij) then
          ArgumentError(ER_INVALID_VALUE_AT, 'X', i);
        
        var cj := fCenters[c,j];
        if double.IsNaN(cj) or double.IsInfinity(cj) then
          ArgumentError(ER_INVALID_VALUE_AT, 'Centers', c);
        
        var d := xij - cj;
        dist += d * d;
      end;

      if dist < bestDist then
      begin
        bestDist := dist;
        bestC := c;
      end;
    end;

    Result[i] := bestC; // 0..k-1
  end;
end;

function KMeans.Predict(X: Matrix): Vector;
begin
  var labels := PredictLabels(X);

  var n := Length(labels);
  Result := new Vector(n);

  for var i := 0 to n - 1 do
    Result[i] := labels[i];
end;

function KMeans.FitPredict(X: Matrix): array of integer;
begin
  Fit(X);
  Result := PredictLabels(X);
end;

function KMeans.Clone: IModel;
begin
  var km := new KMeans(
    fNClusters,
    fMaxIter,
    fTol,
    fNInit,
    fRandomSeed
  );

  // --- seed ---
  km.fRandomSeed := fRandomSeed;
  km.fUserProvidedSeed := fUserProvidedSeed;

  if fUserProvidedSeed then
    km.fRng := new System.Random(fRandomSeed)
  else
    km.fRng := new System.Random;

  // --- state ---
  km.fFitted := fFitted;
  km.fFeatureCount := fFeatureCount;
  km.fInertia := fInertia;
  km.fIterations := fIterations;
  km.fHasConverged := fHasConverged;

  // --- centers ---
  if fCenters <> nil then
    km.fCenters := fCenters.Clone;

  Result := km;
end;

function KMeans.ClustersCount: integer;
begin
  if not fFitted then
    ArgumentError(ER_MODEL_NOT_FITTED, 'KMeans');

  Result := fCenters.RowCount;
end;

//-----------------------------
//           DBSCAN 
//-----------------------------

constructor DBSCAN.Create(
  eps: real;
  minSamples: integer
);
begin
  if eps <= 0 then
    ArgumentOutOfRangeError(ER_EPS_INVALID, eps);

  if minSamples < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLES_INVALID, minSamples);

  fEps := eps;
  fMinSamples := minSamples;

  fFitted := False;
end;

function DBSCAN.RegionQuery(X: Matrix; i: integer; neighbors: List<integer>): integer;
begin
  var n := X.RowCount;
  var p := X.ColCount;

  var eps2 := fEps * fEps;

  neighbors.Clear;

  for var j := 0 to n-1 do
  begin
    var dist := 0.0;

    for var k := 0 to p-1 do
    begin
      var d := X[i,k] - X[j,k];
      dist += d*d;
    end;

    if dist <= eps2 then
      neighbors.Add(j);
  end;

  Result := neighbors.Count;
end;

function DBSCAN.Fit(X: Matrix): IUnsupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  var n := X.RowCount;
  var p := X.ColCount;

  if n = 0 then
    ArgumentError(ER_EMPTY_DATA, 'DBSCAN');

  fFeatureCount := p;

  var labels := new integer[n];

  for var i := 0 to n - 1 do
    labels[i] := -1; // noise

  var visited := new boolean[n];

  var neighbors := new List<integer>;
  var queue := new Queue<integer>;

  var clusterId := 0;

  for var i := 0 to n - 1 do
  begin
    if visited[i] then
      continue;

    visited[i] := True;

    var count := RegionQuery(X, i, neighbors);

    if count < fMinSamples then
      continue;

    labels[i] := clusterId;

    queue.Clear;

    foreach var j in neighbors do
      queue.Enqueue(j);

    while queue.Count > 0 do
    begin
      var q := queue.Dequeue;

      if not visited[q] then
      begin
        visited[q] := True;

        var count2 := RegionQuery(X, q, neighbors);

        if count2 >= fMinSamples then
          foreach var t in neighbors do
            queue.Enqueue(t);
      end;

      if labels[q] = -1 then
        labels[q] := clusterId;
    end;

    clusterId += 1;
  end;

  fClusterCount := clusterId;
  fLabels := labels;
  fFitted := True;

  Result := Self;
end;

function DBSCAN.PredictLabels(X: Matrix): array of integer;
begin
  if X = nil then
    ArgumentNullError(ER_ARG_NULL, 'X');

  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X.RowCount <> Length(fLabels) then
    ArgumentError(ER_DBSCAN_PREDICT_NEW_DATA);

  Result := Copy(fLabels);
end;

function DBSCAN.FitPredict(X: Matrix): array of integer;
begin
  Fit(X);
  Result := PredictLabels(X);
end;

function DBSCAN.Clone: IModel;
begin
  var m := new DBSCAN(fEps, fMinSamples);

  m.fFitted := fFitted;
  m.fFeatureCount := fFeatureCount;
  m.fClusterCount := fClusterCount;

  if fLabels <> nil then
  begin
    var n := Length(fLabels);
    SetLength(m.fLabels, n);

    for var i := 0 to n - 1 do
      m.fLabels[i] := fLabels[i];
  end;

  Result := m;
end;

function DBSCAN.ClustersCount: integer := fClusterCount;

//-----------------------------
//          Pipeline 
//-----------------------------

constructor Pipeline.Create;
begin
  fTransformers := new List<ITransformer>;
  fModel := nil;
  fFitted := false;
end;

constructor Pipeline.Create(model: ISupervisedModel);
begin
  Create;
  if model = nil then
    ArgumentError(ER_MODEL_NULL);
  fModel := model;
end;

class function Pipeline.Build(params steps: array of IPipelineStep): Pipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  // последний шаг
  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is ISupervisedModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_SUPERVISED_MODEL);

  var pipe := new Pipeline(last as ISupervisedModel);

  // все шаги кроме последнего — трансформеры
  for var i := 0 to High(steps) - 1 do
  begin
    var step := steps[i];

    if step = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL, i);

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

function Pipeline.SetModel(m: ISupervisedModel): Pipeline;
begin
  if m = nil then
    ArgumentError(ER_MODEL_NULL);

  fModel := m;
  Result := Self;
end;

function Pipeline.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  var Xt := X;

  for var i := 0 to fTransformers.Count - 1 do
  begin
    var t := fTransformers[i];
  
    if t = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL);
  
    if t is ISupervisedTransformer(var sup) then
      fTransformers[i] := sup.Fit(Xt, y)
    else if t is IUnsupervisedTransformer(var unsup) then
      fTransformers[i] := unsup.Fit(Xt)
    else
      ArgumentError(ER_PIPELINE_TRANSFORMER_NO_FIT, i, t.GetType.Name);
  
    Xt := fTransformers[i].Transform(Xt);
  
    if Xt = nil then
      ArgumentError(ER_PIPELINE_TRANSFORM_RETURNED_NULL);
  end;

  if fModel is ISupervisedModel(var supModel) then
    fModel := supModel.Fit(Xt, y)
  else
    ArgumentError(ER_Model_NoFit, fModel.GetType.Name);

  fFitted := true;
  Result := Self;
end;

function Pipeline.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  var Xt := X;

  foreach var t in fTransformers do
  begin
    if t = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL);

    Xt := t.Transform(Xt);

    if Xt = nil then
      ArgumentError(ER_PIPELINE_TRANSFORM_RETURNED_NULL);
  end;

  Result := Xt;
end;

function Pipeline.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var Xt := Transform(X);
  Result := fModel.Predict(Xt);
end;

function Pipeline.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if not (fModel is IProbabilisticClassifier) then
    ArgumentError(ER_PROBA_NOT_SUPPORTED);

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

  var m := fModel.Clone;

  if not (m is ISupervisedModel) then
    Error(ER_INTERNAL_INVALID_MODEL_CLONE);
  
  p.SetModel(m as ISupervisedModel);
  
  p.fFitted := fFitted;

  Result := p;
end;

//-----------------------------
//          UPipeline 
//-----------------------------
constructor UPipeline.Create;
begin
  fTransformers := new List<ITransformer>;
  fModel := nil;
  fFitted := false;
end;

constructor UPipeline.Create(model: IModel);
begin
  Create;
  if model = nil then
    ArgumentError(ER_MODEL_NULL);
  fModel := model;
end;

class function UPipeline.Build(params steps: array of IPipelineStep): UPipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is IModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_MODEL);

  var pipe := new UPipeline(last as IModel);

  for var i := 0 to High(steps) - 1 do
  begin
    var step := steps[i];

    if step = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL, i);

    if not (step is ITransformer) then
      ArgumentError(ER_PIPELINE_INVALID_STEP_ORDER);

    pipe.Add(step as ITransformer);
  end;

  Result := pipe;
end;

function UPipeline.Add(t: ITransformer): UPipeline;
begin
  if t = nil then
    ArgumentError(ER_TRANSFORMER_NULL);

  fTransformers.Add(t);
  Result := Self;
end;

function UPipeline.SetModel(m: IModel): UPipeline;
begin
  if m = nil then
    ArgumentError(ER_MODEL_NULL);

  fModel := m;
  Result := Self;
end;

function UPipeline.Fit(X: Matrix): IUnsupervisedModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  var Xt := X;

  for var i := 0 to fTransformers.Count - 1 do
  begin
    var t := fTransformers[i];

    if t = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL);

    if t is IUnsupervisedTransformer(var unsup) then
      fTransformers[i] := unsup.Fit(Xt)
    else
      ArgumentError(ER_PIPELINE_TRANSFORMER_NO_FIT, i, t.GetType.Name);

    Xt := fTransformers[i].Transform(Xt);

    if Xt = nil then
      ArgumentError(ER_PIPELINE_TRANSFORM_RETURNED_NULL);
  end;

  if fModel is IUnsupervisedModel(var m) then
    fModel := m.Fit(Xt)
  else
    ArgumentError(ER_MODEL_NOT_UNSUPERVISED, fModel.GetType.Name);

  fFitted := true;
  Result := Self;
end;

function UPipeline.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  var Xt := X;

  foreach var t in fTransformers do
  begin
    if t = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL);

    Xt := t.Transform(Xt);

    if Xt = nil then
      ArgumentError(ER_PIPELINE_TRANSFORM_RETURNED_NULL);
  end;

  Result := Xt;
end;

function UPipeline.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var Xt := Transform(X);

  if not (fModel is IPredictiveModel) then
    Error(ER_PREDICT_NOT_SUPPORTED);
  
  Result := (fModel as IPredictiveModel).Predict(Xt);
end;

function UPipeline.ToString: string;
begin
  var sb := 'UPipeline (' +
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

function UPipeline.Clone: IModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var p := new UPipeline;

  foreach var t in fTransformers do
    p.Add(t.Clone);

  p.SetModel(fModel.Clone);

  p.fFitted := fFitted;

  Result := p;
end;

//-----------------------------
//        StandardScaler
//-----------------------------

function StandardScaler.Fit(X: Matrix): IUnsupervisedTransformer;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  fFeatureCount := X.ColCount;

  fMean := X.ColumnMeans;
  fStd := X.ColumnStd;

  // защита от нулевой дисперсии
  for var j := 0 to fStd.Length - 1 do
    if Abs(fStd[j]) < 1e-12 then
      fStd[j] := 1.0;

  fFitted := true;
  Result := Self;
end;

function StandardScaler.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
      Result[i,j] := (X[i,j] - fMean[j]) / fStd[j];
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
  s.fFeatureCount := fFeatureCount;
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

function MinMaxScaler.Fit(X: Matrix): IUnsupervisedTransformer;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  fFeatureCount := X.ColCount;

  fMin := X.ColumnMins;
  fMax := X.ColumnMaxs;

  // защита от константных признаков
  for var j := 0 to fMin.Length - 1 do
    if Abs(fMax[j] - fMin[j]) < 1e-12 then
      fMax[j] := fMin[j] + 1.0;

  fFitted := true;
  Result := Self;
end;

function MinMaxScaler.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);

  var scale := fRangeMax - fRangeMin;

  for var j := 0 to p - 1 do
  begin
    var minVal := fMin[j];
    var maxVal := fMax[j];
    var denom := maxVal - minVal;

    if Abs(denom) < 1e-12 then
    begin
      // Константный столбец → всё = fRangeMin
      for var i := 0 to n - 1 do
        Result[i,j] := fRangeMin;
    end
    else
    begin
      for var i := 0 to n - 1 do
        Result[i,j] :=
          fRangeMin + (X[i,j] - minVal) / denom * scale;
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
  s.fFeatureCount := fFeatureCount;
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

function PCATransformer.Fit(X: Matrix): IUnsupervisedTransformer;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fK <= 0 then
    ArgumentOutOfRangeError(ER_K_INVALID, fK);

  if fK > X.ColCount then
    ArgumentOutOfRangeError(ER_K_EXCEEDS_FEATURES, fK);

  fFeatureCount := X.ColCount;

  // --- центрирование
  fMean := X.ColumnMeans;

  var Xc := X.Clone;

  for var j := 0 to X.ColCount - 1 do
    for var i := 0 to X.RowCount - 1 do
      Xc[i,j] -= fMean[j];

  // --- PCA
  var (W, variances) := Xc.PCA(fK);

  fComponents := W;

  fFitted := true;
  Result := Self;
end;

function PCATransformer.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var n := X.RowCount;
  var p := X.ColCount;

  // Центрирование без лишнего Clone
  var Xc := new Matrix(n, p);

  for var j := 0 to p - 1 do
    for var i := 0 to n - 1 do
      Xc[i,j] := X[i,j] - fMean[j];

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
  t.fFeatureCount := fFeatureCount;
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

function VarianceThreshold.Fit(X: Matrix): IUnsupervisedTransformer;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fThreshold < 0 then
    ArgumentOutOfRangeError(ER_THRESHOLD_INVALID, fThreshold);

  fFeatureCount := X.ColCount;

  var vars := X.ColumnVariances;

  var tmp := new List<integer>;

  for var j := 0 to X.ColCount - 1 do
    if vars[j] >= fThreshold then
      tmp.Add(j);

  if tmp.Count = 0 then
    ArgumentError(ER_ALL_FEATURES_REMOVED);

  fSelected := tmp.ToArray;
  fFitted := true;

  Result := Self;
end;

function VarianceThreshold.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  if fSelected = nil then
    Error(ER_MODEL_NOT_INITIALIZED);

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
  t.fFeatureCount := fFeatureCount;
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
  var n := feature.Length;
  if n = 0 then
    exit(0.0);

  // --- построение отображения классов ---
  var classToIndex := new Dictionary<integer, integer>;
  var uniqueClasses := new List<integer>;
  var yEncoded := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var r := y[i];
    var ir := Round(r);

    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);

    if not classToIndex.ContainsKey(ir) then
    begin
      classToIndex[ir] := uniqueClasses.Count;
      uniqueClasses.Add(ir);
    end;

    yEncoded[i] := classToIndex[ir];
  end;

  var classCount := uniqueClasses.Count;
  if classCount < 2 then
    exit(0.0);

  if n <= classCount then
    exit(0.0);

  var counts := new integer[classCount];
  var means := new real[classCount];

  var globalMean := feature.Mean;

  // --- накопление ---
  for var i := 0 to n - 1 do
  begin
    var classIdx := yEncoded[i];
    counts[classIdx] += 1;
    means[classIdx] += feature[i];
  end;

  for var c := 0 to classCount - 1 do
    if counts[c] > 0 then
      means[c] /= counts[c];

  // --- SS_between ---
  var ssBetween := 0.0;
  for var c := 0 to classCount - 1 do
    if counts[c] > 0 then
    begin
      var diff := means[c] - globalMean;
      ssBetween += counts[c] * diff * diff;
    end;

  // --- SS_within ---
  var ssWithin := 0.0;
  for var i := 0 to n - 1 do
  begin
    var classIdx := yEncoded[i];
    var diff := feature[i] - means[classIdx];
    ssWithin += diff * diff;
  end;

  if Abs(ssWithin) < 1e-12 then
    exit(0.0);

  var msBetween := ssBetween / (classCount - 1);
  var msWithin := ssWithin / (n - classCount);

  if Abs(msWithin) < 1e-12 then
    exit(0.0);

  Result := msBetween / msWithin;
end;

function SelectKBest.ComputeChiSquare(feature: Vector; y: Vector): real;
begin
  var n := feature.Length;
  if n = 0 then
    exit(0.0);

  if y.Length <> n then
    DimensionError(ER_DIM_MISMATCH, y.Length, n);

  // --- проверка неотрицательности ---
  for var i := 0 to n - 1 do
    if feature[i] < 0 then
      ArgumentError(ER_CHI_SQUARE_NEGATIVE);

  // --- построение отображения классов ---
  var classToIndex := new Dictionary<integer, integer>;
  var uniqueClasses := new List<integer>;
  var yEncoded := new integer[n];

  for var i := 0 to n - 1 do
  begin
    var r := y[i];
    var ir := Round(r);

    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);

    if not classToIndex.ContainsKey(ir) then
    begin
      classToIndex[ir] := uniqueClasses.Count;
      uniqueClasses.Add(ir);
    end;

    yEncoded[i] := classToIndex[ir];
  end;

  var classCount := uniqueClasses.Count;
  if classCount < 2 then
    exit(0.0);

  var counts := new integer[classCount];
  var observedSums := new real[classCount];
  var totalSum := 0.0;

  for var i := 0 to n - 1 do
  begin
    var classIdx := yEncoded[i];
    counts[classIdx] += 1;
    observedSums[classIdx] += feature[i];
    totalSum += feature[i];
  end;

  if Abs(totalSum) < 1e-12 then
    exit(0.0);

  var globalMean := totalSum / n;

  var chiSquare := 0.0;

  for var c := 0 to classCount - 1 do
  begin
    if counts[c] = 0 then
      continue;

    var expected := counts[c] * globalMean;

    if Abs(expected) > 1e-12 then
    begin
      var diff := observedSums[c] - expected;
      chiSquare += diff * diff / expected;
    end;
  end;

  Result := chiSquare;
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

function SelectKBest.Fit(X: Matrix; y: Vector): ISupervisedTransformer;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fK <= 0 then
    ArgumentOutOfRangeError(ER_SELECTKBEST_K_INVALID, fK);

  fFeatureCount := X.ColCount;

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
        ComputeScore(col, y);

    if double.IsNaN(s) or double.IsInfinity(s) then
      s := real.NegativeInfinity;

    scores[j] := (s, j);
  end;

  scores := scores
              .OrderByDescending(t -> t.Item1)
              .ToArray;

  var k := Min(fK, p);
  SetLength(fSelected, k);

  for var i := 0 to k - 1 do
    fSelected[i] := scores[i].Item2;

  if k = 0 then
    ArgumentError(ER_ALL_FEATURES_REMOVED);

  fFitted := true;
  Result := Self;
end;

function SelectKBest.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  if fSelected = nil then
    Error(ER_MODEL_NOT_INITIALIZED);

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

  if fSelected <> nil then
    t.fSelected := Copy(fSelected);

  t.fFeatureCount := fFeatureCount;   // 🔥 ОБЯЗАТЕЛЬНО
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

function Normalizer.Fit(X: Matrix): IUnsupervisedTransformer;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  fFeatureCount := X.ColCount;

  fFitted := true;
  Result := Self;
end;

function Normalizer.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

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

    if Abs(norm) < 1e-12 then
    begin
      // нулевая строка — оставляем нулями
      continue;
    end;

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
  t.fFeatureCount := fFeatureCount;
  Result := t;
end;

function LabelsToInts(y: Vector): array of integer;
begin
  Result := ArrGen(y.Length, i -> Round(y[i]));
end;

    
end.