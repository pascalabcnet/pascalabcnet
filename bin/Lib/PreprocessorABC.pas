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

/// Кодирует строковый категориальный столбец в целочисленные индексы (0,1,2,...).
/// Соответствие значений и индексов фиксируется при вызове Fit
/// в порядке первого появления категорий.
/// Работает только со строковыми столбцами и предназначен для признаков.
/// Не должен применяться к целевому столбцу (target).
  LabelEncoder = class(IPreprocessor, IColumnBoundStep)
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
    
    function ToString: string; override;
    
    property ColumnName: string read col;
  end;

/// Кодирует строковый категориальный столбец в набор бинарных (one-hot) столбцов
/// Категории фиксируются при Fit
/// Неизвестные категории кодируются нулями
/// Пропущенные значения (NA) кодируются нулями
  OneHotEncoder = class(IPreprocessor, IColumnBoundStep)
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
    
    function ToString: string; override;
    
    property ColumnName: string read col;
  end;

  ImputeStrategy = (isMean, isConstant);

/// Заполняет пропущенные значения (NA) в числовых столбцах
/// Поддерживает стратегии isMean и isConstant
/// Работает только с Int и Float столбцами
  Imputer = class(IPreprocessor, IColumnsBoundStep)
  private
    cols: array of string;
    strategy: ImputeStrategy;
    constants: array of object;
    means: array of real;
    fitted: boolean;
  public
    /// Создаёт Imputer с заполнением средним значением
    constructor Create(params columns: array of string);
    /// Создаёт Imputer с заданной стратегией заполнения
    constructor Create(strategy: ImputeStrategy; params columns: array of string);
    /// Создаёт Imputer с константной стратегией заполнения
    constructor Create(value: object; params columns: array of string);
  
    /// Вычисляет значения для заполнения пропусков
    function Fit(df: DataFrame): IPreprocessor;
    /// Заполняет пропущенные значения в DataFrame
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
    
    function ToString: string; override;
    
    property Columns: array of string read cols;
  end;
  

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
  ER_ONEHOT_EMPTY_COLUMN =
    'Столбец "{0}" не содержит категориальных значений!!Column "{0}" contains no categorical values';  
  ER_COLUMN_NOT_FOUND =
    'Столбец "{0}" не найден!!Column "{0}" not found';    
  
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
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  
  if not df.HasColumn(col) then
    ArgumentError(ER_COLUMN_NOT_FOUND, col);

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
  
  Result := Result.SetCategorical([col]);
end;

function LabelEncoder.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

function LabelEncoder.ToString: string;
begin
  Result := 'LabelEncoder(' + col + ')';
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
  
  if values.Count = 0 then
    Error(ER_ONEHOT_EMPTY_COLUMN, col);

  categories := values.ToArray;
  fitted := true;
  Result := Self;
end;

function OneHotEncoder.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var srcIdx := df.Schema.IndexOf(col);
  if srcIdx < 0 then
    ArgumentError(ER_COLUMN_NOT_FOUND, col);
  
  var catCount := categories.Length;

  var res := df;

  // === Генерация one-hot столбцов ===
  for var j := 0 to catCount - 1 do
  begin
    var catIdx := j;
    var newName := col + '_' + categories[j];

    var Encode: DataFrameCursor -> integer := c ->
    begin
      if not c.IsValid(col) then
      begin
        Result := 0;
        exit;
      end;
    
      var s := c.Str(col);
    
      var idx: integer;
      if not indexByValue.TryGetValue(s, idx) then
      begin
        Result := 0;
        exit;
      end;
    
      Result := Ord(idx = catIdx);
    end;

    res := res.AddDerivedIntColumn(newName, Encode);
  end;

  // === удаление исходного столбца ===
  res := res.Drop([srcIdx]);

  Result := res;
end;

function OneHotEncoder.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

function OneHotEncoder.ToString: string;
begin
  Result := 'OneHotEncoder(column=' + col + ')';
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

constructor Imputer.Create(params columns: array of string);
begin
  Create(ImputeStrategy.isMean, columns);
end;

constructor Imputer.Create(value: object; params columns: array of string);
begin
  if (columns = nil) or (columns.Length = 0) then
    ArgumentError(ER_IMPUTER_NO_COLUMNS);

  self.strategy := ImputeStrategy.isConstant;
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

function Imputer.ToString: string;
begin
  var colsStr := '[' + cols.JoinToString(', ') + ']';

  case strategy of
    isMean:
      Result := 'Imputer(strategy=mean, columns=' + colsStr + ')';

    isConstant:
      Result := 'Imputer(strategy=constant, value=' +
                constants[0].ToString + ', columns=' + colsStr + ')';
  end;
end;

end.