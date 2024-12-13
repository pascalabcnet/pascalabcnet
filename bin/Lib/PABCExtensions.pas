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
  OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT = 'Выход за границы типа множества!!Out of type range in set assignment';
  OUT_OF_TYPE_RANGE_IN_SET_OPERATION = 'Выход за границы типа множества!!Out of type range in set operation';
  OUT_OF_TYPE_RANGE_IN_ARR_OPERATION = 'Выход за границы типа массива!!Out of type range in array operation';

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

{///--
function operator implicit(a: array of integer): set of integer; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of real): set of real; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of string): set of string; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of char): set of char; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of shortint): set of shortint; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of int64): set of int64; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of byte): set of byte; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of word): set of word; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of longword): set of longword; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of BigInteger): set of BigInteger; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of decimal): set of decimal; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit(a: array of single): set of single; extensionmethod := TypedSet.InitBy(a);
///--
function operator implicit<T>(a: array of T): set of T; extensionmethod := TypedSet.InitBy(a);
}

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

// Важнейший для новых множеств !!!
{procedure operator:=<T>(var Self: set of T>; st: set of T>); extensionmethod;
begin
  Self._hs := new HashSet<T>(st.hs);
end;}

{procedure operator:=<T>(var Self: set of T>; st: NewSetEmpty); extensionmethod;
begin
  Self._hs := new HashSet<T>();
end;}

// --------------------------------------
// Extension-методы для новых множеств
// --------------------------------------

// operator implicit
function operator implicit(n: set of integer): set of byte; extensionmethod;
begin
  foreach var x in n._hs do
    if (x >= byte.MinValue) and (x <= byte.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: set of integer): set of shortint; extensionmethod;
begin
  foreach var x in n._hs do
    if (x >= shortint.MinValue) and (x <= shortint.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: set of integer): set of smallint; extensionmethod;
begin
  foreach var x in n._hs do
    if (x >= smallint.MinValue) and (x <= smallint.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: set of integer): set of word; extensionmethod;
begin
  foreach var x in n._hs do
    if (x >= word.MinValue) and (x <= word.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: set of integer): set of longword; extensionmethod;
begin
  foreach var x in n._hs do
    if (x >= longword.MinValue) and (x <= longword.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: set of integer): set of int64; extensionmethod;
begin
  foreach var x in n._hs do
    Result._hs.Add(x)
end;

function operator implicit(n: set of integer): set of uint64; extensionmethod;
begin
  foreach var x in n._hs do
    if x >= 0 then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(a: set of integer): set of BigInteger; extensionmethod;
begin
  foreach var x in a do
    Result._hs.Add(x);
end;

//-- operator implicit array of integer -> set of целое
function operator implicit(n: array of integer): set of byte; extensionmethod;
begin
  foreach var x in n do
    if (x >= byte.MinValue) and (x <= byte.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: array of integer): set of shortint; extensionmethod;
begin
  foreach var x in n do
    if (x >= shortint.MinValue) and (x <= shortint.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: array of integer): set of smallint; extensionmethod;
begin
  foreach var x in n do
    if (x >= smallint.MinValue) and (x <= smallint.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: array of integer): set of word; extensionmethod;
begin
  foreach var x in n do
    if (x >= word.MinValue) and (x <= word.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(n: array of integer): set of longword; extensionmethod;
begin
  foreach var x in n do
    if (x >= longword.MinValue) and (x <= longword.MaxValue) then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(a: array of integer): set of int64; extensionmethod;
begin
  foreach var x in a do
    Result._hs.Add(x);
end;

function operator implicit(a: array of integer): set of uint64; extensionmethod;
begin
  foreach var x in a do
    if x >= 0 then
      Result._hs.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_ASSIGNMENT));
end;

function operator implicit(a: array of integer): set of BigInteger; extensionmethod;
begin
  foreach var x in a do
    Result._hs.Add(x);
end; 

function NSToInts(ns: set of byte) := ns._hs.Select(x -> integer(x));
function NSToInts(ns: set of shortint) := ns._hs.Select(x -> integer(x));
function NSToInts(ns: set of smallint) := ns._hs.Select(x -> integer(x));
function NSToInts(ns: set of word) := ns._hs.Select(x -> integer(x));
function NSToInts64(ns: set of byte) := ns._hs.Select(x -> int64(x));
function NSToInts64(ns: set of integer) := ns._hs.Select(x -> int64(x));

