// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit PABCExtensions;

{$zerobasedstrings off}

uses PABCSystem;

function GetCurrentLocale: string;
begin
  var locale: object;
  if __CONFIG__.TryGetValue('locale', locale) then
    Result := locale as string
  else
    Result := 'ru';
end;

function GetTranslation(message: string): string;
begin
  var cur_locale := GetCurrentLocale();
  var arr := message.Split(new string[1]('!!'), System.StringSplitOptions.None);
  if (cur_locale = 'en') and (arr.Length > 1) then
    Result := arr[1]
  else
    Result := arr[0]
end;

const
  BAD_TYPE_IN_TYPED_FILE = 'Для типизированных файлов нельзя указывать тип элементов, являющийся ссылочным или содержащий ссылочные поля!!Typed file cannot contain elements that are references or contains fields-references';
  PARAMETER_STEP_MUST_BE_NOT_EQUAL_0 = 'Параметр step не может быть равен 0!!The step parameter must be not equal to 0';
  PARAMETER_FROM_OUT_OF_RANGE = 'Параметр from за пределами диапазона!!The from parameter out of bounds';
  PARAMETER_TO_OUT_OF_RANGE = 'Параметр to за пределами диапазона!!The to parameter out of bounds';
  SLICE_SIZE_AND_RIGHT_VALUE_SIZE_MUST_BE_EQUAL = 'Размеры среза и присваиваемого выражения должны быть равны!!Slice size and assigned expression size must be equal';

//{{{doc: Начало секции расширений строк для срезов }}} 

///--
procedure CorrectFromTo(situation: integer; Len: integer; var from, &to: integer; step: integer);
begin
  if step > 0 then
  begin
    case situation of
      1: from := 0;
      2: &to := Len;
      3: (from, &to) := (0, Len)
    end;  
  end
  else
  begin
    case situation of
      1: from := Len - 1;
      2: &to := -1;
      3: (from, &to) := (Len - 1, -1);
    end;
  end;
end;

///--
function CheckAndCorrectFromToAndCalcCountForSystemSlice(situation: integer; Len: integer; var from, &to: integer; step: integer): integer;
begin
  // situation = 0 - все параметры присутствуют
  // situation = 1 - from отсутствует
  // situation = 2 - to отсутствует
  // situation = 3 - from и to отсутствуют
  if step = 0 then
    raise new System.ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));
  
  if (situation = 0) or (situation = 2) then
    if (from < 0) or (from > Len - 1) then
      raise new System.ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  if (situation = 0) or (situation = 1) then
    if (&to < -1) or (&to > Len) then
      raise new System.ArgumentException(GetTranslation(PARAMETER_TO_OUT_OF_RANGE));
  
  CorrectFromTo(situation, Len, from, &to, step);
  
  var count: integer;
  
  if step > 0 then
  begin
    var cnt := &to - from;
    if cnt <= 0 then 
      count := 0
    else count := (cnt - 1) div step + 1;
  end
  else
  begin
    var cnt := from - &to;
    if cnt <= 0 then 
      count := 0
    else count := (cnt - 1) div (-step) + 1;
  end;
  
  Result := count;
end;

procedure SystemSliceAssignmentStringImpl(var Self: string; rightValue: string; situation: integer; 
  from, &to: SystemIndex; step: integer; baseIndex: integer := 1);
begin
  from.IndexValue := from.IndexValue - baseIndex;
  &to.IndexValue := &to.IndexValue - baseIndex;
  
  var fromValue := from.IndexValue;
  var toValue := &to.IndexValue;
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation, Self.Count, fromValue, toValue, step);
  if count <> rightValue.Length then
    raise new System.ArgumentException(GetTranslation(SLICE_SIZE_AND_RIGHT_VALUE_SIZE_MUST_BE_EQUAL));
    
  var f := fromValue + 1;
  
  var strInd := 1;
  loop count do
  begin
    Self[f] := rightValue[strInd];
    f += step;
    strInd += 1;
  end;
end;  

///--
procedure SystemSliceAssignment(var Self: string; rightValue: string; situation: integer; from, &to: integer; 
  step: integer := 1); extensionmethod;
begin
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, step);
end;

///--
{procedure SystemSliceAssignment(var Self: string; rightValue: string; situation: integer; from, &to: integer); extensionmethod;
begin
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, 1);
end;}

///--
procedure SystemSliceAssignment(var Self: string; rightValue: string; situation: integer; from, &to: SystemIndex; 
  step: integer := 1); extensionmethod;
begin
  if from.IsInverted then
    from.IndexValue := Self.Length - from.IndexValue + 1;
  if &to.IsInverted then
    &to.IndexValue := Self.Length - &to.IndexValue + 1;
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, step);
end;

