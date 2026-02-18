unit DataFrameABCCore;

interface

type
  // Базовые типы массивов
  StringArray = array of string;
  IntArray = array of integer;
  RealArray = array of real;
  BoolArray = array of boolean;
  
  ColumnType = (ctInt, ctFloat, ctStr, ctBool);
  
  /// Неизменяемое описание структуры столбцов DataFrame
  DataFrameSchema = sealed class
  private
    fNames: array of string;
    fTypes: array of ColumnType;
    fIsCategorical: array of boolean;
    fIndexByName: Dictionary<string, integer>;
    
    class function BuildIndex(names: array of string): Dictionary<string, integer>;
  public
    property ColumnCount: integer read fNames.Length;
    property Names: array of string read fNames;
    property Types: array of ColumnType read fTypes;
    property IsCategorical: array of boolean read fIsCategorical;

    function IndexOf(name: string): integer;
    function HasColumn(name: string): boolean;

    function ColumnTypeAt(i: integer): ColumnType;
    function IsCategoricalAt(i: integer): boolean;
    function NameAt(i: integer): string;

    constructor Create(names: array of string; types: array of ColumnType;
      isCategorical: array of boolean := nil);

    { --- schema operations (immutable) --- }
    function Select(indices: array of integer): DataFrameSchema;
    function Drop(indices: array of integer): DataFrameSchema;
    function Rename(oldName, newName: string): DataFrameSchema;
    function WithCategorical(name: string; value: boolean := True): DataFrameSchema;

    { --- join helpers --- }
    class function Merge(
      left, right: DataFrameSchema;
      leftKeys, rightKeys: array of integer;
      rightPrefix: string
    ): DataFrameSchema;

    { --- debug --- }
    procedure AssertConsistent;
  end;
  
  ColumnInfo = auto class
    Name: string;
    ColType: ColumnType;
    IsCategorical: boolean;
  end;
  
  DataFrameCursor = class;
  
  /// Абстрактный базовый класс столбца
  Column = abstract class
    Info: ColumnInfo;
  public
    /// Пытается извлечь i-тое данное из столбца как числовое если это возможно
    function TryGetNumericValue(i: integer; var value: real): boolean; virtual; abstract;
    /// Возвращает количество строк в столбце
    function RowCount: integer; virtual; abstract;
    /// Добавляет невалидное (NA) значение в конец столбца
    procedure AppendInvalid; virtual; abstract;
    /// Добавляет значение из курсора в указанной позиции
    procedure AppendFromCursor(cur: DataFrameCursor; colIndex: integer); virtual; abstract;
  end;
  
  /// Столбец целых чисел
  IntColumn = class(Column)
    Data: array of integer;     // Данные столбца
    IsValid: array of boolean;  // Флаги валидности (может быть nil)
  public
    constructor Create; begin end;
    constructor Create(name: string; isCategorical: boolean);
    
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
    /// Добавляет невалидное (NA) значение в конец столбца
    procedure AppendInvalid; override;
    /// Добавляет значение из курсора в указанной позиции
    procedure AppendFromCursor(cur: DataFrameCursor; colIndex: integer); override;
  end;
  
  /// Столбец вещественных чисел
  FloatColumn = class(Column)
    Data: array of real;        // Данные столбца
    IsValid: array of boolean;  // Флаги валидности
  public  
    constructor Create; begin end;
    constructor Create(name: string);
    
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
    /// Добавляет невалидное (NA) значение в конец столбца
    procedure AppendInvalid; override;
    /// Добавляет значение из курсора в указанной позиции
    procedure AppendFromCursor(cur: DataFrameCursor; colIndex: integer); override;
  end;

  /// Столбец строк
  StrColumn = class(Column)
    Data: array of string;      // Данные столбца
    IsValid: array of boolean;  // Флаги валидности
  public
    constructor Create; begin end;
    constructor Create(name: string; isCategorical: boolean);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
    /// Добавляет невалидное (NA) значение в конец столбца
    procedure AppendInvalid; override;
    /// Добавляет значение из курсора в указанной позиции
    procedure AppendFromCursor(cur: DataFrameCursor; colIndex: integer); override;
  end;

  /// Столбец булевых значений
  BoolColumn = class(Column)
    Data: array of boolean;     // Данные столбца
    IsValid: array of boolean;  // Флаги валидности
  public  
    constructor Create; begin end;
    constructor Create(name: string);
    function TryGetNumericValue(i: integer; var value: real): boolean; override;
    /// Возвращает количество строк в столбце
    function RowCount: integer; override := Data.Length;
    /// Добавляет невалидное (NA) значение в конец столбца
    procedure AppendInvalid; override;
    /// Добавляет значение из курсора в указанной позиции
    procedure AppendFromCursor(cur: DataFrameCursor; colIndex: integer); override;
  end;
  
  // Accessor типы для курсора
  IntAccessor = function(pos: integer): integer;
  FloatAccessor = function(pos: integer): real;
  StrAccessor = function(pos: integer): string;
  BoolAccessor = function(pos: integer): boolean;
  ValidAccessor = function(pos: integer): boolean;
  
  // Структуры для Join
  JoinKeyLayout = record
    ColIndices: array of integer;
    ColTypes: array of ColumnType;
  end;
  
  JoinKey = record
    Ints: array of integer;
    Floats: array of real;
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
    validAcc: array of ValidAccessor;
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
    /// Возвращает целочисленное значение из столбца по имени
    function Int(name: string): integer;
    /// Возвращает вещественное значение из столбца по имени
    function Float(name: string): real;
    /// Возвращает строковое значение из столбца по имени
    function Str(name: string): string;
    /// Возвращает булево значение из столбца по имени
    function Bool(name: string): boolean;
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
  end;
  
