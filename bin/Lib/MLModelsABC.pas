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

// =============================================================
// СТАТИСТИЧЕСКОЕ СОГЛАШЕНИЕ (MLModelsABC)
//
// Модели и трансформеры используют статистические методы
// из LinearAlgebraML.
//
// В частности:
//   • StandardScaler, VarianceThreshold и др. используют
//     дисперсию с делением на n (population variance)
//
// См. статистическую политику в модуле MLABC.
// =============================================================

{
  Производительность
  
  DecisionTreeRegressor.Fit - 340 мс против 156 мс в Питоне при той же точности
  GradientBoostingRegressor.Fit - 5500 мс против 5500 мс в Питоне
  RandomForestRegressor.Fit - 1280 мс против 480 мс в Питоне 
}

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
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
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

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
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
    
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
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
    
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
    function Clone: IModel;
    
    function Name: string := Self.GetType.Name;
  end;
  
  /// Модель Lasso-регрессии (линейная регрессия с L1-регуляризацией).
  /// Минимизирует квадратичную ошибку с L1-штрафом на веса, что приводит к разреженным решениям.
  /// Внутренне реализована через ElasticNet с нулевой L2-регуляризацией
  LassoRegression = class(IRegressor)
    private
      fModel: ElasticNet;
      fAlpha: real;
      fMaxIter: integer;
      fTol: real;
      
      function GetCoefficients: Vector;
      function GetIntercept: real;
      function GetIsFitted: boolean;
    public
      /// Создаёт модель Lasso-регрессии.
      /// alpha — коэффициент L1-регуляризации.
      /// maxIter — максимальное число итераций обучения.
      /// tol — порог сходимости.
      constructor Create(
        alpha: real := 1.0;
        maxIter: integer := 1000;
        tol: real := 1e-6
      );
  
/// Обучает модель на числовых данных.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с непрерывными значениями.
/// Выполняется центрирование признаков и целевой переменной.
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
      function Fit(X: Matrix; y: Vector): ISupervisedModel;
  
      /// Предсказывает значения целевой переменной для входных данных.
      /// X — матрица признаков размера [nSamples x nFeatures].
      /// Возвращает вектор предсказаний длины nSamples.
      function Predict(X: Matrix): Vector;
  
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
      function Clone: IModel;
      
      /// Вектор коэффициентов модели.
      /// Длина равна числу признаков.
      /// Доступен после обучения (Fit).
      property Coefficients: Vector read GetCoefficients;
    
      /// Свободный член модели (смещение, bias).
      property Intercept: real read GetIntercept;
    
      /// Коэффициент L1-регуляризации.
      property Alpha: real read fAlpha;
      
      /// Показывает, была ли модель обучена.
      /// После вызова Fit значение становится True.
      property IsFitted: boolean read GetIsFitted;
      
      function Name: string := Self.GetType.Name;
    end;
    
/// Логистическая регрессия.
/// Поддерживает бинарную и многоклассовую классификацию.
/// Для нескольких классов используется softmax,
///   для двух классов — частный случай softmax.
/// Оптимизация выполняется по кросс-энтропийной функции потерь
///   с поддержкой L2-регуляризации
  LogisticRegression = class(IProbabilisticClassifier, IClassifierInternal)
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
    
    fTol: real;
    fCheckConvergence: boolean;
    fMinImprovement: real;
    
    fClassLabels: array of string; 
    
    fUseFastExp: boolean;
    
    function GetWeights: Matrix;
    function GetIntercept: Vector;
  public
    /// lambda — коэффициент L2-регуляризации.
    /// lr — шаг градиентного спуска.
    /// epochs — число итераций обучения
    constructor Create(
      lambda: real := 0.0;
      learningRate: real := 0.01;
      epochs: integer := 1000;
      tol: real := 1e-6;
      checkConvergence: boolean := true;
      minImprovement: real := 1e-8;
      useFastExp: boolean := True
    );
  
/// Обучает модель логистической регрессии.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с метками классов (целочисленные значения).
/// После вызова модель содержит обученные параметры и готова к Predict.
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
    
    /// Возвращает матрицу вероятностей классов для всех объектов из X.
    /// Размер результата: nSamples × nClasses, где:
    /// - nSamples — число объектов в X;
    /// - nClasses — число классов модели.
    /// Элемент [i, k] содержит вероятность того, что объект i принадлежит классу k.
    /// Сумма вероятностей в каждой строке равна 1.
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
    
    /// Матрица коэффициентов модели (p × k).
    property Weights: Matrix read GetWeights;
    
    /// Вектор свободных членов (bias) для каждого класса.
    property Intercept: Vector read GetIntercept;
    
    function Name: string := Self.GetType.Name;
    
    procedure SetClassLabels(classes: array of string);
    
    function GetClassLabels: array of string;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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

  RegSplitResult = record
    Found: boolean;
    Feature: integer;
    Threshold: real;
    LeftCount: integer;
    LeftOrderSize: integer;
    WeightedScore: real;
    
    static function Invalid: RegSplitResult;
    begin
      Result.Found := false;
      Result.Feature := -1;
      Result.Threshold := 0.0;
      Result.LeftCount := 0;
      Result.LeftOrderSize := 0;
      Result.WeightedScore := real.PositiveInfinity;
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
  
  /// Внутреннее ядро дерева решений (Decision Tree).
  /// Реализует алгоритм построения и предсказания без учёта кодирования меток.
  ///
  /// Ожидает, что целевые значения y уже закодированы в диапазон 0..K-1.
  /// Возвращает предсказания в том же закодированном виде.
  ///
  /// Не выполняет:
  /// - кодирование/декодирование меток
  /// - проверку корректности входных данных
  ///
  /// Используется как вычислительный модуль внутри моделей
  /// (например, DecisionTreeClassifier, RandomForest и др.)
  DecisionTreeCore = class
  private
    fRoot: DecisionTreeNode;
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fCriterion: ISplitCriterion;
    fClassCount: integer;
    fMaxFeatures: integer;
    fUserProvidedSeed: boolean;
    fRandomSeed: integer;
    fRng: System.Random;
    fFeatureImportances: Vector;

  public
    constructor Create(
      maxDepth: integer;
      minSamplesSplit: integer;
      minSamplesLeaf: integer;
      criterion: ISplitCriterion;
      classCount: integer;
      maxFeatures: integer;
      seed: integer := -1
    );

    procedure Fit(X: Matrix; y: Vector);     // y уже 0..K-1
    function Predict(X: Matrix): Vector;     // возвращает 0..K-1
    function PredictRow(X: Matrix; row: integer): integer;
    
  private
    function GetFeatureImportances: Vector;

    function CreateLeaf(y: Vector; indices: array of integer): DecisionTreeNode;

    function BuildNode(X: Matrix; y: Vector; indices: array of integer; depth: integer): DecisionTreeNode;

    function FindBestSplitCore(X: Matrix; y: Vector; indices: array of integer;
      var bestF: integer; var bestT: real): boolean;

    function MajorityClass(y: Vector; indices: array of integer): integer;
  public
    property FeatureImportances: Vector read GetFeatureImportances;

    function PredictOne(x: Vector; node: DecisionTreeNode): integer;
    function Clone: DecisionTreeCore;
  end;  
  

//============================  
//   DecisionTreeClassifier  
//============================  
/// Дерево решений для задачи классификации.
/// Использует критерий нечистоты (обычно Gini) для выбора оптимальных разбиений.
/// В листьях хранится наиболее частый класс
  DecisionTreeClassifier = class(IClassifier, IClassifierInternal)
  private
    fMaxDepth: integer;
    fMinSamplesSplit: integer;
    fMinSamplesLeaf: integer;
    fFitted: boolean;
    fCriterion: ISplitCriterion;
    fFeatureImportances: Vector;
    fRandomSeed: integer;
    fMaxFeatures: integer := 0;
    fUserProvidedSeed: boolean;
    fRng: System.Random;  
  
    fCore: DecisionTreeCore;
    fIndexToClass: array of integer;
    fClassLabels: array of string;

  public
/// Создает классификационное дерево:
///   • maxDepth — максимальная глубина дерева (-1 означает без ограничения).
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе
    constructor Create(maxDepth: integer := 10; minSamplesSplit: integer := 2; minSamplesLeaf: integer := 1; 
      criterion: ISplitCriterion := nil; 
      maxFeatures: integer := 0;
      seed: integer := -1);

/// Обучает классификационное дерево.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с метками классов (целочисленные значения).
/// Строит структуру дерева, минимизируя нечистоту в узлах.
///
/// Примечание:
///   • обученное состояние дерева НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; 
    
/// Выполняет предсказание меток классов для X.
/// Для каждого объекта возвращается класс, соответствующий листу дерева.
    function Predict(X: Matrix): Vector; 
    
    /// Возвращает предсказанные метки классов для объектов из X.
    /// Каждый элемент результата — индекс класса (целое число).
    /// Порядок элементов соответствует строкам матрицы X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;
    
/// Возвращает строковое представление модели.
    function ToString: string; override;
    
    function ClassCount: integer := fIndexToClass.Length;
    
    function IndexToClass: array of integer := Copy(fIndexToClass);
    
    function Name: string := Self.GetType.Name;
    
    procedure SetClassLabels(classes: array of string);
    
    function GetClassLabels: array of string;
    
/// Возвращает true, если дерево обучено.
/// Если false — Predict вызовет ошибку.
    property IsFitted: boolean read fFitted;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
    function Clone: IModel; 
  end;
  
//============================  
//    DecisionTreeRegressorBase
//============================  
/// Базовый абстрактный класс дерева решений.
/// Используется только DecisionTreeRegressor
/// Classifier использует DecisionTreeCore
/// Реализует общую логику построения структуры дерева:
///   рекурсивное разбиение, контроль глубины,
///   минимального числа объектов и расчет важности признаков.
/// Конкретная логика вычисления значения листа и критерия разбиения задается в наследнике
  DecisionTreeRegressorBase = abstract class(ITreeModel)
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
  
    function IsPure(y: Vector; indices: array of integer): boolean; virtual;
// --------------------------    
  
    function LeafValue(y: Vector; indices: array of integer): real; virtual; abstract;
    function LeafNode(value: real): DecisionTreeNode;
    
    procedure SetRowIndices(rows: array of integer);
  public
/// Создает дерево решений:
///   • maxDepth — максимальная глубина дерева.
///   • minSamplesSplit — минимальное число объектов для разбиения узла.
///   • minSamplesLeaf — минимальное число объектов в листе
    constructor Create(maxDepth: integer; minSamplesSplit: integer; minSamplesLeaf: integer; 
      criterion: ISplitCriterion; seed: integer);
  
/// Возвращает вектор важности признаков.
/// Важность вычисляется как суммарное уменьшение
///   нечистоты (impurity reduction) по всем разбиениям.
/// Значения нормированы так, что сумма равна 1.
    function FeatureImportances: Vector;
    
/// Обучает дерево решений для задачи регрессии.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с непрерывными значениями.
///
/// Примечание:
///   • обученное состояние дерева НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; virtual; abstract;

/// Выполняет предсказание для матрицы X.
/// Возвращает вектор прогнозов.
/// Для регрессии — вещественные значения.
/// Для классификации — метки классов.
    function Predict(X: Matrix): Vector; virtual; abstract;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
    function Clone: IModel; virtual; abstract;

/// Возвращает true, если дерево обучено.
/// Если false — Predict вызовет ошибку.
    property IsFitted: boolean read fFitted;
    
    function Name: string := Self.GetType.Name;
  end;
  
//============================  
//   DecisionTreeRegressor  
//============================  
/// Дерево решений для задачи регрессии.
/// Наследуется от DecisionTreeBase.
/// Использует критерий дисперсии для выбора разбиений.
/// В листьях хранится среднее значение целевой переменной.
/// Поддерживает L2-регуляризацию значения листа (leafL2)
  DecisionTreeRegressor = class(DecisionTreeRegressorBase, IRegressor)
  private
    fLeafL2: real;
    fSortedOrders: array of array of integer;
    fUseSortedOrdersAsRoot: boolean;
    fRowWeights: array of integer;
      
    fVisitMarks: array of integer;
    fVisitId: integer;
  
    function PredictOne(X: Matrix; rowIndex: integer): real;
  
  protected
/// Вычисляет значение листа для набора индексов.
/// В регрессии это среднее целевой переменной
/// с учетом L2-регуляризации (если leafL2 > 0).
    function LeafValue(y: Vector; indices: array of integer): real; override;
    function BuildTreeNew(X: Matrix; y: Vector; indices: array of integer; depth: integer): DecisionTreeNode;
    function BuildTreeNode(X: Matrix; y: Vector; nodeOrders: array of array of integer; depth: integer): DecisionTreeNode;
    
    function FindBestSplitReg(X: Matrix; y: Vector; nodeOrders: array of array of integer): RegSplitResult;
    procedure BuildSortedOrders(X: Matrix; indices: array of integer);
    function BuildInitialNodeOrders(indices: array of integer): array of array of integer;
    procedure SplitNodeOrders(nodeOrders: array of array of integer; feature: integer; leftCount, leftOrderSize: integer;
      var leftOrders, rightOrders: array of array of integer);
    function BuildMembershipMask(rowCount: integer; indices: array of integer): array of boolean;
    procedure ComputeNodeStats(yData: array of real; indices: array of integer; var sumAll, sumSqAll: real);
    function WeightedVariance(n, leftCount: integer; leftSum, leftSumSq, sumAll, sumSqAll: real): real;
    function GetFeatureSubset(p: integer): array of integer;
    function SampleWeight(rowIndex: integer): integer;
    function TotalWeight(indices: array of integer): integer;
    
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
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с непрерывными значениями.
///
/// Примечание:
///   • обученное состояние дерева НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;

/// Выполняет предсказание для всех объектов X.
/// Возвращает вектор вещественных значений.
    function Predict(X: Matrix): Vector; override;
    
/// Внутренний hook для ансамблей: позволяет переиспользовать
/// уже отсортированные порядки строк по признакам.
/// Обычному пользовательскому коду не нужен.
    procedure SetPreSortedOrders(sortedOrders: array of array of integer);
    
