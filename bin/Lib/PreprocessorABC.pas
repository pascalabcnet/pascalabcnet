// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
// PreprocessorABC v.1.0

{
DataFrameABC
   ↓
PreprocessorABC
   ├─ IPreprocessor
   ├─ Scalers / Encoders / Imputer
   └─ Pipeline   
   ↓
MLCore 
}

/// PreprocessorABC — модуль подготовки табличных данных для анализа данных и машинного обучения.
/// Содержит типовые преобразования признаков с семантикой Fit / Transform.
/// Работает совместно с DataFrameABC и не содержит моделей машинного обучения.
unit PreprocessorABC;

interface

uses DataFrameABC;
uses System;
uses MLCoreABC;

type
/// Базовый интерфейс шагов подготовки данных.
/// Определяет семантику операций Fit и Transform
  IPreprocessor = interface(IDataStep)
    /// Анализирует DataFrame и и сохраняет параметры шага
    function Fit(df: DataFrame): IPreprocessor;
    /// Применяет сохранённые параметры к DataFrame.
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
  end;

/// Кодирует строковый категориальный столбец в числовые значения
/// Категории фиксируются при Fit
/// Работает только со строковыми столбцами
  LabelEncoder = class(IPreprocessor)
  private
    col: string;
    mapping: Dictionary<string, integer>;
    fitted: boolean;
  public
    /// Создаёт LabelEncoder для указанного столбца
    constructor Create(column: string);
  
    /// Определяет множество категорий и сохраняет их коды
    function Fit(df: DataFrame): IPreprocessor;
    /// Заменяет категории их числовыми кодами
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
  end;

/// Кодирует строковый категориальный столбец в набор бинарных (one-hot) столбцов
/// Категории фиксируются при Fit
/// Неизвестные категории приводят к ошибке
/// Пропущенные значения (NA) кодируются нулями
  OneHotEncoder = class(IPreprocessor)
  private
    col: string;
    categories: array of string;
    indexByValue: Dictionary<string, integer>;
    fitted: boolean;
  public
    /// Создаёт OneHotEncoder для указанного столбца
    constructor Create(column: string);
  
    /// Определяет множество категорий столбца
    function Fit(df: DataFrame): IPreprocessor;
    /// Заменяет столбец набором бинарных столбцов
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
  end;

  ImputeStrategy = (isMean, isConstant);

/// Заполняет пропущенные значения (NA) в числовых столбцах
/// Поддерживает стратегии isMean и isConstant
/// Работает только с Int и Float столбцами
  Imputer = class(IPreprocessor)
  private
    cols: array of string;
    strategy: ImputeStrategy;
    constants: array of object;
    means: array of real;
    fitted: boolean;
  public
    /// Создаёт Imputer с заданной стратегией заполнения
    constructor Create(strategy: ImputeStrategy; params columns: array of string);
    /// Создаёт Imputer с константной стратегией заполнения
    constructor Create(strategy: ImputeStrategy; value: object; params columns: array of string);
  
    /// Вычисляет значения для заполнения пропусков
    function Fit(df: DataFrame): IPreprocessor;
    /// Заполняет пропущенные значения в DataFrame
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
  end;
  
