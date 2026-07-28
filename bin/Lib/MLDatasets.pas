unit MLDatasets;

interface

uses DataFrameABC, LinearAlgebraML;

type
  TaskType = (Classification, Regression, Clustering);

  /// ML-датасет: таблица данных вместе с метаданными задачи.
  ///
  /// Dataset содержит:
  ///   Data     — таблицу данных (DataFrame)
  ///   Features — список признаков
  ///   Target   — целевую переменную (если есть)
  ///   Task     — тип задачи (classification/regression/clustering)
  ///
  /// Предоставляет удобные методы для получения матриц признаков
  /// и целевых значений для обучения моделей
  Dataset = class
  private
    fData: DataFrame;
    function ValueLabel(feature, value: string): string;
    function CloneMeta(df: DataFrame): Dataset;
    function GetCategoricalFeatures: array of string;
  public
    FeatureLabels: Dictionary<string,string>;
    ValueLabels: Dictionary<string,Dictionary<string,string>>;
    Description: string;

    constructor Create;

    /// Создаёт Dataset на основе готовой таблицы данных и основных метаданных задачи.
    static function FromData(
      df: DataFrame;
      task: TaskType;
      features: array of string;
      target: string := nil
    ): Dataset;

    auto property Name: string;
    property Data: DataFrame read fData;
    auto property Features: array of string;
    auto property Target: string;
    auto property Task: TaskType;
    
    /// Возвращает true, если датасет относится к задаче с учителем
    /// (classification или regression).
    function IsSupervised: boolean;
  
    /// Разбивает датасет на обучающую и тестовую части.
    /// testRatio — доля тестовой выборки (0 < testRatio < 1).
    /// shuffle — перемешивать ли строки перед разбиением.
    /// seed — начальное значение генератора случайных чисел (для воспроизводимости).
    /// Возвращает два датасета: (train, test).
    function TrainTestSplit(testRatio: real := 0.2; shuffle: boolean := True; seed: integer := -1): (Dataset, Dataset);
    
    /// Выполняет стратифицированное разбиение датасета на обучающую и тестовую части.
    /// Сохраняет распределение значений целевой переменной в обеих выборках.
    /// testRatio — доля тестовой выборки (0 < testRatio < 1).
    /// seed — начальное значение генератора случайных чисел (для воспроизводимости).
    /// Возвращает два датасета: (train, test).
    function StratifiedTrainTestSplit(testRatio: real := 0.2; seed: integer := -1): (Dataset, Dataset);
  
    /// Возвращает первые n строк таблицы данных.
    function Head(n: integer := 10): DataFrame;
  
    /// Возвращает краткое описание датасета (метаданные).
    function Describe: DataFrame;
    
    procedure Info;
    
    function Classes: array of string;
    function ClassCounts: Dictionary<string,integer>;
    function ClassName(value: string): string;
    
    function RowCount: integer := Data.RowCount;
    
    function GetFeatureColumns: array of string;
    property CategoricalFeatures: array of string read GetCategoricalFeatures;
    
    function HasTarget: boolean;
  end;

  /// Кодирует target-колонку классификационного Dataset в массив целых меток.
  /// Используется только для целевой переменной, не для признаков.
  LabelEncoder = class
  private
    fClasses: array of string;
    fClassToIndex: Dictionary<string, integer>;
    
    function GetClasses: array of string;
    procedure EnsureFitted;
    procedure CheckDataset(ds: Dataset);
    function TargetLabels(ds: Dataset): array of string;
  public
    function Fit(ds: Dataset): LabelEncoder;
    function Transform(ds: Dataset): array of integer;
    function FitTransform(ds: Dataset): array of integer;
    
    property Classes: array of string read GetClasses;
    
    function ClassName(index: integer): string;
    function ClassIndex(name: string): integer;
    function Decode(y: array of integer): array of string;
  end;

  /// Набор генераторов и загрузчиков датасетов для задач машинного обучения.
  /// Содержит синтетические генераторы (MakeBlobs, MakeMoons, MakeRegression)
  /// и реальные учебные датасеты (например, RussianHousing, StudentExam).
  /// Используется в примерах, экспериментах и демонстрациях алгоритмов ML
  Datasets = static class
  public
    static Language: string := 'ru';
    // --- Синтетические датасеты (Matrix + Vector)
    
    /// Генерирует синтетический датасет из гауссовых кластеров.
    /// Каждый кластер задаётся центром и разбросом. Используется для задач кластеризации и классификации.
    ///
    /// Параметры:
    /// • n — число объектов
    /// • centers — число кластеров
    /// • nFeatures — размерность пространства
    /// • clusterStd — базовое стандартное отклонение
    /// • clusterStdVar — разброс std между кластерами (0 → одинаковые)
    /// • centerBox — диапазон генерации центров [-centerBox, centerBox]
    /// • classBalance — равномерность распределения объектов по кластерам (0..1]
    ///   • classBalance = 1.0  — строго равномерное распределение (детерминированное, не зависит от seed)
    ///   • classBalance < 1.0  — случайное распределение; чем меньше значение, тем выше дисбаланс в среднем
    /// • noisePoints — число шумовых точек (outliers)
    /// • shuffle — перемешивание
    /// • seed — генератор (seed < 0 → случайный)
    static function MakeBlobs(
      n: integer := 300;
      centers: integer := 3;
      nFeatures: integer := 2;
      clusterStd: real := 1.0;
      clusterStdVar: real := 0.0;
      centerBox: real := 5.0;
      classBalance: real := 1.0;
      noisePoints: integer := 0;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, array of integer);
      
    /// Генерирует синтетический датасет «две луны» (two interleaving moons),
    /// часто используемый для демонстрации алгоритмов классификации и кластеризации.
    /// Возвращает матрицу признаков X (n × 2) и вектор меток классов y (0 или 1).
    ///
    /// • n — число генерируемых точек.
    /// • noise — стандартное отклонение гауссовского шума, добавляемого к координатам.
    /// • shuffle — перемешивать ли порядок объектов.
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время).
    static function MakeMoons(
      n: integer := 300;
      noise: real := 0.05;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, array of integer);
    
    /// Генерирует синтетический датасет для задачи регрессии.
    /// Данные создаются по модели y = Xβ + f(X) + ε, где:
    /// X — матрица признаков,
    /// β — вектор коэффициентов (часть признаков может быть неинформативной),
    /// f(X) — добавочная нелинейная компонента,
    /// ε — гауссовский шум.
    /// Возвращает матрицу признаков X (n × nFeatures) и вектор целевой переменной y.
    ///
    /// • n — число объектов.
    /// • nFeatures — общее число признаков.
    /// • nInformative — число информативных признаков (остальные имеют нулевые коэффициенты).
    /// • noise — стандартное отклонение гауссовского шума.
    /// • coefScale — масштаб коэффициентов β.
    /// • bias — свободный член (смещение).
    /// • nonlinearStrength — коэффициент нелинейной компоненты (например, квадрат первого признака).
    /// • shuffle — перемешивать ли порядок объектов.
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время).
    static function MakeRegression(
      n: integer := 300;
      nFeatures: integer := 10;
      nInformative: integer := 5;
      noise: real := 0.1;
      coefScale: real := 1.0;
      bias: real := 0.0;
      nonlinearStrength: real := 0.0;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, Vector);      
    
    /// Генерирует синтетический датасет из двух концентрических окружностей.
    /// Используется для демонстрации задач классификации и кластеризации, в которых граница разделения является нелинейной.
    ///
    /// Датасет состоит из двух классов: внешний круг (класс 0) и внутренний круг (класс 1)
    ///
    /// Параметры позволяют управлять сложностью задачи:
    /// • noise — отклонение точек от идеальной окружности
    /// • factor — отношение радиусов внутреннего и внешнего круга
    /// • classBalance — доля объектов внутреннего круга
    /// • flipProb — вероятность случайной инверсии метки
    /// • scale — общий масштаб (радиус внешнего круга)
    ///
    /// Полезен для демонстрации:
    /// • ограничений линейных моделей (LogisticRegression, LinearSVM)
    /// • преимуществ нелинейных моделей (DecisionTree, RandomForest, kNN)
    /// • методов кластеризации (DBSCAN, Spectral Clustering)
    static function MakeCircles(
      n: integer := 300;
      noise: real := 0.05;
      factor: real := 0.5;
      classBalance: real := 0.5;
      flipProb: real := 0.0;
      scale: real := 1.0;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, array of integer);
    
    /// Генерирует синтетический датасет в виде спиралей.
    /// Используется для демонстрации сложных нелинейных границ
    /// классификации и возможностей нейросетей и деревьев решений.
    ///
    /// • n — число объектов.
    /// • classes — число спиральных ветвей (классов).
    /// • noise — стандартное отклонение гауссовского шума.
    /// • turns — число оборотов спирали.
    /// • radius — максимальный радиус спирали.
    /// • shuffle — перемешивать ли порядок объектов.
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время)
    static function MakeSpiral(
      n: integer := 300;
      classes: integer := 2;
      noise: real := 0.1;
      turns: real := 3.0;
      radius: real := 1.0;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, array of integer);

    /// Генерирует синтетический датасет для задачи классификации.
    /// Формирует линейно (или почти линейно) разделимые классы с контролируемым шумом.
    ///
    /// • n — число объектов.
    /// • nFeatures — общее число признаков.
    /// • nInformative — число информативных признаков.
    /// • nRedundant — число линейно зависимых признаков.
    /// • noise — уровень шума в модели.
    /// • classSep — расстояние между классами.
    /// • flipProb — вероятность случайной смены метки (label noise).
    /// • classBalance — доля класса 1 (0..1).
    /// • shuffle — перемешивание объектов.
    /// • seed — генератор случайных чисел (-1 → авто)
    static function MakeClassification(
      n: integer := 300;
      nFeatures: integer := 10;
      nInformative: integer := 5;
      nRedundant: integer := 2;
      noise: real := 0.1;
      classSep: real := 1.0;
      flipProb: real := 0.0;
      classBalance: real := 0.5;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, array of integer);      
    
    /// Загружает датасет по имени.
    ///
    /// Датасет ищется в каталоге:
    /// Files\Datasets
    ///
    /// Для датасета должны существовать файлы:
    ///   name.meta — метаданные датасета
    ///   name.csv  — данные таблицы
    ///
    /// Метаданные определяют:
    ///   тип задачи (classification / regression / clustering)
    ///   целевую переменную
    ///   список признаков (опционально)
    ///
    /// Если признаки не указаны в .meta, используются
    /// все столбцы таблицы, кроме целевой переменной.
    ///
    /// Пример:
    ///   var ds := Datasets.Load('Iris');
    static function Load(name: string): Dataset;
    
    // --- DataFrame датасеты (реалистичные таблицы, считываемые из csv)
    
    /// Датасет Iris (классификация).
    ///
    /// 150 объектов, 4 признака:
    /// sepal_length, sepal_width, petal_length, petal_width.
    /// Целевая переменная: species.
    ///
    /// Пример:
    ///   var ds := Datasets.Iris;
    ///   var df := ds.Data;
    ///   var X := df.ToMatrix(ds.Features);
    ///   var y := df.EncodeLabels(ds.Target);
    static function Iris: Dataset;
    
    /// Датасет цен на квартиры (задача регрессии)
    static function MoscowHousing: Dataset;

    /// Датасет российских городов (задача кластеризации)
    static function RussianCities: Dataset;

    /// Датасет пассажиров Титаника (задача классификации)
    static function TitanicRu: Dataset;

    /// Датасет цен на автомобили с пробегом (задача регрессии)
    static function UsedCarsPrice: Dataset;

    /// Сбалансированный набор MNIST из 5000 изображений для классификации рукописных цифр
    static function MnistSmall: Dataset;

    
    {/// Датасет результатов экзамена студентов (классификация)
    static function StudentExam: Dataset;
    
    /// Датасет банковских клиентов (классификация одобрения кредита)
    static function BankClients: Dataset;
    
    /// Датасет поездок такси (регрессия стоимости поездки)
    static function TaxiTrips: Dataset;
    
    /// Датасет транспортной активности пассажиров (кластеризация)
    static function MoscowTransport: Dataset;
    
    /// Датасет интернет-покупок пользователей (классификация покупки)
    static function OnlineShopping: Dataset;}
    
    static function LoadMeta(path: string): Dictionary<string,string>;
    static function ParseFeatures(meta: Dictionary<string,string>): array of string;
  end;
  

