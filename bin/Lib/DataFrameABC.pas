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
    
    //procedure AppendJoinedRow(leftCur, rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
    //procedure AppendLeftOnlyRow(leftCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
    //procedure AppendRightOnlyRow(rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer; leftColumnCount: integer);
    
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
    constructor Create(cols: List<Column>);
    
    function BuildJoinSchema(right: DataFrame; leftKeys, rightKeys: array of integer; 
      rightPrefix: string): DataFrameSchema;
    function BuildJoinSchema(right: DataFrame; leftKeys, rightKeys: array of string): DataFrameSchema;

    function CreateEmptyBySchema(schema: DataFrameSchema): DataFrame;
    
    function GetColumnIndex(name: string): integer;
    function GetColumn(name: string): Column;
  public
    /// Создает пустой DataFrame
    constructor Create;

    /// Схема DataFrame: имена, типы и признаки категориальности
    property Schema: DataFrameSchema read fschema;
    
    procedure SetSchema(schema: DataFrameSchema);
    
    function SetCategorical(names: array of string): DataFrame;
    
    function IsCategorical(name: string): boolean; 
    
    property Item[name: string]: Column read GetColumn; default;
    
    function GetColumns: sequence of Column;
    
    function GetColumn(i: integer): Column;

    /// Добавляет в DataFrame столбец-представление (view),
    /// использующий те же данные, что и исходный столбец
    procedure AddColumnView(src: Column);
    
    function ExtendSchema(name: string; colType: ColumnType; isCategorical: boolean): DataFrameSchema;
    
    
    /// Возвращает количество строк в DataFrame
    function RowCount: integer;
    /// Возвращает количество столбцов в DataFrame
    function ColumnCount: integer;
    /// Возвращает тип столбца по номеру
    function GetColumnType(colIndex: integer): ColumnType;
    /// Возвращает тип столбца по имени
    function GetColumnType(name: string): ColumnType := GetColumnType(ColumnIndex(name));
    /// Возвращает индекс столбца по имени
    function ColumnIndex(name: string): integer;
    
    function HasColumn(name: string): boolean;
   
    /// Создает курсор для итерации по строкам
    function GetCursor: DataFrameCursor;
    
    /// Разбивает таблицу на обучающую и тестовую выборки.
    /// 
    /// Строки случайным образом перемешиваются и делятся на две части:
    /// train и test. Доля тестовой выборки задаётся параметром testRatio.
    /// 
    /// testRatio = 0.2 означает, что 20% строк попадут в тестовую выборку.
    /// 
    /// Если seed >= 0, разбиение будет детерминированным.
    /// Если seed = -1, используется случайная инициализация генератора.
    /// 
    /// Возвращает кортеж (trainDataFrame, testDataFrame).
    function TrainTestSplit(testRatio: real := 0.2; shuffle: boolean := true; seed: integer := -1): (DataFrame, DataFrame);
    
    /// Добавляет столбец целых чисел
    procedure AddIntColumn(name: string; data: array of integer; valid: array of boolean := nil);
    /// Добавляет столбец вещественных чисел
    procedure AddFloatColumn(name: string; data: array of real; valid: array of boolean := nil);
    /// Добавляет строковый столбец
    procedure AddStrColumn(name: string; data: array of string; valid: array of boolean := nil);
    /// Добавляет строковый столбец
    procedure AddStrColumn(name: string; data: array of char; valid: array of boolean := nil);

    /// Добавляет столбец логических значений
    procedure AddBoolColumn(name: string; data: array of boolean; valid: array of boolean := nil);
    
    /// Возвращает данные целочисленного столбца по имени
    function GetIntColumn(name: string): array of integer;
    /// Возвращает данные вещественного столбца по имени
    function GetFloatColumn(name: string): array of real;
    /// Возвращает данные строкового столбца по имени
    function GetStrColumn(name: string): array of string;
    /// Возвращает данные логического столбца по имени
    function GetBoolColumn(name: string): array of boolean;
    
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
    
    /// Преобразует значения целочисленного столбца.
    /// Возвращает новый DataFrame.
    function TransformIntColumn(name: string; f: integer -> integer): DataFrame;
    /// Преобразует значения вещественного столбца.
    /// Возвращает новый DataFrame.
    function TransformFloatColumn(name: string; f: real -> real): DataFrame;
    /// Преобразует значения строкового столбца.
    /// Возвращает новый DataFrame.
    function TransformStrColumn(name: string; f: string -> string): DataFrame;
    /// Преобразует значения логического столбца.
    /// Возвращает новый DataFrame.
    function TransformBoolColumn(name: string; f: boolean -> boolean): DataFrame;

    /// Возвращает новый DataFrame со строками с заданными номерами из исходного DataFrame
    function TakeRows(indices: array of integer): DataFrame;
    
    /// Преобразует указанные вещественные столбцы в целочисленные (округление).
    /// Возвращает новый датафрейм
    function CastFloatToIntColumns(names: array of string): DataFrame;

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
    /// Выводит размер, схему и количество валидных значений 
    procedure PrintlnInfo;

    /// Загружает DataFrame из CSV файла
    static function FromCsv(filename: string): DataFrame;
    /// Загружает DataFrame из многострочной строки в формате CSV
    static function FromCsvText(text: string): DataFrame;
    /// Сохраняет DataFrame в csv-файл
    procedure ToCsv(filename: string);
  private
    /// Проверяет валидность индекса столбца
    procedure CheckColumnIndex(colIndex: integer);
    /// Добавляет строку из курсора
    //procedure AppendRowFromCursor(src: DataFrame; cur: DataFrameCursor);
  end;
  
  /// Тип операции группировки
  AggregationKind = (akCount, akSum, akMean, akStd, akMin, akMax);
  
  GroupView = class
  private
    source: DataFrame;
    indices: List<integer>;
  public
    constructor Create(df: DataFrame; idxs: List<integer>);
  
    function Count: integer;
    function Sum(colName: string): real;
    function Mean(colName: string): real;
    function Min(colName: string): real;
    function Max(colName: string): real;
  end;
  
  /// Интерфейс для группировки данных
  IGroupByContext = interface
    /// Возвращает DataFrame с количеством строк в каждой группе
    function Count: DataFrame;
    /// Возвращает DataFrame со средними значениями указанного столбца по группам
    function Mean(colName: string): DataFrame;
    /// Возвращает DataFrame со средними значениями указанных столбцов по группам
    function Mean(colNames: array of string): DataFrame;
    /// Возвращает сумму значений указанного столбца по группам
    function Sum(colName: string): DataFrame;
    /// Возвращает сумму значений указанных столбцов по группам
    function Sum(colNames: array of string): DataFrame;
    /// Возвращает минимальные значения указанного столбца по группам
    function Min(colName: string): DataFrame;
    /// Возвращает минимальные значения указанных столбцов по группам
    function Min(colNames: array of string): DataFrame;
    /// Возвращает максимальные значения указанного столбца по группам
    function Max(colName: string): DataFrame;
    /// Возвращает максимальные значения указанных столбцов по группам
    function Max(colNames: array of string): DataFrame;
    /// Возвращает стандартное отклонение указанного столбца по группам
    function Std(colName: string): DataFrame;
    /// Возвращает стандартное отклонение указанных столбцов по группам
    function Std(colNames: array of string): DataFrame;
    /// Универсальная агрегация для одной колонки
    function Aggregate(colName: string; kinds: array of AggregationKind): DataFrame;
    /// Универсальная агрегация для нескольких колонок
    function Aggregate(colNames: array of string; kinds: array of AggregationKind): DataFrame;
    
    function Aggregate(map: Dictionary<string, array of AggregationKind>): DataFrame;
    
    /// Возвращает DataFrame с полной статистикой указанного столбца по группам
    function Describe(colName: string): DataFrame;
    /// Возвращает DataFrame с полной статистикой всех числовых столбцов по группам
    function DescribeAll: DataFrame;
    /// Фильтрация групп по пользовательскому условию
    function Filter(pred: Func<GroupView, boolean>): DataFrame;
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
    static function Load(
      filename: string; 
      delimiter: char := ','; 
      hasHeader: boolean := true;      
      encoding: Encoding := nil;
      missingValues: array of string := nil; // Значения, считающиеся пропущенными
      trimWhitespace: boolean := true;
      strict: boolean := False;              // Строгий режим (проверка формата) - бросать ли исключения 
      schema: Dictionary<string, ColumnType> := nil;
      sampleSize: integer := 1000;
      ignoreColumns: array of string := nil;
      inferTypes: boolean := True;
      forceStringColumns: array of string := nil;
      categoricalColumns: array of string := nil;
      inferCategorical: boolean := false;
      maxCategoricalCardinality: integer := 100;
      maxCategoricalRatio: real := 0.2;
      idThreshold: real := 0.8;
      minFrequency: integer := 5
    ): DataFrame;

    /// Загружает DataFrame из многострочной строки в формате CSV
    static function LoadFromLines(
      lines: sequence of string;
      delimiter: char := ',';
      hasHeader: boolean := true;
      missingValues: array of string := nil;
      trimWhitespace: boolean := true;
      strict: boolean := False;
      schema: Dictionary<string, ColumnType> := nil;
      sampleSize: integer := 1000;
      ignoreColumns: array of string := nil;
      inferTypes: boolean := True;
      forceStringColumns: array of string := nil;
      categoricalColumns: array of string := nil;
      inferCategorical: boolean := false;
      maxCategoricalCardinality: integer := 100;
      maxCategoricalRatio: real := 0.2;
      idThreshold: real := 0.8;
      minFrequency: integer := 5
    ): DataFrame;
  end;
  
  CsvSaver = static class 
    static procedure Save(df: DataFrame; filename: string;
      delimiter: char := ','; header: boolean := true);
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
  ER_TEST_RATIO_INVALID =
    'Некорректное значение testRatio = {0}. Ожидается число в диапазоне (0, 1).' +
    'Invalid testRatio = {0}. Expected value in range (0, 1).';
  ER_CAST_COLUMN_NOT_FLOAT =
    'Столбец {0} не является вещественным!!Column {0} is not Float';
  ER_CAST_NON_INTEGER_VALUE =
    'Столбец {0} содержит нецелое значение {1} в строке {2}!!Column {0} contains non-integer value {1} at row {2}';
  ER_FEATURES_EMPTY = 
    'Список признаков пуст!!Feature list is empty';
  ER_AGGREGATIONS_EMPTY = 
    'Список агрегатов пуст!!Aggregation list is empty';
  ER_COLUMN_OUT_OF_RANGE = 
    'Индекс столбца вне диапазона!!Column index is out of range';
  ER_AGG_COLUMN_DUPLICATE = 
    'Дублирующееся имя агрегированной колонки!!Duplicate aggregated column name';
  ER_ROW_INDEX_OUT_OF_RANGE = 
    'Индекс строки вне диапазона!!Row index is out of range';
  ER_UNSUPPORTED_COLUMN_TYPE = 
    'Неподдерживаемый тип столбца!!Unsupported column type';
    
