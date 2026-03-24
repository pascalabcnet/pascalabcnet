/// Линейная алгебра для алгоритмов машинного обучения.
///
/// Содержит типы Vector и Matrix и численные методы,
/// используемые в моделях ML:
/// решение систем линейных уравнений, least squares,
/// ridge-регрессия и специализированные методы
/// для симметричных положительно определённых матриц.
///
/// Модуль предназначен для внутреннего использования
/// в ML-алгоритмах и оптимизирован для задач обучения моделей.
unit LinearAlgebraML;

interface

type
  Vector = class
  private
    fdata: array of real;
    static procedure CheckSameLength(const a, b: Vector);
    static procedure CheckNonEmpty(const v: Vector); 
    procedure SetData(i: integer; value: real) := fdata[i] := value;
  public
    property Data: array of real read fdata;
    property Length: integer read fdata.Length;
    property Item[i: integer]: real read fdata[i] write SetData; default;
    
    constructor Create(n: integer);
    constructor Create(values: array of real);
    constructor Create(values: array of integer);
    
    function Clone: Vector;
    
    function ToArray: array of real;
    function ToIntArray: array of integer;
    
    static function operator +(a, b: Vector): Vector;
    static function operator -(a, b: Vector): Vector;
    
    static function operator *(alpha: real; v: Vector): Vector;
    static function operator *(v: Vector; alpha: real): Vector;
    static function operator /(v: Vector; alpha: real): Vector;
    
    static function operator +=(a: Vector; b: Vector): Vector;
    static function operator -=(a: Vector; b: Vector): Vector;
    static function operator *=(a: Vector; alpha: real): Vector;
    
    static function operator +(v: Vector; c: real): Vector;
    static function operator +(c: real; v: Vector): Vector;
    
    static function operator implicit(a: array of real): Vector := new Vector(a);
    static function operator implicit(a: array of integer): Vector := new Vector(a);

    // ---------- Векторные функции ----------
    /// Применить функцию ко всем элементам вектора
    function Apply(f: real -> real): Vector;
    function Sqrt: Vector;
    function Exp: Vector := Apply(PABCSystem.Exp);
    function Ln: Vector := Apply(PABCSystem.Ln);
    function Abs: Vector := Apply(PABCSystem.Abs);    
    
    // ---------- Основные методы ----------
    function Sum: real;
    function Average: real;
    function Mean: real;
    function Norm2: real;
    function Norm: real;
    function Max: real;
    function Min: real;
    /// Скалярное произведение
    function Dot(b: Vector): real;
    
    // ---------- Сервисные методы ----------
    function ToString: string; override := $'{fdata.Select(x -> x.ToString(''G3''))}';
    function ToString(digits: integer): string := $'{fdata.Select(x -> x.ToString(''G''+digits))}';
    procedure Print := fdata.Print;
    procedure Println := fdata.Println;
    
    function SubvectorBy(indices: array of integer): Vector;
  end;
  
  Matrix = class
  private
    fdata: array[,] of real;

    static procedure CheckSameSize(A, B: Matrix);
    static procedure CheckMulSize(A, B: Matrix);
    static procedure CheckVecSize(A: Matrix; x: Vector);
    
    procedure SetData(i, j: integer; value: real) := fdata[i, j] := value;
  public
    property RowCount: integer read fdata.RowCount;
    property ColCount: integer read fdata.ColCount;
    property Data: array[,] of real read fdata;
    
    property Item[i, j: integer]: real
      read fdata[i, j] write SetData; default;
    
    constructor Create(r, c: integer);
    constructor Create(values: array[,] of real);
    
    function ToArray2D: array[,] of real;
    function RowToArray(r: integer): array of real;
    
    function Clone: Matrix;
    
    function Row(i: integer): array of real := fdata.Row(i);
    function Col(j: integer): array of real := fdata.Col(j);

    function ColumnSums: Vector;
    function RowSums: Vector;
    function ColumnMeans: Vector;
    function RowMeans: Vector;
    function ColumnVariances: Vector;
    function RowVariances: Vector;
    function ColumnStd: Vector;
    function RowStd: Vector;
    function ColumnMins: Vector;
    function ColumnMaxs: Vector;
    function RowMins: Vector;
    function RowMaxs: Vector;
    
    function RowSum(i: integer): real;
    function RowMean(i: integer): real;
    function RowVariance(i: integer): real;
    function RowStd(i: integer): real;
    function RowMin(i: integer): real;
    function RowMax(i: integer): real;
    function RowArgMin(i: integer): integer;
    function RowArgMax(i: integer): integer;
    
    function ColumnSum(j: integer): real;
    function ColumnMean(j: integer): real;
    function ColumnVariance(j: integer): real;
    function ColumnStd(j: integer): real;
    function ColumnMin(j: integer): real;
    function ColumnMax(j: integer): real;
    function ColumnArgMin(j: integer): integer;
    function ColumnArgMax(j: integer): integer;
    
    function FrobeniusNorm: real;
    procedure AddScaledIdentity(lambda: real);

    
    static function operator implicit(a: array [,] of real): Matrix := new Matrix(a);
    static function operator implicit(a: array of array of real): Matrix := new Matrix(Matr(a));
    static function operator implicit(a: array of array of integer): Matrix := new Matrix(Matr(a.ConvertAll(x -> x.ConvertAll(y -> real(y)))));
    
    // ---------- value operators ----------
    static function operator +(A, B: Matrix): Matrix;
    static function operator -(A, B: Matrix): Matrix;
    
    static function operator *(A: Matrix; x: Vector): Vector;
    static function operator *(A, B: Matrix): Matrix;
    
    //static function operator *(alpha: real; A: Matrix): Matrix;
    //static function operator *(A: Matrix; alpha: real): Matrix;
    
    // ---------- in-place operators ----------
    static function operator +=(A, B: Matrix): Matrix;
    static function operator -=(A, B: Matrix): Matrix;
    static function operator *=(A: Matrix; alpha: real): Matrix;
    
    // ---------- Основные методы ----------
    
    /// Возвращает транспонированную матрицу Aᵀ
    function Transpose: Matrix;
    /// Проверяет, является ли матрица симметричной с заданным допуском.
    function IsSymmetric(tol: real := 1e-12): boolean;
    /// Вычисляет собственные значения и собственные векторы вещественной квадратной симметричной матрицы.
    ///
    /// Возвращает:
    /// - values  — вектор длины n, содержащий собственные значения,
    ///             отсортированные по убыванию;
    /// - vectors — матрицу n × n, столбцы которой являются
    ///             ортонормированными собственными векторами.
    ///
    /// Выполняется разложение:
    ///     A = V * diag(values) * Vᵀ
    ///
    /// Метод основан на итерациях Якоби.    
    function EigenSymmetric(tol: real := 1e-12; maxIter: integer := 100): (Vector, Matrix);
    /// Выполняет анализ главных компонент (PCA) по матрице данных.
    ///
    /// Строки интерпретируются как объекты, столбцы — как признаки.
    /// Параметр k задаёт число главных компонент (1 ≤ k ≤ Cols).
    ///
    /// Возвращает:
    /// - components — матрицу Cols × k главных компонент;
    /// - variances  — соответствующие дисперсии.
    ///
    /// Компоненты ортонормированы и отсортированы по убыванию дисперсии.
    function PCA(k: integer): (Matrix, Vector);
    
    // ---------- Сервисные методы ----------
    function ToString: string; override := $'{fdata}';
    procedure Print := fdata.Print;
    procedure Println := fdata.Println;
    
    function GetRow(i: integer): Vector;
    function GetCol(j: integer): Vector;

    // ---------- Статические методы ----------
    /// Возвращает единичную матрицу размера n
    static function Identity(n: integer): Matrix;
    /// Возвращает внешнее произведение двух векторов
    static function OuterProduct(a, b: Vector): Matrix;
    
  end;

