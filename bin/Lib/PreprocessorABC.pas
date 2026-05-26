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
uses DataFrameABCCore;
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
    
    /// Копирует только конфигурацию препроцессора (без обученного состояния).
    /// Используется для создания независимых экземпляров препроцессора.
    function Clone: IPreprocessor;
  end;

/// Кодирует строковый категориальный столбец в целочисленные индексы (0,1,2,...).
/// Соответствие значений и индексов фиксируется при вызове Fit
/// в порядке первого появления категорий.
/// Пропущенные значения (NA) игнорируются при обучении
/// и сохраняются как пропуски при преобразовании.
/// Работает только со строковыми столбцами и предназначен для признаков.
/// Не должен применяться к целевому столбцу (target)
  OrdinalEncoder = class(IPreprocessor, IColumnBoundStep)
  private
    col: string;
    mapping: Dictionary<string, integer>;
    fitted: boolean;
  public
    /// Создаёт OrdinalEncoder для указанного столбца
    constructor Create(column: string);
  
/// Определяет множество категорий столбца и сохраняет их числовое кодирование.
///   df — таблица данных.
/// Сохраняет отображение категорий в числовые коды для последующего применения.
///
/// Примечание:
///   • отображение категорий НЕ копируется методом Clone    
    function Fit(df: DataFrame): IPreprocessor;
    
    /// Заменяет категории их числовыми кодами.
    /// Неизвестные категории кодируются значением -1.
    /// Пропущенные значения сохраняются как пропуски.
    /// Возвращает новый DataFrame.
    function Transform(df: DataFrame): DataFrame;
    
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
    
    function ToString: string; override;
    
    property ColumnName: string read col;
    
/// Копирует только конфигурацию препроцессора (без обученного состояния).
/// Используется для создания независимых экземпляров препроцессора.
    function Clone: IPreprocessor;
  end;

/// Кодирует строковый категориальный столбец в набор бинарных (one-hot) столбцов
/// Категории фиксируются при Fit
/// Неизвестные категории кодируются нулями
/// Пропущенные значения (NA) кодируются нулями
  OneHotEncoder = class(IPreprocessor, IColumnBoundStep, IColumnExpander)
  private
    col: string;
    categories: array of string;
    indexByValue: Dictionary<string, integer>;
    fitted: boolean;
  public
    /// Создаёт OneHotEncoder для указанного столбца
    constructor Create(column: string);
  
/// Определяет множество категорий столбца.
///   df — таблица данных.
/// Сохраняет категории для последующего преобразования в one-hot представление.
///
/// Примечание:
///   • категории НЕ копируются методом Clone
    function Fit(df: DataFrame): IPreprocessor;
    
    /// Заменяет столбец набором бинарных столбцов
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
    
    function ToString: string; override;
    
    property ColumnName: string read col;
    
    function GetExpandedColumns(sourceColumn: string): array of string;

/// Копирует только конфигурацию препроцессора (без обученного состояния).
/// Используется для создания независимых экземпляров препроцессора.
    function Clone: IPreprocessor;
  end;

  ImputeStrategy = (isMean, isConstant, isMedian);

/// Заполняет пропущенные значения (NA) в столбцах DataFrame
/// Стратегии isMean и isMedian работают только с числовыми столбцами
/// Стратегия isConstant работает с любыми поддерживаемыми типами
  Imputer = class(IPreprocessor, IColumnsBoundStep)
  private
    cols: array of string;
    strategy: ImputeStrategy;
    constants: array of object;
    means: array of real;
    medians: array of real;
    fitted: boolean;
    function BuildImputedColumn(df: DataFrame; idx, imputerIndex: integer): Column;
  public
    /// Создаёт Imputer с заполнением средним значением
    constructor Create(columns: array of string);
    /// Создаёт Imputer с заданной стратегией заполнения
    constructor Create(strategy: ImputeStrategy; columns: array of string);
    /// Создаёт Imputer с константной стратегией заполнения
    constructor Create(value: object; columns: array of string);
  
