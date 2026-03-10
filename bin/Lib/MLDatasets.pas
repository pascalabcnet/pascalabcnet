unit MLDatasets;

interface

uses DataFrameABC, LinearAlgebraML;

type
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
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время).
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
    /// • seed — значение генератора случайных чисел (-1 означает использовать текущее время).
    static function MakeSpiral(
      n: integer := 300;
      classes: integer := 2;
      noise: real := 0.1;
      turns: real := 3.0;
      radius: real := 1.0;
      shuffle: boolean := true;
      seed: integer := -1): (Matrix, Vector);    
    
    // --- DataFrame датасеты (реалистичные таблицы, считываемые из csv)
    
    /// Датасет цен на квартиры (задача регрессии)
    static function RussianHousing: DataFrame;
    
    /// Датасет результатов экзамена студентов (классификация)
    static function StudentExam: DataFrame;
    
    /// Датасет банковских клиентов (классификация одобрения кредита)
    static function BankClients: DataFrame;
    
    /// Датасет поездок такси (регрессия стоимости поездки)
    static function TaxiTrips: DataFrame;
    
    /// Датасет транспортной активности пассажиров (кластеризация)
    static function MoscowTransport: DataFrame;
    
    /// Датасет интернет-покупок пользователей (классификация покупки)
    static function OnlineShopping: DataFrame;
  
  end;

implementation

uses MLExceptions;

const
  ER_PARAM_GT_ZERO =
    'Параметр {0} должен быть > 0!!Parameter {0} must be > 0';
  ER_PARAM_GT_ONE =
    'Параметр {0} должен быть > 1!!Parameter {0} must be > 1';
  ER_PARAM_GE_ZERO =
    'Параметр {0} должен быть >= 0!!Parameter {0} must be >= 0';
  ER_PARAM_BETWEEN_01 =
    'Параметр {0} должен быть в диапазоне (0,1)!!Parameter {0} must be in range (0,1)';
  
function Normal(rnd: System.Random): real;
begin
  var u1 := rnd.NextDouble;
  var u2 := rnd.NextDouble;
  Result := Sqrt(-2 * Ln(u1)) * Cos(2 * Pi * u2);
end;

/// Генерирует синтетический датасет из нескольких гауссовых кластеров.
/// Возвращает матрицу признаков X и вектор меток кластеров y.
///
/// Параметры:
/// • n — количество объектов (точек)
/// • centers — число кластеров
/// • clusterStd — стандартное отклонение точек внутри кластера
/// • nFeatures — число признаков (размерность пространства)
/// • seed — значение генератора случайных чисел (seed < 0 → случайный)
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

static function Datasets.RussianHousing: DataFrame;
begin
  Result := nil;
end;

static function Datasets.StudentExam: DataFrame;
begin
  Result := nil;
end;

static function Datasets.BankClients: DataFrame;
begin
  Result := nil;
end;

static function Datasets.TaxiTrips: DataFrame;
begin
  Result := nil;
end;

static function Datasets.MoscowTransport: DataFrame;
begin
  Result := nil;
end;

static function Datasets.OnlineShopping: DataFrame;
begin
  Result := nil;
end;


end.