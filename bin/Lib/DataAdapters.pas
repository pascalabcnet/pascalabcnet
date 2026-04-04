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

/// Кодирует строковые метки классов в целочисленные индексы.
/// Каждому уникальному значению присваивается номер 0,1,2,...
/// Порядок кодирования соответствует порядку первого появления меток.
/// В параметр classes возвращается массив уникальных значений в порядке кодирования.
/// Используется при обучении моделей и визуализации
function EncodeLabels(labels: array of string; var classes: array of string): array of integer;

/// Преобразует строковые метки классов в целочисленные индексы
///   с использованием заранее заданного массива classes (mapping).
/// classes должен быть получен из EncodeLabels.
/// Если встречается неизвестная метка — выбрасывается исключение.
/// Используется для применения кодирования к тестовым данным (Transform).
function TransformLabels(labels: array of string; classes: array of string): array of integer;

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
  ER_ENCODELABELS_NOT_CATEGORICAL =
    'Столбец "{0}" должен быть категориальным для EncodeLabels!!Column "{0}" must be categorical for EncodeLabels';
  ER_ENCODELABELS_UNSUPPORTED_TYPE =
    'Неподдерживаемый тип столбца "{0}" для EncodeLabels!!Unsupported column type "{0}" for EncodeLabels';
  ER_UNKNOWN_CLASS_IN_TRANSFORM =
    'Неизвестное значение класса "{0}" при преобразовании меток!!Unknown class value "{0}" in TransformLabels';
  ER_LABEL_INDEX_OUT_OF_RANGE =
    'Индекс метки {0} вне диапазона [0, {1})!!Label index {0} is out of range [0, {1})';
  
function LabelsToInts(y: Vector): array of integer;
begin
  if y = nil then
    ArgumentNullError(ER_ARG_NULL, 'y');

  Result := ArrGen(y.Length, i -> Round(y[i]));
end;

function EncodeLabels(labels: array of string; var classes: array of string): array of integer;
begin
  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var classList := new List<string>;
  var map := new Dictionary<string, integer>;

  // собираем классы в порядке первого появления
  for var i := 0 to labels.Length - 1 do
  begin
    var lbl := labels[i];
    if not map.ContainsKey(lbl) then
    begin
      map[lbl] := classList.Count;
      classList.Add(lbl);
    end;
  end;

  // кодируем
  var res := new integer[labels.Length];
  for var i := 0 to labels.Length - 1 do
    res[i] := map[labels[i]];

  classes := classList.ToArray;
  Result := res;
end;

function TransformLabels(labels: array of string; classes: array of string): array of integer;
begin
  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var map := new Dictionary<string, integer>;
  for var i := 0 to classes.Length - 1 do
    map[classes[i]] := i;

  var res := new integer[labels.Length];

  for var i := 0 to labels.Length - 1 do
  begin
    var lbl := labels[i];

    if not map.ContainsKey(lbl) then
      Error(ER_UNKNOWN_CLASS_IN_TRANSFORM, lbl);

    res[i] := map[lbl];
  end;

  Result := res;
end;

function EncodeLabels(labels: array of string): array of integer;
begin
  var classes: array of string;
  Result := EncodeLabels(labels, classes);
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

// Helper
function EncodeLabelsIntHelper(labels: array of integer): array of integer;
begin
  var classList := new List<integer>;
  var map := new Dictionary<integer, integer>;

  for var i := 0 to labels.Length - 1 do
  begin
    var lbl := labels[i];
    if not map.ContainsKey(lbl) then
    begin
      map[lbl] := classList.Count;
      classList.Add(lbl);
    end;
  end;

  var y := new integer[labels.Length];
  for var i := 0 to labels.Length - 1 do
    y[i] := map[labels[i]];

  Result := y;
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

  if not Self.IsCategorical(target) then
    ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, target);

  if Self.GetColumnType(target) = ColumnType.ctStr then
  begin
    var labels := Self.GetStrColumn(target);
    Result := EncodeLabels(labels);
    exit;
  end;

  if Self.GetColumnType(target) = ColumnType.ctInt then
  begin
    var labels := Self.GetIntColumn(target).ToArray;
    Result := EncodeLabelsIntHelper(labels);
    exit;
  end;

  ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, target);
end;

function EncodeLabelsInt(labels: array of integer; var classes: array of integer): array of integer;
begin
  if labels = nil then
    ArgumentNullError(ER_ARG_NULL, 'labels');

  var classList := new List<integer>;
  var map := new Dictionary<integer, integer>;

  // собираем уникальные значения в порядке первого появления
  for var i := 0 to labels.Length - 1 do
  begin
    var lbl := labels[i];
    if not map.ContainsKey(lbl) then
    begin
      map[lbl] := classList.Count;
      classList.Add(lbl);
    end;
  end;

  // кодируем
  var res := new integer[labels.Length];
  for var i := 0 to labels.Length - 1 do
    res[i] := map[labels[i]];

  classes := classList.ToArray;
  Result := res;
end;