/// Решает систему линейных уравнений A * x = b с помощью LU-разложения с частичным выбором главного элемента.
/// A должна быть квадратной матрицей. 
///
/// Временная сложность: O(n³).
///
/// Вызывает исключение, если A вырождена или размеры не согласованы
function Solve(A: Matrix; b: Vector): Vector;

/// Решает систему линейных уравнений A * x = b, предполагая, что матрица A является
/// симметричной положительно определённой (SPD).
/// Используется разложение Холецкого (A = L * L^T).
///
/// Временная сложность: O(n³ / 3).
///
/// Вызывает исключение, если матрица A не симметричная положительно определенная
/// или если размеры A и b не согласованы.
function SolveSPD(A: Matrix; b: Vector): Vector;

/// Решает систему линейных уравнений A * x = b.
///
/// Метод автоматически выбирается для обеспечения
/// максимальной производительности и численной устойчивости.
///
/// Вызывает исключение, если система неразрешима
/// или размеры A и b не согласованы.
function SolveAuto(A: Matrix; b: Vector): Vector;


/// Решает систему A * x ≈ b методом ridge-регрессии с подавлением неустойчивых направлений.
/// 
/// Матрица A имеет размер m × n: m — число уравнений (строк A), n — число неизвестных (столбцов A).
///
/// Метод используется, когда обычное решение (lambda = 0) даёт слишком большие коэффициенты из-за  
/// сильной зависимости между столбцами матрицы A или из-за шума в данных.
///
/// Параметр lambda задаёт степень регуляризации:
/// - lambda = 0
///     эквивалентно обычному решению по методу наименьших квадратов;
/// - lambda ≈ 1e-6 .. 1e-3
///     слабая регуляризация;
/// - lambda ≈ 1e-2 .. 1
///     умеренная регуляризация (типично для практических и ML-задач);
/// - lambda > 1
///     сильная регуляризация, коэффициенты заметно уменьшаются.
///
/// Если данные хорошо обусловлены, используйте lambda = 0
/// При шумных или коррелированных данных часто подходят значения 0.01 .. 0.1
/// Увеличивайте lambda, если коэффициенты x становятся слишком большими.
///
/// Требуется, чтобы число уравнений было не меньше числа неизвестных (m ≥ n).
/// Входные A и b изменяются. Возвращаемый вектор x имеет длину n.
function SolveRidge(A: Matrix; b: Vector; lambda: real := 0): Vector;

/// Решает задачу наименьших квадратов для системы A * x ≈ b.
///
/// Используется QR-разложение матрицы A.
/// Подходит для переопределённых систем (число строк больше числа столбцов).
///
/// Временная сложность: O(m * n²), где
/// m — число строк A,
/// n — число столбцов A.
///
/// Вызывает исключение, если размеры A и b не согласованы.
function SolveLeastSquaresQR(A: Matrix; b: Vector): Vector;

implementation

uses MLExceptions;