/// Вычисляет значения для заполнения пропусков.
///   df — таблица данных.
/// В зависимости от стратегии вычисляет параметры заполнения
/// (например, среднее, медиану или константу) для каждого столбца.
///
/// Примечание:
///   • вычисленные параметры НЕ копируются методом Clone
    function Fit(df: DataFrame): IPreprocessor;
    
    /// Заполняет пропущенные значения в DataFrame
    /// Возвращает новый DataFrame
    function Transform(df: DataFrame): DataFrame;
    
    /// Выполняет Fit и Transform последовательно
    function FitTransform(df: DataFrame): DataFrame;
    
    function ToString: string; override;
    
    property Columns: array of string read cols;
    
/// Копирует только конфигурацию препроцессора (без обученного состояния).
/// Используется для создания независимых экземпляров препроцессора.
    function Clone: IPreprocessor;
  end;
  

implementation

uses MLExceptions;

const
  ER_ORDINALENCODER_NO_COLUMN =
    'OrdinalEncoder: столбец не указан!!OrdinalEncoder: column not specified';
  ER_ORDINALENCODER_NOT_STRING =
    'OrdinalEncoder: столбец "{0}" не является строковым!!' +
    'OrdinalEncoder: column "{0}" is not string';
  ER_ORDINALENCODER_UNSEEN_CATEGORY =
    'OrdinalEncoder: неизвестная категория "{0}"!!' +
    'OrdinalEncoder: unseen category "{0}"';
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
  ER_IMPUTER_CONSTANTS_INVALID =
    'Массив констант не задан или имеет неверный размер!!Constants array is null or has invalid length';
  ER_IMPUTER_STRATEGY_NOT_SUPPORTED =
    'Стратегия импутации {0} не поддерживается!!Imputation strategy {0} is not supported';
  ER_UNSUPPORTED_IMPUTE_STRATEGY =
    'Неподдерживаемая стратегия заполнения: {0}!!Unsupported impute strategy: {0}';
  ER_UNSUPPORTED_COLUMN_TYPE =
    'Неподдерживаемый тип столбца!!Unsupported column type';
  ER_ONEHOT_NAME_EQUALS_SOURCE =
    'Сгенерированная колонка совпадает с исходной: {0}!!Generated column equals source column: {0}';
  ER_ONEHOT_COLUMN_COLLISION =
    'Конфликт имён колонок: {0}!!Column name collision: {0}';
  ER_ONEHOT_DUPLICATE_COLUMN =
    'Дублирующаяся сгенерированная колонка: {0}!!Duplicate generated column: {0}';  
    
  
//-----------------------------
//        OrdinalEncoder
//-----------------------------

constructor OrdinalEncoder.Create(column: string);
begin
  if column = '' then
    ArgumentError(ER_ORDINALENCODER_NO_COLUMN);

  col := column;
  fitted := false;
end;

function OrdinalEncoder.Fit(df: DataFrame): IPreprocessor;
begin
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  
  if not df.HasColumn(col) then
    ArgumentError(ER_COLUMN_NOT_FOUND, col);

  var idx := df.Schema.IndexOf(col);

  if df.Schema.ColumnTypeAt(idx) <> ColumnType.ctStr then
    Error(ER_ORDINALENCODER_NOT_STRING, col);

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

function OrdinalEncoder.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  var idx := df.Schema.IndexOf(col);
  var n := df.RowCount;

  var data := new integer[n];
  var valid := new boolean[n];

  var cur := df.GetCursor;
  var row := 0;
  while cur.MoveNext do
  begin
    if not cur.IsValid(idx) then
    begin
      valid[row] := False;
      data[row] := 0;
    end
    else
    begin
      var s := cur.Str(idx);

      if s not in mapping then
        data[row] := -1
      else
        data[row] := mapping[s];
      
      valid[row] := True;
    end;

    row += 1;
  end;

  var res := new DataFrame;

  foreach var src in df.GetColumns do
    if src.Info.Name <> col then
      res.AddColumnAlias(src)
    else
      res.AddIntColumn(col, data, valid);

  var catCols := new List<string>;
  
  foreach var name in df.Schema.ColumnNames do
    if df.IsCategorical(name) and (name <> col) then
      catCols.Add(name);
  
  catCols.Add(col); // encoded колонка тоже categorical
  
  Result := res.SetCategorical(catCols.ToArray);
end;

function OrdinalEncoder.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

function OrdinalEncoder.ToString: string;
begin
  Result := 'OrdinalEncoder(' + col + ')';
end;

function OrdinalEncoder.Clone: IPreprocessor;
begin
  Result := new OrdinalEncoder(col);
end;