///--
{procedure SystemSliceAssignment(var Self: string; rightValue: string; situation: integer; from, &to: SystemIndex); extensionmethod;
begin
  if from.IsInverted then
    from.IndexValue := Self.Length - from.IndexValue + 1;
  if &to.IsInverted then
    &to.IndexValue := Self.Length - &to.IndexValue + 1;
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, 1);
end;}

///--
procedure SystemSliceAssignment0(var Self: string; rightValue: string; situation: integer; from, &to: integer; 
  step: integer := 1); extensionmethod;
begin
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, step, 0);
end;

///--
{procedure SystemSliceAssignment0(var Self: string; rightValue: string; situation: integer; from, &to: integer); extensionmethod;
begin
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, 1, 0);
end;}

///--
procedure SystemSliceAssignment0(var Self: string; rightValue: string; situation: integer; from, &to: SystemIndex; 
  step: integer := 1); extensionmethod;
begin
  if from.IsInverted then
    from.IndexValue := Self.Length - from.IndexValue;
  if &to.IsInverted then
    &to.IndexValue := Self.Length - &to.IndexValue;
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, step, 0);
end;

///--
{procedure SystemSliceAssignment0(var Self: string; rightValue: string; situation: integer; from, &to: SystemIndex); extensionmethod;
begin
  if from.IsInverted then
    from.IndexValue := Self.Length - from.IndexValue;
  if &to.IsInverted then
    &to.IndexValue := Self.Length - &to.IndexValue;
  SystemSliceAssignmentStringImpl(Self, rightValue, situation, from, &to, 1, 0);
end;}

//{{{--doc: Конец секции расширений строк для срезов }}} 

//{{{doc: Начало секции подпрограмм для типизированных файлов для документации }}} 

// -----------------------------------------------------
//>>     Подпрограммы для работы с типизированными и бестиповыми файлами # Subroutines for typed and untyped files
// -----------------------------------------------------

/// Открывает бестиповой файл и возвращает значение для инициализации файловой переменной
function OpenBinary(fname: string): file;
begin
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет бестиповой файл и возвращает значение для инициализации файловой переменной
function CreateBinary(fname: string): file;
begin
  PABCSystem.Rewrite(Result, fname);
end;

/// Открывает бестиповой файл в заданной кодировке и возвращает значение для инициализации файловой переменной
function OpenBinary(fname: string; en: Encoding): file;
begin
  PABCSystem.Reset(Result, fname, en);
end;

/// Создаёт или обнуляет бестиповой файл в заданной кодировке и возвращает значение для инициализации файловой переменной
function CreateBinary(fname: string; en: Encoding): file;
begin
  PABCSystem.Rewrite(Result, fname, en);
end;

function ContainsReferenceTypes(t: System.Type): boolean;
begin
  if t.IsPrimitive then
    Result := False
  else if t.IsValueType then 
  begin
    var fa := t.GetFields(System.Reflection.BindingFlags.GetField or System.Reflection.BindingFlags.Instance or System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.NonPublic);
    Result := fa.Any(x->ContainsReferenceTypes(x.FieldType));
  end
  else Result := True;
end;

/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string): file of T;
begin
  if ContainsReferenceTypes(typeof(T)) then
    raise new System.SystemException(GetTranslation(BAD_TYPE_IN_TYPED_FILE));
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string): file of T;
begin
  if ContainsReferenceTypes(typeof(T)) then
  begin
    raise new System.SystemException(GetTranslation(BAD_TYPE_IN_TYPED_FILE));
  end;  
  var res: file of T;
  PABCSystem.Rewrite(res, fname);
  Result := res;
end;

/// Открывает типизированный файл в заданной кодировке и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string; en: Encoding): file of T;
begin
  if ContainsReferenceTypes(typeof(T)) then
    raise new System.SystemException(GetTranslation(BAD_TYPE_IN_TYPED_FILE));
  PABCSystem.Reset(Result, fname, en);
end;

/// Создаёт или обнуляет типизированный файл в заданной кодировке и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string; en: Encoding): file of T;
begin
  if ContainsReferenceTypes(typeof(T)) then
    raise new System.SystemException(GetTranslation(BAD_TYPE_IN_TYPED_FILE));
  var res: file of T;
  PABCSystem.Rewrite(res, fname, en);
  Result := res;
end;

/// Открывает типизированный файл целых и возвращает значение для инициализации файловой переменной
function OpenFileInteger(fname: string): file of integer;
begin
  Result := OpenFile&<integer>(fname);
end;

