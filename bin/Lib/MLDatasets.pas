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
  public
    Name: string;
    Data: DataFrame;
    Features: array of string;
    Target: string;
    Task: TaskType;

    /// Возвращает матрицу признаков X.
    /// Используются столбцы, указанные в Features.
    function ToX: Matrix;
  
    /// Возвращает вектор целевой переменной y.
    /// Доступно только для задач с учителем.
    function ToY: Vector;
  
    /// Возвращает пару (X, y) для обучения модели.
    function ToXY: (Matrix, Vector);
  
    /// Возвращает (X, y), где y преобразован в целочисленные метки классов.
    /// Используется для задач классификации.
    function ToXYInt: (Matrix, array of integer);
  
    /// Возвращает true, если датасет относится к задаче с учителем
    /// (classification или regression).
    function IsSupervised: boolean;
  
    /// Разбивает датасет на обучающую и тестовую части.
    /// testRatio — доля тестовой выборки.
    function TrainTestSplit(testRatio: real := 0.2; seed: integer := -1): (Dataset, Dataset);
  
    /// Возвращает первые n строк таблицы данных.
    function Head(n: integer := 5): DataFrame;
  
    /// Возвращает краткое описание датасета (метаданные).
    function Describe: DataFrame;
  end;

  /// Набор генераторов и загрузчиков датасетов для задач машинного обучения.
  /// Содержит синтетические генераторы (MakeBlobs, MakeMoons, MakeRegression)
  /// и реальные учебные датасеты (например, RussianHousing, StudentExam).
  /// Используется в примерах, экспериментах и демонстрациях алгоритмов ML
  Datasets = static class
  public
    // --- Синтетические датасеты (Matrix + Vector)
    
    /// Генерирует синтетический датасет из нескольких гауссовых кластеров.
    /// Возвращает матрицу признаков X и вектор меток кластеров y.
    ///
    /// Параметры:
    /// • n — количество объектов (точек)
    /// • centers — число кластеров
    /// • clusterStd — стандартное отклонение точек внутри кластера
    /// • nFeatures — число признаков (размерность пространства)
    /// • seed — значение генератора случайных чисел (seed < 0 → случайный)
    static function MakeBlobs(
      n: integer := 300;
      centers: integer := 3;
      clusterStd: real := 1.0;
      nFeatures: integer := 2;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, Vector);
    
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
      seed: integer := -1): (Matrix, Vector);
    
    /// Генерирует синтетический датасет для задачи линейной регрессии.
    /// Данные создаются по модели y = Xβ + ε, где X — матрица признаков,
    /// β — случайный вектор коэффициентов, ε — гауссовский шум.
    /// Возвращает матрицу признаков X (n × nFeatures) и вектор целевой переменной y.
    ///
    /// • n — число объектов.
    /// • nFeatures — число признаков.
    /// • noise — стандартное отклонение гауссовского шума, добавляемого к y.
    /// • shuffle — перемешивать ли порядок объектов.
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время).
    static function MakeRegression(
      n: integer := 300;
      nFeatures: integer := 10;
      noise: real := 0.1;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, Vector);
    
    /// Генерирует синтетический датасет из двух концентрических окружностей.
    /// Используется для демонстрации задач классификации и кластеризации,
    /// где граница разделения является нелинейной.
    ///
    /// Датасет состоит из двух классов:
    /// внешний круг (класс 0) и внутренний круг (класс 1).
    /// При добавлении шума точки отклоняются от идеальной окружности.
    ///
    /// Полезен для демонстрации:
    /// • преимуществ нелинейных моделей (RandomForest, GradientBoosting, kNN)
    /// • работы DBSCAN и спектральной кластеризации
    /// • ограничений линейных моделей (LogisticRegression, LinearSVM).
    ///
    /// • n — число объектов.
    /// • noise — стандартное отклонение гауссовского шума, добавляемого к координатам.
    /// • factor — отношение радиуса внутреннего круга к внешнему (0 < factor < 1).
    /// • shuffle — перемешивать ли порядок объектов.
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время)
    static function MakeCircles(
      n: integer := 300;
      noise: real := 0.05;
      factor: real := 0.5;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, Vector);
    
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
      seed: integer := -1): (Matrix, Vector);    
    
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
    ///   var (X,y) := ds.ToXYInt;
    static function Iris: Dataset;
    
    /// Датасет цен на квартиры (задача регрессии)
    static function RussianHousing: Dataset;
    
    /// Датасет результатов экзамена студентов (классификация)
    static function StudentExam: Dataset;
    
    /// Датасет банковских клиентов (классификация одобрения кредита)
    static function BankClients: Dataset;
    
    /// Датасет поездок такси (регрессия стоимости поездки)
    static function TaxiTrips: Dataset;
    
    /// Датасет транспортной активности пассажиров (кластеризация)
    static function MoscowTransport: Dataset;
    
    /// Датасет интернет-покупок пользователей (классификация покупки)
    static function OnlineShopping: Dataset;
    
    static function LoadMeta(path: string): Dictionary<string,string>;
    static function ParseFeatures(meta: Dictionary<string,string>): array of string;
  end;
  