function NSToBytes(ns: set of integer): sequence of byte;
begin
  var res := new List<byte>;
  foreach var x in ns._hs do
    if (x >= byte.MinValue) and (x <= byte.MaxValue) then
      res.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_OPERATION));
  Result := res;  
end; 

function NSToBytes(ns: set of int64): sequence of byte; 
begin
  var res := new List<byte>;
  foreach var x in ns._hs do
    if (x >= byte.MinValue) and (x <= byte.MaxValue) then
      res.Add(x)
    else raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_SET_OPERATION));
  Result := res;  
end; 

// operator= для set
// set of integer сравнивается со всеми
function operator=(a: set of integer; b: set of byte); extensionmethod 
  := (a.Count = b.Count) and b.All(x -> x in a); // x в меньшем диапазоне, поэтому x in a работает верно
function operator=(a: set of byte; b: set of integer): boolean; extensionmethod 
  := b = a;
function operator=(a: set of integer; b: set of shortint); extensionmethod 
  := (a.Count = b.Count) and b.All(x -> x in a);
function operator=(a: set of shortint; b: set of integer): boolean; extensionmethod 
  := b = a;
function operator=(a: set of integer; b: set of smallint); extensionmethod 
  := (a.Count = b.Count) and b.All(x -> x in a);
function operator=(a: set of smallint; b: set of integer): boolean; extensionmethod 
  := b = a;
function operator=(a: set of integer; b: set of word); extensionmethod 
  := (a.Count = b.Count) and b.All(x -> x in a);
function operator=(a: set of word; b: set of integer): boolean; extensionmethod 
  := b = a;

// В этом случае integer и longword не вкладываются друг в друга
function operator=(a: set of integer; b: set of longword): boolean; extensionmethod
  := (a.Count = b.Count) and a.All(x -> x >= 0) and a.All(x -> x in b);
function operator=(a: set of longword; b: set of integer): boolean; extensionmethod 
  := b = a;

function operator=(a: set of int64; b: set of integer); extensionmethod 
  := (a.Count = b.Count) and b.All(x -> x in a);
function operator=(a: set of integer; b: set of int64): boolean; extensionmethod 
  := b = a;

// Здесь оба типа нельзя расширить до общего типа, и есть проблема
function operator=(a: set of integer; b: set of uint64): boolean; extensionmethod
  := (a.Count = b.Count) and a.All(x -> x >= 0) and a.All(x -> x in b);
function operator=(a: set of uint64; b: set of integer): boolean; extensionmethod := b = a;


// arrays = sets
function operator=(a: array of integer; b: set of byte); extensionmethod 
  := a.ToSet = b; 
function operator=(a: set of byte; b: array of integer): boolean; extensionmethod := b = a;
function operator=(a: array of integer; b: set of shortint); extensionmethod 
  := a.ToSet = b;
function operator=(a: set of shortint; b: array of integer): boolean; extensionmethod := b = a;
function operator=(a: array of integer; b: set of smallint); extensionmethod 
  := a.ToSet = b;
function operator=(a: set of smallint; b: array of integer): boolean; extensionmethod := b = a;
function operator=(a: array of integer; b: set of word); extensionmethod 
  := a.ToSet = b; 
function operator=(a: set of word; b: array of integer): boolean; extensionmethod := b = a;
function operator=(a: array of integer; b: set of longword): boolean; extensionmethod
  := a.ToSet = b; 
function operator=(a: set of longword; b: array of integer): boolean; extensionmethod := b = a;
function operator=(a: array of integer; b: set of int64): boolean; extensionmethod 
  := a.ToSet = b; 
function operator=(a: set of int64; b: array of integer); extensionmethod := b = a;
function operator=(a: array of integer; b: set of uint64): boolean; extensionmethod
  := a.ToSet = b;
function operator=(a: set of uint64; b: array of integer): boolean; extensionmethod := b = a;
function operator=(a: array of integer; b: set of integer); extensionmethod 
  := (a.Count = b.Count) and a.All(x -> x in b); 