implementation

uses MLExceptions;
uses DataAdapters;
uses DataFrameABCCore;

constructor Dataset.Create;
begin
  fData := nil;
  Features := [];
  Target := nil;
  Task := TaskType.Clustering;
  Name := nil;
  Description := '';
  FeatureLabels := new Dictionary<string,string>;
  ValueLabels := new Dictionary<string,Dictionary<string,string>>;
end;

const
  ER_PARAM_GT_ZERO =
    'Параметр {0} должен быть > 0!!Parameter {0} must be > 0';
  ER_PARAM_GT_ONE =
    'Параметр {0} должен быть > 1!!Parameter {0} must be > 1';
  ER_PARAM_GE_ZERO =
    'Параметр {0} должен быть >= 0!!Parameter {0} must be >= 0';
  ER_PARAM_BETWEEN_01 =
    'Параметр {0} должен быть в диапазоне (0,1)!!Parameter {0} must be in range (0,1)';
  ER_DATASET_NO_TARGET =
    'У датасета нет целевой переменной (задача кластеризации).!!' +
    'Dataset has no target variable (clustering task).';
  ER_DATASET_TARGET_NOT_FOUND =
    'Целевая переменная "{0}" не найдена в таблице.!!' +
    'Target column "{0}" not found in DataFrame';
  ER_DATASET_META_NOT_FOUND =
    'Файл метаданных датасета "{0}" не найден!!Dataset meta file "{0}" not found';
  ER_DATASET_CSV_NOT_FOUND =
    'Файл данных датасета "{0}" не найден!!Dataset csv file "{0}" not found';
  ER_DATASET_TASK_UNKNOWN =
    'Неизвестный тип задачи датасета "{0}"!!Unknown dataset task "{0}"';  
  ER_DATASET_TASK_MISSING =
    'В метаданных датасета "{0}" отсутствует поле task!!Dataset meta missing field "task" for "{0}"';  
  ER_DATASET_FEATURE_NOT_FOUND =
    'Признак "{0}" не найден в датасете!!Feature column "{0}" not found in dataset';
  ER_DATASET_FEATURE_EQUALS_TARGET =
    'Признак "{0}" совпадает с целевой переменной!!Feature "{0}" equals target column';
  ER_DATASET_FEATURES_EMPTY =
    'Список признаков не должен быть пустым!!Feature list must not be empty';
  ER_DATASET_META_FEATURE_NOT_IN_FEATURES =
    'В метаданных указан feature.{0}, но столбец "{0}" не входит в список features!!' +
    'Meta contains feature.{0}, but column "{0}" is not listed in features';
  ER_DATASET_FEATURES_WILDCARD_REQUIRES_FEATURES =
    'Ключ features.* допустим только когда задан список features!!' +
    'Key features.* is only allowed when features are explicitly listed';
  ER_DATASET_META_UNKNOWN_COLUMN_TYPE =
    'Неизвестный тип столбца в метаданных: {0}!!Unknown column type in dataset meta: {0}';
  ER_DATASET_TARGETTYPE_REQUIRES_TARGET =
    'Ключ targetType допустим только когда задан target!!' +
    'Key targetType is only allowed when target is defined';
  ER_CLASSES_ONLY_CLASSIFICATION =
    'Classes доступны только для задач классификации!!Classes are only available for classification datasets';
  ER_VALUECOUNTS_ONLY_CLASSIFICATION =
    'ValueCounts доступны только для задач классификации!!ValueCounts are only available for classification datasets';    
  ER_DATASET_TARGET_MISSING =
    'Target обязателен для задач с учителем!!Target is required for supervised learning datasets'; 
  ER_DATASET_TARGET_FOR_CLUSTERING =
    'Target не должен задаваться для задачи кластеризации!!Target must not be specified for clustering datasets';
  ER_PARAM_LE =
    'Параметр {0} должен быть <= допустимого максимума!!Parameter {0} must be <= the allowed maximum value';
  ER_PARAM_RANGE_01 =
    'Параметр {0} должен быть в диапазоне (0, 1)!!Parameter {0} must be in range (0, 1)';   
  ER_PARAM_LT =
    'Параметр {0} должен быть меньше допустимого значения!!Parameter {0} must be less than allowed value';    
  ER_GROUPBY_UNSUPPORTED_KEY_TYPE = 
    'Неподдерживаемый тип ключа для группировки!!Unsupported key type for grouping';
  ER_STRATIFIED_ONLY_FOR_CLASSIFICATION =
    'Стратифицированное разбиение доступно только для задач классификации!!Stratified split is only for classification tasks';
  ER_CLASS_BALANCE_TOO_SMALL =
    'Слишком малое значение classBalance: {0}. Минимально допустимое значение — 1e-3!!classBalance is too small: {0}. Minimum allowed value is 1e-3';
  ER_UNSUPPORTED_TARGET_TYPE =
    'Неподдерживаемый тип целевого столбца: {0}!!Unsupported target column type: {0}';    
  ER_DATASET_TARGET_HAS_MISSING =
    'Целевой столбец "{0}" содержит пропуски (NA), что здесь не поддерживается!!' +
    'Target column "{0}" contains missing values (NA), which is not supported here';
  ER_ENCODELABELS_UNSUPPORTED_TYPE =
    'Неподдерживаемый тип столбца для кодирования меток: {0}!!' +
    'Unsupported column type for label encoding: {0}';  
  ER_LABEL_ENCODER_NOT_FITTED =
    'LabelEncoder не обучен. Сначала вызовите Fit или FitTransform.!!' +
    'LabelEncoder is not fitted. Call Fit or FitTransform first.';
  ER_LABEL_ENCODER_UNKNOWN_CLASS =
    'Неизвестная метка класса: {0}!!Unknown class label: {0}';
  ER_LABEL_ENCODER_INDEX_OUT_OF_RANGE =
    'Индекс класса вне диапазона: {0}!!Class index out of range: {0}';
  
  C_DATASET      = 'Датасет: {0}!!Dataset: {0}';
  C_DESCRIPTION  = 'Описание:!!Description:';
  C_TASK         = 'Задача: {0}!!Task: {0}';
  C_ROWS         = 'Строк: {0}!!Rows: {0}';
  C_FEATURES     = 'Признаков: {0}!!Features: {0}';
  C_TARGET_BLOCK = 'Целевой столбец:!!Target column:';
  C_NAME         = 'Имя: {0}!!Name: {0}';
  C_SOURCE       = 'Источник: {0}!!Source: {0}';
  C_URL          = 'Ссылка: {0}!!URL: {0}';
  C_CLASSES      = 'Классов: {0}!!Classes: {0}';
  C_FEATURE_LIST = 'Признаки:!!Features:';
  C_CATEGORICAL  = 'Категориальные признаки: {0}!!Categorical features: {0}';

