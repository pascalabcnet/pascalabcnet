unit LinearAlgebraML;

interface

type
  Vector = class
  private
    static procedure CheckSameLength(const a, b: Vector);
    static procedure CheckNonEmpty(const v: Vector); 
    procedure SetData(i: integer; value: real) := data[i] := value;
  public
    data: array of real;
    property Length: integer read data.Length;
    property Item[i: integer]: real read data[i] write SetData; default;
    
    constructor Create(n: integer);
    constructor Create(values: array of real);
    constructor Create(values: array of integer);
    
    function Clone: Vector;
    
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
    function Mean: real;
    function Norm2: real;
    function Norm: real;
    function Max: real;
    function Min: real;
    /// Скалярное произведение
    function Dot(b: Vector): real;

    // ---------- Сервисные методы ----------
    function ToString: string; override := $'{data.Select(x -> x.ToString(''G3''))}';
    function ToString(digits: integer): string := $'{data.Select(x -> x.ToString(''G''+digits))}';
    procedure Print := data.Print;
    procedure Println := data.Println;
  end;
  
  Matrix = class
  private
    static procedure CheckSameSize(A, B: Matrix);
    static procedure CheckMulSize(A, B: Matrix);
    static procedure CheckVecSize(A: Matrix; x: Vector);
    
    procedure SetData(i, j: integer; value: real) := data[i, j] := value;
  public
    data: array[,] of real;
    property Rows: integer read data.GetLength(0);
    property Cols: integer read data.GetLength(1);
    
    property Item[i, j: integer]: real
    read data[i, j] write SetData; default;
    
    constructor Create(r, c: integer);
    constructor Create(values: array[,] of real);
    
    function Clone: Matrix;
    
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
    
    static function operator *(alpha: real; A: Matrix): Matrix;
    static function operator *(A: Matrix; alpha: real): Matrix;
    
    // ---------- in-place operators ----------
    static function operator +=(A, B: Matrix): Matrix;
    static function operator -=(A, B: Matrix): Matrix;
    static function operator *=(A: Matrix; alpha: real): Matrix;
    
    // ---------- Основные методы ----------
    
    function RowCount: integer := data.RowCount;
    function ColCount: integer := data.ColCount;
    
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
    function ToString: string; override := $'{data}';
    procedure Print := data.Print;
    procedure Println := data.Println;
    
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

implementation

uses System;

//-----------------------------
//           Vector
//-----------------------------

constructor Vector.Create(n: integer);
begin
  if n < 0 then
    raise new ArgumentOutOfRangeException('n', 'Vector length must be non-negative');
  data := new real[n];
end;

constructor Vector.Create(values: array of real);
begin
  if values = nil then
    raise new ArgumentNullException('values');
  data := Copy(values);
end;

constructor Vector.Create(values: array of integer);
begin
  if values = nil then
    raise new ArgumentNullException('values');
  data := values.Select(x -> real(x)).ToArray;
end;

function Vector.Clone: Vector;
begin
  Result := new Vector(data);
end;

static procedure Vector.CheckSameLength(a, b: Vector);
begin
  if a.Length <> b.Length then
    raise new ArgumentException(
    $'Vector length mismatch: {a.Length} vs {b.Length}');
end;

static procedure Vector.CheckNonEmpty(v: Vector);
begin
  if v.Length = 0 then
    raise new ArgumentException('Vector is empty');
end;