function operator=(a: set of integer; b: array of integer): boolean; extensionmethod := b = a;

// operator<> для set
function operator<>(a: set of integer; b: set of byte); extensionmethod := not(a = b);
function operator<>(a: set of byte; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: set of shortint); extensionmethod := not(a = b);
function operator<>(a: set of shortint; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: set of smallint); extensionmethod := not(a = b);
function operator<>(a: set of smallint; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: set of word); extensionmethod := not(a = b);
function operator<>(a: set of word; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: set of longword); extensionmethod := not(a = b);
function operator<>(a: set of longword; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: set of int64); extensionmethod := not(a = b);
function operator<>(a: set of int64; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: set of uint64); extensionmethod := not(a = b);
function operator<>(a: set of uint64; b: set of integer); extensionmethod := not(a = b);

// set<>array
function operator<>(a: array of integer; b: set of byte); extensionmethod := not(a = b);
function operator<>(a: set of byte; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of shortint); extensionmethod := not(a = b);
function operator<>(a: set of shortint; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of smallint); extensionmethod := not(a = b);
function operator<>(a: set of smallint; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of word); extensionmethod := not(a = b);
function operator<>(a: set of word; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of longword); extensionmethod := not(a = b);
function operator<>(a: set of longword; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of int64); extensionmethod := not(a = b);
function operator<>(a: set of int64; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of uint64); extensionmethod := not(a = b);
function operator<>(a: set of uint64; b: array of integer); extensionmethod := not(a = b);
function operator<>(a: array of integer; b: set of integer); extensionmethod := not(a = b);
function operator<>(a: set of integer; b: array of integer); extensionmethod := not(a = b);

// operator< для set
function operator<(a: set of integer; b: set of byte); extensionmethod 
  := (a.Count < b.Count) and a.All(x -> (x >= 0) and (x <= byte.MaxValue) and (x in b)); 
function operator<(a: set of byte; b: set of integer); extensionmethod 
  := (a.Count < b.Count) and a.All(x -> x in b);
function operator<(a: set of integer; b: set of int64); extensionmethod 
  := (a.Count < b.Count) and a.All(x -> x in b);
function operator<(a: set of int64; b: set of integer); extensionmethod 
  := (a.Count < b.Count) and a.All(x -> (x >= 0) and (x <= byte.MaxValue) and (x in b));
 
// set < array
function operator<(a: array of integer; b: set of byte); extensionmethod 
  := a.ToSet < b; 
function operator<(a: set of byte; b: array of integer); extensionmethod 
  := a < b.ToSet;
function operator<(a: array of integer; b: set of int64); extensionmethod 
  := a.ToSet < b;
function operator<(a: set of int64; b: array of integer); extensionmethod 
  := a < b.ToSet;
function operator<(a: array of integer; b: set of integer); extensionmethod 
  := (a.Count < b.Count) and a.All(x -> x in b); 
function operator<(a: set of integer; b: array of integer); extensionmethod 
  := (a.Count < b.Count) and a.All(x -> x in b);

// operator> для set
function operator>(a: set of integer; b: set of byte); extensionmethod 
  := b < a;
function operator>(a: set of byte; b: set of integer); extensionmethod 
  := b < a;
function operator>(a: set of integer; b: set of int64); extensionmethod 
  := b < a;
function operator>(a: set of int64; b: set of integer); extensionmethod 
  := b < a;
  
// set > array
function operator>(a: array of integer; b: set of byte); extensionmethod 
  := b < a;
function operator>(a: set of byte; b: array of integer); extensionmethod 
  := b < a;
function operator>(a: array of integer; b: set of int64); extensionmethod 
  := b < a;
function operator>(a: set of int64; b: array of integer); extensionmethod 
  := b < a;
function operator>(a: array of integer; b: set of integer); extensionmethod 
  := b < a;
function operator>(a: set of integer; b: array of integer); extensionmethod 
  := b < a;

// operator<= для set - доделать!!!
function operator<=(a: set of integer; b: set of byte); extensionmethod 
  := (a.Count <= b.Count) and a.All(x -> (x >= 0) and (x <= byte.MaxValue) and (x in b));
