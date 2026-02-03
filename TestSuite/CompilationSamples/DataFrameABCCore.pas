unit DataFrameABCCore;

interface

type
  // Базовые типы массивов
  StringArray = array of string;
  IntArray = array of integer;
  RealArray = array of real;
  BoolArray = array of boolean;
  
  ColumnType = (ctInt, ctFloat, ctStr, ctBool);
  
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
    colIndexByName: Dictionary<string, integer>;
    intAcc: array of IntAccessor;
    floatAcc: array of FloatAccessor;
    strAcc: array of StrAccessor;
    boolAcc: array of BoolAccessor;
    validAcc: array of ValidAccessor;
  public
    /// Создает курсор для указанных столбцов
    constructor Create(cols: array of Column; colIndexByName: Dictionary<string, integer>);
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

//-----------------------------
//      Сервисные функции
//-----------------------------

function NotInt(pos: integer): integer;
begin
  Result := 0;
  raise new Exception('Column is not Int');
end;

function NotFloat(pos: integer): real;
begin
  Result := 0;
  raise new Exception('Column is not Float');
end;

function NotStr(pos: integer): string;
begin
  Result := nil;
  raise new Exception('Column is not Str');
end;

function NotBool(pos: integer): boolean;
begin
  Result := False;
  raise new Exception('Column is not Bool');
end;

//-----------------------------
//           Columns
//-----------------------------

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

constructor DataFrameCursor.Create(cols: array of Column; colIndexByName: Dictionary<string, integer>);
begin
  pos := -1;
  self.colIndexByName := colIndexByName;

  if cols.Length = 0 then rowCnt := 0
  else
    case cols[0].Info.ColType of
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

    // IsValid
    if col is IntColumn then
    begin
      var c := IntColumn(col);
      if c.IsValid = nil then
        validAcc[i] := pos -> true
      else
        validAcc[i] := pos -> c.IsValid[pos];

      intAcc[i] := pos -> c.Data[pos];
      floatAcc[i] := pos -> c.Data[pos];
    end
    else if col is FloatColumn then
    begin
      var c := FloatColumn(col);
      if c.IsValid = nil then
        validAcc[i] := pos -> true
      else
        validAcc[i] := pos -> c.IsValid[pos];

      floatAcc[i] := pos -> c.Data[pos];
      intAcc[i] := NotInt;
    end
    else if col is StrColumn then
    begin
      var c := StrColumn(col);
      if c.IsValid = nil then
        validAcc[i] := pos -> true
      else
        validAcc[i] := pos -> c.IsValid[pos];

      strAcc[i] := pos -> c.Data[pos];
    end
    else if col is BoolColumn then
    begin
      var c := BoolColumn(col);
      if c.IsValid = nil then
        validAcc[i] := pos -> true
      else
        validAcc[i] := pos -> c.IsValid[pos];

      boolAcc[i] := pos -> c.Data[pos];
    end
    else raise new Exception('Unknown column type');
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
  Result := IsValid(colIndexByName[name]);
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
  Result := Int(colIndexByName[name]);
end;

function DataFrameCursor.Float(name: string): real;
begin
  Result := Float(colIndexByName[name]);
end;

function DataFrameCursor.Str(name: string): string;
begin
  Result := Str(colIndexByName[name]);
end;

function DataFrameCursor.Bool(name: string): boolean;
begin
  Result := Bool(colIndexByName[name]);
end;  
  
procedure DataFrameCursor.MoveTo(p: integer);
begin
  if (p < 0) or (p >= rowCnt) then
    raise new Exception('Cursor.MoveTo: index out of range');

  pos := p;
end;  

end.