{/// Приводит числовые столбцы к нулевому среднему и единичному стандартному отклонению.
/// При Fit вычисляет среднее значение и стандартное отклонение столбцов.
/// Применяет преобразование: x' = (x - mean) / std.
/// Пропущенные значения (NA) сохраняются.
  DataStandardScaler = class(IPreprocessor)
  private
    cols: array of string;
    means: array of real;
    stds: array of real;
    fitted: boolean;
  public
    /// Создаёт StandardScaler для указанных числовых столбцов.
    constructor Create(params columns: array of string);
    /// Вычисляет среднее значение и стандартное отклонение для каждого столбца.
    function Fit(df: DataFrame): IPreprocessor;
    /// Возвращает DataFrame со стандартизованными числовыми столбцами.
    function Transform(df: DataFrame): DataFrame;
    /// Последовательно выполняет Fit и Transform.
    function FitTransform(df: DataFrame): DataFrame;
  end;
  
/// Приводит числовые столбцы к заданному диапазону значений
/// При Fit вычисляет минимальное и максимальное значения столбцов
/// Применяет преобразование: x' = (x - min) / (max - min)
/// Пропущенные значения (NA) сохраняются
  DataMinMaxScaler = class(IPreprocessor)
  private
    cols: array of string;
    mins: array of real;
    maxs: array of real;
    fitted: boolean;
  public
    /// Создаёт MinMaxScaler для указанных столбцов
    constructor Create(params columns: array of string);
  
    /// Вычисляет минимальные и максимальные значения столбцов
    function Fit(df: DataFrame): IPreprocessor;
    /// Применяет масштабирование к DataFrame
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Последовательно выполняет Fit и Transform.
    function FitTransform(df: DataFrame): DataFrame;
  end;}
    

implementation

uses MLExceptions;

const
  ER_SCALER_NO_COLUMNS =
    'StandardScaler: столбцы не указаны!!StandardScaler: columns not specified';
  ER_SCALER_COLUMN_NOT_NUMERIC =
    'StandardScaler: столбец "{0}" не является числовым!!StandardScaler: column "{0}" is not numeric';  
  ER_SCALER_NO_VALID_VALUES =
    'StandardScaler: столбец "{0}" не содержит допустимых значений!!' +
    'StandardScaler: column "{0}" has no valid values';
  ER_SCALER_ZERO_VARIANCE =
    'StandardScaler: нулевая дисперсия в столбце "{0}"!!' +
    'StandardScaler: zero variance in column "{0}"';
  ER_MINMAX_NO_COLUMNS =
    'MinMaxScaler: столбцы не указаны!!MinMaxScaler: columns not specified';
  ER_MINMAX_COLUMN_NOT_NUMERIC =
    'MinMaxScaler: столбец "{0}" не является числовым!!MinMaxScaler: column "{0}" is not numeric';
  ER_MINMAX_NO_VALID_VALUES =
    'MinMaxScaler: столбец "{0}" не содержит допустимых значений!!' +
    'MinMaxScaler: column "{0}" has no valid values';
  ER_MINMAX_CONSTANT_COLUMN =
    'MinMaxScaler: постоянный столбец "{0}"!!MinMaxScaler: constant column "{0}"';
  ER_LABELENCODER_NO_COLUMN =
    'LabelEncoder: столбец не указан!!LabelEncoder: column not specified';
  ER_LABELENCODER_NOT_STRING =
    'LabelEncoder: столбец "{0}" не является строковым!!' +
    'LabelEncoder: column "{0}" is not string';
  ER_LABELENCODER_NA =
    'LabelEncoder: NA значение не допускается!!LabelEncoder: NA value not allowed';
  ER_LABELENCODER_UNSEEN_CATEGORY =
    'LabelEncoder: неизвестная категория "{0}"!!' +
    'LabelEncoder: unseen category "{0}"';
  ER_ONEHOT_NO_COLUMN =
    'OneHotEncoder: столбец не указан!!OneHotEncoder: column not specified';
  ER_ONEHOT_NOT_STRING =
    'OneHotEncoder: столбец "{0}" не является строковым или не содержит допустимых значений!!' +
    'OneHotEncoder: column "{0}" is not string or has no valid values';
  ER_ONEHOT_UNSEEN_CATEGORY =
    'OneHotEncoder: неизвестная категория "{0}"!!' +
    'OneHotEncoder: unseen category "{0}"';
  ER_IMPUTER_INVALID_STRATEGY_MEAN =
    'Imputer: данный конструктор предназначен для стратегии isMean!!' +
    'Imputer: this constructor is for isMean';
  ER_IMPUTER_INVALID_STRATEGY_CONSTANT =
    'Imputer: данный конструктор предназначен для стратегии isConstant!!' +
    'Imputer: this constructor is for isConstant';
  ER_IMPUTER_NO_COLUMNS =
    'Imputer: столбцы не указаны!!Imputer: columns not specified';
  ER_IMPUTER_COLUMN_NOT_NUMERIC =
    'Imputer: столбец "{0}" не является числовым!!' +
    'Imputer: column "{0}" is not numeric';
  ER_IMPUTER_NO_VALID_VALUES =
    'Imputer(mean): столбец "{0}" не содержит допустимых значений!!' +
    'Imputer(mean): column "{0}" has no valid values';
  ER_IMPUTER_CONSTANT_VALUE_NULL =
    'Imputer(constant): значение nil для столбца "{0}"!!' +
    'Imputer(constant): value is nil for column "{0}"';
  ER_IMPUTER_CONSTANT_TYPE_MISMATCH =
    'Imputer(constant): несоответствие типа значения для столбца "{0}"!!' +
    'Imputer(constant): value type mismatch for column "{0}"';
  
