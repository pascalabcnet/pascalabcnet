unit DataFrameABCCore;

// =============================================================
// ИНВАРИАНТ ВАЛИДНОСТИ СТОЛБЦОВ
//
// Для всех столбцов:
//   • IsValid всегда инициализирован
//   • Length(IsValid) = Length(Data)
//   • nil не используется как специальное значение
//
// Пустой столбец:
//   • Data имеет длину 0
//   • IsValid = new boolean[0]
//
// Нарушение этого инварианта считается ошибкой.
// =============================================================

interface

type
  // Базовые типы массивов
  StringArray = array of string;
  IntArray = array of integer;
  RealArray = array of real;
  BoolArray = array of boolean;
  DateTimeArray = array of System.DateTime;
  
  ColumnType = (ctInt, ctFloat, ctStr, ctBool, ctDateTime);
  
  /// Неизменяемое описание структуры столбцов DataFrame
  DataFrameSchema = sealed class
  private
    fNames: array of string;
    fTypes: array of ColumnType;
    fCategoricalFlags: array of boolean;
    fIndexByName: Dictionary<string, integer>;
    
    class function BuildIndex(names: array of string): Dictionary<string, integer>;
    function GetColumnNames: array of string;
    function GetTypes: array of ColumnType;
    function GetCategoricalFlags: array of boolean;
  public
    /// Возвращает количество столбцов в схеме
    property ColumnCount: integer read fNames.Length;
    /// Возвращает имена столбцов
    property ColumnNames: array of string read GetColumnNames;
    /// Возвращает копию массива типов столбцов
    property Types: array of ColumnType read GetTypes;
    /// Возвращает копию массива categorical-флагов
    property CategoricalFlags: array of boolean read GetCategoricalFlags;

    /// Возвращает индекс столбца по имени
    function IndexOf(name: string): integer;
    /// Проверяет наличие столбца с указанным именем
    function HasColumn(name: string): boolean;

    /// Возвращает тип столбца по индексу
    function ColumnTypeAt(i: integer): ColumnType;
    /// Проверяет categorical-флаг столбца по индексу
    function IsCategoricalAt(i: integer): boolean;
    
    /// Возвращает тип столбца по имени
    function GetColumnType(name: string): ColumnType;
    /// Проверяет categorical-флаг столбца по имени
    function IsCategorical(name: string): boolean;
    
    /// Возвращает имя столбца по индексу
    function NameAt(i: integer): string;

    /// Создаёт схему по именам, типам и categorical-флагам
    constructor Create(names: array of string; types: array of ColumnType;
      isCategorical: array of boolean := nil);
      
    /// Печатает схему без перевода строки в конце
    procedure Print;
    
    /// Печатает схему и переводит строку
    procedure Println;

    { --- schema operations (immutable) --- }
    /// Возвращает схему, содержащую только указанные столбцы
    function Select(indices: array of integer): DataFrameSchema;
    /// Возвращает схему без указанных столбцов
    function Drop(indices: array of integer): DataFrameSchema;
    /// Возвращает схему с переименованным столбцом
    function Rename(oldName, newName: string): DataFrameSchema;
    /// Возвращает схему с изменённым categorical-флагом столбца
    function WithCategorical(name: string; value: boolean := True): DataFrameSchema;

    { --- join helpers --- }
    /// Объединяет две схемы по правилам Join.
    /// Для объединённых key-столбцов metadata берётся из левой схемы:
    /// имя, тип и categorical-флаг правого key в результат не переносятся.
    class function Merge(
      left, right: DataFrameSchema;
      leftKeys, rightKeys: array of integer;
      rightPrefix: string
    ): DataFrameSchema;

    { --- DEBUG ONLY ---
    Проверка внутренних инвариантов схемы.
    Использует Assert и выполняется только в debug-сборке.
    Не предназначена для обработки пользовательских ошибок. }
    procedure AssertConsistent;
  end;
  
  ColumnInfo = sealed class
  private
    fName: string;
    fColType: ColumnType;
  public
    property Name: string read fName;
    property ColType: ColumnType read fColType;
    constructor Create(name: string; colType: ColumnType);
    //IsCategorical - только в Schema!
  end;

  /// Типизированное значение ячейки DataFrame.
  /// Используется для доступа через row['Column'].
  ///
  /// Позволяет писать в фильтрах и вычислениях:
  ///   row['score'].IsValid
  ///   row['score'].Float
  ///   row['date'].DateTime
  ///
  /// Если значение отсутствует, IsValid = False,
  /// а попытка взять Int/Float/Str/Bool/DateTime
  /// приводит к исключению.
  DataValue = sealed class
  private
    fName: string;
    fColType: ColumnType;
    fIsValid: boolean;
    fInt: integer;
    fFloat: real;
    fStr: string;
    fBool: boolean;
    fDateTime: System.DateTime;

    class function IsNumericType(t: ColumnType): boolean;
    class function Compare(a, b: DataValue): integer;
    class function ArithmeticType(a, b: DataValue): ColumnType;
    class function MakeNumeric(name: string; value: real; asInt: boolean := False): DataValue;
  public
    /// Имя столбца, из которого получено значение
    property Name: string read fName;
    /// Тип столбца, из которого получено значение
    property ColType: ColumnType read fColType;
    /// Показывает, что в ячейке есть корректное значение, а не NA
    property IsValid: boolean read fIsValid;

    constructor Create(name: string; value: integer);
    constructor Create(name: string; value: real);
    constructor Create(name: string; value: string);
    constructor Create(name: string; value: boolean);
    constructor Create(name: string; value: System.DateTime);
    class function NA(name: string; colType: ColumnType): DataValue;

    /// Возвращает значение как integer
    function GetInt: integer;
    /// Возвращает значение как real
    function GetFloat: real;
    /// Возвращает значение как string
    function GetStr: string;
    /// Возвращает значение как boolean
    function GetBool: boolean;
    /// Возвращает значение как DateTime
    function GetDateTime: System.DateTime;

    /// Значение ячейки как integer
    property Int: integer read GetInt;
    /// Значение ячейки как real
    property Float: real read GetFloat;
    /// Значение ячейки как string
    property Str: string read GetStr;
    /// Значение ячейки как boolean
    property Bool: boolean read GetBool;
    /// Значение ячейки как DateTime
    property DateTime: System.DateTime read GetDateTime;

    function ToInt: integer := GetInt;
    function ToFloat: real := GetFloat;
    function ToStr: string := GetStr;
    function ToBool: boolean := GetBool;
    function ToDateTime: System.DateTime := GetDateTime;

    function ToString: string; override;

    static function operator implicit(v: integer): DataValue := new DataValue('', v);
    static function operator implicit(v: real): DataValue := new DataValue('', v);
    static function operator implicit(v: string): DataValue := new DataValue('', v);
    static function operator implicit(v: boolean): DataValue := new DataValue('', v);
    static function operator implicit(v: System.DateTime): DataValue := new DataValue('', v);

    {static function operator implicit(v: DataValue): integer;
    static function operator implicit(v: DataValue): real;
    static function operator implicit(v: DataValue): string;
    static function operator implicit(v: DataValue): boolean;
    static function operator implicit(v: DataValue): System.DateTime;}

    static function operator =(a, b: DataValue): boolean;
    static function operator <>(a, b: DataValue): boolean;
    static function operator <(a, b: DataValue): boolean;
    static function operator <=(a, b: DataValue): boolean;
    static function operator >(a, b: DataValue): boolean;
    static function operator >=(a, b: DataValue): boolean;

    static function operator +(a, b: DataValue): DataValue;
    static function operator -(a, b: DataValue): DataValue;
    static function operator *(a, b: DataValue): DataValue;
    static function operator *(a: DataValue; b: integer): real;
    static function operator *(a: DataValue; b: real): real;
    static function operator *(a: integer; b: DataValue): real;
    static function operator *(a: real; b: DataValue): real;
    static function operator /(a, b: DataValue): DataValue;
    static function operator /(a: DataValue; b: integer): real;
    static function operator /(a: DataValue; b: real): real;
    static function operator /(a: integer; b: DataValue): real;
    static function operator /(a: real; b: DataValue): real;
  end;
  
  DataFrameCursor = class;
  
  /// Базовый класс столбца.
  /// 
  /// Столбцы являются неизменяемыми (immutable) после создания.
  /// Массивы данных (Data, IsValid) не должны изменяться после передачи
  /// в DataFrame через Add*Column.
  /// 
  /// Любые операции (Filter, TakeRows, GroupBy и др.) создают новые столбцы,
  /// не модифицируя существующие
  ///!#
  Column = abstract class
    Info: ColumnInfo;
  public
    IsValid: array of boolean;  // Флаги валидности (может быть nil)
    /// Возвращает число валидных (non-NA) значений в столбце
    function Count: integer; virtual;
    /// Возвращает число пропусков в столбце
    function MissingCount: integer; virtual;
    /// Возвращает минимальное числовое значение столбца
    function Min: real; virtual;
    /// Возвращает максимальное числовое значение столбца
    function Max: real; virtual;
    /// Возвращает среднее числовое значение столбца
    function Mean: real; virtual;
    /// Возвращает медиану числовых значений столбца
    function Median: real; virtual;
    /// Возвращает выборочную дисперсию числовых значений столбца
    function Variance: real; virtual;
    /// Возвращает выборочное стандартное отклонение числовых значений столбца
    function Std: real; virtual;
    /// Возвращает уникальные непустые значения столбца в порядке первого появления
    function Unique: array of DataValue; virtual;
    /// Возвращает число различных непустых значений столбца
    function NUnique: integer; virtual;
    /// Пытается извлечь i-тое данное из столбца как числовое если это возможно
    function TryGetNumericValue(i: integer; var value: real): boolean; virtual; abstract;
    /// Возвращает количество строк в столбце
    function RowCount: integer; virtual; abstract;
  end;
  
  /// Столбец целых чисел
  IntColumn = class(Column)
    // Data и IsValid считаются immutable после создания
    Data: array of integer;     
  public
    constructor Create; begin end;
    constructor Create(name: string);
    constructor Create(name: string; values: array of integer; valid: array of boolean := nil);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
  end;
  
  /// Столбец вещественных чисел
  FloatColumn = class(Column)
    Data: array of real;        
  public  
    constructor Create; begin end;
    constructor Create(name: string);
    constructor Create(name: string; values: array of real;
      valid: array of boolean := nil);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
  end;

  /// Столбец строк
  StrColumn = class(Column)
    Data: array of string;      
  public
    constructor Create; begin end;
    constructor Create(name: string);
    constructor Create(name: string; values: array of string;
      valid: array of boolean := nil);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
  end;

  /// Столбец булевых значений
  BoolColumn = class(Column)
    Data: array of boolean;     
  public  
    constructor Create; begin end;
    constructor Create(name: string);
    constructor Create(name: string; values: array of boolean;
      valid: array of boolean := nil);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
  end;

  /// Столбец значений DateTime
  DateTimeColumn = class(Column)
    Data: array of System.DateTime;
  public
    constructor Create; begin end;
    constructor Create(name: string);
    constructor Create(name: string; values: array of System.DateTime;
      valid: array of boolean := nil);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
  end;
  
  // Accessor типы для курсора
  IntAccessor = function(pos: integer): integer;
  FloatAccessor = function(pos: integer): real;
  StrAccessor = function(pos: integer): string;
  BoolAccessor = function(pos: integer): boolean;
  DateTimeAccessor = function(pos: integer): System.DateTime;
  ValidAccessor = function(pos: integer): boolean;
  
  // Структуры для Join
  JoinKeyLayout = record
    ColIndices: array of integer;
    ColTypes: array of ColumnType;
  end;
  
  // Нет Floats - по ним нельзя Join!!!
  JoinKey = record
    Ints: array of integer;
    Strs: array of string;
    Bools: array of boolean;
    function Equals(oth: object): boolean; override;
    function GetHashCode: integer; override;
  end;

    /// Курсор для итерации по строкам DataFrame
  DataFrameCursor = class
  private
    pos: integer;
    rowCnt: integer;
    colCnt: integer;
    fSchema: DataFrameSchema;
    
    intAcc: array of IntAccessor;
    floatAcc: array of FloatAccessor;
    strAcc: array of StrAccessor;
    boolAcc: array of BoolAccessor;
    dtAcc: array of DateTimeAccessor;
    validAcc: array of ValidAccessor;
    function GetValue(i: integer): DataValue;
    function GetValue(name: string): DataValue;
  public
    /// Создает курсор для указанных столбцов
    constructor Create(cols: array of Column; schema: DataFrameSchema);
    /// Возвращает количество столбцов
    function ColumnCount: integer := colCnt;
    /// Возвращает количество строк
    function RowCount: integer := rowCnt;
    /// Переходит к следующей строке, возвращает true если успешно
    function MoveNext: boolean;
    /// Возвращает текущую позицию курсора
    function Position: integer;
    /// Возвращает целочисленное значение из столбца по индексу
    function Int(i: integer): integer;
    /// Возвращает вещественное значение из столбца по индексу
    function Float(i: integer): real;
    /// Возвращает строковое значение из столбца по индексу
    function Str(i: integer): string;
    /// Возвращает булево значение из столбца по индексу
    function Bool(i: integer): boolean;
    /// Возвращает значение DateTime из столбца по индексу
    function DateTime(i: integer): System.DateTime;
    /// Возвращает целочисленное значение из столбца по имени
    function Int(name: string): integer;
    /// Возвращает вещественное значение из столбца по имени
    function Float(name: string): real;
    /// Возвращает строковое значение из столбца по имени
    function Str(name: string): string;
    /// Возвращает булево значение из столбца по имени
    function Bool(name: string): boolean;
    /// Возвращает значение DateTime из столбца по имени
    function DateTime(name: string): System.DateTime;
    /// Возвращает типизированное значение ячейки по индексу.
    /// Пример: row.Value(0).IsValid
    function Value(i: integer): DataValue := GetValue(i);
    /// Возвращает типизированное значение ячейки по имени столбца.
    /// Это основной способ проверки и чтения значения:
    ///   row['score'].IsValid
    ///   row['score'].Float
    ///   row['name'].Str
    property Item[name: string]: DataValue read GetValue; default;
    /// Проверяет валидность значения в столбце по индексу
    function IsValid(i: integer): boolean;
    /// Проверяет валидность значения в столбце по имени
    function IsValid(name: string): boolean;
    /// Перемещает курсор на указанную позицию
    procedure MoveTo(p: integer);
  end;
  
  // Предикат для фильтрации
  CursorPredicate = function(d: DataFrameCursor): boolean;
  
  /// Статистическая информация о числовом столбце
  ///!#
  DescribeStats = record
    /// Количество валидных значений
    Count: integer;
    /// Среднее значение
    Mean: real;
    /// Стандартное отклонение
    Std: real;
    /// Минимальное значение
    Min: real;
    /// Максимальное значение
    Max: real;
    function ToString: string; override;
    function ToString(decimals: integer): string;
    procedure Print;
    procedure Print(decimals: integer);
    procedure Println;
    procedure Println(decimals: integer);
  end;