/// Открывает типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function OpenFileReal(fname: string): file of real;
begin
  Result := OpenFile&<real>(fname);
end;

/// Создаёт или обнуляет типизированный файл целых и возвращает значение для инициализации файловой переменной
function CreateFileInteger(fname: string): file of integer;
begin
  Result := CreateFile&<integer>(fname);
end;

/// Создаёт или обнуляет типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function CreateFileReal(fname: string): file of real;
begin
  Result := CreateFile&<real>(fname);
end;

/// Открывает типизированный файл, записывает в него последовательность элементов ss и закрывает его
procedure WriteElements<T>(fname: string; ss: sequence of T);
begin
  var f := CreateFile&<T>(fname);
  foreach var x in ss do
    f.Write(x);
  f.Close
end;

// -----------------------------------------------------
//>>     Методы расширения типизированных файлов # Extension methods for typed files
// -----------------------------------------------------

/// Устанавливает текущую позицию файлового указателя в типизированном файле на элемент с номером n
function Seek<T>(Self: file of T; n: int64): file of T; extensionmethod;
begin
  PABCSystem.Seek(Self, n);
  Result := Self;
end;

/// Считывает и возвращает следующий элемент типизированного файла
function Read<T>(Self: file of T): T; extensionmethod;
begin
  PABCSystem.Read(Self, Result);
end;

/// Считывает и возвращает два следующих элемента типизированного файла в виде кортежа
function Read2<T>(Self: file of T): (T,T); extensionmethod;
begin
  var a,b: T;
  PABCSystem.Read(Self, a);
  PABCSystem.Read(Self, b);
  Result := (a,b);
end;

/// Считывает и возвращает три следующих элемента типизированного файла в виде кортежа
function Read3<T>(Self: file of T): (T,T,T); extensionmethod;
begin
  var a,b,c: T;
  PABCSystem.Read(Self, a);
  PABCSystem.Read(Self, b);
  PABCSystem.Read(Self, c);
  Result := (a,b,c);
end;

/// Возвращает последовательность элементов открытого типизированного файла от текущего элемента до конечного
function ReadElements<T>(Self: file of T): sequence of T; extensionmethod;
begin
  while not Self.Eof do
  begin
    var x := Self.Read;
    yield x;
  end;
end;

/// Возвращает последовательность элементов открытого типизированного файла
function Elements<T>(Self: file of T): sequence of T; extensionmethod;
begin
  Reset(Self); // Если файл открыт, то файловый указатель просто устанавливается на 0 позицию
  Result := Self.ReadElements;
end;


/// Открывает типизированный файл, возвращает последовательность его элементов и закрывает его
function ReadElements<T>(fname: string): sequence of T;
begin
  var f := OpenFile&<T>(fname);
  while not f.Eof do
  begin
    var x := f.Read;
    yield x;
  end;
  f.Close
end;

/// Записывает данные в типизированный файл
procedure Write<T>(Self: file of T; params vals: array of T); extensionmethod;
begin
  foreach var x in vals do
    PABCSystem.Write(Self, x);
end;

/// Открывает существующий типизированный файл
procedure Reset<T>(Self: file of T); extensionmethod;
begin
  PABCSystem.Reset(Self);
end;

/// Создает новый или обнуляет существующий типизированный файл 
procedure Rewrite<T>(Self: file of T); extensionmethod;
begin
  PABCSystem.Rewrite(Self);
end;

//{{{--doc: Конец секции подпрограмм для типизированных файлов для документации }}} 

// -----------------------------------------------------
//>>     Функции, создающие HashSet и SortedSet по встроенным множествам # Function for creation HashSet and SortedSet from set of T
// -----------------------------------------------------

{/// Создает HashSet по встроенному множеству
function HSet<T>(s: set of T): HashSet<T>;
begin
  Result := new HashSet<T>;
  foreach var x in s do
    Result += x;
end;

/// Создает SortedSet по встроенному множеству
function SSet<T>(s: set of T): SortedSet<T>;
begin
  Result := new SortedSet<T>;
  foreach var x in s do
    Result += x;
end;}


//------------------------------------------------------------------------------
//          Операции для procedure
//------------------------------------------------------------------------------
///--
function operator*(p: procedure; n: integer): procedure; extensionmethod;
begin
  Result := () -> for var i:=1 to n do p
end;

///--
function operator*(n: integer; p: procedure): procedure; extensionmethod;
begin
  Result := () -> for var i:=1 to n do p
end;

var __initialized: boolean;

{procedure __InitModule;
begin
end;}

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitPABCSystem;
    //__InitModule;
  end;
end;

begin
  //__InitModule;
end.