/// Внутренний hook для bootstrap-подвыборок с повторами.
/// Передаются уже готовые сортированные порядки именно для корневого узла.
    procedure SetPreSortedRootOrders(sortedOrders: array of array of integer);
    procedure SetBootstrapRootOrders(sortedOrders: array of array of integer; rowWeights: array of integer);

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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

/// Обучает ансамбль деревьев на данных.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с целевыми значениями.
/// Для каждого дерева используется bootstrap-подвыборка
/// и случайное подмножество признаков.
///
/// Примечание:
///   • обученное состояние ансамбля НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; virtual; abstract;

/// Выполняет предсказание для матрицы X.
/// В регрессии — усреднение предсказаний деревьев.
/// В классификации — голосование (majority vote) или усреднение вероятностей.    
    function Predict(X: Matrix): Vector; virtual; abstract;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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

/// Обучает регрессионный случайный лес.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с непрерывными значениями.
/// Для каждого дерева используется bootstrap-подвыборка
/// и случайное подмножество признаков.
///
/// Примечание:
///   • обученное состояние ансамбля НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;

/// Выполняет предсказание для X.
/// Итоговое значение — среднее предсказаний всех деревьев ансамбля.
    function Predict(X: Matrix): Vector; override;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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
  RandomForestClassifier = class(RandomForestBase, IProbabilisticClassifier, IClassifierInternal)
  private
    fTrees: array of DecisionTreeCore;
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
  
/// Обучает классификационный случайный лес.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с метками классов (целочисленные значения).
/// Для каждого дерева используется bootstrap-подвыборка обучающих объектов.
/// В каждом узле рассматривается случайное подмножество признаков согласно maxFeaturesMode.
///
/// Примечание:
///   • обученное состояние ансамбля НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;

/// Выполняет предсказание меток классов для матрицы X.
/// Для каждого объекта агрегируются предсказания всех деревьев.
/// Итоговый класс определяется большинством голосов или максимальной суммарной вероятностью.
    function Predict(X: Matrix): Vector; override;
    
    /// Возвращает предсказанные метки классов для объектов из X.
    /// Каждый элемент результата — исходная метка класса (целое число).
    /// Порядок элементов соответствует строкам матрицы X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;
    
    /// Возвращает матрицу вероятностей классов для всех объектов из X.
    /// Размер результата: nSamples × nClasses, где:
    /// - nSamples — число объектов в X;
    /// - nClasses — число классов модели.
    /// Элемент [i, k] содержит вероятность того, что объект i принадлежит классу k.
    /// Сумма вероятностей в каждой строке равна 1.
    function PredictProba(X: Matrix): Matrix;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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

/// Обучает модель градиентного бустинга для регрессии.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с непрерывными значениями.
/// Обучение выполняется без early stopping.
/// Для предотвращения переобучения используйте FitWithValidation.
/// Early stopping работает только при наличии валидационной выборки
/// и параметре earlyStoppingPatience > 0.
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
    
/// Предсказывает значения целевой переменной.
/// Используются все обученные деревья.
    function Predict(X: Matrix): Vector;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
    function Clone: IModel;
    
/// Обучает модель градиентного бустинга с использованием валидационной выборки.
/// Поддерживает early stopping:
/// обучение останавливается, если метрика не улучшается
/// в течение earlyStoppingPatience итераций.
/// Возвращает обученную модель.
    function FitWithValidation(XTrain: Matrix; yTrain: Vector; XVal: Matrix; yVal: Vector): ISupervisedModel;

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
  GradientBoostingClassifier = class(IProbabilisticClassifier, IClassifierInternal)
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

    //procedure BuildClassMapping(y: Vector);
    //function ApplyLabelEncoding(y: Vector): array of integer;

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

/// Обучает модель градиентного бустинга для классификации.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с метками классов (целочисленные значения).
/// Обучение выполняется без early stopping.
/// Для предотвращения переобучения используйте FitWithValidation.
/// Early stopping работает только при наличии валидационной выборки
/// и параметре earlyStoppingPatience > 0.
///
/// Примечание:
///   • обученное состояние модели НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
    
/// Обучает модель градиентного бустинга с использованием валидационной выборки.
/// Поддерживает early stopping:
///   обучение останавливается, если метрика не улучшается в течение earlyStoppingPatience итераций.
/// Возвращает обученную модель.
    function FitWithValidation(XTrain: Matrix; yTrain: Vector; XVal: Matrix; yVal: Vector): ISupervisedModel;

/// Предсказывает метки классов.
/// Возвращает исходные значения классов, а не внутренние индексы.
    function Predict(X: Matrix): Vector;
    
    function PredictLabels(X: Matrix): array of integer;

/// Возвращает матрицу вероятностей классов для всех объектов из X.
/// Размер результата: nSamples × nClasses, где:
/// - nSamples — число объектов в X;
/// - nClasses — число классов модели.
/// Элемент [i, k] содержит вероятность того, что объект i принадлежит классу k.
/// Сумма вероятностей в каждой строке равна 1.
    function PredictProba(X: Matrix): Matrix;
    
    function GetClasses: array of real;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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
    
/// Обучает модель k ближайших соседей.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с целевыми значениями.
/// Обучение заключается в запоминании обучающей выборки.
///
/// Примечание:
///   • обученное состояние модели (обучающая выборка) НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; virtual; abstract;
    
    /// Выполняет предсказание для матрицы признаков X.
    /// Возвращает вектор предсказанных значений или меток
    function Predict(X: Matrix): Vector; virtual; abstract;
    
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
    function Clone: IModel; virtual; abstract;
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;
  
  /// Классификатор на основе алгоритма k ближайших соседей (kNN).
  /// Поддерживает равномерное (Uniform) и взвешенное по расстоянию (Distance) голосование.
  /// Реализует вероятностные предсказания через PredictProba
  /// ВАЖНО: KNN чувствителен к масштабу признаков.
  /// Всегда используйте StandardScaler или MinMaxScaler в Pipeline перед KNN.
  KNNClassifier = class(KNNBase, IProbabilisticClassifier, IClassifierInternal)
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
    
  public
    /// Создаёт классификатор kNN.
    /// k — число ближайших соседей (k > 0).
    /// weighting — режим взвешивания голосов соседей
    constructor Create(k: integer; weighting: KNNWeighting := KNNWeighting.Uniform);
    
/// Обучает классификатор k ближайших соседей.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с метками классов (произвольные числовые значения).
/// Обучение заключается в запоминании обучающей выборки.
///
/// Примечание:
///   • обученное состояние модели (обучающая выборка) НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;
    
/// Выполняет предсказание меток классов для объектов X.
/// Возвращает вектор предсказанных меток.
/// Не является потокобезопасным: не вызывать одновременно из нескольких потоков
/// для одного экземпляра модели.
    function Predict(X: Matrix): Vector; override;
    
    /// Выполняет предсказание меток классов для объектов X.
    /// Возвращает массив индексов классов.
    /// Не является потокобезопасным: не вызывать одновременно из нескольких потоков
    /// для одного экземпляра модели.
    function PredictLabels(X: Matrix): array of integer;
    
    /// Возвращает матрицу вероятностей классов для всех объектов из X.
    /// Размер результата: nSamples × nClasses, где:
    /// - nSamples — число объектов в X;
    /// - nClasses — число классов модели.
    /// Элемент [i, k] содержит вероятность того, что объект i принадлежит классу k.
    /// Сумма вероятностей в каждой строке равна 1.
    function PredictProba(X: Matrix): Matrix;
    
    /// Возвращает массив меток классов в порядке столбцов PredictProba
    function GetClasses: array of real;
    
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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
    
/// Обучает регрессор k ближайших соседей.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с непрерывными значениями.
/// Обучение заключается в запоминании обучающей выборки.
///
/// Примечание:
///   • обученное состояние модели (обучающая выборка) НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel; override;
    
    /// Выполняет предсказание числовых значений для объектов X.
    /// Возвращает вектор предсказанных значений
    function Predict(X: Matrix): Vector; override;
    
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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

/// Обучает модель k-средних по матрице признаков.
///   X — матрица m × n (m объектов, n признаков).
/// Выполняет nInit запусков и выбирает решение с минимальной инерцией.
///
/// Примечание:
///   • обученное состояние модели (центры кластеров) НЕ копируется методом Clone
    function Fit(X: Matrix): IUnsupervisedModel;
    
    /// Возвращает индекс кластера для каждого объекта из X.
    /// Требует предварительного вызова Fit.
    function Predict(X: Matrix): Vector;
    
    /// Возвращает индекс кластера для каждого объекта из X.
    /// Требует предварительного вызова Fit.
    function PredictLabels(X: Matrix): array of integer;

    /// Выполняет обучение и сразу возвращает метки кластеров.
    function FitPredict(X: Matrix): array of integer;

/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
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
  
/// Обучает модель DBSCAN по матрице признаков.
///   X — матрица m × n (m объектов, n признаков).
/// Выполняет кластеризацию на основе плотности и определяет метки кластеров.
///
/// Примечание:
///   • обученное состояние модели (метки кластеров) НЕ копируется методом Clone
    function Fit(X: Matrix): IUnsupervisedModel;
  
    /// Возвращает метки кластеров.
    function PredictLabels(X: Matrix): array of integer;

    /// Возвращает метки после обучения.
    function FitPredict(X: Matrix): array of integer;
    
/// Копирует только конфигурацию модели (без обученного состояния).
/// Используется для создания независимых экземпляров модели.
    function Clone: IModel;
  
    property Labels: array of integer read fLabels;
    
    /// Количество кластеров, найденное моделью
    function ClustersCount: integer; 
    
    function Name: string := Self.GetType.Name;
    
    property IsFitted: boolean read fFitted;
  end;  
  
{$endregion Models}