function MergedRightColumnName(leftSchema, rightSchema: DataFrameSchema; rightIndex: integer): string;
function ColumnNumericValues(col: Column): List<real>;
  
implementation

uses MLExceptions;

const
  DS_LABEL_MEAN =
    'Среднее!!Mean';
  DS_LABEL_MIN =
    'Мин!!Min';
  DS_LABEL_MAX =
    'Макс!!Max';

function UiLabel(s: string): string;
begin
  var p := s.IndexOf('!!');
  if p >= 0 then
    Result := s.Substring(0, p)
  else
    Result := s;
end;

function DescribeStats.ToString: string;
begin
  Result := ToString(2);
end;

function DescribeStats.ToString(decimals: integer): string;
begin
  if decimals < 0 then
    decimals := 0;

  var fmt := '0.' + new string('#', decimals);
  if decimals = 0 then
    fmt := '0';

  Result :=
    'Count=' + Count.ToString +
    ', ' + UiLabel(DS_LABEL_MEAN) + '=' + Mean.ToString(fmt) +
    ', Std=' + Std.ToString(fmt) +
    ', ' + UiLabel(DS_LABEL_MIN) + '=' + Min.ToString(fmt) +
    ', ' + UiLabel(DS_LABEL_MAX) + '=' + Max.ToString(fmt);