function TaskDisplayName(task: TaskType): string;
begin
  case task of
    TaskType.Classification: Result := 'классификация';
    TaskType.Regression: Result := 'регрессия';
    TaskType.Clustering: Result := 'кластеризация';
    else Result := task.ToString;
  end;
end;

function TryParseFeatureTypeKey(key: string; var columnName: string): boolean;
begin
  Result := False;
  columnName := nil;

  if (key = nil) or not key.StartsWith('feature.') then
    exit;

  columnName := key.Substring('feature.'.Length);
  if (columnName = '') or (columnName.IndexOf('.') >= 0) then
  begin
    columnName := nil;
    exit;
  end;

  Result := True;
end;

function TryParseMetaColumnType(s: string; var columnType: ColumnType; var isCategorical: boolean): boolean;
begin
  Result := True;
  isCategorical := False;

  case s.ToLower of
    'int', 'integer':
      columnType := ctInt;
    'float', 'real', 'double':
      columnType := ctFloat;
    'str', 'string':
      columnType := ctStr;
    'bool', 'boolean':
      columnType := ctBool;
    'datetime', 'date', 'time':
      columnType := ctDateTime;
    'categorical':
      begin
        columnType := ctStr;
        isCategorical := True;
      end;
  else
    Result := False;
  end;
end;

procedure ValidateDatasetMeta(meta: Dictionary<string,string>; target: string; features: array of string);
begin
  var featureSet := if features = nil then nil else new HashSet<string>(features);

  foreach var key in meta.Keys do
  begin
    var colName: string;
    if not TryParseFeatureTypeKey(key, colName) then
      continue;

    // Legacy-хвост: target раньше иногда описывался как feature.<target> = categorical.
    // Не считаем это ошибкой, но и не рассматриваем target как признак.
    if (target <> nil) and (colName = target) then
      continue;

    if featureSet = nil then
      continue;

    if not (colName in featureSet) then
      ArgumentError(ER_DATASET_META_FEATURE_NOT_IN_FEATURES, colName);
  end;

  if meta.ContainsKey('features.*') and (featureSet = nil) then
    ArgumentError(ER_DATASET_FEATURES_WILDCARD_REQUIRES_FEATURES);

  if meta.ContainsKey('targetType') and ((target = nil) or (target = '')) then
    ArgumentError(ER_DATASET_TARGETTYPE_REQUIRES_TARGET);
end;

procedure BuildFeatureTyping(
  meta: Dictionary<string,string>;
  target: string;
  features: array of string;
  var columnTypes: Dictionary<string, ColumnType>;
  var categoricalColumns: array of string);