{$region MatrixPipeline}
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
/// 
/// MatrixPipeline используется, когда данные уже представлены 
/// в виде Matrix X и Vector y
  MatrixPipeline = class(ISupervisedModel)
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
    // MatrixPipeline.Build используется, когда данные уже представлены
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
    // MatrixPipeline объединяет несколько матричных преобразований (ITransformer)
    // и модель (IModel) в единый объект, который можно обучать и использовать
    // для предсказаний.
    //
    // В отличие от этого, DataPipeline.Build используется,
    // когда исходные данные представлены в виде DataFrame
    // и требуется выполнить препроцессинг таблицы
    // (Imputer, OneHotEncoder, OrdinalEncoder и др.)
    // перед преобразованием данных в Matrix/Vector.
    
    var pipe1 :=
      MatrixPipeline.Build(
        new StandardScaler,
        new LogisticRegression
      );
    
    var pipe2 :=
      MatrixPipeline.Build(
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
    static function Build(params steps: array of IPipelineStep): MatrixPipeline;
    
    /// Устанавливает или заменяет модель.
    function SetModel(m: ISupervisedModel): MatrixPipeline;
  
    /// Добавляет преобразование в конец пайплайна
    function Add(t: ITransformer): MatrixPipeline;
  
/// Обучает пайплайн на данных.
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с целевыми значениями.
/// Последовательно обучает все трансформеры и применяет их к данным,
/// после чего обучает финальную supervised-модель.
///
/// Примечание:
///   • обученное состояние пайплайна НЕ копируется методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  
    /// Применяет только преобразования (без модели)
    function Transform(X: Matrix): Matrix;
  
    /// Делает предсказание
    function Predict(X: Matrix): Vector;
  
    /// Возвращает матрицу вероятностей классов для всех объектов из X.
    /// Размер результата: nSamples × nClasses, где:
    /// - nSamples — число объектов в X;
    /// - nClasses — число классов модели.
    /// Элемент [i, k] содержит вероятность того, что объект i принадлежит классу k.
    /// Сумма вероятностей в каждой строке равна 1.
    function PredictProba(X: Matrix): Matrix;
    
    /// Показывает, был ли пайплайн обучен (вызван метод Fit).
    property IsFitted: boolean read fFitted;
    
/// Возвращает строковое представление пайплайна.
    function ToString: string; override;

/// Копирует только конфигурацию пайплайна (без обученного состояния).
/// Используется для создания независимых экземпляров пайплайна.
    function Clone: IModel;
    
    function Name: string := Self.GetType.Name;
  end;
  
/// Последовательный конвейер машинного обучения (unsupervised).
/// Гарантирует строгий порядок выполнения шагов:
///   [преобразователи].
///
/// Поддерживает:
///   • преобразователи без учёта целевой переменной (unsupervised).
///
/// Все преобразователи применяются последовательно к признакам X.
///
/// Обеспечивает единый интерфейс Fit(X) / Transform(X)
/// и воспроизводимость полного процесса преобразования данных.
///
/// UMatrixPipeline используется, когда данные уже представлены
/// в виде Matrix X и отсутствует целевая переменная
  UMatrixPipeline = class(IUnsupervisedModel)
  private
    fTransformers: List<ITransformer>;
    fModel: IModel;
    fFitted: boolean;
  public
  /// Создаёт конвейер машинного обучения для заданной модели:
///   • model — модель, которая будет применена
///     после последовательного применения всех преобразователей.
/// Модель должна реализовывать интерфейс IModel (без учёта целевой переменной)
    constructor Create(model: IModel);

/// Создаёт пустой пайплайн (конвейер машинного обучения).
/// Модель должна быть установлена через SetModel    
    constructor Create;
  
/// Строит конвейер машинного обучения из последовательности шагов.
/// Шаги указываются в порядке выполнения:
///   сначала преобразователи, затем модель.
/// Последний шаг обязан быть моделью (IModel).
/// Возвращает сконструированный конвейер.
    static function Build(params steps: array of IPipelineStep): UMatrixPipeline;
  
/// Устанавливает или заменяет модель.
    function SetModel(m: IModel): UMatrixPipeline;
    
/// Добавляет преобразование в конец пайплайна    
    function Add(t: ITransformer): UMatrixPipeline;
  
/// Обучает пайплайн на данных.
///   X — матрица m × n (m объектов, n признаков).
/// Последовательно обучает все трансформеры и применяет их к данным,
/// после чего обучает финальную модель.
///
/// Примечание:
///   • обученное состояние пайплайна НЕ копируется методом Clone  
    function Fit(X: Matrix): IUnsupervisedModel;
    
/// Применяет последовательность преобразований к данным.
///   X — матрица m × n (m объектов, n признаков).
/// Последовательно применяет все трансформеры к данным,
/// после чего возвращает преобразованную матрицу.
///
/// Примечание:
///   • обученное состояние пайплайна НЕ копируется методом Clone    
    function Transform(X: Matrix): Matrix;
    
/// Применяет пайплайн и возвращает результат модели.
///   X — матрица m × n (m объектов, n признаков).
/// После применения всех преобразований вызывает модель,
/// реализующую интерфейс IModel.
///
/// Примечание:
///   • семантика результата зависит от конкретной модели
///     (например, кластерные метки, оценки и т.д.)    
    function Predict(X: Matrix): Vector;
  
/// Показывает, был ли пайплайн обучен (вызван метод Fit у шагов пайплайна).  
    property IsFitted: boolean read fFitted;
  
/// Возвращает строковое представление пайплайна.
    function ToString: string; override;
    
/// Копирует только конфигурацию пайплайна (без обученного состояния).
/// Используется для создания независимых экземпляров пайплайна.
    function Clone: IModel;
    function Name: string := Self.GetType.Name;
  end;
  
{$endregion MatrixPipeline}
  
{$region Transformers}

/// Стандартизирует признаки: вычитает среднее
///   и делит на стандартное отклонение по каждому столбцу.
/// Используется для приведения признаков к сопоставимому масштабу.
///
/// Примечание:
///   • стандартное отклонение вычисляется с делением на n
///     (population variance, как в sklearn)
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
///   X — матрица m × n (m объектов, n признаков).
/// Сохраняет параметры масштабирования для последующего применения в Transform.
///
/// Примечание:
///   • вычисленные параметры НЕ копируются методом Clone
    function Fit(X: Matrix): IUnsupervisedTransformer;
    
    /// Последовательно выполняет Fit и Transform на одних и тех же данных.
    function FitTransform(X: Matrix): Matrix;
  
    /// Применяет стандартизацию к данным.
    function Transform(X: Matrix): Matrix;
    
    /// Обратная операция к Transform.
    function InverseTransform(X: Matrix): Matrix;
  
    /// Средние значения признаков, вычисленные при обучении.
    property Mean: Vector read fMean;
  
    /// Стандартные отклонения признаков, вычисленные при обучении.
    property Std: Vector read fStd;
  
    /// Признак того, что преобразование обучено.
    property IsFitted: boolean read fFitted;

    function ToString: string; override;
    
/// Копирует только конфигурацию трансформера (без обученного состояния).
/// Используется для создания независимых экземпляров трансформера.
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
///   X — матрица m × n (m объектов, n признаков).
/// Сохраняет параметры масштабирования для последующего применения в Transform.
///
/// Примечание:
///   • вычисленные параметры НЕ копируются методом Clone
    function Fit(X: Matrix): IUnsupervisedTransformer;
    
    /// Последовательно выполняет Fit и Transform на одних и тех же данных.
    function FitTransform(X: Matrix): Matrix;
  
    /// Применяет линейное масштабирование признаков к диапазону [0, 1].
    function Transform(X: Matrix): Matrix;
    
    /// Преобразование, обратное Transform
    function InverseTransform(X: Matrix): Matrix;
  
    /// Минимальные значения признаков, вычисленные при обучении.
    property Min: Vector read fMin;
  
    /// Максимальные значения признаков, вычисленные при обучении.
    property Max: Vector read fMax;
  
    /// Признак того, что преобразование обучено.
    property IsFitted: boolean read fFitted;

    function ToString: string; override;

/// Копирует только конфигурацию трансформера (без обученного состояния).
/// Используется для создания независимых экземпляров трансформера.
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
  
/// Обучает трансформер PCA на матрице признаков.
///   X — матрица m × n (m объектов, n признаков).
/// Вычисляет главные компоненты и среднее значение признаков
/// для последующего снижения размерности в Transform.
///
/// Примечание:
///   • вычисленные параметры НЕ копируются методом Clone
    function Fit(X: Matrix): IUnsupervisedTransformer;
    
    /// Последовательно выполняет Fit и Transform на одних и тех же данных.
    function FitTransform(X: Matrix): Matrix;
  
    /// Преобразует матрицу X в пространство главных компонент.
    /// Возвращает матрицу m × k.
    function Transform(X: Matrix): Matrix;
  
    property Components: Matrix read fComponents;
    property Mean: Vector read fMean;
    property IsFitted: boolean read fFitted;

    function ToString: string; override;

/// Копирует только конфигурацию трансформера (без обученного состояния).
/// Используется для создания независимых экземпляров трансформера.
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
  
/// Вычисляет дисперсии признаков и выбирает признаки,
/// удовлетворяющие заданному порогу.
///   X — матрица m × n (m объектов, n признаков).
/// Сохраняет индексы выбранных признаков для последующего применения в Transform.
///
/// Примечание:
///   • выбранные признаки НЕ копируются методом Clone
    function Fit(X: Matrix): IUnsupervisedTransformer;
    
    /// Последовательно выполняет Fit и Transform на одних и тех же данных.
    function FitTransform(X: Matrix): Matrix;
  
    /// Возвращает матрицу, содержащую только отобранные признаки.
    function Transform(X: Matrix): Matrix;
  
    /// Индексы выбранных признаков.
    property SelectedFeatures: array of integer read fSelected;
  
    /// Показывает, был ли выполнен Fit.
    property IsFitted: boolean read fFitted;

    function ToString: string; override;
    
/// Копирует только конфигурацию трансформера (без обученного состояния).
/// Используется для создания независимых экземпляров трансформера.
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
///   X — матрица m × n (m объектов, n признаков).
///   y — вектор длины m с целевыми значениями.
/// Сохраняет индексы выбранных признаков для последующего применения в Transform.
///
/// Примечание:
///   • выбранные признаки НЕ копируются методом Clone
    function Fit(X: Matrix; y: Vector): ISupervisedTransformer;
    
    /// Последовательно выполняет Fit и Transform на одних и тех же данных.
    function FitTransform(X: Matrix; y: Vector): Matrix;
  
    /// Возвращает матрицу, содержащую только выбранные признаки.
    function Transform(X: Matrix): Matrix;
  
    /// Индексы выбранных признаков.
    property SelectedFeatures: array of integer read fSelected;
  
    /// Показывает, был ли выполнен Fit.
    property IsFitted: boolean read fFitted;

/// Возвращает строковое представление трансформера
    function ToString: string; override;

/// Копирует только конфигурацию трансформера (без обученного состояния).
/// Используется для создания независимых экземпляров трансформера.
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
  
/// Подготавливает трансформер нормализации к работе.
///   X — матрица m × n (m объектов, n признаков).
/// Нормализация выполняется независимо для каждой строки,
/// поэтому этап Fit является формальным и не накапливает параметры.
///
/// Примечание:
///   • у трансформера отсутствует обученное состояние
    function Fit(X: Matrix): IUnsupervisedTransformer;

    /// Последовательно выполняет Fit и Transform на одних и тех же данных.
    function FitTransform(X: Matrix): Matrix;

/// Применяет нормализацию к матрице X.
/// Каждая строка масштабируется так, чтобы ее норма соответствовала выбранному типу.
/// Возвращает новую матрицу с нормализованными объектами.
    function Transform(X: Matrix): Matrix;
  
/// Показывает, был ли вызван метод Fit.
/// Если False, вызов Transform может привести к ошибке.
    property IsFitted: boolean read fFitted;

/// Возвращает строковое представление трансформера
    function ToString: string; override;
    
/// Копирует только конфигурацию трансформера (без обученного состояния).
/// Используется для создания независимых экземпляров трансформера.
    function Clone: ITransformer;
  end;
  
{$endregion Transformers}


{$region Utility functions}


{$endregion Utility functions}

type
/// Проверять ли входные данные моделей на NaN и Infinity.
///
/// По умолчанию: True — модели валидируют вход и выбрасывают исключение.
///
/// Установка в False отключает проверки ГЛОБАЛЬНО для всех моделей.
/// Использовать только если данные гарантированно очищены.
///
/// При наличии NaN/Inf поведение моделей не определено
  MLConfig = static class
  public
    /// Проверять ли входные данные моделей на NaN, Inf
    /// Изменение этого флага влияет на все модели и все параллельные вычисления.
    static ValidateFiniteInputs: boolean := True;
  end;

implementation  

uses MLExceptions;
uses MLUtilsABC;

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
  ER_N_ESTIMATORS_NOT_POSITIVE =
    'Параметр nEstimators должен быть > 0!!nEstimators must be > 0';
  ER_LEARNING_RATE_NOT_POSITIVE =
    'Параметр learningRate должен быть > 0!!learningRate must be > 0';
  ER_SUBSAMPLE_OUT_OF_RANGE =
    'Параметр subsample должен быть в диапазоне (0, 1]!!subsample must be in (0, 1]'; 
  ER_N_ESTIMATORS_INVALID =
    'nEstimators должен быть > 0!!nEstimators must be > 0';
  ER_LEARNING_RATE_INVALID =
    'learningRate должен быть > 0!!learningRate must be > 0';
  ER_MAX_DEPTH_INVALID =
    'maxDepth должен быть > 0!!maxDepth must be > 0';
  ER_SUBSAMPLE_INVALID =
    'subsample должен быть > 0!!subsample must be > 0'; 
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
  ER_MIN_LEAF_GT_SPLIT =
    'minSamplesSplit ({1}) должен быть >= 2 * minSamplesLeaf ({0}).!!' +
    'minSamplesSplit ({1}) must be >= 2 * minSamplesLeaf ({0}).';
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
  ER_NEED_AT_LEAST_TWO_CLASSES =
    'Необходимо как минимум два различных класса!!At least two distinct classes are required';  
  ER_UNKNOWN_CLASS_LABEL =
    'Неизвестная метка класса: {0}!!Unknown class label: {0}';
  ER_MAX_FEATURES_INVALID =
    'maxFeatures должен быть >= 0!!maxFeatures must be >= 0';
  ER_OOB_REQUIRES_SUBSAMPLE =
    'Для OOB early stopping требуется subsample < 1.0!!OOB early stopping requires subsample < 1.0';    
    
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
  if MLConfig.ValidateFiniteInputs then
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
  
  if X = nil then
    ArgumentNullError(ER_X_NULL);
  
  if MLConfig.ValidateFiniteInputs then
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
  Result := new LinearRegression;
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
  if MLConfig.ValidateFiniteInputs then
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

  if MLConfig.ValidateFiniteInputs then
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
  Result := new RidgeRegression(fLambda);
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

  if MLConfig.ValidateFiniteInputs then
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

  if MLConfig.ValidateFiniteInputs then
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
  Result := new ElasticNet(fLambda1, fLambda2, fMaxIter, fTol);
end;

//-----------------------------
//     LassoRegression 
//-----------------------------

constructor LassoRegression.Create(alpha: real; maxIter: integer; tol: real);
begin
  fAlpha := alpha;
  fMaxIter := maxIter;
  fTol := tol;

  // Lasso = ElasticNet с L2 = 0
  fModel := new ElasticNet(fAlpha, 0.0, fMaxIter, fTol);
end;

function LassoRegression.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  fModel.Fit(X, y);
  Result := Self;
end;

function LassoRegression.Predict(X: Matrix): Vector;
begin
  Result := fModel.Predict(X);
end;

function LassoRegression.Clone: IModel;
begin
  Result := new LassoRegression(fAlpha, fMaxIter, fTol);
end;

function LassoRegression.GetCoefficients: Vector;
begin
  Result := fModel.Coefficients;
end;

function LassoRegression.GetIntercept: real;
begin
  Result := fModel.Intercept;
end;

function LassoRegression.GetIsFitted: boolean;
begin
  Result := (fModel <> nil) and fModel.IsFitted;
end;
//-----------------------------
//     LogisticRegression 
//-----------------------------

constructor LogisticRegression.Create(lambda: real; learningRate: real; epochs: integer;
  tol: real; checkConvergence: boolean; minImprovement: real; useFastExp: boolean);
begin
  fLambda := lambda;
  fLearningRate := learningRate;
  fEpochs := epochs;
  fFitted := false;
  fTol := tol;
  fCheckConvergence := checkConvergence;
  fMinImprovement := minImprovement;
  fUseFastExp := useFastExp
end;

function FastExp(x: real): real;
begin
  if x < -5 then exit(0.0);
  if x > 5 then x := 5;

  Result := 1.0 + x + 0.5*x*x + (1.0/6.0)*x*x*x;
end;

type RealArr = array of real;

function LogisticRegression.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if MLConfig.ValidateFiniteInputs then
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

  // --- convert to integer labels
  var yInt := LabelsToInts(y);
  
  // --- encode (порядок первого появления)
  var unique: array of integer;
  var yEncoded := EncodeLabelsInt(yInt, unique);
  
  fClassCount := unique.Length;
  
  if fClassCount < 2 then
    ArgumentError(ER_LOGISTIC_NEED_AT_LEAST_TWO_CLASSES);
  
  // --- build mappings
  fClassToIndex := new Dictionary<integer, integer>;
  SetLength(fIndexToClass, fClassCount);
  
  for var i := 0 to fClassCount - 1 do
  begin
    fClassToIndex[unique[i]] := i;
    fIndexToClass[i] := unique[i];
  end;
  
  SetLength(fClassLabels, fIndexToClass.Length);
  for var i := 0 to fIndexToClass.Length - 1 do
    fClassLabels[i] := fIndexToClass[i].ToString;

  // --- init
  fW := new Matrix(p, fClassCount);

  fIntercept := new Vector(fClassCount);

  var prevLoss := real.PositiveInfinity;

  var xRows := X.Data.Rows;
  var gradW := new RealArr[fClassCount];
  for var k := 0 to fClassCount - 1 do
    gradW[k] := new real[p];
    
  var gradB := new real[fClassCount];
    
  var zi := new real[fClassCount];
  
  for var epoch := 1 to fEpochs do
  begin
    var WCols := fW.Data.Cols;
    //var WRows := fW.Data.Rows;
    
    var loss := 0.0;
  
    // --- zero gradients
    &Array.Clear(gradB, 0, fClassCount);
    for var k := 0 to fClassCount - 1 do
      &Array.Clear(gradW[k], 0, p);
  
    // --- one pass: logits -> softmax -> loss -> gradient
    for var i := 0 to m - 1 do
    begin
      var xi := XRows[i];
      var yi := yEncoded[i];
  
      // --- logits
      for var k := 0 to fClassCount - 1 do
      begin
        var wk := WCols[k];
        var s := fIntercept.Data[k];
  
        for var j := 0 to p - 1 do
          s += xi[j] * wk[j];
  
        zi[k] := s;
      end;
  
      // --- stable softmax
      var maxVal := zi[0];
      for var k := 1 to fClassCount - 1 do
        if zi[k] > maxVal then
          maxVal := zi[k];
  
      var sumExp := 0.0;
      for var k := 0 to fClassCount - 1 do
      begin
        var v: real;
        if fUseFastExp then
          v := FastExp(zi[k] - maxVal)
        else  
          v := Exp(zi[k] - maxVal);
        zi[k] := v;
        sumExp += v;
      end;
  
      if sumExp <= 0 then
      begin
        var uniformProb := 1.0 / fClassCount;
  
        for var k := 0 to fClassCount - 1 do
          zi[k] := uniformProb;
      end
      else
      begin
        var invSum := 1.0 / sumExp;
  
        for var k := 0 to fClassCount - 1 do
          zi[k] *= invSum;
      end;
  
      // --- loss
      var prob := zi[yi];
      if prob < 1e-300 then
        prob := 1e-300;
  
      loss -= Ln(prob);
  
      // --- gradient
      for var k := 0 to fClassCount - 1 do
      begin
        var diff := zi[k] - Ord(k = yi);
  
        gradB[k] += diff;
  
        var gwk := gradW[k];
        for var j := 0 to p - 1 do
          gwk[j] += xi[j] * diff;
      end;
    end;
  
    loss /= m;
  
    // --- L2 penalty
    if fLambda <> 0 then
    begin
      var l2 := 0.0;
      for var j := 0 to p - 1 do
        for var k := 0 to fClassCount - 1 do
          l2 += fW.Data[j,k] * fW.Data[j,k];
  
      loss += 0.5 * fLambda * l2;
    end;
  
    // --- divergence check
    if double.IsNaN(loss) or double.IsInfinity(loss) then
      ArgumentError(ER_LOGISTIC_INVALID_LOSS);
  
    // --- convergence check
    if fCheckConvergence then
    begin
      if Abs(prevLoss - loss) < Max(fMinImprovement, fTol * Max(1.0, Abs(prevLoss))) then
        break;
  
      prevLoss := loss;
    end;
  
    // --- update
    var invM := 1.0 / m;
  
    for var k := 0 to fClassCount - 1 do
    begin
      gradB[k] *= invM;
      fIntercept.Data[k] -= fLearningRate * gradB[k];
      
      for var j := 0 to p - 1 do
      begin
        var g := gradW[k][j] * invM;
  
        if fLambda <> 0 then
          g += fLambda * fW.Data[j,k];
  
        fW.Data[j,k] -= fLearningRate * g;
      end;
    end;
  end;

  fFitted := true;
  Result := Self;
end;


{function LogisticRegression.FitOld(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if MLConfig.ValidateFiniteInputs then
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

  // --- convert to integer labels
  var yInt := new integer[m];
  
  for var i := 0 to m - 1 do
  begin
    var r := y[i];
    var ir := Round(r);
  
    if Abs(r - ir) > 1e-12 then
      ArgumentError(ER_LABELS_NOT_INTEGER);
  
    yInt[i] := ir;
  end;
  
  // --- encode (порядок первого появления)
  var unique: array of integer;
  var yEncoded := EncodeLabelsInt(yInt, unique);
  
  fClassCount := unique.Length;
  
  if fClassCount < 2 then
    ArgumentError(ER_LOGISTIC_NEED_AT_LEAST_TWO_CLASSES);
  
  // --- build mappings
  fClassToIndex := new Dictionary<integer, integer>;
  SetLength(fIndexToClass, fClassCount);
  
  for var i := 0 to fClassCount - 1 do
  begin
    fClassToIndex[unique[i]] := i;
    fIndexToClass[i] := unique[i];
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
      if Abs(prevLoss - loss) < Max(fTol, fMinImprovement) then
        break;

      prevLoss := loss;
    end;

    // --- gradient
    var gradW := new Matrix(p, fClassCount);
    var gradB := new Vector(fClassCount);

    for var i := 0 to m - 1 do
    begin
      var yi := yEncoded[i];
    
      // заранее считаем diff[k]
      var diffArr := new real[fClassCount];
    
      for var k := 0 to fClassCount - 1 do
      begin
        var diff := Z[i,k];
        if k = yi then
          diff -= 1.0;
    
        diffArr[k] := diff;
        gradB[k] += diff;
      end;
    
      // теперь идём по j (строка X читается линейно)
      for var j := 0 to p - 1 do
      begin
        var xij := X[i,j];
    
        for var k := 0 to fClassCount - 1 do
          gradW[j,k] += xij * diffArr[k];
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
end;}

function LogisticRegression.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
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

  var labels := PredictLabels(X);
  
  Result := new Vector(labels.Length);
  
  for var i := 0 to labels.Length - 1 do
    Result[i] := fIndexToClass[labels[i]];
end;

function LogisticRegression.PredictLabels(X: Matrix): array of integer;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fW.RowCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fW.RowCount);

  var m := X.RowCount;
  var Z := X * fW;

  SetLength(Result, m);

  for var i := 0 to m - 1 do
  begin
    var best := 0;
    var bestVal := Z[i,0] + fIntercept[0];

    for var k := 1 to fClassCount - 1 do
    begin
      var score := Z[i,k] + fIntercept[k];
      if score > bestVal then
      begin
        bestVal := score;
        best := k;
      end;
    end;

    Result[i] := best;
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
  Result := new LogisticRegression(
    fLambda,
    fLearningRate,
    fEpochs,
    fTol,
    fCheckConvergence,
    fMinImprovement,
    fUseFastExp
  );
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

  Result := Copy(fClassLabels);
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

// DecisionTreeCore

constructor DecisionTreeCore.Create(
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  criterion: ISplitCriterion;
  classCount: integer;
  maxFeatures: integer;
  seed: integer
);
begin
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fCriterion := criterion;
  fMaxFeatures := maxFeatures;
  fClassCount := classCount;

  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
  fRng := new System.Random(fRandomSeed);
end;

function DecisionTreeCore.GetFeatureImportances: Vector;
begin
  if fRoot = nil then
    NotFittedError(ER_FIT_NOT_CALLED);

  Result := fFeatureImportances.Normalized;
end;

procedure DecisionTreeCore.Fit(X: Matrix; y: Vector);
begin
  // --- init importance
  fFeatureImportances := new Vector(X.ColCount);

  // --- определить число классов (0..K-1)
  fClassCount := 0;

  for var i := 0 to y.Length - 1 do
  begin
    var v := Round(y[i]);
    if v + 1 > fClassCount then
      fClassCount := v + 1;
  end;

  // --- build tree
  var indices := new integer[X.RowCount];
  for var i := 0 to X.RowCount - 1 do
    indices[i] := i;
  
  fRoot := BuildNode(X, y, indices, 0);

  // --- нормализация importance
  var s := fFeatureImportances.Sum;
  if s > 0 then
    for var i := 0 to fFeatureImportances.Length - 1 do
      fFeatureImportances[i] /= s;
end;

function DecisionTreeCore.Predict(X: Matrix): Vector;
begin
  Result := new Vector(X.RowCount);

  for var i := 0 to X.RowCount - 1 do
    Result[i] := PredictOne(X.GetRow(i), fRoot);
end;

function DecisionTreeCore.PredictRow(X: Matrix; row: integer): integer;
var node: DecisionTreeNode;
begin
  node := fRoot;

  while not node.IsLeaf do
  begin
    if X[row, node.FeatureIndex] <= node.Threshold then
      node := node.Left
    else
      node := node.Right;
  end;

  Result := Round(node.LeafValue);
end;

function DecisionTreeCore.PredictOne(x: Vector; node: DecisionTreeNode): integer;
begin
  if node.IsLeaf then
    exit(Round(node.LeafValue));

  if x[node.FeatureIndex] <= node.Threshold then
    Result := PredictOne(x, node.Left)
  else
    Result := PredictOne(x, node.Right);
end;

function DecisionTreeCore.MajorityClass(y: Vector; indices: array of integer): integer;
begin
  var counts := new integer[fClassCount];

  for var i := 0 to indices.Length - 1 do
  begin
    var cls := Round(y[indices[i]]);
    counts[cls] += 1;
  end;

  var best := 0;
  var bestCnt := counts[0];

  for var k := 1 to fClassCount - 1 do
    if counts[k] > bestCnt then
    begin
      bestCnt := counts[k];
      best := k;
    end;

  Result := best;
end;

function DecisionTreeCore.CreateLeaf(y: Vector; indices: array of integer): DecisionTreeNode;
begin
  Result := new DecisionTreeNode;
  Result.IsLeaf := true;
  Result.LeafValue := MajorityClass(y, indices);
end;

function DecisionTreeCore.BuildNode(X: Matrix; y: Vector; indices: array of integer; depth: integer): DecisionTreeNode;
begin
  // 1. stop: depth
  if (fMaxDepth > 0) and (depth >= fMaxDepth) then
    exit(CreateLeaf(y, indices));

  // 2. stop: min samples
  if indices.Length < fMinSamplesSplit then
    exit(CreateLeaf(y, indices));

  // --- impurity родителя
  var parentImp := fCriterion.Impurity(y, indices);

  // 3. лучший сплит (пока используем старый API)
  var bestFeature: integer;
  var bestThreshold: real;

  if not FindBestSplitCore(X, y, indices, bestFeature, bestThreshold) then
    exit(CreateLeaf(y, indices));

  // 4. split → только индексы
  var leftList := new List<integer>;
  var rightList := new List<integer>;

  for var ii := 0 to indices.Length - 1 do
  begin
    var i := indices[ii];

    if X[i, bestFeature] <= bestThreshold then
      leftList.Add(i)
    else
      rightList.Add(i);
  end;

  if (leftList.Count < fMinSamplesLeaf) or (rightList.Count < fMinSamplesLeaf) then
    exit(CreateLeaf(y, indices));

  var leftIdx := leftList.ToArray;
  var rightIdx := rightList.ToArray;

  // --- impurity детей
  var leftImp := fCriterion.Impurity(y, leftIdx);
  var rightImp := fCriterion.Impurity(y, rightIdx);

  var weighted :=
    (leftIdx.Length * leftImp + rightIdx.Length * rightImp) / indices.Length;

  var gain := parentImp - weighted;

  // --- FEATURE IMPORTANCE
  if gain > 0 then
    fFeatureImportances[bestFeature] += gain;

  // 5. recursion
  var left := BuildNode(X, y, leftIdx, depth + 1);
  var right := BuildNode(X, y, rightIdx, depth + 1);

  // 6. node
  Result := new DecisionTreeNode;
  Result.IsLeaf := false;
  Result.FeatureIndex := bestFeature;
  Result.Threshold := bestThreshold;
  Result.Left := left;
  Result.Right := right;
end;

// Новая реализация (O(n log n · p))
function DecisionTreeCore.FindBestSplitCore(
  X: Matrix;
  y: Vector;
  indices: array of integer;
  var bestF: integer;
  var bestT: real
): boolean;
begin
  Result := false;

  var n := indices.Length;
  if n < 2 then exit;

  var parentImp := fCriterion.Impurity(y, indices);
  var bestScore := parentImp;

  var featureCount := X.ColCount;

  // --- выбор признаков (как было)
  var features: array of integer;

  if (fMaxFeatures > 0) and (fMaxFeatures < featureCount) then
  begin
    var perm := new integer[featureCount];
    for var i := 0 to featureCount - 1 do
      perm[i] := i;

    for var i := 0 to fMaxFeatures - 1 do
    begin
      var j := i + fRng.Next(featureCount - i);
      var tmp := perm[i];
      perm[i] := perm[j];
      perm[j] := tmp;
    end;

    SetLength(features, fMaxFeatures);
    for var i := 0 to fMaxFeatures - 1 do
      features[i] := perm[i];
  end
  else
  begin
    SetLength(features, featureCount);
    for var i := 0 to featureCount - 1 do
      features[i] := i;
  end;

  // --- перебор признаков
  for var fi := 0 to features.Length - 1 do
  begin
    var f := features[fi];

    // пары (value, index)
    var pairs: array of (real, integer);
    SetLength(pairs, n);

    for var i := 0 to n - 1 do
    begin
      var idx := indices[i];
      pairs[i] := (X[idx, f], idx);
    end;

    pairs.Sort(p -> p.Item1);

    // --- counts
    var leftCounts := new integer[fClassCount];
    var rightCounts := new integer[fClassCount];

    for var i := 0 to n - 1 do
      rightCounts[Round(y[pairs[i].Item2])] += 1;

    var leftCount := 0;

    // --- проход
    for var i := 1 to n - 1 do
    begin
      var idx := pairs[i-1].Item2;
      var cls := Round(y[idx]);

      leftCounts[cls] += 1;
      rightCounts[cls] -= 1;
      leftCount += 1;

      var rightCount := n - leftCount;

      if (leftCount < fMinSamplesLeaf) or (rightCount < fMinSamplesLeaf) then
        continue;

      var x1 := pairs[i-1].Item1;
      var x2 := pairs[i].Item1;

      if x1 = x2 then
        continue;

      // --- impurity left
      var leftImp := 0.0;
      var rightImp := 0.0;

      if fCriterion is GiniCriterion then
      begin
        var sumsqL := 0.0;
        var sumsqR := 0.0;

        for var c := 0 to fClassCount - 1 do
        begin
          if leftCounts[c] > 0 then
          begin
            var p := leftCounts[c] / leftCount;
            sumsqL += p * p;
          end;

          if rightCounts[c] > 0 then
          begin
            var p := rightCounts[c] / rightCount;
            sumsqR += p * p;
          end;
        end;

        leftImp := 1.0 - sumsqL;
        rightImp := 1.0 - sumsqR;
      end
      else
      begin
        // entropy
        for var c := 0 to fClassCount - 1 do
        begin
          if leftCounts[c] > 0 then
          begin
            var p := leftCounts[c] / leftCount;
            leftImp -= p * Ln(p);
          end;

          if rightCounts[c] > 0 then
          begin
            var p := rightCounts[c] / rightCount;
            rightImp -= p * Ln(p);
          end;
        end;
      end;

      var score :=
        (leftCount * leftImp + rightCount * rightImp) / n;

      if score < bestScore then
      begin
        bestScore := score;
        bestF := f;
        bestT := (x1 + x2) * 0.5;
        Result := true;
      end;
    end;
  end;
end;

function DecisionTreeCore.Clone: DecisionTreeCore;
begin
  Result := new DecisionTreeCore(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fCriterion,
    fClassCount,
    fMaxFeatures,
    fRandomSeed
  );

  // копируем дерево
  if fRoot <> nil then
    Result.fRoot := fRoot.Clone;
end;

// DecisionTreeBase

constructor DecisionTreeRegressorBase.Create(
  maxDepth: integer;
  minSamplesSplit: integer;
  minSamplesLeaf: integer;
  criterion: ISplitCriterion;
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

 
  if minSamplesSplit < 2 * minSamplesLeaf then
    ArgumentOutOfRangeError(ER_MIN_LEAF_GT_SPLIT, minSamplesLeaf, minSamplesSplit);

  // --- parameters valid → assign

  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fCriterion := criterion;

  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
  fRng := new System.Random(fRandomSeed);
end;

procedure DecisionTreeRegressorBase.SetRowIndices(rows: array of integer);
begin
  if Length(rows) = 0 then
    ArgumentError('Row subset cannot be empty!!Row subset cannot be empty');

  fRowIndices := Copy(rows);
end;

function DecisionTreeRegressorBase.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fFeatureImportances = nil then
    exit(new Vector(0));

  Result := fFeatureImportances.Clone;
end;

function DecisionTreeRegressorBase.LeafNode(value: real): DecisionTreeNode;
begin
  var n := new DecisionTreeNode;
  n.IsLeaf := true;
  n.LeafValue := value;
  Result := n;
end;

function DecisionTreeRegressor.BuildTreeNew(X: Matrix; y: Vector;
  indices: array of integer; depth: integer): DecisionTreeNode;
begin
  var nodeOrders := BuildInitialNodeOrders(indices);
  Result := BuildTreeNode(X, y, nodeOrders, depth);
end;

function DecisionTreeRegressor.BuildTreeNode(X: Matrix; y: Vector;
  nodeOrders: array of array of integer; depth: integer): DecisionTreeNode;
begin
  var indices := nodeOrders[0];
  var n := TotalWeight(indices);
  
  if (fMaxDepth >= 0) and (depth >= fMaxDepth) then
    exit(LeafNode(LeafValue(y, indices)));

  if n < fMinSamplesSplit then
    exit(LeafNode(LeafValue(y, indices)));

  var yData := y.Data;
  var sumAll, sumSqAll: real;
  ComputeNodeStats(yData, indices, sumAll, sumSqAll);

  var meanAll := sumAll / n;
  var parentVar := (sumSqAll / n) - meanAll * meanAll;
  if parentVar < 0 then
    parentVar := 0.0;

  if parentVar < 1e-12 then
    exit(LeafNode(LeafValue(y, indices)));

  var split := FindBestSplitReg(X, y, nodeOrders);

  if not split.Found then
    exit(LeafNode(LeafValue(y, indices)));

  var leftOrders, rightOrders: array of array of integer;
  SplitNodeOrders(nodeOrders, split.Feature, split.LeftCount, split.LeftOrderSize, leftOrders, rightOrders);
  //var leftArr := leftOrders[0];
  //var rightArr := rightOrders[0];
  var rightCount := n - split.LeftCount;

  if (split.LeftCount < fMinSamplesLeaf) or
     (rightCount < fMinSamplesLeaf) then
    exit(LeafNode(LeafValue(y, indices)));

  var delta := parentVar - split.WeightedScore;

  if double.IsNaN(delta) or double.IsInfinity(delta) then
    exit(LeafNode(LeafValue(y, indices)));

  if delta < 0 then
    delta := 0.0;

  if delta <= 0 then
    exit(LeafNode(LeafValue(y, indices)));

  fFeatureImportances[split.Feature] += delta;

  var leftNode := BuildTreeNode(X, y, leftOrders, depth + 1);
  var rightNode := BuildTreeNode(X, y, rightOrders, depth + 1);

  var node := new DecisionTreeNode;
  node.IsLeaf := false;
  node.FeatureIndex := split.Feature;
  node.Threshold := split.Threshold;
  node.Left := leftNode;
  node.Right := rightNode;

  Result := node;
end;

const EPS = 1e-12;

function DecisionTreeRegressorBase.IsPure(y: Vector; indices: array of integer): boolean;
begin
  Result := fCriterion.Impurity(y, indices) < EPS;
end;

function DecisionTreeRegressor.FindBestSplitReg(X: Matrix; y: Vector;
  nodeOrders: array of array of integer): RegSplitResult;
begin
  Result := RegSplitResult.Invalid;

  var indices := nodeOrders[0];
  var n := TotalWeight(indices);
  if n < 2 then
    exit;

  var xData := X.Data;
  var yData := y.Data;

  var sumAll, sumSqAll: real;
  ComputeNodeStats(yData, indices, sumAll, sumSqAll);

  var bestScore := real.PositiveInfinity;
  var bestFeature := -1;
  var bestThreshold := 0.0;
  var bestLeftCount := 0;
  var bestLeftOrderSize := 0;
  var invN := 1.0 / n;
  var minLeaf := fMinSamplesLeaf;
  var maxLeftCount := n - minLeaf;

  var features := GetFeatureSubset(X.ColCount);
  
  for var fj := 0 to features.Length - 1 do
  begin
    var j := features[fj];
    var order := nodeOrders[j];
    var orderLen := order.Length;
    
    if orderLen < 2 then
      continue;

    var firstIdx := order[0];
    var firstWeight := SampleWeight(firstIdx);
    var leftCount := firstWeight;
    var leftSum := firstWeight * yData[firstIdx];
    var leftSumSq := firstWeight * yData[firstIdx] * yData[firstIdx];
    var prevValue := xData[firstIdx, j];

    for var i := 1 to orderLen - 1 do
    begin
      if leftCount > maxLeftCount then
        break;
        
      var idx := order[i];
      var xCur := xData[idx, j];
      
      if (leftCount >= minLeaf) and (prevValue <> xCur) then
      begin
        var rightCount := n - leftCount;
        var rightSum := sumAll - leftSum;
        var rightSumSq := sumSqAll - leftSumSq;
        
        var leftScore := leftSumSq - (leftSum * leftSum) / leftCount;
        var rightScore := rightSumSq - (rightSum * rightSum) / rightCount;
        var weighted := (leftScore + rightScore) * invN;
        
        if weighted < 0 then
          weighted := 0.0;

        if weighted < bestScore then
        begin
          bestScore := weighted;
          bestFeature := j;
          bestThreshold := (prevValue + xCur) * 0.5;
          bestLeftCount := leftCount;
          bestLeftOrderSize := i;
        end;
      end;

      var wCur := SampleWeight(idx);
      var yCur := yData[idx];
      leftCount += wCur;
      leftSum += wCur * yCur;
      leftSumSq += wCur * yCur * yCur;
      prevValue := xCur;
    end;
  end;

  Result.Found := bestFeature <> -1;
  Result.Feature := bestFeature;
  Result.Threshold := bestThreshold;
  Result.LeftCount := bestLeftCount;
  Result.LeftOrderSize := bestLeftOrderSize;
  Result.WeightedScore := bestScore;
end;

type
  SortPair = record
    Value: real;
    Index: integer;
  end;
  SortPairComparer = class(IComparer<SortPair>)
  public
    function Compare(a, b: SortPair): integer;
    begin
      Result := a.Value.CompareTo(b.Value);
    end;
  end;

function BuildPreSortedOrders(X: Matrix): array of array of integer;
begin
  var p := X.ColCount;
  var n := X.RowCount;
  var xData := X.Data;
  
  SetLength(Result, p);
  
  for var j := 0 to p - 1 do
  begin
    var pairs: array of SortPair;
    SetLength(pairs, n);
    
    for var i := 0 to n - 1 do
    begin
      pairs[i].Value := xData[i, j];
      pairs[i].Index := i;
    end;
    
    System.Array.Sort(pairs, new SortPairComparer);
    
    Result[j] := new integer[n];
    for var i := 0 to n - 1 do
      Result[j][i] := pairs[i].Index;
  end;
end;

function BuildRowCounts(rows: array of integer; rowCount: integer): array of integer;
begin
  Result := new integer[rowCount];
  
  for var i := 0 to rows.Length - 1 do
    Result[rows[i]] += 1;
end;

function BuildSortedOrdersFromCounts(
  fullSortedOrders: array of array of integer;
  rowCounts: array of integer
): array of array of integer;
begin
  var p := Length(fullSortedOrders);
  SetLength(Result, p);
  
  var total := 0;
  for var i := 0 to rowCounts.Length - 1 do
    total += rowCounts[i];
  
  for var j := 0 to p - 1 do
  begin
    Result[j] := new integer[total];
    var k := 0;
    
    foreach var idx in fullSortedOrders[j] do
      for var rep := 1 to rowCounts[idx] do
      begin
        Result[j][k] := idx;
        k += 1;
      end;
  end;
end;

function BuildUniqueOrdersFromCounts(
  fullSortedOrders: array of array of integer;
  rowCounts: array of integer
): array of array of integer;
begin
  var p := Length(fullSortedOrders);
  SetLength(Result, p);
  
  var total := 0;
  for var i := 0 to rowCounts.Length - 1 do
    if rowCounts[i] > 0 then
      total += 1;
  
  for var j := 0 to p - 1 do
  begin
    Result[j] := new integer[total];
    var k := 0;
    
    foreach var idx in fullSortedOrders[j] do
      if rowCounts[idx] > 0 then
      begin
        Result[j][k] := idx;
        k += 1;
      end;
  end;
end;

procedure DecisionTreeRegressor.BuildSortedOrders(X: Matrix; indices: array of integer);
begin
  // Обычный путь для отдельного дерева:
  // построить сортировку только по реально используемым строкам.
  var p := X.ColCount;
  var n := indices.Length;
  var xData := X.Data;
  
  SetLength(fSortedOrders, p);
  
  for var j := 0 to p - 1 do
  begin
    var pairs: array of SortPair;
    SetLength(pairs, n);
    
    for var i := 0 to n - 1 do
    begin
      var idx := indices[i];
      pairs[i].Value := xData[idx, j];
      pairs[i].Index := idx;
    end;
    
    System.Array.Sort(pairs,new SortPairComparer);
    
    fSortedOrders[j] := new integer[n];
    
    for var i := 0 to n - 1 do
      fSortedOrders[j][i] := pairs[i].Index;
  end;
end;

function DecisionTreeRegressor.BuildInitialNodeOrders(indices: array of integer): array of array of integer;
begin
  var maxIdx := -1;
  for var i := 0 to indices.Length - 1 do
    if indices[i] > maxIdx then
      maxIdx := indices[i];
  
  var inNode := BuildMembershipMask(maxIdx + 1, indices);
  SetLength(Result, Length(fSortedOrders));
  
  for var j := 0 to Length(fSortedOrders) - 1 do
  begin
    var cnt := 0;
    foreach var idx in fSortedOrders[j] do
      if inNode[idx] then
        cnt += 1;
    
    Result[j] := new integer[cnt];
    var k := 0;
    foreach var idx in fSortedOrders[j] do
      if inNode[idx] then
      begin
        Result[j][k] := idx;
        k += 1;
      end;
  end;
end;

procedure DecisionTreeRegressor.SplitNodeOrders(
  nodeOrders: array of array of integer;
  feature: integer;
  leftCount: integer;
  leftOrderSize: integer;
  var leftOrders, rightOrders: array of array of integer);
begin
  var p := Length(nodeOrders);
  
  fVisitId += 1;
  var mark := fVisitId;
  
  var splitArr := nodeOrders[feature];
  var splitLen := splitArr.Length;
  var rightCount := splitLen - leftOrderSize;
  var markLeft := leftOrderSize <= rightCount;
  
  if markLeft then
    for var i := 0 to leftOrderSize - 1 do
      fVisitMarks[splitArr[i]] := mark
  else
    for var i := leftOrderSize to splitLen - 1 do
      fVisitMarks[splitArr[i]] := mark;

  SetLength(leftOrders, p);
  SetLength(rightOrders, p);

  leftOrders[feature] := new integer[leftOrderSize];
  rightOrders[feature] := new integer[rightCount];
  System.Array.Copy(splitArr, 0, leftOrders[feature], 0, leftOrderSize);
  System.Array.Copy(splitArr, leftOrderSize, rightOrders[feature], 0, rightCount);

  for var j := 0 to p - 1 do
  begin
    if j = feature then
      continue;
    
    var src := nodeOrders[j];
    var n := src.Length;
    
    var left := new integer[leftOrderSize];
    var right := new integer[rightCount];
    
    var li := 0;
    var ri := 0;

    for var k := 0 to n - 1 do
    begin
      var idx := src[k];
      
      if markLeft then
      begin
        if fVisitMarks[idx] = mark then
        begin
          left[li] := idx;
          li += 1;
        end
        else
        begin
          right[ri] := idx;
          ri += 1;
        end;
      end
      else
      begin
        if fVisitMarks[idx] = mark then
        begin
          right[ri] := idx;
          ri += 1;
        end
        else
        begin
          left[li] := idx;
          li += 1;
        end;
      end;
    end;

    leftOrders[j] := left;
    rightOrders[j] := right;
  end;
end;

function DecisionTreeRegressor.BuildMembershipMask(rowCount: integer; indices: array of integer): array of boolean;
begin
  Result := new boolean[rowCount];
  for var i := 0 to indices.Length - 1 do
    Result[indices[i]] := true;
end;

procedure DecisionTreeRegressor.ComputeNodeStats(yData: array of real; indices: array of integer; var sumAll, sumSqAll: real);
begin
  sumAll := 0.0;
  sumSqAll := 0.0;

  for var i := 0 to indices.Length - 1 do
  begin
    var idx := indices[i];
    var w := SampleWeight(idx);
    var v := yData[idx];
    sumAll += w * v;
    sumSqAll += w * v * v;
  end;
end;

function DecisionTreeRegressor.WeightedVariance(n, leftCount: integer; leftSum, leftSumSq, sumAll, sumSqAll: real): real;
begin
  var rightCount := n - leftCount;
  var rightSum := sumAll - leftSum;
  var rightSumSq := sumSqAll - leftSumSq;

  var leftMean := leftSum / leftCount;
  var rightMean := rightSum / rightCount;

  var leftVar := (leftSumSq / leftCount) - leftMean * leftMean;
  var rightVar := (rightSumSq / rightCount) - rightMean * rightMean;

  if leftVar < 0 then leftVar := 0.0;
  if rightVar < 0 then rightVar := 0.0;

  Result :=
    (real(leftCount) / n) * leftVar +
    (real(rightCount) / n) * rightVar;
end;

function DecisionTreeRegressor.GetFeatureSubset(p: integer): array of integer;
begin
  if (fMaxFeatures <= 0) or (fMaxFeatures >= p) then
  begin
    Result := Arr(0..p - 1);
    exit;
  end;

  Result := new integer[fMaxFeatures];
  var feat := new integer[p];
  for var i := 0 to p - 1 do
    feat[i] := i;

  for var i := 0 to fMaxFeatures - 1 do
  begin
    var r := i + fRng.Next(p - i);
    var tmp := feat[i];
    feat[i] := feat[r];
    feat[r] := tmp;
    Result[i] := feat[i];
  end;
end;

procedure DecisionTreeRegressor.SetPreSortedOrders(sortedOrders: array of array of integer);
begin
  // Внутренний fast-path для ансамблей:
  // используем готовую полную сортировку X и затем фильтруем её по fRowIndices.
  fSortedOrders := sortedOrders;
  fRowWeights := nil;
  fUseSortedOrdersAsRoot := false;
end;

procedure DecisionTreeRegressor.SetPreSortedRootOrders(sortedOrders: array of array of integer);
begin
  // Внутренний fast-path для bootstrap-выборок с повторами:
  // сортировка уже соответствует корневому узлу текущего дерева.
  fSortedOrders := sortedOrders;
  fRowWeights := nil;
  fUseSortedOrdersAsRoot := true;
end;

procedure DecisionTreeRegressor.SetBootstrapRootOrders(
  sortedOrders: array of array of integer;
  rowWeights: array of integer);
begin
  fSortedOrders := sortedOrders;
  fRowWeights := rowWeights;
  fUseSortedOrdersAsRoot := true;
end;

function DecisionTreeRegressor.SampleWeight(rowIndex: integer): integer;
begin
  if fRowWeights = nil then
    Result := 1
  else
    Result := fRowWeights[rowIndex];
end;

function DecisionTreeRegressor.TotalWeight(indices: array of integer): integer;
begin
  Result := 0;
  for var i := 0 to indices.Length - 1 do
    Result += SampleWeight(indices[i]);
end;

//==============================
//    DecisionTreeClassifier
//==============================

constructor DecisionTreeClassifier.Create(maxDepth: integer; 
  minSamplesSplit: integer; minSamplesLeaf: integer; 
  criterion: ISplitCriterion; 
  maxFeatures: integer;
  seed: integer);
begin
  if maxDepth = 0 then
    ArgumentOutOfRangeError(ER_MAX_DEPTH_INVALID, maxDepth);

  if minSamplesSplit < 2 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_SPLIT_INVALID, minSamplesSplit);

  if minSamplesLeaf < 1 then
    ArgumentOutOfRangeError(ER_MIN_SAMPLES_LEAF_INVALID, minSamplesLeaf);

  if minSamplesSplit < 2 * minSamplesLeaf then
    ArgumentOutOfRangeError(ER_MIN_LEAF_GT_SPLIT, minSamplesLeaf, minSamplesSplit);
  
  if maxFeatures < 0 then
    ArgumentOutOfRangeError(ER_MAX_FEATURES_INVALID);

  fMaxFeatures := maxFeatures;

  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  
  fCriterion := criterion;

  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
  fRng := new System.Random(fRandomSeed);
end;

function DecisionTreeClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if MLConfig.ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if X.RowCount <> y.Length then
    DimensionError(ER_DIM_MISMATCH, X.RowCount, y.Length);

  fFeatureImportances := new Vector(X.ColCount);

  // --- convert to integer labels
  var yInt := LabelsToInts(y);

  // --- encode
  var classes: array of integer;
  var yEncArr := EncodeLabelsInt(yInt, classes);
  
  if classes.Length < 2 then
    ArgumentError(ER_NEED_AT_LEAST_TWO_CLASSES);

  // --- criterion
  if fCriterion = nil then
    fCriterion := new GiniCriterion(classes.Length);

  fIndexToClass := classes;
  SetLength(fClassLabels, fIndexToClass.Length);
  for var i := 0 to fIndexToClass.Length - 1 do
    fClassLabels[i] := fIndexToClass[i].ToString;

  // --- encoded vector
  var yEncoded := new Vector(yEncArr);

  // --- Core
  fCore := new DecisionTreeCore(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fCriterion,
    classes.Length,
    fMaxFeatures,
    fRandomSeed
  );

  fCore.Fit(X, yEncoded);

  fFitted := true;
  Result := Self;
end;

function DecisionTreeClassifier.Predict(X: Matrix): Vector;
begin
  var labels := PredictLabels(X);
  
  Result := new Vector(labels.Length);
  
  for var i := 0 to labels.Length - 1 do
    Result[i] := fIndexToClass[labels[i]];
end;

function DecisionTreeClassifier.PredictLabels(X: Matrix): array of integer;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureImportances.Length then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureImportances.Length);
  
  var predIdx := fCore.Predict(X);
  
  Result := new integer[predIdx.Length];
  
  for var i := 0 to predIdx.Length - 1 do
    Result[i] := Round(predIdx[i]);