end;

procedure DescribeStats.Print;
begin
  PABCSystem.Print(ToString);
end;

procedure DescribeStats.Print(decimals: integer);
begin
  PABCSystem.Print(ToString(decimals));
end;

procedure DescribeStats.Println;
begin
  PABCSystem.Println(ToString);
end;

procedure DescribeStats.Println(decimals: integer);
begin
  PABCSystem.Println(ToString(decimals));
end;

const
  ER_COLUMN_NOT_INT =
    'Столбец не является Int!!Column is not Int';
  ER_COLUMN_NOT_FLOAT =
    'Столбец не является Float!!Column is not Float';
  ER_COLUMN_NOT_STR =
    'Столбец не является Str!!Column is not Str';
  ER_COLUMN_NOT_BOOL =
    'Столбец не является Bool!!Column is not Bool';
  ER_COLUMN_NOT_DATETIME =
    'Столбец не является DateTime!!Column is not DateTime';
  ER_VALUE_IS_NA =
    'Значение в столбце "{0}" равно NA!!Value in column "{0}" is NA';
  ER_DUPLICATE_COLUMN_NAME =
    'Повторяющееся имя столбца "{0}"!!Duplicate column name "{0}"';
  ER_NAMES_NULL =
    'names не может быть nil!!names is nil';
  ER_TYPES_NULL =
    'types не может быть nil!!types is nil';
  ER_NAMES_TYPES_LENGTH_MISMATCH =
    'Длины names и types не совпадают!!names and types length mismatch';
  ER_ISCATEGORICAL_LENGTH_MISMATCH =
    'Длина isCategorical не совпадает с names!!isCategorical length mismatch';
  ER_COLUMN_NOT_EXISTS =
    'Столбец "{0}" не существует!!Column "{0}" does not exist';
  ER_COLUMN_ALREADY_EXISTS =
    'Столбец "{0}" уже существует!!Column "{0}" already exists';
  ER_INDEX_OUT_OF_RANGE =
    'Индекс {0} вне диапазона [0..{1})!!Index {0} out of range [0..{1})';
  ER_INDICES_NULL =
    'indices не может быть nil!!indices is nil';
  ER_LEFT_SCHEMA_NULL =
    'Left schema не может быть nil!!Left schema cannot be nil';
  ER_RIGHT_SCHEMA_NULL =
    'Right schema не может быть nil!!Right schema cannot be nil';
  ER_JOIN_KEYS_LENGTH_MISMATCH =
    'Длины leftKeys и rightKeys не совпадают!!join keys length mismatch';
  ER_UNKNOWN_COLUMN_TYPE =
    'Неизвестный тип столбца!!Unknown column type';
  ER_ROW_INDEX_OUT_OF_RANGE =
    'Индекс строки {0} вне диапазона [0..{1})!!' +
    'Row index {0} out of range [0..{1})'; 
  ER_INVALID_ISVALID_LENGTH =
    'Длина IsValid должна совпадать с длиной Data!!IsValid length must match Data length';
  ER_DATAVALUE_NOT_INT =
    'Значение "{0}" не является Int!!Value "{0}" is not Int';
  ER_DATAVALUE_NOT_FLOAT =
    'Значение "{0}" не является Float!!Value "{0}" is not Float';
  ER_DATAVALUE_NOT_STR =
    'Значение "{0}" не является Str!!Value "{0}" is not Str';
  ER_DATAVALUE_NOT_BOOL =
    'Значение "{0}" не является Bool!!Value "{0}" is not Bool';
  ER_DATAVALUE_NOT_DATETIME =
    'Значение "{0}" не является DateTime!!Value "{0}" is not DateTime';
  ER_DATAVALUE_COMPARE_TYPES_MISMATCH =
    'Нельзя сравнивать значения типов {0} и {1}!!Cannot compare values of types {0} and {1}';
  ER_DATAVALUE_ARITHMETIC_TYPES_MISMATCH =
    'Нельзя выполнять арифметические операции для типов {0} и {1}!!Cannot apply arithmetic to types {0} and {1}';
  ER_DATAVALUE_DIVISION_BY_ZERO =
    'Деление на ноль!!Division by zero';
  ER_COLUMN_NUMERIC_REQUIRED =
    'Столбец "{0}" должен быть числовым!!Column "{0}" must be numeric';
  ER_COLUMN_NO_VALID_VALUES =
    'Столбец "{0}" не содержит валидных значений!!Column "{0}" has no valid values';
    