begin
  var typeMap := new Dictionary<string, ColumnType>;
  var categoricalSet := new HashSet<string>;

  if meta.ContainsKey('features.*') then
  begin
    var wildcardType: ColumnType;
    var wildcardCategorical: boolean;

    if not TryParseMetaColumnType(meta['features.*'], wildcardType, wildcardCategorical) then
      ArgumentError(ER_DATASET_META_UNKNOWN_COLUMN_TYPE, meta['features.*']);

    foreach var feature in features do
    begin
      if wildcardCategorical then
        categoricalSet.Add(feature)
      else
        typeMap[feature] := wildcardType;
    end;
  end;

  foreach var key in meta.Keys do
  begin
    var colName: string;
    if not TryParseFeatureTypeKey(key, colName) then
      continue;

    if (target <> nil) and (colName = target) then
      continue;

    var t: ColumnType;
    var isCategorical: boolean;
    if not TryParseMetaColumnType(meta[key], t, isCategorical) then
      ArgumentError(ER_DATASET_META_UNKNOWN_COLUMN_TYPE, meta[key]);

    if isCategorical then
    begin
      typeMap.Remove(colName);
      categoricalSet.Add(colName);
    end
    else
    begin
      categoricalSet.Remove(colName);
      typeMap[colName] := t;
    end;
  end;

  columnTypes := if typeMap.Count = 0 then nil else typeMap;
  categoricalColumns := if categoricalSet.Count = 0 then nil else categoricalSet.ToArray;
end;

procedure ApplyTargetTyping(
  meta: Dictionary<string,string>;
  target: string;
  var columnTypes: Dictionary<string, ColumnType>;
  var categoricalColumns: array of string);
begin
  if (target = nil) or (target = '') or not meta.ContainsKey('targetType') then
    exit;

  var t: ColumnType;
  var isCategorical: boolean;
  if not TryParseMetaColumnType(meta['targetType'], t, isCategorical) then
    ArgumentError(ER_DATASET_META_UNKNOWN_COLUMN_TYPE, meta['targetType']);

  var cats := if categoricalColumns = nil then new HashSet<string> else new HashSet<string>(categoricalColumns);
  var types := if columnTypes = nil then new Dictionary<string, ColumnType> else columnTypes;

  if isCategorical then
  begin
    types.Remove(target);
    cats.Add(target);
  end
  else
  begin
    cats.Remove(target);
    types[target] := t;
  end;

  columnTypes := if types.Count = 0 then nil else types;
  categoricalColumns := if cats.Count = 0 then nil else cats.ToArray;
end;

function HasExplicitTypeForColumn(
  columnName: string;
  columnTypes: Dictionary<string, ColumnType>;
  categoricalColumns: array of string
): boolean;
begin
  if (columnTypes <> nil) and columnTypes.ContainsKey(columnName) then
    exit(True);

  if categoricalColumns <> nil then
    foreach var name in categoricalColumns do
      if name = columnName then
        exit(True);

  Result := False;
end;

function CanSkipCsvInference(
  features: array of string;
  target: string;
  columnTypes: Dictionary<string, ColumnType>;
  categoricalColumns: array of string
): boolean;
begin
  if features = nil then
    exit(False);

  foreach var feature in features do
    if not HasExplicitTypeForColumn(feature, columnTypes, categoricalColumns) then
      exit(False);

  if (target <> nil) and (target <> '') and not HasExplicitTypeForColumn(target, columnTypes, categoricalColumns) then
    exit(False);

  Result := True;
end;
    
function Normal(rnd: System.Random): real;
begin
  var u1 := rnd.NextDouble;
  var u2 := rnd.NextDouble;
  Result := Sqrt(-2 * Ln(u1)) * Cos(2 * Pi * u2);
end;

function Tr(s: string): string;
begin
  var p := s.IndexOf('!!');

  if p < 0 then
    exit(s);

  if Datasets.Language = 'ru' then
    Result := s.Substring(0, p)
  else
    Result := s.Substring(p + 2);
end;

procedure PrintTr(s: string; params args: array of object);
begin
  Print(Format(Tr(s), args));
end;

procedure PrintlnTr(s: string; params args: array of object);
begin
  Println(Format(Tr(s), args));
end;

//-----------------------------
//          Dataset
//-----------------------------

function Dataset.IsSupervised: boolean;
begin
  Result := Task <> Clustering;
end;

class function Dataset.FromData(
  df: DataFrame;
  task: TaskType;
  features: array of string;
  target: string): Dataset;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');

  if (features = nil) or (features.Length = 0) then
    ArgumentError(ER_DATASET_FEATURES_EMPTY);

  if task in [TaskType.Classification, TaskType.Regression] then
  begin
    if (target = nil) or (target = '') then
      ArgumentError(ER_DATASET_TARGET_MISSING);

    if not df.HasColumn(target) then
      ArgumentError(ER_DATASET_TARGET_NOT_FOUND, target);
  end
  else if (target <> nil) and (target <> '') then
    ArgumentError(ER_DATASET_TARGET_FOR_CLUSTERING);

  Result := new Dataset;
  Result.fData := df;
  Result.Task := task;
  Result.Features := Copy(features);
  Result.Target := target;

  foreach var f in Result.Features do
  begin
    if (f = nil) or (f = '') then
      ArgumentError(ER_DATASET_FEATURE_NOT_FOUND, f);

    if not df.HasColumn(f) then
      ArgumentError(ER_DATASET_FEATURE_NOT_FOUND, f);

    if (target <> nil) and (target <> '') and (f = target) then
      ArgumentError(ER_DATASET_FEATURE_EQUALS_TARGET, f);

    Result.FeatureLabels[f] := f;
  end;

  if (target <> nil) and (target <> '') then
    Result.FeatureLabels[target] := target;
end;

function Dataset.CloneMeta(df: DataFrame): Dataset;
begin
  Result := new Dataset;

  Result.Name := Name;
  Result.fData := df;

  Result.Features := Copy(Features);
  Result.Target := Target;
  Result.Task := Task;

  Result.Description := Description;

  Result.FeatureLabels := new Dictionary<string,string>(FeatureLabels);

  Result.ValueLabels := new Dictionary<string, Dictionary<string,string>>;
  foreach var kvp in ValueLabels do
    Result.ValueLabels[kvp.Key] := new Dictionary<string,string>(kvp.Value);
end;

function Dataset.TrainTestSplit(testRatio: real; shuffle: boolean; seed: integer): (Dataset, Dataset);
begin
  if Data = nil then
    ArgumentNullError(ER_ARG_NULL, 'Data');

  var (trainDf, testDf) := Data.TrainTestSplit(testRatio, shuffle, seed);

  var trainDs := CloneMeta(trainDf);
  var testDs := CloneMeta(testDf);

  Result := (trainDs, testDs);
end;

function Dataset.StratifiedTrainTestSplit(testRatio: real; seed: integer): (Dataset, Dataset);
begin
  if Data = nil then
    ArgumentNullError(ER_ARG_NULL, 'Data');

  if Task <> Classification then
    Error(ER_STRATIFIED_ONLY_FOR_CLASSIFICATION);

  var (trainDf, testDf) :=
    Data.StratifiedTrainTestSplit(Target, testRatio, seed);

  var trainDs := CloneMeta(trainDf);
  var testDs  := CloneMeta(testDf);

  Result := (trainDs, testDs);
end;

function Dataset.Head(n: integer): DataFrame;
begin
  if Data = nil then
    ArgumentNullError(ER_ARG_NULL, 'Data');

  Result := Data.Head(n);
end;

