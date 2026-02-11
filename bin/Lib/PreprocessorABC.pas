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

type
/// Базовый интерфейс шагов подготовки данных.
/// Определяет семантику операций Fit и Transform
  IPreprocessor = interface
    /// Анализирует DataFrame и и сохраняет параметры шага
    function Fit(df: DataFrame): IPreprocessor;
    /// Применяет сохранённые параметры к DataFrame.
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
  end;

/// Приводит числовые столбцы к нулевому среднему и единичному стандартному отклонению.
/// При Fit вычисляет среднее значение и стандартное отклонение столбцов.
/// Применяет преобразование: x' = (x - mean) / std.
/// Пропущенные значения (NA) сохраняются.
  StandardScaler = class(IPreprocessor)
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
  MinMaxScaler = class(IPreprocessor)
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

  
type
/// Pipeline (конвейер) шагов подготовки данных.
/// Выполняет шаги последовательно с семантикой Fit / Transform.  Pipeline = class
  Pipeline = class
  private
    steps: List<IPreprocessor>;
    fitted: boolean;
  public
    constructor;
    /// Добавляет шаг в конец pipeline
    function Add(p: IPreprocessor): Pipeline;
    /// Обучает все шаги pipeline на DataFrame
    function Fit(df: DataFrame): Pipeline;
    /// Применяет обученный pipeline к DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
  end;

implementation

//-----------------------------
//        StandardScaler
//-----------------------------

constructor StandardScaler.Create(params columns: array of string);
begin
  if (columns = nil) or (columns.Length = 0) then
    raise new ArgumentException('StandardScaler: columns not specified');

  cols := columns;
  fitted := false;
end;

function StandardScaler.Fit(df: DataFrame): IPreprocessor;
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
      raise new Exception($'StandardScaler: column "{colName}" is not numeric');

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
      raise new Exception($'StandardScaler: column "{colName}" has no valid values');

    means[i] := sum / cnt;
    var v := sum2 / cnt - means[i] * means[i];
    stds[i] := Sqrt(v);

    if stds[i] = 0 then
      raise new Exception($'StandardScaler: zero variance in column "{colName}"');
  end;

  fitted := true;
  Result := Self;
end;

function StandardScaler.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    raise new Exception('StandardScaler: not fitted');

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

function StandardScaler.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

//-----------------------------
//        MinMaxScaler
//-----------------------------

constructor MinMaxScaler.Create(params columns: array of string);
begin
  if (columns = nil) or (columns.Length = 0) then
    raise new ArgumentException('MinMaxScaler: columns not specified');

  cols := columns;
  fitted := false;
end;

function MinMaxScaler.Fit(df: DataFrame): IPreprocessor;
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
      raise new Exception($'MinMaxScaler: column "{colName}" is not numeric');

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
      raise new Exception($'MinMaxScaler: column "{colName}" has no valid values');

    if minv = maxv then
      raise new Exception($'MinMaxScaler: constant column "{colName}"');

    mins[i] := minv;
    maxs[i] := maxv;
  end;

  fitted := true;
  Result := Self;
end;

function MinMaxScaler.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    raise new Exception('MinMaxScaler: not fitted');

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

function MinMaxScaler.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

//-----------------------------
//        LabelEncoder
//-----------------------------

constructor LabelEncoder.Create(column: string);
begin
  if column = '' then
    raise new ArgumentException('LabelEncoder: column not specified');

  col := column;
  fitted := false;
end;

function LabelEncoder.Fit(df: DataFrame): IPreprocessor;
begin
  var idx := df.Schema.IndexOf(col);

  if df.Schema.ColumnTypeAt(idx) <> ColumnType.ctStr then
    raise new Exception($'LabelEncoder: column "{col}" is not string');

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
    raise new Exception('LabelEncoder: not fitted');

  var idx := df.Schema.IndexOf(col);

  Result := df.ReplaceColumnInt(
    col,
    c ->
      if not c.IsValid(idx) then
        raise new Exception('NA')  // будет поймано как NA
      else
      begin
        var s := c.Str(idx);
        if not mapping.ContainsKey(s) then
          raise new Exception(
            $'LabelEncoder: unseen category "{s}"'
          );
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
    raise new ArgumentException('OneHotEncoder: column not specified');
  col := column;
  fitted := false;
end;

function OneHotEncoder.Fit(df: DataFrame): IPreprocessor;
begin
  var idx := df.Schema.IndexOf(col);
  if df.Schema.ColumnTypeAt(idx) <> ColumnType.ctStr then
  raise new Exception(
    $'OneHotEncoder: column "{col}" is not string or has no valid values'
  );

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
    raise new Exception('OneHotEncoder: not fitted');

  var srcIdx := df.Schema.IndexOf(col);
  var catCount := categories.Length;

  // === ШАГ 1. Проверка на unseen категории ===
  var cur := df.GetCursor;
  while cur.MoveNext do
  begin
    if not cur.IsValid(srcIdx) then continue;

    var s := cur.Str(srcIdx);
    if not indexByValue.ContainsKey(s) then
      raise new Exception($'OneHotEncoder: unseen category "{s}"');
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
    raise new ArgumentException('Imputer: this constructor is for isMean');

  if (columns = nil) or (columns.Length = 0) then
    raise new ArgumentException('Imputer: columns not specified');

  self.strategy := strategy;
  self.cols := columns;
  self.constants := nil;
  fitted := false;
end;

constructor Imputer.Create(strategy: ImputeStrategy; value: object; params columns: array of string);
begin
  if strategy <> isConstant then
    raise new ArgumentException('Imputer: this constructor is for isConstant');

  if (columns = nil) or (columns.Length = 0) then
    raise new ArgumentException('Imputer: columns not specified');

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
        raise new Exception($'Imputer(mean): column "{name}" is not numeric');

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
        raise new Exception($'Imputer(mean): column "{name}" has no valid values');

      means[i] := sum / cnt;
    end;
  end;

  fitted := true;
  Result := Self;
end;

function Imputer.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    raise new Exception('Imputer: not fitted');

  var res := df;

  for var i := 0 to cols.Length - 1 do
  begin
    var name := cols[i];
    var idx := df.Schema.IndexOf(name);
    var ct := df.Schema.ColumnTypeAt(idx);

    if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
      raise new Exception($'Imputer: column "{name}" is not numeric');

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
        raise new Exception($'Imputer(constant): value is nil for column "{name}"');

      if ct = ColumnType.ctInt then
      begin
        var k: integer;
        try
          k := integer(v);
        except
          on e: Exception do
            raise new Exception($'Imputer(constant): value type mismatch for column "{name}"');
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
            raise new Exception($'Imputer(constant): value type mismatch for column "{name}"');
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

//-----------------------------
//          Pipeline
//-----------------------------

constructor Pipeline.Create;
begin
  steps := new List<IPreprocessor>;
  fitted := false;
end;

function Pipeline.Add(p: IPreprocessor): Pipeline;
begin
  if fitted then
    raise new Exception('Cannot add step after Fit');

  steps.Add(p);
  Result := Self;
end;

function Pipeline.Fit(df: DataFrame): Pipeline;
begin
  var current := df;

  for var i := 0 to steps.Count - 1 do
  begin
    steps[i] := steps[i].Fit(current);
    current := steps[i].Transform(current);
  end;

  fitted := true;
  Result := Self;
end;

function Pipeline.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    raise new Exception('Pipeline is not fitted');

  var current := df;
  foreach var step in steps do
    current := step.Transform(current);

  Result := current;
end;

function Pipeline.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;


end.