//-----------------------------
//      Сервисные функции
//-----------------------------

function NotInt(pos: integer): integer;
begin
  Result := 0;
  Error(ER_COLUMN_NOT_INT);
end;

function NotFloat(pos: integer): real;
begin
  Result := 0;
  Error(ER_COLUMN_NOT_FLOAT);
end;

function NotStr(pos: integer): string;
begin
  Result := nil;
  Error(ER_COLUMN_NOT_STR);
end;

function NotBool(pos: integer): boolean;
begin
  Result := False;
  Error(ER_COLUMN_NOT_BOOL);
end;

function NotDateTime(pos: integer): System.DateTime;
begin
  Result := default(System.DateTime);
  Error(ER_COLUMN_NOT_DATETIME);
end;

procedure EnsureNumericColumn(col: Column);
begin
  if (col = nil) or (col.Info = nil) then
    ArgumentNullError('col');

  if not (col.Info.ColType in [ctInt, ctFloat]) then
    Error(ER_COLUMN_NUMERIC_REQUIRED, col.Info.Name);
end;

function ColumnNumericValues(col: Column): List<real>;
begin
  EnsureNumericColumn(col);

  Result := new List<real>;
  for var i := 0 to col.RowCount - 1 do
  begin
    var value: real;
    if col.TryGetNumericValue(i, value) then
      Result.Add(value);
  end;
end;

function ColumnTypeName(t: ColumnType): string;
begin
  case t of
    ctInt: Result := 'Int';
    ctFloat: Result := 'Float';
    ctStr: Result := 'Str';
    ctBool: Result := 'Bool';
    ctDateTime: Result := 'DateTime';
  else
    Result := t.ToString;
  end;
end;

//-----------------------------
//       DataFrameSchema
//-----------------------------
class function DataFrameSchema.BuildIndex(names: array of string): Dictionary<string, integer>;
begin
  Result := new Dictionary<string, integer>;
  for var i := 0 to names.Length - 1 do
  begin
    if Result.ContainsKey(names[i]) then
      ArgumentError(ER_DUPLICATE_COLUMN_NAME, names[i]);
    Result.Add(names[i], i);
  end;
end;

function DataFrameSchema.GetColumnNames: array of string;
begin
  Result := Copy(fNames);
end;

function DataFrameSchema.GetTypes: array of ColumnType;
begin
  Result := Copy(fTypes);
end;

function DataFrameSchema.GetCategoricalFlags: array of boolean;
begin
  Result := Copy(fCategoricalFlags);
end;

constructor DataFrameSchema.Create(names: array of string; types: array of ColumnType;
  isCategorical: array of boolean);
begin
  if names = nil then 
    ArgumentNullError(ER_NAMES_NULL);
  if types = nil then 
    ArgumentNullError(ER_TYPES_NULL);
  if names.Length <> types.Length then
    ArgumentError(ER_NAMES_TYPES_LENGTH_MISMATCH);
  if (isCategorical <> nil) and (isCategorical.Length <> names.Length) then
    ArgumentError(ER_ISCATEGORICAL_LENGTH_MISMATCH);

  fNames := Copy(names);
  fTypes := Copy(types);

  fCategoricalFlags := 
    if isCategorical = nil then 
      new boolean[names.Length] 
    else 
      Copy(isCategorical);

  fIndexByName := BuildIndex(fNames);

  AssertConsistent;
end;

constructor ColumnInfo.Create(name: string; colType: ColumnType);
begin
  fName := name;
  fColType := colType;
end;

class function DataValue.IsNumericType(t: ColumnType): boolean;
begin
  Result := t in [ctInt, ctFloat];
end;

constructor DataValue.Create(name: string; value: integer);
begin
  fName := name;
  fColType := ctInt;
  fIsValid := true;
  fInt := value;
  fFloat := value;
end;

constructor DataValue.Create(name: string; value: real);
begin
  fName := name;
  fColType := ctFloat;
  fIsValid := true;
  fFloat := value;
end;

constructor DataValue.Create(name: string; value: string);
begin
  fName := name;
  fColType := ctStr;
  fIsValid := true;
  fStr := value;
end;

constructor DataValue.Create(name: string; value: boolean);
begin
  fName := name;
  fColType := ctBool;
  fIsValid := true;
  fBool := value;
end;

constructor DataValue.Create(name: string; value: System.DateTime);
begin
  fName := name;
  fColType := ctDateTime;
  fIsValid := true;
  fDateTime := value;
end;

class function DataValue.NA(name: string; colType: ColumnType): DataValue;
begin
  Result := new DataValue(name, 0);
  Result.fColType := colType;
  Result.fIsValid := false;
end;

function DataValue.GetInt: integer;
begin
  if not fIsValid then
    Error(ER_VALUE_IS_NA, fName);
  if fColType <> ctInt then
    Error(ER_DATAVALUE_NOT_INT, fName);
  Result := fInt;
end;

function DataValue.GetFloat: real;
begin
  if not fIsValid then
    Error(ER_VALUE_IS_NA, fName);
  if not IsNumericType(fColType) then
    Error(ER_DATAVALUE_NOT_FLOAT, fName);
  if fColType = ctInt then
    Result := fInt
  else
    Result := fFloat;
end;

function DataValue.GetStr: string;
begin
  if not fIsValid then
    Error(ER_VALUE_IS_NA, fName);
  if fColType <> ctStr then
    Error(ER_DATAVALUE_NOT_STR, fName);
  Result := fStr;
end;

function DataValue.GetBool: boolean;
begin
  if not fIsValid then
    Error(ER_VALUE_IS_NA, fName);
  if fColType <> ctBool then
    Error(ER_DATAVALUE_NOT_BOOL, fName);
  Result := fBool;
end;

function DataValue.GetDateTime: System.DateTime;
begin
  if not fIsValid then
    Error(ER_VALUE_IS_NA, fName);
  if fColType <> ctDateTime then
    Error(ER_DATAVALUE_NOT_DATETIME, fName);
  Result := fDateTime;
end;

function DataValue.ToString: string;
begin
  if not fIsValid then
    exit('NA');

  case fColType of
    ctInt: Result := fInt.ToString;
    ctFloat: Result := fFloat.ToString;
    ctStr: Result := fStr;
    ctBool: Result := fBool.ToString;
    ctDateTime: Result := fDateTime.ToString('yyyy-MM-dd HH:mm:ss');
  else
    Result := '';
  end;
end;

