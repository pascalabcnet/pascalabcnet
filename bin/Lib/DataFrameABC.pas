// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
// DataFrameABC v.1.1

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
  
  IntColumn = DataFrameABCCore.IntColumn;
  FloatColumn = DataFrameABCCore.FloatColumn;
  BoolColumn = DataFrameABCCore.BoolColumn;
  StrColumn = DataFrameABCCore.StrColumn;
  
  IGroupByContext = interface;
  
/// Класс для работы с табличными данными (датасетами).
/// Поддерживает основные операции: фильтрацию, сортировку, группировку, 
/// соединение таблиц и статистический анализ 
  DataFrame = class
  private
    columns: List<Column>;
    fschema: DataFrameSchema;

    procedure RebuildSchema;
    
    // Join методы
    
    procedure AppendJoinedRow(leftCur, rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
    procedure AppendLeftOnlyRow(leftCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
    procedure AppendRightOnlyRow(rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer; leftColumnCount: integer);
    
    // Single key методы
    {function DataFrame.JoinInnerSingleKey(other: DataFrame; leftKey, rightKey: integer;
      resultSchema: DataFrameSchema): DataFrame;}
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
    
    function ReorderBySchema(schema: DataFrameSchema): DataFrame;
    function RightJoinViaSchema(other: DataFrame; keys: array of string): DataFrame;
    
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
    
    constructor Create(cols: List<Column>; schema: DataFrameSchema);
    
    function BuildJoinSchema(right: DataFrame; leftKeys, rightKeys: array of integer; 
      rightPrefix: string): DataFrameSchema;
    function BuildJoinSchema(right: DataFrame; leftKeys, rightKeys: array of string): DataFrameSchema;

    function CreateEmptyBySchema(schema: DataFrameSchema): DataFrame;
    
    function GetColumnIndex(name: string): integer;
    function GetColumn(name: string): Column;
  public
    /// Схема DataFrame: имена, типы и признаки категориальности
    property Schema: DataFrameSchema read fschema;
    
    property Item[name: string]: Column read GetColumn; default;

    /// Добавляет в DataFrame столбец-представление (view),
    /// использующий те же данные, что и исходный столбец
    procedure AddColumnView(src: Column);

    /// Создает пустой DataFrame
    constructor Create;
    
    /// Возвращает количество строк в DataFrame
    function RowCount: integer;
    /// Возвращает количество столбцов в DataFrame
    function ColumnCount: integer;
    /// Возвращает тип столбца по номеру
    function GetColumnType(colIndex: integer): ColumnType;
    /// Возвращает индекс столбца по имени
    function ColumnIndex(name: string): integer;
    
    /// Создает курсор для итерации по строкам
    function GetCursor: DataFrameCursor;
    
    /// Добавляет столбец целых чисел
    procedure AddIntColumn(name: string; data: array of integer; valid: array of boolean := nil; isCategorical: boolean := false);
    /// Добавляет столбец вещественных чисел
    procedure AddFloatColumn(name: string; data: array of real; valid: array of boolean := nil);
    /// Добавляет строковый столбец
    procedure AddStrColumn(name: string; data: array of string; valid: array of boolean := nil; isCategorical: boolean := true);
    /// Добавляет столбец логических значений
    procedure AddBoolColumn(name: string; data: array of boolean; valid: array of boolean := nil);
    
    /// Возвращает данные целочисленного столбца по имени
    function GetIntColumn(name: string): array of integer;
    /// Возвращает данные вещественного столбца по имени
    function GetFloatColumn(name: string): array of real;
    
    /// Вычисляет сумму значений столбца по индексу
    function Sum(colIndex: integer): real; 
    /// Вычисляет сумму значений столбца по имени
    function Sum(colName: string): real; 
    /// Подсчитывает количество валидных значений в столбце по индексу
    function Count(colIndex: integer): integer; 
    /// Подсчитывает количество валидных значений в столбце по имени
    function Count(colName: string): integer; 
    /// Вычисляет среднее значение столбца по индексу
    function Mean(colIndex: integer): real; 
    /// Вычисляет среднее значение столбца по имени
    function Mean(colName: string): real; 
    /// Вычисляет медиану по валидным (non-NA) значениям столбца по индексу
    function Median(colIndex: integer): real;
    /// Вычисляет медиану по валидным (non-NA) значениям столбца по имени
    function Median(colName: string): real;
    /// Находит минимальное значение столбца по индексу
    function Min(colIndex: integer): real; 
    /// Находит минимальное значение столбца по имени
    function Min(colName: string): real; 
    /// Находит максимальное значение столбца по индексу
    function Max(colIndex: integer): real;
    /// Находит максимальное значение столбца по имени
    function Max(colName: string): real;
    /// Находит минимальное и максимальное значения столбца по индексу
    function MinMax(colIndex: integer): (real, real); 
    /// Находит минимальное и максимальное значения столбца по имени
    function MinMax(colName: string): (real, real); 
    /// Вычисляет дисперсию значений столбца по индексу
    function Variance(colIndex: integer): real; 
    /// Вычисляет дисперсию значений столбца по имени
    function Variance(colName: string): real; 
    /// Вычисляет стандартное отклонение столбца по индексу
    function Std(colIndex: integer): real; 
    /// Вычисляет стандартное отклонение столбца по имени
    function Std(colName: string): real; 
    /// Вычисляет среднее и дисперсию столбца по индексу
    function MeanVariance(colIndex: integer): (real, real); 
    /// Вычисляет среднее и дисперсию столбца по имени
    function MeanVariance(colName: string): (real, real); 
    
    /// Возвращает статистику столбца по индексу
    function Describe(colIndex: integer): DescribeStats; 
    /// Возвращает статистику столбца по имени
    function Describe(colName: string): DescribeStats; 
    /// Возвращает статистику по нескольким столбцам по именам
    function Describe(colNames: array of string): Dictionary<string, DescribeStats>; 
    /// Возвращает статистику по нескольким столбцам по индексам
    function Describe(colIndices: array of integer): Dictionary<integer, DescribeStats>; 
    /// Возвращает статистику по всем числовым столбцам
    function DescribeAll: Dictionary<string, DescribeStats>; 
    
    /// Группирует данные по столбцу по индексу
    function GroupBy(colIndex: integer): IGroupByContext; 
    /// Группирует данные по столбцу по имени
    function GroupBy(colName: string): IGroupByContext; 
    /// Группирует данные по нескольким столбцам по индексам
    function GroupBy(colIndices: array of integer): IGroupByContext; 
    /// Группирует данные по нескольким столбцам по именам
    function GroupBy(colNames: array of string): IGroupByContext; 
    
    /// Возвращает первые n строк 
    function Head(n: integer): DataFrame;
    /// Возвращает последние n строк 
    function Tail(n: integer): DataFrame;
    
    /// Фильтрует строки по предикату
    function Filter(pred: CursorPredicate): DataFrame;
    /// Выбирает столбцы по индексам
    function Select(colIndices: array of integer): DataFrame; 
    /// Выбирает столбцы по именам
    function Select(colNames: array of string): DataFrame; 
    /// Сортирует по столбцу по индексу
    function SortBy(colIndex: integer; descending: boolean := false): DataFrame; 
    /// Сортирует по столбцу по имени
    function SortBy(colName: string; descending: boolean := false): DataFrame; 
    /// Сортирует по нескольким столбцам по индексам
    function SortBy(colIndices: array of integer; descending: array of boolean): DataFrame; 
    /// Сортирует по нескольким столбцам по именам
    function SortBy(colNames: array of string; descending: array of boolean): DataFrame; 
    
    /// Удаляет столбцы по индексам
    function Drop(colIndices: array of integer): DataFrame; 
    /// Удаляет столбцы по именам
    function Drop(colNames: array of string): DataFrame; 
    
    /// Переименовывает столбец по индексу
    function Rename(colIndex: integer; newName: string): DataFrame; 
    /// Переименовывает столбец по имени
    function Rename(oldName, newName: string): DataFrame; 
    /// Переименовывает несколько столбцов
    function Rename(pairs: array of (string, string)): DataFrame; 
    
    /// Добавляет вычисляемый целочисленный столбец
    function WithColumnInt(name: string; f: DataFrameCursor -> integer): DataFrame;
    /// Добавляет вычисляемый целочисленный столбец
    function WithColumn(name: string; f: DataFrameCursor -> integer): DataFrame := WithColumnInt(name, f);
    /// Добавляет вычисляемый вещественный столбец
    function WithColumnFloat(name: string; f: DataFrameCursor -> real): DataFrame;
    /// Добавляет вычисляемый строковый столбец
    function WithColumnStr(name: string; f: DataFrameCursor -> string): DataFrame;
    /// Добавляет вычисляемый логический столбец
    function WithColumnBool(name: string; f: DataFrameCursor -> boolean): DataFrame;
    
    /// Заменяет существующий числовой столбец, пересчитывая его по функции от курсора
    /// Пропущенные значения (NA) сохраняются
    function ReplaceColumnFloat(colName: string; f: DataFrameCursor -> real): DataFrame;
    /// Заменяет существующий числовой столбец, пересчитывая его по функции от курсора
    /// Пропущенные значения (NA) сохраняются
    function ReplaceColumnInt(colName: string; f: DataFrameCursor -> integer): DataFrame;
    
    function AddDerivedIntColumn(name: string; f: DataFrameCursor -> integer): DataFrame;
    
    /// Соединяет с другим DataFrame по одному ключу
    function Join(other: DataFrame; key: string; kind: JoinKind := jkInner): DataFrame; 
    /// Соединяет с другим DataFrame по нескольким ключам
    function Join(other: DataFrame; keys: array of string; kind: JoinKind := jkInner): DataFrame; 
    /// Соединяет с другим DataFrame по разным именам ключей
    function Join(other: DataFrame; leftKeys, rightKeys: array of string; kind: JoinKind := jkInner): DataFrame; 
    
    /// Выводит DataFrame 
    procedure Print(decimals: integer := 3);
    /// Выводит DataFrame и переходит на новую строку
    procedure Println(decimals: integer := 3);
    /// Выводит DataFrame с настраиваемым числом строк 
    procedure PrintPreview(maxRows: integer; headRows: integer := -1; decimals: integer := 3);
    /// Выводит DataFrame с настраиваемым числом строк и переходит на новую строку
    procedure PrintlnPreview(maxRows: integer; headRows: integer := -1; decimals: integer := 3);
    /// Выводит схему датафрейма
    procedure PrintSchema;
    /// Выводит размер, схему и количество валидных значений 
    procedure PrintInfo;

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
/// Статистические методы для анализа табличных данных
/// Используются совместно с DataFrame для анализа и подготовки данных
  Statistics = static class
  public
    /// Коэффициент корреляции Пирсона между двумя числовыми столбцами
    /// Пропущенные значения (NA) игнорируются попарно
    static function Correlation(df: DataFrame; colX, colY: string): real;
  
    /// Матрица корреляций Пирсона для всех числовых столбцов 
    /// Первый столбец содержит имена признаков
    static function CorrelationMatrix(df: DataFrame): DataFrame;
  
    /// Стандартизует числовой столбец: (x - mean) / std
    /// Пропущенные значения (NA) сохраняются
    static function Standardize(df: DataFrame; colName: string): DataFrame;
    
    /// Стандартизует все числовые столбцы: (x - mean) / std
    static function StandardizeAll(df: DataFrame): DataFrame;
  
    /// Нормализует числовой столбец в диапазон [0, 1]
    /// Пропущенные значения (NA) сохраняются
    static function Normalize(df: DataFrame; colName: string): DataFrame;
    
    /// Нормализует все числовые столбцы в диапазон [0, 1]
    /// Пропущенные значения (NA) сохраняются
    static function NormalizeAll(df: DataFrame): DataFrame;  
  
    /// Вычисляет p-квантиль числового столбца (0 ≤ p ≤ 1)
    /// Пропущенные значения (NA) игнорируются
    static function Quantile(df: DataFrame; colName: string; p: real): real;
  
    /// Вычисляет медиану числового столбца
    /// Эквивалентна Quantile(..., 0.5)
    static function Median(df: DataFrame; colName: string): real;
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

uses MLExceptions;

const
  ER_COLS_NULL =
    'cols не может быть nil!!cols cannot be nil';
  ER_SCHEMA_NULL =
    'schema не может быть nil!!schema cannot be nil';
  ER_COLS_SCHEMA_MISMATCH =
    'Количество столбцов не совпадает со схемой!!Columns count and schema mismatch';
  ER_JOIN_KEY_TYPE_MISMATCH =
    'Типы ключей соединения не совпадают!!Join key types mismatch';
  ER_JOIN_KEY_NOT_FOUND =
    'Ключ соединения "{0}" не найден!!Join key "{0}" not found';  
  ER_JOIN_KIND_NOT_IMPLEMENTED =
    'Тип соединения не реализован!!Join kind not implemented';
  ER_JOIN_KEYS_LENGTH_MISMATCH =
    'leftKeys и rightKeys должны иметь одинаковую длину!!leftKeys and rightKeys must have the same length';
  ER_COLUMN_NOT_FOUND =
    'Столбец "{0}" не найден!!Column not found: {0}';
  ER_ADD_COLUMN_ROW_MISMATCH =
    'Несоответствие числа строк при добавлении столбца!!Row count mismatch when adding column';  
  ER_COLUMN_VALID_LENGTH_MISMATCH =
    'Длины data и valid не совпадают при добавлении столбца!!Data and valid length mismatch when adding column';
  ER_COLUMN_INDEX_OUT_OF_RANGE =
    'Индекс столбца {0} вне диапазона [0..{1})!!Column index {0} out of range [0..{1})';
  ER_NO_VALID_VALUES_COLUMN =
    'Нет допустимых значений в столбце {0}!!No valid values in column {0}';
  ER_SORTBY_LENGTH_MISMATCH =
    'SortBy: длины colIndices и descending не совпадают!!SortBy: colIndices and descending length mismatch';
  ER_UNKNOWN_COLUMN_TYPE =
    'Неизвестный тип столбца!!Unknown column type';
  ER_COLUMN_ALREADY_EXISTS =
    'Столбец "{0}" уже существует!!Column "{0}" already exists';
  ER_SCHEMA_INCONSISTENT =
    'Несогласованная схема: столбец "{0}" имеет RowCount={1}, ожидалось {2}!!' +
    'Schema inconsistent: column "{0}" has RowCount={1}, expected {2}';
  ER_SCHEMA_COLUMNCOUNT_MISMATCH =
    'Несогласованная схема: ColumnCount={0}, columns.Count={1}!!' +
    'Schema inconsistent: ColumnCount={0}, columns.Count={1}';
  ER_SCHEMA_COLUMN_MISSING =
    'Несогласованная схема: столбец "{0}" отсутствует в схеме!!' +
    'Schema inconsistent: column "{0}" missing in schema';
  ER_SCHEMA_COLUMN_INDEX_INCONSISTENT =
    'Несогласованная схема: GetColumnIndex("{0}")={1}, ожидалось {2}!!' +
    'Schema inconsistent: GetColumnIndex("{0}")={1}, expected {2}';
  ER_COLUMN_NOT_NUMERIC =
    'Столбец не является числовым!!Column is not numeric';
  ER_GROUPBY_UNSUPPORTED_KEY_TYPE =
    'Неподдерживаемый тип ключа GroupBy: {0}!!Unsupported GroupBy key type: {0}';
  ER_ZERO_VARIANCE =
    'Нулевая дисперсия при вычислении корреляции!!Zero variance in correlation';
  ER_NO_VALID_PAIRS =
    'Нет допустимых пар для вычисления корреляции!!No valid pairs for correlation';
  ER_NO_NUMERIC_COLUMNS =
    'Нет числовых столбцов для матрицы корреляции!!No numeric columns for correlation matrix';
  ER_ZERO_STD_STANDARDIZE =
    'Нулевое стандартное отклонение при вызове Standardize!!Zero standard deviation in Standardize';
  ER_INVALID_VALUE_IN_COLUMN =
    'Недопустимое значение в столбце "{0}"!!' +
    'Invalid value in column "{0}"';
  ER_ZERO_STD_COLUMN =
    'Нулевое стандартное отклонение в столбце "{0}" при StandardizeAll!!' +
    'Zero standard deviation in column "{0}" in StandardizeAll';
  ER_ZERO_RANGE =
    'Нулевой диапазон при нормализации!!Zero range in normalization';
  ER_ZERO_RANGE_COLUMN =
    'Нулевой диапазон в столбце "{0}"!!Zero range in column "{0}"';
  ER_QUANTILE_P_INVALID =
    'Параметр p должен быть в диапазоне [0,1]!!Quantile p must be in [0,1]';
  ER_NO_VALID_VALUES_QUANTILE =
    'Нет допустимых значений для вычисления квантиля!!No valid values for quantile';
  ER_CSV_COLUMN_COUNT_MISMATCH =
    'Ошибка формата CSV: ожидалось {0} столбцов, получено {1}!!' +
    'CSV format error: expected {0} columns, got {1}';
  ER_EMPTY_CSV =
    'CSV-файл пуст!!Empty CSV';
  ER_CSV_UNCLOSED_QUOTE =
    'Ошибка формата CSV: незакрытая кавычка!!CSV format error: unclosed quote';
  ER_CSV_INVALID_BOOL =
    'Некорректное логическое значение "{0}" в столбце "{1}"!!' +
    'Invalid bool "{0}" in column "{1}"';


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
  fschema := new DataFrameSchema([], []);
end;

constructor DataFrame.Create(cols: List<Column>; schema: DataFrameSchema);
begin
  if cols = nil then
    ArgumentNullError(ER_COLS_NULL);
  if schema = nil then
    ArgumentNullError(ER_SCHEMA_NULL);
  if cols.Count <> schema.ColumnCount then
    ArgumentError(ER_COLS_SCHEMA_MISMATCH);

  self.columns := cols;
  self.fSchema := schema;

  RebuildSchema;
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
  // 1. индексы ключей — через Schema
  var leftKey := fSchema.IndexOf(key);
  var rightKey := other.fSchema.IndexOf(key);

  // 2. проверка типов ключей — через Schema
  var lt := fSchema.ColumnTypeAt(leftKey);
  var rt := other.fSchema.ColumnTypeAt(rightKey);

  if lt <> rt then
    Error(ER_JOIN_KEY_TYPE_MISMATCH);

  // 3. типоспецифичный алгоритм (КАК РАНЬШЕ)
  case lt of
    ctInt:   Result := LeftJoinSingleKeyInt(other, leftKey, rightKey);
    ctFloat: Result := LeftJoinSingleKeyFloat(other, leftKey, rightKey);
    ctStr:   Result := LeftJoinSingleKeyStr(other, leftKey, rightKey);
    ctBool:  Result := LeftJoinSingleKeyBool(other, leftKey, rightKey);
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

  //=== Заменяем на Schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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
  // 1. индексы ключей — через Schema
  var n := keys.Length;
  var leftKeyIdx := new integer[n];
  var rightKeyIdx := new integer[n];

  for var i := 0 to n - 1 do
  begin
    leftKeyIdx[i] := fSchema.IndexOf(keys[i]);
    rightKeyIdx[i] := other.fSchema.IndexOf(keys[i]);
  end;

  // 2. проверка типов ключей — через Schema
  for var i := 0 to n - 1 do
    if fSchema.ColumnTypeAt(leftKeyIdx[i]) <> other.fSchema.ColumnTypeAt(rightKeyIdx[i]) then
      Error(ER_JOIN_KEY_TYPE_MISMATCH);

  // 3. layout'ы (как раньше)
  var leftLayout := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 4. hash index по правой таблице
  var hash := other.BuildHashIndex(rightLayout);

  // 5. создаём результат 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

  // 6. probe
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

function DataFrame.ReorderBySchema(schema: DataFrameSchema): DataFrame;
begin
  var cols := new Column[schema.ColumnCount];

  for var i := 0 to schema.ColumnCount - 1 do
    cols[i] := columns[fSchema.IndexOf(schema.Names[i])];

  Result := new DataFrame(cols.ToList, schema);
end;

function DataFrame.RightJoinViaSchema(other: DataFrame; keys: array of string): DataFrame;
begin
  // 1. Схема в порядке Self + other
  var schema := Self.BuildJoinSchema(other, keys, keys);

  // 2. Строки берём из перевёрнутого left join
  var tmp := other.LeftJoinMultiKey(Self, keys);

  // 3. ПЕРЕСОБИРАЕМ колонки по схеме
  Result := tmp.ReorderBySchema(schema);
end;



function DataFrame.FullJoinSingleKey(other: DataFrame; key: string): DataFrame;
begin
  // 1. Индексы ключей
  var li := fSchema.IndexOf(key);
  var ri := other.fSchema.IndexOf(key);
  
  if (li < 0) or (ri < 0) then
    Error(ER_JOIN_KEY_NOT_FOUND, key);
  
  var leftKeyIdx  := [li];
  var rightKeyIdx := [ri];

  // 2. Layout'ы ключей
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 3. Hash-индекс по right
  var hash := other.BuildHashIndex(rightLayout);

  // 4. Результат (схема такая же, как у inner/left)
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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
  // 1. Индексы ключей 
  var leftKeyIdx  := new integer[keys.Length];
  var rightKeyIdx := new integer[keys.Length];
  
  for var i := 0 to keys.Length - 1 do
  begin
    leftKeyIdx[i]  := fSchema.IndexOf(keys[i]);
    rightKeyIdx[i] := other.fSchema.IndexOf(keys[i]);
  end;

  // 2. Layout'ы ключей (ОТЛИЧИЕ №2)
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 3. Hash-индекс по right (без изменений)
  var hash := other.BuildHashIndex(rightLayout);

  // 4. Результат
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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
  var leftKey := fSchema.IndexOf(key);
  var rightKey := other.fSchema.IndexOf(key);

  var lt := fSchema.ColumnTypeAt(leftKey);
  var rt := other.fSchema.ColumnTypeAt(rightKey);

  if lt <> rt then
    Error(ER_JOIN_KEY_TYPE_MISMATCH);

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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 

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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 
  
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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 
  
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

  //=== 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 
  
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
  // 1. индексы ключей — через Schema
  var n := keys.Length;
  var leftKeyIdx := new integer[n];
  var rightKeyIdx := new integer[n];

  for var i := 0 to n - 1 do
  begin
    leftKeyIdx[i] := fSchema.IndexOf(keys[i]);
    rightKeyIdx[i] := other.fSchema.IndexOf(keys[i]);
  end;

  // 2. проверка типов ключей — через Schema
  for var i := 0 to n - 1 do
    if fSchema.ColumnTypeAt(leftKeyIdx[i]) <> other.fSchema.ColumnTypeAt(rightKeyIdx[i]) then
      Error(ER_JOIN_KEY_TYPE_MISMATCH);

  // 3. строим layout'ы
  var leftLayout := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // 4. hash index по правой таблице
  var hash := other.BuildHashIndex(rightLayout);

  // 5. создаём результат 
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );
  var res := CreateEmptyBySchema(schema);
  //=== 
  
  // 6. probe
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
  /// NOTE:
  /// Join result structure is fully defined by DataFrameSchema.
  /// Any post-hoc column reordering (Select, Rename) inside Join
  /// is deprecated and must not be used.
  
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
    exit(RightJoinViaSchema(other, keys));
  
  if kind = jkFull then
    if keys.Length = 1 then
      exit(FullJoinSingleKey(other, keys[0]))
    else
      exit(FullJoinMultiKey(other, keys));

  Error(ER_JOIN_KIND_NOT_IMPLEMENTED);
end;

function DataFrame.Join(other: DataFrame; key: string; kind: JoinKind): DataFrame;
begin
  Result := Join(other, [key], kind);
end;

function DataFrame.Join(other: DataFrame; leftKeys, rightKeys: array of string; kind: JoinKind): DataFrame;
begin
  if leftKeys.Length <> rightKeys.Length then
    ArgumentError(ER_JOIN_KEYS_LENGTH_MISMATCH);

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

procedure DataFrame.RebuildSchema;
begin
  var n := columns.Count;
  var names := new string[n];
  var types := new ColumnType[n];
  var anyCat := false;

  for var i := 0 to n - 1 do
  begin
    var info := columns[i].Info;
    names[i] := info.Name;
    types[i] := info.ColType;
    if info.IsCategorical then anyCat := true;
  end;

  if not anyCat then
    fschema := new DataFrameSchema(names, types)
  else
  begin
    var cats := new boolean[n];
    for var i := 0 to n - 1 do cats[i] := columns[i].Info.IsCategorical;
    fschema := new DataFrameSchema(names, types, cats);
  end;
end;

function DataFrame.GetColumnType(colIndex: integer): ColumnType;
begin
  Result := columns[colIndex].Info.ColType
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
  Error(ER_COLUMN_NOT_FOUND, name);
end;

function DataFrame.GetCursor: DataFrameCursor :=
  new DataFrameCursor(columns.ToArray,fSchema);
  
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
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new IntColumn;
  c.Info := new ColumnInfo(name, ctInt, isCategorical);
  c.Data := data;

  if valid = nil then
    c.IsValid := [True] * data.Length
  else
  begin
    if valid.Length <> data.Length then
      DimensionError(ER_COLUMN_VALID_LENGTH_MISMATCH);;
    c.IsValid := valid;
  end;

  columns.Add(c);
  RebuildSchema;
end;

procedure DataFrame.AddFloatColumn(name: string; data: array of real; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new FloatColumn;
  c.Info := new ColumnInfo(name, ctFloat, False);
  c.Data := data;

  if valid = nil then
    c.IsValid := [True] * data.Length
  else
  begin
    if valid.Length <> data.Length then
      DimensionError(ER_COLUMN_VALID_LENGTH_MISMATCH);
    c.IsValid := valid;
  end;

  columns.Add(c);
  RebuildSchema;
end;

procedure DataFrame.AddStrColumn(name: string; data: array of string; valid: array of boolean; isCategorical: boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new StrColumn;
  c.Info := new ColumnInfo(name, ctStr, isCategorical);
  c.Data := data;

  if valid = nil then
    c.IsValid := [True] * data.Length
  else
  begin
    if valid.Length <> data.Length then
      DimensionError(ER_COLUMN_VALID_LENGTH_MISMATCH);
    c.IsValid := valid;
  end;

  columns.Add(c);
  RebuildSchema;
end;

procedure DataFrame.AddBoolColumn(name: string; data: array of boolean; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new BoolColumn;
  c.Info := new ColumnInfo(name, ctBool, False);
  c.Data := data;

  if valid = nil then
    c.IsValid := [True] * data.Length
  else
  begin
    if valid.Length <> data.Length then
      DimensionError(ER_COLUMN_VALID_LENGTH_MISMATCH);
    c.IsValid := valid;
  end;

  columns.Add(c);
  RebuildSchema;
end;


procedure DataFrame.CheckColumnIndex(colIndex: integer);
begin
  if (colIndex < 0) or (colIndex >= ColumnCount) then
    ArgumentOutOfRangeError(ER_COLUMN_INDEX_OUT_OF_RANGE, colIndex, ColumnCount);
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
  
function DataFrame.Median(colIndex: integer): real;
begin
  CheckColumnIndex(colIndex);
  Result := Statistics.Median(Self, fSchema.Names[colIndex]);
end; 

function DataFrame.Median(colName: string): real;
begin
  Result := Statistics.Median(Self, colName);
end;

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
    Error(ER_NO_VALID_VALUES_COLUMN, colIndex);

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
    Error(ER_NO_VALID_VALUES_COLUMN, colIndex);

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
    Error(ER_NO_VALID_VALUES_COLUMN, colIndex);

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

function DataFrame.DescribeAll: Dictionary<string, DescribeStats>;
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
    ArgumentError(ER_SORTBY_LENGTH_MISMATCH);

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
      Error(ER_UNKNOWN_COLUMN_TYPE);
  end;

  Result := res;
  AssertSchemaConsistent;
end;


{function DataFrame.Select(colIndices: array of integer): DataFrame;
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
end;}

function DataFrame.Select(colIndices: array of integer): DataFrame;
begin
  foreach var i in colIndices do
    CheckColumnIndex(i);

  var newSchema := fSchema.Select(colIndices);
  var newColumns := new List<Column>;

  foreach var i in colIndices do
    newColumns.Add(columns[i]);

  Result := new DataFrame(newColumns, newSchema);
end;

function DataFrame.Select(colNames: array of string): DataFrame;
begin
  var indices := new integer[colNames.Length];

  for var i := 0 to colNames.Length - 1 do
    indices[i] := fSchema.IndexOf(colNames[i]);

  Result := Select(indices);
end;


function DataFrame.Rename(colIndex: integer; newName: string): DataFrame;
begin
  CheckColumnIndex(colIndex);

  var oldName := fSchema.NameAt(colIndex);
  if oldName = newName then
    exit(Self);

  var newSchema := fSchema.Rename(oldName, newName);
  Result := new DataFrame(columns, newSchema);
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
  foreach var i in colIndices do
    CheckColumnIndex(i);

  var drop := new boolean[columns.Count];
  foreach var i in colIndices do
    drop[i] := true;

  var keep := new List<integer>;
  for var i := 0 to columns.Count - 1 do
    if not drop[i] then
      keep.Add(i);

  Result := Select(keep.ToArray);
end;

function DataFrame.Drop(colNames: array of string): DataFrame;
begin
  var indices := new integer[colNames.Length];

  for var i := 0 to colNames.Length - 1 do
    indices[i] := fSchema.IndexOf(colNames[i]);

  Result := Drop(indices);
end;

function DataFrame.BuildJoinSchema(right: DataFrame; leftKeys, rightKeys: array of integer;
  rightPrefix: string): DataFrameSchema;
begin
  Result := DataFrameSchema.Merge(
    fSchema,
    right.fSchema,
    leftKeys,
    rightKeys,
    rightPrefix
  );
end;

function DataFrame.BuildJoinSchema(right: DataFrame; leftKeys, rightKeys: array of string): DataFrameSchema;
begin
  var leftIdx := new integer[leftKeys.Length];
  var rightIdx := new integer[rightKeys.Length];

  for var i := 0 to leftKeys.Length - 1 do
  begin
    leftIdx[i] := fSchema.IndexOf(leftKeys[i]);
    rightIdx[i] := right.fSchema.IndexOf(rightKeys[i]);
  end;

  Result := BuildJoinSchema(
    right,
    leftIdx,
    rightIdx,
    'right_'
  );
end;

function DataFrame.CreateEmptyBySchema(schema: DataFrameSchema): DataFrame;
begin
  var cols := new List<Column>;

  for var i := 0 to schema.ColumnCount - 1 do
  begin
    case schema.Types[i] of
      ctInt:
        cols.Add(new IntColumn(schema.Names[i], schema.IsCategorical[i]));
      ctFloat:
        cols.Add(new FloatColumn(schema.Names[i]));
      ctStr:
        cols.Add(new StrColumn(schema.Names[i], schema.IsCategorical[i]));
      ctBool:
        cols.Add(new BoolColumn(schema.Names[i]));
    end;
  end;

  Result := new DataFrame(cols, schema);
end;

function DataFrame.GetColumnIndex(name: string): integer;
begin
  Result := fschema.IndexOf(name);  // если у вас есть такой метод
end;

function DataFrame.GetColumn(name: string): Column;
begin
  var idx := GetColumnIndex(name);
  Result := columns[idx];
end;

function DataFrame.WithColumnInt(name: string; f: DataFrameCursor -> integer): DataFrame;
begin
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
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
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
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
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
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
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
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

function DataFrame.ReplaceColumnFloat(colName: string; f: DataFrameCursor -> real): DataFrame;
begin
  var colIndex := ColumnIndex(colName);
  var rowCount := RowCount;

  var data := new real[rowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var row := 0;
  while cur.MoveNext do
  begin
    try
      data[row] := f(cur);
      if valid <> nil then valid[row] := true;
    except
      on e: Exception do
      begin
        data[row] := 0.0;
        if valid = nil then
        begin
          valid := new boolean[rowCount];
          for var j := 0 to row - 1 do valid[j] := true;
        end;
        valid[row] := false;
      end;
    end;
    row += 1;
  end;

  var res := new DataFrame;
  for var i := 0 to columns.Count - 1 do
    if i <> colIndex then
      res.AddColumnView(columns[i])
    else
      res.AddFloatColumn(colName, data, valid);

  Result := res;
  Result.AssertSchemaConsistent;
end;

function DataFrame.ReplaceColumnInt(colName: string; f: DataFrameCursor -> integer): DataFrame;
begin
  var colIndex := ColumnIndex(colName);
  var rowCount := RowCount;

  var data := new integer[rowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var row := 0;
  while cur.MoveNext do
  begin
    try
      data[row] := f(cur);
      if valid <> nil then valid[row] := true;
    except
      on e: Exception do
      begin
        data[row] := 0;
        if valid = nil then
        begin
          valid := new boolean[rowCount];
          for var j := 0 to row - 1 do valid[j] := true;
        end;
        valid[row] := false;
      end;
    end;
    row += 1;
  end;

  var res := new DataFrame;
  for var i := 0 to columns.Count - 1 do
    if i <> colIndex then
      res.AddColumnView(columns[i])
    else
      res.AddIntColumn(colName, data, valid);

  Result := res;
  Result.AssertSchemaConsistent;
end;

function DataFrame.AddDerivedIntColumn(
  name: string;
  f: DataFrameCursor -> integer
): DataFrame;
begin
  if Schema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);

  var rowCount := RowCount;
  var data := new integer[rowCount];
  var valid: array of boolean := nil;

  var cur := GetCursor;
  var row := 0;
  while cur.MoveNext do
  begin
    try
      data[row] := f(cur);
      if valid <> nil then valid[row] := true;
    except
      on e: Exception do
      begin
        data[row] := 0;
        if valid = nil then
        begin
          valid := new boolean[rowCount];
          for var j := 0 to row - 1 do valid[j] := true;
        end;
        valid[row] := false;
      end;
    end;
    row += 1;
  end;

  var res := new DataFrame;
  for var i := 0 to columns.Count - 1 do
    res.AddColumnView(columns[i]);

  res.AddIntColumn(name, data, valid);

  Result := res;
  Result.AssertSchemaConsistent;
end;


procedure DataFrame.PrintPreview(maxRows: integer; headRows: integer; decimals: integer);
begin
  var ColumnSeparator := ' ';
  var colCount := columns.Count;
  if colCount = 0 then exit;

  var rowCount := RowCount;
  if rowCount = 0 then exit;

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

  // --- ширины ---
  var widths := new integer[colCount];      // для не-float
  var intWidth := new integer[colCount];    // целая часть float (со знаком)
  var hasFloat := new boolean[colCount];

  for var j := 0 to colCount - 1 do
  begin
    widths[j] := columns[j].Info.Name.Length;
    if columns[j].Info.ColType = ctFloat then
      hasFloat[j] := true;
  end;

  var cursor := GetCursor;

  // --- сканирование строк ---
  var ScanRow: integer -> () := row ->
  begin
    cursor.MoveTo(row);
    for var j := 0 to colCount - 1 do
    begin
      if not cursor.IsValid(j) then
      begin
        if widths[j] < 2 then widths[j] := 2; // 'NA'
        continue;
      end;

      case columns[j].Info.ColType of
        ctInt:
        begin
          var s := cursor.Int(j).ToString;
          if s.Length > widths[j] then widths[j] := s.Length;
        end;

        ctFloat:
        begin
          var v := cursor.Float(j);
          var absInt := Abs(Trunc(v));
          var len := absInt.ToString.Length;
          if v < 0 then len += 1; // знак
          if len > intWidth[j] then intWidth[j] := len;
        end;

        ctStr:
        begin
          var s := cursor.Str(j);
          if s.Length > widths[j] then widths[j] := s.Length;
        end;

        ctBool:
        begin
          var s := cursor.Bool(j).ToString;
          if s.Length > widths[j] then widths[j] := s.Length;
        end;
      end;
    end;
  end;

  for var i := 0 to headRows - 1 do
    ScanRow(i);

  if rowCount > headRows then
    for var i := rowCount - tailRows to rowCount - 1 do
      if i >= headRows then
        ScanRow(i);

  // --- нормализация ширин (табличная детерминированность) ---
  for var j := 0 to colCount - 1 do
  begin
    // "NA" должно влезать всегда
    if columns[j].Info.ColType = ctFloat then
    begin
      var w := intWidth[j] + 1 + decimals;
      if w < 2 then
        intWidth[j] := 2 - 1 - decimals; // чтобы итоговая ширина была >= 2
    end
    else
    begin
      if widths[j] < 2 then
        widths[j] := 2;
    end;
  end;
  

  // --- заголовки ---
  for var j := 0 to colCount - 1 do
  begin
    var w :=
      if columns[j].Info.ColType = ctFloat
      then intWidth[j] + 1 + decimals
      else widths[j];
    PABCSystem.Print(columns[j].Info.Name.PadLeft(w) + ColumnSeparator);
  end;
  PABCSystem.Println;

  // --- форматирование значения ---
  var FormatValue: integer -> string := j ->
  begin
    if not cursor.IsValid(j) then
    begin
      if columns[j].Info.ColType = ctFloat then
        Result := 'NA'.PadLeft(intWidth[j] + 1 + decimals)
      else
        Result := 'NA'.PadLeft(widths[j]);
      exit;
    end;

    case columns[j].Info.ColType of
      ctInt:
        Result := cursor.Int(j).ToString.PadLeft(widths[j]);

      ctFloat:
      begin
        var s := cursor.Float(j).ToString('F' + decimals);
        var p := s.IndexOf('.');
        var left := s.Substring(0, p);
        var right := s.Substring(p + 1);
        Result :=
          left.PadLeft(intWidth[j]) + '.' + right;
      end;

      ctStr:
        Result := cursor.Str(j).PadLeft(widths[j]);

      ctBool:
        Result := cursor.Bool(j).ToString.PadLeft(widths[j]);
    end;
  end;

  // --- печать head ---
  for var i := 0 to headRows - 1 do
  begin
    cursor.MoveTo(i);
    for var j := 0 to colCount - 1 do
      PABCSystem.Print(FormatValue(j) + ColumnSeparator);
    PABCSystem.Println;
  end;

  // --- многоточие ---
// 4. многоточие
  if headRows + tailRows < rowCount then
  begin
    for var j := 0 to colCount - 1 do
    begin
      var w: integer;
  
      if columns[j].Info.ColType = ctFloat then
        w := PABCSystem.Max(widths[j], intWidth[j] + 1 + decimals)
      else
        w := widths[j];
  
      PABCSystem.Print($'…'.PadLeft(w) + ColumnSeparator);
    end;
    PABCSystem.Println;
  end;

  // --- печать tail ---
  for var i := rowCount - tailRows to rowCount - 1 do
    if i >= headRows then
    begin
      cursor.MoveTo(i);
      for var j := 0 to colCount - 1 do
        PABCSystem.Print(FormatValue(j) + ColumnSeparator);
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

procedure DataFrame.PrintSchema;
begin
  var nameWidth := fSchema.Names.Max(s -> s.Length);
  var typeWidth := 6; // Int / Float / Bool

  for var i := 0 to ColumnCount - 1 do
  begin
    var name := fSchema.Names[i].PadRight(nameWidth);
    var typ := GetColumnType(i).ToString.Replace('ct','').PadRight(typeWidth);
    PABCSystem.Println($'{name} : {typ}');
  end;
end;

procedure DataFrame.PrintInfo;
begin
  PABCSystem.Println($'Rows    : {RowCount}');
  PABCSystem.Println($'Columns : {ColumnCount}');

  var nameWidth := fSchema.Names.Max(s -> s.Length);
  var typeWidth := 6; // Int / Float / Bool
  var infoWidth := nameWidth + 3 + typeWidth + 12;

  PABCSystem.Println('=' * infoWidth);

  for var i := 0 to ColumnCount - 1 do
  begin
    var name := fSchema.Names[i].PadRight(nameWidth);
    var typ := GetColumnType(i).ToString.Replace('ct','').PadRight(typeWidth);
    var cnt := Count(i);
    PABCSystem.Println($'{name} : {typ} ({cnt} non-NA)');
  end;
end;



procedure DataFrame.AssertSchemaConsistent;
begin
  {$IFNDEF Test}

  // --- 1. одинаковая RowCount у всех столбцов ---
  var rc := columns[0].RowCount;
  for var i := 1 to columns.Count - 1 do
    if columns[i].RowCount <> rc then
      Error(ER_SCHEMA_INCONSISTENT, columns[i].Info.Name, columns[i].RowCount, rc);

  // --- 2. fschema.ColumnCount = columns.Count ---
  if fschema.ColumnCount <> columns.Count then
    Error(ER_SCHEMA_COLUMNCOUNT_MISMATCH, fSchema.ColumnCount, columns.Count);
  
  // --- 3. имена уникальны и корректно индексированы ---
  for var i := 0 to columns.Count - 1 do
  begin
    var name := columns[i].Info.Name;

    if not fSchema.HasColumn(name) then
      Error(ER_SCHEMA_COLUMN_MISSING, name);
      
    var idx := GetColumnIndex(name);
    if idx <> i then
      Error(ER_SCHEMA_COLUMN_INDEX_INCONSISTENT, name, idx, i);
  end;

  {$ENDIF}
end;

/// Добавляет в DataFrame столбец-представление (view),
/// использующий те же данные, что и исходный столбец
procedure DataFrame.AddColumnView(src: Column);
begin
  case src.Info.ColType of
    ctInt:   AddIntColumn(src.Info.Name, IntColumn(src).Data, IntColumn(src).IsValid, src.Info.IsCategorical);
    ctFloat: AddFloatColumn(src.Info.Name, FloatColumn(src).Data, FloatColumn(src).IsValid);
    ctStr:   AddStrColumn(src.Info.Name, StrColumn(src).Data, StrColumn(src).IsValid, src.Info.IsCategorical);
    ctBool:  AddBoolColumn(src.Info.Name, BoolColumn(src).Data, BoolColumn(src).IsValid);
  end;
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
    Error(ER_COLUMN_NOT_NUMERIC);
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
        else Error(ER_GROUPBY_UNSUPPORTED_KEY_TYPE, df.columns[keyColumn].Info.ColType);
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
          else Error(ER_GROUPBY_UNSUPPORTED_KEY_TYPE, df.columns[c].Info.ColType);
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
//          Statistics
//-----------------------------

static function Statistics.Correlation(df: DataFrame; colX, colY: string): real;
begin
  var ix := df.ColumnIndex(colX);
  var iy := df.ColumnIndex(colY);

  var mx := df.Mean(ix);
  var my := df.Mean(iy);
  var sx := df.Std(ix);
  var sy := df.Std(iy);

  if (sx = 0) or (sy = 0) then
    Error(ER_ZERO_VARIANCE);

  var cur := df.GetCursor;
  var sum := 0.0;
  var cnt := 0;

  while cur.MoveNext do
    if cur.IsValid(ix) and cur.IsValid(iy) then
    begin
      sum += (cur.Float(ix) - mx) * (cur.Float(iy) - my);
      cnt += 1;
    end;

  if cnt = 0 then
    Error(ER_NO_VALID_PAIRS);

  Result := sum / (cnt * sx * sy);
end;

static function Statistics.CorrelationMatrix(df: DataFrame): DataFrame;
begin
  var names := new List<string>;

  // выбираем только числовые столбцы
  for var i := 0 to df.ColumnCount - 1 do
    if df.GetColumnType(i) in [ColumnType.ctInt, ColumnType.ctFloat] then
      names.Add(df.fSchema.Names[i]);

  var n := names.Count;
  if n = 0 then
    Error(ER_NO_NUMERIC_COLUMNS);

  var res := new DataFrame;

  // первый столбец — имена признаков
  res.AddStrColumn('Feature', names.ToArray, nil, true);

  // остальные столбцы — корреляции
  for var j := 0 to n - 1 do
  begin
    var data := new real[n];

    for var i := 0 to n - 1 do
      if i = j then
        data[i] := 1.0
      else
        data[i] := Correlation(df, names[i], names[j]);

    res.AddFloatColumn(names[j], data, nil);
  end;

  Result := res;
end;

static function Statistics.Standardize(df: DataFrame; colName: string): DataFrame;
begin
  var idx := df.ColumnIndex(colName);
  var mean := df.Mean(idx);
  var std := df.Std(idx);

  if std = 0 then
    Error(ER_ZERO_STD_STANDARDIZE);

  Result := df.ReplaceColumnFloat(colName, cur ->
  begin
    if not cur.IsValid(idx) then
      Error(ER_INVALID_VALUE_IN_COLUMN, colName);
    Result := (cur.Float(idx) - mean) / std;
  end);
end;

static function Statistics.StandardizeAll(df: DataFrame): DataFrame;
begin
  var res := new DataFrame;
  var cur := df.GetCursor;

  // 1. заранее считаем mean/std для всех числовых столбцов
  var means := new real[df.ColumnCount];
  var stds  := new real[df.ColumnCount];
  var isNumeric := new boolean[df.ColumnCount];

  for var i := 0 to df.ColumnCount - 1 do
  begin
    var t := df.GetColumnType(i);
    if t in [ColumnType.ctInt, ColumnType.ctFloat] then
    begin
      means[i] := df.Mean(i);
      stds[i] := df.Std(i);
      if stds[i] = 0 then
        Error(ER_ZERO_STD_COLUMN, df.fSchema.Names[i]);
      isNumeric[i] := true;
    end;
  end;

  // 2. создаём схему результата
  for var i := 0 to df.ColumnCount - 1 do
  begin
    if isNumeric[i] then
      res.AddFloatColumn(df.fSchema.Names[i], new real[df.RowCount], nil)
    else
      res.AddColumnView(df.columns[i]); // private helper
  end;

  // 3. заполняем данные
  var row := 0;
  while cur.MoveNext do
  begin
    for var i := 0 to df.ColumnCount - 1 do
    begin
      if not isNumeric[i] then continue;
  
      var col := FloatColumn(res.columns[i]);
  
      if cur.IsValid(i) then
      begin
        col.Data[row] := (cur.Float(i) - means[i]) / stds[i];
        if col.IsValid <> nil then
          col.IsValid[row] := true;
      end
      else
      begin
        // первый NA → создаём IsValid
        if col.IsValid = nil then
        begin
          col.IsValid := new boolean[df.RowCount];
          for var r := 0 to row - 1 do
            col.IsValid[r] := true;
        end;
        col.IsValid[row] := false;
      end;
    end;
    row += 1;
  end;
  
  Result := res;
end;

static function Statistics.Normalize(df: DataFrame; colName: string): DataFrame;
begin
  var idx := df.ColumnIndex(colName);
  var (mn, mx) := df.MinMax(idx);

  if mn = mx then
    Error(ER_ZERO_RANGE);

  Result := df.ReplaceColumnFloat(colName, cur ->
  begin
    if not cur.IsValid(idx) then
      Error(ER_INVALID_VALUE_IN_COLUMN, colName);
    Result := (cur.Float(idx) - mn) / (mx - mn);
  end);
end;

static function Statistics.NormalizeAll(df: DataFrame): DataFrame;
begin
  var res := new DataFrame;
  var cur := df.GetCursor;

  // 1. заранее считаем min/max для всех числовых столбцов
  var mins := new real[df.ColumnCount];
  var maxs := new real[df.ColumnCount];
  var isNumeric := new boolean[df.ColumnCount];

  for var i := 0 to df.ColumnCount - 1 do
  begin
    var t := df.GetColumnType(i);
    if t in [ColumnType.ctInt, ColumnType.ctFloat] then
    begin
      var (mn, mx) := df.MinMax(i);
      if mn = mx then
        Error(ER_ZERO_RANGE_COLUMN, df.fSchema.Names[i]);
      mins[i] := mn;
      maxs[i] := mx;
      isNumeric[i] := true;
    end;
  end;

  // 2. создаём схему результата
  for var i := 0 to df.ColumnCount - 1 do
  begin
    if isNumeric[i] then
      res.AddFloatColumn(df.fSchema.Names[i], new real[df.RowCount], nil)
    else
      res.AddColumnView(df.columns[i]); // private helper
  end;

  // 3. заполняем данные
  var row := 0;
  while cur.MoveNext do
  begin
    for var i := 0 to df.ColumnCount - 1 do
    begin
      if not isNumeric[i] then continue;

      var col := FloatColumn(res.columns[i]);

      if cur.IsValid(i) then
      begin
        col.Data[row] := (cur.Float(i) - mins[i]) / (maxs[i] - mins[i]);
        if col.IsValid <> nil then
          col.IsValid[row] := true;
      end
      else
      begin
        // первый NA → создаём IsValid
        if col.IsValid = nil then
        begin
          col.IsValid := new boolean[df.RowCount];
          for var r := 0 to row - 1 do
            col.IsValid[r] := true;
        end;
        col.IsValid[row] := false;
      end;
    end;
    row += 1;
  end;

  Result := res;
end;

static function Statistics.Quantile(df: DataFrame; colName: string; p: real): real;
begin
  if (p < 0) or (p > 1) then
    ArgumentError(ER_QUANTILE_P_INVALID);

  var idx := df.ColumnIndex(colName);
  var values := new List<real>;

  var cur := df.GetCursor;
  while cur.MoveNext do
    if cur.IsValid(idx) then
      values.Add(cur.Float(idx));

  if values.Count = 0 then
    Error(ER_NO_VALID_VALUES_QUANTILE);

  values.Sort;

  var n := values.Count;
  if n = 1 then
  begin
    Result := values[0];
    exit;
  end;

  // позиция квантиля
  var pos := p * (n - 1);
  var i := Floor(pos);
  var frac := pos - i;

  if i + 1 < n then
    Result := values[i] * (1 - frac) + values[i + 1] * frac
  else
    Result := values[i];
end;

static function Statistics.Median(df: DataFrame; colName: string): real;
begin
  Result := Quantile(df, colName, 0.5);
end;

//-----------------------------
//          CSVLoader
//-----------------------------


{procedure ScanFields(line: string; delimiter: char;
  starts, lens: array of integer; var actualCount: integer);
begin
  var j := 1;
  var col := 0;
  var n := line.Length;

  starts[0] := 1;

  while j <= n do
  begin
    if line[j] = delimiter then
    begin
      if col < starts.Length then
        lens[col] := j - starts[col];
      col += 1;
      if col < starts.Length then
        starts[col] := j + 1;
    end;
    j += 1;
  end;

  if col < starts.Length then
    lens[col] := n - starts[col] + 1;

  for var k := col + 1 to starts.Length - 1 do
    lens[k] := 0;

  actualCount := col + 1;
end;}

procedure ScanFieldsQuoted(
  line: string; delimiter: char;
  starts, lens: array of integer;
  var actualCount: integer;
  var unclosedQuote: boolean);
begin
  var n := line.Length;
  var i := 1;
  var col := 0;

  var inQuotes := False;
  var quotedField := False;

  starts[0] := 1;

  while i <= n do
  begin
    var ch := line[i];

    if inQuotes then
    begin
      // экранированная кавычка ""
      if (ch = '"') and (i < n) and (line[i+1] = '"') then
      begin
        i += 2;
        continue;
      end;

      // закрывающая кавычка
      if ch = '"' then
      begin
        inQuotes := False;
        i += 1;
        continue;
      end;
    end
    else
    begin
      // начало quoted-поля (только если это первый символ поля)
      if (ch = '"') and (i = starts[col]) then
      begin
        inQuotes := True;
        quotedField := True;
        starts[col] := i + 1; // значение начинается после "
        i += 1;
        continue;
      end;

      // разделитель вне кавычек
      if ch = delimiter then
      begin
        if col < starts.Length then
        begin
          if quotedField then
            lens[col] := (i - 1) - starts[col]   // до закрывающей "
          else
            lens[col] := i - starts[col];
        end;

        col += 1;
        quotedField := False;

        if col < starts.Length then
          starts[col] := i + 1;

        i += 1;
        continue;
      end;
    end;

    i += 1;
  end;

  // последний столбец
  if col < starts.Length then
  begin
    if quotedField then
      lens[col] := i - 1 - starts[col]
    else
      lens[col] := i - starts[col];
  end;

  // недостающие столбцы → пустые
  for var k := col + 1 to starts.Length - 1 do
    lens[k] := 0;

  actualCount := col + 1;
  unclosedQuote := inQuotes;
end;


function IsMissingRange(s: string; start, len: integer): boolean;
begin
  // empty
  if len = 0 then
    exit(True);

  // NA
  if (len = 2) and
     (s[start] = 'N') and (s[start+1] = 'A') then
    exit(True);

  // NaN
  if (len = 3) and
     (s[start] = 'N') and (s[start+1] = 'a') and (s[start+2] = 'N') then
    exit(True);

  // null
  if (len = 4) and
     (s[start] = 'n') and (s[start+1] = 'u') and
     (s[start+2] = 'l') and (s[start+3] = 'l') then
    exit(True);

  Result := False;
end;

function TryStrToInt(s: string; start, len: integer; var value: integer): boolean;
begin
  Result := False;
  if len = 0 then exit;

  var j := start;
  var endp := start + len - 1;

  // leading spaces
  while (j <= endp) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j > endp then exit;

  // sign
  var sign := 1;
  if s[j] = '-' then
  begin
    sign := -1;
    j += 1;
  end
  else if s[j] = '+' then
    j += 1;

  if j > endp then exit;

  // first digit
  var c := integer(s[j]);
  if (c < 48) or (c > 57) then exit;

  var res := c - 48;
  j += 1;

  // remaining digits
  while j <= endp do
  begin
    c := integer(s[j]);
    if (c < 48) or (c > 57) then break;

    // overflow check: res * 10 + digit <= Int32.MaxValue
    if res > 214748364 then exit;

    res := res * 10 + (c - 48);
    j += 1;
  end;

  // trailing spaces
  while (j <= endp) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j <= endp then exit;

  if sign = -1 then
    res := -res;

  value := res;
  Result := True;
end;

function TryStrToReal(s: string; start, len: integer; var value: real): boolean;
begin
  Result := False;
  if len = 0 then exit;

  var j := start;
  var endp := start + len - 1;

  // leading spaces
  while (j <= endp) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j > endp then exit;

  // sign
  var sign := 1.0;
  if s[j] = '-' then
  begin
    sign := -1.0;
    j += 1;
  end
  else if s[j] = '+' then
    j += 1;
  if j > endp then exit;

  // integer part
  var intPart := 0.0;
  var hasDigits := False;
  while j <= endp do
  begin
    var c := integer(s[j]);
    if (c < 48) or (c > 57) then break;
    hasDigits := True;
    intPart := intPart * 10.0 + (c - 48);
    j += 1;
  end;

  // fractional part
  var fracPart := 0.0;
  var scale := 1.0;
  if (j <= endp) and (s[j] = '.') then
  begin
    j += 1;
    while j <= endp do
    begin
      var c := integer(s[j]);
      if (c < 48) or (c > 57) then break;
      hasDigits := True;
      scale *= 0.1;
      fracPart += (c - 48) * scale;
      j += 1;
    end;
  end;

  if not hasDigits then exit;

  var res := intPart + fracPart;

  // exponent
  if (j <= endp) and ((s[j] = 'e') or (s[j] = 'E')) then
  begin
    j += 1;
    if j > endp then exit;

    var expSign := 1;
    if s[j] = '-' then
    begin
      expSign := -1;
      j += 1;
    end
    else if s[j] = '+' then
      j += 1;
    if j > endp then exit;

    var exp := 0;
    var hasExp := False;
    while j <= endp do
    begin
      var c := integer(s[j]);
      if (c < 48) or (c > 57) then break;
      hasExp := True;
      exp := exp * 10 + (c - 48);
      j += 1;
    end;
    if not hasExp then exit;

    res := res * Power(10.0, expSign * exp);
  end;

  // trailing spaces
  while (j <= endp) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j <= endp then exit;

  value := sign * res;
  Result := True;
end;

function TryStrToBoolStrictRange(s: string; start, len: integer; var value: boolean): boolean;
begin
  Result := False;
  if len = 0 then exit;

  var j := start;
  var endp := start + len - 1;

  // leading spaces
  while (j <= endp) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j > endp then exit;

  var rem := endp - j + 1;

  // true
  if (rem = 4) and (s[j] = 't') and (s[j+1] = 'r') and (s[j+2] = 'u') and (s[j+3] = 'e') then
  begin value := True; exit(True) end;

  // True
  if (rem = 4) and (s[j] = 'T') and (s[j+1] = 'r') and (s[j+2] = 'u') and (s[j+3] = 'e') then
  begin value := True; exit(True) end;

  // yes
  if (rem = 3) and (s[j] = 'y') and (s[j+1] = 'e') and (s[j+2] = 's') then
  begin value := True; exit(True) end;

  // false
  if (rem = 5) and (s[j] = 'f') and (s[j+1] = 'a') and
     (s[j+2] = 'l') and (s[j+3] = 's') and (s[j+4] = 'e') then
  begin value := False; exit(True) end;

  // False
  if (rem = 5) and (s[j] = 'F') and (s[j+1] = 'a') and
     (s[j+2] = 'l') and (s[j+3] = 's') and (s[j+4] = 'e') then
  begin value := False; exit(True) end;

  // no
  if (rem = 2) and (s[j] = 'n') and (s[j+1] = 'o') then
  begin value := False; exit(True) end;

  // No
  if (rem = 2) and (s[j] = 'N') and (s[j+1] = 'o') then
  begin value := False; exit(True) end;
end;


static function CSVLoader.LoadFromLines(lines: sequence of string; delimiter: char; hasHeader: boolean; 
  missingValues: array of string; strict: boolean; schema: Dictionary<string, ColumnType>): DataFrame;
begin
  // ---------- missing values ----------
  var missing: HashSet<string>;
  if missingValues = nil then
    missing := new HashSet<string>(Arr('', 'NA', 'NaN', 'null'))
  else
    missing := new HashSet<string>(missingValues);

  var raw := lines.ToArray;
  
  // исключаем пустые строки в начале и конце
  var l := 0;
  while (l < raw.Length) and (raw[l].Trim = '') do
    l += 1;
  
  var r := raw.Length;
  while (r > l) and (raw[r-1].Trim = '') do
    r -= 1;
  
  var linesArray := raw[l:r];
  
  // ---------- PASS 1: headers + infer ----------
  var headers: array of string := nil;
  var colCount := 0;
  var rowCount := linesArray.Count;
  if hasHeader then
    rowCount -= 1;
  
  var canBool, canInt, canFloat: array of boolean;
  (canBool, canInt, canFloat) := (nil, nil, nil);
  
  var inferLimit := 1000; // Для определения типа считываем максимум 1000 строк

  var first := true;
  foreach var line in linesArray index inferRead do
  begin
    if inferRead >= inferLimit then break;
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
      canBool  := [True] * colCount;
      canInt   := [True] * colCount;
      canFloat := [True] * colCount;
  
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
        Error(ER_CSV_COLUMN_COUNT_MISMATCH, colCount, parts.Length);
  
    for var j := 0 to colCount - 1 do
    begin
      var s := if j < parts.Length then parts[j] else '';
      if s in missing then continue;
  
      // если тип зафиксирован схемой — инференс не делаем
      if (schema <> nil) and schema.ContainsKey(headers[j]) then
        continue;
  
      var sl := s{.ToLower};
  
      if not ((sl = 'true') or (sl = 'false') or (sl = 'True') or (sl = 'False') or (sl = 'yes') or (sl = 'no')) then
        canBool[j] := false;
  
      var iv: integer;
      if not TryStrToInt(s, iv) then
        canInt[j] := false;
  
      var fv: real;
      if not TryStrToReal(s, fv) then
        canFloat[j] := false;
    end;
  end;
  
  if headers = nil then
    Error(ER_EMPTY_CSV);

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
  var starts := new integer[colCount]; // Для разбиения строки на части вместо lines.Split
  var lens   := new integer[colCount];
  
  var row := 0;
  first := true;
  foreach var line in linesArray do
  begin
    if first then
    begin
      first := false;
      if hasHeader then continue;
    end;
  
    var actualCount: integer;
    var unclosedQuote: boolean;

    ScanFieldsQuoted(line, delimiter, starts, lens, actualCount, unclosedQuote);

    if unclosedQuote then
    begin
      if strict then
        Error(ER_CSV_UNCLOSED_QUOTE);
    
      for var j := 0 to colCount - 1 do
        valid[j][row] := false;
    
      row += 1;
      continue;
    end;
    
    if (actualCount <> colCount) and strict then
      Error(ER_CSV_COLUMN_COUNT_MISMATCH, colCount, actualCount);
  
    for var j := 0 to colCount - 1 do
    begin
      if missingValues = nil then
      begin
        if IsMissingRange(line, starts[j], lens[j]) then
        begin
          valid[j][row] := false;
          continue;
        end;
      end
      else
      begin
        // Тут не очень хорошо - если missingValues заполнено, то материализуем строку - так она и дальше материализуется!
        var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
        if s in missing then
        begin
          valid[j][row] := false;
          continue;
        end;
      end;
      
      // ----- BOOL -----
      if canBool[j] then
      begin
        var bv: boolean;
        if TryStrToBoolStrictRange(line, starts[j], lens[j], bv) then
        begin
          boolData[j][row] := bv;
          valid[j][row] := true;
        end
        else
        begin
          if strict then
            Error(ER_CSV_INVALID_BOOL, line.Substring(starts[j]-1, lens[j]), headers[j]);
          valid[j][row] := false;
        end;
      end
  
      // ----- INT -----
      else if canInt[j] then
      begin
        var iv: integer;
        if TryStrToInt(line, starts[j], lens[j], iv) then
        begin
          intData[j][row] := iv;
          valid[j][row] := true;
        end
        else
        begin
          if strict then
          begin  
            var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
            raise new Exception($'Invalid int "{s}" in column {headers[j]}');
          end;  
          valid[j][row] := false;
        end;
      end
  
      // ----- FLOAT -----
      else if canFloat[j] then
      begin
        var fv: real;
        if TryStrToReal(line, starts[j], lens[j], fv) then
        begin
          floatData[j][row] := fv;
          valid[j][row] := true;
        end
        else
        begin
          if strict then
          begin  
            var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
            raise new Exception($'Invalid float "{s}" in column {headers[j]}');
          end;  
          valid[j][row] := false;
        end;
      end
  
      // ----- STRING -----
      else
      begin
        var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
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