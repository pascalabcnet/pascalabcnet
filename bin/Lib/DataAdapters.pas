unit DataAdapters;

interface

uses DataFrameABC;
uses LinearAlgebraML;

/// Преобразует вектор меток классов в массив целых чисел.
/// Используется при визуализации и других задачах,
///   где метки должны быть представлены как 0,1,2,...
/// Значения округляются функцией Round, чтобы устранить
///   возможные небольшие численные ошибки 
function LabelsToInts(y: Vector): array of integer;

/// Кодирует строковые метки классов в целочисленные индексы.
/// Каждому уникальному значению присваивается номер 0,1,2,...
/// Порядок кодирования соответствует порядку первого появления меток.
/// Используется при обучении моделей и визуализации.
function EncodeLabels(labels: array of string): array of integer;

/// Преобразует целочисленные индексы классов обратно в строковые метки.
/// Массив classes задаёт соответствие: classes[i] — имя класса с индексом i.
/// Используется для получения текстовых предсказаний моделей.
function DecodeLabels(y: array of integer; classes: array of string): array of string;

/// Возвращает список уникальных меток классов.
/// Порядок соответствует первому появлению значений во входном массиве.
/// Используется для определения множества классов в задаче классификации.
function UniqueLabels(labels: array of string): array of string;
  
implementation

uses MLExceptions;

const
  ER_TO_MATRIX_NO_COLUMNS =
    'ToMatrix: не указаны столбцы!!ToMatrix: no columns specified';
  ER_TO_VECTOR_NON_NUMERIC =
    'ToVector: столбец "{0}" содержит нечисловые или NA значения!!' +
    'ToVector: column "{0}" contains non-numeric or NA values';  
  ER_TO_MATRIX_NON_NUMERIC =
    'Столбец "{0}" содержит нечисловые или NA значения!!Column "{0}" contains non-numeric or NA values';
  ER_ENCODELABELS_COLUMN_NOT_STRING =
    'Столбец {0} должен быть строковым для EncodeLabels!!Column {0} must be string for EncodeLabels';  
    
function LabelsToInts(y: Vector): array of integer;
begin
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  Result := ArrGen(y.Length, i -> Round(y[i]));
end;

function EncodeLabels(labels: array of string): array of integer;
begin
  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var classes := labels.Distinct.Order.ToArray;

  var map := new Dictionary<string,integer>;

  for var i := 0 to classes.Length-1 do
    map[classes[i]] := i;

  var res := new integer[labels.Length];

  for var i := 0 to labels.Length-1 do
    res[i] := map[labels[i]];

  Result := res;
end;
    
function ToMatrix(Self: DataFrame; colNames: array of string): Matrix; extensionmethod;
begin
  var df := Self;
  var n := df.RowCount;
  var p := colNames.Length;

  if p = 0 then
    ArgumentError(ER_TO_MATRIX_NO_COLUMNS);

  Result := new Matrix(n, p);

  for var j := 0 to p - 1 do
  begin
    var col := df[colNames[j]];

    for var i := 0 to n - 1 do
    begin
      var value: real;

      if not col.TryGetNumericValue(i, value) then
        ArgumentError(ER_TO_MATRIX_NON_NUMERIC, colNames[j]);

      Result[i,j] := value;
    end;
  end;
end;

function ToVector(Self: DataFrame; colName: string): Vector; extensionmethod;
begin
  var df := Self;
  var n := df.RowCount;
  Result := new Vector(n);

  var col := df[colName];

  for var i := 0 to n - 1 do
  begin
    var value: real;

    if not col.TryGetNumericValue(i, value) then
      ArgumentError(ER_TO_VECTOR_NON_NUMERIC, colName);

    Result[i] := value;
  end;
end;

/// Кодирует строковые метки классов в целочисленные индексы.
/// Каждому уникальному значению присваивается номер 0,1,2,...
/// Порядок кодирования соответствует порядку первого появления меток.
/// Используется при обучении моделей и визуализации.
function EncodeLabels(Self: DataFrame; target: string): array of integer; extensionmethod;
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'Self');

  if target = nil then
    ArgumentNullError(ER_ARG_NULL, 'target');

  if not Self.HasColumn(target) then
    ArgumentError(ER_COLUMN_NOT_FOUND, target);

  if Self.GetColumnType(target) <> ColumnType.ctStr then
    ArgumentError(ER_ENCODELABELS_COLUMN_NOT_STRING, target);

  var labels := Self.GetStrColumn(target);

  Result := EncodeLabels(labels);
end;

function DecodeLabels(y: array of integer; classes: array of string): array of string;
begin
  var res := new string[y.Length];

  for var i := 0 to y.Length-1 do
    res[i] := classes[y[i]];

  Result := res;
end;

function UniqueLabels(labels: array of string): array of string;
begin
  Result := labels.Distinct.ToArray;
end;

end.