class function DataValue.Compare(a, b: DataValue): integer;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);

  if IsNumericType(a.fColType) and IsNumericType(b.fColType) then
  begin
    var av := a.Float;
    var bv := b.Float;
    if av < bv then
      Result := -1
    else if av > bv then
      Result := 1
    else
      Result := 0;
    exit;
  end;

  if a.fColType <> b.fColType then
    Error(ER_DATAVALUE_COMPARE_TYPES_MISMATCH, ColumnTypeName(a.fColType), ColumnTypeName(b.fColType));

  case a.fColType of
    ctStr:
      if a.fStr < b.fStr then Result := -1 else if a.fStr > b.fStr then Result := 1 else Result := 0;
    ctBool:
      if integer(a.fBool) < integer(b.fBool) then Result := -1 else if integer(a.fBool) > integer(b.fBool) then Result := 1 else Result := 0;
    ctDateTime:
      if a.fDateTime < b.fDateTime then Result := -1 else if a.fDateTime > b.fDateTime then Result := 1 else Result := 0;
  else
    Result := 0;
  end;
end;

class function DataValue.ArithmeticType(a, b: DataValue): ColumnType;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);

  if not (IsNumericType(a.fColType) and IsNumericType(b.fColType)) then
    Error(ER_DATAVALUE_ARITHMETIC_TYPES_MISMATCH, ColumnTypeName(a.fColType), ColumnTypeName(b.fColType));

  if (a.fColType = ctInt) and (b.fColType = ctInt) then
    Result := ctInt
  else
    Result := ctFloat;
end;

class function DataValue.MakeNumeric(name: string; value: real; asInt: boolean): DataValue;
begin
  if asInt then
    Result := new DataValue(name, integer(value))
  else
    Result := new DataValue(name, value);
end;