procedure AppendAllColumnsExcept(
  res, src: DataFrame;
  skipIndex: integer;
  names: List<string>;
  types: List<ColumnType>;
  cats: List<boolean>
);
begin
  for var i := 0 to src.ColumnCount - 1 do
    if i <> skipIndex then
    begin
      res.AddColumnAlias(src.GetColumn(i));
      names.Add(src.Schema.NameAt(i));
      types.Add(src.Schema.ColumnTypeAt(i));
      cats.Add(src.Schema.IsCategoricalAt(i));
    end;
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
  if df = nil then
    ArgumentNullError(ER_ARG_NULL, 'df');
  
  var idx := df.Schema.IndexOf(col);
  if idx < 0 then
    Error(ER_COLUMN_NOT_FOUND, col);

  if df.Schema.ColumnTypeAt(idx) <> ColumnType.ctStr then
    Error(ER_ONEHOT_NOT_STRING, col);

  indexByValue := new Dictionary<string, integer>;
  var values := new List<string>;

  var cur := df.GetCursor;
  while cur.MoveNext do
  begin
    if not cur.IsValid(idx) then continue;

    var s := cur.Str(idx);

    var dummy: integer;
    if not indexByValue.TryGetValue(s, dummy) then
    begin
      indexByValue[s] := values.Count;
      values.Add(s);
    end;
  end;
  
  if values.Count = 0 then
    Error(ER_ONEHOT_EMPTY_COLUMN, col);

  categories := values.ToArray;
  
  // --- проверка коллизий имён колонок
  var used := new HashSet<string>;
  
  for var i := 0 to categories.Length - 1 do
  begin
    var newName := col + '_' + categories[i];
  
    // 1. совпадение с исходным именем
    if newName = col then
      Error(ER_ONEHOT_NAME_EQUALS_SOURCE, newName);
  
    // 2. коллизия с существующими колонками DataFrame
    if df.HasColumn(newName) then
      Error(ER_ONEHOT_COLUMN_COLLISION, newName);
  
    // 3. дубликаты среди сгенерированных колонок
    if used.Contains(newName) then
      Error(ER_ONEHOT_DUPLICATE_COLUMN, newName);
  
    used.Add(newName);
  end;
  
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
  if df.Schema.ColumnTypeAt(srcIdx) <> ColumnType.ctStr then
    Error(ER_ONEHOT_NOT_STRING, col);
  
  var rowCount := df.RowCount;
  var catCount := categories.Length;
  var res := new DataFrame;
  
  var names := new List<string>;
  var types := new List<ColumnType>;
  var cats := new List<boolean>;
  
  AppendAllColumnsExcept(res, df, srcIdx, names, types, cats);
  
  var srcCol := StrColumn(df.GetColumn(srcIdx));
  var srcData := srcCol.Data;
  var srcValid := srcCol.IsValid;
  
  var dataCols := new List<array of integer>;
  var validCols := new List<array of boolean>;
  
  for var j := 0 to catCount - 1 do
  begin
    dataCols.Add(new integer[rowCount]);
    validCols.Add([True] * rowCount);
  end;
  
  for var row := 0 to rowCount - 1 do
  begin
    if (srcValid <> nil) and not srcValid[row] then
      continue;
    
    var s := srcData[row];
    
    var idx: integer;
    if indexByValue.TryGetValue(s, idx) then
      dataCols[idx][row] := 1;
  end;
  
  for var j := 0 to catCount - 1 do
  begin
    var newName := col + '_' + categories[j];
    res.AddIntColumn(newName, dataCols[j], validCols[j]);
    names.Add(newName);
    types.Add(ColumnType.ctInt);
    cats.Add(False);
  end;
  
  res.SetSchema(new DataFrameSchema(
    names.ToArray,
    types.ToArray,
    cats.ToArray
  ));
  
  Result := res;
end;

function OneHotEncoder.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

function OneHotEncoder.GetExpandedColumns(sourceColumn: string): array of string;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);
  
  if sourceColumn <> col then
    exit(nil);

  var res := new string[categories.Length];
  for var i := 0 to categories.Length - 1 do
    res[i] := col + '_' + categories[i];

  Result := res;
end;

function OneHotEncoder.ToString: string;
begin
  Result := 'OneHotEncoder(column=' + col + ')';
end;

function OneHotEncoder.Clone: IPreprocessor;
begin
  Result := new OneHotEncoder(col);