function operator<=(a: set of byte; b: set of integer); extensionmethod 
  := (a.Count <= b.Count) and a.All(x -> x in b);
function operator<=(a: set of int64; b: set of integer); extensionmethod 
  := (a.Count <= b.Count) and a.All(x -> x in b);
function operator<=(a: set of integer; b: set of int64); extensionmethod 
  := (a.Count <= b.Count) and a.All(x -> (x >= 0) and (x <= byte.MaxValue) and (x in b));

// set <= array
function operator<=(a: array of integer; b: set of byte); extensionmethod 
  := a.ToSet <= b; 
function operator<=(a: set of byte; b: array of integer); extensionmethod 
  := a <= b.ToSet;
function operator<=(a: array of integer; b: set of int64); extensionmethod 
  := a.ToSet <= b;
function operator<=(a: set of int64; b: array of integer); extensionmethod 
  := a <= b.ToSet;
function operator<=(a: array of integer; b: set of integer); extensionmethod 
  := (a.Count <= b.Count) and a.All(x -> x in b); 
function operator<=(a: set of integer; b: array of integer); extensionmethod 
  := (a.Count <= b.Count) and a.All(x -> x in b);


// operator>= для set
function operator>=(a: set of integer; b: set of byte); extensionmethod 
  := b <= a;
function operator>=(a: set of byte; b: set of integer); extensionmethod 
  := b <= a;
function operator>=(a: set of int64; b: set of integer); extensionmethod 
  := b <= a;
function operator>=(a: set of integer; b: set of int64); extensionmethod 
  := b <= a;
  
// set >= array
function operator>=(a: array of integer; b: set of byte); extensionmethod 
  := b <= a;
function operator>=(a: set of byte; b: array of integer); extensionmethod 
  := b <= a;
function operator>=(a: array of integer; b: set of int64); extensionmethod 
  := b <= a;
function operator>=(a: set of int64; b: array of integer); extensionmethod 
  := b <= a;
function operator>=(a: array of integer; b: set of integer); extensionmethod 
  := b <= a;
function operator>=(a: set of integer; b: array of integer); extensionmethod 
  := b <= a;
  

// set *= set
procedure operator*=(a: set of byte; b: set of integer); extensionmethod 
  := a *= NewSet&<byte>(b);
procedure operator*=(a: set of word; b: set of integer); extensionmethod 
  := a *= NewSet&<word>(b);
procedure operator*=(a: set of shortint; b: set of integer); extensionmethod 
  := a *= NewSet&<shortint>(b);
procedure operator*=(a: set of smallint; b: set of integer); extensionmethod 
  := a *= NewSet&<smallint>(b);
procedure operator*=(a: set of longword; b: set of integer); extensionmethod 
  := a *= NewSet&<longword>(b);
procedure operator*=(a: set of int64; b: set of integer); extensionmethod 
  := a *= NewSet&<int64>(b);
procedure operator*=(a: set of uint64; b: set of integer); extensionmethod 
  := a *= NewSet&<uint64>(b);

// set *= array
procedure operator*=(a: set of byte; b: array of integer); extensionmethod 
  := a *= NewSet&<byte>(b);
procedure operator*=(a: set of word; b: array of integer); extensionmethod 
  := a *= NewSet&<word>(b);
procedure operator*=(a: set of shortint; b: array of integer); extensionmethod 
  := a *= NewSet&<shortint>(b);
procedure operator*=(a: set of smallint; b: array of integer); extensionmethod 
  := a *= NewSet&<smallint>(b);
procedure operator*=(a: set of longword; b: array of integer); extensionmethod 
  := a *= NewSet&<longword>(b);
procedure operator*=(a: set of int64; b: array of integer); extensionmethod 
  := a *= NewSet&<int64>(b);
procedure operator*=(a: set of uint64; b: array of integer); extensionmethod 
  := a *= NewSet&<uint64>(b);


// set += set
procedure operator+=(a: set of byte; b: set of integer); extensionmethod 
  := a += NewSet&<byte>(b);
procedure operator+=(a: set of word; b: set of integer); extensionmethod 
  := a += NewSet&<word>(b);