{static function DataValue.operator implicit(v: DataValue): integer;
begin
  if System.Object.ReferenceEquals(v, nil) then
    ArgumentNullError('v');
  Result := v.Int;
end;

static function DataValue.operator implicit(v: DataValue): string;
begin
  if System.Object.ReferenceEquals(v, nil) then
    ArgumentNullError('v');
  Result := v.Str;
end;

static function DataValue.operator implicit(v: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(v, nil) then
    ArgumentNullError('v');
  Result := v.Bool;
end;

static function DataValue.operator implicit(v: DataValue): System.DateTime;
begin
  if System.Object.ReferenceEquals(v, nil) then
    ArgumentNullError('v');
  Result := v.DateTime;
end;}

{static function DataValue.operator implicit(v: DataValue): real;
begin
  if System.Object.ReferenceEquals(v, nil) then
    ArgumentNullError('v');
  Result := v.Float;
end;}

static function DataValue.operator =(a, b: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(a, nil) then
    exit(System.Object.ReferenceEquals(b, nil));
  if System.Object.ReferenceEquals(b, nil) then
    exit(false);
  if not a.fIsValid then
    exit(false);
  if not b.fIsValid then
    exit(false);
  Result := Compare(a, b) = 0;
end;

static function DataValue.operator <>(a, b: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(a, nil) then
    exit(false);
  if System.Object.ReferenceEquals(b, nil) then
    exit(false);
  if not a.fIsValid then
    exit(false);
  if not b.fIsValid then
    exit(false);
  Result := Compare(a, b) <> 0;
end;

static function DataValue.operator <(a, b: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(a, nil) then
    exit(false);
  if System.Object.ReferenceEquals(b, nil) then
    exit(false);
  if not a.fIsValid then
    exit(false);
  if not b.fIsValid then
    exit(false);
  Result := Compare(a, b) < 0;
end;

static function DataValue.operator <=(a, b: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(a, nil) then
    exit(false);
  if System.Object.ReferenceEquals(b, nil) then
    exit(false);
  if not a.fIsValid then
    exit(false);
  if not b.fIsValid then
    exit(false);
  Result := Compare(a, b) <= 0;
end;

static function DataValue.operator >(a, b: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(a, nil) then
    exit(false);
  if System.Object.ReferenceEquals(b, nil) then
    exit(false);
  if not a.fIsValid then
    exit(false);
  if not b.fIsValid then
    exit(false);
  Result := Compare(a, b) > 0;
end;

static function DataValue.operator >=(a, b: DataValue): boolean;
begin
  if System.Object.ReferenceEquals(a, nil) then
    exit(false);
  if System.Object.ReferenceEquals(b, nil) then
    exit(false);
  if not a.fIsValid then
    exit(false);
  if not b.fIsValid then
    exit(false);
  Result := Compare(a, b) >= 0;
end;

static function DataValue.operator +(a, b: DataValue): DataValue;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);

  if (a.fColType = ctStr) and (b.fColType = ctStr) then
  begin
    Result := new DataValue(a.Name, a.fStr + b.fStr);
    exit;
  end;

  var t := ArithmeticType(a, b);
  Result := MakeNumeric(a.Name, a.Float + b.Float, t = ctInt);
end;

static function DataValue.operator -(a, b: DataValue): DataValue;
begin
  var t := ArithmeticType(a, b);
  Result := MakeNumeric(a.Name, a.Float - b.Float, t = ctInt);
end;

static function DataValue.operator *(a, b: DataValue): DataValue;
begin
  var t := ArithmeticType(a, b);
  Result := MakeNumeric(a.Name, a.Float * b.Float, t = ctInt);
end;

static function DataValue.operator *(a: DataValue; b: integer): real;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  Result := a.Float * b;
end;

static function DataValue.operator *(a: DataValue; b: real): real;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  Result := a.Float * b;
end;

static function DataValue.operator *(a: integer; b: DataValue): real;
begin
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);
  Result := a * b.Float;
end;

static function DataValue.operator *(a: real; b: DataValue): real;
begin
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);
  Result := a * b.Float;
end;

static function DataValue.operator /(a, b: DataValue): DataValue;
begin
  ArithmeticType(a, b);
  if b.Float = 0 then
    Error(ER_DATAVALUE_DIVISION_BY_ZERO);
  Result := new DataValue(a.Name, a.Float / b.Float);
end;

static function DataValue.operator /(a: DataValue; b: integer): real;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  if b = 0 then
    Error(ER_DATAVALUE_DIVISION_BY_ZERO);
  Result := a.Float / b;
end;

static function DataValue.operator /(a: DataValue; b: real): real;
begin
  if System.Object.ReferenceEquals(a, nil) then
    ArgumentNullError('a');
  if not a.fIsValid then
    Error(ER_VALUE_IS_NA, a.fName);
  if b = 0 then
    Error(ER_DATAVALUE_DIVISION_BY_ZERO);
  Result := a.Float / b;
end;

static function DataValue.operator /(a: integer; b: DataValue): real;
begin
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);
  if b.Float = 0 then
    Error(ER_DATAVALUE_DIVISION_BY_ZERO);
  Result := a / b.Float;
end;

static function DataValue.operator /(a: real; b: DataValue): real;
begin
  if System.Object.ReferenceEquals(b, nil) then
    ArgumentNullError('b');
  if not b.fIsValid then
    Error(ER_VALUE_IS_NA, b.fName);
  if b.Float = 0 then
    Error(ER_DATAVALUE_DIVISION_BY_ZERO);
  Result := a / b.Float;
end;

procedure DataFrameSchema.Print;
begin
  if fNames.Length = 0 then
  begin
    PABCSystem.Println('Schema: <empty>');
    exit;
  end;
  
  var maxName := fNames.Max(n -> n.Length);

  for var i := 0 to ColumnCount-1 do
  begin
    var name := fNames[i].PadRight(maxName);

    var t: string;

    case fTypes[i] of
      ctInt:   t := 'int';
      ctFloat: t := 'float';
      ctStr:   t := 'string';
      ctBool:  t := 'bool';
      ctDateTime: t := 'datetime';
    end;

    if fCategoricalFlags[i] then
      PABCSystem.Println(name, ':', t, '(categorical)')
    else
      PABCSystem.Println(name, ':', t);
  end;
end;

procedure DataFrameSchema.Println;
begin
  Print;
  PABCSystem.Println
end;


function DataFrameSchema.IndexOf(name: string): integer;
begin
  if not fIndexByName.ContainsKey(name) then
    ArgumentError(ER_COLUMN_NOT_EXISTS, name);
  Result := fIndexByName[name];
end;

function DataFrameSchema.HasColumn(name: string): boolean :=
  fIndexByName.ContainsKey(name);

function DataFrameSchema.NameAt(i: integer): string;
begin
  if (i < 0) or (i >= ColumnCount) then
    ArgumentOutOfRangeError(ER_INDEX_OUT_OF_RANGE, i, ColumnCount);
  Result := fNames[i];
end;

function DataFrameSchema.ColumnTypeAt(i: integer): ColumnType;
begin
  if (i < 0) or (i >= ColumnCount) then
    ArgumentOutOfRangeError(ER_INDEX_OUT_OF_RANGE, i, ColumnCount);
  Result := fTypes[i];
end;

function DataFrameSchema.IsCategoricalAt(i: integer): boolean;
begin
  if (i < 0) or (i >= ColumnCount) then
    ArgumentOutOfRangeError(ER_INDEX_OUT_OF_RANGE, i, ColumnCount);
  if fCategoricalFlags = nil then
    Result := false
  else
    Result := fCategoricalFlags[i];
end;

function DataFrameSchema.GetColumnType(name: string): ColumnType;
begin
  Result := ColumnTypeAt(IndexOf(name));
end;

function DataFrameSchema.IsCategorical(name: string): boolean;
begin
  Result := IsCategoricalAt(IndexOf(name));
end;

function DataFrameSchema.Select(indices: array of integer): DataFrameSchema;
begin
  if indices = nil then 
    ArgumentNullError(ER_INDICES_NULL);

  var n := indices.Length;
  var names := new string[n];
  var types := new ColumnType[n];
  var cats := if fCategoricalFlags = nil then nil else new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var k := indices[i];
    if (k < 0) or (k >= ColumnCount) then
      ArgumentOutOfRangeError(ER_INDEX_OUT_OF_RANGE, k, ColumnCount);
    names[i] := fNames[k];
    types[i] := fTypes[k];
    if cats <> nil then cats[i] := fCategoricalFlags[k];
  end;

  Result := new DataFrameSchema(names, types, cats);
end;

function DataFrameSchema.Drop(indices: array of integer): DataFrameSchema;
begin
  if indices = nil then 
    ArgumentNullError(ER_INDICES_NULL);

  var drop := new boolean[ColumnCount];
  foreach var i in indices do
  begin
    if (i < 0) or (i >= ColumnCount) then
      ArgumentOutOfRangeError(ER_INDEX_OUT_OF_RANGE, i, ColumnCount);
    drop[i] := true;
  end;

  var keep := new List<integer>;
  for var i := 0 to ColumnCount - 1 do
    if not drop[i] then
      keep.Add(i);

  Result := Select(keep.ToArray);
end;

function DataFrameSchema.Rename(oldName, newName: string): DataFrameSchema;
begin
  if not HasColumn(oldName) then
    ArgumentError(ER_COLUMN_NOT_EXISTS, oldName);
  if HasColumn(newName) then
    ArgumentError(ER_COLUMN_ALREADY_EXISTS, newName);

  var names := Copy(fNames);
  names[IndexOf(oldName)] := newName;

  Result := new DataFrameSchema(names, fTypes, fCategoricalFlags);
end;

function DataFrameSchema.WithCategorical(name: string; value: boolean): DataFrameSchema;
begin
  if not HasColumn(name) then
    ArgumentError(ER_COLUMN_NOT_EXISTS, name);

  var cats := if fCategoricalFlags = nil then new boolean[ColumnCount] else Copy(fCategoricalFlags);
  cats[IndexOf(name)] := value;

  Result := new DataFrameSchema(fNames, fTypes, cats);
end;

class function DataFrameSchema.Merge(left, right: DataFrameSchema;
  leftKeys, rightKeys: array of integer; rightPrefix: string): DataFrameSchema;
begin
  // Важно: объединённые key-столбцы наследуют metadata слева.
  // Правые key-столбцы исключаются из итоговой схемы.
  if left = nil then 
    ArgumentNullError(ER_LEFT_SCHEMA_NULL);
  if right = nil then 
    ArgumentNullError(ER_RIGHT_SCHEMA_NULL);
  if leftKeys.Length <> rightKeys.Length then
    ArgumentError(ER_JOIN_KEYS_LENGTH_MISMATCH);

  var skip := new boolean[right.ColumnCount];
  foreach var i in rightKeys do
  begin
    if (i < 0) or (i >= right.ColumnCount) then
      ArgumentOutOfRangeError(ER_INDEX_OUT_OF_RANGE, i, right.ColumnCount);
    skip[i] := true;
  end;

  var names := new List<string>;
  var types := new List<ColumnType>;
  var cats := new List<boolean>;

  for var i := 0 to left.ColumnCount - 1 do
  begin
    names.Add(left.NameAt(i));
    types.Add(left.ColumnTypeAt(i));
    cats.Add(left.IsCategoricalAt(i));
  end;

  for var i := 0 to right.ColumnCount - 1 do
    if not skip[i] then
    begin
      var name := right.NameAt(i);
      if left.HasColumn(name) then name := rightPrefix + name;
      names.Add(name);
      types.Add(right.ColumnTypeAt(i));
      cats.Add(right.IsCategoricalAt(i));
    end;

  Result := new DataFrameSchema(names.ToArray, types.ToArray, cats.ToArray);
end;

function MergedRightColumnName(leftSchema, rightSchema: DataFrameSchema; rightIndex: integer): string;
begin
  Result := rightSchema.NameAt(rightIndex);
  if leftSchema.HasColumn(Result) then
    Result := 'right_' + Result;
end;

procedure DataFrameSchema.AssertConsistent;
begin
  Assert(fNames.Length = fTypes.Length);
  if fCategoricalFlags <> nil then Assert(fCategoricalFlags.Length = fNames.Length);
  Assert(fIndexByName.Count = fNames.Length);
end;

//-----------------------------
//           Columns
//-----------------------------

function Column.Count: integer;
begin
  Result := 0;
  for var i := 0 to RowCount - 1 do
    if IsValid[i] then
      Result += 1;
end;

function Column.MissingCount: integer;
begin
  Result := 0;
  for var i := 0 to RowCount - 1 do
    if not IsValid[i] then
      Result += 1;
end;

function Column.Min: real;
begin
  var values := ColumnNumericValues(Self);
  if values.Count = 0 then
    Error(ER_COLUMN_NO_VALID_VALUES, Info.Name);
  Result := values.Min;
end;

function Column.Max: real;
begin
  var values := ColumnNumericValues(Self);
  if values.Count = 0 then
    Error(ER_COLUMN_NO_VALID_VALUES, Info.Name);
  Result := values.Max;
end;

function Column.Mean: real;
begin
  var values := ColumnNumericValues(Self);
  if values.Count = 0 then
    exit(0.0);
  Result := values.Sum / values.Count;
end;

function Column.Median: real;
begin
  var values := ColumnNumericValues(Self);
  if values.Count = 0 then
    Error(ER_COLUMN_NO_VALID_VALUES, Info.Name);

  values.Sort;
  var n := values.Count;
  if n mod 2 = 1 then
    Result := values[n div 2]
  else
    Result := (values[n div 2 - 1] + values[n div 2]) / 2.0;
end;

function Column.Variance: real;
begin
  var values := ColumnNumericValues(Self);
  var cnt := values.Count;

  if cnt <= 1 then
    exit(0.0);

  var mean := values.Sum / cnt;
  var acc := 0.0;
  foreach var v in values do
  begin
    var d := v - mean;
    acc += d * d;
  end;

  Result := acc / (cnt - 1);
end;

function Column.Std: real;
begin
  Result := Sqrt(Variance);
end;

function Column.Unique: array of DataValue;
begin
  var values := new List<DataValue>;

  case Info.ColType of
    ctInt:
    begin
      var c := IntColumn(Self);
      var seen := new HashSet<integer>;
      for var i := 0 to c.Data.Length - 1 do
        if c.IsValid[i] and seen.Add(c.Data[i]) then
          values.Add(new DataValue(Info.Name, c.Data[i]));
    end;

    ctFloat:
    begin
      var c := FloatColumn(Self);
      var seen := new HashSet<real>;
      for var i := 0 to c.Data.Length - 1 do
        if c.IsValid[i] and seen.Add(c.Data[i]) then
          values.Add(new DataValue(Info.Name, c.Data[i]));
    end;

    ctStr:
    begin
      var c := StrColumn(Self);
      var seen := new HashSet<string>;
      for var i := 0 to c.Data.Length - 1 do
        if c.IsValid[i] and seen.Add(c.Data[i]) then
          values.Add(new DataValue(Info.Name, c.Data[i]));
    end;

    ctBool:
    begin
      var c := BoolColumn(Self);
      var seen := new HashSet<boolean>;
      for var i := 0 to c.Data.Length - 1 do
        if c.IsValid[i] and seen.Add(c.Data[i]) then
          values.Add(new DataValue(Info.Name, c.Data[i]));
    end;

    ctDateTime:
    begin
      var c := DateTimeColumn(Self);
      var seen := new HashSet<System.DateTime>;
      for var i := 0 to c.Data.Length - 1 do
        if c.IsValid[i] and seen.Add(c.Data[i]) then
          values.Add(new DataValue(Info.Name, c.Data[i]));
    end;

    else
      Error(ER_UNKNOWN_COLUMN_TYPE);
  end;

  Result := values.ToArray;
end;

function Column.NUnique: integer;
begin
  Result := Unique.Length;
end;

constructor IntColumn.Create(name: string; values: array of integer; valid: array of boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctInt);

  var n := Length(values);
  Self.Data := if n = 0 then [] else values;

  if valid = nil then
    IsValid := [True] * n
  else
  begin
    if Length(valid) <> n then
      Error(ER_INVALID_ISVALID_LENGTH);

    IsValid := valid;
  end;
end;

constructor IntColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctInt);

  Data := new integer[0];
  IsValid := new boolean[0];
end;

function IntColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  if not IsValid[i] then
    exit(False);

  value := Data[i];
  exit(True);
end;

constructor FloatColumn.Create(name: string; values: array of real; valid: array of boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctFloat);

  var n := Length(values);
  Self.Data := if n = 0 then [] else values;

  if valid = nil then
    IsValid := [True] * n
  else
  begin
    if Length(valid) <> n then
      Error(ER_INVALID_ISVALID_LENGTH);

    IsValid := valid;
  end;
end;

constructor FloatColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctFloat);

  Data := new real[0];
  IsValid := new boolean[0];