//-----------------------------
//        StandardScaler
//-----------------------------

{constructor DataStandardScaler.Create(params columns: array of string);
begin
  if (columns = nil) or (columns.Length = 0) then
    ArgumentError(ER_SCALER_NO_COLUMNS);

  cols := columns;
  fitted := false;
end;

function DataStandardScaler.Fit(df: DataFrame): IPreprocessor;
begin
  var n := cols.Length;
  SetLength(means, n);
  SetLength(stds, n);

  for var i := 0 to n - 1 do
  begin
    var colName := cols[i];
    var idx := df.Schema.IndexOf(colName);

    var ct := df.Schema.ColumnTypeAt(idx);
    if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
      Error(ER_SCALER_COLUMN_NOT_NUMERIC, colName);

    var sum := 0.0;
    var sum2 := 0.0;
    var cnt := 0;

    var cur := df.GetCursor;
    while cur.MoveNext do
    begin
      if not cur.IsValid(idx) then continue;
      var x := cur.Float(idx);
      sum += x;
      sum2 += x * x;
      cnt += 1;
    end;

    if cnt = 0 then
      Error(ER_SCALER_NO_VALID_VALUES, colName);

    means[i] := sum / cnt;
    var v := sum2 / cnt - means[i] * means[i];
    stds[i] := Sqrt(v);

    if stds[i] = 0 then
      Error(ER_SCALER_ZERO_VARIANCE, colName);
  end;

  fitted := true;
  Result := Self;
end;

function DataStandardScaler.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var res := df;

  for var i := 0 to cols.Length - 1 do
  begin
    var colName := cols[i];
    var idx := df.Schema.IndexOf(colName);
    var mean := means[i];
    var std  := stds[i];

    res := res.ReplaceColumnFloat(
      colName,
      c ->
        (if c.IsValid(idx)
        then (c.Float(idx) - mean) / std
        else real.NaN)
    );
  end;

  Result := res;
end;

function DataStandardScaler.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

//-----------------------------
//        MinMaxScaler
//-----------------------------

constructor DataMinMaxScaler.Create(params columns: array of string);
begin
  if (columns = nil) or (columns.Length = 0) then
    ArgumentError(ER_MINMAX_NO_COLUMNS);

  cols := columns;
  fitted := false;
end;

function DataMinMaxScaler.Fit(df: DataFrame): IPreprocessor;
begin
  var n := cols.Length;
  SetLength(mins, n);
  SetLength(maxs, n);

  for var i := 0 to n - 1 do
  begin
    var colName := cols[i];
    var idx := df.Schema.IndexOf(colName);

    var ct := df.Schema.ColumnTypeAt(idx);
    if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
      Error(ER_MINMAX_COLUMN_NOT_NUMERIC, colName);

    var first := true;
    var minv, maxv: real;

    var cur := df.GetCursor;
    while cur.MoveNext do
    begin
      if not cur.IsValid(idx) then continue;
      var x := cur.Float(idx);

      if first then
      begin
        minv := x;
        maxv := x;
        first := false;
      end
      else
      begin
        if x < minv then minv := x;
        if x > maxv then maxv := x;
      end;
    end;

    if first then
      Error(ER_MINMAX_NO_VALID_VALUES, colName);

    if minv = maxv then
      Error(ER_MINMAX_CONSTANT_COLUMN, colName);

    mins[i] := minv;
    maxs[i] := maxv;
  end;

  fitted := true;
  Result := Self;
end;

function DataMinMaxScaler.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var res := df;

  for var i := 0 to cols.Length - 1 do
  begin
    var colName := cols[i];
    var idx := df.Schema.IndexOf(colName);
    var minv := mins[i];
    var maxv := maxs[i];
    var scale := maxv - minv;

    res := res.ReplaceColumnFloat(
      colName,
      c ->
        (if c.IsValid(idx)
        then (c.Float(idx) - minv) / scale
        else real.NaN)
    );
  end;

  Result := res;
end;

function DataMinMaxScaler.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;}