const
  ER_VECTOR_LENGTH_NEGATIVE =
    'Длина вектора должна быть неотрицательной!!Vector length must be non-negative';
  ER_VALUES_NULL =
    'values не может быть nil!!values cannot be nil';
  ER_VECTOR_LENGTH_MISMATCH =
    'Несоответствие длины векторов: {0} и {1}!!Vector length mismatch: {0} vs {1}';
  ER_VECTOR_EMPTY =
    'Вектор пуст!!Vector is empty';
  ER_VECTOR_DIVIDE_BY_ZERO =
    'Деление на ноль при делении вектора на скаляр!!Division by zero in Vector / scalar';
  ER_DIM_MISMATCH =
    'Несоответствие размерностей: {0} и {1}!!Dimension mismatch: {0} and {1}';
  ER_MATRIX_SIZE_NEGATIVE =
    'Размеры матрицы должны быть неотрицательными!!Matrix size must be non-negative';
  ER_MATRIX_SIZE_MISMATCH =
    'Несоответствие размеров матриц: {0}x{1} и {2}x{3}!!' +
    'Matrix size mismatch: {0}x{1} vs {2}x{3}';
  ER_MATRIX_MUL_SIZE_MISMATCH =
    'Несоответствие размеров при умножении матриц: {0}x{1} * {2}x{3}!!' +
    'Matrix multiply size mismatch: {0}x{1} * {2}x{3}';
  ER_MATRIX_VECTOR_SIZE_MISMATCH =
    'Несоответствие размеров при умножении матрицы на вектор: {0}x{1} * {2}!!' +
    'Matrix-vector size mismatch: {0}x{1} * {2}';
  ER_ROW_INDEX_OUT_OF_RANGE =
    'Индекс строки {0} вне диапазона [0..{1})!!Row index {0} out of range [0..{1})';
  ER_COL_INDEX_OUT_OF_RANGE =
    'Индекс столбца {0} вне диапазона [0..{1})!!Column index {0} out of range [0..{1})';
  ER_MATRIX_NOT_SQUARE =
    'Матрица должна быть квадратной!!Matrix must be square';
  ER_MATRIX_NOT_SYMMETRIC =
    'Матрица должна быть симметричной!!Matrix must be symmetric';
  ER_PCA_K_INVALID =
    'k должно быть >= 1!!k must be >= 1';
  ER_PCA_K_TOO_LARGE =
    'k не может превышать число признаков!!k cannot exceed number of features';
  ER_PCA_NEED_TWO_SAMPLES =
    'Требуется как минимум два объекта!!At least two samples required';
  ER_CHOLESKY_NOT_SQUARE =
    'Матрица должна быть квадратной для Cholesky!!Matrix must be square for Cholesky';
  ER_MATRIX_NOT_SPD =
    'Матрица не является положительно определённой (SPD)!!Matrix is not SPD';
  ER_MATRIX_SINGULAR =
    'Матрица вырождена!!Matrix is singular';
  ER_VECTOR_SIZE_MISMATCH =
    'Несоответствие длины вектора: {0} и {1}!!Vector size mismatch: {0} and {1}';
  ER_QR_REQUIRES_M_GE_N =
    'Для QR-разложения требуется m >= n!!QR decomposition requires m >= n';
  ER_SINGULAR_MATRIX =
    'Матрица вырождена или плохо обусловлена!!Matrix is singular or ill-conditioned';
  
//-----------------------------
//           Vector
//-----------------------------

constructor Vector.Create(n: integer);
begin
  if n < 0 then
    ArgumentOutOfRangeError(ER_VECTOR_LENGTH_NEGATIVE);
  fdata := new real[n];
end;

constructor Vector.Create(values: array of real);
begin
  if values = nil then
    ArgumentNullError(ER_VALUES_NULL);
  fdata := Copy(values);
end;

constructor Vector.Create(values: array of integer);
begin
  if values = nil then
    ArgumentNullError(ER_VALUES_NULL);
  fdata := values.Select(x -> real(x)).ToArray;
end;

function Vector.ToArray: array of real;
begin
  Result := Copy(fdata);
end;

function Vector.ToIntArray: array of integer;
begin
  Result := new integer[Length];
  
  for var i := 0 to Length - 1 do
    Result[i] := integer(Data[i]);
end;

function Vector.Clone: Vector;
begin
  Result := new Vector(fdata);
end;

static procedure Vector.CheckSameLength(a, b: Vector);
begin
  if a.Length <> b.Length then
    ArgumentError(ER_VECTOR_LENGTH_MISMATCH, a.Length, b.Length);
end;

static procedure Vector.CheckNonEmpty(v: Vector);
begin
  if v.Length = 0 then
    ArgumentError(ER_VECTOR_EMPTY);
end;