procedure operator+=(a: set of shortint; b: set of integer); extensionmethod 
  := a += NewSet&<shortint>(b);
procedure operator+=(a: set of smallint; b: set of integer); extensionmethod 
  := a += NewSet&<smallint>(b);
procedure operator+=(a: set of longword; b: set of integer); extensionmethod 
  := a += NewSet&<longword>(b);
procedure operator+=(a: set of int64; b: set of integer); extensionmethod 
  := a += NewSet&<int64>(b);
procedure operator+=(a: set of uint64; b: set of integer); extensionmethod 
  := a += NewSet&<uint64>(b);

// set += array
procedure operator+=(a: set of byte; b: array of integer); extensionmethod 
  := a += NewSet&<byte>(b);
procedure operator+=(a: set of word; b: array of integer); extensionmethod 
  := a += NewSet&<word>(b);
procedure operator+=(a: set of shortint; b: array of integer); extensionmethod 
  := a += NewSet&<shortint>(b);
procedure operator+=(a: set of smallint; b: array of integer); extensionmethod 
  := a += NewSet&<smallint>(b);
procedure operator+=(a: set of longword; b: array of integer); extensionmethod 
  := a += NewSet&<longword>(b);
procedure operator+=(a: set of int64; b: array of integer); extensionmethod 
  := a += NewSet&<int64>(b);
procedure operator+=(a: set of uint64; b: array of integer); extensionmethod 
  := a += NewSet&<uint64>(b);

// set -= set
procedure operator-=(a: set of byte; b: set of integer); extensionmethod 
  := a -= NewSet&<byte>(b);
procedure operator-=(a: set of word; b: set of integer); extensionmethod 
  := a -= NewSet&<word>(b);
procedure operator-=(a: set of shortint; b: set of integer); extensionmethod 
  := a -= NewSet&<shortint>(b);
procedure operator-=(a: set of smallint; b: set of integer); extensionmethod 
  := a -= NewSet&<smallint>(b);
procedure operator-=(a: set of longword; b: set of integer); extensionmethod 
  := a -= NewSet&<longword>(b);
procedure operator-=(a: set of int64; b: set of integer); extensionmethod 
  := a -= NewSet&<int64>(b);
procedure operator-=(a: set of uint64; b: set of integer); extensionmethod 
  := a -= NewSet&<uint64>(b);

// set -= array
procedure operator-=(a: set of byte; b: array of integer); extensionmethod 
  := a -= NewSet&<byte>(b);
procedure operator-=(a: set of word; b: array of integer); extensionmethod 
  := a -= NewSet&<word>(b);
procedure operator-=(a: set of shortint; b: array of integer); extensionmethod 
  := a -= NewSet&<shortint>(b);
procedure operator-=(a: set of smallint; b: array of integer); extensionmethod 
  := a -= NewSet&<smallint>(b);
procedure operator-=(a: set of longword; b: array of integer); extensionmethod 
  := a -= NewSet&<longword>(b);
procedure operator-=(a: set of int64; b: array of integer); extensionmethod 
  := a -= NewSet&<int64>(b);
procedure operator-=(a: set of uint64; b: array of integer); extensionmethod 
  := a -= NewSet&<uint64>(b);

// Один из операндов должен быть set of integer (array of integer)
// operator* sets
function operator*(a: set of integer; b: set of byte): set of integer; extensionmethod;
begin
  Result := a;
  Result.hs.IntersectWith(b.Select(x -> integer(x)));
end;

function operator*(a: set of byte; b: set of integer): set of integer; extensionmethod
  := b * a;
  
function operator*(a: set of integer; b: set of int64): set of int64; extensionmethod;
begin
  Result := b;
  Result.hs.IntersectWith(a.Select(x -> int64(x)));
end;

function operator*(a: set of int64; b: set of integer): set of int64; extensionmethod
  := b * a;

// set * array
function operator*(a: array of integer; b: set of byte): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.IntersectWith(b.Select(x -> integer(x)));
end;

function operator*(a: set of byte; b: array of integer): set of integer; extensionmethod
  := b * a;
  
function operator*(a: array of integer; b: set of int64): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(b);
  Result.hs.IntersectWith(a.Select(x -> int64(x)));
end;