implementation

uses MLExceptions;

const
  ER_COLUMN_NOT_INT =
    'Столбец не является Int!!Column is not Int';
  ER_COLUMN_NOT_FLOAT =
    'Столбец не является Float!!Column is not Float';
  ER_COLUMN_NOT_STR =
    'Столбец не является Str!!Column is not Str';
  ER_COLUMN_NOT_BOOL =
    'Столбец не является Bool!!Column is not Bool';
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
  fIsCategorical := if isCategorical = nil then nil else Copy(isCategorical);
  fIndexByName := BuildIndex(fNames);

  AssertConsistent;
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
  if fIsCategorical = nil then
    Result := false
  else
    Result := fIsCategorical[i];
end;

function DataFrameSchema.Select(indices: array of integer): DataFrameSchema;
begin
  if indices = nil then 
    ArgumentNullError(ER_INDICES_NULL);

  var n := indices.Length;
  var names := new string[n];
  var types := new ColumnType[n];
  var cats := if fIsCategorical = nil then nil else new boolean[n];

  for var i := 0 to n - 1 do
  begin
    var k := indices[i];
    if (k < 0) or (k >= ColumnCount) then
      raise new System.ArgumentOutOfRangeException('indices');
    names[i] := fNames[k];
    types[i] := fTypes[k];
    if cats <> nil then cats[i] := fIsCategorical[k];
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

  Result := new DataFrameSchema(names, fTypes, fIsCategorical);
end;

function DataFrameSchema.WithCategorical(name: string; value: boolean): DataFrameSchema;
begin
  if not HasColumn(name) then
    raise new System.ArgumentException($'Column "{name}" does not exist');

  var cats := if fIsCategorical = nil then new boolean[ColumnCount] else Copy(fIsCategorical);
  cats[IndexOf(name)] := value;

  Result := new DataFrameSchema(fNames, fTypes, cats);
end;

class function DataFrameSchema.Merge(left, right: DataFrameSchema;
  leftKeys, rightKeys: array of integer; rightPrefix: string): DataFrameSchema;
begin
  if left = nil then 
    raise new System.ArgumentException('left is nil');
  if right = nil then 
    raise new System.ArgumentException('right is nil');
  if leftKeys.Length <> rightKeys.Length then
    raise new System.ArgumentException('join keys length mismatch');

  var skip := new boolean[right.ColumnCount];
  foreach var i in rightKeys do
  begin
    if (i < 0) or (i >= right.ColumnCount) then
      raise new System.ArgumentOutOfRangeException('rightKeys');
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

procedure DataFrameSchema.AssertConsistent;
begin
  Assert(fNames.Length = fTypes.Length);
  if fIsCategorical <> nil then Assert(fIsCategorical.Length = fNames.Length);
  Assert(fIndexByName.Count = fNames.Length);
end;



//-----------------------------
//           Columns
//-----------------------------

constructor IntColumn.Create(name: string; isCategorical: boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctInt, isCategorical);

  Data := new integer[0];
  IsValid := nil;
end;

procedure IntColumn.AppendInvalid;
begin
  Data := Data + [0];

  if IsValid = nil then
  begin
    IsValid := new boolean[Length(Data) - 1];
    for var i := 0 to IsValid.Length - 1 do
      IsValid[i] := true;
  end;

  IsValid := IsValid + [false];
end;

procedure IntColumn.AppendFromCursor(cur: DataFrameCursor; colIndex: integer);
begin
  if cur.IsValid(colIndex) then
  begin
    Data := Data + [cur.Int(colIndex)];
    if IsValid <> nil then IsValid := IsValid + [true];
  end
  else AppendInvalid; 
end;

function IntColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  if not IsValid[i] then
    exit(false);

  value := Data[i];
  exit(true);
end;

constructor FloatColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctFloat, false);
  Data := new real[0];
  IsValid := nil;
end;

procedure FloatColumn.AppendFromCursor(cur: DataFrameCursor; colIndex: integer);
begin
  if cur.IsValid(colIndex) then
  begin
    Data := Data + [cur.Float(colIndex)];
    if IsValid <> nil then IsValid := IsValid + [true];
  end
  else AppendInvalid;
end;

procedure FloatColumn.AppendInvalid;
begin
  Data := Data + [0.0];

  if IsValid = nil then
  begin
    IsValid := new boolean[Length(Data) - 1];
    for var i := 0 to IsValid.Length - 1 do
      IsValid[i] := true;
  end;

  IsValid := IsValid + [false];