function Dataset.Describe: DataFrame;
begin
  if Data = nil then
    ArgumentNullError(ER_ARG_NULL, 'Data');

  var props := Arr(
    'Name',
    'Rows',
    'Columns',
    'Task',
    'Target',
    'FeatureCount',
    'Features'
  );

  var values := Arr(
    Name,
    Data.RowCount.ToString,
    Data.ColumnCount.ToString,
    Task.ToString,
    Target,
    Features.Length.ToString,
    Features.JoinToString(', ')
  );

  var valid := ArrFill(props.Length, true);

  var df := new DataFrame;
  df.AddStrColumn('Property', props, valid);
  df.AddStrColumn('Value', values, valid);
  
  var names := Arr('Property', 'Value');
  var types := Arr(ColumnType.ctStr, ColumnType.ctStr);
  var cats  := Arr(true, false);
  
  df.SetSchema(new DataFrameSchema(names, types, cats));

  Result := df;
end;

procedure Dataset.Info;
begin
  PrintlnTr(C_DATASET, Name);
  Println;

  if (Description <> nil) and (Description <> '') then
  begin
    Println(Tr(C_DESCRIPTION));
    Println(Description);
    Println;
  end;

  PrintlnTr(C_TASK, TaskDisplayName(Task));
  PrintlnTr(C_ROWS, Data.RowCount);
  PrintlnTr(C_FEATURES, Features.Length);

  if CategoricalFeatures.Length > 0 then
    PrintlnTr(C_CATEGORICAL, CategoricalFeatures.JoinToString(', '));

  var labeledFeatures := Features
    .Where(f -> FeatureLabels.ContainsKey(f) and (FeatureLabels[f] <> nil) and (FeatureLabels[f] <> '') and (FeatureLabels[f] <> f))
    .ToArray;

  var labelWidth := 0;
  if labeledFeatures.Length > 0 then
    labelWidth := labeledFeatures.Max(f -> f.Length);
  if (Target <> nil) and (Target <> '') then
    labelWidth := Max(labelWidth, Target.Length);

  if (Target <> nil) and (Target <> '') then
  begin
    Println;
    Println(Tr(C_TARGET_BLOCK));

    var hasTargetLabel :=
      FeatureLabels.ContainsKey(Target) and
      (FeatureLabels[Target] <> nil) and
      (FeatureLabels[Target] <> '') and
      (FeatureLabels[Target] <> Target);

    if hasTargetLabel then
      Println(Target.PadRight(labelWidth), ' → ', FeatureLabels[Target])
    else
      Println(Target);
  end;

  if labeledFeatures.Length > 0 then
  begin
    Println;
    Println(Tr(C_FEATURE_LIST));
    
    foreach var f in labeledFeatures do
    begin
      Println(f.PadRight(labelWidth), ' → ', FeatureLabels[f]);
    end;
  end;
end;

function Dataset.Classes: array of string;
begin
  if Task <> TaskType.Classification then
    ArgumentError(ER_CLASSES_ONLY_CLASSIFICATION);

  var idx := Data.ColumnIndex(Target);
  var t := Data.GetColumnType(idx);

  case t of
    ctStr:
    begin
      var col := StrColumn(Data.GetColumn(idx));
      var classes := new List<string>;
      var seen := new HashSet<string>;

      for var i := 0 to Data.RowCount - 1 do
      begin
        if not col.IsValid[i] then
          ArgumentError(ER_DATASET_TARGET_HAS_MISSING, Target);

        var value := col.Data[i];
        if seen.Add(value) then
          classes.Add(value);
      end;

      Result := classes.ToArray;
    end;

    ctInt:
    begin
      var col := IntColumn(Data.GetColumn(idx));
      var classes := new List<string>;
      var seen := new HashSet<integer>;

      for var i := 0 to Data.RowCount - 1 do
      begin
        if not col.IsValid[i] then
          ArgumentError(ER_DATASET_TARGET_HAS_MISSING, Target);

        var value := col.Data[i];
        if seen.Add(value) then
          classes.Add(value.ToString);
      end;

      Result := classes.ToArray;
    end;
    else Error(ER_UNSUPPORTED_TARGET_TYPE, t);
  end;
end;

function Dataset.ClassCounts: Dictionary<string,integer>;
begin
  if Task <> TaskType.Classification then
    ArgumentError(ER_VALUECOUNTS_ONLY_CLASSIFICATION);

  var dict := new Dictionary<string,integer>;

  case Data.GetColumnType(Target) of

    ColumnType.ctStr:
    begin
      var idx := Data.ColumnIndex(Target);
      var labels := StrColumn(Data.GetColumn(idx));

      for var i := 0 to Data.RowCount - 1 do
      begin
        if not labels.IsValid[i] then
          ArgumentError(ER_DATASET_TARGET_HAS_MISSING, Target);

        var v := labels.Data[i];
        if dict.ContainsKey(v) then
          dict[v] += 1
        else
          dict[v] := 1;
      end;
    end;

    ColumnType.ctInt:
    begin
      var idx := Data.ColumnIndex(Target);
      var labels := IntColumn(Data.GetColumn(idx));

      for var i := 0 to Data.RowCount - 1 do
      begin
        if not labels.IsValid[i] then
          ArgumentError(ER_DATASET_TARGET_HAS_MISSING, Target);

        var s := labels.Data[i].ToString;

        if dict.ContainsKey(s) then
          dict[s] += 1
        else
          dict[s] := 1;
      end;
    end;

    else
      ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, Target);
  end;

  Result := dict;
end;

function Dataset.ValueLabel(feature, value: string): string;
begin
  if (ValueLabels <> nil) and
     ValueLabels.ContainsKey(feature) and
     ValueLabels[feature].ContainsKey(value) then
    Result := ValueLabels[feature][value]
  else
    Result := value;
end;

function Dataset.ClassName(value: string): string;
begin
  Result := ValueLabel(Target, value);
end;

function Dataset.GetFeatureColumns: array of string;
begin
  if (Features <> nil) and (Features.Length > 0) then
    Result := Features
  else if Target <> nil then
    Result := Data.Schema.ColumnNames
      .Where(c -> c <> Target)
      .ToArray
  else
    Result := Data.Schema.ColumnNames;
end;

function Dataset.GetCategoricalFeatures: array of string;
begin
  Result := Features
    .Where(f -> Data.IsCategorical(f))
    .ToArray;
end;

function Dataset.HasTarget: boolean;
begin
  Result := (Target <> nil) and (Target <> '');
end;


//-----------------------------
//        LabelEncoder
//-----------------------------

function LabelEncoder.GetClasses: array of string;
begin
  EnsureFitted;
  Result := Copy(fClasses);
end;

procedure LabelEncoder.EnsureFitted;
begin
  if (fClasses = nil) or (fClassToIndex = nil) then
    Error(ER_LABEL_ENCODER_NOT_FITTED);
end;

procedure LabelEncoder.CheckDataset(ds: Dataset);
begin
  if ds = nil then
    ArgumentNullError(ER_ARG_NULL, 'ds');

  if ds.Data = nil then
    ArgumentNullError(ER_ARG_NULL, 'Data');

  if ds.Task <> TaskType.Classification then
    ArgumentError(ER_CLASSES_ONLY_CLASSIFICATION);

  if not ds.HasTarget then
    ArgumentError(ER_DATASET_TARGET_MISSING);

  if not ds.Data.HasColumn(ds.Target) then
    ArgumentError(ER_DATASET_TARGET_NOT_FOUND, ds.Target);
end;