static function Vector.operator +(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  Result := new Vector(a.Length);
  for var i := 0 to a.Length - 1 do
    Result.fdata[i] := a.fdata[i] + b.fdata[i];
end;

static function Vector.operator -(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  Result := new Vector(a.Length);
  for var i := 0 to a.Length - 1 do
    Result.fdata[i] := a.fdata[i] - b.fdata[i];
end;

static function Vector.operator *(v: Vector; alpha: real): Vector;
begin
  Result := new Vector(v.Length);
  for var i := 0 to v.Length - 1 do
    Result.fdata[i] := alpha * v.fdata[i];
end;

static function Vector.operator *(alpha: real; v: Vector): Vector;
begin
  Result := v * alpha;
end;

static function Vector.operator /(v: Vector; alpha: real): Vector;
begin
  if alpha = 0.0 then
    raise new System.DivideByZeroException(GetTranslation(ER_VECTOR_DIVIDE_BY_ZERO));
  Result := new Vector(v.Length);
  var inv := 1.0 / alpha;
  for var i := 0 to v.Length - 1 do
    Result.fdata[i] := v.fdata[i] * inv;
end;

static function Vector.operator +=(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  for var i := 0 to a.Length - 1 do
    a.fdata[i] += b.fdata[i];
  Result := a;
end;

static function Vector.operator -=(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  for var i := 0 to a.Length - 1 do
    a.fdata[i] -= b.fdata[i];
  Result := a;
end;

static function Vector.operator *=(a: Vector; alpha: real): Vector;
begin
  for var i := 0 to a.Length - 1 do
    a.fdata[i] *= alpha;
  Result := a;
end;

static function Vector.operator +(v: Vector; c: real): Vector;
begin
  Result := new Vector(v.Length);
  for var i := 0 to v.Length - 1 do
    Result[i] := v[i] + c;
end;

static function Vector.operator +(c: real; v: Vector): Vector;
begin
  Result := v + c;
end;

function Vector.Sqrt: Vector := Apply(PABCSystem.Sqrt);

function Vector.Sum: real;
begin
  var s := 0.0;
  for var i := 0 to Length - 1 do
    s += fdata[i];
  Result := s;
end;

function Vector.Average: real;
begin
  if Length = 0 then
    ArgumentError('Vector is empty');

  Result := Sum / Length;
end;

function Vector.Mean: real;
begin
  CheckNonEmpty(Self);
  Result := Sum / Length;
end;

function Vector.Norm2: real;
begin
  Result := Self.Dot(Self);
end;

function Vector.Norm: real := PABCSystem.Sqrt(Norm2);

function Vector.Max: real := fdata.Max;

function Vector.Min: real := fdata.Min;

function Vector.Dot(b: Vector): real;
begin
  if Length <> b.Length then
    DimensionError(ER_DIM_MISMATCH, Length, b.Length);

  var s := 0.0;
  for var i := 0 to Length - 1 do
    s += self[i] * b[i];
  Result := s;
end;

function Vector.Apply(f: real -> real): Vector;
begin
  Result := new Vector(Length);
  for var i := 0 to Length - 1 do
    Result[i] := f(self[i]);
end;

function Vector.SubvectorBy(indices: array of integer): Vector;
begin
  var n := indices.Length;
  var resultVec := new Vector(n);

  for var i := 0 to n - 1 do
    resultVec[i] := self[indices[i]];

  Result := resultVec;
end;


//-----------------------------
//           Matrix
//-----------------------------
constructor Matrix.Create(r, c: integer);
begin
  if (r < 0) or (c < 0) then
    ArgumentOutOfRangeError(ER_MATRIX_SIZE_NEGATIVE);
  fdata := new real[r, c];
end;

constructor Matrix.Create(values: array[,] of real);
begin
  if values = nil then
    ArgumentNullError(ER_VALUES_NULL);
  fdata := Copy(values);
end;

function Matrix.ToArray2D: array[,] of real;
begin
  Result := Copy(fdata);
end;

function Matrix.RowToArray(r: integer): array of real;
begin
  Result := fdata.Row(r);
end;

function Matrix.Clone: Matrix;
begin
  Result := new Matrix(fdata);
end;

function Matrix.ColumnSums: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(p);

  for var j := 0 to p - 1 do
    for var i := 0 to n - 1 do
      Result[j] += fdata[i, j];
end;

function Matrix.ColumnMeans: Vector;
begin
  var n := RowCount;
  if n = 0 then 
    exit( new Vector(ColCount) );
  Result := ColumnSums / n;
end;

function Matrix.RowSums: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
      Result[i] += fdata[i, j];
end;

function Matrix.RowMeans: Vector;
begin
  var p := ColCount;
  if p = 0 then
    exit (new Vector(RowCount));
  Result := RowSums / p;
end;

function Matrix.ColumnVariances: Vector;
begin
  var n := RowCount;
  if n = 0 then
    exit (new Vector(ColCount));

  var means := ColumnMeans;
  var p := ColCount;
  Result := new Vector(p);

  for var j := 0 to p - 1 do
    for var i := 0 to n - 1 do
    begin
      var d := fdata[i, j] - means[j];
      Result[j] += d * d;
    end;

  Result := Result / n;
end;

function Matrix.ColumnStd: Vector;
begin
  Result := ColumnVariances.Sqrt;
end;

function Matrix.RowVariances: Vector;
begin
  var p := ColCount;
  if p = 0 then
    exit (new Vector(RowCount));

  var means := RowMeans;
  var n := RowCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
    for var j := 0 to p - 1 do
    begin
      var d := fdata[i, j] - means[i];
      Result[i] += d * d;
    end;

  Result := Result / p;
end;

function Matrix.RowStd: Vector;
begin
  Result := RowVariances.Sqrt;
end;

function Matrix.ColumnMins: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(p);

  for var j := 0 to p - 1 do
  begin
    Result[j] := fdata[0, j];

    for var i := 1 to n - 1 do
      if fdata[i, j] < Result[j] then
        Result[j] := fdata[i, j];
  end;
end;

function Matrix.ColumnMaxs: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(p);

  for var j := 0 to p - 1 do
  begin
    Result[j] := fdata[0, j];

    for var i := 1 to n - 1 do
      if fdata[i, j] > Result[j] then
        Result[j] := fdata[i, j];
  end;
end;

function Matrix.RowMins: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    Result[i] := fdata[i, 0];

    for var j := 1 to p - 1 do
      if fdata[i, j] < Result[i] then
        Result[i] := fdata[i, j];
  end;
end;

function Matrix.RowMaxs: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    Result[i] := fdata[i, 0];

    for var j := 1 to p - 1 do
      if fdata[i, j] > Result[i] then
        Result[i] := fdata[i, j];
  end;
end;

function Matrix.RowArgMin(i: integer): integer;
begin
  var minVal := fdata[i,0];
  var arg := 0;

  for var j := 1 to ColCount - 1 do
    if fdata[i,j] < minVal then
    begin
      minVal := fdata[i,j];
      arg := j;
    end;

  Result := arg;
end;

function Matrix.RowMin(i: integer): real;
begin
  Result := fdata[i, RowArgMin(i)];
end;

function Matrix.RowArgMax(i: integer): integer;
begin
  var maxVal := fdata[i,0];
  var arg := 0;

  for var j := 1 to ColCount - 1 do
    if fdata[i,j] > maxVal then
    begin
      maxVal := fdata[i,j];
      arg := j;
    end;

  Result := arg;
end;

function Matrix.RowMax(i: integer): real;
begin
  Result := fdata[i, RowArgMax(i)];
end;

function Matrix.RowSum(i: integer): real;
begin
  var sum := 0.0;

  for var j := 0 to ColCount - 1 do
    sum += fdata[i,j];

  Result := sum;
end;

function Matrix.RowMean(i: integer): real;
begin
  Result := RowSum(i) / ColCount;
end;

function Matrix.RowVariance(i: integer): real;
begin
  var mean := RowMean(i);
  var sum := 0.0;

  for var j := 0 to ColCount - 1 do
  begin
    var d := fdata[i,j] - mean;
    sum += d * d;
  end;

  Result := sum / ColCount;
end;

function Matrix.RowStd(i: integer): real;
begin
  Result := Sqrt(RowVariance(i));
end;

function Matrix.ColumnArgMin(j: integer): integer;
begin
  var minVal := fdata[0,j];
  var arg := 0;

  for var i := 1 to RowCount - 1 do
    if fdata[i,j] < minVal then
    begin
      minVal := fdata[i,j];
      arg := i;
    end;

  Result := arg;
end;

function Matrix.ColumnMin(j: integer): real;
begin
  Result := fdata[ColumnArgMin(j), j];
end;

function Matrix.ColumnArgMax(j: integer): integer;
begin
  var maxVal := fdata[0,j];
  var arg := 0;

  for var i := 1 to RowCount - 1 do
    if fdata[i,j] > maxVal then
    begin
      maxVal := fdata[i,j];
      arg := i;
    end;

  Result := arg;
end;

function Matrix.ColumnMax(j: integer): real;
begin
  Result := fdata[ColumnArgMax(j), j];
end;

function Matrix.ColumnSum(j: integer): real;
begin
  var sum := 0.0;

  for var i := 0 to RowCount - 1 do
    sum += fdata[i,j];

  Result := sum;
end;

function Matrix.ColumnMean(j: integer): real;
begin
  Result := ColumnSum(j) / RowCount;
end;

function Matrix.ColumnVariance(j: integer): real;
begin
  var mean := ColumnMean(j);
  var sum := 0.0;

  for var i := 0 to RowCount - 1 do
  begin
    var d := fdata[i,j] - mean;
    sum += d * d;
  end;

  Result := sum / RowCount;
end;

function Matrix.ColumnStd(j: integer): real;
begin
  Result := Sqrt(ColumnVariance(j));
end;


function Matrix.FrobeniusNorm: real;
begin
  var s := 0.0;
  for var i := 0 to RowCount - 1 do
    for var j := 0 to ColCount - 1 do
      s += fdata[i, j] * fdata[i, j];
  Result := PABCSystem.Sqrt(s);
end;

procedure Matrix.AddScaledIdentity(lambda: real);
begin
  var n := RowCount;
  for var i := 0 to n - 1 do
    fdata[i, i] += lambda;
end;

static procedure Matrix.CheckSameSize(A, B: Matrix);
begin
  if (A.RowCount <> B.RowCount) or (A.ColCount <> B.ColCount) then
    DimensionError(ER_MATRIX_SIZE_MISMATCH, A.RowCount, A.ColCount, B.RowCount, B.ColCount);
end;

static procedure Matrix.CheckMulSize(A, B: Matrix);
begin
  if A.ColCount <> B.RowCount then
    DimensionError(ER_MATRIX_MUL_SIZE_MISMATCH, A.RowCount, A.ColCount, B.RowCount, B.ColCount);
end;

static procedure Matrix.CheckVecSize(A: Matrix; x: Vector);
begin
  if A.ColCount <> x.Length then
    DimensionError(ER_MATRIX_VECTOR_SIZE_MISMATCH, A.RowCount, A.ColCount, x.Length);
end;



static function Matrix.operator +(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  Result := new Matrix(A.RowCount, A.ColCount);
  for var i := 0 to A.RowCount - 1 do
    for var j := 0 to A.ColCount - 1 do
      Result.fdata[i, j] := A.fdata[i, j] + B.fdata[i, j];
end;

static function Matrix.operator -(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  Result := new Matrix(A.RowCount, A.ColCount);
  for var i := 0 to A.RowCount - 1 do
    for var j := 0 to A.ColCount - 1 do
      Result.fdata[i, j] := A.fdata[i, j] - B.fdata[i, j];
end;

function operator *(A: Matrix; alpha: real): Matrix; extensionmethod;
begin
  Result := new Matrix(A.RowCount, A.ColCount);
  for var i := 0 to A.RowCount - 1 do
    for var j := 0 to A.ColCount - 1 do
      Result.fdata[i, j] := alpha * A.fdata[i, j];
end;

function operator *(alpha: real; A: Matrix): Matrix; extensionmethod;
begin
  Result := A * alpha;
end;

function operator *(A: Matrix; alpha: integer): Matrix; extensionmethod;
begin
  Result := A * real(alpha);
end;

function operator *(alpha: integer; A: Matrix): Matrix; extensionmethod;
begin
  Result := real(alpha) * A;
end;



static function Matrix.operator *(A: Matrix; x: Vector): Vector;
begin
  CheckVecSize(A, x);
  Result := new Vector(A.RowCount);
  for var i := 0 to A.RowCount - 1 do
  begin
    var s := 0.0;
    for var j := 0 to A.ColCount - 1 do
      s += A.fdata[i, j] * x[j];
    Result[i] := s;
  end;
end;

static function Matrix.operator *(A, B: Matrix): Matrix;
begin
  CheckMulSize(A, B);
  Result := new Matrix(A.RowCount, B.ColCount);
  for var i := 0 to A.RowCount - 1 do
    for var k := 0 to A.ColCount - 1 do
    begin
      var aik := A.fdata[i, k];
      if aik <> 0.0 then
        for var j := 0 to B.ColCount - 1 do
          Result.fdata[i, j] += aik * B.fdata[k, j];
    end;
end;

static function Matrix.operator +=(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  for var i := 0 to A.RowCount - 1 do
    for var j := 0 to A.ColCount - 1 do
      A.fdata[i, j] += B.fdata[i, j];
  Result := A;
end;

static function Matrix.operator -=(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  for var i := 0 to A.RowCount - 1 do
    for var j := 0 to A.ColCount - 1 do
      A.fdata[i, j] -= B.fdata[i, j];
  Result := A;
end;

static function Matrix.operator *=(A: Matrix; alpha: real): Matrix;
begin
  for var i := 0 to A.RowCount - 1 do
    for var j := 0 to A.ColCount - 1 do
      A.fdata[i, j] *= alpha;
  Result := A;
end;

function Matrix.Transpose: Matrix;
begin
  Result := new Matrix(ColCount, RowCount);
  for var i := 0 to RowCount - 1 do
    for var j := 0 to ColCount - 1 do
      Result.fdata[j, i] := fdata[i, j];
end;

function Matrix.GetRow(i: integer): Vector;
begin
  if (i < 0) or (i >= RowCount) then
    ArgumentOutOfRangeError(ER_ROW_INDEX_OUT_OF_RANGE, i, RowCount);
  Result := new Vector(ColCount);
  for var j := 0 to ColCount - 1 do
    Result[j] := fdata[i, j];
end;

function Matrix.GetCol(j: integer): Vector;
begin
  if (j < 0) or (j >= ColCount) then
    ArgumentOutOfRangeError(ER_COL_INDEX_OUT_OF_RANGE, j, ColCount);
  Result := new Vector(RowCount);
  for var i := 0 to RowCount - 1 do
    Result[i] := fdata[i, j];
end;

static function Matrix.Identity(n: integer): Matrix;
begin
  if n < 0 then
    ArgumentOutOfRangeError(ER_MATRIX_SIZE_NEGATIVE);
  
  Result := new Matrix(n, n);
  for var i := 0 to n - 1 do
    Result[i, i] := 1.0;
end;

static function Matrix.OuterProduct(a, b: Vector): Matrix;
begin
  var m := a.Length;
  var n := b.Length;
  Result := new Matrix(m, n);
  
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      Result[i, j] := a[i] * b[j];
end;

function Matrix.IsSymmetric(tol: real): boolean;
begin
  if RowCount <> ColCount then
  begin
    Result := false;
    exit;
  end;
  
  for var i := 0 to RowCount - 1 do
    for var j := i + 1 to ColCount - 1 do
      if Abs(fdata[i, j] - fdata[j, i]) > tol then
      begin
        Result := false;
        exit;
      end;
  
  Result := true;
end;

function Matrix.EigenSymmetric(tol: real; maxIter: integer): (Vector, Matrix);
begin
  if RowCount <> ColCount then
    ArgumentError(ER_MATRIX_NOT_SQUARE);
  
  if not IsSymmetric(tol) then
    ArgumentError(ER_MATRIX_NOT_SYMMETRIC);
  
  var n := RowCount;
  
  var M := Clone;
  var V := Matrix.Identity(n);
  
  for var iter := 0 to maxIter - 1 do
  begin
    // --- Норма вне-диагонали
    var off := 0.0;
    for var i := 0 to n - 1 do
      for var j := i + 1 to n - 1 do
        off += M[i, j] * M[i, j];
    
    if Sqrt(off) < tol then
      break;
    
    // --- Поиск максимального вне-диагонального
    var p := 0;
    var q := 1;
    var maxVal := 0.0;
    
    for var i := 0 to n - 1 do
      for var j := i + 1 to n - 1 do
      begin
        var v1 := Abs(M[i, j]);
        if v1 > maxVal then
        begin
          maxVal := v1;
          p := i;
          q := j;
        end;
      end;
    
    var app := M[p, p];
    var aqq := M[q, q];
    var apq := M[p, q];
    
    if Abs(apq) < tol then
      continue;
    
    // --- Устойчивая формула вращения
    var tau := (aqq - app) / (2.0 * apq);
    
    var t: real;
    if tau >= 0.0 then
      t := 1.0 / (tau + Sqrt(1.0 + tau * tau))
    else
      t := -1.0 / (-tau + Sqrt(1.0 + tau * tau));
    
    var c := 1.0 / Sqrt(1.0 + t * t);
    var s := t * c;
    
    // --- Обновление M
    for var k := 0 to n - 1 do
    begin
      if (k <> p) and (k <> q) then
      begin
        var mkp := M[k, p];
        var mkq := M[k, q];
        
        var newkp := c * mkp - s * mkq;
        var newkq := s * mkp + c * mkq;
        
        M[k, p] := newkp;
        M[p, k] := newkp;
        
        M[k, q] := newkq;
        M[q, k] := newkq;
      end;
    end;
    
    M[p, p] := app - t * apq;
    M[q, q] := aqq + t * apq;
    M[p, q] := 0.0;
    M[q, p] := 0.0;
    
    // --- Обновление V
    for var k := 0 to n - 1 do
    begin
      var vkp := V[k, p];
      var vkq := V[k, q];
      
      V[k, p] := c * vkp - s * vkq;
      V[k, q] := s * vkp + c * vkq;
    end;
  end;
  
  // --- Eigenvalues
  var values := new Vector(n);
  for var i := 0 to n - 1 do
    values[i] := M[i, i];
  
  // --- Сортировка по убыванию
  for var i := 0 to n - 2 do
    for var j := i + 1 to n - 1 do
      if values[j] > values[i] then
      begin
        var tmp := values[i];
        values[i] := values[j];
        values[j] := tmp;
        
        for var k := 0 to n - 1 do
        begin
          var tv := V[k, i];
          V[k, i] := V[k, j];
          V[k, j] := tv;
        end;
      end;
  
  // --- Нормализация знаков (для стабильности PCA)
  for var i := 0 to n - 1 do
  begin
    var maxAbs := 0.0;
    var idx := 0;
    
    for var k := 0 to n - 1 do
    begin
      var v1 := Abs(V[k, i]);
      if v1 > maxAbs then
      begin
        maxAbs := v1;
        idx := k;
      end;
    end;
    
    if V[idx, i] < 0.0 then
      for var k := 0 to n - 1 do
        V[k, i] := -V[k, i];
  end;
  
  Result := (values, V);
end;

function Matrix.PCA(k: integer): (Matrix, Vector);
begin
  var m := RowCount;
  var n := ColCount;

  if k < 1 then
    ArgumentError(ER_PCA_K_INVALID);

  if k > n then
    ArgumentError(ER_PCA_K_TOO_LARGE);

  if m < 2 then
    ArgumentError(ER_PCA_NEED_TWO_SAMPLES);

  // --- Центрирование
  var Xc := Clone;

  for var j := 0 to n - 1 do
  begin
    var mean := 0.0;
    for var i := 0 to m - 1 do
      mean += Xc[i, j];
    mean /= m;

    for var i := 0 to m - 1 do
      Xc[i, j] -= mean;
  end;

  // --- Ковариационная матрица
  var C := new Matrix(n, n);

  for var i := 0 to n - 1 do
    for var j := i to n - 1 do
    begin
      var s := 0.0;
      for var t := 0 to m - 1 do
        s += Xc[t, i] * Xc[t, j];

      s /= (m - 1);

      C[i, j] := s;
      C[j, i] := s;
    end;

  // --- Eigen
  var (values, V) := C.EigenSymmetric;

  // --- Выбор k компонент
  var components := new Matrix(n, k);
  var variances := new Vector(k);

  for var j := 0 to k - 1 do
  begin
    variances[j] := values[j];
    for var i := 0 to n - 1 do
      components[i, j] := V[i, j];
  end;

  Result := (components, variances);
end;



// Helper
function Cholesky(A: Matrix): Matrix;
begin
  if A.RowCount <> A.ColCount then
    ArgumentError(ER_CHOLESKY_NOT_SQUARE);
  
  var n := A.RowCount;
  var L := new Matrix(n, n);
  
  for var i := 0 to n - 1 do
    for var j := 0 to i do
    begin
      var s := A[i, j];
      for var k := 0 to j - 1 do
        s -= L[i, k] * L[j, k];
      
      if i = j then
      begin
        if s <= 0.0 then
          Error(ER_MATRIX_NOT_SPD);
        L[i, i] := Sqrt(s);
      end
      else
        L[i, j] := s / L[j, j];
    end;
  
  Result := L;
end;

// Helper
function LUDecompose(A: Matrix): (Matrix, array of integer);
begin
  if A.RowCount <> A.ColCount then
    ArgumentError(ER_MATRIX_NOT_SQUARE);
  
  var n := A.RowCount;
  var LU := A.Clone;
  var p := new integer[n];
  
  for var i := 0 to n - 1 do
    p[i] := i;
  
  for var k := 0 to n - 1 do
  begin
    var maxRow := k;
    var maxVal := Abs(LU[k, k]);
    
    for var i := k + 1 to n - 1 do
      if Abs(LU[i, k]) > maxVal then
      begin
        maxVal := Abs(LU[i, k]);
        maxRow := i;
      end;
    
    if maxVal = 0.0 then
      Error(ER_MATRIX_SINGULAR);
    
    if maxRow <> k then
    begin
      for var j := 0 to n - 1 do
      begin
        var t := LU[k, j];
        LU[k, j] := LU[maxRow, j];
        LU[maxRow, j] := t;
      end;
      
      var tp := p[k];
      p[k] := p[maxRow];
      p[maxRow] := tp;
    end;
    
    for var i := k + 1 to n - 1 do
    begin
      LU[i, k] /= LU[k, k];
      for var j := k + 1 to n - 1 do
        LU[i, j] -= LU[i, k] * LU[k, j];
    end;
  end;
  
  Result := (LU, p);
end;

// Helper
function Permute(b: Vector; p: array of integer): Vector;
begin
  var n := b.Length;
  var r := new Vector(n);
  
  for var i := 0 to n - 1 do
    r[i] := b[p[i]];
  
  Result := r;
end;

// Helper
function SolveLowerTriangularUnit(LU: Matrix; b: Vector): Vector;
begin
  var n := LU.RowCount;
  var y := new Vector(n);
  
  for var i := 0 to n - 1 do
  begin
    var s := b[i];
    for var j := 0 to i - 1 do
      s -= LU[i, j] * y[j];
    y[i] := s; 
  end;
  
  Result := y;
end;

function SolveLowerTriangular(L: Matrix; b: Vector): Vector;
begin
  var n := L.RowCount;
  var y := new Vector(n);
  
  for var i := 0 to n - 1 do
  begin
    var s := b[i];
    for var j := 0 to i - 1 do
      s -= L[i, j] * y[j];
    y[i] := s / L[i, i];
  end;
  
  Result := y;
end;


// Helper
function SolveUpperTriangular(U: Matrix; y: Vector): Vector;
begin
  var n := U.RowCount;
  var x := new Vector(n);
  
  for var i := n - 1 downto 0 do
  begin
    var s := y[i];
    for var j := i + 1 to n - 1 do
      s -= U[i, j] * x[j];
    x[i] := s / U[i, i];
  end;
  
  Result := x;
end;

function Solve(A: Matrix; b: Vector): Vector;
begin
  if A.RowCount <> A.ColCount then
    ArgumentError(ER_MATRIX_NOT_SQUARE);
  
  if b.Length <> A.RowCount then
    DimensionError(ER_VECTOR_SIZE_MISMATCH, b.Length, A.RowCount);
  
  var (LU, p) := LUDecompose(A);
  var pb := Permute(b, p);
  var y := SolveLowerTriangularUnit(LU, pb);
  Result := SolveUpperTriangular(LU, y);
end;

function SolveSPD(A: Matrix; b: Vector): Vector;
begin
  if A.RowCount <> A.ColCount then
    ArgumentError(ER_MATRIX_NOT_SQUARE);
  
  if b.Length <> A.RowCount then
    DimensionError(ER_VECTOR_SIZE_MISMATCH, b.Length, A.RowCount);
  
  var L := Cholesky(A);
  var y := SolveLowerTriangular(L, b);
  Result := SolveUpperTriangular(L.Transpose, y);
end;

function SolveAuto(A: Matrix; b: Vector): Vector;
begin
  try
    Result := SolveSPD(A, b);
  except
    Result := Solve(A, b);
  end;
end;

function SolveRidge(A: Matrix; b: Vector; lambda: real): Vector;
begin
  if lambda < 0.0 then
    ArgumentError(ER_LAMBDA_NEGATIVE);
  
  var m := A.RowCount;
  var n := A.ColCount;
  
  if b.Length <> m then
    DimensionError(ER_VECTOR_SIZE_MISMATCH, b.Length, m);
  
  // ------------------------------------------------------------
  // 1. Compute AtA = A^T * A
  // ------------------------------------------------------------
  var AtA := new Matrix(n, n);
  
  for var i := 0 to n - 1 do
    for var j := 0 to n - 1 do
    begin
      var s := 0.0;
      for var k := 0 to m - 1 do
        s += A[k, i] * A[k, j];
      AtA[i, j] := s;
    end;
  
  // ------------------------------------------------------------
  // 2. Add lambda * I
  // ------------------------------------------------------------
  if lambda <> 0.0 then
    for var i := 0 to n - 1 do
      AtA[i, i] += lambda;
  
  // ------------------------------------------------------------
  // 3. Compute Atb = A^T * b
  // ------------------------------------------------------------
  var Atb := new Vector(n);
  
  for var i := 0 to n - 1 do
  begin
    var s := 0.0;
    for var k := 0 to m - 1 do
      s += A[k, i] * b[k];
    Atb[i] := s;
  end;
  
  // ------------------------------------------------------------
  // 4. Solve SPD system
  // ------------------------------------------------------------
  Result := SolveSPD(AtA, Atb);
end;


function SolveLeastSquaresQR(A: Matrix; b: Vector): Vector;
begin
  if A.RowCount <> b.Length then
    DimensionError(ER_DIM_MISMATCH, A.RowCount, b.Length);

  var m := A.RowCount;
  var n := A.ColCount;

  if m < n then
    ArgumentError(ER_QR_REQUIRES_M_GE_N);

  var R := A.Clone;
  var y := b.Clone;

  for var k := 0 to n - 1 do
  begin
    // ---- вычислить норму столбца k начиная с строки k
    var normx := 0.0;
    for var i := k to m - 1 do
      normx += R[i,k] * R[i,k];
    
    normx := Sqrt(normx);
    
    if normx = 0 then
      continue;
    
    if R[k,k] >= 0 then
      normx := -normx;
    
    R[k,k] -= normx;
    
    var vnorm2 := 0.0;
    for var i := k to m - 1 do
      vnorm2 += R[i,k] * R[i,k];
    
    var beta := 2.0 / vnorm2;

    // ---- применить отражение к R
    for var j := k + 1 to n - 1 do
    begin
      var s := 0.0;
      for var i := k to m - 1 do
        s += R[i,k] * R[i,j];

      s *= beta;

      for var i := k to m - 1 do
        R[i,j] -= s * R[i,k];
    end;

    // ---- применить отражение к y
    var sy := 0.0;
    for var i := k to m - 1 do
      sy += R[i,k] * y[i];

    sy *= beta;

    for var i := k to m - 1 do
      y[i] -= sy * R[i,k];

    // ---- восстановить диагональный элемент
    R[k,k] := normx;

    for var i := k + 1 to m - 1 do
      R[i,k] := 0.0;
  end;

  // ---- Back substitution (решаем R[0:n,0:n] * x = y[0:n])
  var x := new Vector(n);

  for var i := n - 1 downto 0 do
  begin
    var sum := y[i];

    for var j := i + 1 to n - 1 do
      sum -= R[i,j] * x[j];

    if Abs(R[i,i]) < 1e-14 then
      ArgumentError(ER_SINGULAR_MATRIX);

    x[i] := sum / R[i,i];
  end;

  Result := x;
end;


end.