implementation

uses MLExceptions;
uses DataAdapters;

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
    'У датасета нет целевой переменной (задача кластеризации).' +
    'Dataset has no target variable (clustering task).';
  ER_DATASET_TARGET_NOT_FOUND =
    'Целевая переменная "{0}" не найдена в таблице.' +
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
    
function Normal(rnd: System.Random): real;
begin
  var u1 := rnd.NextDouble;
  var u2 := rnd.NextDouble;
  Result := Sqrt(-2 * Ln(u1)) * Cos(2 * Pi * u2);
end;

//-----------------------------
//          Dataset
//-----------------------------

function Dataset.ToX(): Matrix;
begin
  Result := Data.ToMatrix(Features);
end;

function Dataset.ToY: Vector;
begin
  var t := Data.GetColumnType(Target);
  case t of
    ColumnType.ctInt:
      Result := new Vector(Data.GetIntColumn(Target));
    ColumnType.ctFloat:
      Result := Data.ToVector(Target);
    ColumnType.ctStr:
      Result := new Vector(EncodeLabels(Data.GetStrColumn(Target)));
    ColumnType.ctBool:
      begin
        var b := Data.GetBoolColumn(Target);
        var v := new Vector(b.Length);
        
        for var i := 0 to b.Length - 1 do
          v[i] := if b[i] then 1.0 else 0.0;
        
        Result := v;
      end;
  end;
end;

function Dataset.ToXY(): (Matrix, Vector);
begin
  Result := (ToX, ToY);
end;

function Dataset.ToXYInt: (Matrix, array of integer);
begin
  if Task <> Classification then
    ArgumentError(ER_DATASET_NO_TARGET);

  var X := ToX;

  var labels := Data.GetStrColumn(Target);
  var y := EncodeLabels(labels);

  Result := (X, y);
end;

function Dataset.IsSupervised: boolean;
begin
  Result := Task <> Clustering;
end;

function Dataset.TrainTestSplit(testRatio: real; seed: integer): (Dataset, Dataset);
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'Dataset');

  var (trainDf, testDf) := Data.TrainTestSplit(testRatio, seed);

  var trainDs := new Dataset;
  trainDs.Name := Name;
  trainDs.Data := trainDf;
  trainDs.Features := Features;
  trainDs.Target := Target;
  trainDs.Task := Task;

  var testDs := new Dataset;
  testDs.Name := Name;
  testDs.Data := testDf;
  testDs.Features := Features;
  testDs.Target := Target;
  testDs.Task := Task;

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
  df.AddStrColumn('Property', props, valid, true);
  df.AddStrColumn('Value', values, valid, false);

  Result := df;
end;

//-----------------------------
//          Datasets
//-----------------------------

