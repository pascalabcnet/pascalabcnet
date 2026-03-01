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
/// После вызова Fit значение становится True.
/// Используется для проверки корректности вызова Predict.
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
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
    /// После вызова Fit значение становится True.
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
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
    function Fit(X: Matrix; y: Vector): IModel;
  
    /// Возвращает матрицу вероятностей (m x k).
    function PredictProba(X: Matrix): Matrix;
  
    /// Возвращает массив меток классов в порядке столбцов PredictProba.
    function GetClasses: array of real;

    /// Возвращает вектор предсказанных классов.
    function Predict(X: Matrix): Vector;
  
/// Показывает, была ли модель обучена.
/// Если false — вызов Predict или PredictProba приведет к ошибке.
    property IsFitted: boolean read fFitted;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
    
/// Создает глубокую копию модели.
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
    function Fit(X: Matrix; y: Vector): IModel; virtual; abstract;

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
    function IsFitted: boolean;
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

    function PredictOne(X: Matrix; rowIndex: integer): integer;
    function MajorityClass(y: Vector; indices: array of integer): integer;
    
  protected  
    function LeafValue(y: Vector; indices: array of integer): real; override;
    
    function FindBestSplit(X: Matrix; y: Vector; indices: array of integer): SplitResult; override;

  public
/// Создает классификационное дерево:
///   • maxDepth — максимальная глубина дерева.
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1; seed: integer := -1);

/// Обучает классификационное дерево.
/// X — матрица признаков.
/// y — вектор целевых меток (целые значения).
/// Строит структуру дерева путем минимизации нечистоты в узлах.
    function Fit(X: Matrix; y: Vector): IModel; override;
    
/// Выполняет предсказание меток классов для X.
/// Для каждого объекта возвращается класс, соответствующий листу дерева.
    function Predict(X: Matrix): Vector; override;
    
/// Создает глубокую копию дерева классификации.
/// Копируется структура узлов, параметры и обученное состояние.
    function Clone: IModel; override;

/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function ClassCount: integer := fClassCount;
    
    function IndexToClass: array of integer := Copy(fIndexToClass);
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
///   • maxDepth — максимальная глубина.
///   • minSamplesSplit — минимальное число объектов для разбиения.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • leafL2 — коэффициент L2-регуляризации значения листа
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1;
      leafL2: real := 0.0; seed: integer := -1);
    
/// Обучает регрессионное дерево.
/// X — матрица признаков.
/// y — вещественная целевая переменная.
    function Fit(X: Matrix; y: Vector): IModel; override;

/// Выполняет предсказание для всех объектов X.
/// Возвращает вектор вещественных значений.
    function Predict(X: Matrix): Vector; override;

/// Создает глубокую копию дерева регрессии.
/// Копируется структура узлов, параметры и обученное состояние.
    function Clone: IModel; override;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
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
      seed: integer := -1);

/// Обучает ансамбль деревьев на данных X и y.
/// Для каждого дерева используется bootstrap-подвыборка
/// и случайное подмножество признаков.  
    function Fit(X: Matrix; y: Vector): IModel; virtual; abstract;

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
  end;  
  
/// Случайный лес для задачи регрессии.
/// Строит ансамбль регрессионных деревьев,
/// обученных на bootstrap-подвыборках данных и случайных подмножествах признаков.
/// Итоговое предсказание — среднее значение по всем деревьям ансамбля
  RandomForestRegressor = class(RandomForestBase, IModel)
  private
    fTrees: array of DecisionTreeRegressor;
  public
/// Создает регрессионный случайный лес:
///   • nTrees — число деревьев в ансамбле.
///   • maxDepth — максимальная глубина деревьев.
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • maxFeaturesMode — режим выбора числа признаков, рассматриваемых при поиске разбиения
    constructor Create(nTrees: integer := 100; 
      maxDepth: integer := integer.MaxValue;
      minSamplesSplit: integer := 2; 
      minSamplesLeaf: integer := 1;
      maxFeaturesMode: TMaxFeaturesMode := TMaxFeaturesMode.HalfFeatures;
      seed: integer := -1);

/// Обучает случайный лес на данных X и y.
/// Для каждого дерева используется bootstrap-выборка.
/// Возвращает обученную модель.
    function Fit(X: Matrix; y: Vector): IModel; override;

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
  RandomForestClassifier = class(RandomForestBase, IClassifier, IProbabilisticClassifier)
  private
    fTrees: array of DecisionTreeClassifier;
    fIndexToClass: array of integer;
    fClassCount: integer;
  public
/// Создает классификационный случайный лес:
///   • nTrees — число деревьев в ансамбле.
///   • maxDepth — максимальная глубина каждого дерева.
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе.
///   • maxFeaturesMode — режим выбора числа признаков при поиске разбиения, по умолчанию используется sqrt(p), 
///     что является стандартом для классификации
    constructor Create(nTrees: integer := 100; 
      maxDepth: integer := integer.MaxValue;
      minSamplesSplit: integer := 2;
      minSamplesLeaf: integer := 1;
      maxFeaturesMode: TMaxFeaturesMode := TMaxFeaturesMode.SqrtFeatures;
      seed: integer := -1);
  