type
  GroupKey = class
  private
    fValues: array of object;
  public
    constructor Create(values: array of object);
    function Equals(obj: object): boolean; override;
    function GetHashCode: integer; override;
  
    property Values: array of object read fValues;
  end;
  
  /// Класс для группировки данных
  GroupByContext = class(IGroupByContext)
  private
    source: DataFrame;
    singleKey: boolean;
    keyColumn: integer;
    groups1: Dictionary<object, List<integer>>;
    keyColumns: array of integer;
    groupsN: Dictionary<GroupKey, List<integer>>;
    
  public
    /// Создает контекст группировки для указанных столбцов
    constructor Create(df: DataFrame; keyCols: array of integer);
    /// Возвращает DataFrame с количеством строк в каждой группе
    function Count: DataFrame;
    /// Возвращает DataFrame со средними значениями указанного столбца по группам
    function Mean(colName: string): DataFrame;
    /// Возвращает сумму значений указанного столбца по группам
    function Sum(colName: string): DataFrame;
    /// Возвращает минимальные значения указанного столбца по группам
    function Min(colName: string): DataFrame;
    /// Возвращает максимальные значения указанного столбца по группам
    function Max(colName: string): DataFrame;
    /// Возвращает стандартное отклонение указанного столбца по группам
    function Std(colName: string): DataFrame;
    /// Возвращает средние значения указанных столбцов по группам
    function Mean(colNames: array of string): DataFrame;
    /// Возвращает суммы указанных столбцов по группам
    function Sum(colNames: array of string): DataFrame;
    /// Возвращает минимумы указанных столбцов по группам
    function Min(colNames: array of string): DataFrame;
    /// Возвращает максимумы указанных столбцов по группам
    function Max(colNames: array of string): DataFrame;
    /// Возвращает стандартные отклонения указанных столбцов по группам
    function Std(colNames: array of string): DataFrame;
    /// Возвращает DataFrame, содержащий строки групп, для которых предикат возвращает true
    function Filter(pred: Func<GroupView, boolean>): DataFrame;    

    function Aggregate(colName: string; kinds: array of AggregationKind): DataFrame;
    
    function Aggregate(colNames: array of string; kinds: array of AggregationKind): DataFrame;
    
    function Aggregate(map: Dictionary<string, array of AggregationKind>): DataFrame;

    /// Возвращает DataFrame с полной статистикой указанного столбца по группам
    function Describe(colName: string): DataFrame;
    /// Возвращает DataFrame с полной статистикой всех числовых столбцов по группам
    function DescribeAll: DataFrame;
  end;
  
//-----------------------------
//        Внешние хелперы
//-----------------------------