static function Datasets.MakeBlobs(
  n: integer; centers: integer;
  clusterStd: real; nFeatures: integer;
  shuffle: boolean; seed: integer): (Matrix, Vector);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');
  
  if centers <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'centers');
  
  if clusterStd <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'clusterStd');
  
  if nFeatures <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'nFeatures');
  
  var actualSeed :=
  if seed >= 0 then seed
  else System.Environment.TickCount and integer.MaxValue;
  
  var rnd := new System.Random(actualSeed);
  
  var X := new Matrix(n, nFeatures);
  var y := new Vector(n);
  
  // --- генерируем центры кластеров
  var centersM := new Matrix(centers, nFeatures);
  
  for var c := 0 to centers - 1 do
    for var j := 0 to nFeatures - 1 do
      centersM[c, j] := rnd.NextDouble * 20 - 10;
  
  // --- индексы строк
  var idx := Arr(0..n - 1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  // --- генерация точек
  for var i := 0 to n - 1 do
  begin
    var row := idx[i];
    
    var c := rnd.Next(centers);
    y[row] := c;
    
    for var j := 0 to nFeatures - 1 do
      X[row, j] := centersM[c, j] + clusterStd * Normal(rnd);
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeMoons(
  n: integer;
  noise: real;
  shuffle: boolean;
  seed: integer): (Matrix, Vector);
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
  var y := new Vector(n);
  
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
  noise: real;
  shuffle: boolean;
  seed: integer): (Matrix, Vector);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');
  
  if nFeatures <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'nFeatures');
  
  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');
  
  var actualSeed :=
  if seed >= 0 then seed
  else System.Environment.TickCount and integer.MaxValue;
  
  var rnd := new System.Random(actualSeed);
  
  var X := new Matrix(n, nFeatures);
  var y := new Vector(n);
  
  // --- истинные коэффициенты β
  var beta := new Vector(nFeatures);
  for var j := 0 to nFeatures - 1 do
    beta[j] := Normal(rnd);
  
  var idx := Arr(0..n - 1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  // --- генерация данных
  for var i := 0 to n - 1 do
  begin
    var row := idx[i];
    
    var s := 0.0;
    
    for var j := 0 to nFeatures - 1 do
    begin
      var xx := Normal(rnd);
      X[row, j] := xx;
      s += xx * beta[j];
    end;
    
    y[row] := s + noise * Normal(rnd);
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeCircles(n: integer; noise: real;
  factor: real; shuffle: boolean; seed: integer): (Matrix, Vector);
begin
  if n <= 0 then
    ArgumentOutOfRangeError(ER_PARAM_GT_ZERO, 'n');
  
  if noise < 0 then
    ArgumentOutOfRangeError(ER_PARAM_GE_ZERO, 'noise');
  
  if (factor <= 0) or (factor >= 1) then
    ArgumentOutOfRangeError(ER_PARAM_BETWEEN_01, 'factor');
  
  var actualSeed :=
  if seed >= 0 then seed
  else System.Environment.TickCount and integer.MaxValue;
  
  var rnd := new System.Random(actualSeed);
  
  var X := new Matrix(n, 2);
  var y := new Vector(n);
  
  var idx := Arr(0..n - 1);
  if shuffle then
    PABCSystem.Shuffle(idx, rnd);
  
  var half := n div 2;
  
  // --- внешний круг
  for var i := 0 to half - 1 do
  begin
    var row := idx[i];
    var t := 2*Pi*i/half + 0.1*Normal(rnd);
    
    y[row] := 0;
    
    X[row, 0] := Cos(t);
    X[row, 1] := Sin(t);
    
    if noise > 0 then
    begin
      X[row, 0] += noise * Normal(rnd);
      X[row, 1] += noise * Normal(rnd);
    end;
  end;
  
  // --- внутренний круг
  for var i := half to n - 1 do
  begin
    var row := idx[i];
    var t := 2*Pi*(i-half)/half + 0.1*Normal(rnd);
    
    y[row] := 1;
    
    X[row, 0] := factor * Cos(t);
    X[row, 1] := factor * Sin(t);
    
    if noise > 0 then
    begin
      X[row, 0] += noise * Normal(rnd);
      X[row, 1] += noise * Normal(rnd);
    end;
  end;
  
  Result := (X, y);
end;

static function Datasets.MakeSpiral(
  n: integer; classes: integer;
  noise: real; turns: real; radius: real;
  shuffle: boolean; seed: integer): (Matrix, Vector);
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
  var y := new Vector(n);

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

  var metaPath := baseDir + name + '.meta';
  var csvPath  := baseDir + name + '.csv';

  if not FileExists(metaPath) then
    ArgumentError(ER_DATASET_META_NOT_FOUND, name);
  
  if not FileExists(csvPath) then
    ArgumentError(ER_DATASET_CSV_NOT_FOUND, name);
  
  var meta := LoadMeta(metaPath);

  if not meta.ContainsKey('task') then
    ArgumentError(ER_DATASET_TASK_MISSING, name);

  var df := DataFrame.FromCsv(csvPath);

  var ds := new Dataset;

  ds.Name := name;
  ds.Data := df;

  ds.Task := ParseTask(meta['task']);
  ds.Target := meta['target'];
  ds.Features := ParseFeatures(meta);
  
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

static function Datasets.RussianHousing: Dataset;
begin
  Result := nil;
end;

static function Datasets.StudentExam: Dataset;
begin
  Result := nil;
end;

static function Datasets.BankClients: Dataset;
begin
  Result := nil;
end;

static function Datasets.TaxiTrips: Dataset;
begin
  Result := nil;
end;

static function Datasets.MoscowTransport: Dataset;
begin
  Result := nil;
end;

static function Datasets.OnlineShopping: Dataset;
begin
  Result := nil;
end;

static function Datasets.LoadMeta(path: string): Dictionary<string,string>;
begin
  if path = nil then
    ArgumentNullError(ER_ARG_NULL, 'path');

  if not FileExists(path) then
    ArgumentError(ER_DATASET_META_NOT_FOUND, path);

  var dict := new Dictionary<string,string>;

  foreach var line in ReadLines(path) do
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