/// Обучает случайный лес на данных X и y.
/// Для каждого дерева используется bootstrap-выборка обучающих объектов.
/// В каждом узле рассматривается случайное подмножество признаков согласно maxFeaturesMode.
/// Возвращает обученную модель.
    function Fit(X: Matrix; y: Vector): IModel; override;

/// Выполняет предсказание меток классов для матрицы X.
/// Для каждого объекта агрегируются предсказания всех деревьев.
/// Итоговый класс определяется большинством голосов или максимальной суммарной вероятностью.
    function Predict(X: Matrix): Vector; override;
    
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
      XVal: Matrix; yVal: Vector; useValidation: boolean): IModel;
      
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
      randomSeed: integer := 42;
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
    function Fit(X: Matrix; y: Vector): IModel;
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
    fBestValLoss: real;
    
    fFeatureImportances: Vector;
    
    fOOBLossHistory: List<real>;
    
    fRandomSeed: integer;
    fRng: System.Random;
    fUserProvidedSeed: boolean;

  private
    function FitInternal(XTrain: Matrix; yTrain: Vector; XVal: Matrix; yVal: Vector; useValidation: boolean): IModel;

    procedure BuildClassMapping(y: Vector);
    function EncodeLabels(y: Vector): array of integer;

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
    function Fit(X: Matrix; y: Vector): IModel;
    
/// Обучает классификатор с использованием validation-набора.
/// Если включен early stopping, он основан на validation loss.
    function FitWithValidation(XTrain: Matrix; yTrain: Vector; XVal: Matrix; yVal: Vector): IModel;

/// Предсказывает метки классов.
/// Возвращает исходные значения классов, а не внутренние индексы.
    function Predict(X: Matrix): Vector;
    
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
  KNNBase = abstract class(IModel)
  protected
    // ==== train state ====
    fXTrain: Matrix;
    fK: integer;
    fIsFitted: boolean;
    
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
    function Fit(X: Matrix; y: Vector): IModel; virtual; abstract;
    
    /// Выполняет предсказание для матрицы признаков X.
    /// Возвращает вектор предсказанных значений или меток
    function Predict(X: Matrix): Vector; virtual; abstract;
    
    /// Создаёт глубокую копию модели
    function Clone: IModel; virtual; abstract;
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
    function Fit(X: Matrix; y: Vector): IModel; override;
    
    /// Выполняет предсказание меток классов для объектов X.
    /// Возвращает вектор предсказанных меток
    function Predict(X: Matrix): Vector; override;
    
    /// Возвращает матрицу вероятностей размера (nSamples × nClasses).
    /// Столбцы соответствуют классам в порядке, возвращаемом GetClasses()
    function PredictProba(X: Matrix): Matrix;
    
    /// Возвращает массив меток классов в порядке столбцов PredictProba
    function GetClasses: array of real;
    
    /// Создаёт глубокую копию классификатора
    function Clone: IModel; override;
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
    function Fit(X: Matrix; y: Vector): IModel; override;
    
    /// Выполняет предсказание числовых значений для объектов X.
    /// Возвращает вектор предсказанных значений
    function Predict(X: Matrix): Vector; override;
    
    /// Создаёт глубокую копию регрессора
    function Clone: IModel; override;
  end;
  
{$endregion Models}

{$region Pipeline}
/// Последовательный конвейер машинного обучения.
/// Гарантирует строгий порядок выполнения шагов:
///   [преобразователи] → [модель].
/// 
/// Поддерживает:
///   • преобразователи без учёта целевой переменной;
///   • преобразователи с учётом целевой переменной;
///   • одну финальную модель.
///
/// Обеспечивает единый интерфейс Fit / Predict
/// и воспроизводимость полного процесса обучения
  Pipeline = class(IModel, IProbabilisticClassifier)
  private
    fTransformers: List<ITransformer>;
    fModel: IModel;
    fFitted: boolean;
  public
    /// Создаёт конвейер машинного обучения для заданной модели:
    ///   • model — модель, которая будет обучена
    ///     после последовательного применения всех преобразователей.
    constructor Create(model: IModel);
    
    /// Создаёт пустой пайплайн (конвейер машинного обучения).
    /// Модель должна быть установлена через SetModel.
    constructor Create;

    /// Строит конвейер машинного обучения из последовательности шагов.
    /// Шаги указываются в порядке выполнения:
    ///   сначала преобразователи, затем модель.
    /// Последний шаг обязан быть моделью (IModel).
    /// Возвращает сконструированный конвейер.
    static function Build(params steps: array of IPipelineStep): Pipeline;
    
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
    
    function GetClasses: array of real;
  
    /// Показывает, был ли пайплайн обучен (вызван метод Fit).
    property IsFitted: boolean read fFitted;
    
    function ToString: string; override;
    
    function Clone: IModel;
  end;
  
{$endregion Pipeline}
  