function operator*(a: set of int64; b: array of integer): set of int64; extensionmethod
  := b * a;

function operator*(a: array of integer; b: set of integer): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.IntersectWith(b);
end;

function operator*(a: set of integer; b: array of integer): set of integer; extensionmethod
  := b * a;
  

// operator+ sets
function operator+(a: set of integer; b: set of byte): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.UnionWith(b.Select(x -> integer(x)));
end;

function operator+(a: set of byte; b: set of integer): set of integer; extensionmethod
  := b + a;

function operator+(a: set of integer; b: set of int64): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(b);
  Result.hs.UnionWith(a.Select(x -> int64(x)));
end;

function operator+(a: set of int64; b: set of integer): set of int64; extensionmethod
  := b + a;

// set + array
function operator+(a: array of integer; b: set of byte): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.UnionWith(b.Select(x -> integer(x)));
end;

function operator+(a: set of byte; b: array of integer): set of integer; extensionmethod
  := b + a;

function operator+(a: array of integer; b: set of int64): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(b);
  Result.hs.UnionWith(a.Select(x -> int64(x)));
end;

function operator+(a: set of int64; b: array of integer): set of int64; extensionmethod
  := b + a;

function operator+(a: set of integer; b: array of integer): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a);
  Result.hs.UnionWith(b);
end;

function operator+(a: array of integer; b: set of integer): set of integer; extensionmethod
  := b + a;


// operator+ для array of num + [1,2,3]
function operator+(a: array of byte; b: array of integer): array of integer; extensionmethod 
  := a.ConvertAll(x -> integer(x)) + b;
function operator+(a: array of integer; b: array of byte): array of integer; extensionmethod 
  := b + a;
function operator+(a: array of shortint; b: array of integer): array of integer; extensionmethod 
  := a.ConvertAll(x -> integer(x)) + b;
function operator+(a: array of integer; b: array of shortint): array of integer; extensionmethod 
  := b + a;
function operator+(a: array of smallint; b: array of integer): array of integer; extensionmethod 
  := a.ConvertAll(x -> integer(x)) + b;
function operator+(a: array of integer; b: array of smallint): array of integer; extensionmethod 
  := b + a;
function operator+(a: array of word; b: array of integer): array of integer; extensionmethod 
  := a.ConvertAll(x -> integer(x)) + b;
function operator+(a: array of integer; b: array of word): array of integer; extensionmethod 
  := b + a;
function operator+(a: array of longword; b: array of integer): array of int64; extensionmethod 
  := a.ConvertAll(x -> int64(x)) + b.ConvertAll(x -> int64(x));
function operator+(a: array of integer; b: array of longword): array of int64; extensionmethod 
  := b + a;
function operator+(a: array of int64; b: array of integer): array of int64; extensionmethod 
  := a + b.ConvertAll(x -> int64(x));
function operator+(a: array of integer; b: array of int64): array of int64; extensionmethod 
  := b + a;
function operator+(a: array of uint64; b: array of integer): array of uint64; extensionmethod;
begin
  if b.Any(x -> x < 0) then
    raise new System.ArgumentException(GetTranslation(OUT_OF_TYPE_RANGE_IN_ARR_OPERATION));
  Result := a + b.ConvertAll(x -> uint64(x));
end;  
function operator+(a: array of integer; b: array of uint64): array of uint64; extensionmethod 
  := b + a;


// operator- sets
function operator-(a: set of integer; b: set of byte): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.ExceptWith(b.Select(x -> integer(x)));
end;

function operator-(a: set of byte; b: set of integer): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a.Select(x -> integer(x))); 
  Result.hs.ExceptWith(b);
end;

function operator-(a: set of integer; b: set of int64): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(a.Select(x -> int64(x))); 
  Result.hs.ExceptWith(b);
end;

function operator-(a: set of int64; b: set of integer): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.ExceptWith(b.Select(x -> int64(x)));
end;

// set - array
function operator-(a: array of integer; b: set of byte): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.ExceptWith(b.Select(x -> integer(x)));
end;

function operator-(a: set of byte; b: array of integer): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a.Select(x -> integer(x))); 
  Result.hs.ExceptWith(b);
end;