procedure BuildMergedIntKeyColumnFromFullJoin(
  res: DataFrame;
  name: string;
  leftCol: IntColumn;
  rightCol: IntColumn;
  leftIdx, rightIdx: array of integer
);
begin
  var n := leftIdx.Length;
  var data := new integer[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var li := leftIdx[i];
    var ri := rightIdx[i];

    if li >= 0 then
    begin
      data[i] := leftCol.Data[li];
      valid[i] := if leftCol.IsValid = nil then true else leftCol.IsValid[li];
    end
    else if ri >= 0 then
    begin
      data[i] := rightCol.Data[ri];
      valid[i] := if rightCol.IsValid = nil then true else rightCol.IsValid[ri];
    end
    else
    begin
      data[i] := 0;
      valid[i] := false;
    end;
  end;

  res.AddIntColumn(name, data, valid);
end;

procedure BuildMergedFloatKeyColumnFromFullJoin(
  res: DataFrame;
  name: string;
  leftCol: FloatColumn;
  rightCol: FloatColumn;
  leftIdx, rightIdx: array of integer
);
begin
  var n := leftIdx.Length;
  var data := new real[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var li := leftIdx[i];
    var ri := rightIdx[i];

    if li >= 0 then
    begin
      data[i] := leftCol.Data[li];
      valid[i] := if leftCol.IsValid = nil then true else leftCol.IsValid[li];
    end
    else if ri >= 0 then
    begin
      data[i] := rightCol.Data[ri];
      valid[i] := if rightCol.IsValid = nil then true else rightCol.IsValid[ri];
    end
    else
    begin
      data[i] := 0.0;
      valid[i] := false;
    end;
  end;

  res.AddFloatColumn(name, data, valid);
end;

procedure BuildMergedStrKeyColumnFromFullJoin(
  res: DataFrame;
  name: string;
  leftCol: StrColumn;
  rightCol: StrColumn;
  leftIdx, rightIdx: array of integer
);
begin
  var n := leftIdx.Length;
  var data := new string[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var li := leftIdx[i];
    var ri := rightIdx[i];

    if li >= 0 then
    begin
      data[i] := leftCol.Data[li];
      valid[i] := if leftCol.IsValid = nil then true else leftCol.IsValid[li];
    end
    else if ri >= 0 then
    begin
      data[i] := rightCol.Data[ri];
      valid[i] := if rightCol.IsValid = nil then true else rightCol.IsValid[ri];
    end
    else
    begin
      data[i] := '';
      valid[i] := false;
    end;
  end;

  res.AddStrColumn(name, data, valid);
end;

procedure BuildMergedBoolKeyColumnFromFullJoin(
  res: DataFrame;
  name: string;
  leftCol: BoolColumn;
  rightCol: BoolColumn;
  leftIdx, rightIdx: array of integer
);
begin
  var n := leftIdx.Length;
  var data := new boolean[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var li := leftIdx[i];
    var ri := rightIdx[i];

    if li >= 0 then
    begin
      data[i] := leftCol.Data[li];
      valid[i] := if leftCol.IsValid = nil then true else leftCol.IsValid[li];
    end
    else if ri >= 0 then
    begin
      data[i] := rightCol.Data[ri];
      valid[i] := if rightCol.IsValid = nil then true else rightCol.IsValid[ri];
    end
    else
    begin
      data[i] := false;
      valid[i] := false;
    end;
  end;

  res.AddBoolColumn(name, data, valid);
end;
  
procedure BuildIntColumnFromJoin(
  res: DataFrame;
  name: string;
  src: IntColumn;
  idx: array of integer
);
begin
  var n := idx.Length;
  var data := new integer[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var j := idx[i];

    if j < 0 then
    begin
      data[i] := 0;
      valid[i] := false;
    end
    else
    begin
      data[i] := src.Data[j];
      valid[i] := if src.IsValid = nil then true else src.IsValid[j];
    end;
  end;

  res.AddIntColumn(name, data, valid);
end;

procedure BuildFloatColumnFromJoin(
  res: DataFrame;
  name: string;
  src: FloatColumn;
  idx: array of integer
);
begin
  var n := idx.Length;
  var data := new real[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var j := idx[i];

    if j < 0 then
    begin
      data[i] := 0.0;
      valid[i] := false;
    end
    else
    begin
      data[i] := src.Data[j];
      valid[i] := if src.IsValid = nil then true else src.IsValid[j];
    end;
  end;

  res.AddFloatColumn(name, data, valid);
end;

procedure BuildStrColumnFromJoin(
  res: DataFrame;
  name: string;
  src: StrColumn;
  idx: array of integer
);
begin
  var n := idx.Length;
  var data := new string[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var j := idx[i];

    if j < 0 then
    begin
      data[i] := '';
      valid[i] := false;
    end
    else
    begin
      data[i] := src.Data[j];
      valid[i] := if src.IsValid = nil then true else src.IsValid[j];
    end;
  end;

  res.AddStrColumn(name, data, valid);
end;

procedure BuildBoolColumnFromJoin(
  res: DataFrame;
  name: string;
  src: BoolColumn;
  idx: array of integer
);
begin
  var n := idx.Length;
  var data := new boolean[n];
  var valid := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var j := idx[i];

    if j < 0 then
    begin
      data[i] := false;
      valid[i] := false;
    end
    else
    begin
      data[i] := src.Data[j];
      valid[i] := if src.IsValid = nil then true else src.IsValid[j];
    end;
  end;

  res.AddBoolColumn(name, data, valid);
end;

//-----------------------------
//          DataFrame
//-----------------------------

constructor DataFrame.Create;
begin
  columns := [];
  fschema := new DataFrameSchema([], []);
end;

constructor DataFrame.Create(cols: List<Column>);
begin
  if cols = nil then
    ArgumentNullError(ER_COLS_NULL);

  self.columns := cols;

  RebuildSchema;
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

{procedure DataFrame.AppendJoinedRow(leftCur, rightCur: DataFrameCursor; leftKeyIdx, rightKeyIdx: array of integer);
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
end;}


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

procedure BuildColumnFromJoin(res: DataFrame; name: string; col: Column; idx: array of integer);
begin
  case col.Info.ColType of
    ctInt:
      BuildIntColumnFromJoin(res, name, IntColumn(col), idx);
    ctFloat:
      BuildFloatColumnFromJoin(res, name, FloatColumn(col), idx);
    ctStr:
      BuildStrColumnFromJoin(res, name, StrColumn(col), idx);
    ctBool:
      BuildBoolColumnFromJoin(res, name, BoolColumn(col), idx);
    else
      Error(ER_UNSUPPORTED_COLUMN_TYPE, col.Info.ColType);
  end;
end;

function DataFrame.LeftJoinSingleKeyInt(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<integer, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Int(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    var lpos := lcur.Position;

    if not lcur.IsValid(leftKey) then
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
      continue;
    end;

    var k := lcur.Int(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.LeftJoinSingleKeyFloat(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<real, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Float(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    var lpos := lcur.Position;

    if not lcur.IsValid(leftKey) then
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
      continue;
    end;

    var k := lcur.Float(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.LeftJoinSingleKeyStr(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<string, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Str(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    var lpos := lcur.Position;

    if not lcur.IsValid(leftKey) then
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
      continue;
    end;

    var k := lcur.Str(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.LeftJoinSingleKeyBool(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<boolean, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Bool(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    var lpos := lcur.Position;

    if not lcur.IsValid(leftKey) then
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
      continue;
    end;

    var k := lcur.Bool(leftKey);

    if index.ContainsKey(k) then
      foreach var rpos in index[k] do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

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
  //=== 

  // 6. probe
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;
  
  var lcur := GetCursor;
  
  while lcur.MoveNext do
  begin
    var lpos := lcur.Position;
  
    var hasNA := false;
    var key := BuildJoinKey(lcur, leftLayout, hasNA);
  
    if (not hasNA) and hash.ContainsKey(key) then
      foreach var rpos in hash[key] do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;
  
  var res := new DataFrame;
  
  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);
  
  // --- right columns (exclude keys)
  for var ci := 0 to other.ColumnCount - 1 do
    if not rightKeyIdx.Contains(ci) then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);
  
  // --- schema
  res.SetSchema(schema);
  
  Result := res;
end;

function DataFrame.ReorderBySchema(schema: DataFrameSchema): DataFrame;
begin
  var cols := new Column[schema.ColumnCount];

  for var i := 0 to schema.ColumnCount - 1 do
    cols[i] := columns[fSchema.IndexOf(schema.ColumnNames[i])];

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
  // --- 1. индексы ключей
  var li := fSchema.IndexOf(key);
  var ri := other.fSchema.IndexOf(key);
  
  if (li < 0) or (ri < 0) then
    Error(ER_JOIN_KEY_NOT_FOUND, key);
  
  var leftKeyIdx  := [li];
  var rightKeyIdx := [ri];

  // --- 2. layout
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // --- 3. hash index (right)
  var hash := other.BuildHashIndex(rightLayout);

  // --- 4. схема
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );

  // --- 5. индексы результата
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var rightUsed := new boolean[other.RowCount];

  var leftCur := GetCursor;

  // --- 6. проход по left
  while leftCur.MoveNext do
  begin
    var lpos := leftCur.Position;

    var hasNA := false;
    var lk := BuildJoinKey(leftCur, leftLayout, hasNA);

    if hasNA then
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
      continue;
    end;

    var rows: List<integer>;
    if hash.TryGetValue(lk, rows) then
      foreach var r in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(r);
        rightUsed[r] := true;
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  // --- 7. строки только справа
  for var r := 0 to other.RowCount - 1 do
    if not rightUsed[r] then
    begin
      leftIdx.Add(-1);
      rightIdx.Add(r);
    end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- 8. left колонки
  for var ci := 0 to ColumnCount - 1 do
  begin
    var col := columns[ci];
    var name := col.Info.Name;
  
    if ci = li then
      case col.Info.ColType of
        ctInt:
          BuildMergedIntKeyColumnFromFullJoin(
            res,
            name,
            IntColumn(col),
            IntColumn(other.columns[ri]),
            leftArr,
            rightArr
          );
  
        ctFloat:
          BuildMergedFloatKeyColumnFromFullJoin(
            res,
            name,
            FloatColumn(col),
            FloatColumn(other.columns[ri]),
            leftArr,
            rightArr
          );
  
        ctStr:
          BuildMergedStrKeyColumnFromFullJoin(
            res,
            name,
            StrColumn(col),
            StrColumn(other.columns[ri]),
            leftArr,
            rightArr
          );
  
        ctBool:
          BuildMergedBoolKeyColumnFromFullJoin(
            res,
            name,
            BoolColumn(col),
            BoolColumn(other.columns[ri]),
            leftArr,
            rightArr
          );
  
        else
          Error(ER_UNSUPPORTED_COLUMN_TYPE, col.Info.ColType);
      end
    else
      BuildColumnFromJoin(res, name, col, leftArr);
  end;

  // --- 9. right колонки (без ключа)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> ri then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- 10. схема
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.FullJoinMultiKey(other: DataFrame; keys: array of string): DataFrame;
begin
  // --- 1. индексы ключей
  var leftKeyIdx  := new integer[keys.Length];
  var rightKeyIdx := new integer[keys.Length];
  
  for var i := 0 to keys.Length - 1 do
  begin
    leftKeyIdx[i]  := fSchema.IndexOf(keys[i]);
    rightKeyIdx[i] := other.fSchema.IndexOf(keys[i]);
  end;

  // --- 2. layout
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // --- 3. hash index (right)
  var hash := other.BuildHashIndex(rightLayout);

  // --- 4. схема
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );

  // --- 5. индексы результата
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;
  var rightUsed := new boolean[other.RowCount];

  var leftCur := GetCursor;

  // --- 6. проход по left
  while leftCur.MoveNext do
  begin
    var lpos := leftCur.Position;

    var hasNA := false;
    var lk := BuildJoinKey(leftCur, leftLayout, hasNA);

    if hasNA then
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
      continue;
    end;

    var rows: List<integer>;
    if hash.TryGetValue(lk, rows) then
      foreach var r in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(r);
        rightUsed[r] := true;
      end
    else
    begin
      leftIdx.Add(lpos);
      rightIdx.Add(-1);
    end;
  end;

  // --- 7. строки только справа
  for var r := 0 to other.RowCount - 1 do
    if not rightUsed[r] then
    begin
      leftIdx.Add(-1);
      rightIdx.Add(r);
    end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- 8. left колонки
  for var ci := 0 to ColumnCount - 1 do
  begin
    var col := columns[ci];
    var name := col.Info.Name;
  
    var keyPos := leftKeyIdx.IndexOf(ci);
  
    if keyPos >= 0 then
    begin
      var ri := rightKeyIdx[keyPos];
  
      case col.Info.ColType of
        ctInt:
          BuildMergedIntKeyColumnFromFullJoin(
            res, name,
            IntColumn(col),
            IntColumn(other.columns[ri]),
            leftArr, rightArr
          );
  
        ctFloat:
          BuildMergedFloatKeyColumnFromFullJoin(
            res, name,
            FloatColumn(col),
            FloatColumn(other.columns[ri]),
            leftArr, rightArr
          );
  
        ctStr:
          BuildMergedStrKeyColumnFromFullJoin(
            res, name,
            StrColumn(col),
            StrColumn(other.columns[ri]),
            leftArr, rightArr
          );
  
        ctBool:
          BuildMergedBoolKeyColumnFromFullJoin(
            res, name,
            BoolColumn(col),
            BoolColumn(other.columns[ri]),
            leftArr, rightArr
          );
  
        else
          Error(ER_UNSUPPORTED_COLUMN_TYPE, col.Info.ColType);
      end;
    end
    else
      BuildColumnFromJoin(res, name, col, leftArr);
  end;

  // --- 9. right колонки (без ключей)
  for var ci := 0 to other.ColumnCount - 1 do
    if not rightKeyIdx.Contains(ci) then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- 10. схема
  res.SetSchema(schema);

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

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Int(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs (только совпадения!)
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
      continue;

    var lpos := lcur.Position;
    var k := lcur.Int(leftKey);

    var rows: List<integer>;
    if index.TryGetValue(k, rows) then
      foreach var rpos in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.JoinInnerSingleKeyFloat(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<real, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Float(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs (только совпадения)
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
      continue;

    var lpos := lcur.Position;
    var k := lcur.Float(leftKey);

    var rows: List<integer>;
    if index.TryGetValue(k, rows) then
      foreach var rpos in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.JoinInnerSingleKeyStr(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<string, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Str(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs (only matches)
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
      continue;

    var lpos := lcur.Position;
    var k := lcur.Str(leftKey);

    var rows: List<integer>;
    if index.TryGetValue(k, rows) then
      foreach var rpos in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.JoinInnerSingleKeyBool(other: DataFrame; leftKey, rightKey: integer): DataFrame;
begin
  var index := new Dictionary<boolean, List<integer>>;

  // --- build index (right)
  var rcur := other.GetCursor;
  while rcur.MoveNext do
    if rcur.IsValid(rightKey) then
    begin
      var k := rcur.Bool(rightKey);

      var lst: List<integer>;
      if not index.TryGetValue(k, lst) then
      begin
        lst := new List<integer>;
        index[k] := lst;
      end;

      lst.Add(rcur.Position);
    end;

  // --- collect index pairs (only matches)
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;
  while lcur.MoveNext do
  begin
    if not lcur.IsValid(leftKey) then
      continue;

    var lpos := lcur.Position;
    var k := lcur.Bool(leftKey);

    var rows: List<integer>;
    if index.TryGetValue(k, rows) then
      foreach var rpos in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude key)
  for var ci := 0 to other.ColumnCount - 1 do
    if ci <> rightKey then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    [leftKey],
    [rightKey],
    'right_'
  );
  res.SetSchema(schema);

  Result := res;
end;

function DataFrame.JoinInnerMultiKey(other: DataFrame; keys: array of string): DataFrame;
begin
  // --- 1. индексы ключей
  var n := keys.Length;
  var leftKeyIdx := new integer[n];
  var rightKeyIdx := new integer[n];

  for var i := 0 to n - 1 do
  begin
    leftKeyIdx[i] := fSchema.IndexOf(keys[i]);
    rightKeyIdx[i] := other.fSchema.IndexOf(keys[i]);
  end;

  // --- 2. проверка типов
  for var i := 0 to n - 1 do
    if fSchema.ColumnTypeAt(leftKeyIdx[i]) <> other.fSchema.ColumnTypeAt(rightKeyIdx[i]) then
      Error(ER_JOIN_KEY_TYPE_MISMATCH);

  // --- 3. layout
  var leftLayout  := BuildJoinKeyLayout(leftKeyIdx);
  var rightLayout := other.BuildJoinKeyLayout(rightKeyIdx);

  // --- 4. hash index (right)
  var hash := other.BuildHashIndex(rightLayout);

  // --- 5. сбор индексов (только совпадения)
  var leftIdx  := new List<integer>;
  var rightIdx := new List<integer>;

  var lcur := GetCursor;

  while lcur.MoveNext do
  begin
    var hasNA := false;
    var key := BuildJoinKey(lcur, leftLayout, hasNA);

    if hasNA then
      continue;

    var lpos := lcur.Position;

    var rows: List<integer>;
    if hash.TryGetValue(key, rows) then
      foreach var rpos in rows do
      begin
        leftIdx.Add(lpos);
        rightIdx.Add(rpos);
      end;
  end;

  var leftArr  := leftIdx.ToArray;
  var rightArr := rightIdx.ToArray;

  var res := new DataFrame;

  // --- left columns
  for var ci := 0 to ColumnCount - 1 do
    BuildColumnFromJoin(res, columns[ci].Info.Name, columns[ci], leftArr);

  // --- right columns (exclude keys)
  for var ci := 0 to other.ColumnCount - 1 do
    if not rightKeyIdx.Contains(ci) then
      BuildColumnFromJoin(res, 'right_' + other.columns[ci].Info.Name, other.columns[ci], rightArr);

  // --- schema
  var schema := DataFrameSchema.Merge(
    fSchema,
    other.fSchema,
    leftKeyIdx,
    rightKeyIdx,
    'right_'
  );
  res.SetSchema(schema);

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
  var cats := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var info := columns[i].Info;
    names[i] := info.Name;
    types[i] := info.ColType;

    // 🔥 берем из старой schema
    if (fSchema <> nil) and (i < fSchema.ColumnCount) then
      cats[i] := fSchema.CategoricalFlags[i]
    else
      cats[i] := false;
  end;

  fSchema := new DataFrameSchema(names, types, cats);
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

function DataFrame.HasColumn(name: string): boolean;
begin
  Result := fSchema.HasColumn(name);
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

function DataFrame.GetStrColumn(name: string): array of string;
begin
  var i := ColumnIndex(name);
  var c := StrColumn(columns[i]);
  Result := c.Data;
end;

function DataFrame.GetBoolColumn(name: string): array of boolean;
begin
  var i := ColumnIndex(name);
  var c := BoolColumn(columns[i]);
  Result := c.Data;
end;


function DataFrame.TrainTestSplit(testRatio: real; shuffle: boolean; seed: integer): (DataFrame, DataFrame);
begin
  if Self = nil then
    ArgumentNullError(ER_ARG_NULL, 'DataFrame');

  if (testRatio <= 0.0) or (testRatio >= 1.0) then
    ArgumentError(ER_TEST_RATIO_INVALID, testRatio);

  var n := RowCount;

  if n < 2 then
    ArgumentError(ER_EMPTY_DATA, 'TrainTestSplit');

  var idx := Arr(0..n - 1);

  if shuffle then
  begin
    var actualSeed := if seed >= 0 then seed else System.Environment.TickCount and integer.MaxValue;
    var rnd := new System.Random(actualSeed);
    idx.Shuffle(rnd);
  end;

  var rawSize := Round(n * testRatio);
  var testSize := rawSize.Clamp(1, n - 1);
  var trainSize := n - testSize;

  var testIdx := new integer[testSize];
  var trainIdx := new integer[trainSize];

  if shuffle then
  begin
    // как раньше
    for var i := 0 to testSize - 1 do
      testIdx[i] := idx[i];

    for var i := 0 to trainSize - 1 do
      trainIdx[i] := idx[testSize + i];
  end
  else
  begin
    // сохраняем порядок
    for var i := 0 to trainSize - 1 do
      trainIdx[i] := idx[i];

    for var i := 0 to testSize - 1 do
      testIdx[i] := idx[trainSize + i];
  end;

  var trainDf := TakeRows(trainIdx);
  var testDf := TakeRows(testIdx);

  Result := (trainDf, testDf);
end;

procedure DataFrame.AddIntColumn(name: string; data: array of integer; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new IntColumn;
  c.Info := new ColumnInfo(name, ctInt);
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

procedure DataFrame.AddFloatColumn(name: string; data: array of real; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new FloatColumn;
  c.Info := new ColumnInfo(name, ctFloat);
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

procedure DataFrame.AddStrColumn(name: string; data: array of string; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new StrColumn;
  c.Info := new ColumnInfo(name, ctStr);
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

procedure DataFrame.AddStrColumn(name: string; data: array of char; valid: array of boolean);
begin
  var dataS: array of string := data.Select(c -> string(c)).ToArray;
  AddStrColumn(name, dataS, valid);
end;

procedure DataFrame.AddBoolColumn(name: string; data: array of boolean; valid: array of boolean);
begin
  if (columns.Count > 0) and (data.Length <> RowCount) then
    DimensionError(ER_ADD_COLUMN_ROW_MISMATCH);

  var c := new BoolColumn;
  c.Info := new ColumnInfo(name, ctBool);
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
  Result := Statistics.Median(Self, fSchema.ColumnNames[colIndex]);
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

{procedure DataFrame.AppendRowFromCursor(src: DataFrame; cur: DataFrameCursor);
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
end;}

function DataFrame.Head(n: integer): DataFrame;
begin
  if n <= 0 then
    exit(new DataFrame);

  var k := PABCSystem.Min(n, RowCount);

  Result := TakeRows(Arr(0..k-1));
end;

function DataFrame.Tail(n: integer): DataFrame;
begin
  if n <= 0 then
    exit(new DataFrame);

  var total := RowCount;
  var k := PABCSystem.Min(n, total);
  var start := total - k;

  Result := TakeRows(Arr(start..total-1));
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
  var idx := new integer[keys.Count];
  
  for var i := 0 to keys.Count - 1 do
    idx[i] := keys[i].Row;
  
  Result := TakeRows(idx);
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

procedure DataFrame.SetSchema(schema: DataFrameSchema);
begin
  if schema = nil then
    ArgumentNullError(ER_SCHEMA_NULL);

  if schema.ColumnCount <> ColumnCount then
    ArgumentError(ER_COLS_SCHEMA_MISMATCH);

  fSchema := schema;
end;

function DataFrame.SetCategorical(names: array of string): DataFrame;
begin
  var n := fSchema.ColumnCount;

  var cats := new boolean[n];

  // копируем старые значения
  for var i := 0 to n - 1 do
    cats[i] := fSchema.CategoricalFlags[i];

  // применяем новые
  foreach var name in names do
    cats[fSchema.IndexOf(name)] := true;

  // создаём НОВУЮ schema
  var newSchema := new DataFrameSchema(
    fSchema.ColumnNames,
    fSchema.Types,
    cats
  );

  // создаём новый DataFrame (columns не копируем!)
  Result := new DataFrame(columns, newSchema);
end;

function DataFrame.IsCategorical(name: string): boolean;
begin
  if name = nil then
    ArgumentNullError(ER_ARG_NULL, 'name');

  if not HasColumn(name) then
    ArgumentError(ER_COLUMN_NOT_FOUND, name);

  Result := Schema.IsCategorical(name);
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

  // --- перенос колонок ---
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

      res.AddIntColumn(src.Info.Name, data, valid);
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

      res.AddStrColumn(src.Info.Name, data, valid);
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

  // 🔥 КЛЮЧЕВОЕ: перенос schema
  var cats := new boolean[ColumnCount];
  for var i := 0 to ColumnCount - 1 do
    cats[i] := fSchema.CategoricalFlags[i];

  res.SetSchema(new DataFrameSchema(
    fSchema.ColumnNames,
    fSchema.Types,
    cats
  ));

  Result := res;
  AssertSchemaConsistent;
end;

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

  // --- копируем колонки ---
  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    var oldName := col.Info.Name;
    var newName := if map.ContainsKey(oldName) then map[oldName] else oldName;

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(newName, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(newName, c.Data, c.IsValid);
      end;

      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(newName, c.Data, c.IsValid);
      end;

      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(newName, c.Data, c.IsValid);
      end;
    end;
  end;

  // 🔥 КЛЮЧЕВОЕ: пересобираем schema
  var n := ColumnCount;
  var names := new string[n];
  var types := new ColumnType[n];
  var cats := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var oldName := fSchema.ColumnNames[i];
    var newName := if map.ContainsKey(oldName) then map[oldName] else oldName;

    names[i] := newName;
    types[i] := fSchema.Types[i];
    cats[i] := fSchema.CategoricalFlags[i];
  end;

  res.SetSchema(new DataFrameSchema(names, types, cats));

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
        cols.Add(new IntColumn(schema.ColumnNames[i]));
      ctFloat:
        cols.Add(new FloatColumn(schema.ColumnNames[i]));
      ctStr:
        cols.Add(new StrColumn(schema.ColumnNames[i]));
      ctBool:
        cols.Add(new BoolColumn(schema.ColumnNames[i]));
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

  // 1. копируем столбцы (без categorical!)
  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
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

  // 2. вычисляем новый столбец
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

  // 3. добавляем новый столбец
  res.AddIntColumn(name, data, valid);

  res.SetSchema(ExtendSchema(name, ctInt, false));

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.WithColumnFloat(name: string; f: DataFrameCursor -> real): DataFrame;
begin
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
  var res := new DataFrame;

  // 1. копируем существующие колонки
  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
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

  // 2. вычисляем новый столбец
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

  // 3. добавляем новый столбец
  res.AddFloatColumn(name, data, valid);

  res.SetSchema(ExtendSchema(name, ctFloat, false));

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.WithColumnStr(name: string; f: DataFrameCursor -> string): DataFrame;
begin
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
  var res := new DataFrame;

  // 1. копируем существующие колонки
  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
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

  // 2. вычисляем новый столбец
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

  // 3. добавляем новый столбец
  res.AddStrColumn(name, data, valid);

  res.SetSchema(ExtendSchema(name, ctStr, false));

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.WithColumnBool(name: string; f: DataFrameCursor -> boolean): DataFrame;
begin
  if fSchema.HasColumn(name) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, name);
  
  var res := new DataFrame;

  // 1. копируем существующие колонки
  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];
    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
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

  // 2. вычисляем новый столбец
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

  // 3. добавляем новый столбец
  res.AddBoolColumn(name, data, valid);

  res.SetSchema(ExtendSchema(name, ctBool, false));

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.ExtendSchema(
  name: string;
  colType: ColumnType;
  isCategorical: boolean
): DataFrameSchema;
begin
  var oldN := fSchema.ColumnCount;
  var newN := oldN + 1;

  var names := new string[newN];
  var types := new ColumnType[newN];
  var cats := new boolean[newN];

  for var j := 0 to oldN - 1 do
  begin
    names[j] := fSchema.ColumnNames[j];
    types[j] := fSchema.Types[j];
    cats[j] := fSchema.CategoricalFlags[j];
  end;

  names[oldN] := name;
  types[oldN] := colType;
  cats[oldN] := isCategorical;

  Result := new DataFrameSchema(names, types, cats);
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

function DataFrame.TransformIntColumn(name: string; f: integer -> integer): DataFrame;
begin
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);

        if c.Info.Name = name then
        begin
          var data := c.Data;
          var newData := new integer[data.Length];

          for var j := 0 to data.Length - 1 do
            newData[j] := f(data[j]);

          res.AddIntColumn(name, newData, c.IsValid);
        end
        else
          res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid);
      end;
    end;
  end;

  // 🔥 КЛЮЧЕВОЕ: schema НЕ меняется
  res.SetSchema(fSchema);

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.TransformFloatColumn(name: string; f: real -> real): DataFrame;
begin
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctFloat:
      begin
        var c := FloatColumn(col);

        if c.Info.Name = name then
        begin
          var data := c.Data;
          var newData := new real[data.Length];

          for var j := 0 to data.Length - 1 do
            newData[j] := f(data[j]);

          res.AddFloatColumn(name, newData, c.IsValid);
        end
        else
          res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctBool:
      begin
        var c := BoolColumn(col);
        res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid);
      end;
    end;
  end;

  // 🔥 schema просто копируется
  res.SetSchema(fSchema);

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.TransformStrColumn(name: string; f: string -> string): DataFrame;
begin
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
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

      ctStr:
      begin
        var c := StrColumn(col);

        if c.Info.Name = name then
        begin
          var data := c.Data;
          var newData := new string[data.Length];

          for var j := 0 to data.Length - 1 do
            newData[j] := f(data[j]);

          res.AddStrColumn(name, newData, c.IsValid);
        end
        else
          res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
      end;
    end;
  end;

  // 🔥 schema НЕ меняется
  res.SetSchema(fSchema);

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.TransformBoolColumn(name: string; f: boolean -> boolean): DataFrame;
begin
  var res := new DataFrame;

  for var i := 0 to columns.Count - 1 do
  begin
    var col := columns[i];

    case col.Info.ColType of
      ctBool:
      begin
        var c := BoolColumn(col);

        if c.Info.Name = name then
        begin
          var data := c.Data;
          var newData := new boolean[data.Length];

          for var j := 0 to data.Length - 1 do
            newData[j] := f(data[j]);

          res.AddBoolColumn(name, newData, c.IsValid);
        end
        else
          res.AddBoolColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctInt:
      begin
        var c := IntColumn(col);
        res.AddIntColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctFloat:
      begin
        var c := FloatColumn(col);
        res.AddFloatColumn(c.Info.Name, c.Data, c.IsValid);
      end;

      ctStr:
      begin
        var c := StrColumn(col);
        res.AddStrColumn(c.Info.Name, c.Data, c.IsValid);
      end;
    end;
  end;

  // 🔥 schema просто копируется
  res.SetSchema(fSchema);

  Result := res;

  AssertSchemaConsistent;
end;

function DataFrame.TakeRows(indices: array of integer): DataFrame;
begin
  if indices = nil then
    ArgumentNullError(ER_ARG_NULL, 'indices');

  var k := indices.Length;
  var res := new DataFrame;

  var names := new List<string>;
  var types := new List<ColumnType>;
  var cats := new List<boolean>;

  for var ci := 0 to ColumnCount - 1 do
  begin
    var col := columns[ci];
    var name := col.Info.Name;
    var colType := col.Info.ColType;

    case colType of

      ctInt:
      begin
        var src := IntColumn(col);
        var data := new integer[k];
        var validSrc := src.IsValid;
        var validDst: array of boolean := nil;

        if validSrc <> nil then
          validDst := new boolean[k];

        for var j := 0 to k - 1 do
        begin
          var i := indices[j];

          if (i < 0) or (i >= RowCount) then
            ArgumentError(ER_ROW_INDEX_OUT_OF_RANGE, i);

          if validSrc = nil then
            data[j] := src.Data[i]
          else if validSrc[i] then
          begin
            data[j] := src.Data[i];
            validDst[j] := true;
          end
          else
            validDst[j] := false;
        end;

        res.AddIntColumn(name, data, validDst);
      end;

      ctFloat:
      begin
        var src := FloatColumn(col);
        var data := new real[k];
        var validSrc := src.IsValid;
        var validDst: array of boolean := nil;

        if validSrc <> nil then
          validDst := new boolean[k];

        for var j := 0 to k - 1 do
        begin
          var i := indices[j];

          if (i < 0) or (i >= RowCount) then
            ArgumentError(ER_ROW_INDEX_OUT_OF_RANGE, i);

          if validSrc = nil then
            data[j] := src.Data[i]
          else if validSrc[i] then
          begin
            data[j] := src.Data[i];
            validDst[j] := true;
          end
          else
            validDst[j] := false;
        end;

        res.AddFloatColumn(name, data, validDst);
      end;

      ctStr:
      begin
        var src := StrColumn(col);
        var data := new string[k];
        var validSrc := src.IsValid;
        var validDst: array of boolean := nil;

        if validSrc <> nil then
          validDst := new boolean[k];

        for var j := 0 to k - 1 do
        begin
          var i := indices[j];

          if (i < 0) or (i >= RowCount) then
            ArgumentError(ER_ROW_INDEX_OUT_OF_RANGE, i);

          if validSrc = nil then
            data[j] := src.Data[i]
          else if validSrc[i] then
          begin
            data[j] := src.Data[i];
            validDst[j] := true;
          end
          else
            validDst[j] := false;
        end;

        res.AddStrColumn(name, data, validDst);
      end;

      ctBool:
      begin
        var src := BoolColumn(col);
        var data := new boolean[k];
        var validSrc := src.IsValid;
        var validDst: array of boolean := nil;

        if validSrc <> nil then
          validDst := new boolean[k];

        for var j := 0 to k - 1 do
        begin
          var i := indices[j];

          if (i < 0) or (i >= RowCount) then
            ArgumentError(ER_ROW_INDEX_OUT_OF_RANGE, i);

          if validSrc = nil then
            data[j] := src.Data[i]
          else if validSrc[i] then
          begin
            data[j] := src.Data[i];
            validDst[j] := true;
          end
          else
            validDst[j] := false;
        end;

        res.AddBoolColumn(name, data, validDst);
      end;

      else
        Error(ER_UNSUPPORTED_COLUMN_TYPE, colType);
    end;

    names.Add(name);
    types.Add(colType);
    cats.Add(IsCategorical(col.Info.Name));
  end;

  res.SetSchema(new DataFrameSchema(
    names.ToArray,
    types.ToArray,
    cats.ToArray
  ));

  Result := res;
end;

procedure DataFrame.PrintPreview(maxRows: integer; headRows: integer; decimals: integer);
begin
  var colCount := columns.Count;
  if colCount = 0 then exit;

  var rowCount := RowCount;
  if rowCount = 0 then exit;

  if maxRows < 1 then exit;
  if decimals < 0 then decimals := 0;

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

  if tailRows < 0 then
    tailRows := 0;

  if tailRows > rowCount - headRows then
    tailRows := rowCount - headRows;

  var widths := new integer[colCount];

  // --- начальная ширина = длина заголовка ---
  for var j := 0 to colCount - 1 do
  begin
    widths[j] := columns[j].Info.Name.Length;
    if widths[j] < 2 then
      widths[j] := 2;
  end;

  var cursor := GetCursor;

  // --- scan head ---
  for var i := 0 to headRows - 1 do
  begin
    cursor.MoveTo(i);

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

  // --- scan tail ---
  if rowCount > headRows then
    for var i := rowCount - tailRows to rowCount - 1 do
      if i >= headRows then
      begin
        cursor.MoveTo(i);

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

  // --- header ---
  for var j := 0 to colCount - 1 do
  begin
    PABCSystem.Print(columns[j].Info.Name.PadLeft(widths[j]));
    if j < colCount - 1 then
      Write(' ');
  end;

  PABCSystem.Println;

  // --- head rows ---
  for var i := 0 to headRows - 1 do
  begin
    cursor.MoveTo(i);

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

      PABCSystem.Print(s.PadLeft(widths[j]));

      if j < colCount - 1 then
        Write(' ');
    end;

    PABCSystem.Println;
  end;

  // --- ellipsis ---
  if headRows + tailRows < rowCount then
  begin
    for var j := 0 to colCount - 1 do
    begin
      PABCSystem.Print($'…'.PadLeft(widths[j]));
      if j < colCount - 1 then
        Write(' ');
    end;

    PABCSystem.Println;
  end;

  // --- tail rows ---
  for var i := rowCount - tailRows to rowCount - 1 do
    if i >= headRows then
    begin
      cursor.MoveTo(i);

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

        PABCSystem.Print(s.PadLeft(widths[j]));

        if j < colCount - 1 then
          Write(' ');
      end;

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
  PrintPreview(10, 5, decimals);
end;

procedure DataFrame.Println(decimals: integer);
begin
  Print(decimals);
  PABCSystem.Println;
end;

procedure DataFrame.PrintSchema;
begin
  var nameWidth := fSchema.ColumnNames.Max(s -> s.Length);
  var typeWidth := 6; // Int / Float / Bool

  for var i := 0 to ColumnCount - 1 do
  begin
    var name := fSchema.ColumnNames[i].PadRight(nameWidth);
    var typ := GetColumnType(i).ToString.Replace('ct','').PadRight(typeWidth);
    PABCSystem.Println($'{name} : {typ}');
  end;
end;

function ColumnTypeToString(ct: ColumnType): string;
begin
  case ct of
    ctInt: Result := 'int';
    ctFloat: Result := 'float';
    ctStr: Result := 'string';
    ctBool: Result := 'bool';
  end;
end;

procedure DataFrame.PrintInfo;
begin
  PABCSystem.Println($'Rows    : {RowCount}');
  PABCSystem.Println($'Columns : {ColumnCount}');

  var nameWidth := fSchema.ColumnNames.Max(s -> s.Length);

  // сначала считаем реальные строки типов
  var types := new string[ColumnCount];
  var maxTypeWidth := 0;

  for var i := 0 to ColumnCount - 1 do
  begin
    var t := ColumnTypeToString(GetColumnType(i));
    if fSchema.CategoricalFlags[i] then
      t += ' (categorical)';

    types[i] := t;

    if t.Length > maxTypeWidth then
      maxTypeWidth := t.Length;
  end;

  var infoWidth := nameWidth + 3 + maxTypeWidth + 20;

  PABCSystem.Println('=' * infoWidth);

  for var i := 0 to ColumnCount - 1 do
  begin
    var name := fSchema.ColumnNames[i].PadRight(nameWidth);
    var typ := types[i].PadRight(maxTypeWidth);
    var cnt := Count(i);

    PABCSystem.Println($'{name} : {typ} ({cnt} non-NA)');
  end;
end;

procedure DataFrame.PrintlnInfo;
begin
  PrintInfo;
  PABCSystem.Println
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

function DataFrame.GetColumns: sequence of Column;
begin
  foreach var c in columns do
    yield c;
end;

function DataFrame.GetColumn(i: integer): Column;
begin
  Result := columns[i];
end;

/// Добавляет в DataFrame столбец-представление (view),
/// использующий те же данные, что и исходный столбец
procedure DataFrame.AddColumnView(src: Column);
begin
  case src.Info.ColType of
    ctInt:
      AddIntColumn(src.Info.Name, IntColumn(src).Data, IntColumn(src).IsValid);

    ctFloat:
      AddFloatColumn(src.Info.Name, FloatColumn(src).Data, FloatColumn(src).IsValid);

    ctStr:
      AddStrColumn(src.Info.Name, StrColumn(src).Data, StrColumn(src).IsValid);

    ctBool:
      AddBoolColumn(src.Info.Name, BoolColumn(src).Data, BoolColumn(src).IsValid);
  end;
end;

function DataFrame.CastFloatToIntColumns(names: array of string): DataFrame;
begin
  var toCast := new HashSet<string>(names);

  var res := new DataFrame;

  foreach var col in GetColumns do
  begin
    var name := col.Info.Name;

    if toCast.Contains(name) then
    begin
      if col.Info.ColType <> ctFloat then
        ArgumentError(ER_CAST_COLUMN_NOT_FLOAT, name);

      var fc := FloatColumn(col);
      var n := fc.Data.Length;
      var data := new integer[n];

      for var i := 0 to n - 1 do
      begin
        if not fc.IsValid[i] then
          continue;

        var v := fc.Data[i];
        var iv := Round(v);

        if Abs(v - iv) > 1e-9 then
          ArgumentError(ER_CAST_NON_INTEGER_VALUE, name, v, i + 1);

        data[i] := iv;
      end;

      res.AddIntColumn(name, data, fc.IsValid);
    end
    else
      res.AddColumnView(col);
  end;

  // 🔥 пересобираем schema (меняются ТИПЫ)
  var n := fSchema.ColumnCount;

  var namesArr := new string[n];
  var types := new ColumnType[n];
  var cats := new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var name := fSchema.ColumnNames[i];

    namesArr[i] := name;
    cats[i] := fSchema.CategoricalFlags[i];

    if toCast.Contains(name) then
      types[i] := ctInt
    else
      types[i] := fSchema.Types[i];
  end;

  res.SetSchema(new DataFrameSchema(namesArr, types, cats));

  Result := res;
end;

static function DataFrame.FromCsv(filename: string): DataFrame;
begin
  Result := CsvLoader.Load(filename);
end;

static function DataFrame.FromCsvText(text: string): DataFrame;
begin
  Result := CsvLoader.LoadFromLines(text.ToLines);
end;

procedure DataFrame.ToCsv(filename: string);
begin
  CsvSaver.Save(self, filename, ',', true);
end;


//-----------------------------
//           GroupKey
//-----------------------------

constructor GroupKey.Create(values: array of object);
begin
  if values = nil then
    ArgumentNullError(ER_ARG_NULL, 'values');

  fValues := Copy(values);
end;

function GroupKey.Equals(obj: object): boolean;
begin
  if obj = nil then
  begin
    Result := false;
    exit;
  end;

  var other := obj as GroupKey;
  if other = nil then
  begin
    Result := false;
    exit;
  end;

  if fValues.Length <> other.fValues.Length then
  begin
    Result := false;
    exit;
  end;

  for var i := 0 to fValues.Length - 1 do
  begin
    var a := fValues[i];
    var b := other.fValues[i];

    if a = nil then
    begin
      if b <> nil then
      begin
        Result := false;
        exit;
      end;
    end
    else if not a.Equals(b) then
    begin
      Result := false;
      exit;
    end;
  end;

  Result := true;
end;

function GroupKey.GetHashCode: integer;
begin
  var h := 17;

  for var i := 0 to fValues.Length - 1 do
  begin
    var x := fValues[i];
    var xh := if x = nil then 0 else x.GetHashCode;
    h := h * 31 + xh;
  end;

  Result := h;
end;

//-----------------------------
//          GroupView
//-----------------------------

procedure GetNumericColumn(
  df: DataFrame;
  colIndex: integer;
  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean
);
begin
  if (colIndex < 0) or (colIndex >= df.ColumnCount) then
    ArgumentError(ER_COLUMN_OUT_OF_RANGE, colIndex);
  
  var col := df.columns[colIndex];

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

constructor GroupView.Create(df: DataFrame; idxs: List<integer>);
begin
  source := df;
  indices := idxs;
end;

function GroupView.Count: integer;
begin
  Result := indices.Count;
end;

function GroupView.Sum(colName: string): real;
begin
  var ci := source.ColumnIndex(colName);

  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean;

  // используем ТВОЙ helper
  GetNumericColumn(source, ci, dataInt, dataFloat, valid, isInt);

  var s := 0.0;

  for var j := 0 to indices.Count - 1 do
  begin
    var i := indices[j];

    if (valid <> nil) and not valid[i] then
      continue;

    s += if isInt then dataInt[i] else dataFloat[i];
  end;

  Result := s;
end;

function GroupView.Mean(colName: string): real;
begin
  var ci := source.ColumnIndex(colName);

  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean;

  GetNumericColumn(source, ci, dataInt, dataFloat, valid, isInt);

  var s := 0.0;
  var cnt := 0;

  for var j := 0 to indices.Count - 1 do
  begin
    var i := indices[j];

    if (valid <> nil) and not valid[i] then
      continue;

    s += if isInt then dataInt[i] else dataFloat[i];
    cnt += 1;
  end;

  Result := if cnt = 0 then 0.0 else s / cnt;
end;

function GroupView.Min(colName: string): real;
begin
  var ci := source.ColumnIndex(colName);

  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean;

  GetNumericColumn(source, ci, dataInt, dataFloat, valid, isInt);

  var m := real.MaxValue;
  var has := false;

  for var j := 0 to indices.Count - 1 do
  begin
    var i := indices[j];

    if (valid <> nil) and not valid[i] then
      continue;

    var v := if isInt then dataInt[i] else dataFloat[i];

    if not has or (v < m) then
    begin
      m := v;
      has := true;
    end;
  end;

  Result := if has then m else 0.0;
end;

function GroupView.Max(colName: string): real;
begin
  var ci := source.ColumnIndex(colName);

  var dataInt: array of integer;
  var dataFloat: array of real;
  var valid: array of boolean;
  var isInt: boolean;

  GetNumericColumn(source, ci, dataInt, dataFloat, valid, isInt);

  var m := real.MinValue;
  var has := false;

  for var j := 0 to indices.Count - 1 do
  begin
    var i := indices[j];

    if (valid <> nil) and not valid[i] then
      continue;

    var v := if isInt then dataInt[i] else dataFloat[i];

    if not has or (v > m) then
    begin
      m := v;
      has := true;
    end;
  end;

  Result := if has then m else 0.0;
end;

//-----------------------------
//        GroupByContext
//-----------------------------

constructor GroupByContext.Create(df: DataFrame; keyCols: array of integer);
begin
  source := df;
  
  if (keyCols = nil) or (keyCols.Length = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

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

      var lst: List<integer>;
      if not groups1.TryGetValue(key, lst) then
      begin
        lst := new List<integer>;
        groups1[key] := lst;
      end;

      lst.Add(cursor.Position);
    end;
  end
  else
  begin
    singleKey := false;
    keyColumns := Copy(keyCols);
    groupsN := new Dictionary<GroupKey, List<integer>>;
  
    var cursor := df.GetCursor;
    while cursor.MoveNext do
    begin
      var values := new object[keyColumns.Length];
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
          ctInt: values[i] := cursor.Int(c);
          ctStr: values[i] := cursor.Str(c);
          else Error(ER_GROUPBY_UNSUPPORTED_KEY_TYPE, df.columns[c].Info.ColType);
        end;
      end;
  
      if not ok then
        continue;
  
      var key := new GroupKey(values);
  
      var lst: List<integer>;
      if not groupsN.TryGetValue(key, lst) then
      begin
        lst := new List<integer>;
        groupsN[key] := lst;
      end;
      
      lst.Add(cursor.Position);
    end;
  end;
end;

function GroupByContext.Count: DataFrame;
begin
  var res := new DataFrame;

  var names := new List<string>;
  var types := new List<ColumnType>;
  var cats := new List<boolean>;

  if singleKey then
  begin
    var keys := groups1.Select(kvp -> kvp.Key).ToArray;
    var counts := new integer[keys.Length];

    for var i := 0 to keys.Length - 1 do
      counts[i] := groups1[keys[i]].Count;

    var col := source.columns[keyColumn];
    var keyName := col.Info.Name;

    if col.Info.ColType = ctInt then
    begin
      res.AddIntColumn(keyName, keys.Select(k -> integer(k)).ToArray, nil);
      types.Add(ctInt);
    end
    else
    begin
      res.AddStrColumn(keyName, keys.Select(k -> string(k)).ToArray, nil);
      types.Add(ctStr);
    end;

    names.Add(keyName);
    cats.Add(true); // ключ — categorical

    res.AddIntColumn('count', counts, nil);
    names.Add('count');
    types.Add(ctInt);
    cats.Add(false);
  end
  else
  begin
    var keys := groupsN.Select(kvp -> kvp.Key).ToArray;
    var counts := new integer[keys.Length];

    for var i := 0 to keys.Length - 1 do
      counts[i] := groupsN[keys[i]].Count;

    for var k := 0 to keyColumns.Length - 1 do
    begin
      var ci := keyColumns[k];
      var col := source.columns[ci];
      var colName := col.Info.Name;

      if col.Info.ColType = ctInt then
      begin
        res.AddIntColumn(colName, keys.Select(key -> integer(key.Values[k])).ToArray, nil);
        types.Add(ctInt);
      end
      else
      begin
        res.AddStrColumn(colName, keys.Select(key -> string(key.Values[k])).ToArray, nil);
        types.Add(ctStr);
      end;

      names.Add(colName);
      cats.Add(true); // 🔥 ключи — categorical
    end;

    res.AddIntColumn('count', counts, nil);
    names.Add('count');
    types.Add(ctInt);
    cats.Add(false);
  end;

  // 🔥 устанавливаем schema
  res.SetSchema(new DataFrameSchema(
    names.ToArray,
    types.ToArray,
    cats.ToArray
  ));

  Result := res;
end;

function GroupByContext.Mean(colName: string): DataFrame;
begin
  Result := Aggregate([colName], [akMean]);
end;

function GroupByContext.Sum(colName: string): DataFrame;
begin
  Result := Aggregate([colName], [akSum]);
end;

function GroupByContext.Min(colName: string): DataFrame;
begin
  Result := Aggregate([colName], [akMin]);
end;

function GroupByContext.Max(colName: string): DataFrame;
begin
  Result := Aggregate([colName], [akMax]);
end;

function GroupByContext.Std(colName: string): DataFrame;
begin
  Result := Aggregate([colName], [akStd]);
end;

function GroupByContext.Mean(colNames: array of string): DataFrame;
begin
  Result := Aggregate(colNames, [akMean]);
end;

function GroupByContext.Sum(colNames: array of string): DataFrame;
begin
  Result := Aggregate(colNames, [akSum]);
end;

function GroupByContext.Min(colNames: array of string): DataFrame;
begin
  Result := Aggregate(colNames, [akMin]);
end;

function GroupByContext.Max(colNames: array of string): DataFrame;
begin
  Result := Aggregate(colNames, [akMax]);
end;

function GroupByContext.Std(colNames: array of string): DataFrame;
begin
  Result := Aggregate(colNames, [akStd]);
end;

function GroupByContext.Filter(pred: Func<GroupView, boolean>): DataFrame;
begin
  if pred = nil then
    ArgumentNullError(ER_ARG_NULL, 'pred');

  var selected := new List<integer>;

  if singleKey then
  begin
    var keys := groups1.Select(kvp -> kvp.Key).ToArray;

    for var i := 0 to keys.Length - 1 do
    begin
      var idxs := groups1[keys[i]];
      var g := new GroupView(source, idxs);

      if pred(g) then
        selected.AddRange(idxs);
    end;
  end
  else
  begin
    var keys := groupsN.Select(kvp -> kvp.Key).ToArray;

    for var i := 0 to keys.Length - 1 do
    begin
      var idxs := groupsN[keys[i]];
      var g := new GroupView(source, idxs);

      if pred(g) then
        selected.AddRange(idxs);
    end;
  end;

  // важно: порядок строк сохраняется как в исходных группах
  Result := source.TakeRows(selected.ToArray);
end;

function GroupByContext.Describe(colName: string): DataFrame;
begin
  Result := Aggregate(
    [colName],
    [akCount, akMean, akStd, akMin, akMax]
  );
end;

function GroupByContext.DescribeAll: DataFrame;
begin
  var cols := new List<string>;

  // собираем все числовые колонки
  for var i := 0 to source.ColumnCount - 1 do
    case source.columns[i].Info.ColType of
      ctInt, ctFloat:
        cols.Add(source.columns[i].Info.Name);
    end;

  if cols.Count = 0 then
    ArgumentError(ER_FEATURES_EMPTY);

  Result := Aggregate(
    cols.ToArray,
    [akCount, akMean, akStd, akMin, akMax]
  );
end;

function GroupByContext.Aggregate(colName: string; kinds: array of AggregationKind): DataFrame;
begin
  Result := Aggregate([colName], kinds);
end;

function GroupByContext.Aggregate(colNames: array of string; kinds: array of AggregationKind): DataFrame;
begin
  if (colNames = nil) or (colNames.Length = 0) then
    ArgumentError(ER_FEATURES_EMPTY);

  if (kinds = nil) or (kinds.Length = 0) then
    ArgumentError(ER_AGGREGATIONS_EMPTY);

  var m := colNames.Length;
  var n := if singleKey then groups1.Count else groupsN.Count;

  // ----------------------------
  // 1. Какие агрегаты реально нужны
  // ----------------------------
  var needCount := false;
  var needSum := false;
  var needMean := false;
  var needStd := false;
  var needMin := false;
  var needMax := false;

  foreach var kind in kinds do
    case kind of
      akCount: needCount := true;
      akSum:   needSum := true;
      akMean:  needMean := true;
      akStd:   needStd := true;
      akMin:   needMin := true;
      akMax:   needMax := true;
    end;

  if needMean then
  begin
    needCount := true;
    needSum := true;
  end;

  var needSumSq := needStd;
  if needStd then
  begin
    needCount := true;
    needSum := true;
  end;

  // ----------------------------
  // 2. Подготовка ссылок на колонки
  // ----------------------------
  var colIndices := new integer[m];
  var colsInt := new List<array of integer>;
  var colsFloat := new List<array of real>;
  var colsValid := new List<array of boolean>;
  var colsIsInt := new boolean[m];

  for var c := 0 to m - 1 do
  begin
    var ci := source.ColumnIndex(colNames[c]);
    colIndices[c] := ci;

    var dataInt: array of integer;
    var dataFloat: array of real;
    var valid: array of boolean;
    var isInt: boolean;

    GetNumericColumn(source, ci, dataInt, dataFloat, valid, isInt);

    colsInt.Add(dataInt);
    colsFloat.Add(dataFloat);
    colsValid.Add(valid);
    colsIsInt[c] := isInt;
  end;

  // ----------------------------
  // 3. Ключи групп — вычисляем один раз
  // ----------------------------
  var keys1: array of object := nil;
  var keysN: array of GroupKey := nil;

  if singleKey then
    keys1 := groups1.Keys.ToArray
  else
    keysN := groupsN.Keys.ToArray;

  // ----------------------------
  // 4. Аллокации только под нужные агрегаты
  // ----------------------------
  var counts := new List<array of integer>;
  var sums := new List<array of real>;
  var sumsq := new List<array of real>;
  var mins := new List<array of real>;
  var maxs := new List<array of real>;

  for var c := 0 to m - 1 do
  begin
    if needCount then counts.Add(new integer[n]) else counts.Add(nil);
    if needSum then sums.Add(new real[n]) else sums.Add(nil);
    if needSumSq then sumsq.Add(new real[n]) else sumsq.Add(nil);
    if needMin then
    begin
      var arr := new real[n];
      for var g := 0 to n - 1 do
        arr[g] := real.MaxValue;
      mins.Add(arr);
    end
    else mins.Add(nil);

    if needMax then
    begin
      var arr := new real[n];
      for var g := 0 to n - 1 do
        arr[g] := real.MinValue;
      maxs.Add(arr);
    end
    else maxs.Add(nil);
  end;

  // ----------------------------
  // 5. Один проход по группам
  // ----------------------------
  if singleKey then
  begin
    for var g := 0 to n - 1 do
    begin
      var idxs := groups1[keys1[g]];

      for var j := 0 to idxs.Count - 1 do
      begin
        var row := idxs[j];

        for var c := 0 to m - 1 do
        begin
          var validArr := colsValid[c];
          if (validArr <> nil) and not validArr[row] then
            continue;

          var v := if colsIsInt[c] then colsInt[c][row] else colsFloat[c][row];

          if needCount then counts[c][g] += 1;
          if needSum then sums[c][g] += v;
          if needSumSq then sumsq[c][g] += v * v;

          if needMin and (v < mins[c][g]) then mins[c][g] := v;
          if needMax and (v > maxs[c][g]) then maxs[c][g] := v;
        end;
      end;
    end;
  end
  else
  begin
    for var g := 0 to n - 1 do
    begin
      var idxs := groupsN[keysN[g]];

      for var j := 0 to idxs.Count - 1 do
      begin
        var row := idxs[j];

        for var c := 0 to m - 1 do
        begin
          var validArr := colsValid[c];
          if (validArr <> nil) and not validArr[row] then
            continue;

          var v := if colsIsInt[c] then colsInt[c][row] else colsFloat[c][row];

          if needCount then counts[c][g] += 1;
          if needSum then sums[c][g] += v;
          if needSumSq then sumsq[c][g] += v * v;

          if needMin and (v < mins[c][g]) then mins[c][g] := v;
          if needMax and (v > maxs[c][g]) then maxs[c][g] := v;
        end;
      end;
    end;
  end;

  // ----------------------------
  // 6. Пустые группы: min/max делаем 0.0
  // ----------------------------
  if needMin or needMax then
    for var c := 0 to m - 1 do
      for var g := 0 to n - 1 do
        if needCount and (counts[c][g] = 0) then
        begin
          if needMin then mins[c][g] := 0.0;
          if needMax then maxs[c][g] := 0.0;
        end;

  // ----------------------------
  // 7. Формируем результат: сначала ключи
  // ----------------------------
  var res := new DataFrame;

  var names := new List<string>;
  var types := new List<ColumnType>;
  var cats := new List<boolean>;

  if singleKey then
  begin
    var col := source.columns[keyColumn];
    var keyName := col.Info.Name;

    if col.Info.ColType = ctInt then
      res.AddIntColumn(keyName, keys1.Select(k -> integer(k)).ToArray, nil)
    else
      res.AddStrColumn(keyName, keys1.Select(k -> string(k)).ToArray, nil);

    names.Add(keyName);
    types.Add(col.Info.ColType);
    cats.Add(true);
  end
  else
  begin
    for var k := 0 to keyColumns.Length - 1 do
    begin
      var ci := keyColumns[k];
      var col := source.columns[ci];
      var keyName := col.Info.Name;

      if col.Info.ColType = ctInt then
        res.AddIntColumn(keyName, keysN.Select(key -> integer(key.Values[k])).ToArray, nil)
      else
        res.AddStrColumn(keyName, keysN.Select(key -> string(key.Values[k])).ToArray, nil);

      names.Add(keyName);
      types.Add(col.Info.ColType);
      cats.Add(true);
    end;
  end;

  // ----------------------------
  // 8. Добавляем агрегаты
  // Порядок: по колонкам, внутри — в порядке kinds
  // ----------------------------
  var usedNames := new HashSet<string>;

  // сначала добавляем ключи (они уже в names)
  foreach var nme in names do
    usedNames.Add(nme);
  
  for var c := 0 to m - 1 do
  begin
    var baseName := colNames[c];
  
    foreach var kind in kinds do
    begin
      var colNameAgg := '';
  
      case kind of
        akCount: colNameAgg := baseName + '_count';
        akSum:   colNameAgg := baseName + '_sum';
        akMean:  colNameAgg := baseName + '_mean';
        akStd:   colNameAgg := baseName + '_std';
        akMin:   colNameAgg := baseName + '_min';
        akMax:   colNameAgg := baseName + '_max';
      end;
  
      // проверка уникальности
      if usedNames.Contains(colNameAgg) then
        ArgumentError(ER_AGG_COLUMN_DUPLICATE, colNameAgg);
  
      usedNames.Add(colNameAgg);
  
      // добавление данных
      case kind of
        akCount:
          res.AddIntColumn(colNameAgg, counts[c], nil);
  
        akSum:
          res.AddFloatColumn(colNameAgg, sums[c], nil);
  
        akMean:
        begin
          var arr := new real[n];
          for var g := 0 to n - 1 do
            arr[g] := if counts[c][g] = 0 then 0.0 else sums[c][g] / counts[c][g];
          res.AddFloatColumn(colNameAgg, arr, nil);
        end;
  
        akStd:
        begin
          var arr := new real[n];
          for var g := 0 to n - 1 do
            if counts[c][g] <= 1 then
              arr[g] := 0.0
            else
              arr[g] := Sqrt((sumsq[c][g] - sums[c][g] * sums[c][g] / counts[c][g]) / (counts[c][g] - 1));
          res.AddFloatColumn(colNameAgg, arr, nil);
        end;
  
        akMin:
          res.AddFloatColumn(colNameAgg, mins[c], nil);
  
        akMax:
          res.AddFloatColumn(colNameAgg, maxs[c], nil);
      end;
  
      // метаданные
      names.Add(colNameAgg);
  
      if kind = akCount then
        types.Add(ctInt)
      else
        types.Add(ctFloat);
  
      cats.Add(false);
    end;
  end;

  // ----------------------------
  // 9. Schema
  // ----------------------------
  res.SetSchema(new DataFrameSchema(
    names.ToArray,
    types.ToArray,
    cats.ToArray
  ));

  Result := res;
end;

function GroupByContext.Aggregate(map: Dictionary<string, array of AggregationKind>): DataFrame;
begin
  if (map = nil) or (map.Count = 0) then
    ArgumentError(ER_AGGREGATIONS_EMPTY);

  var first := true;
  var res: DataFrame := nil;

  foreach var kvp in map do
  begin
    var col := kvp.Key;
    var kinds := kvp.Value;

    var df := Aggregate([col], kinds);

    if first then
    begin
      res := df;
      first := false;
    end
    else
    begin
      // добавляем только агрегатные колонки (пропускаем ключи)
      for var ci := 0 to df.ColumnCount - 1 do
      begin
        var name := df.columns[ci].Info.Name;

        // ключи пропускаем (они уже есть в res)
        var exists := false;
        
        for var k := 0 to res.ColumnCount - 1 do
          if res.columns[k].Info.Name = name then
          begin
            exists := true;
            break;
          end;
        
        if not exists then
        begin
          var col1 := df.columns[ci];
        
          case col1.Info.ColType of
            ctInt:
              res.AddIntColumn(name, IntColumn(col1).Data, IntColumn(col1).IsValid);
            ctFloat:
              res.AddFloatColumn(name, FloatColumn(col1).Data, FloatColumn(col1).IsValid);
            ctStr:
              res.AddStrColumn(name, StrColumn(col1).Data, StrColumn(col1).IsValid);
            ctBool:
              res.AddBoolColumn(name, BoolColumn(col1).Data, BoolColumn(col1).IsValid);
            else
              Error(ER_UNSUPPORTED_COLUMN_TYPE, col1.Info.ColType);
          end;
        end;
      end;
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

  // числовые столбцы
  for var i := 0 to df.ColumnCount - 1 do
    if df.GetColumnType(i) in [ColumnType.ctInt, ColumnType.ctFloat] then
      names.Add(df.fSchema.ColumnNames[i]);

  var n := names.Count;
  if n = 0 then
    Error(ER_NO_NUMERIC_COLUMNS);

  var res := new DataFrame;

  var schemaNames := new List<string>;
  var schemaTypes := new List<ColumnType>;
  var schemaCats := new List<boolean>;

  // 1️⃣ первый столбец — Feature
  res.AddStrColumn('Feature', names.ToArray, nil);

  schemaNames.Add('Feature');
  schemaTypes.Add(ctStr);
  schemaCats.Add(true); // 🔥 categorical

  // 2️⃣ корреляции
  for var j := 0 to n - 1 do
  begin
    var data := new real[n];

    for var i := 0 to n - 1 do
      if i = j then
        data[i] := 1.0
      else
        data[i] := Correlation(df, names[i], names[j]);

    res.AddFloatColumn(names[j], data, nil);

    schemaNames.Add(names[j]);
    schemaTypes.Add(ctFloat);
    schemaCats.Add(false);
  end;

  // 🔥 schema
  res.SetSchema(new DataFrameSchema(
    schemaNames.ToArray,
    schemaTypes.ToArray,
    schemaCats.ToArray
  ));

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
        Error(ER_ZERO_STD_COLUMN, df.fSchema.ColumnNames[i]);
      isNumeric[i] := true;
    end;
  end;

  // 2. создаём схему результата
  for var i := 0 to df.ColumnCount - 1 do
  begin
    if isNumeric[i] then
      res.AddFloatColumn(df.fSchema.ColumnNames[i], new real[df.RowCount], nil)
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
        Error(ER_ZERO_RANGE_COLUMN, df.fSchema.ColumnNames[i]);
      mins[i] := mn;
      maxs[i] := mx;
      isNumeric[i] := true;
    end;
  end;

  // 2. создаём схему результата
  for var i := 0 to df.ColumnCount - 1 do
  begin
    if isNumeric[i] then
      res.AddFloatColumn(df.fSchema.ColumnNames[i], new real[df.RowCount], nil)
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


static function CSVLoader.LoadFromLines(
  lines: sequence of string; 
  delimiter: char; 
  hasHeader: boolean; 
  missingValues: array of string; 
  trimWhitespace: boolean; 
  strict: boolean; 
  schema: Dictionary<string, ColumnType>; 
  sampleSize: integer;
  ignoreColumns: array of string;
  inferTypes: boolean;
  forceStringColumns: array of string;
  categoricalColumns: array of string;
  inferCategorical: boolean;
  maxCategoricalCardinality: integer;
  maxCategoricalRatio: real;
  idThreshold: real;
  minFrequency: integer
): DataFrame;
begin
  var missing := if missingValues = nil then
    new HashSet<string>(Arr('', 'NA', 'NaN', 'null'))
  else
    new HashSet<string>(missingValues);

  var raw := lines.ToArray;
  
  var l := 0;
  while (l < raw.Length) and (raw[l].Trim = '') do
    l += 1;
  
  var r := raw.Length;
  while (r > l) and (raw[r-1].Trim = '') do
    r -= 1;
  
  var linesArray := raw[l:r];
  
  var headers: array of string := nil;
  var originalColCount := 0;
  var rowCount := linesArray.Count;
  if hasHeader then
    rowCount -= 1;
  
  var canBool, canInt, canFloat: array of boolean;
  (canBool, canInt, canFloat) := (nil, nil, nil);
  
  var inferLimit := sampleSize;
  if inferLimit <= 0 then
    inferLimit := integer.MaxValue;

  var ignoreSet := if ignoreColumns = nil then nil else new HashSet<string>(ignoreColumns);
  var forceStrSet := if forceStringColumns = nil then nil else new HashSet<string>(forceStringColumns);
  var catSet := if categoricalColumns = nil then nil else new HashSet<string>(categoricalColumns);

  var map: array of integer := nil;
  var newColCount := 0;
  
  var uniqueSet: array of HashSet<string> := nil;
  var freqMap: array of Dictionary<string, integer> := nil;
  var nonMissingCount: array of integer := nil;
  var autoCat: array of boolean := nil;

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
        originalColCount := headers.Length;
      end
      else
      begin
        originalColCount := parts.Length;
        headers := ArrGen(originalColCount, i -> 'C' + i.ToString);
        first := false;
        continue;
      end;
  
      map := new integer[originalColCount];
      var idx := 0;
      for var j := 0 to originalColCount - 1 do
        if (ignoreSet <> nil) and (headers[j] in ignoreSet) then
          map[j] := -1
        else
        begin
          map[j] := idx;
          idx += 1;
        end;
      
      newColCount := idx;
      
      var newHeaders := new string[newColCount];
      for var j := 0 to originalColCount - 1 do
        if map[j] <> -1 then
          newHeaders[map[j]] := headers[j];
      
      headers := newHeaders;
      
      canBool  := new boolean[newColCount];
      canInt   := new boolean[newColCount];
      canFloat := new boolean[newColCount];
      
      for var j := 0 to newColCount - 1 do
      begin
        canBool[j] := inferTypes;
        canInt[j] := inferTypes;
        canFloat[j] := inferTypes;
      end;
      
      uniqueSet := new HashSet<string>[newColCount];
      freqMap := new Dictionary<string, integer>[newColCount];
      nonMissingCount := new integer[newColCount];
      autoCat := new boolean[newColCount];
      
      for var j := 0 to newColCount - 1 do
      begin
        uniqueSet[j] := new HashSet<string>;
        freqMap[j] := new Dictionary<string, integer>;
        nonMissingCount[j] := 0;
        autoCat[j] := false;
      end;
  
      for var j := 0 to newColCount - 1 do
      begin
        var name := headers[j];
        
        if (schema <> nil) and schema.ContainsKey(name) then
        begin
          canBool[j] := false;
          canInt[j] := false;
          canFloat[j] := false;
          
          case schema[name] of
            ctBool:  canBool[j] := true;
            ctInt:   canInt[j] := true;
            ctFloat: canFloat[j] := true;
            ctStr:   ;
          end;
          
          continue;
        end;
        
        if (forceStrSet <> nil) and (name in forceStrSet) then
        begin
          canBool[j] := false;
          canInt[j] := false;
          canFloat[j] := false;
          continue;
        end;
        
        if (catSet <> nil) and (name in catSet) then
        begin
          canBool[j] := false;
          canInt[j] := false;
          canFloat[j] := false;
          continue;
        end;
      end;
  
      first := false;
      if hasHeader then continue;
    end;
  
    var parts := line.Split(delimiter);
  
    if parts.Length <> originalColCount then
      if strict then
        Error(ER_CSV_COLUMN_COUNT_MISMATCH, originalColCount, parts.Length);
  
    for var j := 0 to originalColCount - 1 do
    begin
      var nj := map[j];
      if nj = -1 then continue;
      
      var s := if j < parts.Length then parts[j] else '';
      if trimWhitespace then
        s := s.Trim;
      if s in missing then continue;
      
      nonMissingCount[nj] += 1;
  
      var name := headers[nj];
      
      if inferCategorical then
      begin
        if uniqueSet[nj].Count <= maxCategoricalCardinality * 2 then
        begin
          uniqueSet[nj].Add(s);
          if freqMap[nj].ContainsKey(s) then
            freqMap[nj][s] += 1
          else
            freqMap[nj][s] := 1;
        end;
      end;
      
      if not inferTypes then continue;
      
      if (schema <> nil) and schema.ContainsKey(name) then
        continue;
      
      if (forceStrSet <> nil) and (name in forceStrSet) then
        continue;
      
      if (catSet <> nil) and (name in catSet) then
        continue;
  
      var sl := s;
  
      if not ((sl = 'true') or (sl = 'false') or (sl = 'True') or (sl = 'False') or
              (sl = 'yes') or (sl = 'no') or (sl = 'Yes') or (sl = 'No')) then
        canBool[nj] := false;
  
      var iv: integer;
      if not TryStrToInt(s, iv) then
        canInt[nj] := false;
  
      var fv: real;
      if not TryStrToReal(s, fv) then
        canFloat[nj] := false;
    end;
  end;
  
  if headers = nil then
    Error(ER_EMPTY_CSV);
  
  if inferCategorical then
    for var j := 0 to newColCount - 1 do
    begin
      var name := headers[j];
      
      if (schema <> nil) and schema.ContainsKey(name) then continue;
      if (forceStrSet <> nil) and (name in forceStrSet) then continue;
      if (catSet <> nil) and (name in catSet) then continue;
      
      if canBool[j] or canInt[j] or canFloat[j] then continue;
      
      var uc := uniqueSet[j].Count;
      if uc = 0 then continue;
      
      var n := nonMissingCount[j];
      if n = 0 then continue;
      
      var ratio := uc / n;
      
      var maxFreq := 0;
      foreach var kv in freqMap[j] do
        if kv.Value > maxFreq then
          maxFreq := kv.Value;
      
      var isLowCardinality := uc <= maxCategoricalCardinality;
      var isReasonableRatio := ratio <= maxCategoricalRatio;
      
      if isLowCardinality and
         (isReasonableRatio or (uc <= 100)) and
         (ratio < idThreshold) and
         (maxFreq >= minFrequency) then
        autoCat[j] := true;
  end;

  var df := new DataFrame;
  
  var intData := new IntArray[newColCount];
  var floatData := new RealArray[newColCount];
  var strData := new StringArray[newColCount];
  var boolData := new BoolArray[newColCount];
  var valid := new BoolArray[newColCount];
  
  for var j := 0 to newColCount - 1 do
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
  
  var starts := new integer[originalColCount];
  var lens := new integer[originalColCount];
  
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
      
      for var j := 0 to newColCount - 1 do
        valid[j][row] := false;
      
      row += 1;
      continue;
    end;
    
    if (actualCount <> originalColCount) and strict then
      Error(ER_CSV_COLUMN_COUNT_MISMATCH, originalColCount, actualCount);
    
    for var j := 0 to originalColCount - 1 do
    begin
      var nj := map[j];
      if nj = -1 then continue;
      
      if missingValues = nil then
      begin
        if IsMissingRange(line, starts[j], lens[j]) then
        begin
          valid[nj][row] := false;
          continue;
        end;
      end
      else
      begin
        var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
        if trimWhitespace then
          s := s.Trim;
        if s in missing then
        begin
          valid[nj][row] := false;
          continue;
        end;
      end;
      
      if canBool[nj] then
      begin
        var bv: boolean;
        if TryStrToBoolStrictRange(line, starts[j], lens[j], bv) then
        begin
          boolData[nj][row] := bv;
          valid[nj][row] := true;
        end
        else
        begin
          if strict then
          begin
            var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
            if trimWhitespace then
              s := s.Trim;
            Error(ER_CSV_INVALID_BOOL, s, headers[nj]);
          end;
          valid[nj][row] := false;
        end;
      end
      else if canInt[nj] then
      begin
        var iv: integer;
        if TryStrToInt(line, starts[j], lens[j], iv) then
        begin
          intData[nj][row] := iv;
          valid[nj][row] := true;
        end
        else
        begin
          if strict then
          begin
            var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
            if trimWhitespace then
              s := s.Trim;
            raise new Exception($'Invalid int "{s}" in column {headers[nj]}');
          end;
          valid[nj][row] := false;
        end;
      end
      else if canFloat[nj] then
      begin
        var fv: real;
        if TryStrToReal(line, starts[j], lens[j], fv) then
        begin
          floatData[nj][row] := fv;
          valid[nj][row] := true;
        end
        else
        begin
          if strict then
          begin
            var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
            if trimWhitespace then
              s := s.Trim;
            raise new Exception($'Invalid float "{s}" in column {headers[nj]}');
          end;
          valid[nj][row] := false;
        end;
      end
      else
      begin
        var s := if lens[j] > 0 then line.Substring(starts[j]-1, lens[j]) else '';
        if trimWhitespace then
          s := s.Trim;
        strData[nj][row] := s;
        valid[nj][row] := true;
      end;
    end;
    
    row += 1;
  end;
  
  for var j := 0 to newColCount - 1 do
  begin
    if canBool[j] then
      df.AddBoolColumn(headers[j], boolData[j], valid[j])
    else if canInt[j] then
      df.AddIntColumn(headers[j], intData[j], valid[j])
    else if canFloat[j] then
      df.AddFloatColumn(headers[j], floatData[j], valid[j])
    else
      df.AddStrColumn(headers[j], strData[j], valid[j]);
  end;
  
  var names := new string[newColCount];
  var types := new ColumnType[newColCount];
  var cats := new boolean[newColCount];
  
  for var j := 0 to newColCount - 1 do
  begin
    names[j] := headers[j];
  
    if canBool[j] then
      types[j] := ctBool
    else if canInt[j] then
      types[j] := ctInt
    else if canFloat[j] then
      types[j] := ctFloat
    else
      types[j] := ctStr;
  
    // 🔥 вот сюда переносим логику categorical
    cats[j] := ((catSet <> nil) and (headers[j] in catSet)) or autoCat[j];
  end;
  
  df.SetSchema(new DataFrameSchema(names, types, cats));  
  
  Result := df;
end;

function DetectEncoding(filename: string): Encoding;
begin
  var fs := new System.IO.FileStream(
    filename,
    System.IO.FileMode.Open,
    System.IO.FileAccess.Read,
    System.IO.FileShare.ReadWrite
  );
  try
    var bom := new byte[3];
    var n := fs.Read(bom, 0, 3);
    
    if (n >= 3) and (bom[0] = $EF) and (bom[1] = $BB) and (bom[2] = $BF) then
      Result := Encoding.UTF8
    else
      Result := Encoding.UTF8;
  finally
    fs.Close;
  end;
end;

static function CSVLoader.Load(filename: string; 
  delimiter: char; hasHeader: boolean;      
  encoding: Encoding;
  missingValues: array of string;
  trimWhitespace: boolean;
  strict: boolean;
  schema: Dictionary<string, ColumnType>;
  sampleSize: integer;
  ignoreColumns: array of string;
  inferTypes: boolean;
  forceStringColumns: array of string;
  categoricalColumns: array of string;
  inferCategorical: boolean;
  maxCategoricalCardinality: integer;
  maxCategoricalRatio: real;
  idThreshold: real;
  minFrequency: integer
): DataFrame;
begin
  var enc := encoding;
  
  //if enc = nil then
  //  enc := System.Text.Encoding.Utf8;
  
  if enc = nil then
    enc := DetectEncoding(filename);
  
  Result := LoadFromLines(
    ReadLines(filename, enc),
    delimiter,
    hasHeader,
    missingValues,
    trimWhitespace,
    strict,
    schema,
    sampleSize,
    ignoreColumns,
    inferTypes,
    forceStringColumns,
    categoricalColumns,
    inferCategorical,
    maxCategoricalCardinality,
    maxCategoricalRatio,
    idThreshold,
    minFrequency
  );
end;

static procedure CsvSaver.Save(df: DataFrame; filename: string;
  delimiter: char; header: boolean);
begin
  if df = nil then
    ArgumentNullError('df');

  var w := new System.IO.StreamWriter(filename);

  try
    var schema := df.Schema;
    var n := schema.ColumnCount;

    // header
    if header then
    begin
      for var i := 0 to n-1 do
      begin
        if i > 0 then w.Write(delimiter);
        w.Write(schema.NameAt(i));
      end;
      w.WriteLine;
    end;

    var cur := df.GetCursor;

    while cur.MoveNext do
    begin
      for var i := 0 to n-1 do
      begin
        if i > 0 then w.Write(delimiter);

        if not cur.IsValid(i) then
          continue;

        case schema.ColumnTypeAt(i) of
          ctInt:   w.Write(cur.Int(i));
          ctFloat: w.Write(cur.Float(i));
          ctStr:   w.Write(cur.Str(i));
          ctBool:  w.Write(cur.Bool(i));
        end;
      end;

      w.WriteLine;
    end;

  finally
    w.Close;
  end;
end;

end.