{$region Transformers}
/// Стандартизирует признаки: вычитает среднее
///   и делит на стандартное отклонение по каждому столбцу.
/// Используется для приведения признаков к сопоставимому масштабу
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
///   (по умолчанию [0, 1]) на основе минимального и максимального значения каждого столбца.
/// Используется для приведения признаков к единому масштабу без центрирования
  MinMaxScaler = class(ITransformer)
  private
    fMin: Vector;
    fMax: Vector;
    fFitted: boolean;
    fRangeMin: real;
    fRangeMax: real;
  public
    /// Создаёт MinMaxScaler с диапазоном [rangeMin, rangeMax].
    /// По умолчанию масштабирование выполняется к [0, 1]
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
    /// Создаёт PCA-трансформер:
    ///   • k — число главных компонент (k > 0).
    constructor Create(k: integer);
  
    /// Обучает трансформер на матрице признаков X.
    ///   X — матрица m × n.
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
/// Не использует целевую переменную (unsupervised)
  VarianceThreshold = class(ITransformer)
  private
    fThreshold: real;
    fSelected: array of integer;
    fFitted: boolean;
  
  public
    /// threshold — минимальная допустимая дисперсия (>= 0)
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
    fFitted: boolean;
    
    function ComputeScore(feature: Vector; y: Vector): real;
    function ComputeCorrelation(x: Vector; y: Vector): real;
    function ComputeFRegression(feature: Vector; y: Vector): real;
    // Multiclass version
    function ComputeAnovaF(feature: Vector; y: Vector): real;
    // Multiclass version
    function ComputeChiSquare(feature: Vector; y: Vector): real;
  public
    /// Создаёт трансформер с использованием встроенного критерия:
    ///   • k — число отбираемых признаков.
    ///   • score — тип критерия (например, Correlation)
    constructor Create(k: integer; score: FeatureScore := FeatureScore.Correlation);
  
    /// Создаёт трансформер с пользовательской функцией оценки:
    ///   • scoreFunc — функция (feature, y) → real
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
  Normalizer = class(ITransformer)
  private
    fNormType: NormType;
    fFitted: boolean;
  public
/// Создает нормализатор:
///   • norm — тип нормы, используемой для масштабирования строки признаков.
/// По умолчанию используется L2-норма.
    constructor Create(norm: NormType := NormType.L2);
  
/// Подготавливает трансформер к работе.
/// Для Normalizer этап обучения может быть формальным, так как параметры не накапливаются.
    function Fit(X: Matrix): ITransformer;

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

  
var 
  /// Проверять ли входные данные моделей на NaN, Inf 
  ValidateFiniteInputs := true;  
  
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

function LinearRegression.Fit(X: Matrix; y: Vector): IModel;
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

function RidgeRegression.Fit(X: Matrix; y: Vector): IModel;
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

      var newBeta := SoftThreshold(rho, fLambda1) / (zj + fLambda2);
      var delta := newBeta - oldBeta;

      if Abs(delta) > 0 then
        for var i := 0 to n - 1 do
          residual[i] -= Xc[i,j] * delta;

      if Abs(delta) > maxChange then
        maxChange := Abs(delta);

      fCoef[j] := newBeta;
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

      var currentLoss := rss + fLambda1 * l1 + fLambda2 * l2;

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

function LogisticRegression.Fit(X: Matrix; y: Vector): IModel;
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
    yEncoded[i] := fClassToIndex[Round(y[i])];

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
        ArgumentError(ER_LOGISTIC_SOFTMAX_ZERO);

      for var k := 0 to fClassCount - 1 do
        Z[i,k] /= sumExp;

      var yi := yEncoded[i];
      var prob := Z[i, yi];

      if prob <= 0 then
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

function GiniCriterion.Impurity(y: Vector; indices: array of integer): real;
begin
  var n := indices.Length;
  if n = 0 then exit(0.0);

  var counts := new integer[fClassCount];

  foreach var i in indices do
    counts[Round(y[i])] += 1;

  var sumsq := 0.0;

  for var c := 0 to fClassCount - 1 do
    if counts[c] > 0 then
    begin
      var p := counts[c] / n;
      sumsq += p * p;
    end;

  Result := 1.0 - sumsq;
end;

function VarianceCriterion.Impurity(y: Vector; indices: array of integer): real;
begin
  var n := indices.Length;
  if n = 0 then exit(0.0);

  var sum := 0.0;
  var sumsq := 0.0;

  foreach var i in indices do
  begin
    var v := y[i];
    sum += v;
    sumsq += v * v;
  end;

  var mean := sum / n;
  Result := (sumsq / n) - mean * mean;
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