function operator-(a: array of integer; b: set of int64): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(a.Select(x -> int64(x))); 
  Result.hs.ExceptWith(b);
end;

function operator-(a: set of int64; b: array of integer): set of int64; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.ExceptWith(b.Select(x -> int64(x)));
end;

function operator-(a: array of integer; b: set of integer): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.ExceptWith(b);
end;

function operator-(a: set of integer; b: array of integer): set of integer; extensionmethod;
begin
  Result.hs.UnionWith(a); 
  Result.hs.ExceptWith(b);
end;

{function operator in(x: byte; a: array of integer): boolean; extensionmethod
  := a.Contains(integer(x));
function operator in(x: word; a: array of integer): boolean; extensionmethod
  := a.Contains(integer(x));
function operator in(x: shortint; a: array of integer): boolean; extensionmethod
  := a.Contains(integer(x));
function operator in(x: smallint; a: array of integer): boolean; extensionmethod
  := a.Contains(integer(x));
function operator in(x: longword; a: array of integer): boolean; extensionmethod
  := a.Select(x -> int64(x)).Contains(int64(x));
function operator in(x: int64; a: array of integer): boolean; extensionmethod
  := a.Select(x -> int64(x)).Contains(x);
function operator in(x: uint64; a: array of integer): boolean; extensionmethod;
begin
  if x > integer.MaxValue then
    Result := False
  else Result := a.Contains(integer(x));
end;}

function operator in(x: int64; a: array of byte): boolean; extensionmethod
  := (x >= byte.MinValue) and (x <= byte.MaxValue) and a.Contains(byte(x));
function operator in(x: int64; a: array of shortint): boolean; extensionmethod
  := (x >= shortint.MinValue) and (x <= shortint.MaxValue) and a.Contains(shortint(x));
function operator in(x: int64; a: array of smallint): boolean; extensionmethod
  := (x >= smallint.MinValue) and (x <= smallint.MaxValue) and a.Contains(smallint(x));
function operator in(x: int64; a: array of word): boolean; extensionmethod
  := (x >= word.MinValue) and (x <= word.MaxValue) and a.Contains(word(x));
function operator in(x: int64; a: array of longword): boolean; extensionmethod
  := (x >= longword.MinValue) and (x <= longword.MaxValue) and a.Contains(longword(x));
function operator in(x: int64; a: array of integer): boolean; extensionmethod
  := (x >= integer.MinValue) and (x <= integer.MaxValue) and a.Contains(integer(x));
function operator in(x: int64; a: array of uint64): boolean; extensionmethod
  := (x >= uint64.MinValue) and a.Contains(uint64(x));

function operator in(x: byte; a: array of byte): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of shortint): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of smallint): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of word): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of longword): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of integer): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of int64): boolean; extensionmethod := int64(x) in a;
function operator in(x: byte; a: array of uint64): boolean; extensionmethod := int64(x) in a;

function operator in(x: shortint; a: array of byte): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of shortint): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of smallint): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of word): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of longword): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of integer): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of int64): boolean; extensionmethod := int64(x) in a;
function operator in(x: shortint; a: array of uint64): boolean; extensionmethod := int64(x) in a;

function operator in(x: smallint; a: array of byte): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of shortint): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of smallint): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of word): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of longword): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of integer): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of int64): boolean; extensionmethod := int64(x) in a;
function operator in(x: smallint; a: array of uint64): boolean; extensionmethod := int64(x) in a;

function operator in(x: word; a: array of byte): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of shortint): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of smallint): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of word): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of longword): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of integer): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of int64): boolean; extensionmethod := int64(x) in a;
function operator in(x: word; a: array of uint64): boolean; extensionmethod := int64(x) in a;

function operator in(x: longword; a: array of byte): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of shortint): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of smallint): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of word): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of longword): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of integer): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of int64): boolean; extensionmethod := int64(x) in a;
function operator in(x: longword; a: array of uint64): boolean; extensionmethod := int64(x) in a;

function operator in(x: integer; a: array of byte): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of shortint): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of smallint): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of word): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of longword): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of integer): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of int64): boolean; extensionmethod := int64(x) in a;
function operator in(x: integer; a: array of uint64): boolean; extensionmethod := int64(x) in a;

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