end;

function FloatColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  if not IsValid[i] then
    exit(False);

  value := Data[i];
  exit(True);
end;

constructor StrColumn.Create(name: string; values: array of string; valid: array of boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctStr);

  var n := Length(values);
  Self.Data := if n = 0 then [] else values;

  if valid = nil then
    IsValid := [True] * n
  else
  begin
    if Length(valid) <> n then
      Error(ER_INVALID_ISVALID_LENGTH);

    IsValid := valid;
  end;
end;

constructor StrColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctStr);

  Data := new string[0];
  IsValid := new boolean[0];
end;

function StrColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  exit(False);
end;

constructor BoolColumn.Create(name: string; values: array of boolean; valid: array of boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctBool);

  var n := Length(values);
  Self.Data := if n = 0 then [] else values;

  if valid = nil then
    IsValid := [True] * n
  else
  begin
    if Length(valid) <> n then
      Error(ER_INVALID_ISVALID_LENGTH);

    IsValid := valid;
  end;
end;

constructor BoolColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctBool);

  Data := new boolean[0];
  IsValid := new boolean[0];
end;

function BoolColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  if not IsValid[i] then
    exit(False);

  if Data[i] then
    value := 1.0
  else
    value := 0.0;

  exit(True);
end;

constructor DateTimeColumn.Create(name: string; values: array of System.DateTime; valid: array of boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctDateTime);

  var n := Length(values);
  Self.Data := if n = 0 then [] else values;

  if valid = nil then
    IsValid := [True] * n
  else
  begin
    if Length(valid) <> n then
      Error(ER_INVALID_ISVALID_LENGTH);

    IsValid := valid;
  end;
end;

constructor DateTimeColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctDateTime);

  Data := new System.DateTime[0];
  IsValid := new boolean[0];
end;

function DateTimeColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  exit(False);
end;



//-----------------------------
//           JoinKey
//-----------------------------