end;

function DecisionTreeClassifier.Clone: IModel;
begin
  Result := new DecisionTreeClassifier(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fCriterion,
    fMaxFeatures,
    fRandomSeed
  );
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

  Result := Copy(fClassLabels);
end;

// DecisionTreeRegressor

constructor DecisionTreeRegressor.Create(
  maxDepth: integer; 
  minSamplesSplit: integer; 
  minSamplesLeaf: integer; 
  leafL2: real; 
  seed: integer
);
begin
  inherited Create(
    maxDepth,
    minSamplesSplit,
    minSamplesLeaf,
    new VarianceCriterion,
    seed
  );

  if leafL2 < 0 then
    ArgumentOutOfRangeError(ER_LEAFL2_INVALID, leafL2);

  fLeafL2 := leafL2;
end;

function DecisionTreeRegressor.LeafValue(y: Vector; indices: array of integer): real;
begin
  var n := TotalWeight(indices);

  if n = 0 then
    exit(0.0);  // безопасный fallback, не должен происходить

  if fLeafL2 < 0 then
    ArgumentOutOfRangeError(ER_LEAFL2_INVALID, fLeafL2);

  var sum := 0.0;

  foreach var idx in indices do
    sum += SampleWeight(idx) * y[idx];

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

  if MLConfig.ValidateFiniteInputs then
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

  var indices: array of integer := fRowIndices;
  if indices = nil then
    indices := Arr(0..X.RowCount - 1);

  if (fSortedOrders = nil) or (Length(fSortedOrders) = 0) then
    BuildSortedOrders(X, indices);
  
  SetLength(fVisitMarks, X.RowCount);
  fVisitId := 0;

  if fUseSortedOrdersAsRoot then
    fRoot := BuildTreeNode(X, y, fSortedOrders, 0)
  else
    fRoot := BuildTreeNew(X, y, indices, 0);
  
  var s := fFeatureImportances.Sum;
  if s > 0 then
    for var i := 0 to fFeatureImportances.Length - 1 do
      fFeatureImportances[i] /= s;
  
  fFitted := true;
  
  fRowIndices := nil;
  fSortedOrders := nil;
  fUseSortedOrdersAsRoot := false;
  fRowWeights := nil;
  
  fVisitMarks := nil;

  Result := Self;
