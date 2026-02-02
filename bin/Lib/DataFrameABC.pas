// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

/// Стандартный модуль для работы с табличными данными (датасетами)
/// !! DataFrame module for tabular data processing
unit DataFrameABC;

interface

uses DataFrameABCCore;

const DataFrameVersion = '1.0';

// Реэкспортируем публичные типы для удобства
type
  /// Тип соединения (join) таблиц
  JoinKind = (jkInner, jkLeft, jkRight, jkFull);
  /// Тип столбца
  ColumnType = DataFrameABCCore.ColumnType;
  /// Информация о столбце
  ColumnInfo = DataFrameABCCore.ColumnInfo;
  /// Курсор для итерации по строкам DataFrame
  DataFrameCursor = DataFrameABCCore.DataFrameCursor;
  /// Предикат для фильтрации строк
  CursorPredicate = DataFrameABCCore.CursorPredicate;
  /// Статистическая информация о числовом столбце
  DescribeStats = DataFrameABCCore.DescribeStats;
  
  IGroupByContext = interface;
  
/// Класс для работы с табличными данными (датасетами).
/// Поддерживает основные операции: фильтрацию, сортировку, группировку, 
/// соединение таблиц и статистический анализ 
  DataFrame = class
  private
    columns: List<Column>;
    columnIndexByName: Dictionary<string, integer>;
    
    procedure RebuildColumnIndex;
    function GetColumnIndex(name: string): integer;
    function ResolveKeyIndices(keys: array of string): array of integer;
    
    // Join методы
    function CreateInnerJoinResult(other: DataFrame; leftKeyIdx, rightKeyIdx: array of integer): DataFrame;
    procedure AddColumnClone(src: Column);
    procedure AppendJoinedRow(leftCur, rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
    procedure AppendLeftOnlyRow(leftCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
    procedure AppendRightOnlyRow(rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer; leftColumnCount: integer);
    
    // Single key методы
    function JoinInnerSingleKey(other: DataFrame; key: string): DataFrame;
    function JoinInnerSingleKeyInt(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    function JoinInnerSingleKeyFloat(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    function JoinInnerSingleKeyStr(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    function JoinInnerSingleKeyBool(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    
    function LeftJoinSingleKey(other: DataFrame; key: string): DataFrame;
    function LeftJoinSingleKeyInt(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    function LeftJoinSingleKeyFloat(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    function LeftJoinSingleKeyStr(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    function LeftJoinSingleKeyBool(other: DataFrame; leftKey, rightKey: integer): DataFrame;
    
    function FullJoinSingleKey(other: DataFrame; key: string): DataFrame;
    
    // Multi key методы
    function JoinInnerMultiKey(other: DataFrame; keys: array of string): DataFrame;
    function LeftJoinMultiKey(other: DataFrame; keys: array of string): DataFrame;
    function FullJoinMultiKey(other: DataFrame; keys: array of string): DataFrame;
    
    // Вспомогательные для Join
    function BuildHashIndex(layout: JoinKeyLayout): Dictionary<JoinKey, List<integer>>;
    function BuildJoinKeyLayout(keyIndices: array of integer): JoinKeyLayout;
    function BuildJoinKey(cur: DataFrameCursor; layout: JoinKeyLayout; var hasNA: boolean): JoinKey;
    
    procedure AssertSchemaConsistent; // Проверка инвариантов в Debug
  public
    /// Создает пустой DataFrame
    constructor Create;
    
    /// Возвращает количество строк в DataFrame
    function RowCount: integer;
    /// Возвращает количество столбцов в DataFrame
    function ColumnCount: integer;
    /// Возвращает массив имен всех столбцов
    function ColumnNames: array of string;
    /// Возвращает индекс столбца по имени
    function ColumnIndex(name: string): integer;
    
    /// Создает курсор для итерации по строкам
    function GetCursor: DataFrameCursor;
    
    /// Добавляет столбец целых чисел
    procedure AddIntColumn(name: string; data: array of integer; valid: array of boolean; isCategorical: boolean := false);
    /// Добавляет столбец вещественных чисел
    procedure AddFloatColumn(name: string; data: array of real; valid: array of boolean);
    /// Добавляет строковый столбец
    procedure AddStrColumn(name: string; data: array of string; valid: array of boolean; isCategorical: boolean := true);
    /// Добавляет булев столбец
    procedure AddBoolColumn(name: string; data: array of boolean; valid: array of boolean);
    
    /// Возвращает данные целочисленного столбца по имени
    function GetIntColumn(name: string): array of integer;
    /// Возвращает данные вещественного столбца по имени
    function GetFloatColumn(name: string): array of real;
    
    /// Вычисляет сумму значений в столбце по индексу
    function Sum(colIndex: integer): real; 
    /// Вычисляет сумму значений в столбце по имени
    function Sum(colName: string): real; 
    /// Подсчитывает количество валидных значений в столбце по индексу
    function Count(colIndex: integer): integer; 
    /// Подсчитывает количество валидных значений в столбце по имени
    function Count(colName: string): integer; 
    /// Вычисляет среднее значение в столбце по индексу
    function Mean(colIndex: integer): real; 
    /// Вычисляет среднее значение в столбце по имени
    function Mean(colName: string): real; 
    /// Находит минимальное значение в столбце по индексу
    function Min(colIndex: integer): real; 
    /// Находит минимальное значение в столбце по имени
    function Min(colName: string): real; 
    /// Находит максимальное значение в столбце по индексу
    function Max(colIndex: integer): real;
    /// Находит максимальное значение в столбце по имени
    function Max(colName: string): real;
    /// Находит минимальное и максимальное значения в столбце по индексу
    function MinMax(colIndex: integer): (real, real); 
    /// Находит минимальное и максимальное значения в столбце по имени
    function MinMax(colName: string): (real, real); 
    /// Вычисляет дисперсию значений в столбце по индексу
    function Variance(colIndex: integer): real; 
    /// Вычисляет дисперсию значений в столбце по имени
    function Variance(colName: string): real; 
    /// Вычисляет стандартное отклонение в столбце по индексу
    function Std(colIndex: integer): real; 
    /// Вычисляет стандартное отклонение в столбце по имени
    function Std(colName: string): real; 
    /// Вычисляет среднее и дисперсию в столбце по индексу
    function MeanVariance(colIndex: integer): (real, real); 
    /// Вычисляет среднее и дисперсию в столбце по имени
    function MeanVariance(colName: string): (real, real); 
    
    /// Возвращает полную статистику по столбцу по индексу
    function Describe(colIndex: integer): DescribeStats; 
    /// Возвращает полную статистику по столбцу по имени
    function Describe(colName: string): DescribeStats; 
    /// Возвращает статистику по нескольким столбцам по именам
    function Describe(colNames: array of string): Dictionary<string, DescribeStats>; 
    /// Возвращает статистику по нескольким столбцам по индексам
    function Describe(colIndices: array of integer): Dictionary<integer, DescribeStats>; 
    /// Возвращает статистику по всем числовым столбцам
    function Describe: Dictionary<string, DescribeStats>; 
    
    /// Группирует данные по одному столбцу по индексу
    function GroupBy(colIndex: integer): IGroupByContext; 
    /// Группирует данные по одному столбцу по имени
    function GroupBy(colName: string): IGroupByContext; 
    /// Группирует данные по нескольким столбцам по индексам
    function GroupBy(colIndices: array of integer): IGroupByContext; 
    /// Группирует данные по нескольким столбцам по именам
    function GroupBy(colNames: array of string): IGroupByContext; 
    
    /// Возвращает первые n строк DataFrame
    function Head(n: integer): DataFrame;
    /// Возвращает последние n строк DataFrame
    function Tail(n: integer): DataFrame;
    
    /// Фильтрует строки по предикату
    function Filter(pred: CursorPredicate): DataFrame;
    /// Выбирает указанные столбцы по индексам (семантика view)
    function Select(colIndices: array of integer): DataFrame; 
    /// Выбирает указанные столбцы по именам
    function Select(colNames: array of string): DataFrame; 
    /// Сортирует DataFrame по одному столбцу по индексу
    function SortBy(colIndex: integer; descending: boolean := false): DataFrame; 
    /// Сортирует DataFrame по одному столбцу по имени
    function SortBy(colName: string; descending: boolean := false): DataFrame; 
    /// Сортирует DataFrame по нескольким столбцам по индексам
    function SortBy(colIndices: array of integer; descending: array of boolean): DataFrame; 
    /// Сортирует DataFrame по нескольким столбцам по именам
    function SortBy(colNames: array of string; descending: array of boolean): DataFrame; 
    
    /// Удаляет указанные столбцы по индексам
    function Drop(colIndices: array of integer): DataFrame; 
    /// Удаляет указанные столбцы по именам
    function Drop(colNames: array of string): DataFrame; 
    
    /// Переименовывает столбец по индексу
    function Rename(colIndex: integer; newName: string): DataFrame; 
    /// Переименовывает столбец по старому имени
    function Rename(oldName, newName: string): DataFrame; 
    /// Переименовывает несколько столбцов
    function Rename(pairs: array of (string, string)): DataFrame; 
    
    /// Добавляет вычисляемый столбец целых чисел
    function WithColumnInt(name: string; f: DataFrameCursor -> integer): DataFrame;
    /// Добавляет вычисляемый столбец целых чисел (сокращенная версия)
    function WithColumn(name: string; f: DataFrameCursor -> integer): DataFrame := WithColumnInt(name, f);
    /// Добавляет вычисляемый столбец вещественных чисел
    function WithColumnFloat(name: string; f: DataFrameCursor -> real): DataFrame;
    /// Добавляет вычисляемый строковый столбец
    function WithColumnStr(name: string; f: DataFrameCursor -> string): DataFrame;
    /// Добавляет вычисляемый булев столбец
    function WithColumnBool(name: string; f: DataFrameCursor -> boolean): DataFrame;
    
    /// Соединяет с другим DataFrame по нескольким ключам
    function Join(other: DataFrame; keys: array of string; kind: JoinKind := jkInner): DataFrame; 
    /// Соединяет с другим DataFrame по одному ключу
    function Join(other: DataFrame; key: string; kind: JoinKind := jkInner): DataFrame; 
    /// Соединяет с другим DataFrame по разным именам ключей
    function Join(other: DataFrame; leftKeys, rightKeys: array of string; kind: JoinKind := jkInner): DataFrame; 
    
    /// Выводит DataFrame в консоль с ограниченным количеством строк
    procedure Print(decimals: integer := 3);
    /// Выводит DataFrame в консоль и переходит на новую строку
    procedure Println(decimals: integer := 3);
    /// Выводит предпросмотр DataFrame с настройкой отображаемых строк
    procedure PrintPreview(maxRows: integer; headRows: integer := -1; decimals: integer := 3);
    /// Выводит предпросмотр DataFrame и переходит на новую строку
    procedure PrintlnPreview(maxRows: integer; headRows: integer := -1; decimals: integer := 3);

    /// Загружает DataFrame из CSV файла
    static function FromCsv(filename: string): DataFrame;
    /// Загружает DataFrame из многострочной строки в формате CSV
    static function FromCsvText(text: string): DataFrame;
  private
    /// Проверяет валидность индекса столбца
    procedure CheckColumnIndex(colIndex: integer);
    /// Добавляет строку из курсора
    procedure AppendRowFromCursor(src: DataFrame; cur: DataFrameCursor);
  end;
  
  /// Интерфейс для группировки данных
  IGroupByContext = interface
    /// Возвращает DataFrame с количеством строк в каждой группе
    function Count: DataFrame;
    /// Возвращает DataFrame со средними значениями указанного столбца по группам
    function Mean(colName: string): DataFrame;
    /// Возвращает DataFrame с полной статистикой указанного столбца по группам
    function Describe(colName: string): DataFrame;
    /// Возвращает DataFrame с полной статистикой всех числовых столбцов по группам
    function DescribeAll: DataFrame;
  end; 
 
type
  /// Статический класс для загрузки данных из CSV файлов
  CsvLoader = static class
    /// Загружает DataFrame из CSV файла
    static function Load(filename: string; delimiter: char := ','; hasHeader: boolean := true;      
      missingValues: array of string := nil; // Значения, считающиеся пропущенными
      strict: boolean := False;        // Строгий режим (проверка формата)
      schema: Dictionary<string, ColumnType> := nil // Схема типов столбцов
    ): DataFrame;
    
    /// Загружает DataFrame из многострочной строки в формате CSV
    static function LoadFromLines(
      lines: sequence of string;
      delimiter: char;
      hasHeader: boolean;
      missingValues: array of string;
      strict: boolean;
      schema: Dictionary<string, ColumnType>
    ): DataFrame;
  end;

implementation

type
  /// Класс для группировки данных
  GroupByContext = class(IGroupByContext)
  private
    source: DataFrame;
    singleKey: boolean;
    keyColumn: integer;
    groups1: Dictionary<object, List<integer>>;
    keyColumns: array of integer;
    groupsN: Dictionary<array of object, List<integer>>;
    
    procedure GetNumericColumn(
      colIndex: integer;
      var dataInt: array of integer;
      var dataFloat: array of real;
      var valid: array of boolean;
      var isInt: boolean
    );
    
  public
    /// Создает контекст группировки для указанных столбцов
    constructor Create(df: DataFrame; keyCols: array of integer);
    /// Возвращает DataFrame с количеством строк в каждой группе
    function Count: DataFrame;
    /// Возвращает DataFrame со средними значениями указанного столбца по группам
    function Mean(colName: string): DataFrame;
    /// Возвращает DataFrame с полной статистикой указанного столбца по группам
    function Describe(colName: string): DataFrame;
    /// Возвращает DataFrame с полной статистикой всех числовых столбцов по группам
    function DescribeAll: DataFrame;
  end;

//-----------------------------
//          DataFrame
//-----------------------------

constructor DataFrame.Create;
begin
  columns := [];
end;

function DataFrame.GetColumnIndex(name: string): integer;
begin
  if columnIndexByName.ContainsKey(name) then
    Result := columnIndexByName[name]
  else
    Result := -1;
end;

function DataFrame.ResolveKeyIndices(keys: array of string): array of integer;
begin
  Result := new integer[keys.Length];
  for var i := 0 to keys.Length - 1 do
  begin
    if not columnIndexByName.ContainsKey(keys[i]) then
      raise new Exception('Column not found: ' + keys[i]);
    Result[i] := columnIndexByName[keys[i]];
  end;
end;

function DataFrame.BuildJoinKey(cur: DataFrameCursor; layout: JoinKeyLayout; var hasNA: boolean): JoinKey;
begin
  hasNA := false;

  var ic := 0; var fc := 0; var sc := 0; var bc := 0;

  // считаем размеры
  for var i := 0 to layout.ColTypes.Length - 1 do
    case layout.ColTypes[i] of
      ctInt: inc(ic);
      ctFloat: inc(fc);
      ctStr: inc(sc);
      ctBool: inc(bc);
    end;

  Result.Ints := new integer[ic];
  Result.Floats := new real[fc];
  Result.Strs := new string[sc];
  Result.Bools := new boolean[bc];

  ic := 0; fc := 0; sc := 0; bc := 0;

  for var i := 0 to layout.ColIndices.Length - 1 do
  begin
    var col := layout.ColIndices[i];
    if not cur.IsValid(col) then
    begin
      hasNA := true;
      exit;
    end;

    case layout.ColTypes[i] of
      ctInt:
        begin
          Result.Ints[ic] := cur.Int(col);
          inc(ic);
        end;
      ctFloat:
        begin
          Result.Floats[fc] := cur.Float(col);
          inc(fc);
        end;
      ctStr:
        begin
          Result.Strs[sc] := cur.Str(col);
          inc(sc);
        end;
      ctBool:
        begin
          Result.Bools[bc] := cur.Bool(col);
          inc(bc);
        end;
    end;
  end;
end;

function DataFrame.BuildHashIndex(layout: JoinKeyLayout): Dictionary<JoinKey, List<integer>>;
begin
  Result := new Dictionary<JoinKey, List<integer>>();

  var cur := GetCursor;
  while cur.MoveNext do
  begin
    var hasNA := false;
    var key := BuildJoinKey(cur, layout, hasNA);
    if hasNA then continue;

    if not Result.ContainsKey(key) then
      Result[key] := new List<integer>;

    Result[key].Add(cur.Position);
  end;
end;

function DataFrame.CreateInnerJoinResult(other: DataFrame; leftKeyIdx, rightKeyIdx: array of integer): DataFrame;
begin
  Result := new DataFrame;

  // ключевые столбцы (слева)
  for var i := 0 to leftKeyIdx.Length - 1 do
    Result.AddColumnClone(columns[leftKeyIdx[i]]);

  // остальные столбцы слева
  for var j := 0 to columns.Count - 1 do
    if not leftKeyIdx.Contains(j) then
      Result.AddColumnClone(columns[j]);

  // остальные столбцы справа
  for var j := 0 to other.columns.Count - 1 do
    if not rightKeyIdx.Contains(j) then
      Result.AddColumnClone(other.columns[j]);
    
  Result.RebuildColumnIndex;  
end;

procedure DataFrame.AddColumnClone(src: Column);
begin
  case src.Info.ColType of
    ctInt:   AddIntColumn(src.Info.Name, new integer[0], nil, src.Info.IsCategorical);
    ctFloat: AddFloatColumn(src.Info.Name, new real[0], nil);
    ctStr:   AddStrColumn(src.Info.Name, new string[0], nil, src.Info.IsCategorical);
    ctBool:  AddBoolColumn(src.Info.Name, new boolean[0], nil);
  end;
end;

procedure DataFrame.AppendJoinedRow(leftCur, rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
begin
  var col := 0;

  // 1. ключи (из left)
  for var i := 0 to leftKeyIdx.Length - 1 do
  begin
    columns[col].AppendFromCursor(leftCur, leftKeyIdx[i]);
    col += 1;
  end;

  // 2. остальные столбцы слева
  for var j := 0 to leftCur.ColumnCount - 1 do
    if not leftKeyIdx.Contains(j) then
    begin
      columns[col].AppendFromCursor(leftCur, j);
      col += 1;
    end;

  // 3. остальные столбцы справа
  for var j := 0 to rightCur.ColumnCount - 1 do
    if not rightKeyIdx.Contains(j) then
    begin
      columns[col].AppendFromCursor(rightCur, j);
      col += 1;
    end;
end;

procedure DataFrame.AppendLeftOnlyRow(leftCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
begin
  var col := 0;

  // ключи (из left)
  for var i := 0 to leftKeyIdx.Length - 1 do
  begin
    columns[col].AppendFromCursor(leftCur, leftKeyIdx[i]);
    col += 1;
  end;

  // остальные столбцы слева
  for var j := 0 to leftCur.ColumnCount - 1 do
    if not leftKeyIdx.Contains(j) then
    begin
      columns[col].AppendFromCursor(leftCur, j);
      col += 1;
    end;

  while col < columns.Count do
  begin
    columns[col].AppendInvalid;
    col += 1;
  end;
end;

procedure DataFrame.AppendRightOnlyRow(rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer;
  leftColumnCount: integer);
begin
  var col := 0;

  // ключи (из right)
  for var i := 0 to rightKeyIdx.Length - 1 do
  begin
    columns[col].AppendFromCursor(rightCur, rightKeyIdx[i]);
    col += 1;
  end;

  // остальные столбцы слева → NA
  for var j := 0 to leftColumnCount - 1 do
    if not leftKeyIdx.Contains(j) then
    begin
      columns[col].AppendInvalid;
      col += 1;
    end;

  // остальные столбцы справа
  for var j := 0 to rightCur.ColumnCount - 1 do
    if not rightKeyIdx.Contains(j) then
    begin
      columns[col].AppendFromCursor(rightCur, j);
      col += 1;
    end;
end;


function DataFrame.LeftJoinSingleKey(other: DataFrame; key: string): DataFrame;
begin
  var leftKey := ResolveKeyIndices([key])[0];
  var rightKey := other.ResolveKeyIndices([key])[0];

  var lt := columns[leftKey].Info.ColType;
  var rt := other.columns[rightKey].Info.ColType;

  if lt <> rt then
    raise new Exception('Join key types mismatch');

  case lt of
    ctInt:
      Result := LeftJoinSingleKeyInt(other, leftKey, rightKey);
    ctFloat:
      Result := LeftJoinSingleKeyFloat(other, leftKey, rightKey);
    ctStr:
      Result := LeftJoinSingleKeyStr(other, leftKey, rightKey);
    ctBool:
      Result := LeftJoinSingleKeyBool(other, leftKey, rightKey);
  end;
end;


function DataFrame.LeftJoinSingleKeyInt(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<integer, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Int(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
    begin
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
      continue;
    end;

    var k := lcur.Int(leftKey);

    if index.ContainsKey(k) then
    begin
      foreach var rpos in index[k] do
      begin
        rcur.MoveTo(rpos);
        res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
      end;
    end
    else
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
  end;

  Result := res;
end;

function DataFrame.LeftJoinSingleKeyFloat(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<real, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Float(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
    begin
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
      continue;
    end;

    var k := lcur.Float(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        rcur.MoveTo(rpos);
        res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
      end
    else
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
  end;

  Result := res;
end;

function DataFrame.LeftJoinSingleKeyStr(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<string, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Str(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
    begin
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
      continue;
    end;

    var k := lcur.Str(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        rcur.MoveTo(rpos);
        res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
      end
    else
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
  end;

  Result := res;
end;

function DataFrame.LeftJoinSingleKeyBool(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<boolean, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Bool(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
    begin
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
      continue;
    end;

    var k := lcur.Bool(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        rcur.MoveTo(rpos);
        res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
      end
    else
      res.AppendLeftOnlyRow(lcur, [leftKey], [rightKey]);
  end;

  Result := res;
end;


function DataFrame.LeftJoinMultiKey(other: DataFrame; keys: array of string): DataFrame;
begin
  var leftKeyIdx := ResolveKeyIndices(keys);
  var rightKeyIdx := other.ResolveKeyIndices(keys);

  var leftLayout := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  var hash := other.BuildHashIndex(rightLayout);
  var res := CreateInnerJoinResult(other, leftKeyIdx, rightKeyIdx);

  var lcur := GetCursor;
  var rcur := other.GetCursor;

  while lcur.MoveNext do
  begin
    var hasNA := false;
    var key := BuildJoinKey(lcur, leftLayout, hasNA);

    if (not hasNA) and hash.ContainsKey(key) then
    begin
      foreach var rpos in hash[key] do
      begin
        rcur.MoveTo(rpos);
        res.AppendJoinedRow(lcur, rcur, leftKeyIdx, rightKeyIdx);
      end;
    end
    else
      res.AppendLeftOnlyRow(lcur, leftKeyIdx, rightKeyIdx);
  end;

  Result := res;
end;

function DataFrame.FullJoinSingleKey(other: DataFrame; key: string): DataFrame;
begin
  // 1. Индексы ключей
  var leftKeyIdx  := ResolveKeyIndices([key]);
  var rightKeyIdx := other.ResolveKeyIndices([key]);

  // 2. Layout'ы ключей
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 3. Hash-индекс по right
  var hash := other.BuildHashIndex(rightLayout);

  // 4. Результат (схема такая же, как у inner/left)
  var res := CreateInnerJoinResult(other, leftKeyIdx, rightKeyIdx);

  // 5. Курсоры
  var leftCur  := Self.GetCursor;
  var rightCur := other.GetCursor;

  // 6. Отметка использованных строк right
  var rightUsed := new boolean[other.RowCount];

  // 7. Основной проход по left
  while leftCur.MoveNext do
  begin
    var hasNA := false;
    var lk := BuildJoinKey(leftCur, leftLayout, hasNA);

    if hasNA then
    begin
      res.AppendLeftOnlyRow(leftCur, leftKeyIdx, rightKeyIdx);
      continue;
    end;

    var rows: List<integer>;
    if hash.TryGetValue(lk, rows) then
    begin
      foreach var r in rows do
      begin
        rightCur.MoveTo(r);
        res.AppendJoinedRow(leftCur, rightCur, leftKeyIdx, rightKeyIdx);
        rightUsed[r] := true;
      end;
    end
    else
      res.AppendLeftOnlyRow(leftCur, leftKeyIdx, rightKeyIdx);
  end;

  // 8. Дописываем строки, которые есть только в right
  for var r := 0 to other.RowCount - 1 do
    if not rightUsed[r] then
    begin
      rightCur.MoveTo(r);
      res.AppendRightOnlyRow(
        rightCur,
        leftKeyIdx,
        rightKeyIdx,
        leftCur.ColumnCount
      );
    end;

  Result := res;
end;

function DataFrame.FullJoinMultiKey(other: DataFrame; keys: array of string): DataFrame;
begin
  // 1. Индексы ключей (ОТЛИЧИЕ №1)
  var leftKeyIdx  := ResolveKeyIndices(keys);
  var rightKeyIdx := other.ResolveKeyIndices(keys);

  // 2. Layout'ы ключей (ОТЛИЧИЕ №2)
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 3. Hash-индекс по right (без изменений)
  var hash := other.BuildHashIndex(rightLayout);

  // 4. Результат
  var res := CreateInnerJoinResult(other, leftKeyIdx, rightKeyIdx);

  // 5. Курсоры
  var leftCur  := Self.GetCursor;
  var rightCur := other.GetCursor;

  // 6. Учёт использованных строк right
  var rightUsed := new boolean[other.RowCount];

  // 7. Основной проход по left
  while leftCur.MoveNext do
  begin
    var hasNA := false;
    var lk := BuildJoinKey(leftCur, leftLayout, hasNA);

    if hasNA then
    begin
      res.AppendLeftOnlyRow(leftCur, leftKeyIdx, rightKeyIdx);
      continue;
    end;

    var rows: List<integer>;
    if hash.TryGetValue(lk, rows) then
    begin
      foreach var r in rows do
      begin
        rightCur.MoveTo(r);
        res.AppendJoinedRow(leftCur, rightCur, leftKeyIdx, rightKeyIdx);
        rightUsed[r] := true;
      end;
    end
    else
      res.AppendLeftOnlyRow(leftCur, leftKeyIdx, rightKeyIdx);
  end;

  // 8. Right-only строки
  for var r := 0 to other.RowCount - 1 do
    if not rightUsed[r] then
    begin
      rightCur.MoveTo(r);
      res.AppendRightOnlyRow(
        rightCur,
        leftKeyIdx,
        rightKeyIdx,
        leftCur.ColumnCount
      );
    end;

  Result := res;
end;

function DataFrame.JoinInnerSingleKey(other: DataFrame; key: string): DataFrame;
begin
  var leftKey := ResolveKeyIndices([key])[0];
  var rightKey := other.ResolveKeyIndices([key])[0];

  var lt := columns[leftKey].Info.ColType;
  var rt := other.columns[rightKey].Info.ColType;

  if lt <> rt then
    raise new Exception('Join key types mismatch');

  case lt of
    ctInt:   Result := JoinInnerSingleKeyInt(other, leftKey, rightKey);
    ctFloat: Result := JoinInnerSingleKeyFloat(other, leftKey, rightKey);
    ctStr:   Result := JoinInnerSingleKeyStr(other, leftKey, rightKey);
    ctBool:  Result := JoinInnerSingleKeyBool(other, leftKey, rightKey);
  end;
end;

function DataFrame.JoinInnerSingleKeyInt(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<integer, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Int(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then continue;
    var k := lcur.Int(leftKey);

    if not index.ContainsKey(k) then continue;

    foreach var rpos in index[k] do
    begin
      rcur.MoveTo(rpos);
      res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
    end;
  end;

  Result := res;
end;

function DataFrame.JoinInnerSingleKeyFloat(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<real, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Float(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then continue;
    var k := lcur.Float(leftKey);

    if not index.ContainsKey(k) then continue;

    foreach var rpos in index[k] do
    begin
      rcur.MoveTo(rpos);
      res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
    end;
  end;

  Result := res;
end;

function DataFrame.JoinInnerSingleKeyStr(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<string, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Str(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then continue;
    var k := lcur.Str(leftKey);

    if not index.ContainsKey(k) then continue;

    foreach var rpos in index[k] do
    begin
      rcur.MoveTo(rpos);
      res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
    end;
  end;

  Result := res;
end;

function DataFrame.JoinInnerSingleKeyBool(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<boolean, List<integer>>;

  var rcur := other.GetCursor;
  while rcur.MoveNext do
  begin
    if not rcur.IsValid(rightKey) then continue;
    var k := rcur.Bool(rightKey);

    if not index.ContainsKey(k) then
      index[k] := new List<integer>;
    index[k].Add(rcur.Position);
  end;

  var res := CreateInnerJoinResult(other, [leftKey], [rightKey]);

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then continue;
    var k := lcur.Bool(leftKey);

    if not index.ContainsKey(k) then continue;

    foreach var rpos in index[k] do
    begin
      rcur.MoveTo(rpos);
      res.AppendJoinedRow(lcur, rcur, [leftKey], [rightKey]);
    end;
  end;

  Result := res;
end;

function DataFrame.JoinInnerMultiKey(other: DataFrame; keys: array of string): DataFrame;
begin
  // 1. разрешаем индексы ключей
  var leftKeyIdx := ResolveKeyIndices(keys);
  var rightKeyIdx := other.ResolveKeyIndices(keys);

  // 2. строим layout'ы
  var leftLayout := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 3. hash index по правой таблице
  var hash := other.BuildHashIndex(rightLayout);

  // 4. создаём результат
  var res := CreateInnerJoinResult(other, leftKeyIdx, rightKeyIdx);

  // 5. probe
  var lcur := GetCursor;
  var rcur := other.GetCursor;

  while lcur.MoveNext do
  begin
    var hasNA := false;
    var key := BuildJoinKey(lcur, leftLayout, hasNA);
    if hasNA then continue;

    if not hash.ContainsKey(key) then continue;

    foreach var rpos in hash[key] do
    begin
      rcur.MoveTo(rpos);
      res.AppendJoinedRow(lcur, rcur, leftKeyIdx, rightKeyIdx);
    end;
  end;

  Result := res;
end;


function DataFrame.BuildJoinKeyLayout(keyIndices: array of integer): JoinKeyLayout;
begin
  Result.ColIndices := keyIndices;
  Result.ColTypes := ArrGen(keyIndices.Length,
    i -> columns[keyIndices[i]].Info.ColType
  );
end;

function DataFrame.Join(other: DataFrame; keys: array of string; kind: JoinKind): DataFrame;
begin
  if kind = jkInner then
    if keys.Length = 1 then
      exit(JoinInnerSingleKey(other, keys[0]))
    else
      exit(JoinInnerMultiKey(other, keys));

  if kind = jkLeft then
    if keys.Length = 1 then
      exit(LeftJoinSingleKey(other, keys[0]))
    else
      exit(LeftJoinMultiKey(other, keys));

  if kind = jkRight then
  begin
    // 1. Меняем фреймы местами
    var tmp := other.Join(Self, keys, jkLeft);

    // 2. Приводим порядок столбцов
    var cols := Self.ColumnNames + other.ColumnNames;

    exit(tmp.Select(cols));
  end;
  
  if kind = jkFull then
    if keys.Length = 1 then
      exit(FullJoinSingleKey(other, keys[0]))
    else
      exit(FullJoinMultiKey(other, keys));

  raise new Exception('Join kind not implemented');
end;

function DataFrame.Join(other: DataFrame; key: string; kind: JoinKind): DataFrame;
begin
  Result := Join(other, [key], kind);
end;

function DataFrame.Join(other: DataFrame; leftKeys, rightKeys: array of string; kind: JoinKind): DataFrame;
begin
  if leftKeys.Length <> rightKeys.Length then
    raise new Exception('leftKeys and rightKeys must have the same length');

  // временно переименовываем столбцы справа
  var tmp := other;
  var renames := new List<(string,string)>;
  for var i := 0 to leftKeys.Length - 1 do
    if leftKeys[i] <> rightKeys[i] then
      renames.Add((rightKeys[i], leftKeys[i]));

  if renames.Count > 0 then
    tmp := other.Rename(renames.ToArray);

  Result := Self.Join(tmp, leftKeys, kind);
  
  AssertSchemaConsistent;
end;

procedure DataFrame.RebuildColumnIndex;
begin
  columnIndexByName := new Dictionary<string, integer>;
  for var i := 0 to columns.Count - 1 do
    columnIndexByName[columns[i].Info.Name] := i;
end;

function DataFrame.ColumnNames: array of string;
begin
  Result := new string[columns.Count];
  for var i := 0 to columns.Count - 1 do
    Result[i] := columns[i].Info.Name;
end;

function DataFrame.RowCount: integer;
begin
  if columns.Count = 0 then
    Result := 0
  else Result := columns[0].RowCount;
end;

function DataFrame.ColumnCount: integer := columns.Count;

function DataFrame.ColumnIndex(name: string): integer;
begin
  for var i := 0 to columns.Count - 1 do
    if columns[i].Info.Name = name then
      exit(i);
  raise new Exception('Column not found: ' + name);
end;

function DataFrame.GetCursor: DataFrameCursor :=
  new DataFrameCursor(columns.ToArray,columnIndexByName);
  
function DataFrame.GetIntColumn(name: string): array of integer;
begin
  var i := ColumnIndex(name);
  var c := IntColumn(columns[i]);
  Result := c.Data;
end;

function DataFrame.GetFloatColumn(name: string): array of real;
begin
  var i := ColumnIndex(name);
  var c := FloatColumn(columns[i]);
  Result := c.Data;
end;

procedure DataFrame.AddIntColumn(name: string; data: array of integer; valid: array of boolean; isCategorical: boolean);
begin
  var c := new IntColumn;
  c.Info := new ColumnInfo(name, ctInt, isCategorical);
  c.Data := data;
  c.IsValid := valid;
  columns.Add(c);
  RebuildColumnIndex;
end;

procedure DataFrame.AddFloatColumn(name: string; data: array of real; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    raise new Exception('Row count mismatch');

  var c := new FloatColumn;
  c.Info := new ColumnInfo(name, ctFloat, false);
  c.Data := data;
  c.IsValid := valid;

  columns.Add(c);
  RebuildColumnIndex;
end;

procedure DataFrame.AddStrColumn(name: string; data: array of string; valid: array of boolean; isCategorical: boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    raise new Exception('Row count mismatch');

  var c := new StrColumn;
  c.Info := new ColumnInfo(name, ctStr, isCategorical);
  c.Data := data;
  c.IsValid := valid;

  columns.Add(c);
  RebuildColumnIndex;
end;

procedure DataFrame.AddBoolColumn(name: string; data: array of boolean; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    raise new Exception('Row count mismatch');

  var c := new BoolColumn;
  c.Info := new ColumnInfo(name, ctBool, false);
  c.Data := data;
  c.IsValid := valid;

  columns.Add(c);
  RebuildColumnIndex;
end;


procedure DataFrame.CheckColumnIndex(colIndex: integer);
begin
  if (colIndex < 0) or (colIndex >= ColumnCount) then
    raise new Exception('Column index out of range');
end;

function DataFrame.Sum(colIndex: integer): real;
begin
  CheckColumnIndex(colIndex);

  var cursor := GetCursor;
  var s: real := 0.0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
      s += cursor.Float(colIndex);

  Result := s;
end;

function DataFrame.Sum(colName: string): real
  := Sum(ColumnIndex(colName));
  
function DataFrame.Count(colIndex: integer): integer;
begin
  CheckColumnIndex(colIndex);

  var cursor := GetCursor;
  var c := 0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
      c += 1;

  Result := c;
end;

function DataFrame.Count(colName: string): integer
  := Count(ColumnIndex(colName));

function DataFrame.Mean(colIndex: integer): real;
begin
  CheckColumnIndex(colIndex);

  var cursor := GetCursor;
  var s := 0.0;
  var c := 0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      s += cursor.Float(colIndex);
      c += 1;
    end;

  Result := if c = 0 then 0.0 else s / c;
end;

function DataFrame.Mean(colName: string): real
  := Mean(ColumnIndex(colName));
  
function DataFrame.Min(colIndex: integer): real;
begin
  CheckColumnIndex(colIndex);

  var cursor := GetCursor;
  var has := false;
  var m := 0.0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var v := cursor.Float(colIndex);
      if not has then
      begin
        m := v;
        has := true;
      end
      else if v < m then
        m := v;
    end;

  if not has then
    raise new Exception('Min: no valid values');

  Result := m;
end;

function DataFrame.Min(colName: string): real
  := Min(ColumnIndex(colName));

function DataFrame.Max(colIndex: integer): real;
begin
  CheckColumnIndex(colIndex);

  var cursor := GetCursor;
  var has := false;
  var m := 0.0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var v := cursor.Float(colIndex);
      if not has then
      begin
        m := v;
        has := true;
      end
      else if v > m then
        m := v;
    end;

  if not has then
    raise new Exception('Max: no valid values');

  Result := m;
end;

function DataFrame.Max(colName: string): real
  := Max(ColumnIndex(colName));
  
function DataFrame.MinMax(colIndex: integer): (real, real);
begin
  CheckColumnIndex(colIndex);

  var cursor := GetCursor;
  var has := false;
  var mn, mx: real;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var v := cursor.Float(colIndex);
      if not has then
      begin
        mn := v;
        mx := v;
        has := true;
      end
      else
      begin
        if v < mn then mn := v;
        if v > mx then mx := v;
      end;
    end;

  if not has then
    raise new Exception('MinMax: no valid values');

  Result := (mn, mx);
end;

function DataFrame.MinMax(colName: string): (real, real);
begin
  Result := MinMax(ColumnIndex(colName));
end;
  
function DataFrame.Variance(colIndex: integer): real;
begin
  CheckColumnIndex(colIndex);

  // pass 1: mean
  var cursor := GetCursor;
  var sum := 0.0;
  var cnt := 0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      sum += cursor.Float(colIndex);
      cnt += 1;
    end;

  if cnt = 0 then
    exit(0.0);

  var mean := sum / cnt;

  // pass 2: squared deviations
  cursor := GetCursor;
  var acc := 0.0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var d := cursor.Float(colIndex) - mean;
      acc += d * d;
    end;

  Result := acc / cnt;
end;

function DataFrame.Variance(colName: string): real
  := Variance(ColumnIndex(colName));

function DataFrame.Std(colIndex: integer): real
  := Sqrt(Variance(colIndex));

function DataFrame.Std(colName: string): real
  := Std(ColumnIndex(colName));
  
function DataFrame.MeanVariance(colIndex: integer): (real, real);
begin
  CheckColumnIndex(colIndex);

  // pass 1: mean
  var cursor := GetCursor;
  var sum := 0.0;
  var cnt := 0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      sum += cursor.Float(colIndex);
      cnt += 1;
    end;

  if cnt = 0 then
    exit((0.0, 0.0));

  var mean := sum / cnt;

  // pass 2: variance
  cursor := GetCursor;
  var acc := 0.0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var d := cursor.Float(colIndex) - mean;
      acc += d * d;
    end;

  var variance := acc / cnt;

  Result := (mean, variance);
end;

function DataFrame.MeanVariance(colName: string): (real, real)
  := MeanVariance(ColumnIndex(colName));
  
function DataFrame.Describe(colIndex: integer): DescribeStats;
begin
  CheckColumnIndex(colIndex);

  // pass 1: count, sum, min, max
  var cursor := GetCursor;
  var cnt := 0;
  var sum := 0.0;
  var mn, mx: real;
  var has := false;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var v := cursor.Float(colIndex);
      cnt += 1;
      sum += v;

      if not has then
      begin
        mn := v;
        mx := v;
        has := true;
      end
      else
      begin
        if v < mn then mn := v;
        if v > mx then mx := v;
      end;
    end;

  // если нет данных — возвращаем нулевую структуру
  if cnt = 0 then
  begin
    Result.Count := 0;
    Result.Mean := 0.0;
    Result.Std := 0.0;
    Result.Min := 0.0;
    Result.Max := 0.0;
    exit;
  end;

  var mean := sum / cnt;

  // pass 2: variance
  cursor := GetCursor;
  var acc := 0.0;

  while cursor.MoveNext do
    if cursor.IsValid(colIndex) then
    begin
      var d := cursor.Float(colIndex) - mean;
      acc += d * d;
    end;

  Result.Count := cnt;
  Result.Mean := mean;
  Result.Std := Sqrt(acc / cnt);
  Result.Min := mn;
  Result.Max := mx;
end;

function DataFrame.Describe(colName: string): DescribeStats;
begin
  Result := Describe(ColumnIndex(colName));
end;

function DataFrame.Describe(colNames: array of string): Dictionary<string, DescribeStats>;
begin
  var res := new Dictionary<string, DescribeStats>;

  foreach var name in colNames do
  begin
    var idx := ColumnIndex(name);
    res[name] := Describe(idx);
  end;

  Result := res;
end;

function DataFrame.Describe(colIndices: array of integer): Dictionary<integer, DescribeStats>;
begin
  var res := new Dictionary<integer, DescribeStats>;

  foreach var i in colIndices do
    res[i] := Describe(i);

  Result := res;
end;

function DataFrame.Describe: Dictionary<string, DescribeStats>;
begin
  var res := new Dictionary<string, DescribeStats>;

  for var i := 0 to ColumnCount - 1 do
    case columns[i].Info.ColType of
      ctInt, ctFloat:
        res[columns[i].Info.Name] := Describe(i);
    end;

  Result := res;
end;

function DataFrame.GroupBy(colIndex: integer): IGroupByContext;
begin
  CheckColumnIndex(colIndex);
  Result := new GroupByContext(self, [colIndex]);
end;

function DataFrame.GroupBy(colName: string): IGroupByContext;
begin
  Result := GroupBy(ColumnIndex(colName));
end;

function DataFrame.GroupBy(colIndices: array of integer): IGroupByContext;
begin
  foreach var i in colIndices do
    CheckColumnIndex(i);
  Result := new GroupByContext(self, colIndices);
end;

function DataFrame.GroupBy(colNames: array of string): IGroupByContext;
begin
  Result := GroupBy(colNames.Select(n -> ColumnIndex(n)).ToArray);
end;

procedure DataFrame.AppendRowFromCursor(src: DataFrame; cur: DataFrameCursor);
begin
  // 1. если DataFrame пуст — копируем структуру
  if columns.Count = 0 then
  begin
    foreach var col in src.columns do
      case col.Info.ColType of
        ctInt:   AddIntColumn(col.Info.Name, new integer[0], nil);
        ctFloat: AddFloatColumn(col.Info.Name, new real[0], nil);
        ctStr:   AddStrColumn(col.Info.Name, new string[0], nil);
        ctBool:  AddBoolColumn(col.Info.Name, new boolean[0], nil);
      end;
  end;

  // 2. добавляем текущую строку
  for var j := 0 to columns.Count - 1 do
    columns[j].AppendFromCursor(cur, j);
end;

function DataFrame.Head(n: integer): DataFrame;
begin
  var res := new DataFrame;
  if n <= 0 then
    exit(res);

  var cur := GetCursor;
  var cnt := 0;

  while cur.MoveNext do
  begin
    if cnt = n then
      break;

    res.AppendRowFromCursor(self, cur);
    cnt += 1;
  end;

  Result := res;
end;

function DataFrame.Tail(n: integer): DataFrame;
begin
  var res := new DataFrame;
  if n <= 0 then exit(res);

  var total := RowCount;
  var start := PABCSystem.Max(0, total - n);

  var cur := GetCursor;
  while cur.MoveNext do
    if cur.Position >= start then
      res.AppendRowFromCursor(self, cur);

  Result := res;
end;

type
  SortKey = record
    Row: integer;
    Valid: array of boolean;

    IntVals: array of integer;
    FloatVals: array of real;
    StrVals: array of string;
    BoolVals: array of boolean;
  end;
  
function DataFrame.SortBy(colIndices: array of integer; descending: array of boolean): DataFrame;
begin
  if colIndices.Length <> descending.Length then
    raise new Exception('SortBy: length mismatch');

  foreach var c in colIndices do
    CheckColumnIndex(c);

  // ---------- 1. строим ключи ----------
  var keys := new List<SortKey>;
  var cur := GetCursor;
  
  while cur.MoveNext do
  begin
    var k: SortKey;
    k.Row := cur.Position;
    k.Valid := new boolean[colIndices.Length];
  
    k.IntVals   := new integer[colIndices.Length];
    k.FloatVals := new real[colIndices.Length];
    k.StrVals   := new string[colIndices.Length];
    k.BoolVals  := new boolean[colIndices.Length];
  
    for var i := 0 to colIndices.Length - 1 do
    begin
      var c := colIndices[i];
      k.Valid[i] := cur.IsValid(c);
  
      if not k.Valid[i] then continue;
  
      case columns[c].Info.ColType of
        ctInt:   k.IntVals[i]   := cur.Int(c);
        ctFloat: k.FloatVals[i] := cur.Float(c);
        ctStr:   k.StrVals[i]   := cur.Str(c);
        ctBool:  k.BoolVals[i]  := cur.Bool(c);
      end;
    end;
  
    keys.Add(k);
  end;
  // ---------- 2. сортируем ключи ----------
  keys.Sort((a, b) ->
  begin
    for var i := 0 to colIndices.Length - 1 do
    begin
      if a.Valid[i] <> b.Valid[i] then
      begin  
        Result := if a.Valid[i] then -1 else 1;
        exit;
      end;  
  
      if not a.Valid[i] then
        continue;
  
      var cmp: integer;
      case columns[colIndices[i]].Info.ColType of
        ctInt:   cmp := a.IntVals[i] - b.IntVals[i];
        ctFloat: cmp := a.FloatVals[i].CompareTo(b.FloatVals[i]);
        ctStr:   cmp := a.StrVals[i].CompareTo(b.StrVals[i]);
        ctBool:  cmp := a.BoolVals[i].CompareTo(b.BoolVals[i]);
      end; 
      
      if cmp <> 0 then
      begin  
        Result := if descending[i] then -cmp else cmp;  
        exit;
      end;  
    end;
    Result := 0;
  end);
  
    // ---------- 3. собираем результат ----------
    var res := new DataFrame;
    cur := GetCursor;

  foreach var k in keys do
  begin
    cur.MoveTo(k.Row);
    res.AppendRowFromCursor(self, cur);
  end;

  Result := res;
end;

function DataFrame.SortBy(colIndex: integer; descending: boolean): DataFrame;
begin
  Result := SortBy([colIndex], [descending]);
end;

function DataFrame.SortBy(colName: string; descending: boolean): DataFrame;
begin
  Result := SortBy(ColumnIndex(colName), descending);
end;

function DataFrame.SortBy(colNames: array of string; descending: array of boolean): DataFrame;
begin
  Result := SortBy(colNames.Select(n -> ColumnIndex(n)).ToArray, descending);
end;

function DataFrame.Filter(pred: CursorPredicate): DataFrame;
begin
  var cursor := GetCursor;
  var mask := new List<integer>;

  while cursor.MoveNext do
    if pred(cursor) then
      mask.Add(cursor.Position);

  var newCount := mask.Count;
  var res := new DataFrame;

  foreach var col in columns do
  begin
    // ---------- INT ----------
    if col is IntColumn then
    begin
      var src := IntColumn(col);

      var data := new integer[newCount];
      var valid: array of boolean := nil;

      if src.IsValid <> nil then
        valid := new boolean[newCount];

      for var k := 0 to newCount - 1 do
      begin
        var i := mask[k];
        data[k] := src.Data[i];
        if valid <> nil then
          valid[k] := src.IsValid[i];
      end;

      res.AddIntColumn(src.Info.Name, data, valid, src.Info.IsCategorical);
    end

    // ---------- FLOAT ----------
    else if col is FloatColumn then
    begin
      var src := FloatColumn(col);

      var data := new real[newCount];
      var valid: array of boolean := nil;

      if src.IsValid <> nil then
        valid := new boolean[newCount];

      for var k := 0 to newCount - 1 do
      begin
        var i := mask[k];
        data[k] := src.Data[i];
        if valid <> nil then
          valid[k] := src.IsValid[i];
      end;

      res.AddFloatColumn(src.Info.Name, data, valid);
    end

    // ---------- STRING ----------
    else if col is StrColumn then
    begin
      var src := StrColumn(col);

      var data := new string[newCount];
      var valid: array of boolean := nil;

      if src.IsValid <> nil then
        valid := new boolean[newCount];

      for var k := 0 to newCount - 1 do
      begin
        var i := mask[k];
        data[k] := src.Data[i];
        if valid <> nil then
          valid[k] := src.IsValid[i];
      end;

      res.AddStrColumn(src.Info.Name, data, valid, src.Info.IsCategorical);
    end

    // ---------- BOOL ----------
    else if col is BoolColumn then
    begin
      var src := BoolColumn(col);

      var data := new boolean[newCount];
      var valid: array of boolean := nil;

      if src.IsValid <> nil then
        valid := new boolean[newCount];

      for var k := 0 to newCount - 1 do
      begin
        var i := mask[k];
        data[k] := src.Data[i];
        if valid <> nil then
          valid[k] := src.IsValid[i];
      end;

      res.AddBoolColumn(src.Info.Name, data, valid);
    end
    else
      raise new Exception('Unknown column type');
  end;

  Result := res;
  AssertSchemaConsistent;
end;


function DataFrame.Select(colIndices: array of integer): DataFrame;
begin
  var res := new DataFrame;

  foreach var i in colIndices do
    CheckColumnIndex(i);

  foreach var i in colIndices do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(
          c.Info.Name,
          c.Data,
          c.IsValid,
          c.Info.IsCategorical
        );
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(
          c.Info.Name,
          c.Data,
          c.IsValid,
          c.Info.IsCategorical
        );
      end;

      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(
          c.Info.Name,
          c.Data,
          c.IsValid
        );
      end;

      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(
          c.Info.Name,
          c.Data,
          c.IsValid
        );
      end;
    end;
  end;

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.Select(colNames: array of string): DataFrame;
begin
  Result := Select(colNames.Select(n -> ColumnIndex(n)).ToArray);
end;

function DataFrame.Rename(colIndex: integer; newName: string): DataFrame;
begin
  CheckColumnIndex(colIndex);
  
  var oldName := columns[colIndex].Info.Name;
  if (newName <> oldName) and columnIndexByName.ContainsKey(newName) then
    raise new Exception($'Column "{newName}" already exists');
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    var name := if i = colIndex then newName else col.Info.Name;

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(name, c.Data, c.IsValid, c.Info.IsCategorical);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(name, c.Data, c.IsValid, c.Info.IsCategorical);
      end;

      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(name, c.Data, c.IsValid);
      end;

      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(name, c.Data, c.IsValid);
      end;
    end;
  end;

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.Rename(oldName, newName: string): DataFrame;
begin
  Result := Rename(ColumnIndex(oldName), newName);
end;

function DataFrame.Rename(pairs: array of (string, string)): DataFrame;
begin
  var map := new Dictionary<string, string>;
  foreach var p in pairs do
    map[p[0]] := p[1];

  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    var name := if map.ContainsKey(col.Info.Name) then map[col.Info.Name] else col.Info.Name;

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(name, c.Data, c.IsValid, c.Info.IsCategorical);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(name, c.Data, c.IsValid, c.Info.IsCategorical);
      end;

      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(name, c.Data, c.IsValid);
      end;

      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(name, c.Data, c.IsValid);
      end;
    end;
  end;

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.Drop(colIndices: array of integer): DataFrame;
begin
  // проверяем индексы
  foreach var i in colIndices do
    CheckColumnIndex(i);

  // помечаем удаляемые столбцы
  var drop := new boolean[columns.Count];
  foreach var i in colIndices do
    drop[i] := true;

  // собираем список оставшихся
  var keep := new List<integer>;
  for var i := 0 to columns.Count - 1 do
    if not drop[i] then
      keep.Add(i);

  // переиспользуем Select
  Result := Select(keep.ToArray);
end;

function DataFrame.Drop(colNames: array of string): DataFrame;
begin
  Result := Drop(colNames.Select(n -> ColumnIndex(n)).ToArray);
end;

function DataFrame.WithColumnInt(name: string; f: DataFrameCursor -> integer): DataFrame;
begin
  if columnIndexByName.ContainsKey(name) then
    raise new Exception($'Column "{name}" already exists');
  
  var res := new DataFrame;

  // 1. скопировать все существующие столбцы
  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical);
      end;
      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical);
      end;
      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid);
      end;
      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid);
      end;
    end;
  end;

  // 2. вычислить новый столбец
  var data := new integer[RowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var i := 0;

  while cur.MoveNext do
  begin
    try
      data[i] := f(cur);
    except
      on e: Exception do
      begin
        data[i] := 0;
        if valid = nil then
        begin
          valid := new boolean[RowCount];
          for var j := 0 to i - 1 do
            valid[j] := true;
        end;
        valid[i] := false;
        i += 1;
        continue;
      end;
    end;

    if valid <> nil then
      valid[i] := true;

    i += 1;
  end;

  // 3. добавить новый столбец
  res.AddIntColumn(name, data, valid);

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.WithColumnFloat(name: string; f: DataFrameCursor -> real): DataFrame;
begin
  if columnIndexByName.ContainsKey(name) then
    raise new Exception($'Column "{name}" already exists');
  
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    case col.Info.ColType of
      ctInt:   begin var c := IntColumn(col);   res.AddIntColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical); end;
      ctStr:   begin var c := StrColumn(col);   res.AddStrColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical); end;
      ctFloat: begin var c := FloatColumn(col); res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid); end;
      ctBool:  begin var c := BoolColumn(col);  res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid); end;
    end;
  end;

  var data := new real[RowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var i := 0;

  while cur.MoveNext do
  begin
    try
      data[i] := f(cur);
    except
      begin
        data[i] := 0.0;
        if valid = nil then
        begin
          valid := new boolean[RowCount];
          for var j := 0 to i - 1 do valid[j] := true;
        end;
        valid[i] := false;
        i += 1;
        continue;
      end;
    end;
    if valid <> nil then valid[i] := true;
    i += 1;
  end;

  res.AddFloatColumn(name, data, valid);
  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.WithColumnStr(name: string; f: DataFrameCursor -> string): DataFrame;
begin
  if columnIndexByName.ContainsKey(name) then
    raise new Exception($'Column "{name}" already exists');
  
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    case col.Info.ColType of
      ctInt:   begin var c := IntColumn(col);   res.AddIntColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical); end;
      ctStr:   begin var c := StrColumn(col);   res.AddStrColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical); end;
      ctFloat: begin var c := FloatColumn(col); res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid); end;
      ctBool:  begin var c := BoolColumn(col);  res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid); end;
    end;
  end;

  var data := new string[RowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var i := 0;

  while cur.MoveNext do
  begin
    try
      data[i] := f(cur);
    except
      begin
        data[i] := '';
        if valid = nil then
        begin
          valid := new boolean[RowCount];
          for var j := 0 to i - 1 do valid[j] := true;
        end;
        valid[i] := false;
        i += 1;
        continue;
      end;
    end;
    if valid <> nil then valid[i] := true;
    i += 1;
  end;

  res.AddStrColumn(name, data, valid, false);
  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.WithColumnBool(name: string; f: DataFrameCursor -> boolean): DataFrame;
begin
  if columnIndexByName.ContainsKey(name) then
    raise new Exception($'Column "{name}" already exists');
  
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    case col.Info.ColType of
      ctInt:   begin var c := IntColumn(col);   res.AddIntColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical); end;
      ctStr:   begin var c := StrColumn(col);   res.AddStrColumn(c.Info.Name, c.Data, c.IsValid, c.Info.IsCategorical); end;
      ctFloat: begin var c := FloatColumn(col); res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid); end;
      ctBool:  begin var c := BoolColumn(col);  res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid); end;
    end;
  end;

  var data := new boolean[RowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var i := 0;

  while cur.MoveNext do
  begin
    try
      data[i] := f(cur);
    except
      begin
        data[i] := false;
        if valid = nil then
        begin
          valid := new boolean[RowCount];
          for var j := 0 to i - 1 do valid[j] := true;
        end;
        valid[i] := false;
        i += 1;
        continue;
      end;
    end;
    if valid <> nil then valid[i] := true;
    i += 1;
  end;

  res.AddBoolColumn(name, data, valid);
  Result := res;

  AssertSchemaConsistent;
end;

procedure DataFrame.PrintPreview(maxRows: integer; headRows: integer; decimals: integer);
begin
  var colCount := columns.Count;
  if colCount = 0 then exit;

  var rowCount := RowCount;
  if rowCount = 0 then exit;

  // вычисляем head / tail
  if maxRows < 1 then exit;

  if rowCount <= maxRows then
    headRows := rowCount
  else
  begin
    if headRows = -1 then
      headRows := (maxRows + 1) div 2;

    if headRows < 0 then headRows := 0;
    if headRows > maxRows then headRows := maxRows;
  end;

  var tailRows := maxRows - headRows;
  if tailRows < 0 then tailRows := 0;
  if tailRows > rowCount - headRows then
    tailRows := rowCount - headRows;

  // 1. вычисляем ширины столбцов
  var widths := new integer[colCount];

  // ширины по заголовкам
  for var j := 0 to colCount - 1 do
    widths[j] := columns[j].Info.Name.Length;

  var cursor := GetCursor;

  // ScanRow перемещает cursor
  var ScanRow: integer -> () := row ->
  begin
    cursor.MoveTo(row);
    for var j := 0 to colCount - 1 do
    begin
      var s: string;

      if not cursor.IsValid(j) then
        s := 'NA'
      else
        case columns[j].Info.ColType of
          ctInt:   s := cursor.Int(j).ToString;
          ctFloat: s := cursor.Float(j).ToString('F' + decimals);
          ctStr:   s := cursor.Str(j);
          ctBool:  s := cursor.Bool(j).ToString;
        end;

      if s.Length > widths[j] then
        widths[j] := s.Length;
    end;
  end;
  
  var FormatValue: (integer) -> string := j ->
  begin
    if not cursor.IsValid(j) then
      Result := 'NA'
    else
      case columns[j].Info.ColType of
        ctInt:   Result := cursor.Int(j).ToString;
        ctFloat: Result := cursor.Float(j).ToString('F' + decimals);
        ctStr:   Result := cursor.Str(j);
        ctBool:  Result := cursor.Bool(j).ToString;
      end;
  end;

  // сканируем head
  for var i := 0 to headRows - 1 do
    ScanRow(i);

  // сканируем tail
  if rowCount > headRows then
    for var i := rowCount - tailRows to rowCount - 1 do
      if i >= headRows then
        ScanRow(i);

  // 2. печать заголовков
  for var j := 0 to colCount - 1 do
    PABCSystem.Print(columns[j].Info.Name.PadRight(widths[j] + 2));
  PABCSystem.Println;

  // 3. печать head
  for var i := 0 to headRows - 1 do
  begin
    cursor.MoveTo(i);
    for var j := 0 to colCount - 1 do
      PABCSystem.Print(FormatValue(j).PadRight(widths[j] + 2));
    PABCSystem.Println;
  end;

  // 4. многоточие
  if headRows + tailRows < rowCount then
  begin
    for var j := 0 to colCount - 1 do
      PABCSystem.Print('...'.PadRight(widths[j] + 2));
    PABCSystem.Println;
  end;

  // 5. печать tail
  for var i := rowCount - tailRows to rowCount - 1 do
    if i >= headRows then
    begin
      cursor.MoveTo(i);
      for var j := 0 to colCount - 1 do
        PABCSystem.Print(FormatValue(j).PadRight(widths[j] + 2));
      PABCSystem.Println;
    end;
end;

procedure DataFrame.PrintlnPreview(maxRows: integer; headRows: integer; decimals: integer);
begin
  PrintPreview(maxRows, headRows, decimals);
  PABCSystem.Println;
end;

procedure DataFrame.Print(decimals: integer);
begin
  PrintPreview(20, 10, decimals);
end;

procedure DataFrame.Println(decimals: integer);
begin
  Print(decimals);
  PABCSystem.Println;
end;

procedure DataFrame.AssertSchemaConsistent;
begin
  {$IFDEF Test}

  // --- 0. пустой DataFrame ---
  if columns.Count = 0 then
  begin
    if columnIndexByName.Count <> 0 then
      raise new Exception('Schema inconsistent: no columns but columnIndexByName not empty');
    exit;
  end;

  // --- 1. одинаковая RowCount у всех столбцов ---
  var rc := columns[0].RowCount;
  for var i := 1 to columns.Count - 1 do
    if columns[i].RowCount <> rc then
      raise new Exception(
        $'Schema inconsistent: column "{columns[i].Info.Name}" has RowCount={columns[i].RowCount}, expected {rc}'
      );

  // --- 2. columnIndexByName.Count = columns.Count ---
  if columnIndexByName.Count <> columns.Count then
    raise new Exception(
      $'Schema inconsistent: columnIndexByName.Count={columnIndexByName.Count}, columns.Count={columns.Count}'
    );

  // --- 3. имена уникальны и корректно индексированы ---
  for var i := 0 to columns.Count - 1 do
  begin
    var name := columns[i].Info.Name;

    if not columnIndexByName.ContainsKey(name) then
      raise new Exception(
        $'Schema inconsistent: column "{name}" missing in columnIndexByName'
      );

    var idx := columnIndexByName[name];
    if idx <> i then
      raise new Exception(
        $'Schema inconsistent: columnIndexByName["{name}"]={idx}, expected {i}'
      );
  end;

  {$ENDIF}
end;

static function DataFrame.FromCsv(filename: string): DataFrame;
begin
  Result := CsvLoader.Load(filename, ',', True, nil, false, nil);
end;

static function DataFrame.FromCsvText(text: string): DataFrame;
begin
  Result := CsvLoader.LoadFromLines(text.ToLines, ',', True, nil, false, nil);
end;


//-----------------------------
//        GroupByContext
//-----------------------------

procedure GroupByContext.GetNumericColumn(
  colIndex: integer;
  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean
);
begin
  var col := source.columns[colIndex];

  if col is IntColumn then
  begin
    var c := IntColumn(col);
    dataInt := c.Data;
    dataFloat := nil;
    valid := c.IsValid;
    isInt := true;
  end
  else if col is FloatColumn then
  begin
    var c := FloatColumn(col);
    dataFloat := c.Data;
    dataInt := nil;
    valid := c.IsValid;
    isInt := false;
  end
  else
    raise new Exception('Column is not numeric');
end;

constructor GroupByContext.Create(df: DataFrame; keyCols: array of integer);
begin
  source := df;

  if keyCols.Length = 1 then
  begin
    // fast path
    singleKey := true;
    keyColumn := keyCols[0];
    groups1 := new Dictionary<object, List<integer>>;

    var cursor := df.GetCursor;
    while cursor.MoveNext do
    begin
      if not cursor.IsValid(keyColumn) then
        continue;

      var key: object;
      case df.columns[keyColumn].Info.ColType of
        ctInt: key := cursor.Int(keyColumn);
        ctStr: key := cursor.Str(keyColumn);
        else raise new Exception('Unsupported GroupBy key type');
      end;

      if not groups1.ContainsKey(key) then
        groups1[key] := new List<integer>;

      groups1[key].Add(cursor.Position);
    end;
  end
  else
  begin
    // multi-key
    singleKey := false;
    keyColumns := keyCols;
    groupsN := new Dictionary<array of object, List<integer>>;

    var cursor := df.GetCursor;
    while cursor.MoveNext do
    begin
      var key := new object[keyColumns.Length];
      var ok := true;

      for var i := 0 to keyColumns.Length - 1 do
      begin
        var c := keyColumns[i];
        if not cursor.IsValid(c) then
        begin
          ok := false;
          break;
        end;

        case df.columns[c].Info.ColType of
          ctInt: key[i] := cursor.Int(c);
          ctStr: key[i] := cursor.Str(c);
          else raise new Exception('Unsupported GroupBy key type');
        end;
      end;

      if not ok then
        continue;

      if not groupsN.ContainsKey(key) then
        groupsN[key] := new List<integer>;

      groupsN[key].Add(cursor.Position);
    end;
  end;
end;

function GroupByContext.Count: DataFrame;
begin
  var res := new DataFrame;

  if singleKey then
  begin
    var keys := groups1.Keys.ToArray;
    var counts := new integer[keys.Length];

    for var i := 0 to keys.Length - 1 do
      counts[i] := groups1[keys[i]].Count;

    if source.columns[keyColumn].Info.ColType = ctInt then
      res.AddIntColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> integer(k)).ToArray,
        nil
      )
    else
      res.AddStrColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> string(k)).ToArray,
        nil,
        true
      );

    res.AddIntColumn('count', counts, nil);
  end
  else
  begin
    var keys := groupsN.Keys.ToArray;
    var counts := new integer[keys.Length];

    for var i := 0 to keys.Length - 1 do
      counts[i] := groupsN[keys[i]].Count;

    for var k := 0 to keyColumns.Length - 1 do
    begin
      var ci := keyColumns[k];
      var col := source.columns[ci];

      if col.Info.ColType = ctInt then
        res.AddIntColumn(
          col.Info.Name,
          keys.Select(key -> integer(key[k])).ToArray,
          nil
        )
      else
        res.AddStrColumn(
          col.Info.Name,
          keys.Select(key -> string(key[k])).ToArray,
          nil,
          true
        );
    end;

    res.AddIntColumn('count', counts, nil);
  end;

  Result := res;
end;

function GroupByContext.Mean(colName: string): DataFrame;
begin
  var colIndex := source.ColumnIndex(colName);

  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean;

  GetNumericColumn(colIndex, dataInt, dataFloat, valid, isInt);

  var res := new DataFrame;

  if singleKey then
  begin
    var keys := groups1.Keys.ToArray;
    var means := new real[keys.Length];

    for var i := 0 to keys.Length - 1 do
    begin
      var sum := 0.0;
      var cnt := 0;
      foreach var row in groups1[keys[i]] do
        if (valid = nil) or valid[row] then
        begin
          sum += if isInt then dataInt[row] else dataFloat[row];
          cnt += 1;
        end;
      means[i] := if cnt = 0 then 0.0 else sum / cnt;
    end;

    if source.columns[keyColumn].Info.ColType = ctInt then
      res.AddIntColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> integer(k)).ToArray,
        nil
      )
    else
      res.AddStrColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> string(k)).ToArray,
        nil,
        true
      );

    res.AddFloatColumn(colName + '_mean', means, nil);
  end
  else
  begin
    var keys := groupsN.Keys.ToArray;
    var means := new real[keys.Length];

    for var i := 0 to keys.Length - 1 do
    begin
      var sum := 0.0;
      var cnt := 0;
      foreach var row in groupsN[keys[i]] do
        if (valid = nil) or valid[row] then
        begin
          sum += if isInt then dataInt[row] else dataFloat[row];
          cnt += 1;
        end;
      means[i] := if cnt = 0 then 0.0 else sum / cnt;
    end;

    for var k := 0 to keyColumns.Length - 1 do
    begin
      var ci := keyColumns[k];
      var col := source.columns[ci];

      if col.Info.ColType = ctInt then
        res.AddIntColumn(col.Info.Name,
          keys.Select(key -> integer(key[k])).ToArray, nil)
      else
        res.AddStrColumn(col.Info.Name,
          keys.Select(key -> string(key[k])).ToArray, nil, true);
    end;

    res.AddFloatColumn(colName + '_mean', means, nil);
  end;

  Result := res;
end;

function GroupByContext.Describe(colName: string): DataFrame;
begin
  var colIndex := source.ColumnIndex(colName);

  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean;

  GetNumericColumn(colIndex, dataInt, dataFloat, valid, isInt);

  var res := new DataFrame;

  if singleKey then
  begin
    var keys := groups1.Keys.ToArray;

    var counts := new integer[keys.Length];
    var means  := new real[keys.Length];
    var stds   := new real[keys.Length];
    var mins   := new real[keys.Length];
    var maxs   := new real[keys.Length];

    // pass 1: count, sum, min, max
    for var i := 0 to keys.Length - 1 do
    begin
      var sum := 0.0;
      var cnt := 0;
      var has := false;
      var mn, mx: real;

      foreach var row in groups1[keys[i]] do
        if (valid = nil) or valid[row] then
        begin
          var v := if isInt then dataInt[row] else dataFloat[row];
          sum += v;
          cnt += 1;

          if not has then
          begin
            mn := v; mx := v; has := true;
          end
          else
          begin
            if v < mn then mn := v;
            if v > mx then mx := v;
          end;
        end;

      counts[i] := cnt;

      if cnt = 0 then
        continue;

      means[i] := sum / cnt;
      mins[i] := mn;
      maxs[i] := mx;
    end;

    // pass 2: std
    for var i := 0 to keys.Length - 1 do
      if counts[i] > 0 then
      begin
        var acc := 0.0;
        foreach var row in groups1[keys[i]] do
          if (valid = nil) or valid[row] then
          begin
            var d := (if isInt then dataInt[row] else dataFloat[row]) - means[i];
            acc += d * d;
          end;
        stds[i] := Sqrt(acc / counts[i]);
      end;

    // key column
    if source.columns[keyColumn].Info.ColType = ctInt then
      res.AddIntColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> integer(k)).ToArray,
        nil
      )
    else
      res.AddStrColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> string(k)).ToArray,
        nil,
        true
      );

    res.AddIntColumn('count', counts, nil);
    res.AddFloatColumn('mean', means, nil);
    res.AddFloatColumn('std', stds, nil);
    res.AddFloatColumn('min', mins, nil);
    res.AddFloatColumn('max', maxs, nil);
  end
  else
  begin
    var keys := groupsN.Keys.ToArray;

    var counts := new integer[keys.Length];
    var means  := new real[keys.Length];
    var stds   := new real[keys.Length];
    var mins   := new real[keys.Length];
    var maxs   := new real[keys.Length];

    // pass 1
    for var i := 0 to keys.Length - 1 do
    begin
      var sum := 0.0;
      var cnt := 0;
      var has := false;
      var mn, mx: real;

      foreach var row in groupsN[keys[i]] do
        if (valid = nil) or valid[row] then
        begin
          var v := if isInt then dataInt[row] else dataFloat[row];
          sum += v;
          cnt += 1;

          if not has then
          begin
            mn := v; mx := v; has := true;
          end
          else
          begin
            if v < mn then mn := v;
            if v > mx then mx := v;
          end;
        end;

      counts[i] := cnt;

      if cnt = 0 then
        continue;

      means[i] := sum / cnt;
      mins[i] := mn;
      maxs[i] := mx;
    end;

    // pass 2
    for var i := 0 to keys.Length - 1 do
      if counts[i] > 0 then
      begin
        var acc := 0.0;
        foreach var row in groupsN[keys[i]] do
          if (valid = nil) or valid[row] then
          begin
            var d := (if isInt then dataInt[row] else dataFloat[row]) - means[i];
            acc += d * d;
          end;
        stds[i] := Sqrt(acc / counts[i]);
      end;

    // key columns
    for var k := 0 to keyColumns.Length - 1 do
    begin
      var ci := keyColumns[k];
      var col := source.columns[ci];

      if col.Info.ColType = ctInt then
        res.AddIntColumn(
          col.Info.Name,
          keys.Select(key -> integer(key[k])).ToArray,
          nil
        )
      else
        res.AddStrColumn(
          col.Info.Name,
          keys.Select(key -> string(key[k])).ToArray,
          nil,
          true
        );
    end;

    res.AddIntColumn('count', counts, nil);
    res.AddFloatColumn('mean', means, nil);
    res.AddFloatColumn('std', stds, nil);
    res.AddFloatColumn('min', mins, nil);
    res.AddFloatColumn('max', maxs, nil);
  end;

  Result := res;
end;

function GroupByContext.DescribeAll: DataFrame;
begin
  var res := new DataFrame;

  // 1) сначала добавляем ключевые колонки
  if singleKey then
  begin
    var keys := groups1.Keys.ToArray;

    if source.columns[keyColumn].Info.ColType = ctInt then
      res.AddIntColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> integer(k)).ToArray,
        nil
      )
    else
      res.AddStrColumn(
        source.columns[keyColumn].Info.Name,
        keys.Select(k -> string(k)).ToArray,
        nil,
        true
      );
  end
  else
  begin
    var keys := groupsN.Keys.ToArray;

    for var k := 0 to keyColumns.Length - 1 do
    begin
      var ci := keyColumns[k];
      var col := source.columns[ci];

      if col.Info.ColType = ctInt then
        res.AddIntColumn(
          col.Info.Name,
          keys.Select(key -> integer(key[k])).ToArray,
          nil
        )
      else
        res.AddStrColumn(
          col.Info.Name,
          keys.Select(key -> string(key[k])).ToArray,
          nil,
          true
        );
    end;
  end;

  // 2) для каждого числового столбца — Describe и дописываем колонки
  for var i := 0 to source.ColumnCount - 1 do
    case source.columns[i].Info.ColType of
      ctInt, ctFloat:
      begin
        var df := Describe(source.columns[i].Info.Name);

        res.AddIntColumn(
          source.columns[i].Info.Name + '_count',
          df.GetIntColumn('count'),
          nil
        );
        res.AddFloatColumn(
          source.columns[i].Info.Name + '_mean',
          df.GetFloatColumn('mean'),
          nil
        );
        res.AddFloatColumn(
          source.columns[i].Info.Name + '_std',
          df.GetFloatColumn('std'),
          nil
        );
        res.AddFloatColumn(
          source.columns[i].Info.Name + '_min',
          df.GetFloatColumn('min'),
          nil
        );
        res.AddFloatColumn(
          source.columns[i].Info.Name + '_max',
          df.GetFloatColumn('max'),
          nil
        );
      end;
    end;

  Result := res;
end;

//-----------------------------
//          CSVLoader
//-----------------------------

static function CSVLoader.LoadFromLines(lines: sequence of string; delimiter: char; hasHeader: boolean; missingValues: array of string; strict: boolean; 
  schema: Dictionary<string, ColumnType>): DataFrame;
begin
  // ---------- missing values ----------
  var missing: HashSet<string>;
  if missingValues = nil then
    missing := new HashSet<string>(Arr('', 'NA', 'NaN', 'null'))
  else
    missing := new HashSet<string>(missingValues);

  var IsMissing: string -> boolean := s -> missing.Contains(s);

  // ---------- PASS 1: headers + infer ----------
  var headers: array of string := nil;
  var colCount := 0;
  var rowCount := 0;
  
  var canBool, canInt, canFloat: array of boolean;
  (canBool, canInt, canFloat) := (nil, nil, nil);
  
  var first := true;
  foreach var line in lines do
  begin
    if first then
    begin
      var parts := line.Split(delimiter);
  
      if hasHeader then
      begin
        headers := parts;
        colCount := headers.Length;
      end
      else
      begin
        colCount := parts.Length;
        headers := ArrGen(colCount, i -> 'C' + i.ToString);
        // эта строка — данные
        first := false;
        continue;
      end;
  
      // инициализация инференса
      canBool := ArrGen(colCount, i -> true);
      canInt := ArrGen(colCount, i -> true);
      canFloat := ArrGen(colCount, i -> true);
  
      // ===== schema override (ИМЕННО ЗДЕСЬ) =====
      if schema <> nil then
        for var j := 0 to colCount - 1 do
          if schema.ContainsKey(headers[j]) then
          begin
            canBool[j] := false;
            canInt[j] := false;
            canFloat[j] := false;
  
            case schema[headers[j]] of
              ctBool:  canBool[j] := true;
              ctInt:   canInt[j] := true;
              ctFloat: canFloat[j] := true;
              ctStr:   ; // всё false → string
            end;
          end;
      // =========================================
  
      first := false;
      if hasHeader then continue;
    end;
  
    var parts := line.Split(delimiter);
  
    if parts.Length <> colCount then
      if strict then
        raise new Exception($'CSV format error: expected {colCount} columns, got {parts.Length}');
  
    for var j := 0 to colCount - 1 do
    begin
      var s := if j < parts.Length then parts[j] else '';
      if IsMissing(s) then continue;
  
      // если тип зафиксирован схемой — инференс не делаем
      if (schema <> nil) and schema.ContainsKey(headers[j]) then
        continue;
  
      var sl := s.ToLower;
  
      if not ((sl = 'true') or (sl = 'false') or (sl = 'yes') or (sl = 'no')) then
        canBool[j] := false;
  
      var iv: integer;
      if not integer.TryParse(s, iv) then
        canInt[j] := false;
  
      var fv: real;
      if not real.TryParse(s, fv) then
        canFloat[j] := false;
    end;
  
    rowCount += 1;
  end;
  
  if headers = nil then
    raise new Exception('Empty CSV');

  // ---------- PASS 2: allocate ----------
  var df := new DataFrame;

  var intData := new IntArray[colCount];
  var floatData := new RealArray[colCount];
  var strData := new StringArray[colCount];
  var boolData := new BoolArray[colCount];
  var valid := new BoolArray[colCount];

  for var j := 0 to colCount - 1 do
  begin
    valid[j] := new boolean[rowCount];

    if canBool[j] then
      boolData[j] := new boolean[rowCount]
    else if canInt[j] then
      intData[j] := new integer[rowCount]
    else if canFloat[j] then
      floatData[j] := new real[rowCount]
    else
      strData[j] := new string[rowCount];
  end;

  // ---------- PASS 2: fill ----------
  var row := 0;
  first := true;
  foreach var line in lines do
  begin
    if first then
    begin
      first := false;
      if hasHeader then continue;
    end;
  
    var parts := line.Split(delimiter);
  
    if parts.Length <> colCount then
      if strict then
        raise new Exception($'CSV format error: expected {colCount} columns, got {parts.Length}');
  
    for var j := 0 to colCount - 1 do
    begin
      var s := if j < parts.Length then parts[j] else '';
  
      if IsMissing(s) then
      begin
        valid[j][row] := false;
        continue;
      end;
  
      // ----- BOOL -----
      if canBool[j] then
      begin
        var sl := s.ToLower;
        if (sl = 'true') or (sl = 'yes') then
        begin
          boolData[j][row] := true;
          valid[j][row] := true;
        end
        else if (sl = 'false') or (sl = 'no') then
        begin
          boolData[j][row] := false;
          valid[j][row] := true;
        end
        else
        begin
          if strict then
            raise new Exception($'Invalid bool "{s}" in column {headers[j]}');
          valid[j][row] := false;
        end;
      end
  
      // ----- INT -----
      else if canInt[j] then
      begin
        var iv: integer;
        if integer.TryParse(s, iv) then
        begin
          intData[j][row] := iv;
          valid[j][row] := true;
        end
        else
        begin
          if strict then
            raise new Exception($'Invalid int "{s}" in column {headers[j]}');
          valid[j][row] := false;
        end;
      end
  
      // ----- FLOAT -----
      else if canFloat[j] then
      begin
        var fv: real;
        if real.TryParse(s, fv) then
        begin
          floatData[j][row] := fv;
          valid[j][row] := true;
        end
        else
        begin
          if strict then
            raise new Exception($'Invalid float "{s}" in column {headers[j]}');
          valid[j][row] := false;
        end;
      end
  
      // ----- STRING -----
      else
      begin
        strData[j][row] := s;
        valid[j][row] := true;
      end;
    end;
  
    row += 1;
  end;

  // ---------- assemble DataFrame ----------
  for var j := 0 to colCount - 1 do
  begin
    if canBool[j] then
      df.AddBoolColumn(headers[j], boolData[j], valid[j])
    else if canInt[j] then
      df.AddIntColumn(headers[j], intData[j], valid[j])
    else if canFloat[j] then
      df.AddFloatColumn(headers[j], floatData[j], valid[j])
    else
      df.AddStrColumn(headers[j], strData[j], valid[j], true);
  end;

  Result := df;
end;

static function CSVLoader.Load(filename: string; delimiter: char; hasHeader: boolean;      
  missingValues: array of string; strict: boolean; 
  schema: Dictionary<string, ColumnType>): DataFrame;
begin
  Result := LoadFromLines(ReadLines(filename),delimiter,hasHeader,missingValues,strict,schema);
end;    


end.