end;

//-----------------------------
//        Imputer
//-----------------------------

constructor Imputer.Create(strategy: ImputeStrategy; columns: array of string);
begin
  if (columns = nil) or (columns.Length = 0) then
    ArgumentError(ER_IMPUTER_NO_COLUMNS);

  self.strategy := strategy;
  self.cols := columns;
  self.constants := nil;
  fitted := false;
end;

constructor Imputer.Create(columns: array of string);
begin
  Create(ImputeStrategy.isMean, columns);
end;

constructor Imputer.Create(value: object; columns: array of string);
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
  case strategy of
    isMean:
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

    isConstant:
    begin
      // ничего делать не нужно
    end;
    
    isMedian:
    begin
      SetLength(medians, cols.Length);
    
      for var i := 0 to cols.Length - 1 do
      begin
        var name := cols[i];
        var idx := df.Schema.IndexOf(name);
        var ct := df.Schema.ColumnTypeAt(idx);
    
        if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
          Error(ER_IMPUTER_COLUMN_NOT_NUMERIC, name);
    
        // --- собираем значения
        var values := new List<real>;
    
        var cur := df.GetCursor;
        while cur.MoveNext do
          if cur.IsValid(idx) then
            values.Add(cur.Float(idx));
    
        if values.Count = 0 then
          Error(ER_IMPUTER_NO_VALID_VALUES, name);
    
        // --- сортируем
        values.Sort;
    
        // --- медиана
        var n := values.Count;
        if n mod 2 = 1 then
          medians[i] := values[n div 2]
        else
          medians[i] := (values[n div 2 - 1] + values[n div 2]) / 2.0;
      end;
    end;    
    
  end;

  fitted := true;
  Result := Self;
end;

function Imputer.BuildImputedColumn(df: DataFrame; idx, imputerIndex: integer): Column;
begin
  var name := cols[imputerIndex];
  var ct := df.Schema.ColumnTypeAt(idx);
  var capturedIdx := idx;
  
  case strategy of
    isMean:
    begin
      if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
        Error(ER_IMPUTER_COLUMN_NOT_NUMERIC, name);

      var m := means[imputerIndex];
      var rowCount := df.RowCount;
      var data := new real[rowCount];
      var valid := new boolean[rowCount];
      
      var cur := df.GetCursor;
      var row := 0;
      while cur.MoveNext do
      begin
        data[row] := if cur.IsValid(capturedIdx) then cur.Float(capturedIdx) else m;
        valid[row] := True;
        row += 1;
      end;
      
      Result := new FloatColumn(name, data, valid);
    end;

    isConstant:
    begin
      var v := constants[imputerIndex];
      if v = nil then
        Error(ER_IMPUTER_CONSTANT_VALUE_NULL, name);

      if ct = ColumnType.ctInt then
      begin
        var k: integer;

        if v is integer then
          k := integer(v)
        else if v is real then
        begin
          var r := real(v);
          var ir := Round(r);
          if Abs(r - ir) > 1e-9 then
            Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
          k := ir;
        end
        else
          Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
        
        var rowCount := df.RowCount;
        var data := new integer[rowCount];
        var valid := new boolean[rowCount];
        
        var cur := df.GetCursor;
        var row := 0;
        while cur.MoveNext do
        begin
          data[row] := if cur.IsValid(capturedIdx) then cur.Int(capturedIdx) else k;
          valid[row] := True;
          row += 1;
        end;
        
        Result := new IntColumn(name, data, valid);
      end
      else if ct = ColumnType.ctFloat then
      begin
        var r: real;
        try
          r := real(v);
        except
          on e: Exception do
            Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
        end;
        
        var rowCount := df.RowCount;
        var data := new real[rowCount];
        var valid := new boolean[rowCount];
        
        var cur := df.GetCursor;
        var row := 0;
        while cur.MoveNext do
        begin
          data[row] := if cur.IsValid(capturedIdx) then cur.Float(capturedIdx) else r;
          valid[row] := True;
          row += 1;
        end;
        
        Result := new FloatColumn(name, data, valid);
      end
      else if ct = ColumnType.ctStr then
      begin
        var s: string;
        try
          s := string(v);
        except
          on e: Exception do
            Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
        end;

        var rowCount := df.RowCount;
        var data := new string[rowCount];
        var valid := new boolean[rowCount];

        var cur := df.GetCursor;
        var row := 0;
        while cur.MoveNext do
        begin
          data[row] := if cur.IsValid(capturedIdx) then cur.Str(capturedIdx) else s;
          valid[row] := True;
          row += 1;
        end;

        Result := new StrColumn(name, data, valid);
      end
      else if ct = ColumnType.ctBool then
      begin
        var b: boolean;
        try
          b := boolean(v);
        except
          on e: Exception do
            Error(ER_IMPUTER_CONSTANT_TYPE_MISMATCH, name);
        end;

        var rowCount := df.RowCount;
        var data := new boolean[rowCount];
        var valid := new boolean[rowCount];

        var cur := df.GetCursor;
        var row := 0;
        while cur.MoveNext do
        begin
          data[row] := if cur.IsValid(capturedIdx) then cur.Bool(capturedIdx) else b;
          valid[row] := True;
          row += 1;
        end;

        Result := new BoolColumn(name, data, valid);
      end
      else
        Error(ER_UNSUPPORTED_COLUMN_TYPE, ct);
    end;

    isMedian:
    begin
      if not (ct in [ColumnType.ctInt, ColumnType.ctFloat]) then
        Error(ER_IMPUTER_COLUMN_NOT_NUMERIC, name);

      var m := medians[imputerIndex];
      var rowCount := df.RowCount;
      var data := new real[rowCount];
      var valid := new boolean[rowCount];
      
      var cur := df.GetCursor;
      var row := 0;
      while cur.MoveNext do
      begin
        data[row] := if cur.IsValid(capturedIdx) then cur.Float(capturedIdx) else m;
        valid[row] := True;
        row += 1;
      end;
      
      Result := new FloatColumn(name, data, valid);
    end;
    
    else
      Error(ER_IMPUTER_STRATEGY_NOT_SUPPORTED, strategy);
  end;