constructor DecisionTreeBase.Create(maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer; seed: integer);
begin
  if maxDepth > MAX_ALLOWED_TREE_DEPTH then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, maxDepth);
  
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
  dest.fRng := new System.Random(fRandomSeed);

  if fCriterion <> nil then
    dest.fCriterion := fCriterion; // можно так, если критерий stateless

  if fFeatureImportances <> nil then
    dest.fFeatureImportances := fFeatureImportances.Clone;

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
  if (fMaxDepth > 0) and (depth >= fMaxDepth) then
    exit(LeafNode(LeafValue(y, indices)));

  // Минимальное число объектов
  if indices.Length < fMinSamplesSplit then
    exit(LeafNode(LeafValue(y, indices)));

  // Если узел уже чистый
  if IsPure(y, indices) then
    exit(LeafNode(LeafValue(y, indices)));
  
  var parentImp := fCriterion.Impurity(y, indices);

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
  
  var leftImp := fCriterion.Impurity(y, leftArr);
  var rightImp := fCriterion.Impurity(y, rightArr);
  
  var n := indices.Length;
  
  var delta :=
    parentImp
    - (leftArr.Length / n) * leftImp
    - (rightArr.Length / n) * rightImp;
  
  if delta > 0 then
    fFeatureImportances[split.Feature] += delta;     

  // Рекурсия
  var leftNode := BuildTree(X, y, leftArr, depth + 1);
  var rightNode := BuildTree(X, y, rightArr, depth + 1);

  // Создание split-узла
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

  var p := X.ColCount;

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
  var bestScore := real.MaxValue;

  // --- формируем список признаков и (при необходимости) выбираем подмножество без повторов
  var feat := new integer[p];
  for var i := 0 to p - 1 do
    feat[i] := i;

  var m := fMaxFeatures;
  if (m <= 0) or (m > p) then
    m := p;

  // partial Fisher–Yates: рандомизируем первые m позиций
  for var i := 0 to m - 1 do
  begin
    var j := fRng.Next(i, p);
    var tmp := feat[i];
    feat[i] := feat[j];
    feat[j] := tmp;
  end;

  // --- перебираем только feat[0..m-1]
  for var fi := 0 to m - 1 do
  begin
    var feature := feat[fi];

    // пары (value, label) по текущему признаку
    var values := new real[n];
    var labels := new integer[n];

    for var i := 0 to n - 1 do
    begin
      var row := indices[i];
      values[i] := X[row, feature];
      
      labels[i] := Round(y[row]);
    end;

    // сортируем по values, а labels переставляются синхронно
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

      // gini(left)
      var giniLeft := 1.0;
      for var c := 0 to fClassCount - 1 do
      begin
        if leftCounts[c] > 0 then
        begin
          var q := leftCounts[c] / leftSize;
          giniLeft -= q * q;
        end;
      end;

      // gini(right)
      var giniRight := 1.0;
      for var c := 0 to fClassCount - 1 do
      begin
        if rightCounts[c] > 0 then
        begin
          var q := rightCounts[c] / rightSize;
          giniRight -= q * q;
        end;
      end;

      var weighted :=
        (leftSize / n) * giniLeft +
        (rightSize / n) * giniRight;

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

function DecisionTreeClassifier.Fit(X: Matrix; y: Vector): IModel;
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

  if fMinSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MINSAMPLESSPLIT_INVALID, fMinSamplesSplit);

  if fMinSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLESLEAF_INVALID, fMinSamplesLeaf);

  if (fMaxDepth < 0) then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, fMaxDepth);

  if fMaxDepth > 10000 then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, fMaxDepth);
  
  
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

function DecisionTreeClassifier.ToString: string;
begin
  Result :=
    $'DecisionTreeClassifier(' +
    $'maxDepth={fMaxDepth}, ' +
    $'minSamplesSplit={fMinSamplesSplit}, ' +
    $'minSamplesLeaf={fMinSamplesLeaf}' +
    ')';
end;

// DecisionTreeRegressor

constructor DecisionTreeRegressor.Create(maxDepth: integer; minSamplesSplit: integer; 
  minSamplesLeaf: integer; leafL2: real; seed: integer);
begin
  inherited Create(maxDepth, minSamplesSplit, minSamplesLeaf, seed);
  fCriterion := new VarianceCriterion;
  fLeafL2 := leafL2;
end;

function DecisionTreeRegressor.LeafValue(y: Vector; indices: array of integer): real;
begin
  var sum := 0.0;
  var n := indices.Length;

  foreach var i in indices do
    sum += y[i];

  if fLeafL2 > 0 then
    Result := sum / (n + fLeafL2)
  else
    Result := sum / n;
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

  if fMinSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MINSAMPLESSPLIT_INVALID, fMinSamplesSplit);

  if fMinSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLESLEAF_INVALID, fMinSamplesLeaf);

  if fLeafL2 < 0 then
    ArgumentOutOfRangeError(ER_L2_NEGATIVE, fLeafL2);

  if fMaxDepth < 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, fMaxDepth);

  if fMaxDepth > 10000 then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, fMaxDepth);
  
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
    fLeafL2
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
  seed: integer);
begin
  fNTrees := nTrees;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fMaxFeaturesMode := maxFeatures;
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
  maxFeaturesMode: TMaxFeaturesMode; seed: integer);
begin
  inherited Create(nTrees,maxDepth,minSamplesSplit,minSamplesLeaf,maxFeaturesMode,seed)
end;