function LabelEncoder.TargetLabels(ds: Dataset): array of string;
begin
  CheckDataset(ds);

  case ds.Data.GetColumnType(ds.Target) of
    ColumnType.ctStr:
    begin
      var idx := ds.Data.ColumnIndex(ds.Target);
      var col := StrColumn(ds.Data.GetColumn(idx));
      SetLength(Result, ds.Data.RowCount);

      for var i := 0 to ds.Data.RowCount - 1 do
      begin
        if not col.IsValid[i] then
          ArgumentError(ER_DATASET_TARGET_HAS_MISSING, ds.Target);
        Result[i] := col.Data[i];
      end;
    end;

    ColumnType.ctInt:
    begin
      var idx := ds.Data.ColumnIndex(ds.Target);
      var col := IntColumn(ds.Data.GetColumn(idx));
      SetLength(Result, ds.Data.RowCount);

      for var i := 0 to ds.Data.RowCount - 1 do
      begin
        if not col.IsValid[i] then
          ArgumentError(ER_DATASET_TARGET_HAS_MISSING, ds.Target);
        Result[i] := col.Data[i].ToString;
      end;
    end;

    else
      ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, ds.Target);
  end;
end;

function LabelEncoder.Fit(ds: Dataset): LabelEncoder;
begin
  var labels := TargetLabels(ds);
  
  var classes := new List<string>;
  fClassToIndex := new Dictionary<string, integer>;

  foreach var labelName in labels do
    if not fClassToIndex.ContainsKey(labelName) then
    begin
      fClassToIndex[labelName] := classes.Count;
      classes.Add(labelName);
    end;

  fClasses := classes.ToArray;
  Result := self;
end;

function LabelEncoder.Transform(ds: Dataset): array of integer;
begin
  EnsureFitted;

  var labels := TargetLabels(ds);
  var y := new integer[labels.Length];

  for var i := 0 to labels.Length - 1 do
  begin
    var labelName := labels[i];

    if not fClassToIndex.ContainsKey(labelName) then
      ArgumentError(ER_LABEL_ENCODER_UNKNOWN_CLASS, labelName);

    y[i] := fClassToIndex[labelName];
  end;

  Result := y;
end;

function LabelEncoder.FitTransform(ds: Dataset): array of integer;
begin
  Fit(ds);
  Result := Transform(ds);
end;

function LabelEncoder.ClassName(index: integer): string;
begin
  EnsureFitted;

  if (index < 0) or (index >= fClasses.Length) then
    ArgumentError(ER_LABEL_ENCODER_INDEX_OUT_OF_RANGE, index);

  Result := fClasses[index];
end;

function LabelEncoder.ClassIndex(name: string): integer;
begin
  EnsureFitted;

  if not fClassToIndex.ContainsKey(name) then
    ArgumentError(ER_LABEL_ENCODER_UNKNOWN_CLASS, name);

  Result := fClassToIndex[name];
end;

function LabelEncoder.Decode(y: array of integer): array of string;
begin
  EnsureFitted;

  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  Result := new string[y.Length];

  for var i := 0 to y.Length - 1 do
  begin
    var index := y[i];

    if (index < 0) or (index >= fClasses.Length) then
      ArgumentError(ER_LABEL_ENCODER_INDEX_OUT_OF_RANGE, index);

    Result[i] := fClasses[index];
  end;
end;


//-----------------------------
//          Datasets
//-----------------------------

static function Datasets.MakeBlobs(
  n, centers, nFeatures: integer;
  clusterStd, clusterStdVar, centerBox, classBalance: real;
  noisePoints: integer; shuffle: boolean;
  seed: integer): (Matrix, array of integer);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');
  
  if centers <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'centers');
  
  if nFeatures <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'nFeatures');
  
  if clusterStd <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'clusterStd');
  
  if clusterStdVar < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'clusterStdVar');
  
  if centerBox <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'centerBox');
  
  if (classBalance <= 0) or (classBalance > 1) then
    ArgumentOutOfRangeError(ER_PARAM_RANGE_01, 'classBalance');
  
  if classBalance < 1e-3 then
    ArgumentOutOfRangeError(ER_CLASS_BALANCE_TOO_SMALL, classBalance);
  
  if noisePoints < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noisePoints');
  
  if noisePoints >= n then
    ArgumentOutOfRangeError(ER_PARAM_LT, 'noisePoints');
  
  var actualSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;
  
  var rnd := new System.Random(actualSeed);
  
  var X := new Matrix(n, nFeatures);
  var y := new integer[n];
  
  // --- центры
  var centersM := new Matrix(centers, nFeatures);
  
  for var c := 0 to centers - 1 do
    for var j := 0 to nFeatures - 1 do
      centersM[c, j] := (rnd.NextDouble * 2 - 1) * centerBox;
  
  // --- std по кластерам
  var stds := new real[centers];
  
  for var c := 0 to centers - 1 do
    stds[c] := clusterStd * (1 + clusterStdVar * (2 * rnd.NextDouble - 1));
  
  // --- вероятности кластеров
  var probs := new real[centers];
  
  if Abs(classBalance - 1.0) < 1e-12 then
  begin
    for var c := 0 to centers - 1 do
      probs[c] := 1.0 / centers;
  end
  else
  begin
    var sum := 0.0;
    
    for var c := 0 to centers - 1 do
    begin
      var p := Power(rnd.NextDouble, 1 / classBalance);
      probs[c] := p;
      sum += p;
    end;
    
    for var c := 0 to centers - 1 do
      probs[c] /= sum;
  end;
  
  // --- CDF
  var cdf := new real[centers];
  cdf[0] := probs[0];
  
  for var c := 1 to centers - 1 do
    cdf[c] := cdf[c-1] + probs[c];
  
  // --- индексы
  var idx := Arr(0..n - 1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  var mainCount := n - noisePoints;
  
  // --- генерация
  for var i := 0 to mainCount - 1 do
  begin
    var row := idx[i];
    
    // --- выбор кластера
    var c: integer;
    if Abs(classBalance - 1.0) < 1e-12 then
      c := i mod centers
    else
    begin
      var r := rnd.NextDouble;
      c := 0;
      
      while (c < centers - 1) and (r > cdf[c]) do
        c += 1;
    end;
    
    y[row] := c;
    
    var std := stds[c];
    
    for var j := 0 to nFeatures - 1 do
      X[row, j] := centersM[c, j] + std * Normal(rnd);
  end;
  
  // --- шум
  for var i := mainCount to n - 1 do
  begin
    var row := idx[i];
    
    y[row] := rnd.Next(centers);
    
    for var j := 0 to nFeatures - 1 do
      X[row, j] := (rnd.NextDouble * 2 - 1) * centerBox;
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeMoons(
  n: integer;
  noise: real;
  shuffle: boolean;
  seed: integer): (Matrix, array of integer);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');
  
  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');
  
  var actualSeed :=
  if seed >= 0 then seed
  else System.Environment.TickCount and integer.MaxValue;
  
  var rnd := new System.Random(actualSeed);
  
  var X := new Matrix(n, 2);
  var y := new integer[n];
  
  var idx := Arr(0..n - 1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  var half := n div 2;
  
  // --- первая луна
  for var i := 0 to half - 1 do
  begin
    var row := idx[i];
    var t := rnd.NextDouble * Pi;
    
    y[row] := 0;
    
    X[row, 0] := Cos(t);
    X[row, 1] := Sin(t);
    
    if noise > 0 then
    begin
      X[row, 0] += noise * Normal(rnd);
      X[row, 1] += noise * Normal(rnd);
    end;
  end;
  
  // --- вторая луна
  for var i := half to n - 1 do
  begin
    var row := idx[i];
    var t := rnd.NextDouble * Pi;
    
    y[row] := 1;
    
    X[row, 0] := 1 - Cos(t);
    X[row, 1] := 0.5 - Sin(t);
    
    if noise > 0 then
    begin
      X[row, 0] += noise * Normal(rnd);
      X[row, 1] += noise * Normal(rnd);
    end;
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeRegression(
  n: integer;
  nFeatures: integer;
  nInformative: integer;
  noise: real;
  coefScale: real;
  bias: real;
  nonlinearStrength: real;
  shuffle: boolean;
  seed: integer): (Matrix, Vector);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');
  
  if nFeatures <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'nFeatures');
  
  if nInformative < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'nInformative');
  
  if nInformative > nFeatures then
    ArgumentOutOfRangeError(ER_PARAM_LE, 'nInformative');
  
  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');
  
  if coefScale <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'coefScale');
  
  var actualSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;
    
    
  var rnd := new System.Random(actualSeed);
  
  var X := new Matrix(n, nFeatures);
  var y := new Vector(n);
  
  // --- коэффициенты
  var beta := new Vector(nFeatures);
  
  for var j := 0 to nFeatures - 1 do
    if j < nInformative then
      beta[j] := coefScale * Normal(rnd)
    else
      beta[j] := 0.0;
  
  var idx := Arr(0..n - 1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  // --- генерация данных
  for var i := 0 to n - 1 do
  begin
    var row := idx[i];
    
    var s := bias;
    
    for var j := 0 to nFeatures - 1 do
    begin
      var xx := Normal(rnd);
      X[row, j] := xx;
      s += xx * beta[j];
    end;
    
    // --- добавляем нелинейность (по первому признаку)
    if nonlinearStrength <> 0 then
    begin
      var x0 := X[row, 0];
      s += nonlinearStrength * x0 * x0;
    end;
    
    // --- шум
    y[row] := s + noise * Normal(rnd);
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeCircles(
  n: integer;
  noise, factor, classBalance, flipProb, scale: real;
  shuffle: boolean;
  seed: integer): (Matrix, array of integer);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');

  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');

  if (factor <= 0) or (factor >= 1) then
    ArgumentOutOfRangeError(ER_PARAM_RANGE_01, 'factor');

  if (classBalance <= 0) or (classBalance >= 1) then
    ArgumentOutOfRangeError(ER_PARAM_RANGE_01, 'classBalance');

  if flipProb < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'flipProb');

  if scale <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'scale');

  var actualSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  var rnd := new System.Random(actualSeed);

  var X := new Matrix(n, 2);
  var y := new integer[n];

  var idx := Arr(0..n-1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);

  for var i := 0 to n - 1 do
  begin
    var row := idx[i];

    // --- класс
    var label1 := Ord(rnd.NextDouble < classBalance);

    // --- радиус
    var r :=
      if label1 = 1 then
        scale * factor
      else
        scale;

    // --- угол
    var angle := 2 * Pi * rnd.NextDouble;

    var xx := r * Cos(angle);
    var yv := r * Sin(angle);

    // --- шум
    xx += noise * Normal(rnd);
    yv += noise * Normal(rnd);

    X[row, 0] := xx;
    X[row, 1] := yv;

    // --- flip labels
    if rnd.NextDouble < flipProb then
      label1 := 1 - label1;

    y[row] := label1;
  end;

  Result := (X, y);