end;

function Imputer.Transform(df: DataFrame): DataFrame;
begin
  if not fitted then
    NotFittedError(ER_FIT_NOT_CALLED);

  // --- проверка constants
  if (strategy = isConstant) and 
     ((constants = nil) or (constants.Length <> cols.Length)) then
    Error(ER_IMPUTER_CONSTANTS_INVALID);

  var imputeCols := new HashSet<string>(cols);
  var res := new DataFrame;
  var names := new List<string>;
  var types := new List<ColumnType>;
  var cats := new List<boolean>;
  
  for var i := 0 to df.ColumnCount - 1 do
  begin
    var name := df.Schema.NameAt(i);
    var imputerIndex := cols.IndexOf(name);
    
    if (name in imputeCols) and (imputerIndex >= 0) then
    begin
      var col := BuildImputedColumn(df, i, imputerIndex);
      res.AddColumnAlias(col);
      names.Add(name);
      types.Add(col.Info.ColType);
      cats.Add(df.Schema.IsCategoricalAt(i));
    end
    else
    begin
      res.AddColumnAlias(df.GetColumn(i));
      names.Add(name);
      types.Add(df.Schema.ColumnTypeAt(i));
      cats.Add(df.Schema.IsCategoricalAt(i));
    end;
  end;
  
  res.SetSchema(new DataFrameSchema(
    names.ToArray,
    types.ToArray,
    cats.ToArray
  ));
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

    isMedian:
      Result := 'Imputer(strategy=median, columns=' + colsStr + ')';

    isConstant:
    begin
      var valStr :=
        if (constants <> nil) and (constants.Length > 0) and (constants[0] <> nil) then
          constants[0].ToString
        else
          'null';

      Result := 'Imputer(strategy=constant, value=' +
                valStr + ', columns=' + colsStr + ')';
    end;

    else
      Result := 'Imputer(strategy=unknown, columns=' + colsStr + ')';
  end;
end;

function Imputer.Clone: IPreprocessor;
begin
  case strategy of
    ImputeStrategy.isMean:
      Result := new Imputer(cols);

    ImputeStrategy.isMedian:
      Result := new Imputer(ImputeStrategy.isMedian, cols);

    ImputeStrategy.isConstant:
      begin
        var val := if (constants <> nil) and (constants.Length > 0) then constants[0] else nil;
        Result := new Imputer(val, cols);
      end;

    else
      Error(ER_UNSUPPORTED_IMPUTE_STRATEGY, strategy);
  end;
end;

end.