function RandomForestRegressor.Fit(X: Matrix; y: Vector): IModel;
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

  if fNTrees <= 0 then
    ArgumentOutOfRangeError(ER_NTREES_INVALID, fNTrees);

  if fMinSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MINSAMPLESSPLIT_INVALID, fMinSamplesSplit);

  if fMinSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLESLEAF_INVALID, fMinSamplesLeaf);

  if fMaxDepth < 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, fMaxDepth);

  if fMaxDepth > 10000 then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, fMaxDepth);

  SetLength(fTrees, fNTrees);

  for var t := 0 to fNTrees - 1 do
  begin
    var treeSeed := fRng.Next(integer.MaxValue);
  
    var tree := new DecisionTreeRegressor(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      treeSeed
    );
  
    var p := X.ColCount;
    var m := ComputeMaxFeatures(p);
    tree.fMaxFeatures := m;
  
    var rows: array of integer;
    BootstrapRowIndices(X.RowCount, rows);
  
    tree.SetRowIndices(rows);
    tree.Fit(X, y);
  
    fTrees[t] := tree;
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
    fRandomSeed
  );

  rf.fUserProvidedSeed := fUserProvidedSeed;
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

function RandomForestRegressor.ToString: string;
begin
  var depthStr :=
    if fMaxDepth = integer.MaxValue then '∞'
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
  maxFeaturesMode: TMaxFeaturesMode; seed: integer);
begin
  inherited Create(nTrees,maxDepth,minSamplesSplit,minSamplesLeaf,maxFeaturesMode,seed);
end;

function RandomForestClassifier.Fit(X: Matrix; y: Vector): IModel;
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

  if fNTrees <= 0 then
    ArgumentOutOfRangeError(ER_NTREES_INVALID, fNTrees);

  if fMinSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MINSAMPLESSPLIT_INVALID, fMinSamplesSplit);

  if fMinSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLESLEAF_INVALID, fMinSamplesLeaf);

  if fMaxDepth < 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, fMaxDepth);

  if fMaxDepth > MAX_ALLOWED_TREE_DEPTH then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, fMaxDepth);
  
  SetLength(fTrees, fNTrees);

  for var t := 0 to fNTrees - 1 do
  begin
    var treeSeed := fRng.Next(integer.MaxValue);
  
    var tree := new DecisionTreeClassifier(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      treeSeed
    );
  
    var p := X.ColCount;
    var m := ComputeMaxFeatures(p);
    tree.fMaxFeatures := m;
  
    var rows: array of integer;
    BootstrapRowIndices(X.RowCount, rows);
  
    tree.SetRowIndices(rows);
    tree.Fit(X, y);
    
    if t = 0 then // скопировать на первой итерации
    begin
      fClassCount := tree.ClassCount;
      fIndexToClass := tree.IndexToClass;
    end;
  
    fTrees[t] := tree;
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

  var n := X.RowCount;
  var treeCount := fTrees.Length;

  if treeCount = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);
  
  var resultVec := new Vector(n);
  var counts := new integer[fClassCount];

  for var i := 0 to n - 1 do
  begin
    // обнулить счётчики
    for var c := 0 to fClassCount - 1 do
      counts[c] := 0;

    // голосование
    for var t := 0 to treeCount - 1 do
    begin
      var cls := fTrees[t].PredictOne(X, i); // внутренний индекс 0..K-1
      counts[cls] += 1;
    end;

    // argmax
    var bestClass := 0;
    var bestCount := -1;

    for var c := 0 to fClassCount - 1 do
      if counts[c] > bestCount then
      begin
        bestCount := counts[c];
        bestClass := c;
      end;

    // вернуть исходную метку
    resultVec[i] := fIndexToClass[bestClass];
  end;

  Result := resultVec;
end;

function RandomForestClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if ValidateFiniteInputs then
    CheckXForPredict(X);

  var n := X.RowCount;
  var treeCount := fTrees.Length;

  if treeCount = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  var resultMat := new Matrix(n, fClassCount);
  var counts := new integer[fClassCount];

  for var i := 0 to n - 1 do
  begin
    // обнулить счётчики
    for var c := 0 to fClassCount - 1 do
      counts[c] := 0;

    // голосование
    for var t := 0 to treeCount - 1 do
    begin
      var cls := fTrees[t].PredictOne(X, i); // внутренний индекс
      counts[cls] += 1;
    end;

    // нормализация → вероятности
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
    fRandomSeed
  );

  rf.fUserProvidedSeed := fUserProvidedSeed;
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

function RandomForestClassifier.ToString: string;
begin
  var depthStr :=
    if fMaxDepth = integer.MaxValue then '∞'
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

  fNEstimators := nEstimators;
  fLearningRate := learningRate;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fSubsample := subsample;
  fRandomSeed := randomSeed;

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
    ArgumentOutOfRangeError('huberDelta must be > 0!!huberDelta must be > 0');
  
  if earlyStoppingPatience < 0 then
    ArgumentOutOfRangeError('earlyStoppingPatience must be >= 0!!earlyStoppingPatience must be >= 0');
  
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