function JoinKey.Equals(oth: object): boolean;
begin
  if oth = nil then
    exit(false);

  if not (oth is JoinKey) then
    exit(false);

  var other := JoinKey(oth);

  if Ints.Length <> other.Ints.Length then exit(false);
  if Strs.Length <> other.Strs.Length then exit(false);
  if Bools.Length <> other.Bools.Length then exit(false);

  for var i := 0 to Ints.Length - 1 do
    if Ints[i] <> other.Ints[i] then exit(false);

  for var i := 0 to Strs.Length - 1 do
    if Strs[i] <> other.Strs[i] then exit(false);

  for var i := 0 to Bools.Length - 1 do
    if Bools[i] <> other.Bools[i] then exit(false);

  Result := true;
end;

function JoinKey.GetHashCode: integer;
begin
  var h := 17;

  foreach var v in Ints do
    h := h * 31 + v.GetHashCode;

  foreach var v in Strs do
    h := h * 31 + (if v = nil then 0 else v.GetHashCode);

  foreach var v in Bools do
    h := h * 31 + v.GetHashCode;

  Result := h;
end;

//-----------------------------
//       DataFrameCursor
//-----------------------------

constructor DataFrameCursor.Create(cols: array of Column; schema: DataFrameSchema);
begin
  pos := -1;
  self.fSchema := schema;

  if cols.Length = 0 then rowCnt := 0
  else
    case fSchema.ColumnTypeAt(0) of
      ctInt:   rowCnt := IntColumn(cols[0]).Data.Length;
      ctFloat: rowCnt := FloatColumn(cols[0]).Data.Length;
      ctStr:   rowCnt := StrColumn(cols[0]).Data.Length;
      ctBool:  rowCnt := BoolColumn(cols[0]).Data.Length;
      ctDateTime: rowCnt := DateTimeColumn(cols[0]).Data.Length;
    end;
    
  var n := cols.Length;
  colCnt := cols.Length;

  intAcc := new IntAccessor[n];
  floatAcc := new FloatAccessor[n];
  strAcc := new StrAccessor[n];
  boolAcc := new BoolAccessor[n];
  dtAcc := new DateTimeAccessor[n];
  validAcc := new ValidAccessor[n];

  for var i := 0 to n - 1 do
  begin
    var col := cols[i];

    intAcc[i] := NotInt;
    floatAcc[i] := NotFloat;
    strAcc[i] := NotStr;
    boolAcc[i] := NotBool;
    dtAcc[i] := NotDateTime;

    case fSchema.ColumnTypeAt(i) of
      ctInt:
      begin
        var c := IntColumn(col);
        validAcc[i] := pos -> c.IsValid[pos];
    
        intAcc[i] := pos -> c.Data[pos];
        floatAcc[i] := pos -> c.Data[pos];
      end;
    
      ctFloat:
      begin
        var c := FloatColumn(col);
        validAcc[i] := pos -> c.IsValid[pos];
    
        floatAcc[i] := pos -> c.Data[pos];
      end;
    
      ctStr:
      begin
        var c := StrColumn(col);
        validAcc[i] := pos -> c.IsValid[pos];
    
        strAcc[i] := pos -> c.Data[pos];
      end;
    
      ctBool:
      begin
        var c := BoolColumn(col);
        validAcc[i] := pos -> c.IsValid[pos];
    
        boolAcc[i] := pos -> c.Data[pos];
      end;

      ctDateTime:
      begin
        var c := DateTimeColumn(col);
        validAcc[i] := pos -> c.IsValid[pos];

        dtAcc[i] := pos -> c.Data[pos];
      end;
    
    else Error(ER_UNKNOWN_COLUMN_TYPE);
    end;
  end;
end;  

function DataFrameCursor.MoveNext: boolean;
begin
  pos += 1;
  Result := pos < rowCnt;
end;

function DataFrameCursor.Position: integer := pos;

function DataFrameCursor.IsValid(i: integer): boolean :=
  validAcc[i](pos);
  
function DataFrameCursor.IsValid(name: string): boolean;
begin
  Result := IsValid(fSchema.IndexOf(name));
end;  

function DataFrameCursor.Int(i: integer): integer;
begin
  if not IsValid(i) then
    Error(ER_VALUE_IS_NA, fSchema.NameAt(i));
  Result := intAcc[i](pos);
end;

function DataFrameCursor.Float(i: integer): real;
begin
  if not IsValid(i) then
    Error(ER_VALUE_IS_NA, fSchema.NameAt(i));
  Result := floatAcc[i](pos);
end;

function DataFrameCursor.Str(i: integer): string;
begin
  if not IsValid(i) then
    Error(ER_VALUE_IS_NA, fSchema.NameAt(i));
  Result := strAcc[i](pos);
end;

function DataFrameCursor.Bool(i: integer): boolean;
begin
  if not IsValid(i) then
    Error(ER_VALUE_IS_NA, fSchema.NameAt(i));
  Result := boolAcc[i](pos);
end;

function DataFrameCursor.DateTime(i: integer): System.DateTime;
begin
  if not IsValid(i) then
    Error(ER_VALUE_IS_NA, fSchema.NameAt(i));
  Result := dtAcc[i](pos);
end;
  
function DataFrameCursor.Int(name: string): integer;
begin
  Result := Int(fSchema.IndexOf(name));
end;

function DataFrameCursor.Float(name: string): real;
begin
  Result := Float(fSchema.IndexOf(name));
end;

function DataFrameCursor.Str(name: string): string;
begin
  Result := Str(fSchema.IndexOf(name));
end;

function DataFrameCursor.Bool(name: string): boolean;
begin
  Result := Bool(fSchema.IndexOf(name));
end;  

function DataFrameCursor.DateTime(name: string): System.DateTime;
begin
  Result := DateTime(fSchema.IndexOf(name));
end;

function DataFrameCursor.GetValue(i: integer): DataValue;
begin
  var name := fSchema.NameAt(i);

  if not IsValid(i) then
    exit(DataValue.NA(name, fSchema.ColumnTypeAt(i)));

  case fSchema.ColumnTypeAt(i) of
    ctInt: Result := new DataValue(name, Int(i));
    ctFloat: Result := new DataValue(name, Float(i));
    ctStr: Result := new DataValue(name, Str(i));
    ctBool: Result := new DataValue(name, Bool(i));
    ctDateTime: Result := new DataValue(name, DateTime(i));
  else
    Error(ER_UNKNOWN_COLUMN_TYPE);
  end;
end;

function DataFrameCursor.GetValue(name: string): DataValue;
begin
  Result := GetValue(fSchema.IndexOf(name));
end;
  
procedure DataFrameCursor.MoveTo(p: integer);
begin
  if (p < 0) or (p >= rowCnt) then
    ArgumentOutOfRangeError(ER_ROW_INDEX_OUT_OF_RANGE, p, rowCnt);

  pos := p;
end;  

end.