end;

function FloatColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  if not IsValid[i] then
    exit(false);

  value := Data[i];
  exit(true);
end;

constructor StrColumn.Create(name: string; isCategorical: boolean);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctStr, isCategorical);

  Data := new string[0];
  IsValid := nil;
end;

procedure StrColumn.AppendFromCursor(cur: DataFrameCursor; colIndex: integer);
begin
  if cur.IsValid(colIndex) then
  begin
    Data := Data + [cur.Str(colIndex)];
    if IsValid <> nil then IsValid := IsValid + [true];
  end
  else AppendInvalid;
end;

procedure StrColumn.AppendInvalid;
begin
  Data := Data + [''];

  if IsValid = nil then
  begin
    IsValid := new boolean[Length(Data) - 1];
    for var i := 0 to IsValid.Length - 1 do
      IsValid[i] := true;
  end;

  IsValid := IsValid + [false];
end;

function StrColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  exit(false);
end;

function BoolColumn.TryGetNumericValue(i: integer; var value: real): boolean;
begin
  if not IsValid[i] then
    exit(false);

  if Data[i] then
    value := 1.0
  else
    value := 0.0;

  exit(true);
end;

constructor BoolColumn.Create(name: string);
begin
  inherited Create;
  Info := new ColumnInfo(name, ctBool, false);

  Data := new boolean[0];
  IsValid := nil;
end;

procedure BoolColumn.AppendFromCursor(cur: DataFrameCursor; colIndex: integer);
begin
  if cur.IsValid(colIndex) then
  begin
    Data := Data + [cur.Bool(colIndex)];
    if IsValid <> nil then IsValid := IsValid + [true];
  end
  else AppendInvalid;
end;

procedure BoolColumn.AppendInvalid;
begin
  Data := Data + [false];

  if IsValid = nil then
  begin
    IsValid := new boolean[Length(Data) - 1];
    for var i := 0 to IsValid.Length - 1 do
      IsValid[i] := true;
  end;

  IsValid := IsValid + [false];
end;

//-----------------------------
//           JoinKey
//-----------------------------

function JoinKey.Equals(oth: object): boolean;
begin
  var other: JoinKey := JoinKey(oth);
  if Ints.Length <> other.Ints.Length then exit(false);
  if Floats.Length <> other.Floats.Length then exit(false);
  if Strs.Length <> other.Strs.Length then exit(false);
  if Bools.Length <> other.Bools.Length then exit(false);

  for var i := 0 to Ints.Length - 1 do
    if Ints[i] <> other.Ints[i] then exit(false);

  for var i := 0 to Floats.Length - 1 do
    if Floats[i] <> other.Floats[i] then exit(false);

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

  foreach var v in Floats do
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
    end;
    
  var n := cols.Length;
  colCnt := cols.Length;

  SetLength(intAcc, n);
  SetLength(floatAcc, n);
  SetLength(strAcc, n);
  SetLength(boolAcc, n);
  SetLength(validAcc, n);

  for var i := 0 to n - 1 do
  begin
    var col := cols[i];

    case fSchema.ColumnTypeAt(i) of
      ctInt:
      begin
        var c := IntColumn(col);
        if c.IsValid = nil then
          validAcc[i] := pos -> true
        else
          validAcc[i] := pos -> c.IsValid[pos];
    
        intAcc[i] := pos -> c.Data[pos];
        floatAcc[i] := pos -> c.Data[pos];
      end;
    
      ctFloat:
      begin
        var c := FloatColumn(col);
        if c.IsValid = nil then
          validAcc[i] := pos -> true
        else
          validAcc[i] := pos -> c.IsValid[pos];
    
        floatAcc[i] := pos -> c.Data[pos];
        intAcc[i] := NotInt;
      end;
    
      ctStr:
      begin
        var c := StrColumn(col);
        if c.IsValid = nil then
          validAcc[i] := pos -> true
        else
          validAcc[i] := pos -> c.IsValid[pos];
    
        strAcc[i] := pos -> c.Data[pos];
      end;
    
      ctBool:
      begin
        var c := BoolColumn(col);
        if c.IsValid = nil then
          validAcc[i] := pos -> true
        else
          validAcc[i] := pos -> c.IsValid[pos];
    
        boolAcc[i] := pos -> c.Data[pos];
      end;
    
    else raise new Exception('Unknown column type');
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

function DataFrameCursor.Int(i: integer): integer :=
  intAcc[i](pos);

function DataFrameCursor.Float(i: integer): real :=
  floatAcc[i](pos);

function DataFrameCursor.Str(i: integer): string :=
  strAcc[i](pos);

function DataFrameCursor.Bool(i: integer): boolean :=
  boolAcc[i](pos);
  
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
  
procedure DataFrameCursor.MoveTo(p: integer);
begin
  if (p < 0) or (p >= rowCnt) then
    raise new Exception('Cursor.MoveTo: index out of range');

  pos := p;
end;  

end.