function GradientBoostingRegressor.ComputeTrainLoss(y, yPred: Vector): real;
begin
  var n := y.Length;
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

  Result := sum / n;
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

  // --- hyperparameter checks ---
  if fNEstimators <= 0 then
    ArgumentOutOfRangeError(ER_NESTIMATORS_INVALID, fNEstimators);

  if fLearningRate <= 0 then
    ArgumentOutOfRangeError(ER_LEARNING_RATE_INVALID, fLearningRate);

  if (fSubsample <= 0.0) or (fSubsample > 1.0) then
    ArgumentOutOfRangeError(ER_SUBSAMPLE_INVALID, fSubsample);

  if fMinSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MINSAMPLESSPLIT_INVALID, fMinSamplesSplit);

  if fMinSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLESLEAF_INVALID, fMinSamplesLeaf);

  if fMaxDepth < 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, fMaxDepth);

  if fMaxDepth > MAX_ALLOWED_TREE_DEPTH then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, fMaxDepth);

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
      var k := Floor(nTrain * fSubsample);
      if k < 1 then k := 1;
      if k > nTrain then k := nTrain;

      var all := new integer[nTrain];
      for var i := 0 to nTrain - 1 do
        all[i] := i;

      // partial Fisher–Yates
      for var i := 0 to k - 1 do
      begin
        var j := i + fRng.Next(nTrain - i);
        var tmp := all[i];
        all[i] := all[j];
        all[j] := tmp;
      end;

      rows := new integer[k];
      for var i := 0 to k - 1 do
        rows[i] := all[i];

      tree.SetRowIndices(rows);

      if useOOB then
      begin
        used := new boolean[nTrain];
        for var i := 0 to k - 1 do
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
    DimensionError(ER_FEATURE_COUNT_MISMATCH);

  var totalTrees := fEstimators.Count;

  if (m < 0) or (m > totalTrees) then
    ArgumentOutOfRangeError(ER_STAGE_OUT_OF_RANGE, m, totalTrees);

  var n := X.RowCount;
  var yPred := new Vector(n);

  // F0
  for var i := 0 to n - 1 do
    yPred[i] := fInitValue;

  // добавить первые m деревьев
  for var t := 0 to m - 1 do
  begin
    var delta := fEstimators[t].Predict(X);

    for var i := 0 to n - 1 do
      yPred[i] += fLearningRate * delta[i];
  end;

  Result := yPred;
end;

function GradientBoostingRegressor.StagedPredict(X: Matrix): sequence of Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  for var m := 1 to fEstimators.Count do
    yield PredictStage(X, m);
end;

function GradientBoostingRegressor.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fFeatureImportances <> nil then
    exit(fFeatureImportances);

  var importances := new Vector(fFeatureCount);

  foreach var tree in fEstimators do
  begin
    var imp := tree.FeatureImportances;

    for var j := 0 to fFeatureCount - 1 do
      importances[j] += imp[j];
  end;

  // нормализация
  var s := importances.Sum;
  if s > 0 then
    for var j := 0 to fFeatureCount - 1 do
      importances[j] /= s;

  fFeatureImportances := importances;

  Result := fFeatureImportances;
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
    fRandomSeed
  );

  // --- seed policy ---
  copy.fUserProvidedSeed := fUserProvidedSeed;

  // --- basic state ---
  copy.fInitValue := fInitValue;
  copy.fFeatureCount := fFeatureCount;
  copy.fFitted := fFitted;

  // --- best iteration state ---
  copy.fBestIteration := fBestIteration;
  copy.fBestTrainLoss := fBestTrainLoss;
  copy.fBestScoreLoss := fBestScoreLoss;

  // --- loss configuration ---
  copy.fLoss := fLoss;
  copy.fHuberDelta := fHuberDelta;
  copy.fQuantileAlpha := fQuantileAlpha;
  copy.fLeafL2 := fLeafL2;
  copy.fEarlyStoppingPatience := fEarlyStoppingPatience;
  copy.fUseOOBEarlyStopping := fUseOOBEarlyStopping;

  // --- deep copy histories ---
  copy.fTrainLossHistory.Clear;
  copy.fTrainLossHistory.AddRange(fTrainLossHistory);

  copy.fValLossHistory.Clear;
  copy.fValLossHistory.AddRange(fValLossHistory);

  copy.fOOBLossHistory.Clear;
  copy.fOOBLossHistory.AddRange(fOOBLossHistory);

  // --- deep copy trees ---
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
  useValidation: boolean): IModel;
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

  // --- hyperparameter checks ---
  if fNEstimators <= 0 then
    ArgumentOutOfRangeError(ER_NESTIMATORS_INVALID, fNEstimators);

  if fLearningRate <= 0 then
    ArgumentOutOfRangeError(ER_LEARNING_RATE_INVALID, fLearningRate);

  if (fSubsample <= 0.0) or (fSubsample > 1.0) then
    ArgumentOutOfRangeError(ER_SUBSAMPLE_INVALID, fSubsample);

  if fMinSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MINSAMPLESSPLIT_INVALID, fMinSamplesSplit);

  if fMinSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MINSAMPLESLEAF_INVALID, fMinSamplesLeaf);

  if fMaxDepth < 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, fMaxDepth);

  if fMaxDepth > MAX_ALLOWED_TREE_DEPTH then
    ArgumentError(ER_MAX_DEPTH_TOO_LARGE, fMaxDepth);

  // --- reset state
  fOOBLossHistory.Clear;
  fEstimators.Clear;
  fTrainLossHistory.Clear;
  fValLossHistory.Clear;

  fFeatureCount := XTrain.ColCount;
  fBestIteration := -1;
  fBestValLoss := real.PositiveInfinity;
  fFitted := false;

  // --- mapping
  BuildClassMapping(yTrain);
  var yEncoded := EncodeLabels(yTrain);

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
    yValEncoded := EncodeLabels(yVal);
  
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
      if fBestValLoss - scoreLoss > MinImprovement then
      begin
        fBestValLoss := scoreLoss;
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