end;

function DecisionTreeRegressor.PredictOne(X: Matrix; rowIndex: integer): real;
begin
  var node := fRoot;

  while not node.IsLeaf do
    if X[rowIndex, node.FeatureIndex] <= node.Threshold then
      node := node.Left
    else
      node := node.Right;

  Result := node.LeafValue;  // для регрессии это уже real
end;

function DecisionTreeRegressor.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureImportances.Length then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureImportances.Length);

  var n := X.RowCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
    Result[i] := PredictOne(X, i);
end;

function DecisionTreeRegressor.Clone: IModel;
begin
  Result := new DecisionTreeRegressor(
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fLeafL2,
    fRandomSeed
  );
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
  
  if minSamplesSplit < 2 * minSamplesLeaf then
    ArgumentOutOfRangeError(ER_MIN_LEAF_GT_SPLIT, minSamplesLeaf, minSamplesSplit);
    
  fNTrees := nTrees;
  fMaxDepth := maxDepth;
  fMinSamplesSplit := minSamplesSplit;
  fMinSamplesLeaf := minSamplesLeaf;
  fMaxFeaturesMode := maxFeatures;
  fFitted := false;
  
  fFeatureCount := 0;
  
  fUseOOB := useOOB;
  fOOBScore := real.NaN;
  
  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
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

  if MLConfig.ValidateFiniteInputs then
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
  var fullSortedOrders := BuildPreSortedOrders(X);

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
    
    var rowCounts := BuildRowCounts(rows, n);
    var bootSortedOrders := BuildUniqueOrdersFromCounts(fullSortedOrders, rowCounts);
    tree.SetBootstrapRootOrders(bootSortedOrders, rowCounts);
    
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

  if MLConfig.ValidateFiniteInputs then
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
  Result := new RandomForestRegressor(
    fNTrees,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fMaxFeaturesMode,
    fUseOOB,
    fRandomSeed
  );
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

  if MLConfig.ValidateFiniteInputs then
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

  // --- reset
  fClassCount := 0;
  fIndexToClass := nil;

  // =========================================================
  // ЕДИНЫЙ ENCODING (forest-level)
  // =========================================================

  var yInt := LabelsToInts(y);

  var yEncArr := EncodeLabelsInt(yInt, fIndexToClass);
  fClassCount := fIndexToClass.Length;
  
  SetLength(fClassLabels, fIndexToClass.Length);
  for var i := 0 to fIndexToClass.Length - 1 do
    fClassLabels[i] := fIndexToClass[i].ToString;

  if fClassCount < 2 then
    ArgumentError(ER_NEED_AT_LEAST_TWO_CLASSES);

  var yEnc := new Vector(yEncArr);

  // =========================================================

  // --- OOB buffers
  var oobVotes: array[,] of integer := nil;
  var oobCnt: array of integer := nil;

  if fUseOOB then
  begin
    oobVotes := new integer[n, fClassCount];
    oobCnt := new integer[n];
  end;

  // --- training
  for var t := 0 to fNTrees - 1 do
  begin
    var treeSeed := fRng.Next(integer.MaxValue);

    var rows: array of integer;
    BootstrapRowIndices(n, rows);

    var XBoot := X.TakeRows(rows);
    var yBoot := yEnc.SubvectorBy(rows);

    var treeCriterion := new GiniCriterion(fClassCount);

    var tree := new DecisionTreeCore(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      treeCriterion,
      fClassCount,
      ComputeMaxFeatures(p),
      treeSeed
    );

    tree.Fit(XBoot, yBoot);

    // --- OOB accumulate
    if fUseOOB then
    begin
      var inBag := new boolean[n];
      for var i := 0 to rows.Length - 1 do
        inBag[rows[i]] := true;

      for var i := 0 to n - 1 do
        if not inBag[i] then
        begin
          var cls := tree.PredictRow(X, i); // encoded class 0..K-1
          oobVotes[i, cls] += 1;
          oobCnt[i] += 1;
        end;
    end;

    fTrees[t] := tree;
  end;

  // --- finalize OOB score
  fHasOOBScore := false;
  fOOBScore := real.NaN;

  if fUseOOB then
  begin
    var correct := 0;
    var cnt := 0;

    for var i := 0 to n - 1 do
      if oobCnt[i] > 0 then
      begin
        var bestK := 0;
        var bestV := oobVotes[i, 0];

        for var k := 1 to fClassCount - 1 do
          if oobVotes[i, k] > bestV then
          begin
            bestV := oobVotes[i, k];
            bestK := k;
          end;

        if bestK = yEncArr[i] then
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
  var labels := PredictLabels(X);
  
  Result := new Vector(labels.Length);
  
  for var i := 0 to labels.Length - 1 do
    Result[i] := fIndexToClass[labels[i]];