static function Vector.operator +(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  Result := new Vector(a.Length);
  for var i := 0 to a.Length - 1 do
    Result.data[i] := a.data[i] + b.data[i];
end;

static function Vector.operator -(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  Result := new Vector(a.Length);
  for var i := 0 to a.Length - 1 do
    Result.data[i] := a.data[i] - b.data[i];
end;

static function Vector.operator *(v: Vector; alpha: real): Vector;
begin
  Result := new Vector(v.Length);
  for var i := 0 to v.Length - 1 do
    Result.data[i] := alpha * v.data[i];
end;

static function Vector.operator *(alpha: real; v: Vector): Vector;
begin
  Result := v * alpha;
end;

static function Vector.operator /(v: Vector; alpha: real): Vector;
begin
  if alpha = 0.0 then
    raise new DivideByZeroException('Division by zero in Vector / scalar');
  Result := new Vector(v.Length);
  var inv := 1.0 / alpha;
  for var i := 0 to v.Length - 1 do
    Result.data[i] := v.data[i] * inv;
end;

static function Vector.operator +=(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  for var i := 0 to a.Length - 1 do
    a.data[i] += b.data[i];
  Result := a;
end;

static function Vector.operator -=(a, b: Vector): Vector;
begin
  CheckSameLength(a, b);
  for var i := 0 to a.Length - 1 do
    a.data[i] -= b.data[i];
  Result := a;
end;

static function Vector.operator *=(a: Vector; alpha: real): Vector;
begin
  for var i := 0 to a.Length - 1 do
    a.data[i] *= alpha;
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
    s += data[i];
  Result := s;
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

function Vector.Max: real := data.Max;

function Vector.Min: real := data.Min;

function Vector.Dot(b: Vector): real;
begin
  if Length <> b.Length then
    raise new Exception('Dimension mismatch in Dot');

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


//-----------------------------
//           Matrix
//-----------------------------
constructor Matrix.Create(r, c: integer);
begin
  if (r < 0) or (c < 0) then
    raise new ArgumentOutOfRangeException('Matrix size must be non-negative');
  data := new real[r, c];
end;

constructor Matrix.Create(values: array[,] of real);
begin
  if values = nil then
    raise new ArgumentNullException('values');
  data := Copy(values);
end;

function Matrix.Clone: Matrix;
begin
  Result := new Matrix(data);
end;

function Matrix.ColumnSums: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(p);

  for var j := 0 to p - 1 do
    for var i := 0 to n - 1 do
      Result[j] += data[i, j];
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
      Result[i] += data[i, j];
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
      var d := data[i, j] - means[j];
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
      var d := data[i, j] - means[i];
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
    Result[j] := data[0, j];

    for var i := 1 to n - 1 do
      if data[i, j] < Result[j] then
        Result[j] := data[i, j];
  end;
end;

function Matrix.ColumnMaxs: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(p);

  for var j := 0 to p - 1 do
  begin
    Result[j] := data[0, j];

    for var i := 1 to n - 1 do
      if data[i, j] > Result[j] then
        Result[j] := data[i, j];
  end;
end;

function Matrix.RowMins: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    Result[i] := data[i, 0];

    for var j := 1 to p - 1 do
      if data[i, j] < Result[i] then
        Result[i] := data[i, j];
  end;
end;

function Matrix.RowMaxs: Vector;
begin
  var n := RowCount;
  var p := ColCount;
  Result := new Vector(n);

  for var i := 0 to n - 1 do
  begin
    Result[i] := data[i, 0];

    for var j := 1 to p - 1 do
      if data[i, j] > Result[i] then
        Result[i] := data[i, j];
  end;
end;

function Matrix.RowArgMin(i: integer): integer;
begin
  var minVal := data[i,0];
  var arg := 0;

  for var j := 1 to ColCount - 1 do
    if data[i,j] < minVal then
    begin
      minVal := data[i,j];
      arg := j;
    end;

  Result := arg;
end;

function Matrix.RowMin(i: integer): real;
begin
  Result := data[i, RowArgMin(i)];
end;

function Matrix.RowArgMax(i: integer): integer;
begin
  var maxVal := data[i,0];
  var arg := 0;

  for var j := 1 to ColCount - 1 do
    if data[i,j] > maxVal then
    begin
      maxVal := data[i,j];
      arg := j;
    end;

  Result := arg;
end;

function Matrix.RowMax(i: integer): real;
begin
  Result := data[i, RowArgMax(i)];
end;

function Matrix.RowSum(i: integer): real;
begin
  var sum := 0.0;

  for var j := 0 to ColCount - 1 do
    sum += data[i,j];

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
    var d := data[i,j] - mean;
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
  var minVal := data[0,j];
  var arg := 0;

  for var i := 1 to RowCount - 1 do
    if data[i,j] < minVal then
    begin
      minVal := data[i,j];
      arg := i;
    end;

  Result := arg;
end;

function Matrix.ColumnMin(j: integer): real;
begin
  Result := data[ColumnArgMin(j), j];
end;

function Matrix.ColumnArgMax(j: integer): integer;
begin
  var maxVal := data[0,j];
  var arg := 0;

  for var i := 1 to RowCount - 1 do
    if data[i,j] > maxVal then
    begin
      maxVal := data[i,j];
      arg := i;
    end;

  Result := arg;
end;

function Matrix.ColumnMax(j: integer): real;
begin
  Result := data[ColumnArgMax(j), j];
end;

function Matrix.ColumnSum(j: integer): real;
begin
  var sum := 0.0;

  for var i := 0 to RowCount - 1 do
    sum += data[i,j];

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
    var d := data[i,j] - mean;
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
      s += data[i, j] * data[i, j];
  Result := PABCSystem.Sqrt(s);
end;

procedure Matrix.AddScaledIdentity(lambda: real);
begin
  var n := RowCount;
  for var i := 0 to n - 1 do
    data[i, i] += lambda;
end;

static procedure Matrix.CheckSameSize(A, B: Matrix);
begin
  if (A.Rows <> B.Rows) or (A.Cols <> B.Cols) then
    raise new ArgumentException(
    $'Matrix size mismatch: {A.Rows}x{A.Cols} vs {B.Rows}x{B.Cols}');
end;

static procedure Matrix.CheckMulSize(A, B: Matrix);
begin
  if A.Cols <> B.Rows then
    raise new ArgumentException(
    $'Matrix multiply size mismatch: {A.Rows}x{A.Cols} * {B.Rows}x{B.Cols}');
end;

static procedure Matrix.CheckVecSize(A: Matrix; x: Vector);
begin
  if A.Cols <> x.Length then
    raise new ArgumentException(
    $'Matrix-vector size mismatch: {A.Rows}x{A.Cols} * {x.Length}');
end;



static function Matrix.operator +(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  Result := new Matrix(A.Rows, A.Cols);
  for var i := 0 to A.Rows - 1 do
    for var j := 0 to A.Cols - 1 do
      Result.data[i, j] := A.data[i, j] + B.data[i, j];
end;

static function Matrix.operator -(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  Result := new Matrix(A.Rows, A.Cols);
  for var i := 0 to A.Rows - 1 do
    for var j := 0 to A.Cols - 1 do
      Result.data[i, j] := A.data[i, j] - B.data[i, j];
end;

static function Matrix.operator *(alpha: real; A: Matrix): Matrix;
begin
  Result := A * alpha;
end;

static function Matrix.operator *(A: Matrix; alpha: real): Matrix;
begin
  Result := new Matrix(A.Rows, A.Cols);
  for var i := 0 to A.Rows - 1 do
    for var j := 0 to A.Cols - 1 do
      Result.data[i, j] := alpha * A.data[i, j];
end;

static function Matrix.operator *(A: Matrix; x: Vector): Vector;
begin
  CheckVecSize(A, x);
  Result := new Vector(A.Rows);
  for var i := 0 to A.Rows - 1 do
  begin
    var s := 0.0;
    for var j := 0 to A.Cols - 1 do
      s += A.data[i, j] * x[j];
    Result[i] := s;
  end;
end;

static function Matrix.operator *(A, B: Matrix): Matrix;
begin
  CheckMulSize(A, B);
  Result := new Matrix(A.Rows, B.Cols);
  for var i := 0 to A.Rows - 1 do
    for var k := 0 to A.Cols - 1 do
    begin
      var aik := A.data[i, k];
      if aik <> 0.0 then
        for var j := 0 to B.Cols - 1 do
          Result.data[i, j] += aik * B.data[k, j];
    end;
end;

static function Matrix.operator +=(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  for var i := 0 to A.Rows - 1 do
    for var j := 0 to A.Cols - 1 do
      A.data[i, j] += B.data[i, j];
  Result := A;
end;

static function Matrix.operator -=(A, B: Matrix): Matrix;
begin
  CheckSameSize(A, B);
  for var i := 0 to A.Rows - 1 do
    for var j := 0 to A.Cols - 1 do
      A.data[i, j] -= B.data[i, j];
  Result := A;
end;

static function Matrix.operator *=(A: Matrix; alpha: real): Matrix;
begin
  for var i := 0 to A.Rows - 1 do
    for var j := 0 to A.Cols - 1 do
      A.data[i, j] *= alpha;
  Result := A;
end;

function Matrix.Transpose: Matrix;
begin
  Result := new Matrix(Cols, Rows);
  for var i := 0 to Rows - 1 do
    for var j := 0 to Cols - 1 do
      Result.data[j, i] := data[i, j];
end;

function Matrix.GetRow(i: integer): Vector;
begin
  if (i < 0) or (i >= Rows) then
    raise new ArgumentOutOfRangeException('i');
  Result := new Vector(Cols);
  for var j := 0 to Cols - 1 do
    Result[j] := data[i, j];
end;

function Matrix.GetCol(j: integer): Vector;
begin
  if (j < 0) or (j >= Cols) then
    raise new ArgumentOutOfRangeException('j');
  Result := new Vector(Rows);
  for var i := 0 to Rows - 1 do
    Result[i] := data[i, j];
end;

static function Matrix.Identity(n: integer): Matrix;
begin
  if n < 0 then
    raise new ArgumentOutOfRangeException('n', 'Matrix size must be non-negative');
  
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
  if Rows <> Cols then
  begin
    Result := false;
    exit;
  end;
  
  for var i := 0 to Rows - 1 do
    for var j := i + 1 to Cols - 1 do
      if Abs(data[i, j] - data[j, i]) > tol then
      begin
        Result := false;
        exit;
      end;
  
  Result := true;
end;

function Matrix.EigenSymmetric(tol: real; maxIter: integer): (Vector, Matrix);
begin
  if Rows <> Cols then
    raise new ArgumentException('Matrix must be square');
  
  if not IsSymmetric(tol) then
    raise new ArgumentException('Matrix must be symmetric');
  
  var n := Rows;
  
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
  var m := Rows;
  var n := Cols;

  if k < 1 then
    raise new ArgumentException('k must be >= 1');

  if k > n then
    raise new ArgumentException('k cannot exceed number of features');

  if m < 2 then
    raise new ArgumentException('At least two samples required');

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
  if A.Rows <> A.Cols then
    raise new ArgumentException('Matrix must be square for Cholesky');
  
  var n := A.Rows;
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
          raise new InvalidOperationException('Matrix is not SPD');
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
  if A.Rows <> A.Cols then
    raise new ArgumentException('Matrix must be square for LU');
  
  var n := A.Rows;
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
      raise new InvalidOperationException('Matrix is singular');
    
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
  var n := LU.Rows;
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
  var n := L.Rows;
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
  var n := U.Rows;
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
  if A.Rows <> A.Cols then
    raise new ArgumentException('Matrix A must be square');
  
  if b.Length <> A.Rows then
    raise new ArgumentException('Vector size mismatch in Solve');
  
  var (LU, p) := LUDecompose(A);
  var pb := Permute(b, p);
  var y := SolveLowerTriangularUnit(LU, pb);
  Result := SolveUpperTriangular(LU, y);
end;

function SolveSPD(A: Matrix; b: Vector): Vector;
begin
  if A.Rows <> A.Cols then
    raise new ArgumentException('Matrix A must be square');
  
  if b.Length <> A.Rows then
    raise new ArgumentException('Vector size mismatch in SolveSPD');
  
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
    raise new ArgumentException('lambda must be >= 0');
  
  var m := A.Rows;
  var n := A.Cols;
  
  if b.Length <> m then
    raise new ArgumentException('Vector length mismatch in SolveRidge');
  
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




end.