//-----------------------------
//        LabelEncoder
//-----------------------------

constructor LabelEncoder.Create(column: string);
begin
  if column = '' then
    ArgumentError(ER_LABELENCODER_NO_COLUMN);

  col := column;
  fitted := false;
end;

function LabelEncoder.Fit(df: DataFrame): IPreprocessor;
begin
  var idx := df.Schema.IndexOf(col);

  if df.Schema.ColumnTypeAt(idx) <> ColumnType.ctStr then
    Error(ER_LABELENCODER_NOT_STRING, col);

  mapping := new Dictionary<string, integer>;

  var cur := df.GetCursor;
  var nextId := 0;

  while cur.MoveNext do
  begin
    if not cur.IsValid(idx) then continue;

    var s := cur.Str(idx);
    if not mapping.ContainsKey(s) then
    begin
      mapping[s] := nextId;
      nextId += 1;
    end;
  end;

  fitted := true;
  Result := Self;
end;

function LabelEncoder.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var idx := df.Schema.IndexOf(col);

  Result := df.ReplaceColumnInt(
    col,
    c ->
      if not c.IsValid(idx) then
        Error(ER_LABELENCODER_NA)  // будет поймано как NA
      else
      begin
        var s := c.Str(idx);
        if not mapping.ContainsKey(s) then
          Error(ER_LABELENCODER_UNSEEN_CATEGORY, s);
        Result := mapping[s];
      end
  );
end;

function LabelEncoder.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

//-----------------------------
//        OneHotEncoder
//-----------------------------

constructor OneHotEncoder.Create(column: string);
begin
  if column = '' then
    ArgumentError(ER_ONEHOT_NO_COLUMN);
  col := column;
  fitted := false;
end;

function OneHotEncoder.Fit(df: DataFrame): IPreprocessor;
begin
  var idx := df.Schema.IndexOf(col);
  if df.Schema.ColumnTypeAt(idx) <> ColumnType.ctStr then
    Error(ER_ONEHOT_NOT_STRING, col);

  indexByValue := new Dictionary<string, integer>;
  var values := new List<string>;

  var cur := df.GetCursor;
  while cur.MoveNext do
  begin
    if not cur.IsValid(idx) then continue;
    var s := cur.Str(idx);
    if not indexByValue.ContainsKey(s) then
    begin
      indexByValue[s] := values.Count;
      values.Add(s);
    end;
  end;

  categories := values.ToArray;
  fitted := true;
  Result := Self;
end;