end;

function RandomForestClassifier.PredictLabels(X: Matrix): array of integer;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);
  
  var n := X.RowCount;
  var treeCount := fTrees.Length;

  if treeCount = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  if fClassCount <= 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  Result := new integer[n];
  var counts := new integer[fClassCount];
  
  for var i := 0 to n - 1 do
  begin
    for var c := 0 to fClassCount - 1 do
      counts[c] := 0;

    for var t := 0 to treeCount - 1 do
    begin
      var cls := fTrees[t].PredictRow(X, i);

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

    Result[i] := bestClass;
  end;
end;

function RandomForestClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH, X.ColCount, fFeatureCount);

  var n := X.RowCount;
  var treeCount := fTrees.Length;

  if treeCount = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  if fClassCount <= 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  Result := new Matrix(n, fClassCount);
  var counts := new integer[fClassCount];

  for var i := 0 to n - 1 do
  begin
    // reset
    for var c := 0 to fClassCount - 1 do
      counts[c] := 0;

    // voting
    for var t := 0 to treeCount - 1 do
    begin
      var cls := fTrees[t].PredictRow(X, i);  // <-- главное изменение

      if (cls < 0) or (cls >= fClassCount) then
        ArgumentError(ER_LABEL_INDEX_INVALID);

      counts[cls] += 1;
    end;

    // normalize
    for var c := 0 to fClassCount - 1 do
      Result[i, c] := counts[c] / treeCount;
  end;