function GradientBoostingClassifier.EncodeLabels(y: Vector): array of integer;
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

  var maxLogit := logits[0];
  for var cls := 1 to classCount - 1 do
    if logits[cls] > maxLogit then
      maxLogit := logits[cls];

  var sumExp := 0.0;

  for var cls := 0 to classCount - 1 do
  begin
    var e := Exp(logits[cls] - maxLogit);
    probs[cls] := e;
    sumExp += e;
  end;

  for var cls := 0 to classCount - 1 do
    probs[cls] /= sumExp;
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

function GradientBoostingClassifier.ComputeLogLoss(yEncoded: array of integer; probs: Matrix): real;
begin
  var n := Length(yEncoded);
  var eps := 1e-15;

  var loss := 0.0;

  for var i := 0 to n - 1 do
  begin
    var cls := yEncoded[i];
    var p := probs[i, cls];

    if p < eps then
      p := eps;

    loss -= Ln(p);
  end;

  Result := loss / n;
end;

function GradientBoostingClassifier.ComputeLogLossMasked(
  yEncoded: array of integer;
  logits: Matrix;
  mask: array of boolean): real;
begin
  var n := logits.RowCount;
  var k := logits.ColCount;

  var rowLogits := new real[k];
  var rowProbs := new real[k];

  var sum := 0.0;
  var cnt := 0;

  for var i := 0 to n - 1 do
    if mask[i] then
    begin
      for var cls := 0 to k - 1 do
        rowLogits[cls] := logits[i, cls];

      SoftmaxRow(rowLogits, rowProbs);

      var yi := yEncoded[i];
      var p := rowProbs[yi];

      // защита от log(0)
      if p < 1e-15 then
        p := 1e-15;

      sum += -Ln(p);
      cnt += 1;
    end;

  if cnt = 0 then
    exit(real.PositiveInfinity);

  Result := sum / cnt;
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
    ArgumentError(ER_N_ESTIMATORS_INVALID);

  if learningRate <= 0 then
    ArgumentError(ER_LEARNING_RATE_INVALID);

  if maxDepth <= 0 then
    ArgumentError(ER_MAX_DEPTH_INVALID);

  if subsample <= 0 then
    ArgumentError(ER_SUBSAMPLE_INVALID);

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
  fBestValLoss := real.PositiveInfinity;
  
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

  // --- накопление логитов от всех итераций
  foreach var trees in fEstimators do
  begin
    for var cls := 0 to classCount - 1 do
    begin
      var delta := trees[cls].Predict(X);

      for var i := 0 to nSamples - 1 do
        logits[i, cls] += fLearningRate * delta[i];
    end;
  end;

  // --- softmax
  var probs := new Matrix(nSamples, classCount);

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

  // --- seed policy ---
  model.fUserProvidedSeed := fUserProvidedSeed;

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

  // --- estimators (deep copy) ---
  foreach var trees in fEstimators do
  begin
    var newTrees := new DecisionTreeRegressor[Length(trees)];

    for var cls := 0 to Length(trees) - 1 do
      newTrees[cls] := trees[cls].Clone as DecisionTreeRegressor;

    model.fEstimators.Add(newTrees);
  end;

  // --- histories ---
  foreach var v in fTrainLossHistory do
    model.fTrainLossHistory.Add(v);

  foreach var v in fValLossHistory do
    model.fValLossHistory.Add(v);

  foreach var v in fOOBLossHistory do
    model.fOOBLossHistory.Add(v);

  model.fBestIteration := fBestIteration;
  model.fBestValLoss := fBestValLoss;

  Result := model;
end;

function GradientBoostingClassifier.Fit(X: Matrix; y: Vector): IModel;
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

//-----------------------------
//          KNNBase 
//-----------------------------

constructor KNNBase.Create(k: integer; weighting: KNNWeighting);
begin
  if k < 1 then
    ArgumentOutOfRangeError(ER_K_MUST_BE_POSITIVE);

  fK := k;
  fWeighting := weighting;
  fIsFitted := False;
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

function KNNClassifier.Fit(X: Matrix; y: Vector): IModel;
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

  fIsFitted := true;

  exit(self);
end;

function KNNClassifier.GetClasses: array of real;
begin
  Result := fClasses;
end;

function KNNClassifier.Clone: IModel;
begin
  var clone := new KNNClassifier(fK, fWeighting);

  if fIsFitted then
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
    clone.fIsFitted := true;
  end;

  Result := clone;
end;

function KNNClassifier.Predict(X: Matrix): Vector;
begin
  if not fIsFitted then
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

function KNNClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fIsFitted then
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

//-----------------------------
//        KNNRegressor 
//-----------------------------

constructor KNNRegressor.Create(k: integer; weighting: KNNWeighting);
begin
  inherited Create(k, weighting);
end;