function OneHotEncoder.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var srcIdx := df.Schema.IndexOf(col);
  var catCount := categories.Length;

  // === ШАГ 1. Проверка на unseen категории ===
  var cur := df.GetCursor;
  while cur.MoveNext do
  begin
    if not cur.IsValid(srcIdx) then continue;

    var s := cur.Str(srcIdx);
    if not indexByValue.ContainsKey(s) then
      Error(ER_ONEHOT_UNSEEN_CATEGORY, s);
  end;

  // === ШАГ 2. Генерация one-hot столбцов ===
  var res := df;

  for var j := 0 to catCount - 1 do
  begin
    var catIdx := j;
    var newName := col + '_' + categories[j];

    var Encode: DataFrameCursor -> integer := c ->
    begin
      if not c.IsValid(srcIdx) then
      begin
        Result := 0;
        exit;
      end;

      if indexByValue[c.Str(srcIdx)] = catIdx then
        Result := 1
      else
        Result := 0;
    end;

    res := res.AddDerivedIntColumn(newName, Encode);
  end;

  // === ШАГ 3. Удаление исходного столбца ===
  res := res.Drop([srcIdx]);

  Result := res;
end;



function OneHotEncoder.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

//-----------------------------
//        Imputer
//-----------------------------

constructor Imputer.Create(strategy: ImputeStrategy; params columns: array of string);
begin
  if strategy <> isMean then
    ArgumentError(ER_IMPUTER_INVALID_STRATEGY_MEAN);

  if (columns = nil) or (columns.Length = 0) then
    ArgumentError(ER_IMPUTER_NO_COLUMNS);

  self.strategy := strategy;
  self.cols := columns;
  self.constants := nil;
  fitted := false;
end;

constructor Imputer.Create(strategy: ImputeStrategy; value: object; params columns: array of string);
begin
  if strategy <> isConstant then
    ArgumentError(ER_IMPUTER_INVALID_STRATEGY_CONSTANT);

  if (columns = nil) or (columns.Length = 0) then
    ArgumentError(ER_IMPUTER_NO_COLUMNS);

  self.strategy := strategy;
  self.cols := columns;

  // одна и та же константа для всех столбцов
  self.constants := new object[columns.Length];
  for var i := 0 to columns.Length - 1 do
    self.constants[i] := value;

  fitted := false;
end;

function Imputer.Fit(df: DataFrame): IPreprocessor;
begin
  if strategy = isMean then
  begin
    SetLength(means, cols.Length);

    for var i := 0 to cols.Length - 1 do
    begin
      var name := cols[i];
      var idx := df.Schema.IndexOf(name);
      var ct := df.Schema.ColumnTypeAt(idx);

      if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
        Error(ER_IMPUTER_COLUMN_NOT_NUMERIC, name);

      var sum := 0.0;
      var cnt := 0;

      var cur := df.GetCursor;
      while cur.MoveNext do
        if cur.IsValid(idx) then
        begin
          sum += cur.Float(idx);
          cnt += 1;
        end;

      if cnt = 0 then
        Error(ER_IMPUTER_NO_VALID_VALUES, name);

      means[i] := sum / cnt;
    end;
  end;

  fitted := true;
  Result := Self;
end;

function Imputer.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var res := df;

  for var i := 0 to cols.Length - 1 do
  begin
    var name := cols[i];
    var idx := df.Schema.IndexOf(name);
    var ct := df.Schema.ColumnTypeAt(idx);

    if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
      Error(ER_IMPUTER_COLUMN_NOT_NUMERIC, name);

    if strategy = isMean then
    begin
      var m := means[i];
      res := res.ReplaceColumnFloat(
        name,
        c -> (if c.IsValid(idx) then c.Float(idx) else m)
      );
    end
    else
    begin
      var v := constants[i];
      if v = nil then
        Error(ER_IMPUTER_CONSTANT_VALUE_NULL, name);

      if ct = ColumnType.ctInt then
      begin
        var k: integer;
        try
          k := integer(v);
        except
          on e: Exception do
            Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
        end;

        res := res.ReplaceColumnInt(
          name,
          c -> (if c.IsValid(idx) then c.Int(idx) else k)
        );
      end
      else
      begin
        var r: real;
        try
          r := real(v);
        except
          on e: Exception do
            Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
        end;

        res := res.ReplaceColumnFloat(
          name,
          c -> (if c.IsValid(idx) then c.Float(idx) else r)
        );
      end;
    end;
  end;

  Result := res;
end;

function Imputer.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;


end.