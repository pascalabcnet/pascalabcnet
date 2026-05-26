unit DataAdapters;

interface

// Здесь пока только методы расширения поэтому секция interface - пуста  
  
implementation

uses MLUtilsABC;
uses DataFrameABC;
uses MLExceptions;
uses LinearAlgebraML;

const
  ER_TO_MATRIX_NO_COLUMNS =
    'ToMatrix: не указаны столбцы!!ToMatrix: no columns specified';
  ER_TO_MATRIX_NON_NUMERIC =
    'Столбец "{0}" содержит нечисловые или NA значения!!Column "{0}" contains non-numeric or NA values';
  ER_ENCODELABELS_COLUMN_NOT_STRING =
    'Столбец {0} должен быть строковым для EncodeLabels!!Column {0} must be string for EncodeLabels';  
  ER_ENCODELABELS_NOT_CATEGORICAL =
    'Столбец "{0}" должен быть категориальным для EncodeLabels!!Column "{0}" must be categorical for EncodeLabels';
  ER_ENCODELABELS_UNSUPPORTED_TYPE =
    'Неподдерживаемый тип столбца "{0}" для EncodeLabels!!Unsupported column type "{0}" for EncodeLabels';
  ER_TARGET_HAS_NA =
    'Целевой столбец "{0}" содержит NA!!Target column "{0}" contains NA values';  
    
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
    case col.Info.ColType of
      ColumnType.ctInt:
        begin
          var c := IntColumn(col);
          for var i := 0 to n - 1 do
          begin
            if not c.IsValid[i] then
              ArgumentError(ER_TO_MATRIX_NON_NUMERIC, colNames[j]);
            Result[i,j] := c.Data[i];
          end;
        end;
      ColumnType.ctFloat:
        begin
          var c := FloatColumn(col);
          for var i := 0 to n - 1 do
          begin
            if not c.IsValid[i] then
              ArgumentError(ER_TO_MATRIX_NON_NUMERIC, colNames[j]);
            Result[i,j] := c.Data[i];
          end;
        end;
      ColumnType.ctBool:
        begin
          var c := BoolColumn(col);
          for var i := 0 to n - 1 do
          begin
            if not c.IsValid[i] then
              ArgumentError(ER_TO_MATRIX_NON_NUMERIC, colNames[j]);
            Result[i,j] := if c.Data[i] then 1.0 else 0.0;
          end;
        end;
      else
        ArgumentError(ER_TO_MATRIX_NON_NUMERIC, colNames[j]);
    end;
  end;
end;

function ToVector(Self: DataFrame; colName: string): Vector; extensionmethod;
begin
  var df := Self;
  var n := df.RowCount;
  Result := new Vector(n);

  var col := df[colName];
  case col.Info.ColType of
    ColumnType.ctInt:
      begin
        var c := IntColumn(col);
        for var i := 0 to n - 1 do
        begin
          if not c.IsValid[i] then
            ArgumentError(ER_TO_VECTOR_NON_NUMERIC, colName);
          Result[i] := c.Data[i];
        end;
      end;
    ColumnType.ctFloat:
      begin
        var c := FloatColumn(col);
        for var i := 0 to n - 1 do
        begin
          if not c.IsValid[i] then
            ArgumentError(ER_TO_VECTOR_NON_NUMERIC, colName);
          Result[i] := c.Data[i];
        end;
      end;
    ColumnType.ctBool:
      begin
        var c := BoolColumn(col);
        for var i := 0 to n - 1 do
        begin
          if not c.IsValid[i] then
            ArgumentError(ER_TO_VECTOR_NON_NUMERIC, colName);
          Result[i] := if c.Data[i] then 1.0 else 0.0;
        end;
      end;
    else
      ArgumentError(ER_TO_VECTOR_NON_NUMERIC, colName);
  end;
end;

/// Кодирует строковые метки классов в целочисленные индексы (0..K-1).
/// Порядок кодирования соответствует порядку первого появления меток.
/// Используйте только при ручной подготовке данных (без DataPipeline).
/// Вызывать только ДО разбиения на train/test, чтобы избежать рассинхронизации меток.
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
  
  var col := Self[target];
  for var i := 0 to Self.RowCount - 1 do
    if not col.IsValid[i] then
      ArgumentError(ER_TARGET_HAS_NA, target);

  if Self.GetColumnType(target) = ColumnType.ctStr then
  begin
    var labels := Self.GetStrColumn(target);
    Result := EncodeLabels(labels);
    exit;
  end;

  if Self.GetColumnType(target) = ColumnType.ctInt then
  begin
    var labels := Self.GetIntColumn(target).ToArray;
    var classes: array of integer;
    Result := EncodeLabelsInt(labels,classes);
    exit;
  end;

  ArgumentError(ER_ENCODELABELS_UNSUPPORTED_TYPE, target);
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
  
  var col := Self[target];
  for var i := 0 to Self.RowCount - 1 do
    if not col.IsValid[i] then
      ArgumentError(ER_TARGET_HAS_NA, target);

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
  
  var col := Self[target];
  for var i := 0 to Self.RowCount - 1 do
    if not col.IsValid[i] then
      ArgumentError(ER_TARGET_HAS_NA, target);

  case Self.GetColumnType(target) of
    ColumnType.ctStr:
      begin
        var data := Self.GetStrColumn(target).ToArray;
        Result := TransformLabels(data, classes);
      end;
    ColumnType.ctInt:
      begin
        var data := Self.GetIntColumn(target).ToArray;
        var strData := new string[data.Length];
  
        for var i := 0 to data.Length - 1 do
          strData[i] := data[i].ToString;
  
        Result := TransformLabels(strData, classes);
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
  
  var col := Self[target];
  for var i := 0 to Self.RowCount - 1 do
    if not col.IsValid[i] then
      ArgumentError(ER_TARGET_HAS_NA, target);

  var data := Self.GetIntColumn(target).ToArray;

  Result := TransformLabelsInt(data, classes);
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

  var col := Self[target];
  for var i := 0 to Self.RowCount - 1 do
    if not col.IsValid[i] then
      ArgumentError(ER_TARGET_HAS_NA, target);

  var labels := Self.GetIntColumn(target).ToArray;

  Result := EncodeLabelsInt(labels, classes);
end;



end.