end;

function RandomForestClassifier.Clone: IModel;
begin
  Result := new RandomForestClassifier(
    fNTrees,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fMaxFeaturesMode,
    fUseOOB,
    fRandomSeed
  );
end;

function RandomForestClassifier.FeatureImportances: Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);
  
  if fTrees.Length = 0 then
    Error(ER_MODEL_NOT_INITIALIZED);

  var p := fTrees[0].FeatureImportances.Length;
  var resultVec := new Vector(p);

  for var t := 0 to fTrees.Length - 1 do
    resultVec += fTrees[t].FeatureImportances;

  resultVec *= 1.0 / fTrees.Length;

  Result := resultVec.Normalized;
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

  Result := Copy(fClassLabels);
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
  
  if minSamplesSplit < 2 * minSamplesLeaf then
    ArgumentOutOfRangeError(ER_MIN_LEAF_GT_SPLIT, minSamplesLeaf, minSamplesSplit);
  
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
  
  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
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
  XVal: Matrix; yVal: Vector): ISupervisedModel;
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
  if MLConfig.ValidateFiniteInputs then
  begin
    CheckXForFit(XTrain);
    CheckYForFit(yTrain);
  end;

  // --- shape checks ---
  if XTrain.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if XTrain.RowCount <> yTrain.Length then
    DimensionError(ER_XY_SIZE_MISMATCH,XTrain.RowCount,yTrain.Length);

  if useValidation then
  begin
    if XVal = nil then
      ArgumentNullError(ER_X_NULL);

    if yVal = nil then
      ArgumentNullError(ER_Y_NULL);

    if MLConfig.ValidateFiniteInputs then
    begin
      CheckXForPredict(XVal);
      CheckYForFit(yVal);
    end;

    if XVal.RowCount <> yVal.Length then
      DimensionError(ER_XY_SIZE_MISMATCH,XVal.RowCount,yVal.Length);

    if XVal.ColCount <> XTrain.ColCount then
      DimensionError(ER_FEATURE_COUNT_MISMATCH,XVal.ColCount,XTrain.ColCount);
  end;
  
  // --- OOB checks ---
  if fUseOOBEarlyStopping and (fSubsample >= 1.0) then
    ArgumentError(ER_OOB_REQUIRES_SUBSAMPLE);

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
  
  var residuals := new Vector(nTrain);
  var preSortedOrders := BuildPreSortedOrders(XTrain);

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
    ComputePseudoResiduals(yTrain, yPredTrain, residuals);

    var stageSeed := fRng.Next(integer.MaxValue);

    var tree := new DecisionTreeRegressor(
      fMaxDepth,
      fMinSamplesSplit,
      fMinSamplesLeaf,
      fLeafL2,
      stageSeed
    );
    tree.SetPreSortedOrders(preSortedOrders);

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
    
    tree.Fit(XTrain, residuals);
    fEstimators.Add(tree);

    var deltaTrain := tree.Predict(XTrain);

    // --- update TRAIN ---
    for var i := 0 to nTrain - 1 do
      yPredTrain[i] += fLearningRate * deltaTrain[i];

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

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

  var n := X.RowCount;
  var yPred := new Vector(n);

  for var i := 0 to n - 1 do
    yPred[i] := fInitValue;

  foreach var tree in fEstimators do
    for var i := 0 to n - 1 do
      yPred[i] += fLearningRate * tree.PredictOne(X, i);

  Result := yPred;
end;

function GradientBoostingRegressor.PredictStage(X: Matrix; m: integer): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
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

  if MLConfig.ValidateFiniteInputs then
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
  Result := new GradientBoostingRegressor(
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
  if MLConfig.ValidateFiniteInputs then
  begin
    CheckXForFit(XTrain);
    CheckYForFit(yTrain);
  end;

  // --- shape checks ---
  if XTrain.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if XTrain.RowCount <> yTrain.Length then
    DimensionError(ER_XY_SIZE_MISMATCH,XTrain.RowCount,yTrain.Length);

  if useValidation then
  begin
    if XVal = nil then
      ArgumentNullError(ER_X_NULL);

    if yVal = nil then
      ArgumentNullError(ER_Y_NULL);

    if MLConfig.ValidateFiniteInputs then
    begin
      CheckXForPredict(XVal);
      CheckYForFit(yVal);
    end;

    if XVal.RowCount <> yVal.Length then
      DimensionError(ER_XY_SIZE_MISMATCH,XVal.RowCount,yVal.Length);

    if XVal.ColCount <> XTrain.ColCount then
      DimensionError(ER_FEATURE_COUNT_MISMATCH,XVal.ColCount,XTrain.ColCount);
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

  var nTrain := XTrain.RowCount;

  // =========================================================
  // Новый Encoding
  // =========================================================

  var yTrainInt := LabelsToInts(yTrain);

  var yEncoded := EncodeLabelsInt(yTrainInt, fClasses);

  fClassCount := fClasses.Length;
  
  SetLength(fClassLabels, fClasses.Length);
  for var i := 0 to fClasses.Length - 1 do
    fClassLabels[i] := fClasses[i].ToString;

  if fClassCount < 2 then
    ArgumentError(ER_NEED_AT_LEAST_TWO_CLASSES);

  fClassIndex := new Dictionary<integer, integer>;
  for var cls := 0 to fClassCount - 1 do
    fClassIndex[fClasses[cls]] := cls;

  var classCount := fClassCount;

  // =========================================================

  // --- compute class priors
  fInitLogits := new real[classCount];

  var counts := new integer[classCount];

  for var i := 0 to nTrain - 1 do
    counts[yEncoded[i]] += 1;

  for var cls := 0 to classCount - 1 do
  begin
    var pi := counts[cls] / nTrain;

    if pi <= 0 then
      fInitLogits[cls] := -20.0
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
    var yValInt := LabelsToInts(yVal);

    yValEncoded := new integer[yValInt.Length];

    for var i := 0 to yValInt.Length - 1 do
    begin
      var ir := yValInt[i];

      if ir not in fClassIndex then
        ArgumentError(ER_UNKNOWN_CLASS_LABEL, ir);

      yValEncoded[i] := fClassIndex[ir];
    end;

    logitsVal := new Matrix(XVal.RowCount, classCount);

    for var i := 0 to XVal.RowCount - 1 do
      for var cls := 0 to classCount - 1 do
        logitsVal[i, cls] := fInitLogits[cls];
  end;

  var noImprove := 0;

  // --- boosting loop
  for var iter := 0 to fNEstimators - 1 do
  begin
    var probsTrain := new Matrix(nTrain, classCount);
    SoftmaxMatrix(logitsTrain, probsTrain);

    var residuals := new Matrix(nTrain, classCount);

    for var i := 0 to nTrain - 1 do
      for var cls := 0 to classCount - 1 do
      begin
        var yik := 0.0;
        if yEncoded[i] = cls then
          yik := 1.0;

        residuals[i, cls] := yik - probsTrain[i, cls];
      end;

    var useSubsample := fSubsample < 1.0;
    var subIndices: array of integer := nil;
    var inBag: array of boolean := nil;

    if useSubsample then
      subIndices := BuildSubsampleIndices(nTrain, fSubsample, fRng);

    if useOOB then
    begin
      SetLength(inBag, nTrain);

      if useSubsample then
        for var i := 0 to subIndices.Length - 1 do
          inBag[subIndices[i]] := true
      else
        for var i := 0 to nTrain - 1 do
          inBag[i] := true;
    end;

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

      if useSubsample then
        tree.SetRowIndices(subIndices);

      tree.Fit(XTrain, rvec);
      trees[cls] := tree;

      var deltaTrain := tree.Predict(XTrain);

      for var i := 0 to nTrain - 1 do
        logitsTrain[i, cls] += fLearningRate * deltaTrain[i];

      if useOOB then
      begin
        for var i := 0 to nTrain - 1 do
          if not inBag[i] then
            logitsOOB[i, cls] += fLearningRate * deltaTrain[i];
      end;

      if useValidation then
      begin
        var deltaVal := tree.Predict(XVal);
        for var i := 0 to XVal.RowCount - 1 do
          logitsVal[i, cls] += fLearningRate * deltaVal[i];
      end;
    end;
    
    if useOOB then
    begin
      for var i := 0 to nTrain - 1 do
        if not inBag[i] then
          oobCount[i] += 1;
    end;

    fEstimators.Add(trees);

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
  
  if minSamplesSplit < 2 * minSamplesLeaf then
    ArgumentOutOfRangeError(ER_MIN_LEAF_GT_SPLIT, minSamplesLeaf, minSamplesSplit);
  
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
  
  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
  fRng := new System.Random(fRandomSeed);
end;

function GradientBoostingClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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
      for var i := 0 to nSamples - 1 do
        logits[i, cls] += fLearningRate * trees[cls].PredictOne(X, i);

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

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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
    exit(fFeatureImportances.Clone);

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

  Result := fFeatureImportances.Clone;
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
  Result := new GradientBoostingClassifier(
    fNEstimators,
    fLearningRate,
    fMaxDepth,
    fMinSamplesSplit,
    fMinSamplesLeaf,
    fSubsample,
    fEarlyStoppingPatience,
    fRandomSeed
  );
end;

function GradientBoostingClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  Result := FitInternal(X, y, nil, nil, false);
end;

function GradientBoostingClassifier.FitWithValidation(
  XTrain: Matrix; yTrain: Vector;
  XVal: Matrix; yVal: Vector): ISupervisedModel;
begin
  Result := FitInternal(XTrain, yTrain, XVal, yVal, true);
end;

function GradientBoostingClassifier.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var labels := PredictLabels(X);
  
  Result := new Vector(labels.Length);
  
  for var i := 0 to labels.Length - 1 do
    Result[i] := fClasses[labels[i]];
end;

function GradientBoostingClassifier.PredictLabels(X: Matrix): array of integer;
begin
  var probs := PredictProba(X);
  
  SetLength(Result, probs.RowCount);
  
  for var i := 0 to probs.RowCount - 1 do
    Result[i] := probs.RowArgMax(i);
end;

procedure GradientBoostingClassifier.SetClassLabels(classes: array of string);
begin
  fClassLabels := Copy(classes);
end;

function GradientBoostingClassifier.GetClassLabels: array of string;
begin
  if fClassLabels = nil then
    ArgumentError(ER_CLASSES_NOT_AVAILABLE);

  Result := Copy(fClassLabels);
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

  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

  if X.ColCount <> fXTrain.ColCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fXTrain.ColCount);
end;

function KNNBase.SquaredL2(trainRow: integer; XTest: Matrix; testRow: integer): double;
begin
  var sum := 0.0; 
  var d := fXTrain.ColCount;

  var train := fXTrain;   // локальная ссылка
  var test  := XTest;

  for var j := 0 to d - 1 do
  begin
    var diff := train.Data[trainRow, j] - test.Data[testRow, j];
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
//        topK
//-----------------------------

type TopK = class
private
  fDist: array of real;
  fIdx: array of integer;
  fCount: integer;
  fK: integer;
  fWorstIdx: integer;

  procedure UpdateWorst;

public
  constructor Create(k: integer);
  procedure Clear;

  function Count: integer;
  function Worst: real;

  procedure Add(d: real; id: integer);

  function GetIndex(i: integer): integer;
  function GetDist(i: integer): real;
end;

constructor TopK.Create(k: integer);
begin
  fK := k;
  SetLength(fDist, k);
  SetLength(fIdx, k);
  fCount := 0;
  fWorstIdx := 0;
end;

procedure TopK.Clear;
begin
  fCount := 0;
  fWorstIdx := 0;
end;

function TopK.Count: integer;
begin
  Result := fCount;
end;

function TopK.Worst: real;
begin
  if fCount < fK then
    Result := real.PositiveInfinity
  else
    Result := fDist[fWorstIdx];
end;

procedure TopK.UpdateWorst;
begin
  var wi := 0;
  var wd := fDist[0];

  for var i := 1 to fCount - 1 do
    if fDist[i] > wd then
    begin
      wd := fDist[i];
      wi := i;
    end;

  fWorstIdx := wi;
end;

procedure TopK.Add(d: real; id: integer);
begin
  // ещё не заполнено
  if fCount < fK then
  begin
    fDist[fCount] := d;
    fIdx[fCount] := id;
    fCount += 1;

    if fCount = fK then
      UpdateWorst;

    exit;
  end;

  // быстрый отсев
  if d >= fDist[fWorstIdx] then
    exit;

  // замена худшего
  fDist[fWorstIdx] := d;
  fIdx[fWorstIdx] := id;

  UpdateWorst;
end;

function TopK.GetIndex(i: integer): integer;
begin
  Result := fIdx[i];
end;

function TopK.GetDist(i: integer): real;
begin
  Result := fDist[i];
end;

//-----------------------------
//        KNNClassifier 
//-----------------------------

const KNN_EPS = 1e-12;

constructor KNNClassifier.Create(k: integer; weighting: KNNWeighting);
begin
  inherited Create(k, weighting);
end;