function KNNRegressor.Fit(X: Matrix; y: Vector): IModel;
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

  fIsFitted := true;
  Result := Self;
end;

function KNNRegressor.Predict(X: Matrix): Vector;
begin
  if not fIsFitted then
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

  if fIsFitted then
  begin
    clone.fXTrain := fXTrain.Clone;
    clone.fYTrain := fYTrain.Clone;

    SetLength(clone.fNeighbors, fNeighbors.Length);

    clone.fIsFitted := True;
  end;

  Result := clone;
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

class function Pipeline.Build(params steps: array of IPipelineStep): Pipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  // последний шаг
  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is IModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_MODEL);

  var pipe := new Pipeline(last as IModel);

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

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  var Xt := X;

  foreach var t in fTransformers do
  begin
    if t = nil then
      ArgumentError(ER_PIPELINE_STEP_NULL);

    if t is ISupervisedTransformer (var sup) then
      sup.Fit(Xt, y)
    else
      t.Fit(Xt);

    Xt := t.Transform(Xt);

    if Xt = nil then
      ArgumentError(ER_PIPELINE_TRANSFORM_RETURNED_NULL);
  end;

  fModel.Fit(Xt, y);

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

function Pipeline.GetClasses: array of real;
begin
  if not (fModel is IProbabilisticClassifier) then
    ArgumentError(ER_PROBA_NOT_SUPPORTED);

  Result := (fModel as IProbabilisticClassifier).GetClasses;
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
  
  p.fFitted := fFitted;

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
  var n := feature.Length;
  if n = 0 then
    exit(0.0);

  // --- построение отображения классов ---
  var classToIndex := new Dictionary<integer, integer>;
  var uniqueClasses := new List<integer>;

  for var i := 0 to y.Length - 1 do
  begin
    var cls := Round(y[i]);
    if not classToIndex.ContainsKey(cls) then
    begin
      classToIndex[cls] := uniqueClasses.Count;
      uniqueClasses.Add(cls);
    end;
  end;

  var classCount := uniqueClasses.Count;
  if classCount < 2 then
    exit(0.0);

  var counts := new integer[classCount];
  var means := new real[classCount];

  // --- глобальное среднее ---
  var globalMean := feature.Mean;

  // --- накопление по классам ---
  for var i := 0 to n - 1 do
  begin
    var classIdx := classToIndex[Round(y[i])];
    counts[classIdx] += 1;
    means[classIdx] += feature[i];
  end;

  for var classIdx := 0 to classCount - 1 do
    if counts[classIdx] > 0 then
      means[classIdx] /= counts[classIdx];

  // --- SS_between ---
  var ssBetween := 0.0;
  for var classIdx := 0 to classCount - 1 do
    if counts[classIdx] > 0 then
    begin
      var diff := means[classIdx] - globalMean;
      ssBetween += counts[classIdx] * diff * diff;
    end;

  // --- SS_within ---
  var ssWithin := 0.0;
  for var i := 0 to n - 1 do
  begin
    var classIdx := classToIndex[Round(y[i])];
    var diff := feature[i] - means[classIdx];
    ssWithin += diff * diff;
  end;

  if ssWithin = 0 then
    exit(0.0);

  var msBetween := ssBetween / (classCount - 1);
  var msWithin := ssWithin / (n - classCount);

  if msWithin = 0 then
    exit(0.0);

  Result := msBetween / msWithin;
end;

function SelectKBest.ComputeChiSquare(feature: Vector; y: Vector): real;
begin
  var n := feature.Length;
  if n = 0 then
    exit(0.0);

  // --- проверка неотрицательности ---
  for var i := 0 to n - 1 do
    if feature[i] < 0 then
      ArgumentError(ER_CHI_SQUARE_NEGATIVE);

  // --- построение отображения классов ---
  var classToIndex := new Dictionary<integer, integer>;
  var uniqueClasses := new List<integer>;

  for var i := 0 to y.Length - 1 do
  begin
    var cls := Round(y[i]);
    if not classToIndex.ContainsKey(cls) then
    begin
      classToIndex[cls] := uniqueClasses.Count;
      uniqueClasses.Add(cls);
    end;
  end;

  var classCount := uniqueClasses.Count;
  if classCount < 2 then
    exit(0.0);

  var counts := new integer[classCount];
  var observedSums := new real[classCount];

  var totalSum := 0.0;

  for var i := 0 to n - 1 do
  begin
    var classIdx := classToIndex[Round(y[i])];
    counts[classIdx] += 1;
    observedSums[classIdx] += feature[i];
    totalSum += feature[i];
  end;

  if totalSum = 0 then
    exit(0.0);

  var globalMean := totalSum / n;

  var chiSquare := 0.0;

  for var classIdx := 0 to classCount - 1 do
  begin
    if counts[classIdx] = 0 then
      continue;

    var expected := counts[classIdx] * globalMean;

    if expected > 0 then
    begin
      var diff := observedSums[classIdx] - expected;
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

function SelectKBest.Fit(X: Matrix; y: Vector): ITransformer;
begin
  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);
  
  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);
  
  if fK <= 0 then
    ArgumentError(ER_SELECTKBEST_K_INVALID, fK);
     
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