/// Кодирует значения категориального столбца DataFrame в целочисленные индексы.
/// Каждому уникальному значению присваивается номер 0,1,2,...
/// Порядок кодирования соответствует порядку первого появления значений в столбце.
/// В параметр classes возвращается массив уникальных значений в порядке кодирования.
/// Работает только для категориальных столбцов типов string и integer.
/// Для целочисленных категориальных столбцов значения преобразуются в строки.
/// Используется при подготовке данных для задач классификации и визуализации.
function EncodeLabels(Self: DataFrame; target: string; var classes: array of string): array of integer; extensionmethod;
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'Self');

  if target = nil then
    ArgumentNullError(ER_ARG_NULL, 'target');

  if not Self.HasColumn(target) then
    ArgumentError(ER_COLUMN_NOT_FOUND, target);

  if not Self.IsCategorical(target) then
    ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, target);

  case Self.GetColumnType(target) of

    ColumnType.ctStr:
      begin
        var labels := Self.GetStrColumn(target).ToArray;
        Result := EncodeLabels(labels, classes);
      end;

    ColumnType.ctInt:
      begin
        var labels := Self.GetIntColumn(target).ToArray;
      
        var intClasses: array of integer;
        Result := EncodeLabelsInt(labels, intClasses);
      
        // если API требует string classes:
        classes := intClasses.Select(x -> x.ToString).ToArray;
      end;

    else
      ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, target);
  end;
end;

/// Преобразует строковые метки целевого столбца в целочисленные индексы (0,1,2,...)
/// с использованием заданного массива classes (mapping индекс → метка).
/// classes должен быть получен ранее с помощью EncodeLabels.
/// При обнаружении неизвестного значения выбрасывается исключение.
/// Используется для применения кодирования к тестовым данным (Transform).
function TransformLabels(Self: DataFrame; target: string; classes: array of string): array of integer; extensionmethod;
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'Self');

  if target = nil then
    ArgumentNullError(ER_ARG_NULL, 'target');

  if classes = nil then
    ArgumentNullError(ER_ARG_NULL, 'classes');

  if not Self.HasColumn(target) then
    ArgumentError(ER_COLUMN_NOT_FOUND, target);

  if not Self.IsCategorical(target) then
    ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, target);

  // --- строим mapping
  var map := new Dictionary<string, integer>;
  for var i := 0 to classes.Length - 1 do
    map[classes[i]] := i;

  case Self.GetColumnType(target) of

    ColumnType.ctStr:
      begin
        var data := Self.GetStrColumn(target).ToArray;
        var res := new integer[data.Length];

        for var i := 0 to data.Length - 1 do
        begin
          var lbl := data[i];

          if not map.ContainsKey(lbl) then
            Error(ER_UNKNOWN_CLASS_IN_TRANSFORM, lbl);

          res[i] := map[lbl];
        end;

        Result := res;
      end;

    ColumnType.ctInt:
      begin
        var data := Self.GetIntColumn(target).ToArray;
        var res := new integer[data.Length];

        for var i := 0 to data.Length - 1 do
        begin
          var lbl := data[i].ToString;

          if not map.ContainsKey(lbl) then
            Error(ER_UNKNOWN_CLASS_IN_TRANSFORM, lbl);

          res[i] := map[lbl];
        end;

        Result := res;
      end;

    else
      ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, target);
  end;
end;

/// Преобразует значения целочисленного категориального столбца
/// в плотные индексы (0,1,2,...) с использованием заданного массива classes.
/// classes должен быть получен ранее с помощью EncodeLabelsInt.
/// При обнаружении неизвестного значения выбрасывается исключение.
function TransformLabelsInt(Self: DataFrame; target: string; classes: array of integer): array of integer; extensionmethod;
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'Self');

  if target = nil then
    ArgumentNullError(ER_ARG_NULL, 'target');

  if classes = nil then
    ArgumentNullError(ER_ARG_NULL, 'classes');

  if not Self.HasColumn(target) then
    ArgumentError(ER_COLUMN_NOT_FOUND, target);

  if not Self.IsCategorical(target) then
    ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, target);

  if Self.GetColumnType(target) <> ColumnType.ctInt then
    ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, target);

  // --- mapping: значение → индекс
  var map := new Dictionary<integer, integer>;
  for var i := 0 to classes.Length - 1 do
    map[classes[i]] := i;

  var data := Self.GetIntColumn(target).ToArray;
  var res := new integer[data.Length];

  for var i := 0 to data.Length - 1 do
  begin
    var v := data[i];

    if not map.ContainsKey(v) then
      Error(ER_UNKNOWN_CLASS_IN_TRANSFORM, v);

    res[i] := map[v];
  end;

  Result := res;
end;

/// Кодирует значения целочисленного категориального столбца DataFrame
/// в плотные целочисленные индексы 0,1,2,...
/// Каждому уникальному значению присваивается номер в порядке первого появления.
/// В параметр classes возвращается массив уникальных значений в порядке кодирования.
/// Работает только для категориальных столбцов типа integer.
/// Используется при подготовке данных для задач классификации и анализа.
function EncodeLabelsInt(Self: DataFrame; target: string; var classes: array of integer): array of integer; extensionmethod;
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'Self');

  if target = nil then
    ArgumentNullError(ER_ARG_NULL, 'target');

  if not Self.HasColumn(target) then
    ArgumentError(ER_COLUMN_NOT_FOUND, target);

  if not Self.IsCategorical(target) then
    ArgumentError(ER_ENCODELABELS_NOT_CATEGORICAL, target);

  if Self.GetColumnType(target) <> ColumnType.ctInt then
    ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, target);

  var labels := Self.GetIntColumn(target).ToArray;

  Result := EncodeLabelsInt(labels, classes);
end;

function DecodeLabels(y: array of integer; classes: array of string): array of string;
begin
  var res := new string[y.Length];

  for var i := 0 to y.Length - 1 do
  begin
    var idx := y[i];
  
    if (idx < 0) or (idx >= classes.Length) then
      Error(ER_LABEL_INDEX_OUT_OF_RANGE, idx, classes.Length);
  
    res[i] := classes[idx];
  end;

  Result := res;
end;

function UniqueLabels(labels: array of string): array of string;
begin
  Result := labels.Distinct.ToArray;
end;

end.