end;

static function Datasets.MakeSpiral(
  n: integer; classes: integer;
  noise: real; turns: real; radius: real;
  shuffle: boolean; seed: integer): (Matrix, array of integer);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');

  if classes <= 1 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ONE, 'classes');

  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');

  if turns <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'turns');

  if radius <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'radius');

  var actualSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  var rnd := new System.Random(actualSeed);

  var X := new Matrix(n,2);
  var y := new integer[n];

  var idx := Arr(0..n-1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);

  var perClass := n div classes;

  for var c := 0 to classes-1 do
  begin
    for var i := 0 to perClass-1 do
    begin
      var row := idx[c*perClass + i];
  
      var u := i / perClass;

      var r := radius * u;
      
      var t := turns * 2 * Pi * u + c * 2 * Pi / classes + noise * 0.5 * Normal(rnd);
      
      y[row] := c;
      
      X[row,0] := r * Cos(t) + noise * Normal(rnd);
      X[row,1] := r * Sin(t) + noise * Normal(rnd);
    end;
  end;
  
  // --- обработка хвоста (если n не делится на classes)
  for var i := classes * perClass to n - 1 do
  begin
    var row := idx[i];
  
    var c := i mod classes;  // равномернее, чем rnd
  
    var u := rnd.NextDouble;
  
    var r := radius * u;
  
    var t := turns * 2 * Pi * u + c * 2 * Pi / classes + noise * 0.5 * Normal(rnd);
  
    y[row] := c;
  
    X[row,0] := r * Cos(t) + noise * Normal(rnd);
    X[row,1] := r * Sin(t) + noise * Normal(rnd);
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeClassification(
  n, nFeatures, nInformative, nRedundant: integer;
  noise, classSep, flipProb, classBalance: real;
  shuffle: boolean;
  seed: integer): (Matrix, array of integer);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');

  if nFeatures <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'nFeatures');

  if nInformative <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'nInformative');

  if nInformative > nFeatures then
    ArgumentOutOfRangeError(ER_PARAM_LE, 'nInformative');

  if nRedundant < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'nRedundant');

  if nInformative + nRedundant > nFeatures then
    ArgumentOutOfRangeError(ER_PARAM_LE, 'nRedundant');

  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');

  if classSep <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'classSep');

  if (classBalance <= 0) or (classBalance >= 1) then
    ArgumentOutOfRangeError(ER_PARAM_RANGE_01, 'classBalance');

  if flipProb < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'flipProb');

  var actualSeed :=
    if seed >= 0 then seed
    else System.Environment.TickCount and integer.MaxValue;

  var rnd := new System.Random(actualSeed);

  var X := new Matrix(n, nFeatures);
  var y := new integer[n];
  
  // --- направление разделения классов
  var center := new Vector(nInformative);
  for var j := 0 to nInformative - 1 do
    center[j] := Normal(rnd);
  
  // --- нормализуем направление, чтобы classSep имел предсказуемый смысл
  var norm := 0.0;
  for var j := 0 to nInformative - 1 do
    norm += center[j] * center[j];
  norm := Sqrt(norm);
  
  if norm > 0 then
    for var j := 0 to nInformative - 1 do
      center[j] := center[j] / norm;
  
  // --- индексы
  var idx := Arr(0..n-1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  for var i := 0 to n - 1 do
  begin
    var row := idx[i];
  
    // --- сначала выбираем класс
    var label1 := Ord(rnd.NextDouble < classBalance);
    var sign := if label1 = 1 then 1.0 else -1.0;
  
    // --- информативные признаки:
    // два класса имеют разные центры вдоль направления center
    for var j := 0 to nInformative - 1 do
      X[row, j] := Normal(rnd) + sign * classSep * center[j] + noise * Normal(rnd);
  
    // --- редундантные признаки: линейные комбинации информативных
    for var j := 0 to nRedundant - 1 do
    begin
      var s := 0.0;
      for var k := 0 to nInformative - 1 do
        s += Normal(rnd) * X[row, k];
      X[row, nInformative + j] := s / nInformative;
    end;
  
    // --- шумовые признаки
    for var j := nInformative + nRedundant to nFeatures - 1 do
      X[row, j] := Normal(rnd);
  
    // --- шум в метках
    if rnd.NextDouble < flipProb then
      label1 := 1 - label1;
  
    y[row] := label1;
  end;
  
  Result := (X, y);
end;

function ParseTask(s: string): TaskType;
begin
  case s.ToLower of
    'classification': Result := Classification;
    'regression':     Result := Regression;
    'clustering':     Result := Clustering;
  else
    ArgumentError(ER_DATASET_TASK_UNKNOWN, s);
  end;
end;

static function Datasets.Load(name: string): Dataset;
begin
  if name = nil then
    ArgumentNullError(ER_ARG_NULL, 'name');

  var baseDir := PascalABCDirectory + 'Files\Datasets\';
  //var baseDir := 'C:\Program Files (x86)\PascalABC.NET\Files\Datasets\';

  var metaPath := baseDir + name + '.meta';
  var csvPath  := baseDir + name + '.csv';

  if not FileExists(metaPath) then
    ArgumentError(ER_DATASET_META_NOT_FOUND, name);
  
  if not FileExists(csvPath) then
    ArgumentError(ER_DATASET_CSV_NOT_FOUND, name);
  
  var meta := LoadMeta(metaPath);

  if not meta.ContainsKey('task') then
    ArgumentError(ER_DATASET_TASK_MISSING, name);

  var task := ParseTask(meta['task']);
  var target := if task in [TaskType.Regression, TaskType.Classification] then
    if meta.ContainsKey('target') then meta['target'] else nil
  else
    nil;
  var features := ParseFeatures(meta);

  if task in [TaskType.Regression, TaskType.Classification] then
    if target = nil then
      ArgumentError(ER_DATASET_TARGET_MISSING, name);

  ValidateDatasetMeta(meta, target, features);
  
  var columnTypes: Dictionary<string, ColumnType>;
  var categoricalColumns: array of string;
  BuildFeatureTyping(meta, target, features, columnTypes, categoricalColumns);
  ApplyTargetTyping(meta, target, columnTypes, categoricalColumns);

  var canSkipInference := CanSkipCsvInference(features, target, columnTypes, categoricalColumns);

  var df := DataFrame.FromCsv(
    csvPath,
    ',',
    True,
    columnTypes,
    categoricalColumns,
    1000,
    not canSkipInference,
    not canSkipInference
  );

  var ds := new Dataset;

  ds.Name := name;
  ds.fData := df;
  
  // --- description
  ds.Description := '';
  
  var dkey := 'description.' + Language;
  
  if meta.ContainsKey(dkey) then
    ds.Description := meta[dkey]
  else if meta.ContainsKey('description.en') then
    ds.Description := meta['description.en'];

  ds.Task := task;
  
  ds.Target := target;
    
  ds.Features := features;
  
  ds.FeatureLabels := [];
  foreach var f in ds.Features do
  begin
    var key := 'feature.' + f + '.' + Datasets.Language;
  
    if meta.ContainsKey(key) then
      ds.FeatureLabels[f] := meta[key]
    else
      ds.FeatureLabels[f] := f;
  end;

  if (ds.Target <> nil) and (ds.Target <> '') then
  begin
    var targetKey := 'target.' + Datasets.Language;
    var legacyTargetKey := 'feature.' + ds.Target + '.' + Datasets.Language;

    if meta.ContainsKey(targetKey) then
      ds.FeatureLabels[ds.Target] := meta[targetKey]
    else if meta.ContainsKey(legacyTargetKey) then
      ds.FeatureLabels[ds.Target] := meta[legacyTargetKey]
    else
      ds.FeatureLabels[ds.Target] := ds.Target;
  end;
  
  ds.ValueLabels := [];
  foreach var k in meta.Keys do
  if k.StartsWith('value.') then
  begin
    var parts := k.Split('.');

    if parts.Length <> 4 then
      continue;

    var feature := parts[1];
    var value := parts[2];
    var lang := parts[3];

    if lang <> Datasets.Language then
      continue;

    if not ds.ValueLabels.ContainsKey(feature) then
      ds.ValueLabels[feature] := new Dictionary<string,string>;

    ds.ValueLabels[feature][value] := meta[k];
  end;
  
  // --- проверка target
  if (ds.Target <> nil) and not df.HasColumn(ds.Target) then
    ArgumentError(ER_DATASET_TARGET_NOT_FOUND, ds.Target);

  // --- проверка features
  foreach var f in ds.Features do
  begin
    if not df.HasColumn(f) then
      ArgumentError(ER_DATASET_FEATURE_NOT_FOUND, f);
  
    if (ds.Target <> nil) and (f = ds.Target) then
      ArgumentError(ER_DATASET_FEATURE_EQUALS_TARGET, f);
  end;
  Result := ds;
end;

static function Datasets.Iris: Dataset;
begin
  Result := Load('Iris');
end;

static function Datasets.MoscowHousing: Dataset;
begin
  Result := Load('moscow_housing');
end;

static function Datasets.RussianCities: Dataset;
begin
  Result := Load('russian_cities');
end;

static function Datasets.TitanicRu: Dataset;
begin
  Result := Load('titanic_ru');
end;

static function Datasets.UsedCarsPrice: Dataset;
begin
  Result := Load('used_cars_price');
end;

static function Datasets.MnistSmall: Dataset;
begin
  Result := Load('mnist_small');
end;

{static function Datasets.StudentExam: Dataset;
begin
  NotImplementedError(ER_NOT_IMPLEMENTED, 'Datasets.StudentExam');
  Result := nil;
end;

static function Datasets.BankClients: Dataset;
begin
  NotImplementedError(ER_NOT_IMPLEMENTED, 'Datasets.BankClients');
  Result := nil;
end;

static function Datasets.TaxiTrips: Dataset;
begin
  NotImplementedError(ER_NOT_IMPLEMENTED, 'Datasets.TaxiTrips');
  Result := nil;
end;

static function Datasets.MoscowTransport: Dataset;
begin
  NotImplementedError(ER_NOT_IMPLEMENTED, 'Datasets.MoscowTransport');
  Result := nil;
end;

static function Datasets.OnlineShopping: Dataset;
begin
  NotImplementedError(ER_NOT_IMPLEMENTED, 'Datasets.OnlineShopping');
  Result := nil;
end;}

static function Datasets.LoadMeta(path: string): Dictionary<string,string>;
begin
  if path = nil then
    ArgumentNullError(ER_ARG_NULL, 'path');

  if not FileExists(path) then
    ArgumentError(ER_DATASET_META_NOT_FOUND, path);

  var dict := new Dictionary<string,string>;

  foreach var line in ReadLines(path, System.Text.Encoding.UTF8) do
  begin
    var s := line.Trim;

    if s = '' then
      continue;

    if s.StartsWith('#') then
      continue;

    var p := s.IndexOf('=');

    if p < 0 then
      continue;

    var key := s.Substring(0,p).Trim;
    var val := s.Substring(p+1).Trim;

    dict[key] := val;
  end;

  Result := dict;
end;

static function Datasets.ParseFeatures(meta: Dictionary<string,string>): array of string;
begin
  if not meta.ContainsKey('features') then
    exit(nil);

  Result :=
    meta['features']
      .Split(',')
      .Select(s -> s.Trim)
      .ToArray;
end;

end.