function KNNClassifier.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH,X.RowCount,y.Length);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fK > X.RowCount then
    ArgumentOutOfRangeError(ER_K_EXCEEDS_SAMPLES);

  if MLConfig.ValidateFiniteInputs then
  begin
    CheckXForFit(X);
    CheckYForFit(y);
  end;

  var n := X.RowCount;

  // --- copy train data
  fXTrain := X.Clone;
  
  // =========================================================
  // ЕДИНЫЙ ENCODING
  // =========================================================

  var yInt := LabelsToInts(y);

  var classesInt: array of integer;
  var yEncArr := EncodeLabelsInt(yInt, classesInt);

  fClassCount := classesInt.Length;

  if fClassCount < 2 then
    ArgumentError(ER_NEED_AT_LEAST_TWO_CLASSES);

  // сохранить оригинальные метки (double API)
  SetLength(fClasses, fClassCount);
  for var i := 0 to fClassCount - 1 do
    fClasses[i] := classesInt[i];
  
  SetLength(fClassLabels, fClassCount);
  for var i := 0 to fClassCount - 1 do
    fClassLabels[i] := fClasses[i].ToString;

  // сохранить encoded y
  SetLength(fYEnc, n);
  for var i := 0 to n - 1 do
    fYEnc[i] := yEncArr[i];

  // =========================================================

  // --- buffers
  SetLength(fNeighbors, n);

  SetLength(fVotes, fClassCount);
  SetLength(fMark, fClassCount);
  SetLength(fTouched, fClassCount);

  fEpoch := 0;

  fFitted := true;

  Result := Self;
end;

function KNNClassifier.GetClasses: array of real;
begin
  Result := fClasses;
end;

function KNNClassifier.Clone: IModel;
begin
  Result := new KNNClassifier(fK, fWeighting);
end;

function KNNClassifier.Predict(X: Matrix): Vector;
begin
  var labels := PredictLabels(X);
  
  Result := new Vector(labels.Length);
  
  for var i := 0 to labels.Length - 1 do
    Result[i] := fClasses[labels[i]];
end;

function KNNClassifier.PredictLabels(X: Matrix): array of integer;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  ValidatePredictInput(X);

  var m := X.RowCount;
  var n := fXTrain.RowCount;
  var p := fXTrain.ColCount;
  
  var trainRows := fXTrain.Data.Rows;
  var testRows  := X.Data.Rows;

  Result := new integer[m];

  for var i := 0 to m - 1 do
  begin
    var rowTest := testRows[i];
    for var t := 0 to n - 1 do
    begin      
      var rowTrain := trainRows[t];
      
      var sum := 0.0;
    
      for var j := 0 to p - 1 do
      begin
        var diff := rowTrain[j] - rowTest[j];
        sum += diff * diff;
      end;
    
      fNeighbors[t].dist := sum;
      fNeighbors[t].idx := t;
    end;

    QuickSelect(fK - 1);

    var exactCls := -1;
    for var t := 0 to fK - 1 do
      if fNeighbors[t].dist < KNN_EPS then
      begin
        exactCls := fYEnc[fNeighbors[t].idx];
        break;
      end;

    if exactCls <> -1 then
    begin
      Result[i] := exactCls;
      continue;
    end;

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
      Result[i] := exactCls;
      continue;
    end;

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

    Result[i] := bestCls;
  end;
end;

function KNNClassifier.PredictProba(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  ValidatePredictInput(X);

  var m := X.RowCount;
  var n := fXTrain.RowCount;
  var p := fXTrain.ColCount;
  
  var trainRows := fXTrain.Data.Rows;
  var testRows := X.Data.Rows;

  Result := new Matrix(m, fClassCount); // предполагаем нулевую инициализацию

  for var i := 0 to m - 1 do
  begin
    // заполнить расстояния
    var rowTest := testRows[i];
    for var t := 0 to n - 1 do
    begin
      var rowTrain := trainRows[t];
      var sum := 0.0;
      
      for var j := 0 to p - 1 do
      begin
        var diff := rowTrain[j] - rowTest[j];
        sum += diff * diff;
      end;
      
      fNeighbors[t].dist := sum;
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
    
      // ОБЯЗАТЕЛЬНО проверить exact снова
    
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
        var uniform := 1.0 / touchCount;
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

  Result := Copy(fClassLabels);
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
    DimensionError(ER_XY_SIZE_MISMATCH,X.RowCount,y.Length);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  if fK > X.RowCount then
    ArgumentOutOfRangeError(ER_K_EXCEEDS_SAMPLES);

  if MLConfig.ValidateFiniteInputs then
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
  Result := new KNNRegressor(fK, fWeighting);
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
  fRandomSeed := ResolveRandomSeed(seed, fUserProvidedSeed);
  fRng := new System.Random(fRandomSeed);

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

  // --- 1. K-Means++ инициализация

  var centers := new Matrix(k, p);
  
  // первый центр — случайный
  var first := rnd.Next(n);
  for var j := 0 to p - 1 do
    centers[0,j] := X[first,j];
  
  // расстояния до ближайшего центра
  var dist1 := new real[n];
  
  for var i := 0 to n - 1 do
    dist1[i] := double.MaxValue;
  
  // выбираем остальные центры
  for var c := 1 to k - 1 do
  begin
    // обновляем dist (минимальное расстояние до уже выбранных центров)
    for var i := 0 to n - 1 do
    begin
      var d := 0.0;
  
      for var j := 0 to p - 1 do
      begin
        var diff := X[i,j] - centers[c-1,j];
        d += diff * diff;
      end;
  
      if d < dist1[i] then
        dist1[i] := d;
    end;
  
    // сумма расстояний
    var sumDist := 0.0;
    for var i := 0 to n - 1 do
      sumDist += dist1[i];
  
    // если всё совпало (редкий случай)
    if sumDist = 0 then
    begin
      var r := rnd.Next(n);
      for var j := 0 to p - 1 do
        centers[c,j] := X[r,j];
      continue;
    end;
  
    // выбор по вероятности
    var target := rnd.NextDouble * sumDist;
    var acc := 0.0;
    var chosen := n - 1;
  
    for var i := 0 to n - 1 do
    begin
      acc += dist1[i];
      if acc >= target then
      begin
        chosen := i;
        break;
      end;
    end;
  
    for var j := 0 to p - 1 do
      centers[c,j] := X[chosen,j];
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
  
  if MLConfig.ValidateFiniteInputs then
    CheckXForFit(X);

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
  
  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);
  
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
  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);
  
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
  Result := new KMeans(
    fNClusters,
    fMaxIter,
    fTol,
    fNInit,
    fRandomSeed
  );
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

constructor DBSCAN.Create(eps: real; minSamples: integer);
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
  
  if MLConfig.ValidateFiniteInputs then
    CheckXForFit(X);

  if X.RowCount = 0 then
    ArgumentError(ER_EMPTY_DATASET);

  var n := X.RowCount;
  var p := X.ColCount;

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
  
  if MLConfig.ValidateFiniteInputs then
    CheckXForPredict(X);

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
  Result := new DBSCAN(fEps, fMinSamples);
end;

function DBSCAN.ClustersCount: integer := fClusterCount;

//-----------------------------
//          MatrixPipeline 
//-----------------------------

constructor MatrixPipeline.Create;
begin
  fTransformers := new List<ITransformer>;
  fModel := nil;
  fFitted := false;
end;

constructor MatrixPipeline.Create(model: ISupervisedModel);
begin
  Create;
  if model = nil then
    ArgumentError(ER_MODEL_NULL);
  fModel := model;
end;

class function MatrixPipeline.Build(params steps: array of IPipelineStep): MatrixPipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  // последний шаг
  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is ISupervisedModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_SUPERVISED_MODEL);

  var pipe := new MatrixPipeline(last as ISupervisedModel);

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

function MatrixPipeline.Add(t: ITransformer): MatrixPipeline;
begin
  if t = nil then
    ArgumentError(ER_TRANSFORMER_NULL);

  fTransformers.Add(t);
  Result := Self;
end;

function MatrixPipeline.SetModel(m: ISupervisedModel): MatrixPipeline;
begin
  if m = nil then
    ArgumentError(ER_MODEL_NULL);

  fModel := m;
  Result := Self;
end;

function MatrixPipeline.Fit(X: Matrix; y: Vector): ISupervisedModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if y = nil then
    ArgumentNullError(ER_Y_NULL);

  if X.RowCount <> y.Length then
    DimensionError(ER_XY_SIZE_MISMATCH,X.RowCount,y.Length);

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

function MatrixPipeline.Transform(X: Matrix): Matrix;
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

function MatrixPipeline.Predict(X: Matrix): Vector;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var Xt := Transform(X);
  Result := fModel.Predict(Xt);
end;

function MatrixPipeline.PredictProba(X: Matrix): Matrix;
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

function MatrixPipeline.ToString: string;
begin
  var sb := 'MatrixPipeline (' +
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

function MatrixPipeline.Clone: IModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var p := new MatrixPipeline;

  foreach var t in fTransformers do
    p.Add(t.Clone);

  var m := fModel.Clone;

  if not (m is ISupervisedModel) then
    Error(ER_INTERNAL_INVALID_MODEL_CLONE);

  p.SetModel(m as ISupervisedModel);

  Result := p;
end;

//-----------------------------
//          UMatrixPipeline 
//-----------------------------
constructor UMatrixPipeline.Create;
begin
  fTransformers := new List<ITransformer>;
  fModel := nil;
  fFitted := false;
end;

constructor UMatrixPipeline.Create(model: IModel);
begin
  Create;
  if model = nil then
    ArgumentError(ER_MODEL_NULL);
  fModel := model;
end;

class function UMatrixPipeline.Build(params steps: array of IPipelineStep): UMatrixPipeline;
begin
  if (steps = nil) or (Length(steps) = 0) then
    ArgumentError(ER_PIPELINE_NO_STEPS);

  var last := steps[High(steps)];

  if last = nil then
    ArgumentError(ER_PIPELINE_STEP_NULL, High(steps));

  if not (last is IModel) then
    ArgumentError(ER_PIPELINE_LAST_NOT_MODEL);

  var pipe := new UMatrixPipeline(last as IModel);

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

function UMatrixPipeline.Add(t: ITransformer): UMatrixPipeline;
begin
  if t = nil then
    ArgumentError(ER_TRANSFORMER_NULL);

  fTransformers.Add(t);
  Result := Self;
end;

function UMatrixPipeline.SetModel(m: IModel): UMatrixPipeline;
begin
  if m = nil then
    ArgumentError(ER_MODEL_NULL);

  fModel := m;
  Result := Self;
end;

function UMatrixPipeline.Fit(X: Matrix): IUnsupervisedModel;
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

function UMatrixPipeline.Transform(X: Matrix): Matrix;
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

function UMatrixPipeline.Predict(X: Matrix): Vector;
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

function UMatrixPipeline.ToString: string;
begin
  var sb := 'UMatrixPipeline (' +
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

function UMatrixPipeline.Clone: IModel;
begin
  if fModel = nil then
    ArgumentError(ER_MODEL_NULL);

  var p := new UMatrixPipeline;

  foreach var t in fTransformers do
    p.Add(t.Clone);

  p.SetModel(fModel.Clone);

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

  fFitted := true;
  Result := Self;
end;

function StandardScaler.FitTransform(X: Matrix): Matrix;
begin
  Fit(X);
  Result := Transform(X);
end;

function StandardScaler.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);

  for var i := 0 to n - 1 do
  for var j := 0 to p - 1 do
    if Abs(fStd[j]) < 1e-12 then
      Result[i,j] := 0.0
    else
      Result[i,j] := (X[i,j] - fMean[j]) / fStd[j];
end;

function StandardScaler.InverseTransform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
      if fStd[j] <> 0 then
        Result[i,j] := X[i,j] * fStd[j] + fMean[j]
      else
        Result[i,j] := fMean[j];
end;

function StandardScaler.ToString: string;
begin
  Result := 'StandardScaler';
end;

function StandardScaler.Clone: ITransformer;
begin
  Result := new StandardScaler;
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

function MinMaxScaler.FitTransform(X: Matrix): Matrix;
begin
  Fit(X);
  Result := Transform(X);
end;

function MinMaxScaler.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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

function MinMaxScaler.InverseTransform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

  var n := X.RowCount;
  var p := X.ColCount;

  Result := new Matrix(n, p);
  
  var scale := fRangeMax - fRangeMin;

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
    begin
      var range := fMax[j] - fMin[j];

      if (scale > 1e-12) and (range > 1e-12) then
        Result[i,j] := (X[i,j] - fRangeMin) / scale * range + fMin[j]
      else
        Result[i,j] := fMin[j];
    end;
end;

function MinMaxScaler.ToString: string;
begin
  Result := 'MinMaxScaler(min=' + fRangeMin + ', max=' + fRangeMax + ')';
end;

function MinMaxScaler.Clone: ITransformer;
begin
  Result := new MinMaxScaler(fRangeMin, fRangeMax);
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
  
  fMean := X.ColumnMeans;

  var (W, variances) := X.PCA(fK);
  
  fComponents := W;

  fFitted := true;
  Result := Self;
end;

function PCATransformer.FitTransform(X: Matrix): Matrix;
begin
  Fit(X);
  Result := Transform(X);
end;

function PCATransformer.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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
  Result := new PCATransformer(fK);
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

function VarianceThreshold.FitTransform(X: Matrix): Matrix;
begin
  Fit(X);
  Result := Transform(X);
end;

function VarianceThreshold.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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
  Result := new VarianceThreshold(fThreshold);
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
  var yInt := LabelsToInts(y);

  for var i := 0 to n - 1 do
  begin
    var ir := yInt[i];

    if ir not in classToIndex then
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
  var yInt := LabelsToInts(y);

  for var i := 0 to n - 1 do
  begin
    var ir := yInt[i];

    if ir not in classToIndex then
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

function SelectKBest.FitTransform(X: Matrix; y: Vector): Matrix;
begin
  Fit(X, y);
  Result := Transform(X);
end;

function SelectKBest.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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
  if fScoreFunc <> nil then
    Result := new SelectKBest(fK, fScoreFunc)
  else
    Result := new SelectKBest(fK, fScoreType);
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

function Normalizer.FitTransform(X: Matrix): Matrix;
begin
  Fit(X);
  Result := Transform(X);
end;

function Normalizer.Transform(X: Matrix): Matrix;
begin
  if not fFitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  if X = nil then
    ArgumentNullError(ER_X_NULL);

  if X.ColCount <> fFeatureCount then
    DimensionError(ER_FEATURE_COUNT_MISMATCH,X.ColCount,fFeatureCount);

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
  Result := new Normalizer(fNormType);
end;
    
end.
