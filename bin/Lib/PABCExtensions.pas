// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit PABCExtensions;

uses PABCSystem;

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

/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string): file of T;
begin
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string): file of T;
begin
  var res: file of T;
  PABCSystem.Rewrite(res, fname);
  Result := res;
end;

/// Открывает типизированный файл в заданной кодировке и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string; en: Encoding): file of T;
begin
  PABCSystem.Reset(Result, fname, en);
end;

/// Создаёт или обнуляет типизированный файл в заданной кодировке и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string; en: Encoding): file of T;
begin
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

procedure __InitModule;
begin
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitPABCSystem;
    __InitModule;
  end;
end;

begin
  __InitModule;
end.