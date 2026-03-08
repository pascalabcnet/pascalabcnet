unit MLDatasets;

interface

uses DataFrameABC, LinearAlgebraML;

type
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
      seed: integer := -1
    ): (Matrix, Vector);
    
    /// Генерирует датасет "две луны" для демонстрации нелинейной классификации и кластеризации
    static function MakeMoons(
      n: integer := 300;
      noise: real := 0.05;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует синтетический датасет для задач регрессии
    static function MakeRegression(
      n: integer := 300;
      noise: real := 0.1;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует концентрические круги (сложная нелинейная классификация)
    static function MakeCircles(
      n: integer := 300;
      noise: real := 0.05;
      seed: integer := 1): (Matrix, Vector);
    
    /// Генерирует спиральный датасет (очень сложная нелинейная классификация)
    static function MakeSpiral(
      n: integer := 300;
      classes: integer := 2;
      seed: integer := 1): (Matrix, Vector);
    
    
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
  clusterStd: real; nFeatures: integer; seed: integer): (Matrix, Vector);
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
      centersM[c,j] := rnd.NextDouble * 20 - 10;

  // --- генерация точек
  for var i := 0 to n - 1 do
  begin
    var c := rnd.Next(centers);
    y[i] := c;

    for var j := 0 to nFeatures - 1 do
      X[i,j] := centersM[c,j] + clusterStd * Normal(rnd);
  end;

  Result := (X, y);
end;

static function Datasets.MakeMoons(
      n: integer ;
      noise: real ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeRegression(
      n: integer ;
      noise: real ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeCircles(
      n: integer ;
      noise: real ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
end;

static function Datasets.MakeSpiral(
      n: integer ;
      classes: integer ;
      seed: integer): (Matrix, Vector);
begin
  Result := nil;
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