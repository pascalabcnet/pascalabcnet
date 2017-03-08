// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

/// Стандартный модуль
/// !! System unit
unit PABCSystem;

 {$define PascalABC}

{$gendoc true}

// Default Application type
{$apptype console}

{$reference 'System.dll'}
{$reference 'mscorlib.dll'}
{$reference 'System.Core.dll'}
{$reference 'System.Numerics.dll'}

interface

uses
  System.Runtime.InteropServices,
  System.IO, 
  System.Collections, 
  System.Collections.Generic,
  System;

//{{{doc: Начало секции стандартных констант для документации }}} 

// -----------------------------------------------------
//>>     Стандартные константы # Standard constants
// -----------------------------------------------------
const
  /// Максимальное значение типа shortint
  MaxShortInt = shortint.MaxValue; 
  /// Максимальное значение типа byte
  MaxByte = byte.MaxValue;
  /// Максимальное значение типа smallint
  MaxSmallInt = smallint.MaxValue;
  /// Максимальное значение типа word
  MaxWord = word.MaxValue;
  /// Максимальное значение типа longword
  MaxLongWord = longword.MaxValue; 
  /// Максимальное значение типа int64
  MaxInt64 = int64.MaxValue; 
  /// Максимальное значение типа uint64
  MaxUInt64 = uint64.MaxValue; 
  /// Максимальное значение типа double
  MaxDouble = real.MaxValue; 
  /// Минимальное положительное значение типа double
  MinDouble = real.Epsilon;
  /// Максимальное значение типа real
  MaxReal = real.MaxValue; 
  /// Минимальное положительное значение типа real
  MinReal = real.Epsilon;
  /// Максимальное значение типа single
  MaxSingle = single.MaxValue; 
  /// Минимальное положительное значение типа single
  MinSingle = single.Epsilon;
  /// Максимальное значение типа integer
  MaxInt = integer.MaxValue;
  /// Константа Pi
  /// !! Pi constant
  Pi = 3.141592653589793;
  /// Константа E
  /// !! E constant
  E  = 2.718281828459045;
  /// Константа перехода на новую строку
  /// !! The newline string defined for this environment.
  NewLine = System.Environment.NewLine;
//{{{--doc: Конец секции стандартных констант для документации }}} 


//Маркер того, что это системный модуль
///--
const
  END_OF_LINE_SYMBOL = #10;
  __IS_SYSTEM_MODULE = true;

//{{{doc: Начало секции стандартных типов для документации }}} 
// -----------------------------------------------------
//>>     Стандартные типы # Standard types
// -----------------------------------------------------
type
  /// Базовый тип объектов
  Object = System.Object;
  
  /// Базовый тип исключений
  Exception = System.Exception;
  
  /// type double = real
  double = System.Double;
  
  /// type longint = integer
  longint = System.Int32;
  
  /// type cardinal = longword
  cardinal = System.UInt32;
  
  /// Представляет 128-битное вещественное число
  /// !! Represents a decimal number
  decimal = System.Decimal;
  
  /// Представляет произвольно большое целое число
  BigInteger = System.Numerics.BigInteger;
  
  /// Представляет комплексное число
  Complex = System.Numerics.Complex;
  
  /// Представляет кортеж
  Tuple = System.Tuple;
  
  /// Представляет список на базе динамического массива
  List<T> = System.Collections.Generic.List<T>;
  
  /// Представляет базовый класс для реализации интерфейса IComparer
  Comparer<T> = System.Collections.Generic.Comparer<T>;
  
  IComparable<T> = System.IComparable<T>;

  /// Представляет множество значений, реализованное на базе хеш-таблицы
  HashSet<T> = System.Collections.Generic.HashSet<T>;
  
  /// Представляет множество значений, реализованное на базе бинарного дерева поиска
  SortedSet<T> = System.Collections.Generic.SortedSet<T>;

  /// Представляет ассоциативный массив (набор пар Ключ-Значение), реализованный на базе хеш-таблицы
  Dictionary<Key,Value> = System.Collections.Generic.Dictionary<Key,Value>;
  
  /// Представляет ассоциативный массив, реализованный на базе бинарного дерева поиска
  SortedDictionary<Key,Value> = System.Collections.Generic.SortedDictionary<Key,Value>;
  
  /// Представляет ассоциативный массив (набор пар ключ-значение), реализованный на базе динамического массива пар
  SortedList<Key,Value> = System.Collections.Generic.SortedList<Key,Value>;
  
  /// Представляет пару Ключ-Значение для ассоциативного массива 
  KeyValuePair<Key,Value> = System.Collections.Generic.KeyValuePair<Key,Value>;
  
  /// Представляет двусвязный список
  LinkedList<T> = System.Collections.Generic.LinkedList<T>;
  
  /// Представляет узел двусвязного списка
  LinkedListNode<T> = System.Collections.Generic.LinkedListNode<T>;
  
  /// Представляет очередь - набор элементов, реализованных по принципу "первый вошел-первый вышел"
  Queue<T> = System.Collections.Generic.Queue<T>;
  
  /// Представляет стек - набор элементов, реализованных по принципу "последний вошел-первый вышел"
  Stack<T> = System.Collections.Generic.Stack<T>;

  /// Представляет интерфейс для коллекции
  ICollection<T> = System.Collections.Generic.ICollection<T>;

  /// Представляет интерфейс для сравнения двух элементов
  IComparer<T> = System.Collections.Generic.IComparer<T>;

  /// Представляет интерфейс для набора пар Ключ-Значение
  IDictionary<Key,Value> = System.Collections.Generic.IDictionary<Key,Value>;
  
  /// Представляет интерфейс, предоставляющий перечислитель для перебора элементов коллекции
  IEnumerable<T> = System.Collections.Generic.IEnumerable<T>;

  /// Представляет интерфейс для перебора элементов коллекции
  IEnumerator<T> = System.Collections.Generic.IEnumerator<T>;
  
  /// Представляет интерфейс для поддержки сравнения на равенство
  IEqualityComparer<T> = System.Collections.Generic.IEqualityComparer<T>;

  /// Представляет интерфейс для коллекции с доступом по индексу
  IList<T> = System.Collections.Generic.IList<T>;

  /// Представляет интерфейс для множества
  ISet<T> = System.Collections.Generic.ISet<T>;

  /// Представляет изменяемую строку символов
  StringBuilder = System.Text.StringBuilder;
  
  /// Тип кодировки символов  
  Encoding = System.Text.Encoding;
  
  /// Представляет действие без параметров
  Action0 = System.Action;
  
  /// Представляет действие с одним параметром
  Action<T> = System.Action<T>;

  /// Представляет действие с двумя параметрами
  Action2<T1,T2> = System.Action<T1,T2>;

  /// Представляет действие с тремя параметрами
  Action3<T1,T2,T3> = System.Action<T1,T2,T3>;

  /// Представляет функцию без параметров
  Func0<Res> = System.Func<Res>;

  /// Представляет функцию с одним параметром
  Func<T,Res> = System.Func<T,Res>;

  /// Представляет функцию с двумя параметрами
  Func2<T1,T2,Res> = System.Func<T1,T2,Res>;

  /// Представляет функцию с тремя параметрами
  Func3<T1,T2,T3,Res> = System.Func<T1,T2,T3,Res>;
  
  /// Представляет функцию с одним параметром целого типа, возвращающую целое
  IntFunc = Func<integer,integer>;
  
  /// Представляет функцию с одним параметром вещественного типа, возвращающую вещественное
  RealFunc = Func<real,real>;
  
  /// Представляет функцию с одним параметром строкового типа, возвращающую строку
  StringFunc = Func<string,string>;

  /// Представляет функцию с одним параметром, возвращающую boolean 
  Predicate<T> = System.Predicate<T>;

  /// Представляет функцию с двумя параметрами, возвращающую boolean 
  Predicate2<T1,T2> = function(x1: T1; x2: T2): boolean;

  /// Представляет функцию с тремя параметрами, возвращающую boolean 
  Predicate3<T1,T2,T3> = function(x1: T1; x2: T2; x3: T3): boolean;
  
  /// Представляет регулярное выражение
  Regex = System.Text.RegularExpressions.Regex;

  /// Представляет результаты из отдельного совпадения регулярного выражения
  Match = System.Text.RegularExpressions.Match;

  /// Представляет метод, вызываемый при обнаружении совпадения в Regex.Replace
  MatchEvaluator = System.Text.RegularExpressions.MatchEvaluator;

  /// Представляет набор успешных совпадений регулярного выражения
  MatchCollection = System.Text.RegularExpressions.MatchCollection;
  
  /// Представляет параметры регулярного выражения
  RegexOptions = System.Text.RegularExpressions.RegexOptions;
  
  /// Представляет результаты из одной группы при выполнении Regex.Match
  RegexGroup = System.Text.RegularExpressions.Group;
  
  /// Представляет результаты из набора групп при выполнении Regex.Match
  RegexGroupCollection = System.Text.RegularExpressions.GroupCollection;
  
  /// Представляет тип короткой строки фиксированной длины 255 символов
  ShortString = string[255];

//{{{--doc: Конец секции стандартных типов для документации }}} 

//------------------------------------------------------------------------------
//Pointers
//------------------------------------------------------------------------------
  
  //1                   //pointed to
  PBoolean  = ^boolean;//bool
  PByte     = ^byte;//byte
  PShortint = ^shortint;//sbyte
  //2
  PChar     = ^char;//char
  PSmallint = ^smallint;//short
  PWord     = ^word;//ushort
  //4
  PPointer  = ^pointer;//void*
  PInteger  = ^integer;//int32
  PLongword = ^longword;//uint32
  PLongint  = ^longint;//int64
  //8
  PInt64    = ^int64;
  PUInt64   = ^uint64;//unit64
  
  //8
  PSingle   = ^single;//single
  //16
  PReal     = ^real;//double
  PDouble   = ^double;//double  //ошибка, не сохранится, надо исправить
  //------------------------------------------------------------------------------
  
// -----------------------------------------------------
// IOSystem interface & IOStandardSystem implementation
// -----------------------------------------------------
type
  /// Интерфейс подсистемы ввода/вывода
  IOSystem = interface
    function peek: integer;
    function read_symbol: char;
    procedure read(var x: integer);
    procedure read(var x: real);
    procedure read(var x: char);
    procedure read(var x: string);
    procedure read(var x: byte);
    procedure read(var x: shortint);
    procedure read(var x: smallint);
    procedure read(var x: word);
    procedure read(var x: longword);
    procedure read(var x: int64);
    procedure read(var x: uint64);
    procedure read(var x: single);
    procedure read(var x: boolean);
    procedure readln;
    procedure write(obj: object);
    procedure write(p: pointer);
    procedure writeln;
  end;
  
  /// Стандартная подсистема ввода-вывода
  IOStandardSystem = class(IOSystem)
    state := 0; // 0 - нет символа в буфере char, 1 - есть символ в буфере char
    sym: integer;  // буфер в 1 символ для моделирования Peek в консоли
  public 
    function peek: integer;          virtual; // использует state и sym
    function read_symbol: char;      virtual; // использует state и sym 
    procedure read(var x: integer);  virtual;
    procedure read(var x: real);     virtual;
    procedure read(var x: char);     virtual;
    procedure read(var x: string);   virtual;
    procedure read(var x: byte);     virtual;
    procedure read(var x: shortint); virtual;
    procedure read(var x: smallint); virtual;
    procedure read(var x: word);     virtual;
    procedure read(var x: longword); virtual;
    procedure read(var x: int64);    virtual;
    procedure read(var x: uint64);   virtual;
    procedure read(var x: single);   virtual;
    procedure read(var x: boolean);  virtual;
    procedure readln;                virtual;
    procedure write(p: pointer);     virtual;
    procedure write(obj: object);    virtual;
    procedure writeln;               virtual;
  end;
  
// -----------------------------------------------------
//                  Classes for files & typed sets
// -----------------------------------------------------
type
  /// Тип текстового файла
  Text = class
  private 
    fi: FileInfo;
    sr: StreamReader;
    sw: StreamWriter;
  public 
    /// Возвращает значение типа integer, введенное из текстового файла
    function ReadInteger: integer;
    /// Возвращает значение типа real, введенное из текстового файла
    function ReadReal: real;
    /// Возвращает значение типа char, введенное из текстового файла
    function ReadChar: char;
    /// Возвращает значение типа string, введенное из текстового файла, без перехода на следующую строку
    function ReadString: string;
    /// Возвращает значение типа boolean, введенное из текстового файла
    function ReadBoolean: boolean;
    /// Возвращает значение типа integer, введенное из текстового файла, и переходит на следующую строку
    function ReadlnInteger: integer;
    /// Возвращает значение типа real, введенное из текстового файла, и переходит на следующую строку
    function ReadlnReal: real;
    /// Возвращает значение типа char, введенное из текстового файла, и переходит на следующую строку
    function ReadlnChar: char;
    /// Возвращает значение типа string, введенное из текстового файла, и переходит на следующую строку
    function ReadlnString: string;
    /// Возвращает значение типа boolean, введенное из текстового файла, и переходит на следующую строку
    function ReadlnBoolean: boolean;
    /// Записывает в текстовый файл значения
    procedure Write(params o: array of Object);
    /// Записывает в текстовый файл значения и переходит на следующую строку
    procedure Writeln(params o: array of Object);
    /// Возвращает True, если достигнут конец файла, и False в противном случае
    function Eof: boolean;
    /// Возвращает True, если достигнут конец строки, и False в противном случае
    function Eoln: boolean;
    /// Закрывает файл
    procedure Close;
    /// Пропускает пробельные символы, после чего возвращает True, если достигнут конец файла
    function SeekEof: boolean;
    /// Пропускает пробельные символы, после чего возвращает True, если достигнут конец строки в файле
    function SeekEoln: boolean;
    /// Записывает содержимое буфера файла на диск
    procedure Flush;
    /// Удаляет файл
    procedure Erase;
    /// Переименовывает файл, давая ему имя newname
    procedure Rename(newname: string);
    /// Возвращает имя файла
    function Name: string;
    /// Возвращает полное имя файла
    function FullName: string;
    /// Возвращает в виде строки содержимое файла от текущего положения до конца
    function ReadToEnd: string;
    /// Устанавливает файловый указатель на начало файла
    procedure Reset;
  end;
  
  /// Тип текстового файла
  TextFile = Text;
  
type 
  // Вспомогательный тип для диапазонного типа
  ///--
  Diapason = record
    low, high: integer;
    clow, chigh: object;
    constructor(_low, _high: integer);
    constructor(_low, _high: object);
  end;
  
  ///--
  {TypedSetComparer = class(System.Collections.IEqualityComparer)
    public function Equals(x: System.Object; y: System.Object): boolean;
    public function GetHashCode(obj: System.Object): integer;
  end;}
  
type
  // Вспомогательный тип для множества
  ///-- 
  TypedSet = class (System.Collections.IEnumerable)
  private 
    ht: Hashtable;
    len: integer;
    //copy_ht: Hashtable;
    low_bound, upper_bound: object;
  public 
    constructor Create;
    constructor Create(len: integer);
    constructor Create(low_bound, upper_bound: object);
    constructor Create(vals: array of byte);
    constructor Create(initValue: TypedSet);
    constructor Create(low_bound, upper_bound: object; initValue: TypedSet);
    procedure CreateIfNeed;
    function UnionSet(s: TypedSet): TypedSet;
    function SubtractSet(s: TypedSet): TypedSet;
    function IntersectSet(s: TypedSet): TypedSet;
    function CloneSet: TypedSet;
    function GetBytes: array of byte;
    function IsInDiapason(elem: object): boolean;
    function Contains(elem: object): boolean;
    procedure Clip;
    procedure Clip(len: integer);
    procedure IncludeElement(elem: object);
    procedure ExcludeElement(elem: object);
    procedure Init(params elems: array of object);
    procedure AssignSetFrom(s: TypedSet);
    function CompareEquals(s: TypedSet): boolean;
    function CompareInEquals(s: TypedSet): boolean;
    function CompareLess(s: TypedSet): boolean;
    function CompareLessEqual(s: TypedSet): boolean;
    function CompareGreater(s: TypedSet): boolean;
    function CompareGreaterEqual(s: TypedSet): boolean;
    function GetEnumerator: System.Collections.IEnumerator;
    function ToString: string; override;
    class function operator implicit<T>(s: TypedSet): HashSet<T>;
    class function operator implicit<T>(s: HashSet<T>): TypedSet;
  end;

type
// Base class for typed and binary files
///--
  AbstractBinaryFile = class
  private 
    fi: FileInfo;
    fs: FileStream;
    br: BinaryReader;
    bw: BinaryWriter;
  public 
    /// Закрывает файл
    procedure Close;
    /// Усекает двоичный файл, отбрасывая все элементы с позиции файлового указателя
    procedure Truncate;
    /// Возвращает True, если достигнут конец файла
    function Eof: boolean;
    /// Удаляет файл
    procedure Erase;
    /// Переименовывает файл, давая ему имя newname 
    procedure Rename(newname: string);
    ///- f.Write(a,b,...)
    /// Выводит значения a,b,... в двоичный файл
    procedure Write(params vals: array of object);
    /// Устанавливает файловый указатель на начало файла
    procedure Reset;
  end;

  // Class for typed files
  ///--
  TypedFile = sealed class(AbstractBinaryFile)
  private 
    ElementSize: int64;
    offset: integer;
    offsets: array of integer;
  public 
    ElementType: System.Type;
    constructor Create(ElementType: System.Type);
    constructor Create(ElementType: System.Type; offs: integer; params offsets: array of integer);
    function ToString: string; override;
    /// Возвращает текущую позицию файлового указателя в типизированном файле
    function FilePos: int64;
    /// Возвращает количество элементов в типизированном файле
    function FileSize: int64;
    /// Устанавливает текущую позицию файлового указателя в типизированном файле на элемент с номером n  
    procedure Seek(n: int64);
  end;
  
  // Class for binary files
  ///--
  BinaryFile = sealed class(AbstractBinaryFile)
  public 
    function ToString: string; override;
    /// Возвращает текущую позицию файлового указателя в бестиповом файле
    function FilePos: int64;
    /// Возвращает количество байт в бестиповом файле
    function FileSize: int64;
    /// Устанавливает текущую позицию файлового указателя в бестиповом файле на байт с номером n  
    procedure Seek(n: int64);
  end;
 
//{{{doc: Начало секции интерфейса для документации }}} 

// -----------------------------------------------------
//>>     Подпрограммы ввода # Read subroutines
// -----------------------------------------------------
///- procedure Read(a,b,...);
/// Вводит значения a,b,... с клавиатуры
procedure Read;
///--
procedure Read(var x: integer);
///--
procedure Read(var x: real);
///--
procedure Read(var x: char);
///--
procedure Read(var x: string);
///--
procedure Read(var x: byte);
///--
procedure Read(var x: shortint);
///--
procedure Read(var x: smallint);
///--
procedure Read(var x: word);
///--
procedure Read(var x: longword);
///--
procedure Read(var x: int64);
///--
procedure Read(var x: uint64);
///--
procedure Read(var x: single);
///--
procedure Read(var x: boolean);
///- procedure Readln(a,b,...);
/// Вводит значения a,b,... с клавиатуры и осуществляет переход на следующую строку
procedure Readln;

/// Возвращает значение типа integer, введенное с клавиатуры
function ReadInteger: integer;
/// Возвращает значение типа real, введенное с клавиатуры
function ReadReal: real;
/// Возвращает значение типа char, введенное с клавиатуры
function ReadChar: char;
/// Возвращает значение типа string, введенное с клавиатуры
function ReadString: string;
/// Возвращает значение типа boolean, введенное с клавиатуры
function ReadBoolean: boolean;

/// Возвращает значение типа integer, введенное с клавиатуры, и переходит на следующую строку ввода
function ReadlnInteger: integer;
/// Возвращает значение типа real, введенное с клавиатуры, и переходит на следующую строку ввода
function ReadlnReal: real;
/// Возвращает значение типа char, введенное с клавиатуры, и переходит на следующую строку ввода
function ReadlnChar: char;
/// Возвращает значение типа string, введенное с клавиатуры, и переходит на следующую строку ввода
function ReadlnString: string;
/// Возвращает значение типа boolean, введенное с клавиатуры, и переходит на следующую строку ввода
function ReadlnBoolean: boolean;

/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры
function ReadInteger(prompt: string): integer;
/// Выводит приглашение к вводу и возвращает значение типа real, введенное с клавиатуры
function ReadReal(prompt: string): real;
/// Выводит приглашение к вводу и возвращает значение типа char, введенное с клавиатуры
function ReadChar(prompt: string): char;
/// Выводит приглашение к вводу и возвращает значение типа string, введенное с клавиатуры
function ReadString(prompt: string): string;
/// Выводит приглашение к вводу и возвращает значение типа boolean, введенное с клавиатуры
function ReadBoolean(prompt: string): boolean;

/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры, 
///и осуществляет переход на следующую строку ввода
function ReadlnInteger(prompt: string): integer;
/// Выводит приглашение к вводу и возвращает значение типа real, введенное с клавиатуры, 
///и осуществляет переход на следующую строку ввода
function ReadlnReal(prompt: string): real;
/// Выводит приглашение к вводу и возвращает значение типа char, введенное с клавиатуры, 
///и осуществляет переход на следующую строку ввода
function ReadlnChar(prompt: string): char;
/// Выводит приглашение к вводу и возвращает значение типа string, введенное с клавиатуры, 
///и осуществляет переход на следующую строку ввода
function ReadlnString(prompt: string): string;
/// Выводит приглашение к вводу и возвращает значение типа boolean, введенное с клавиатуры, 
///и осуществляет переход на следующую строку ввода
function ReadlnBoolean(prompt: string): boolean;


///--
procedure ReadShortString(var s: string; n: integer);
///--
procedure ReadShortStringFromFile(f: Text; var s: string; n: integer);

///- procedure Read(f: файл; a,b,...);
/// Вводит значения a,b,... из файла f
procedure Read(f: Text);
///--
procedure Read(f: Text; var x: integer);
///--
procedure Read(f: Text; var x: real);
///--
procedure Read(f: Text; var x: char);
///--
procedure Read(f: Text; var x: string);
///--
procedure Read(f: Text; var x: byte);
///--
procedure Read(f: Text; var x: shortint);
///--
procedure Read(f: Text; var x: smallint);
///--
procedure Read(f: Text; var x: word);
///--
procedure Read(f: Text; var x: longword);
///--
procedure Read(f: Text; var x: int64);
///--
procedure Read(f: Text; var x: uint64);
///--
procedure Read(f: Text; var x: single);
///--
procedure Read(f: Text; var x: boolean);

///- procedure Readln(f: Text; a,b,...);
/// Вводит значения a,b,... из текстового файла f и осуществляет переход на следующую строку
procedure Readln(f: Text);
///--
procedure Readln(f: Text; var x: string);

/// Возвращает значение типа integer, введенное из текстового файла f
function ReadInteger(f: Text): integer;
/// Возвращает значение типа real, введенное из текстового файла f
function ReadReal(f: Text): real;
/// Возвращает значение типа char, введенное из текстового файла f
function ReadChar(f: Text): char;
/// Возвращает значение типа string, введенное из текстового файла f
function ReadString(f: Text): string;
/// Возвращает значение типа boolean, введенное из текстового файла f
function ReadBoolean(f: Text): boolean;

/// Возвращает значение типа integer, введенное из текстового файла f, 
///и осуществляет переход на следующую строку
function ReadlnInteger(f: Text): integer;
/// Возвращает значение типа real, введенное из текстового файла f, 
///и осуществляет переход на следующую строку
function ReadlnReal(f: Text): real;
/// Возвращает значение типа char, введенное из текстового файла f, 
///и осуществляет переход на следующую строку
function ReadlnChar(f: Text): char;
/// Возвращает значение типа string, введенное из текстового файла f, 
///и осуществляет переход на следующую строку
function ReadlnString(f: Text): string;
/// Возвращает значение типа boolean, введенное из текстового файла f, 
///и осуществляет переход на следующую строку
function ReadlnBoolean(f: Text): boolean;

// -----------------------------------------------------
//>>     Подпрограммы вывода # Write subroutines
// -----------------------------------------------------
///- procedure Write(a,b,...);
/// Выводит значения a,b,... на экран
procedure Write;
///--
procedure Write(obj: object);
///--
procedure Write(obj1, obj2: object);
///--
procedure Write(params args: array of object);

///- procedure Writeln(a,b,...);
/// Выводит значения a,b,... на экран и осуществляет переход на новую строку
///!!- Writeln(a,b,...)
/// Writes a,b,... to standart output stream and appends newline
procedure Writeln;
///--
procedure Writeln(obj: object);
///--
//procedure writeln(ptr: pointer); 
///--
procedure Writeln(obj1, obj2: object);
///--
procedure Writeln(params args: array of object);

///- procedure Write(f: файл; a,b,...);
/// Выводит значения a,b,... в файл f
procedure Write(f: Text);
///--
procedure Write(f: Text; val: object);
///--
procedure Write(f: Text; params args: array of object);

///- procedure Writeln(f: Text; a,b,...);
/// Выводит значения a,b,... в текстовый файл f и осуществляет переход на новую строку
procedure Writeln(f: Text);
///--
procedure Writeln(f: Text; val: object);
///--
procedure Writeln(f: Text; params args: array of object);

/// Выводит значения args согласно форматной строке formatstr
procedure WriteFormat(formatstr: string; params args: array of object);
/// Выводит значения args согласно форматной строке formatstr и осуществляет переход на новую строку
procedure WritelnFormat(formatstr: string; params args: array of object);
/// Выводит значения args в текстовый файл f согласно форматной строке formatstr
procedure WriteFormat(f: Text; formatstr: string; params args: array of object);
/// Выводит значения args в текстовый файл f согласно форматной строке formatstr
///и осуществляет переход на новую строку
procedure WritelnFormat(f: Text; formatstr: string; params args: array of object);

///- procedure Print(a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел
procedure Print(s: string);
///--
procedure Print(params args: array of object);
///- procedure Print(f: Text; a,b,...);
/// Выводит значения a,b,... в текстовый файл f, после каждого значения выводит пробел
procedure Print(f: Text; params args: array of object);
///- procedure Println(a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел и переходит на новую строку
procedure Println(params args: array of object);
///- procedure Println(f: Text; a,b,...);
/// Выводит значения a,b,... в текстовый файл f, после каждого значения выводит пробел и переходит на новую строку
procedure Println(f: Text; params args: array of object);

// -----------------------------------------------------
//>>     Общие подпрограммы для работы с файлами # Common subroutines for files
// -----------------------------------------------------
///- procedure Assign(f: файл; name: string);
/// Связывает файловую переменную с файлом на диске
procedure Assign(f: AbstractBinaryFile; name: string);
///- procedure AssignFile(f: файл; name: string);
/// Связывает файловую переменную с файлом на диске
procedure AssignFile(f: AbstractBinaryFile; name: string);
///- procedure Close(f: файл);
/// Закрывает файл
procedure Close(f: AbstractBinaryFile);
///- procedure CloseFile(f: файл);
/// Закрывает файл
procedure CloseFile(f: AbstractBinaryFile);
///- function Eof(f: файл): boolean;
/// Возвращает True, если достигнут конец файла 
function Eof(f: AbstractBinaryFile): boolean;
///- procedure Erase(f: файл);
/// Удаляет файл, связанный с файловой переменной 
procedure Erase(f: AbstractBinaryFile);
///- procedure Rename(f: файл; newname: string);
/// Переименовывает файл, связаный с файловой переменной, давая ему имя newname. 
procedure Rename(f: AbstractBinaryFile; newname: string);

// -----------------------------------------------------
//>>     Подпрограммы для работы с текстовыми файлами # Subroutines for text files
// -----------------------------------------------------
///--
procedure Assign(f: Text; name: string);
///--
procedure AssignFile(f: Text; name: string);
///--
procedure Close(f: Text);
///--
procedure CloseFile(f: Text);
/// Открывает текстовый файл на чтение в кодировке Windows
procedure Reset(f: Text);
/// Открывает текстовый файл на чтение в указанной кодировке
procedure Reset(f: Text; en: Encoding);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл на чтение в кодировке Windows
procedure Reset(f: Text; name: string);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл на чтение в указанной кодировке
procedure Reset(f: Text; name: string; en: Encoding);
/// Открывает текстовый файл на запись в кодировке Windows. 
///Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text);
/// Открывает текстовый файл на запись в указанной кодировке. 
///Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text; en: Encoding);
/// Связывает файловую переменную с именем файла name и открывает текстовый файл f на запись в кодировке Windows. 
///Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text; name: string);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл f на запись в указанной кодировке. 
///Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text; name: string; en: Encoding);
/// Открывает текстовый файл на дополнение в кодировке Windows
procedure Append(f: Text);
/// Открывает текстовый файл на дополнение в указанной кодировке
procedure Append(f: Text; en: Encoding);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл на дополнение в кодировке Windows
procedure Append(f: Text; name: string);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл на дополнение в указанной кодировке
procedure Append(f: Text; name: string; en: Encoding);
/// Возвращает текстовый файл с именем fname, открытый на чтение в кодировке Windows
function OpenRead(fname: string): Text;
/// Возвращает текстовый файл с именем fname, открытый на чтение в указанной кодировке 
function OpenRead(fname: string; en: Encoding): Text;
/// Возвращает текстовый файл с именем fname, открытый на запись в кодировке Windows
function OpenWrite(fname: string): Text;
/// Возвращает текстовый файл с именем fname, открытый на запись в указанной кодировке
function OpenWrite(fname: string; en: Encoding): Text;
/// Возвращает текстовый файл с именем fname, открытый на дополнение в кодировке Windows
function OpenAppend(fname: string): Text;
/// Возвращает текстовый файл с именем fname, открытый на дополнение в указанной кодировке 
function OpenAppend(fname: string; en: Encoding): Text;

///--
function Eof(f: Text): boolean;
/// Возвращает True, если в файле достигнут конец строки 
function Eoln(f: Text): boolean;
/// Пропускает пробельные символы, после чего возвращает True, если достигнут конец файла
function SeekEof(f: Text): boolean;
/// Пропускает пробельные символы, после чего возвращает True, если в файле достигнут конец строки
function SeekEoln(f: Text): boolean;
/// Записывает содержимое буфера файла на диск
procedure Flush(f: Text);
///--
procedure Erase(f: Text);
///--
procedure Rename(f: Text; newname: string);
///--
procedure TextFileInit(var f: Text);

/// Открывает файл, считывает из него строки в кодировке Windows и закрывает файл. В каждый момент в памяти хранится только текущая строка
function ReadLines(path: string): sequence of string;
/// Открывает файл, считывает из него строки в указаной кодировке и закрывает файл. В каждый момент в памяти хранится только текущая строка 
function ReadLines(path: string; en: Encoding): sequence of string;
/// Открывает файл, считывает из него строки в кодировке Windows в виде массива строк, после чего закрывает файл
function ReadAllLines(path: string): array of string;
/// Открывает файл, считывает из него строки в указаной кодировке в виде массива строк, после чего закрывает файл
function ReadAllLines(path: string; en: Encoding): array of string;
/// Открывает файл, считывает его содержимое в кодировке Windows в виде строки, после чего закрывает файл
function ReadAllText(path: string): string;
/// Открывает файл, считывает его содержимое в указаной кодировке в виде строки, после чего закрывает файл
function ReadAllText(path: string; en: Encoding): string;
/// Создает новый файл, записывает в него строки из последовательности в кодировке Windows, после чего закрывает файл
procedure WriteLines(path: string; ss: sequence of string);
/// Создает новый файл, записывает в него строки из последовательности в указанной кодировке, после чего закрывает файл
procedure WriteLines(path: string; ss: sequence of string; en: Encoding);
/// Создает новый файл, записывает в него строки из массива в кодировке Windows, после чего закрывает файл
procedure WriteAllLines(path: string; ss: array of string);
/// Создает новый файл, записывает в него строки из массива в указанной кодировке, после чего закрывает файл
procedure WriteAllLines(path: string; ss: array of string; en: Encoding);
/// Создает новый файл, записывает в него строку в кодировке Windows, после чего закрывает файл
procedure WriteAllText(path: string; s: string);
/// Создает новый файл, записывает в него строку в указанной кодировке, после чего закрывает файл
procedure WriteAllText(path: string; s: string; en: Encoding);

// -----------------------------------------------------
//>>     Подпрограммы для работы с двоичными файлами # Subroutines for binary files
// -----------------------------------------------------
///- procedure Reset(f: двоичный файл);
/// Открывает двоичный файл на чтение и запись.
///Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Reset(f: AbstractBinaryFile);
///- procedure Reset(f: двоичный файл; name: string);
/// Связывает файловую переменную f с файлом name на диске и открывает двоичный файл на чтение и запись.
///Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Reset(f: AbstractBinaryFile; name: string);
///- procedure Rewrite(f: двоичный файл);
/// Открывает двоичный файл на чтение и запись, при этом обнуляя его содержимое. Если файл существовал, он обнуляется.
///Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Rewrite(f: AbstractBinaryFile);
///- procedure Rewrite(f: двоичный файл; name: string);
/// Связывает файловую переменную f с файлом name на диске и открывает двоичный файл на чтение и запись, при этом обнуляя его содержимое.
///Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Rewrite(f: AbstractBinaryFile; name: string);
///- procedure Truncate(f: двоичный файл);
/// Усекает двоичный файл, отбрасывая все элементы с позиции файлового указателя.
///Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Truncate(f: AbstractBinaryFile);

///--
procedure Write(f: AbstractBinaryFile; params vals: array of object);
///--
procedure Writeln(f: AbstractBinaryFile);
///--
procedure Writeln(f: AbstractBinaryFile; val: object);
///--
procedure Writeln(f: AbstractBinaryFile; params vals: array of object);

///- function FilePos(f: двоичный файл): int64;
/// Возвращает текущую позицию файлового указателя в двоичном файле 
function FilePos(f: TypedFile): int64;
///- function FileSize(f: двоичный файл): int64;
/// Возвращает количество элементов в двоичном файле
function FileSize(f: TypedFile): int64;
///- procedure Seek(f: двоичный файл; n: int64);
/// Устанавливает текущую позицию файлового указателя в двоичном файле на элемент с данным номером  
procedure Seek(f: TypedFile; n: int64);
///--
procedure TypedFileInit(var f: TypedFile; ElementType: System.Type);
///--
procedure TypedFileInit(var f: TypedFile; ElementType: System.Type; off: integer; params offs: array of integer);
///--
procedure TypedFileInitWithShortString(var f: TypedFile; ElementType: System.Type; off: integer; params offs: array of integer);
///--
function TypedFileRead(f: TypedFile): object;

///--
function FilePos(f: BinaryFile): int64;
///--
function FileSize(f: BinaryFile): int64;
///--
procedure Seek(f: BinaryFile; n: int64);
///--
procedure BinaryFileInit(var f: BinaryFile);
///--
function BinaryFileRead(var f: BinaryFile; ElementType: System.Type): object;

// -----------------------------------------------------
//>>     Cистемные подпрограммы # System subroutines
// -----------------------------------------------------
/// Возвращает версию PascalABC.NET
function PascalABCVersion: string;
/// Возвращает количество параметров командной строки
function ParamCount: integer;
/// Возвращает i-тый параметр командной строки
function ParamStr(i: integer): string;
/// Возвращает текущий каталог
function GetDir: string;
/// Меняет текущий каталог
procedure ChDir(s: string);
/// Создает каталог
procedure MkDir(s: string);
/// Удаляет каталог
procedure RmDir(s: string);

/// Создает каталог. Возвращает True, если каталог успешно создан
function CreateDir(s: string): boolean;
/// Удаляет файл. Если файл не может быть удален, то возвращает False
function DeleteFile(s: string): boolean;
/// Возвращает текущий каталог
function GetCurrentDir: string;
/// Удаляет каталог. Возвращает True, если каталог успешно удален
function RemoveDir(s: string): boolean;
/// Переименовывает файл name, давая ему новое имя newname. Возвращает True, если файл успешно переименован
function RenameFile(name, newname: string): boolean;
/// Устанавивает текущий каталог. Возвращает True, если каталог успешно удален
function SetCurrentDir(s: string): boolean;

/// Изменяет расширение файла с именем name на newext
function ChangeFileNameExtension(name, newext: string): string;
/// Возвращает True, если файл с именем name существует
function FileExists(name: string): boolean;

/// Выводит в специальном окне стек вызовов подпрограмм если условие не выполняется
procedure Assert(cond: boolean; sourceFile: string := ''; line: integer := 0);
/// Выводит в специальном окне диагностическое сообщение и стек вызовов подпрограмм если условие не выполняется
procedure Assert(cond: boolean; message: string; sourceFile: string := ''; line: integer := 0);

/// Возвращает свободное место в байтах на диске с именем diskname
function DiskFree(diskname: string): int64;
/// Возвращает размер в байтах на диске с именем diskname
function DiskSize(diskname: string): int64;
/// Возвращает свободное место в байтах на диске disk. disk=0 - текущий диск, disk=1 - диск A: , disk=2 - диск B: и т.д.
function DiskFree(disk: integer): int64;
/// Возвращает размер в байтах на диске disk. disk=0 - текущий диск, disk=1 - диск A: , disk=2 - диск B: и т.д.
function DiskSize(disk: integer): int64;
/// Возвращает количество миллисекунд с момента начала работы программы
function Milliseconds: integer;
/// Возвращает количество миллисекунд с момента последнего вызова Milliseconds или MillisecondsDelta 
function MillisecondsDelta: integer;

/// Завершает работу программы
procedure Halt;
/// Завершает работу программы, возвращая код ошибки exitCode
procedure Halt(exitCode: integer);

/// Делает паузу на ms миллисекунд
procedure Sleep(ms: integer);
/// Возващает имя запущенного .exe-файла
function GetEXEFileName: string;
/// Преобразует указатель к строковому представлению
function PointerToString(p: pointer): string;

/// Запускает программу или документ с именем filename 
procedure Exec(filename: string);
/// Запускает программу или документ с именем filename и параметрами командной строки args
procedure Exec(filename: string; args: string);
/// Запускает программу или документ с именем filename 
procedure Execute(filename: string);
/// Запускает программу или документ с именем filename и параметрами командной строки args
procedure Execute(filename: string; args: string);

/// Возвращает последовательность имен файлов по заданному пути, соответствующих шаблону поиска 
function EnumerateFiles(path: string; searchPattern: string := '*.*'): sequence of string;
/// Возвращает последовательность имен файлов по заданному пути, соответствующих шаблону поиска, включая подкаталоги 
function EnumerateAllFiles(path: string; searchPattern: string := '*.*'): sequence of string;
/// Возвращает последовательность имен каталогов по заданному пути
function EnumerateDirectories(path: string): sequence of string;
/// Возвращает последовательность имен каталогов по заданному пути, включая подкаталоги
function EnumerateAllDirectories(path: string): sequence of string;

///-procedure New<T>(var p: ^T); 
/// Выделяет динамическую память размера sizeof(T) и возвращает в переменной p указатель на нее. Тип T должен быть размерным 
//procedure New<T>(var p: ^T); 

///-procedure Dispose<T>(var p: ^T); 
/// Освобождает динамическую память, на которую указывает p 
//procedure Dispose<T>(var p: ^T); 


// -----------------------------------------------------
//>>     Функции для работы с именами файлов # Functions for file names
// -----------------------------------------------------
/// Выделяет имя файла из полного имени файла fname
function ExtractFileName(fname: string): string;
/// Выделяет расширение из полного имени файла fname
function ExtractFileExt(fname: string): string;
/// Выделяет путь из полного имени файла fname
function ExtractFilePath(fname: string): string;
/// Выделяет имя диска и путь из полного имени файла fname
function ExtractFileDir(fname: string): string;
/// Выделяет путь из полного имени файла fname
function ExtractFileDrive(fname: string): string;
/// Возвращает полное имя файла fname
function ExpandFileName(fname: string): string;

// -----------------------------------------------------
//>>     Математические подпрограммы # Math subroutines
// -----------------------------------------------------
///-function Sign(x: число): число;
/// Возвращает знак числа x
function Sign(x: shortint): integer;
///--
function Sign(x: smallint): integer;
///--
function Sign(x: integer): integer;
///--
function Sign(x: BigInteger): integer;
///--
function Sign(x: longword): integer;
///--
function Sign(x: int64): integer;
///--
function Sign(x: uint64): integer;
///--
function Sign(x: real): integer;
///-function Abs(x: число): число;
/// Возвращает модуль числа x
function Abs(x: integer): integer;
///--
function Abs(x: shortint): shortint;
///--
function Abs(x: smallint): smallint;
///--
function Abs(x: BigInteger): BigInteger;
///--
function Abs(x: longword): longword;
///--
function Abs(x: int64): int64;
///--
function Abs(x: uint64): uint64;
///--
function Abs(x: real): real;
/// Возвращает синус числа x
function Sin(x: real): real;
/// Возвращает гиперболический синус числа x
function Sinh(x: real): real;
/// Возвращает косинус числа x
/// !! Returns the cosine of number x
function Cos(x: real): real;
/// Возвращает гиперболический косинус числа x
function Cosh(x: real): real;
/// Возвращает тангенс числа x
function Tan(x: real): real;
/// Возвращает гиперболический тангенс числа x
function Tanh(x: real): real;
/// Возвращает арксинус числа x
function ArcSin(x: real): real;
/// Возвращает арккосинус числа x
function ArcCos(x: real): real;
/// Возвращает арктангенс числа x
function ArcTan(x: real): real;
/// Возвращает экспоненту числа x
function Exp(x: real): real;
/// Возвращает натуральный логарифм числа x
function Ln(x: real): real;
/// Возвращает логарифм числа x по основанию 2
function Log2(x: real): real;
/// Возвращает десятичный логарифм числа x
function Log10(x: real): real;
/// Возвращает логарифм числа x по основанию base
function LogN(base, x: real): real;
/// Возвращает квадратный корень числа x
function Sqrt(x: real): real;
///-function Sqr(x: число): число;
/// Возвращает квадрат числа x
function Sqr(x: integer): int64;
///--
function Sqr(x: shortint): integer;
///--
function Sqr(x: smallint): integer;
///--
function Sqr(x: BigInteger): BigInteger;
///--
function Sqr(x: longword): uint64;
///--
function Sqr(x: int64): int64;
///--
function Sqr(x: uint64): uint64;
///--
function Sqr(x: real): real;
/// Возвращает x в степени y
function Power(x, y: real): real;
/// Возвращает x в степени y
function Power(x, y: integer): real;
/// Возвращает x в степени y
function Power(x: BigInteger; y: integer): BigInteger;
/// Возвращает x, округленное до ближайшего целого. Если вещественное находится посередине между двумя целыми, 
///то округление осуществляется к ближайшему четному (банковское округление): Round(2.5)=2, Round(3.5)=4
function Round(x: real): integer;
/// Возвращает x, округленное до ближайшего длинного целого
function RoundBigInteger(x: real): BigInteger;
/// Возвращает целую часть вещественного числа x
function Trunc(x: real): integer;
/// Возвращает целую часть вещественного числа x как длинное целое
function TruncBigInteger(x: real): BigInteger;
/// Возвращает целую часть числа x
function Int(x: real): real;
/// Возвращает дробную часть числа x
function Frac(x: real): real;
/// Возвращает наибольшее целое <= x
function Floor(x: real): integer;
/// Возвращает наименьшее целое >= x
function Ceil(x: real): integer;
/// Переводит радианы в градусы
function RadToDeg(x: real): real;
/// Переводит градусы в радианы
function DegToRad(x: real): real;

/// Инициализирует датчик псевдослучайных чисел
procedure Randomize;
/// Инициализирует датчик псевдослучайных чисел, используя значение seed. При одном и том же seed генерируются одинаковые псевдослучайные последовательности
procedure Randomize(seed: integer);
/// Возвращает случайное целое в диапазоне от 0 до maxValue-1
function Random(maxValue: integer): integer;
/// Возвращает случайное целое в диапазоне от a до b
function Random(a, b: integer): integer;
/// Возвращает случайное вещественное в диапазоне [0..1)
function Random: real;

///-function Max(a: число, b: число): число;
/// Возвращает максимальное из чисел a,b
function Max(a, b: byte): byte;
///--
function Max(a, b: shortint): shortint;
///--
function Max(a, b: smallint): smallint;
///--
function Max(a, b: word): word;
///--
function Max(a, b: integer): integer;
///--
function Max(a, b: BigInteger): BigInteger;
///--
function Max(a, b: longword): longword;
///--
function Max(a, b: int64): int64;
///--
function Max(a, b: uint64): uint64;
///--
function Max(a, b: real): real;
///-function Min(a: число, b: число): число;
/// Возвращает минимальное из чисел a,b
function Min(a, b: byte): byte;
///--
function Min(a, b: shortint): shortint;
///--
function Min(a, b: word): word;
///--
function Min(a, b: smallint): smallint;
///--
function Min(a, b: integer): integer;
///--
function Min(a, b: BigInteger): BigInteger;
///--
function Min(a, b: longword): longword;
///--
function Min(a, b: int64): int64;
///--
function Min(a, b: uint64): uint64;
///--
function Min(a, b: real): real;
///-function Odd(i: целое): boolean;
/// Возвращает True, если i нечетно, и False в противном случае
function Odd(i: byte): boolean;
///--
function Odd(i: shortint): boolean;
///--
function Odd(i: word): boolean;
///--
function Odd(i: smallint): boolean;
///--
function Odd(i: integer): boolean;
///--
function Odd(i: BigInteger): boolean;
///--
function Odd(i: longword): boolean;
///--
function Odd(i: int64): boolean;
///--
function Odd(i: uint64): boolean;

// -----------------------------------------------------
//>>     Функции для работы с комплексными числами # Functions for Complex numbers
// -----------------------------------------------------
/// Конструирует комплексное число с вещественной частью re и мнимой частью im
function Cplx(re,im: real): Complex;
/// Конструирует комплексное число по полярным координатам
function CplxFromPolar(magnitude,phase: real) : Complex;
/// Возвращает квадратный корень из комплексного числа
function Sqrt(c: Complex): Complex;
/// Возвращает модуль комплексного числа
function Abs(c: Complex): Complex;
/// Возвращает комплексно сопряженное число
function Conjugate(c: Complex): Complex;
/// Возвращает косинус комплексного числа
function Cos(c: Complex): Complex;
/// Возвращает экспоненту комплексного числа
function Exp(c: Complex): Complex;
/// Возвращает натуральный логарифм комплексного числа
function Log(c: Complex): Complex;
/// Возвращает десятичный логарифм комплексного числа
function Log10(c: Complex): Complex;
/// Возвращает степень комплексного числа
function Power(c,power: Complex): Complex;
/// Возвращает синус комплексного числа
function Sin(c: Complex): Complex;

// -----------------------------------------------------
//>>     Процедуры для работы со стандартными множествами # Subroutines for set of T
// -----------------------------------------------------
///- procedure Include(var s: set of T; element: T);
///Добавляет элемент element во множество s
procedure Include(var s: TypedSet; el: object);
///- procedure Exclude(var s: set of T; element: T);
///Удаляет элемент element из множества s
procedure Exclude(var s: TypedSet; el: object);

// -----------------------------------------------------
//>>     Подпрограммы для работы с символами # Subroutines for char
// -----------------------------------------------------
/// Увеличивает код символа c на 1
procedure Inc(var c: char);
/// Увеличивает код символа c на n
procedure Inc(var c: char; n: integer);
/// Уменьшает код символа c на 1
procedure Dec(var c: char);
/// Уменьшает код символа c на n
procedure Dec(var c: char; n: integer);
/// Возвращает предшествующий x символ
function Pred(x: char): char;
/// Возвращает следующий за x символ
function Succ(x: char): char;
/// Преобразует код в символ в кодировке Windows
function ChrAnsi(a: byte): char;
/// Преобразует символ в код в кодировке Windows
function OrdAnsi(a: char): byte;
/// Преобразует код в символ в кодировке Unicode 
function Chr(a: word): char;
/// Преобразует символ в код в кодировке Unicode 
function Ord(a: char): word;
/// Преобразует код в символ в кодировке Unicode
function ChrUnicode(a: word): char;
/// Преобразует символ в код в кодировке Unicode
function OrdUnicode(a: char): word;
/// Преобразует символ в верхний регистр
function UpperCase(ch: char): char;
/// Преобразует символ в нижний регистр
function LowerCase(ch: char): char;
/// Преобразует символ в верхний регистр
function UpCase(ch: char): char;
/// Преобразует символ в нижний регистр
function LowCase(ch: char): char;

// -----------------------------------------------------
//>>     Подпрограммы для работы со строками # Subroutines for string
// -----------------------------------------------------
///-procedure Str(i: целое; var s: string);
/// Преобразует целое значение i к строковому представлению и записывает результат в s
procedure Str(i: integer; var s: string);
///--
procedure Str(i: longword; var s: string);
///--
procedure Str(i: int64; var s: string);
///--
procedure Str(i: uint64; var s: string);
/// Преобразует вещественное значение r к строковому представлению и записывает результат в s
procedure Str(r: real; var s: string);
/// Преобразует вещественное значение r к строковому представлению и записывает результат в s
procedure Str(r: single; var s: string);
///--
procedure Str(s1: string; var s: string);

/// Возвращает позицию подстроки subs в строке s. Если не найдена, возвращает 0 
function Pos(subs, s: string; from: integer := 1): integer;
/// Возвращает позицию подстроки subs в строке s начиная с позиции from. Если не найдена, возвращает 0 
function PosEx(subs, s: string; from: integer := 1): integer;
/// Возвращает позицию последнего вхождения подстроки subs в строке s. Если не найдена, возвращает 0 
function LastPos(subs, s: string): integer;
/// Возвращает позицию последнего вхождения подстроки subs в строке s начиная с позиции from. Если не найдена, возвращает 0 
function LastPos(subs, s: string; from: integer): integer;

/// Возвращает длину строки 
function Length(s: string): integer;
/// Устанавливает длину строки s равной n
procedure SetLength(var s: string; n: integer);
///--
procedure SetLengthForShortString(var s: string; n, sz: integer);
/// Вставляет подстроку source в строку s с позиции index
procedure Insert(source: string; var s: string; index: integer);
///--
procedure InsertInShortString(source: string; var s: string; index, n: integer);
/// Удаляет из строки s count символов с позиции index
procedure Delete(var s: string; index, count: integer);
/// Возвращает подстроку строки s длины count с позиции index
function Copy(s: string; index, count: integer): string;
///-function Concat(s1,s2,...): string; 
/// Возвращает строку, являющуюся результатом слияния строк s1,s2,...
function Concat(params strs: array of string): string;
/// Возвращает строку, являющуюся результатом слияния строк s1 и s2
function Concat(s1, s2: string): string;
/// Возвращает строку в нижнем регистре
function LowerCase(s: string): string;
/// Возвращает строку в верхнем регистре
function UpperCase(s: string): string;
/// Возвращает строку, состоящую из count символов ch
function StringOfChar(ch: char; count: integer): string;
/// Возвращает инвертированную строку
function ReverseString(s: string): string;
/// Сравнивает строки. Возвращает значение < 0 если s1<s2, > 0 если s1>s2 и = 0 если s1=s2
function CompareStr(s1, s2: string): integer;
/// Возвращает первые count символов строки s
function LeftStr(s: string; count: integer): string;
/// Возвращает последние count символов строки s
function RightStr(s: string; count: integer): string;

/// Возвращает строку с удаленными начальными и конечными пробелами
function Trim(s: string): string;
/// Возвращает строку с удаленными начальными пробелами
function TrimLeft(s: string): string;
/// Возвращает строку с удаленными конечными пробелами
function TrimRight(s: string): string;

/// Преобразует строковое представление целого числа к числовому значению
function StrToInt(s: string): integer;
/// Преобразует строковое представление целого числа к числовому значению
function StrToInt64(s: string): int64;
/// Преобразует строковое представление вещественного числа к числовому значению
function StrToFloat(s: string): real;
/// Преобразует строковое представление s целого числа к числовому значению и записывает его в value. 
///При невозможности преобразования возвращается False
function TryStrToInt(s: string; var value: integer): boolean;
/// Преобразует строковое представление s целого числа к числовому значению и записывает его в value. 
///При невозможности преобразования возвращается False
function TryStrToInt64(s: string; var value: int64): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value. 
///При невозможности преобразования возвращается False
function TryStrToFloat(s: string; var value: real): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value. 
///При невозможности преобразования возвращается False
function TryStrToFloat(s: string; var value: single): boolean;
/// Считывает целое из строки начиная с позиции from и устанавливает from за считанным значением
function ReadIntegerFromString(s: string; var from: integer): integer;
/// Считывает вещественное из строки начиная с позиции from и устанавливает from за считанным значением
function ReadRealFromString(s: string; var from: integer): real;
/// Считывает из строки последовательность символов до пробельного символа начиная с позиции from и устанавливает from за считанным значением
function ReadWordFromString(s: string; var from: integer): string;
/// Возвращает True если достигнут конец строки или в строке остались только пробельные символы и False в противном случае
function StringIsEmpty(s: string; var from: integer): boolean;
/// Считывает целое из строки начиная с позиции from и устанавливает from за считанным значением. 
///Возвращает True если считывание удачно и False в противном случае
function TryReadIntegerFromString(s: string; var from: integer; var res: integer): boolean;
/// Считывает вещественное из строки начиная с позиции from и устанавливает from за считанным значением. 
///Возвращает True если считывание удачно и False в противном случае
function TryReadRealFromString(s: string; var from: integer; var res: real): boolean;

///-procedure Val(s: string; var value: число; var err: integer);
/// Преобразует строковое представление s целого числа к числовому значению и записывает его в переменную value. 
///Если преобразование успешно, то err=0, иначе err>0
procedure Val(s: string; var value: integer; var err: integer);
///--
procedure Val(s: string; var value: shortint; var err: integer);
///--
procedure Val(s: string; var value: smallint; var err: integer);
///--
procedure Val(s: string; var value: int64; var err: integer);
///--
procedure Val(s: string; var value: byte; var err: integer);
///--
procedure Val(s: string; var value: word; var err: integer);
///--
procedure Val(s: string; var value: longword; var err: integer);
///--
procedure Val(s: string; var value: uint64; var err: integer);
///--
procedure Val(s: string; var value: real; var err: integer);
///--
procedure Val(s: string; var value: single; var err: integer);

/// Преобразует целое число к строковому представлению
function IntToStr(a: integer): string;
/// Преобразует целое число к строковому представлению
function IntToStr(a: int64): string;
/// Преобразует вещественное число к строковому представлению
function FloatToStr(a: real): string;

/// Возвращает отформатированную строку, построенную по форматной строке и списку форматируемых параметров 
function Format(formatstring: string; params pars: array of object): string;

// -----------------------------------------------------
//>>     Общие подпрограммы # Common subroutines
// -----------------------------------------------------
/// Увеличивает значение переменной i на 1
procedure Inc(var i: integer);
/// Увеличивает значение переменной i на n
procedure Inc(var i: integer; n: integer);
/// Уменьшает значение переменной i на 1
procedure Dec(var i: integer);
/// Уменьшает значение переменной i на n
procedure Dec(var i: integer; n: integer);
///-procedure Inc(var e: перечислимый тип);
/// Увеличивает значение перечислимого типа на 1
procedure Inc(var b: byte);
///-procedure Inc(var e: перечислимый тип; n: integer);
/// Увеличивает значение перечислимого типа на n
procedure Inc(var b: byte; n: integer);
///-procedure Dec(var e: перечислимый тип);
/// Уменьшает значение перечислимого типа на 1
procedure Dec(var b: byte);
///-procedure Dec(var e: перечислимый тип; n: integer);
/// Уменьшает значение перечислимого типа на n
procedure Dec(var b: byte; n: integer);
///--
procedure Inc(var f: boolean);
///--
procedure Dec(var f: boolean);
///-function Ord(a: целое): целое;
/// Возвращает порядковый номер значения a
function Ord(a: integer): integer;
///-function Ord(a: перечислимый тип): integer;
/// Возвращает порядковый номер значения a
function Ord(a: longword): longword;
///--
function Ord(a: int64): int64;
///--
function Ord(a: uint64): uint64;
///--
function Ord(a: boolean): integer;

///-function Succ(x: целое): целое;
/// Возвращает следующее за x значение
function Succ(x: integer): integer;
///-function Succ(x: перечислимый тип): перечислимый тип;
/// Возвращает следующее за x значение
function Succ(x: byte): byte;
///--
function Succ(x: shortint): shortint;
///--
function Succ(x: smallint): smallint;
///--
function Succ(x: word): word;
///--
function Succ(x: longword): longword;
///--
function Succ(x: int64): int64;
///--
function Succ(x: uint64): uint64;
///--
function Succ(x: boolean): boolean;

///-function Pred(x: целое): целое;
/// Возвращает предшествующее x значение
function Pred(x: integer): integer;
///-function Pred(x: перечислимый тип): перечислимый тип;
/// Возвращает предшествующее x значение
function Pred(x: byte): byte;
///--
function Pred(x: shortint): shortint;
///--
function Pred(x: smallint): smallint;
///--
function Pred(x: word): word;
///--
function Pred(x: longword): longword;
///--
function Pred(x: int64): int64;
///--
function Pred(x: uint64): uint64;
///--
function Pred(x: boolean): boolean;

/// Меняет местами значения двух переменных
procedure Swap<T>(var a, b: T);
/// Возвращает True, если достигнут конец строки
function Eoln: boolean;
/// Возвращает True, если достигнут конец потока ввода
function Eof: boolean;

// -----------------------------------------------------
//>>     Подпрограммы для работы с динамическими массивами # Subroutines for array of T
// -----------------------------------------------------
///- function Low(a: array of T): integer;
/// Возвращает 0
function Low(i: System.Array): integer;
///- function High(a: array of T): integer;
/// Возвращает верхнюю границу динамического массива
function High(i: System.Array): integer;
///- function Length(a: array of T): integer;
/// Возвращает длину динамического массива
function Length(a: System.Array): integer;
///- function Length(a: array of T; dim: integer): integer;
/// Возвращает длину динамического массива по размерности dim
function Length(a: System.Array; dim: integer): integer;
///- procedure SetLength(var a: array of T);
/// Устанавливает длину одномерного динамического массива. Старое содержимое сохраняется
//procedure SetLength(var a: System.Array);
///- procedure SetLength(var a: array of T; n1,n2,...: integer);
/// Устанавливает размеры n-мерного динамического массива. Старое содержимое сохраняется
//procedure SetLength(var a: System.Array);
///- function Copy(a: array of T): array of T;
/// Создаёт копию динамического массива
function Copy(a: System.Array): System.Array;
/// Сортирует динамический массив по возрастанию
procedure Sort<T>(a: array of T);
/// Сортирует динамический массив по критерию сортировки, задаваемому функцией сравнения cmp
procedure Sort<T>(a: array of T; cmp: (T,T)->integer);
/// Сортирует динамический массив по критерию сортировки, задаваемому функцией сравнения less
procedure Sort<T>(a: array of T; less: (T,T)->boolean);
/// Сортирует список по возрастанию
procedure Sort<T>(l: List<T>);
/// Сортирует список по критерию сортировки, задаваемому функцией сравнения cmp
procedure Sort<T>(l: List<T>; cmp: (T,T)->integer);
/// Сортирует список по критерию сортировки, задаваемому функцией сравнения less
procedure Sort<T>(l: List<T>; less: (T,T)->boolean);
/// Изменяет порядок элементов в динамическом массиве на противоположный
procedure Reverse<T>(a: array of T);
/// Изменяет порядок элементов на противоположный в диапазоне динамического массива длины length начиная с индекса index
procedure Reverse<T>(a: array of T; index,count: integer);
/// Изменяет порядок элементов в списке на противоположный
procedure Reverse<T>(a: List<T>);
/// Изменяет порядок элементов на противоположный в диапазоне списка длины length начиная с индекса index
procedure Reverse<T>(a: List<T>; index,count: integer);
/// Перемешивает динамический массив случайным образом
procedure Shuffle<T>(a: array of T);
/// Перемешивает список случайным образом
procedure Shuffle<T>(l: List<T>);

// -----------------------------------------------------
//>>     Подпрограммы для генерации последовательностей # Subroutines for sequence generation
// -----------------------------------------------------
/// Возвращает последовательность целых от a до b
function Range(a,b: integer): sequence of integer;
/// Возвращает последовательность символов от c1 до c2
function Range(c1,c2: char): sequence of char;
/// Возвращает последовательность вещественных в точках разбиения отрезка [a,b] на n равных частей (Используйте Partition)
function Range(a,b: real; n: integer): sequence of real;
/// Возвращает последовательность вещественных в точках разбиения отрезка [a,b] на n равных частей
function Partition(a,b: real; n: integer): sequence of real;
/// Возвращает последовательность целых от a до b с шагом step
function Range(a,b,step: integer): sequence of integer;
/// Возвращает последовательность указанных элементов
function Seq<T>(params a: array of T): sequence of T;
/// Возвращает последовательность из n случайных целых элементов
function SeqRandom(n: integer := 10; a: integer := 0; b: integer := 100): sequence of integer;
/// Возвращает последовательность из n случайных целых элементов
function SeqRandomInteger(n: integer := 10; a: integer := 0; b: integer := 100): sequence of integer;
/// Возвращает последовательность из n случайных вещественных элементов
function SeqRandomReal(n: integer := 10; a: real := 0; b: real := 10): sequence of real;
/// Возвращает последовательность из count элементов, заполненных значениями f(i)
function SeqGen<T>(count: integer; f: integer -> T): sequence of T;
/// Возвращает последовательность из count элементов, заполненных значениями f(i), начиная с i=from
function SeqGen<T>(count: integer; f: integer -> T; from: integer): sequence of T;
/// Возвращает последовательность из count элементов, начинающуюся с first, с функцией next перехода от предыдущего к следующему 
function SeqGen<T>(count: integer; first: T; next: T -> T): sequence of T;
/// Возвращает последовательность из count элементов, начинающуюся с first и second, 
///с функцией next перехода от двух предыдущих к следующему 
function SeqGen<T>(count: integer; first,second: T; next: (T,T) -> T): sequence of T;
/// Возвращает последовательность элементов с начальным значением first, 
///функцией next перехода от предыдущего к следующему и условием pred продолжения последовательности 
function SeqWhile<T>(first: T; next: T -> T; pred: T -> boolean): sequence of T;
/// Возвращает последовательность элементов, начинающуюся с first и second, 
///с функцией next перехода от двух предыдущих к следующему и условием pred продолжения последовательности 
function SeqWhile<T>(first,second: T; next: (T,T) -> T; pred: T -> boolean): sequence of T;
/// Возвращает последовательность из count элементов x 
function SeqFill<T>(count: integer; x: T): sequence of T;

/// Возвращает последовательность из n целых, введенных с клавиатуры
function ReadSeqInteger(n: integer): sequence of integer;
/// Возвращает последовательность из n вещественных, введенных с клавиатуры
function ReadSeqReal(n: integer): sequence of real;
/// Возвращает последовательность из n строк, введенных с клавиатуры
function ReadSeqString(n: integer): sequence of string;

/// Выводит приглашение к вводу и возвращает последовательность из n целых, введенных с клавиатуры
function ReadSeqInteger(const prompt: string; n: integer): sequence of integer;
/// Выводит приглашение к вводу и возвращает последовательность из n вещественных, введенных с клавиатуры
function ReadSeqReal(const prompt: string; n: integer): sequence of real;
/// Выводит приглашение к вводу и возвращает последовательность из n строк, введенных с клавиатуры
function ReadSeqString(const prompt: string; n: integer): sequence of string;

/// Возвращает последовательность целых, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqIntegerWhile(cond: integer -> boolean): sequence of integer;
/// Возвращает последовательность вещественных, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqRealWhile(cond: real -> boolean): sequence of real;
/// Возвращает последовательность строк, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqStringWhile(cond: string -> boolean): sequence of string;

/// Выводит приглашение к вводу и возвращает последовательность целых, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqIntegerWhile(const prompt: string; cond: integer -> boolean): sequence of integer;
/// Выводит приглашение к вводу и возвращает последовательность вещественных, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqRealWhile(const prompt: string; cond: real -> boolean): sequence of real;
/// Выводит приглашение к вводу и возвращает последовательность строк, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqStringWhile(const prompt: string; cond: string -> boolean): sequence of string;

// -----------------------------------------------------
//>>     Подпрограммы для генерации динамических массивов # Subroutines for array of T generation
// -----------------------------------------------------
/// Возвращает массив, заполненный указанными значениями
function Arr<T>(params a: array of T): array of T;
/// Возвращает массив, заполненный значениями из последовательнсти
function Arr<T>(a: sequence of T): array of T;
/// Возвращает массив размера n, заполненный случайными целыми значениями
function ArrRandom(n: integer := 10; a: integer := 0; b: integer := 100): array of integer;
/// Возвращает массив размера n, заполненный случайными целыми значениями
function ArrRandomInteger(n: integer := 10; a: integer := 0; b: integer := 100): array of integer;
/// Возвращает массив размера n, заполненный случайными вещественными значениями
function ArrRandomReal(n: integer := 10; a: real := 0; b: real := 10): array of real;
/// Возвращает массив из count элементов, заполненных значениями gen(i)
function ArrGen<T>(count: integer; gen: integer -> T): array of T;
/// Возвращает массив из count элементов, заполненных значениями gen(i), начиная с i=from
function ArrGen<T>(count: integer; gen: integer -> T; from: integer): array of T;
/// Возвращает массив из count элементов, начинающихся с first, с функцией next перехода от предыдущего к следующему 
function ArrGen<T>(count: integer; first: T; next: T -> T): array of T;
/// Возвращает массив из count элементов, начинающихся с first и second, с функцией next перехода от двух предыдущих к следующему 
function ArrGen<T>(count: integer; first,second: T; next: (T,T) -> T): array of T;
/// Возвращает массив из count элементов x 
function ArrFill<T>(count: integer; x: T): array of T;

/// Возвращает массив из n целых, введенных с клавиатуры
function ReadArrInteger(n: integer): array of integer;
/// Возвращает массив из n вещественных, введенных с клавиатуры
function ReadArrReal(n: integer): array of real;
/// Возвращает массив из n строк, введенных с клавиатуры
function ReadArrString(n: integer): array of string;

/// Выводит приглашение к вводу и возвращает массив из n целых, введенных с клавиатуры
function ReadArrInteger(const prompt: string; n: integer): array of integer;
/// Выводит приглашение к вводу и возвращает массив из n вещественных, введенных с клавиатуры
function ReadArrReal(const prompt: string; n: integer): array of real;
/// Выводит приглашение к вводу и возвращает массив из n строк, введенных с клавиатуры
function ReadArrString(const prompt: string; n: integer): array of string;

// -----------------------------------------------------
//>>     Подпрограммы для матриц # Subroutines for matrixes 
// -----------------------------------------------------
/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandom(m: integer := 5; n: integer := 5; a: integer := 0; b: integer := 100): array [,] of integer;
/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer := 5; n: integer := 5; a: integer := 0; b: integer := 100): array [,] of integer;
/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer := 5; n: integer := 5; a: real := 0; b: real := 10): array [,] of real;
/// Возвращает двумерный массив размера m x n, заполненный элементами x 
function MatrFill<T>(m,n: integer; x: T): array [,] of T;
/// Возвращает двумерный массив размера m x n, заполненный элементами x 
function MatrGen<T>(m,n: integer; gen: (integer,integer)->T): array [,] of T;
/// Транспонирует двумерный массив 
function Transpose<T>(a: array [,] of T): array [,] of T;

// -----------------------------------------------------
//>>     Подпрограммы для создания кортежей # Subroutines for tuple generation
// -----------------------------------------------------
///- function Rec(x1: T1, x2: T2,...): (T1,T2,...);
/// Возвращает кортеж из элементов разных типов
function Rec<T1,T2>(x1: T1; x2: T2): System.Tuple<T1,T2>;
///--
function Rec<T1,T2,T3>(x1: T1; x2: T2; x3: T3): (T1,T2,T3);
///--
function Rec<T1,T2,T3,T4>(x1: T1; x2: T2; x3: T3; x4: T4): (T1,T2,T3,T4);
///--
function Rec<T1,T2,T3,T4,T5>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5): (T1,T2,T3,T4,T5);
///--
function Rec<T1,T2,T3,T4,T5,T6>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5; x6: T6): (T1,T2,T3,T4,T5,T6);
///--
function Rec<T1,T2,T3,T4,T5,T6,T7>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5; x6: T6; x7: T7): (T1,T2,T3,T4,T5,T6,T7);

// -----------------------------------------------------
//>>     Короткие функции Lst, LLst, HSet, SSet, Dict, KV # Short functions Lst, HSet, SSet, Dict, KV
// -----------------------------------------------------
/// Возвращает список, заполненный указанными значениями
function Lst<T>(params a: array of T): List<T>;
/// Возвращает список, заполненный значениями из последовательности
function Lst<T>(a: sequence of T): List<T>;
/// Возвращает двусвязный список, заполненный указанными значениями
function LLst<T>(params a: array of T): LinkedList<T>;
/// Возвращает двусвязный список, заполненный значениями из последовательности
function LLst<T>(a: sequence of T): LinkedList<T>;
/// Возвращает множество на базе хеш таблицы, заполненное указанными значениями
function HSet<T>(params a: array of T): HashSet<T>;
/// Возвращает множество на базе хеш таблицы, заполненное значениями из последовательности
function HSet<T>(a: sequence of T): HashSet<T>;
/// Возвращает множество на базе бинарного дерева поиска, заполненное значениями из последовательности
function SSet<T>(params a: array of T): SortedSet<T>;
/// Возвращает множество на базе бинарного дерева поиска, заполненное значениями из последовательности
function SSet<T>(a: sequence of T): SortedSet<T>;
/// Возвращает словарь пар элементов (ключ, значение)
function Dict<TKey, TVal>(params pairs: array of KeyValuePair<TKey, TVal>): Dictionary<TKey, TVal>;
/// Возвращает словарь пар элементов (ключ, значение)
function Dict<TKey, TVal>(params pairs: array of (TKey, TVal)): Dictionary<TKey, TVal>;
/// Возвращает пару элементов (ключ, значение)
function KV<TKey, TVal>(key: TKey; value: TVal): KeyValuePair<TKey, TVal>;

//{{{--doc: Конец секции интерфейса для документации }}} 


// -----------------------------------------------------
//     Стандартные классы исключений
// -----------------------------------------------------
type
  ///Базовый класс для исключений, бросаемых при создании инстанции generic-типа
  BadGenericInstanceParameterException = class(Exception)
  protected 
    InstanceType: System.Type;
  public 
    constructor Create(ActualParameterType: System.Type);
  end;
  
  ///Бросается если тип непригоден для указателей
  CanNotUseTypeForPointersException = class(BadGenericInstanceParameterException)
  public 
    function ToString: string; override;
  end;
  
  ///Бросается если тип непригоден для типизированных файлов
  CanNotUseTypeForTypedFilesException = class(BadGenericInstanceParameterException)
  public 
    function ToString: string; override;
  end;
  
  ///Бросается если тип непригоден для бинарных файлов
  CanNotUseTypeForFilesException = class(BadGenericInstanceParameterException)
  public 
    function ToString: string; override;
  end;

  RangeException = class(SystemException) end;
  CommandLineArgumentOutOfRangeException = class(SystemException) end;
  

// -----------------------------------------------------
//     Общедоступные переменные
// -----------------------------------------------------
var
/// Содержит аргумены командой строки, с которыми была запущена программа
  CommandLineArgs: array of string;
  /// Стандартный текстовый файл для вывода. Связывается процедурой Assign с файлом на диске, после чего весь вывод на консоль перенаправляется в этот файл
  output: TextFile;
  /// Стандартный текстовый файл для ввода. Связывается процедурой Assign с файлом на диске, после чего весь ввод с консоли перенаправляется из этого файла
  input: TextFile;
  /// Определяет текущую систему ввода-вывода
  CurrentIOSystem: IOSystem;
  /// Принимает значение True, если приложение имеет консольное окно
  IsConsoleApplication: boolean;
  ///--
  RedirectIOInDebugMode := False;
  ///--
  ExecuteBeforeProcessTerminateIn__Mode: procedure(e: Exception);
  //GCHandlersForReferencePointers := new GCHandlersController;
  ///--
  ExitCode := 0; // TODO Сделать возврат в Main
  ///--
  DefaultEncoding: Encoding;
  ///--
  PrintDelimDefault: string := ' ';

///--
var
  __CONFIG__: Dictionary<string, object> := new Dictionary<string, object>;

// Вспомогательные подпрограммы. Из раздела интерфейса не убирать!!! 
// -----------------------------------------------------
//                  Internal System subprograms
// -----------------------------------------------------
///--
function CopyWithSize(source, dest: &Array): &Array;
///--
function check_in_range(val: int64; low, up: int64): int64;
///--
function check_in_range_char(val: char; low, up: char): char;
///--
function RunTimeSizeOf(t: System.Type): integer;
///--
function GetCharInShortString(s: string; ind, n: integer): char;
///--
function SetCharInShortString(s: string; ind, n: integer; c: char): string;
///--
function ClipShortString(s: string; len: integer): string;
///--
function GetResourceStream(ResourceFileName: string): Stream;
///--
function FormatValue(value: object; NumOfChars: integer): string;
///--
function FormatValue(value: integer; NumOfChars: integer): string;
///--
function FormatValue(value: int64; NumOfChars: integer): string;
///--
function FormatValue(value: real; NumOfChars: integer): string;
///--
function FormatValue(value: real; NumOfChars, NumOfSignesAfterDot: integer): string;
///--
procedure StringDefaultPropertySet(var s: string; index: integer; c: char);

//procedure InitShortString(var s: ShortString; Length: integer);

///Проверяет возможность использования указателей на тип T
procedure CheckCanUsePointerOnType(T: System.Type);
///Проверяет возможность записи типа T в файл
procedure CheckCanUseTypeForBinaryFiles(T: System.Type);
///Проверяет возможность создания file of T
procedure CheckCanUseTypeForTypedFiles(T: System.Type);
///Определяет специальные типы
function RuntimeDetermineType(T: System.Type): byte;
///Возвращает объект класса в зависимости от значения kind
function RuntimeInitialize(kind: byte; variable: object): object;
///Вычисление размера типа на этапе выполнения
function GetRuntimeSize<T>: integer;
///Возвращает строку для вывода в write
function _ObjectToString(o: object): string;

function IsUnix: boolean;
///--
function ExecuteAssemlyIsDll: boolean;

// -----------------------------------------------------
//                  Internal for OpenMPSupport
// -----------------------------------------------------
///--
procedure omp_set_nested(nested: integer);
///--
function omp_get_nested: integer;
///--
var OMP_NESTED: boolean := false;

// -----------------------------------------------------
//                  Internal typed sets operations
// -----------------------------------------------------
///--
function Union(s1, s2: TypedSet): TypedSet;
///--
function Subtract(s1, s2: TypedSet): TypedSet;
///--
function Intersect(s1, s2: TypedSet): TypedSet;
///--
function CreateSet(params elems: array of object): TypedSet;
///--
function CreateSet: TypedSet;
///--
function CreateBoundedSet(low, high: object): TypedSet;
///--
function InSet(obj: object; s: TypedSet): boolean;
///--
function CreateDiapason(low, high: integer): Diapason;
///--
function CreateObjDiapason(low, high: object): Diapason;
///--
function CompareSetEquals(s1, s2: TypedSet): boolean;
///--
function CompareSetInEquals(s1, s2: TypedSet): boolean;
///--
function CompareSetLess(s1, s2: TypedSet): boolean;
///--
function CompareSetGreaterEqual(s1, s2: TypedSet): boolean;
///--
function CompareSetLessEqual(s1, s2: TypedSet): boolean;
///--
function CompareSetGreater(s1, s2: TypedSet): boolean;
///--
procedure ClipSet(var s: TypedSet; low, high: object);
///--
procedure AssignSet(var left: TypedSet; right: TypedSet);
///--
function ClipSetFunc(s: TypedSet; low, high: object): TypedSet;
///--
function ClipShortStringInSet(s: TypedSet; len: integer): TypedSet;
///--
procedure ClipShortStringInSetProcedure(var s: TypedSet; len: integer);
///--
procedure AssignSetWithBounds(var left: TypedSet; right: TypedSet; low, high: object);
///--
procedure TypedSetInit(var st: TypedSet);
///--
procedure TypedSetInitWithBounds(var st: TypedSet; low, high: object);
///--
procedure TypedSetInitWithShortString(var st: TypedSet; len: integer);

// -----------------------------------------------------
//                  Internal classes
// -----------------------------------------------------
type  
  ///--
  GCHandlersController = class(System.Collections.IEnumerable)
  private 
    Counters, Handlers: Hashtable;
  public 
    constructor;
    procedure Add(obj: Object);
    procedure Remove(obj: Object);
    function GetCounter(obj: Object): integer;
    function GetEnumerator: System.Collections.IEnumerator;
  end;
  
type
  ///--
  PointerOutput = class
  public 
    p: pointer;
    function ToString: string; override;
    constructor(ptr: pointer);
  end;

// -----------------------------------------------------
//                  Internal procedures for PABCRTL.dll
// -----------------------------------------------------
///--
procedure __InitModule__;
///--
procedure __InitPABCSystem;
///--
procedure __FinalizeModule__;

implementation

var
  rnd: System.Random;
  //  ENCultureInfo: System.Globalization.CultureInfo;
  nfi: System.Globalization.NumberFormatInfo;
  LastReadChar := #0;
  AnsiOrdChrEncoding := Encoding.GetEncoding(1251);
  __one_char := new char[1];
  __one_byte := new byte[1];
  StartTime: DateTime; // Для Milliseconds
  
const
  WRITELN_IN_BINARYFILE_ERROR_MESSAGE = 'Операция Writeln не применима к бинарным файлам!!Writeln is not applicable to binary files';
  InternalNullBasedArrayName = 'NullBasedArray';
  FILE_NOT_ASSIGNED = 'Для файловой переменной не вызвана процедура Assign!!File is not assigned';
  FILE_NOT_OPENED = 'Файл не открыт!!File is not opened';
  FILE_NOT_OPENED_FOR_READING = 'Файл не открыт на чтение!!File is not opened for reading';
  FILE_NOT_OPENED_FOR_WRITING = 'Файл не открыт на запись!!File is not opened for writing';
  RANGE_ERROR_MESSAGE = 'Выход за границы диапазона!!Out of range';
  EOF_FOR_TEXT_WRITEOPENED = 'Функция Eof не может быть вызвана для текстового файла, открытого на запись!!Eof function can''t be called for file, opened on writing';
  EOLN_FOR_TEXT_WRITEOPENED = 'Функция Eoln не может быть вызвана для текстового файла, открытого на запись!!Eoln function can''t be called for file, opened on writing';
  SEEKEOF_FOR_TEXT_WRITEOPENED = 'Функция SeekEof не может быть вызвана для текстового файла, открытого на запись!!SeekEof function can''t be called for file, opened on writing';
  SEEKEOLN_FOR_TEXT_WRITEOPENED = 'Функция SeekEoln не может быть вызвана для текстового файла, открытого на запись!!SeekEoln function can''t be called for file, opened on writing';
  BAD_TYPE_IN_RUNTIMESIZEOF = 'Bad Type in RunTimeSizeOf';
  PARAMETER_COUNT_MUST_BE_GREATER_0 = 'Параметр count должен быть > 0!!Parameter count must be > 0';
  PARAMETER_COUNT_MUST_BE_GREATER_1 = 'Параметр count должен быть > 1!!Parameter count must be > 1';
  PARAMETER_STEP_MUST_BE_NOT_EQUAL_0 = 'Параметр step не может быть равен 0!!The step parameter must be not equal to 0';
  PARAMETER_STEP_MUST_BE_GREATER_0 = 'Параметр step должен быть > 0!!The step parameter must be not greater than 0';
  PARAMETER_FROM_OUT_OF_RANGE = 'Параметр from за пределами диапазона!!The from parameter out of bounds';
  PARAMETER_TO_OUT_OF_RANGE = 'Параметр to за пределами диапазона!!The to parameter out of bounds';

// -----------------------------------------------------
//                  WINAPI
// -----------------------------------------------------

function WINAPI_AllocConsole: longword; external 'kernel32.dll' name 'AllocConsole';

var console_alloc: boolean := false;

// -----------------------------------------------------
//                  Internal functions
// -----------------------------------------------------

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
  var arr := message.Split(new string[1]('!!'),StringSplitOptions.None);
  if (cur_locale = 'en') and (arr.Length > 1) then
    Result := arr[1]
  else
    Result := arr[0]
end;

function IsWDE: boolean;
begin
  Result := AppDomain.CurrentDomain.GetData('_RedirectIO_SpecialArgs') <> nil;
end;

[System.Security.SecuritySafeCriticalAttribute]
procedure AllocConsole;
begin
  if not IsConsoleApplication and (System.Environment.OSVersion.Platform <> PlatformID.Unix) and (AppDomain.CurrentDomain.GetData('_RedirectIO_SpecialArgs') = nil) then
    WINAPI_AllocConsole;
  console_alloc := true;
end;

function GetNullBasedArray(arr: object): System.Array;
var
  fi: System.Reflection.FieldInfo;
begin
  fi := arr.GetType.GetField(InternalNullBasedArrayName);
  if fi <> nil then
    Result := System.Array(fi.GetValue(arr))
  else
    Result := nil;
end;

function FormatFloatNumber(s: string): string;
begin
  Result := s.Replace(',', '.');
end;

procedure ClipSet(var s: TypedSet; low, high: object);
begin
  s.low_bound := low;
  s.upper_bound := high;
  s.Clip;
end;

function ClipSetFunc(s: TypedSet; low, high: object): TypedSet;
begin
  s.low_bound := low;
  s.upper_bound := high;
  s.Clip();
  Result := s;
end;

function ClipShortStringInSet(s: TypedSet; len: integer): TypedSet;
begin
  s.len := len;
  s.Clip;
  Result := s;
end;

procedure ClipShortStringInSetProcedure(var s: TypedSet; len: integer);
begin
  s.len := len;
  s.Clip;
end;

procedure AssignSet(var left: TypedSet; right: TypedSet);
begin
  left := right.CloneSet();
end;

procedure AssignSetWithBounds(var left: TypedSet; right: TypedSet; low, high: object);
begin
  left := right.CloneSet();
  left.low_bound := low;
  right.upper_bound := high;
end;

procedure TypedSetInit(var st: TypedSet);
begin
  if st = nil then
    st := new TypedSet;
end;

procedure TypedSetInitWithBounds(var st: TypedSet; low, high: object);
begin
  if st = nil then
    st := new TypedSet(low, high);
end;

procedure TypedSetInitWithShortString(var st: TypedSet; len: integer);
begin
  if st = nil then
    st := new TypedSet(len);
end;

// -----------------------------------------------------
//                  Diapason
// -----------------------------------------------------
constructor Diapason.Create(_low, _high: integer);
begin
  low := _low; high := _high;
end;

constructor Diapason.Create(_low, _high: object);
begin
  clow := _low; chigh := _high;
end;

// -----------------------------------------------------
//                  TypedSet
// -----------------------------------------------------
///--
constructor TypedSet.Create;
begin
  ht := new Hashtable({new TypedSetComparer()});
end;

///--
constructor TypedSet.Create(len: integer);
begin
  ht := new Hashtable({new TypedSetComparer()});
  Self.len := len;
end;

///--
constructor TypedSet.Create(low_bound, upper_bound: object);
begin
  ht := new Hashtable({new TypedSetComparer()});
  Self.low_bound := low_bound;
  Self.upper_bound := upper_bound;
end;

///--
constructor TypedSet.Create(initValue: TypedSet);
begin
  ht := new Hashtable({new TypedSetComparer()});
  Self.AssignSetFrom(initValue);
  Self.len := initValue.len;
end;

///--
constructor TypedSet.Create(low_bound, upper_bound: object; initValue: TypedSet);
begin
  ht := new Hashtable({new TypedSetComparer()});
  Self.low_bound := low_bound;
  Self.upper_bound := upper_bound;
  Self.AssignSetFrom(initValue);
end;

///--
constructor TypedSet.Create(vals: array of byte);
var
  i: integer;
begin
  ht := new Hashtable({new TypedSetComparer()});
  i := 0;
  while i < 256 div 8 do
  begin
    if vals[i] and 128 = 128 then ht.Add(i * 8, i * 8);
    if vals[i] and 64 = 64 then ht.Add(i * 8 + 1, i * 8 + 1);
    if vals[i] and 32 = 32 then ht.Add(i * 8 + 2, i * 8 + 2);
    if vals[i] and 16 = 16 then ht.Add(i * 8 + 3, i * 8 + 3);
    if vals[i] and 8 = 8 then ht.Add(i * 8 + 4, i * 8 + 4);
    if vals[i] and 4 = 4 then ht.Add(i * 8 + 5, i * 8 + 5);
    if vals[i] and 2 = 2 then ht.Add(i * 8 + 6, i * 8 + 6);
    if vals[i] and 1 = 1 then ht.Add(i * 8 + 7, i * 8 + 7);
    i := i + 1;
  end;
end;

///--
procedure TypedSet.CreateIfNeed;
begin
  if ht = nil then ht := new Hashtable({new TypedSetComparer()});
end;

[System.Diagnostics.DebuggerStepThrough]
function TypedSet.CloneSet: TypedSet;
begin
  Result := new TypedSet();
  Result.ht := ht.Clone() as Hashtable;
  //Result.copy_ht := ht;
  Result.low_bound := low_bound;
  Result.upper_bound := upper_bound;
end;

///--
function TypedSet.GetBytes: array of byte;
var
  ba: System.Collections.BitArray;
  i: integer;
begin
  ba := new BitArray(256);
  Result := nil;
  foreach o: object in ht.Keys do
  begin
    try
      i := Convert.ToInt32(o);
      if (i < 0) and (i >= -128) and (i <= 127) then
        ba[i + 128] := true
      else if (i >= 0) and (i <= 255) then
        ba[i] := true;
    
    except 
      on e: System.Exception do
      begin
        Result := nil;
        //Exit;
      end;
    end;
  end;
  SetLength(Result, 256 div 8);
  i := 0;
  while i < 256 div 8 do
  begin
    Result[i] := Convert.ToByte(ba[i * 8 + 7]) or (Convert.ToByte(ba[i * 8 + 6]) shl 1) or (Convert.ToByte(ba[i * 8 + 5]) shl 2) or (Convert.ToByte(ba[i * 8 + 4]) shl 3)
    or (Convert.ToByte(ba[i * 8 + 3]) shl 4) or (Convert.ToByte(ba[i * 8 + 2]) shl 5) or (Convert.ToByte(ba[i * 8 + 1]) shl 6) or (Convert.ToByte(ba[i * 8]) shl 7);
    i := i + 1;
  end;
end;

///--
function TypedSet.UnionSet(s: TypedSet): TypedSet;
begin
  Result := Union(Self, s);
end;

///--
function TypedSet.SubtractSet(s: TypedSet): TypedSet;
begin
  Result := Subtract(Self, s);
end;

///--
function TypedSet.IntersectSet(s: TypedSet): TypedSet;
begin
  Result := Intersect(Self, s);
end;

///--
function TypedSet.IsInDiapason(elem: object): boolean;
begin
  if (low_bound <> nil) and (upper_bound <> nil) and (elem is System.IComparable) then
  begin
    case System.Type.GetTypeCode(elem.GetType) of
      TypeCode.Char:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToChar(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToChar(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.Int32:
        begin
          if not (elem is integer) then elem := Convert.ToInt32(elem);
          if ((elem as System.IComparable).CompareTo(Convert.ToInt32(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToInt32(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.Byte:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToByte(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToByte(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.SByte:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToSByte(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToSByte(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.Int16:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToInt16(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToInt16(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.UInt16:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToUint16(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToUInt16(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.UInt32:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToUInt32(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToUInt32(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.Int64:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToInt64(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToInt64(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
      TypeCode.UInt64:
        begin
          if ((elem as System.IComparable).CompareTo(Convert.ToUInt64(low_bound)) >= 0) and ((elem as System.IComparable).CompareTo(Convert.ToUInt64(upper_bound)) <= 0) then
            Result := true
          else Result := false
        end;
    else
      if elem.GetType().IsEnum then 
      begin
        if ((Convert.ToInt32(elem)).CompareTo(Convert.ToInt32(low_bound)) >= 0) and ((Convert.ToInt32(elem)).CompareTo(Convert.ToInt32(upper_bound)) <= 0) then
          Result := true
        else Result := false
      end
      else Result := true;
    end// case
  end // then
  else Result := true;
end;

function convert_elem(obj: object): object; // для TypedSet
begin
  var t := obj.GetType;
  if t.IsEnum then
  begin
    Result := obj;
    exit;
  end;
  case System.Type.GetTypeCode(t) of
    TypeCode.Byte, TypeCode.SByte, TypeCode.Int16, TypeCode.UInt16, TypeCode.Int32:
    Result := Convert.ToInt32(obj);
    TypeCode.UInt32:
      begin
        var tmp: longword := longword(obj);
        if tmp <= integer.MaxValue then
          Result := integer(tmp)
        else
          Result := Convert.ToInt64(obj);
      end;
    TypeCode.Int64:
      begin
        var tmp: int64 := int64(obj);
        if tmp <= integer.MaxValue then
          Result := integer(tmp)
        else
          Result := obj;
      end;
    TypeCode.UInt64:
      begin
        var tmp: uint64 := uint64(obj);
        if tmp <= integer.MaxValue then
          Result := integer(tmp)
        else
        if tmp <= int64.MaxValue then
          Result := int64(tmp)
        else
          Result := obj;
      end
  else
    Result := obj;
  end;
end;

///--
function TypedSet.Contains(elem: object): boolean;
begin
  if elem.GetType().IsEnum then
  begin
    Result := ht[elem] <> nil
  end
  else 
  begin
    elem := convert_elem(elem);
    Result := ht.ContainsKey(elem);// <> nil;
    if not Result and (elem is char) then
      Result := ht.ContainsKey(Convert.ToString(elem));
  end;
end;

///--
procedure TypedSet.Clip;
begin
  if Self.len > 0 then
  begin
    Clip(Self.len);
    exit;
  end;
  var tmp_ht := new Hashtable();
  foreach el: object in ht.Keys do
  begin
    if IsInDiapason(el) then 
    begin
      if (Self.low_bound <> nil) then
      begin
        var tmp := convert_elem(el);
        tmp_ht.Add(tmp, tmp)
      end
      else
        tmp_ht.Add(el, el);
    end;
  end;
  ht := tmp_ht;
end;

///--
procedure TypedSet.Clip(len: integer);
begin
  var tmp_ht := new Hashtable();
  foreach el: object in ht.Keys do
  begin
    var str_el := Convert.ToString(el);
    if str_el.Length > len then
    begin
      var s := str_el.Substring(0, len);
      tmp_ht.Add(s, s);
    end
    else
      tmp_ht.Add(str_el, str_el);
  end;
  ht := tmp_ht;
end;

procedure TypedSet.IncludeElement(elem: object);
var
  diap: Diapason;
begin
  if elem = nil then exit;
  elem := convert_elem(elem);
  if not IsInDiapason(elem) then Exit;
  if elem.GetType().IsEnum then
  begin
    ht[elem] := elem;
    //if copy_ht <> nil then 
    //  copy_ht[elem] := elem;
  end
  else
  if not (elem is Diapason) then
  begin
    ht[elem] := elem;
    //if copy_ht <> nil then
    //  copy_ht[elem] := elem;
  end
  else
  begin
    diap := Diapason(elem);
    if diap.clow = nil then
    begin
      for var i := diap.low to diap.high do
      begin
        ht[i] := i;
        //if copy_ht <> nil then
        //  copy_ht[i] := i;
      end
    end
    else 
    begin
      if diap.clow is char then
      begin
        for var c := char(diap.clow) to char(diap.chigh) do
        begin
          ht[c] := c;
          //if copy_ht <> nil then
          //  copy_ht[c] := c;
        end
      end
      else if diap.clow is boolean then
      begin
        for var b := boolean(diap.clow) to boolean(diap.chigh) do
          ht[b] := b;
      end
      else if diap.clow.GetType().IsEnum then
      begin
        for var i := integer(diap.clow) to integer(diap.chigh) do
        begin
          var obj := Enum.ToObject(diap.clow.GetType(), i);
          ht[obj] := obj;
          //if copy_ht <> nil then
          // copy_ht[obj] := obj;
        end;
      end;
    end;
  end;
end;

procedure TypedSet.ExcludeElement(elem: object);
begin
  if elem.GetType().IsEnum then
  begin
    ht.Remove(elem);
  end
  else
  begin
    elem := convert_elem(elem);
    ht.Remove(elem);
  end
end;

///--
procedure TypedSet.Init(params elems: array of object);
begin
  for var i := 0 to elems.Length - 1 do
    ht[elems[i]] := elems[i];
end;

[System.Diagnostics.DebuggerStepThrough]
procedure TypedSet.AssignSetFrom(s: TypedSet);
begin
  ht := s.ht.Clone() as Hashtable;
  Clip;
end;

///--
function TypedSet.GetEnumerator: System.Collections.IEnumerator;
begin
  Result := ht.Keys.GetEnumerator; 
end;

function FormatStr(obj: object): string;
begin
  if (obj.GetType = typeof(char)) or (obj.GetType = typeof(string)) then
    Result := '''' + string.Format(System.Globalization.NumberFormatInfo.InvariantInfo, '{0}', new object[](obj)) + ''''
  else
    Result := string.Format(System.Globalization.NumberFormatInfo.InvariantInfo, '{0}', new object[](obj))
end;

///--
class function TypedSet.operator implicit<T>(s: TypedSet): HashSet<T>;
begin
  var hs := new HashSet<T>();
  foreach key: T in s.ht.Keys do
  begin
    hs.Add(key);  
  end;
  Result := hs; 
end;

///--
class function TypedSet.operator implicit<T>(s: HashSet<T>): TypedSet;
begin
  var ts := new TypedSet();
  foreach key: T in s.ToArray() do
  begin
    ts.ht[key] := key;  
  end;
  Result := ts; 
end;

///--
function TypedSet.ToString: string;
var
  i: System.Collections.IEnumerator;
  lst: ArrayList;
begin
  i := GetEnumerator;
  lst := new ArrayList();
  var t: &Type; 
  var added := false;
  if i.MoveNext then
    if not (i.Current is System.IComparable) then
    begin
      result := '' + FormatStr(i.Current) + '';
      added := true;
    end
    else 
    begin
      lst.Add(i.Current);
      t := i.Current.GetType;
    end;
  while i.MoveNext do
    if not (i.Current is System.IComparable) then
    begin
      result := (added ? result + ',' : '') + FormatStr(i.Current);
      added := true;
    end
    else
    begin
      if (t <> nil) and (t <> i.Current.GetType) then
      begin
        result := (added ? result + ',' : '') + FormatStr(i.Current);
        added := true;
      end
      else
      begin
        t := i.Current.GetType;
        lst.Add(i.Current);
      end;  
    end;
  
  if lst.Count > 0 then
  begin
    lst.Sort;
    var ind := 1;
    if not added then
      result := '' + FormatStr(lst[0]) + ''
    else
      ind := 0;
    for j: integer := ind to lst.Count - 1 do
    begin
      result := result + ',' + FormatStr(lst[j]);
    end;
  end;
  result := '[' + result + ']';
end;

{class function TypedSet.operator implicit<T>(hset: HashSet<T>): TypedSet;
begin
  Result := new TypedSet();
  foreach var x in hset do
    Result.ht[x] := x;
end;}

///--
function TypedSet.CompareEquals(s: TypedSet): boolean;
begin
  Result := CompareSetEquals(Self, s);
end;

///--
function TypedSet.CompareInEquals(s: TypedSet): boolean;
begin
  Result := CompareSetInEquals(Self, s);
end;

///--
function TypedSet.CompareLess(s: TypedSet): boolean;
begin
  Result := CompareSetLess(Self, s);
end;

///--
function TypedSet.CompareLessEqual(s: TypedSet): boolean;
begin
  Result := CompareSetLessEqual(Self, s);
end;

///--
function TypedSet.CompareGreater(s: TypedSet): boolean;
begin
  Result := CompareSetGreater(Self, s);
end;

///--
function TypedSet.CompareGreaterEqual(s: TypedSet): boolean;
begin
  Result := CompareSetGreaterEqual(Self, s);
end;

// -----------------------------------------------------
//                  Typed Set functions
// -----------------------------------------------------
[System.Diagnostics.DebuggerStepThrough]
function CreateSet: TypedSet;
begin
  Result := new TypedSet();
end;

[System.Diagnostics.DebuggerStepThrough]
function CreateBoundedSet(low, high: object): TypedSet;
begin
  Result := new TypedSet(low, high);
end;

[System.Diagnostics.DebuggerStepThrough]
function CreateDiapason(low, high: integer): Diapason;
begin
  Result.low := low;
  Result.high := high;
end;

[System.Diagnostics.DebuggerStepThrough]
function CreateObjDiapason(low, high: object): Diapason;
begin
  Result.clow := low;
  Result.chigh := high;
end;

[System.Diagnostics.DebuggerStepThrough]
function CreateSet(params elems: array of object): TypedSet;
begin
  Result := new TypedSet();
  for var i := 0 to elems.Length - 1 do
    Result.IncludeElement(elems[i]);
end;

[System.Diagnostics.DebuggerStepThrough]
function Subtract(s1, s2: TypedSet): TypedSet;
var
  en: System.Collections.IEnumerator;
begin
  //Result := new TypedSet();
  Result := s1.CloneSet;
  {en := s1.ht.GetEnumerator();
  while en.MoveNext() = true do
  begin
  if not s2.Contains((en as IDictionaryEnumerator).Key) then 
  Result.ht[(en as IDictionaryEnumerator).Key] := (en as IDictionaryEnumerator).Key;
  end;}
  en := s2.ht.GetEnumerator();
  while en.MoveNext() = true do
  begin
    if s1.Contains((en as IDictionaryEnumerator).Key) then 
      Result.ht.Remove((en as IDictionaryEnumerator).Key);
  end;
end;

[System.Diagnostics.DebuggerStepThrough]
procedure Include(var s: TypedSet; el: object);
begin
  s.IncludeElement(el);
end;

[System.Diagnostics.DebuggerStepThrough]
procedure Exclude(var s: TypedSet; el: object);
begin
  s.ExcludeElement(el);
end;

[System.Diagnostics.DebuggerStepThrough]  
function Union(s1, s2: TypedSet): TypedSet;
var
  en: System.Collections.IEnumerator;
begin
  Result := s1.CloneSet;
  en := s2.ht.GetEnumerator();
  while en.MoveNext() = true do
    Result.ht[(en as IDictionaryEnumerator).Key] := (en as IDictionaryEnumerator).Key;
end;

[System.Diagnostics.DebuggerStepThrough]  
function Intersect(s1, s2: TypedSet): TypedSet;
var
  en: System.Collections.IEnumerator;
begin
  Result := new TypedSet();
  en := s1.ht.GetEnumerator();
  while en.MoveNext() = true do
    if s2.Contains((en as IDictionaryEnumerator).Key) then 
      Result.ht[(en as IDictionaryEnumerator).Key] := (en as IDictionaryEnumerator).Key;
end;

[System.Diagnostics.DebuggerStepThrough]  
function InSet(obj: object; s: TypedSet): boolean;
begin
  {if obj.GetType().IsEnum then 
  Result := s.ht[obj] <> nil
  else} 
  Result := (obj <> nil) and s.Contains(obj);
  {Result := (obj <> nil) and (s.ht[obj] <> nil);
  if not Result and (obj is TypedSet) then
  Result := s.Contains(obj as TypedSet);}
  //if Result = true then
  // Result := s.IsInDiapason(obj);
end;

[System.Diagnostics.DebuggerStepThrough]  
function CompareSetEquals(s1, s2: TypedSet): boolean;
var
  en: System.Collections.IEnumerator;
  equals: boolean := true;
begin
  if s1.ht.Count <> s2.ht.Count then
  begin
    Result := false;
    Exit;
  end;
  en := s1.ht.GetEnumerator();
  while en.MoveNext() = true do
  begin
    var is_in_s1 := s1.Contains((en as IDictionaryEnumerator).Key);
    var is_in_s2 := s2.Contains((en as IDictionaryEnumerator).Key);
    if is_in_s1 and not is_in_s2 then 
    begin
      equals := false;
      break;
    end
    else if not is_in_s1 and is_in_s2 then
    begin
      equals := false;
      break;
    end
  end;
  if equals <> false then 
  begin
    en := s2.ht.GetEnumerator();
    en.Reset();
    while en.MoveNext() = true do
    begin
      var is_in_s1 := s1.Contains((en as IDictionaryEnumerator).Key);
      var is_in_s2 := s2.Contains((en as IDictionaryEnumerator).Key);
      if is_in_s2 and not is_in_s1 then
      begin
        equals := false;
        break;
      end
      else if not is_in_s2 and is_in_s1 then
      begin
        equals := false;
        break;
      end
    end;
  end;
  Result := equals;
end;

[System.Diagnostics.DebuggerStepThrough]  
function CompareSetInEquals(s1, s2: TypedSet): boolean;
begin
  Result := not CompareSetEquals(s1, s2);
end;

[System.Diagnostics.DebuggerStepThrough]  
function CompareSetLess(s1, s2: TypedSet): boolean;
var
  en: System.Collections.IEnumerator;
  less: boolean := true;
begin
  en := s1.ht.GetEnumerator();
  en.Reset();
  while en.MoveNext() = true do
  begin
    if not s2.Contains((en as IDictionaryEnumerator).Key) then 
    begin
      less := false;
      break;
    end;
  end;
  if less <> false then
  begin
    en := s2.ht.GetEnumerator();
    en.Reset();
    var b: boolean := false;
    while en.MoveNext() = true do
    begin
      if not s1.Contains((en as IDictionaryEnumerator).Key) then 
      begin
        b := true;
        break;
      end;
    end;
    less := b;
  end;
  Result := less;
end;

[System.Diagnostics.DebuggerStepThrough]  
function CompareSetGreaterEqual(s1, s2: TypedSet): boolean;
var
  en: System.Collections.IEnumerator;
  greater: boolean := true;
begin
  en := s2.ht.GetEnumerator();
  en.Reset();
  while en.MoveNext() = true do
  begin
    if not s1.Contains((en as IDictionaryEnumerator).Key) then 
    begin
      greater := false;
      break;
    end;
  end;
  Result := greater;
end;

[System.Diagnostics.DebuggerStepThrough]  
function CompareSetLessEqual(s1, s2: TypedSet): boolean;
var
  en: System.Collections.IEnumerator;
  less: boolean := true;
begin
  en := s1.ht.GetEnumerator();
  en.Reset();
  while en.MoveNext() = true do
  begin
    if not s2.Contains((en as IDictionaryEnumerator).Key) then 
    begin
      less := false;
      break;
    end;
  end;
  Result := less;
end;

[System.Diagnostics.DebuggerStepThrough]  
function CompareSetGreater(s1, s2: TypedSet): boolean;
var
  greater: boolean := true;
  en: System.Collections.IEnumerator;
begin
  en := s2.ht.GetEnumerator();
  en.Reset();
  while en.MoveNext() = true do
  begin
    if not s1.Contains((en as IDictionaryEnumerator).Key) then 
    begin
      greater := false;
      break;
    end;
  end;
  if greater <> false then
  begin
    en := s1.ht.GetEnumerator();
    en.Reset();
    var b: boolean := false;
    while en.MoveNext() = true do
    begin
      if not s2.Contains((en as IDictionaryEnumerator).Key) then 
      begin
        b := true;
        break;
      end;
    end;
    greater := b;
  end;
  Result := greater;
end;

// -----------------------------------------------------
//                      TypedFile
// -----------------------------------------------------
constructor TypedFile.Create(ElementType: System.Type);
begin
  Self.ElementType := ElementType;
  ElementSize := RuntimeSizeOf(ElementType);
end;

constructor TypedFile.Create(ElementType: System.Type; offs: integer; params offsets: array of integer);
begin
  Self.ElementType := ElementType;
  ElementSize := RuntimeSizeOf(ElementType);
  Self.offsets := offsets;
  if offs <> 0 then
  begin
    ElementSize := ElementSize + offs{*2};
    offset := offs{*2};
  end;
end;

function TypedFile.ToString: string;
begin
  Result := string.Format('file of {0}', ElementType);
end;

// -----------------------------------------------------
//                      BinaryFile
// -----------------------------------------------------
function BinaryFile.ToString: string;
begin
  Result := 'file';
end;

// -----------------------------------------------------
//                  GCHandlersController    
// -----------------------------------------------------
constructor GCHandlersController.Create;
begin
  Counters := new Hashtable;
  Handlers := new Hashtable;
end;

procedure GCHandlersController.Add(obj: Object);
begin
  if obj <> nil then begin
    if Counters.Contains(obj) then
      Counters[obj] := integer(Counters[obj]) + 1
    else begin
      Counters.Add(obj, 1);          
      Handlers.Add(obj, GCHandle.Alloc(obj, GCHandleType.Pinned));
      //var ptr := Marshal.AllocHGlobal(Marshal.SizeOf(obj));
      //Marshal.StructureToPtr(obj, ptr, false);
      //Handlers.Add(obj, ptr);
      //GC.KeepAlive(obj);
      //var ptr:=IntPtr(pointer(@obj));
      //Handlers.Add(obj,new IntPtr(integer(ptr) or 1));
    end;
  end;
end;

procedure GCHandlersController.Remove(obj: Object);
begin
  if obj <> nil then begin
    if Counters.Contains(obj) then begin
      var Count := integer(Counters[obj]);
      if Count > 1 then 
        Counters[obj] := Count - 1
      else begin
        Counters.Remove(obj);
        GCHandle(Handlers[obj]).Free;
        Handlers.Remove(obj);
      end;
    end else
      raise new SystemException('PABCSystem.GCHandleForPointersController not contains object ' + obj.ToString);
  end;  
end;

function GCHandlersController.GetCounter(obj: Object): integer;
begin
  result := 0;
  if Counters.Contains(obj) then
    result := integer(Counters[obj]);      
end;

function GCHandlersController.GetEnumerator: System.Collections.IEnumerator;
begin
  result := Counters.Keys.GetEnumerator; 
end;

//------------------------------------------------------------------------------
//              StructuredObjectToString
//------------------------------------------------------------------------------

// Возвращает переопределенный в последнем потомке ToString или nil если ToString определен в Object
function RedefinedToString(o: object): System.Reflection.MethodInfo;
begin
  var t := o.GetType;
  var meth: System.Reflection.MethodInfo := nil;
  while t<>typeof(Object) do
  begin
    meth := t.GetMethod('ToString',System.Reflection.BindingFlags.Public or
                System.Reflection.BindingFlags.Instance or 
                System.Reflection.BindingFlags.DeclaredOnly,nil,new System.Type[0],nil);
    if meth<>nil then 
      break;
    t := t.BaseType;            
  end;
  if (t=typeof(Object)) or (t=typeof(System.ValueType)) then
    Result := nil
  else Result := meth;   
end;

function ArrNToString(a: System.Array; indexes: array of integer; i: integer): string; forward;

function StructuredObjectToString(o: Object; n: integer := 0): string;
const 
  nmax = 100;
  nmax1 = 30;
begin
  if o is System.Reflection.Pointer then
    Result := PointerToString(System.Reflection.Pointer.Unbox(o))
  else if o=nil then
    Result := 'nil' 
  else if (o.GetType = typeof(real)) or (o.GetType = typeof(decimal)) or (o.GetType = typeof(single)) then
    Result := FormatFloatNumber(o.ToString)
  else if (o.GetType = typeof(Complex)) then 
  begin
    var c := Complex(o);
    Result := '('+StructuredObjectToString(c.Real)+','+StructuredObjectToString(c.Imaginary)+')';
  end
  else if (o.GetType.IsPrimitive) or (o.GetType = typeof(string)) then
    Result := o.ToString
  else if o is System.Array then
  begin
    var a := o as System.Array;  
    Result := ArrNToString(a,new integer[a.Rank],0); 
  end
  else if o is System.Collections.IEnumerable then
  begin
    var sb := new StringBuilder();
    var g := (o as System.Collections.IEnumerable).GetEnumerator();
    
    var isdictorset := o.GetType.Name.Equals('Dictionary`2') or o.GetType.Name.Equals('SortedDictionary`2') or (o.GetType=typeof(TypedSet)) or o.GetType.Name.Equals('HashSet`1') or o.GetType.Name.Equals('SortedSet`1');
    if isdictorset then
      sb.Append('{')
    else sb.Append('[');
    if g.MoveNext() then
      sb.Append(StructuredObjectToString(g.Current,n+1));
    var cnt := 1;  
    while g.MoveNext() and (cnt<nmax) do 
    begin
      sb.Append(',');
      sb.Append(StructuredObjectToString(g.Current,n+1));
      cnt += 1;
    end;
    if cnt >= nmax then 
      sb.Append(',...');

    if isdictorset then
      sb.Append('}')
    else sb.Append(']');
    Result := sb.ToString;
  end
  else if o.GetType.GetField('NullBasedArray')<>nil then
  begin
    var f := o.GetType.GetField('NullBasedArray');
    Result := StructuredObjectToString(f.GetValue(o));
  end
  else
  begin
    var q := RedefinedToString(o);
    var gg := o.GetType.FullName.StartsWith('System.Tuple');
    var gg1 := o.GetType.Name.StartsWith('KeyValuePair');
    if (q<>nil) and q.IsVirtual and not gg and not gg1 then
      Result := o.ToString
    else 
    begin
      var t := o.GetType;
      var sb := new System.Text.StringBuilder();
      sb.Append('(');
      if n>nmax1 then
        sb.Append('....')
      else 
        while t<>typeof(object) do
        begin
          var ff := t.GetFields(System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Instance or System.Reflection.BindingFlags.DeclaredOnly);
          var pp := t.GetProperties(System.Reflection.BindingFlags.Public or System.Reflection.BindingFlags.Instance or System.Reflection.BindingFlags.DeclaredOnly);
  
          for var i:=ff.Length-1 downto 0 do
            sb.Insert(1,StructuredObjectToString(ff[i].GetValue(o),n+1)+',');
    
          for var i:=pp.Length-1 downto 0 do
            sb.Insert(1,StructuredObjectToString(pp[i].GetValue(o, nil),n+1)+',');
    
          t := t.BaseType;
        end; 
      if sb.Length>1 then 
        sb.Length := sb.Length-1;
      sb.Append(')');
      Result := sb.ToString;
    end;
  end;
end;

function ArrNToString(a: System.Array; indexes: array of integer; i: integer): string;
const nmax = 100;
begin
  var sb := new StringBuilder;
  if i=a.Rank then
    sb.Append(StructuredObjectToString(a.GetValue(indexes)))
  else
  begin
    sb.Append('[');
    for var k:=0 to a.GetLength(i)-1 do
    begin
      indexes[i] := k;
      sb.Append(ArrNToString(a,indexes,i+1));
      if (k>=nmax-1) and (k<a.GetLength(i)-1) then 
      begin
        sb.Append(',...');
        break
      end
      else if k<a.GetLength(i)-1 then
        sb.Append(',');            
    end;
    sb.Append(']');
  end;
  Result := sb.ToString;
end;

//------------------------------------------------------------------------------
//          Operations for String
//------------------------------------------------------------------------------
procedure string.operator+=(var left: string; right: string);
begin
  left := left + right;
end;

function string.operator<(left, right: string): boolean;
begin
  result := string.CompareOrdinal(left, right) < 0;
end;

function string.operator<=(left, right: string): boolean;
begin
  result := string.CompareOrdinal(left, right) <= 0;
end;

function string.operator>(left, right: string): boolean;
begin
  result := string.CompareOrdinal(left, right) > 0;
end;

function string.operator>=(left, right: string): boolean;
begin
  result := string.CompareOrdinal(left, right) >= 0;
end;

/// Повторяет строку str n раз
function string.operator*(str: string; n: integer): string;
begin
  var sb := new StringBuilder;
  for var i:=1 to n do
    sb.Append(str);
  result := sb.ToString;
end;

/// Повторяет строку str n раз
function string.operator*(n: integer; str: string): string;
begin
  var sb := new StringBuilder;
  for var i:=1 to n do
    sb.Append(str);
  result := sb.ToString;
end;

/// Повторяет символ c n раз
function char.operator*(c: char; n: integer): string;
begin
  var sb := new StringBuilder;
  for var i:=1 to n do
    sb.Append(c);
  result := sb.ToString;
end;

/// Повторяет символ c n раз
function char.operator*(n: integer; c: char): string;
begin
  var sb := new StringBuilder;
  for var i:=1 to n do
    sb.Append(c);
  result := sb.ToString;
end;

/// Добавляет к строке str строковое представление числа n
function string.operator+(str: string; n: integer): string;
begin
  result := str + n.ToString;
end;

/// Добавляет к строке str строковое представление числа n
function string.operator+(n: integer; str: string): string;
begin
  result := n.ToString + str;
end;

/// Добавляет к строке str строковое представление числа r
function string.operator+(str: string; r: real): string;
begin
  result := str + r.ToString(nfi);
end;

/// Добавляет к строке str строковое представление числа r
function string.operator+(r: real; str: string): string;
begin
  result := r.ToString(nfi) + str;
end;

procedure string.operator+=(var left: string; right: integer);
begin
  left := left + right.ToString;
end;

procedure string.operator+=(var left: string; right: real);
begin
  left := left + right.ToString(nfi);
end;

procedure string.operator*=(var left: string; n: integer);
begin
  var sb := new StringBuilder;
  for var i:=1 to n do
    sb.Append(left);
  left := sb.ToString;
end;

function string.operator in(substr: string; str: string): boolean;
begin
  Result := str.Contains(substr);
end;

//------------------------------------------------------------------------------
//          Операции для array of T
//------------------------------------------------------------------------------
/// Объединяет два массива
function operator+<T>(a, b: array of T): array of T; extensionmethod;
begin
  Result := new T[a.Length+b.Length];
  a.CopyTo(Result,0);
  b.CopyTo(Result,a.Length);
end;

///--
function operator in<T>(x: T; a: array of T): boolean; extensionmethod;
begin
  Result := a.Contains(x);
end;

function operator*<T>(a: array of T; n: integer): array of T; extensionmethod;
begin
  Result := new T[a.Length*n];
  for var i:=0 to n-1 do
    a.CopyTo(Result,a.Length*i);
end;

function operator*<T>(n: integer; a: array of T): array of T; extensionmethod;
begin
  Result := a*n
end;

//------------------------------------------------------------------------------
//          Операции для List<T> 
//------------------------------------------------------------------------------
///--
function operator+=<T>(a, b: List<T>): List<T>; extensionmethod;
begin
  a.AddRange(b);
  Result := a;
end;

///--
function operator+<T>(a, b: List<T>): List<T>; extensionmethod;
begin
  Result := new List<T>(a);
  Result.AddRange(b);
end;

///--
function operator+=<T>(a: List<T>; x: T): List<T>; extensionmethod;
begin
  a.Add(x);
  Result := a;
end;

///--
function List<T>.operator in(x: T; Self: List<T>): boolean;
begin
  Result := Self.Contains(x);
end;

///--
function operator*<T>(a: List<T>; n: integer): List<T>; extensionmethod;
begin
  Result := new List<T>();
  for var i := 1 to n do
    Result.AddRange(a);
end;

///--
function operator*<T>(n: integer; a: List<T>): List<T>; extensionmethod;
begin
  Result := a*n;
end;

//------------------------------------------------------------------------------
//          Операции для Stack<T> 
//------------------------------------------------------------------------------
///--
function operator+=<T>(s: Stack<T>; x: T): Stack<T>; extensionmethod;
begin
  s.Push(x);
  Result := s;
end;

//------------------------------------------------------------------------------
//          Операции для Queue<T> 
//------------------------------------------------------------------------------
///--
function operator+=<T>(q: Queue<T>; x: T): Queue<T>; extensionmethod;
begin
  q.Enqueue(x);
  Result := q;
end;

//------------------------------------------------------------------------------
//          Операции для HashSet<T> 
//------------------------------------------------------------------------------
///--
function operator in<T>(x: T; Self: HashSet<T>): boolean; extensionmethod;
begin
  Result := Self.Contains(x);
end;

function operator+=<T>(var Self: HashSet<T>; x: T): HashSet<T>; extensionmethod;
begin
  Self.Add(x);
  Result := Self;
end;

function operator+=<T>(var Self: HashSet<T>; x: sequence of T): HashSet<T>; extensionmethod;
begin
  Self.UnionWith(x);
  Result := Self;
end;

function operator-=<T>(var Self: HashSet<T>; x: T): HashSet<T>; extensionmethod;
begin
  Self.Remove(x);
  Result := Self;
end;

function operator-=<T>(var Self: HashSet<T>; x: sequence of T): HashSet<T>; extensionmethod;
begin
  Self.ExceptWith(x);
  Result := Self;
end;

function operator=<T>(x,y: HashSet<T>): boolean; extensionmethod;
begin
  Result := x.SetEquals(y)
end;

function operator<><T>(x,y: HashSet<T>): boolean; extensionmethod;
begin
  Result := not x.SetEquals(y)
end;

function operator-<T>(x,y: HashSet<T>): HashSet<T>; extensionmethod;
begin
  var v := new HashSet<T>(x);
  v.ExceptWith(y);
  Result := v;
end;

function operator+<T>(x,y: HashSet<T>): HashSet<T>; extensionmethod;
begin
  var v := new HashSet<T>(x);
  v.UnionWith(y);
  Result := v;
end;

function operator*<T>(x,y: HashSet<T>): HashSet<T>; extensionmethod;
begin
  var v := new HashSet<T>(x);
  v.IntersectWith(y);
  Result := v;
end;

function operator< <T>(x,y: HashSet<T>): boolean; extensionmethod;
begin
  Result := x.IsProperSubsetOf(y);
end;

function operator<= <T>(x,y: HashSet<T>): boolean; extensionmethod;
begin
  Result := x.IsSubsetOf(y);
end;

function operator> <T>(x,y: HashSet<T>): boolean; extensionmethod;
begin
  Result := x.IsProperSupersetOf(y);
end;

function operator>= <T>(x,y: HashSet<T>): boolean; extensionmethod;
begin
  Result := x.IsSupersetOf(y);
end;

//------------------------------------------------------------------------------
//          Операции для SortedSet<T> 
//------------------------------------------------------------------------------
function operator in<T>(x: T; Self: SortedSet<T>): boolean; extensionmethod;
begin
  Result := Self.Contains(x);
end;

function operator+=<T>(var Self: SortedSet<T>; x: T): SortedSet<T>; extensionmethod;
begin
  Self.Add(x);
  Result := Self;
end;

function operator+=<T>(var Self: SortedSet<T>; x: sequence of T): SortedSet<T>; extensionmethod;
begin
  Self.UnionWith(x);
  Result := Self;
end;

function operator-=<T>(var Self: SortedSet<T>; x: T): SortedSet<T>; extensionmethod;
begin
  Self.Remove(x);
  Result := Self;
end;

function operator-=<T>(var Self: SortedSet<T>; x: sequence of T): SortedSet<T>; extensionmethod;
begin
  Self.ExceptWith(x);
  Result := Self;
end;

function operator=<T>(x,y: SortedSet<T>): boolean; extensionmethod;
begin
  Result := x.SetEquals(y)
end;

function operator<><T>(x,y: SortedSet<T>): boolean; extensionmethod;
begin
  Result := not x.SetEquals(y)
end;

function operator-<T>(x,y: SortedSet<T>): SortedSet<T>; extensionmethod;
begin
  var v := new SortedSet<T>(x);
  v.ExceptWith(y);
  Result := v;
end;

function operator+<T>(x,y: SortedSet<T>): SortedSet<T>; extensionmethod;
begin
  var v := new SortedSet<T>(x);
  v.UnionWith(y);
  Result := v;
end;

function operator*<T>(x,y: SortedSet<T>): SortedSet<T>; extensionmethod;
begin
  var v := new SortedSet<T>(x);
  v.IntersectWith(y);
  Result := v;
end;

function operator< <T>(x,y: SortedSet<T>): boolean; extensionmethod;
begin
  Result := x.IsProperSubsetOf(y);
end;

function operator<= <T>(x,y: SortedSet<T>): boolean; extensionmethod;
begin
  Result := x.IsSubsetOf(y);
end;

function operator> <T>(x,y: SortedSet<T>): boolean; extensionmethod;
begin
  Result := x.IsProperSupersetOf(y);
end;

function operator>= <T>(x,y: SortedSet<T>): boolean; extensionmethod;
begin
  Result := x.IsSupersetOf(y);
end;

//------------------------------------------------------------------------------
//          Операции для Dictionary<K,V>, SortedDictionary<K,V>, SortedList<K,V>
//------------------------------------------------------------------------------
function Dictionary<K,V>.operator in(key: K; d: Dictionary<K,V>): boolean;
begin
  Result := d.ContainsKey(key);
end;

function SortedDictionary<K,V>.operator in(key: K; d: SortedDictionary<K,V>): boolean;
begin
  Result := d.ContainsKey(key);
end;

function SortedList<K,V>.operator in(key: K; d: SortedList<K,V>): boolean;
begin
  Result := d.ContainsKey(key);
end;

//------------------------------------------------------------------------------
//          Операции для BigInteger
//------------------------------------------------------------------------------
function BigInteger.operator/(p: BigInteger; q: real): real;
begin
  Result := real(p)/q;
end;

function BigInteger.operator/(q: real; p: BigInteger): real;
begin
  Result := q/real(p);
end;

function BigInteger.operator>(p: BigInteger; q: integer): boolean;
begin
  Result := p > BigInteger.Create(q);
end;

function BigInteger.operator>(p: integer; q: BigInteger): boolean;
begin
  Result := BigInteger.Create(p) > q;
end;

function BigInteger.operator<(p: BigInteger; q: integer): boolean;
begin
  Result := p < BigInteger.Create(q);
end;

function BigInteger.operator<(p: integer; q: BigInteger): boolean;
begin
  Result := BigInteger.Create(p) < q;
end;

function BigInteger.operator>=(p: BigInteger; q: integer): boolean;
begin
  Result := p >= BigInteger.Create(q);
end;

function BigInteger.operator>=(p: integer; q: BigInteger): boolean;
begin
  Result := BigInteger.Create(p) >= q;
end;

function BigInteger.operator<=(p: BigInteger; q: integer): boolean;
begin
  Result := p <= BigInteger.Create(q);
end;

function BigInteger.operator<=(p: integer; q: BigInteger): boolean;
begin
  Result := BigInteger.Create(p) <= q;
end;

function BigInteger.operator=(p: BigInteger; q: integer): boolean;
begin
  Result := p = BigInteger.Create(q);
end;

function BigInteger.operator=(p: integer; q: BigInteger): boolean;
begin
  Result := BigInteger.Create(p) = q;
end;

function BigInteger.operator<>(p: BigInteger; q: integer): boolean;
begin
  Result := p <> BigInteger.Create(q);
end;

function BigInteger.operator<>(p: integer; q: BigInteger): boolean;
begin
  Result := BigInteger.Create(p) <> q;
end;

procedure BigInteger.operator+=(var p: BigInteger; q: BigInteger);
begin
  p := p + q;
end;

procedure BigInteger.operator*=(var p: BigInteger; q: BigInteger);
begin
  p := p * q;
end;

procedure BigInteger.operator-=(var p: BigInteger; q: BigInteger);
begin
  p := p - q;
end;

function BigInteger.operator div(p,q: BigInteger): BigInteger;
begin
  Result := BigInteger.Divide(p,q);
end;

function BigInteger.operator mod(p,q: BigInteger): BigInteger;
begin
  Result := BigInteger.Remainder(p,q);
end;

function BigInteger.operator-(p: BigInteger): BigInteger;
begin
  Result := BigInteger.Negate(p)
end;

{function BigInteger.operator+(p: BigInteger): BigInteger;
begin
  Result := p
end;

function BigInteger.operator+(p,q: BigInteger): BigInteger;
begin
  Result := BigInteger.Add(p,q);
end;}

//------------------------------------------------------------------------------
//          Операции для Complex
//------------------------------------------------------------------------------
function operator-(Self: Complex): Complex; extensionmethod;
begin
  Result := Complex.Negate(Self);
end;

function operator implicit(c: (real,real)): Complex; extensionmethod;
begin
  Result := Cplx(c[0],c[1]);
end;

function operator implicit(c: (real,integer)): Complex; extensionmethod;
begin
  Result := Cplx(c[0],c[1]);
end;

function operator implicit(c: (integer,real)): Complex; extensionmethod;
begin
  Result := Cplx(c[0],c[1]);
end;

function operator implicit(c: (integer,integer)): Complex; extensionmethod;
begin
  Result := Cplx(c[0],c[1]);
end;

procedure operator+=(var c: Complex; x: Complex); extensionmethod;
begin
  c := c + x;
end;

procedure operator*=(var c: Complex; x: Complex); extensionmethod;
begin
  c := c * x;
end;

procedure operator-=(var c: Complex; x: Complex); extensionmethod;
begin
  c := c - x;
end;

procedure operator/=(var c: Complex; x: Complex); extensionmethod;
begin
  c := c / x;
end;

//------------------------------------------------------------------------------
//          Операции для sequence of T
//------------------------------------------------------------------------------
///--
function operator+<T>(a,b: sequence of T): sequence of T; extensionmethod;
begin
  Result := a.Concat(b);
end;

///--
function operator+<T>(a: sequence of T; b: T): sequence of T; extensionmethod;
begin
  Result := a.Concat(new T[1](b));
end;

///--
function operator+<T>(b: T; a: sequence of T): sequence of T; extensionmethod;
begin
  Result := new T[1](b);
  Result := Result.Concat(a);
end;

///--
function operator*<T>(a: sequence of T; n: integer): sequence of T; extensionmethod;
begin
  Result := System.Linq.Enumerable.Empty&<T>();
  for var i:=1 to n do
    Result := Result.Concat(a);
end;

///--
function operator*<T>(n: integer; a: sequence of T): sequence of T; extensionmethod;
begin
  Result := a*n;
end;

///--
function operator in<T>(x: T; Self: sequence of T): boolean; extensionmethod;
begin
  Result := Self.Contains(x);
end;
// -----------------------------------------------------------------------------
//                Функции для последовательностей и динамических массивов
// -----------------------------------------------------------------------------

type
// Вспомогательный класс для генерации всех последовательностей
  SeqBase<T> = class(IEnumerable<T>,IEnumerator<T>)
  public
    function System.Collections.IEnumerable.GetEnumerator(): System.Collections.IEnumerator;
    begin
      Result := Self;
    end;

    function GetEnumerator(): IEnumerator<T>;
    begin
      Result := Self;
    end;

    function get_Current: T; virtual;
    begin
      Result := default(T);
    end;

    function System.Collections.IEnumerator.get_Current(): object;
    begin
      Result := Self.get_Current();
    end;

    function MoveNext(): boolean; virtual; 
    begin
      Result := True;
    end;

    procedure Dispose(); virtual;
    begin
    end;

    procedure Reset();
    begin
    end;
  end;
  
// Вспомогательный класс для генерации целых последовательностей
  SeqBaseInteger = class(IEnumerable<integer>,IEnumerator<integer>)
  public
    function System.Collections.IEnumerable.GetEnumerator(): System.Collections.IEnumerator;
    begin
      Result := Self;
    end;

    function GetEnumerator(): IEnumerator<integer>;
    begin
      Result := Self;
    end;

    function get_Current: integer; virtual;
    begin
      Result := 0;
    end;

    function System.Collections.IEnumerator.get_Current(): object;
    begin
      Result := Self.get_Current();
    end;

    function MoveNext(): boolean; virtual; 
    begin
      Result := True;
    end;

    procedure Dispose(); virtual;
    begin
    end;

    procedure Reset();
    begin
    end;
  end;
  
function Range(a, b: integer): sequence of integer;
begin
  if b<a then 
    Result := System.Linq.Enumerable.Empty&<integer>
  else Result := System.Linq.Enumerable.Range(a, b - a + 1);
end;

function Range(c1,c2: char): sequence of char;
begin
  Result := Range(integer(c1),integer(c2)).Select(x->Chr(x));
end;

function Range(a,b: real; n: integer): sequence of real;
begin
  if n=0 then
    raise new System.ArgumentException('n=0');
  if n<0 then
    raise new System.ArgumentException('n<0');
  var r := a;
  var h := (b-a)/n;
  for var i := 0 to n do
  begin
    yield r;
    r += h
  end;
end;

function Partition(a,b: real; n: integer): sequence of real;
begin
  Result := Range(a,b,n)
end;

type
  ArithmSeq = auto class
    a,step: integer;
    function f(x: integer): integer;
    begin
      Result := x*step+a;
    end;
  end;

function Range(a, b, step: integer): sequence of integer;
begin
  if step=0 then
    raise new System.ArgumentException('step=0');
  if (step>0) and (b<a) or (step<0) and (b>a) then
  begin
    Result := System.Linq.Enumerable.Empty&<integer>;
    exit;
  end;
  var n := abs((b-a) div step) + 1;
  var ar: ArithmSeq;
  {if step<0 then
    ar := new ArithmSeq(b,step)
  else} ar := new ArithmSeq(a,step);
  Result := System.Linq.Enumerable.Range(0, n).Select(ar.f);
end;

function ArrRandom(n: integer; a: integer; b: integer): array of integer; 
begin
  Result := new integer[n];
  for var i:=0 to Result.Length-1 do
    Result[i] := Random(a,b);
end;

function ArrRandomInteger(n: integer; a: integer; b: integer): array of integer;
begin
  Result := ArrRandom(n,a,b);
end;

function ArrRandomReal(n: integer; a: real; b: real): array of real;
begin
  Result := new real[n];
  for var i:=0 to Result.Length-1 do
    Result[i] := Random()*(b-a)+a;
end;

function SeqRandom(n: integer; a: integer; b: integer): sequence of integer;
begin
  for var i:=1 to n do
    yield Random(a,b)
end;

function SeqRandomInteger(n: integer; a: integer; b: integer): sequence of integer;
begin
  for var i:=1 to n do
    yield Random(a,b)
end;

function SeqRandomReal(n: integer; a: real; b: real): sequence of real;
begin
  for var i:=1 to n do
    yield Random()*(b-a)+a
end;

function Arr<T>(params a: array of T): array of T;
begin
  Result := new T[a.Length];
  System.Array.Copy(a,Result,a.Length);
end;

function Arr<T>(a: sequence of T): array of T;
begin
  Result := a.ToArray;
end;

function Seq<T>(params a: array of T): sequence of T;
begin
  var res := new T[a.Length];
  System.Array.Copy(a,res,a.Length);
  Result := res;
end;

/// Возвращает бесконечную рекуррентную последовательность элементов, задаваемую начальным элементом first и функцией next
function Iterate<T>(first: T; next: T->T): sequence of T;
begin
  yield first;
  while True do
  begin
    first := next(first);
    yield first;
  end;
  //Result := IterateClass&<T>.Create(first,next).Select(x->T(x));
end;

/// Возвращает бесконечную рекуррентную последовательность элементов, задаваемую начальными элементами first, second и функцией next
function Iterate<T>(first,second: T; next: (T,T)->T): sequence of T;
begin
  yield first;
  yield second;
  while True do
  begin
    var nxt := next(first,second);
    yield nxt;
    first := second;
    second := nxt;
  end;
//  Result := Iterate2Class&<T>.Create(first,second,next).Select(x->T(x));
end;

function SeqGen<T>(count: integer; first: T; next: T -> T): sequence of T;
begin
  if count<1 then
    raise new System.ArgumentOutOfRangeException('count',count,GetTranslation(PARAMETER_COUNT_MUST_BE_GREATER_0));
  Result := Iterate(first,next).Take(count);
end;  

function SeqGen<T>(count: integer; first,second: T; next: (T,T) -> T): sequence of T;
begin
  if count<1 then
    raise new System.ArgumentOutOfRangeException('count',count,GetTranslation(PARAMETER_COUNT_MUST_BE_GREATER_0));
  Result := Iterate(first,second,next).Take(count);
end;  

function SeqWhile<T>(first: T; next: T -> T; pred: T -> boolean): sequence of T;
begin
  Result := Iterate(first,next).TakeWhile(pred);
end;  

function SeqWhile<T>(first,second: T; next: (T,T) -> T; pred: T -> boolean): sequence of T;
begin
  Result := Iterate(first,second,next).TakeWhile(pred);
end;

function ArrGen<T>(count: integer; first: T; next: T -> T): array of T;
begin
  if count<1 then
    raise new System.ArgumentOutOfRangeException('count',count,GetTranslation(PARAMETER_COUNT_MUST_BE_GREATER_0));
  var a := new T[count];
  a[0] := first;
  for var i:=1 to a.Length-1 do
    a[i] := next(a[i-1]);
  Result := a;
end;

function ArrGen<T>(count: integer; first,second: T; next: (T,T) -> T): array of T;
begin
  if count<2 then
    raise new System.ArgumentOutOfRangeException('count',count,GetTranslation(PARAMETER_COUNT_MUST_BE_GREATER_1));
  var a := new T[count];
  a[0] := first;
  a[1] := second;
  for var i:=2 to a.Length-1 do
    a[i] := next(a[i-2],a[i-1]);
  Result := a;
end;

{function ListWhile<T>(first: T; next: Func<T,T>; pred: Predicate<T>): List<T>;
begin
  var a := new List<T>;
  var x := first;
  while pred(x) do
  begin
    a.Add(x);
    x := next(x);
  end;
  Result := a;
end;

function ListWhile<T>(first,second: T; next: Func2<T,T,T>; pred: Predicate<T>): List<T>;
begin
  var a := new List<T>;
  var x := first;
  var y := second;
  while pred(x) do
  begin
    a.Add(x);
    var z := next(x,y);
    x := y;
    y := z;
  end;
  Result := a;
end;}

function ArrFill<T>(count: integer; x: T): array of T;
begin
  Result := new T[count];
  for var i:=0 to Result.Length-1 do
    Result[i] := x;
end;

function ArrGen<T>(count: integer; gen: integer -> T; from: integer): array of T;
begin
  Result := new T[count];
  for var i:=0 to Result.Length-1 do
    Result[i] := gen(i+from);
end;

function ArrGen<T>(count: integer; gen: integer -> T): array of T;
begin
  Result := new T[count];
  for var i:=0 to Result.Length-1 do
    Result[i] := gen(i);
end;

function SeqFill<T>(count: integer; x: T): sequence of T;
begin
  Result := System.Linq.Enumerable.Repeat(x,count);
end;

function SeqGen<T>(count: integer; f: integer -> T; from: integer): sequence of T;
begin
  Result := Range(from,count+from-1).Select(f)
end;

function SeqGen<T>(count: integer; f: integer -> T): sequence of T;
begin
  Result := Range(0,count-1).Select(f)
end;

function ReadArrInteger(n: integer): array of integer;
begin
  Result := new integer[n];
  for var i:=0 to Result.Length-1 do
    Result[i] := ReadInteger;
end;

function ReadArrInteger(const prompt: string; n: integer): array of integer;
begin
  Print(prompt);
  Result := ReadArrInteger(n);
end;

function ReadArrReal(n: integer): array of real;
begin
  Result := new real[n];
  for var i:=0 to Result.Length-1 do
    Result[i] := ReadReal;
end;

function ReadArrReal(const prompt: string; n: integer): array of real;
begin
  Print(prompt);
  Result := ReadArrReal(n);
end;

function ReadArrString(n: integer): array of string;
begin
  Result := new string[n];
  for var i:=0 to Result.Length-1 do
    Result[i] := ReadString;
end;

function ReadArrString(const prompt: string; n: integer): array of string;
begin
  Print(prompt);
  Result := ReadArrString(n);
end;

function ReadSeqInteger(n: integer): sequence of integer;
begin
  Result := Range(1,n).Select(i->ReadInteger());
end;

function ReadSeqInteger(const prompt: string; n: integer): sequence of integer;
begin
  Print(prompt);
  Result := ReadSeqInteger(n);
end;

function ReadSeqReal(n: integer): sequence of real;
begin
  Result := Range(1,n).Select(i->ReadReal());
end;

function ReadSeqReal(const prompt: string; n: integer): sequence of real;
begin
  Print(prompt);
  Result := ReadSeqReal(n);
end;

function ReadSeqString(n: integer): sequence of string;
begin
  Result := Range(1,n).Select(i->ReadString());
end;

function ReadSeqString(const prompt: string; n: integer): sequence of string;
begin
  Print(prompt);
  Result := ReadSeqString(n);
end;

function ReadSeqIntegerWhile(cond: integer -> boolean): sequence of integer;
begin
  while True do
  begin
    var x := ReadInteger();
    if not cond(x) then
      break;
    yield x;  
  end;
end;

function ReadSeqRealWhile(cond: real -> boolean): sequence of real;
begin
  while True do
  begin
    var x := ReadReal();
    if not cond(x) then
      break;
    yield x;  
  end;
end;

function ReadSeqStringWhile(cond: string -> boolean): sequence of string;
begin
  while True do
  begin
    var x := ReadString();
    if not cond(x) then
      break;
    yield x;  
  end;
end;

function ReadSeqIntegerWhile(const prompt: string; cond: integer -> boolean): sequence of integer;
begin
  Print(prompt);
  Result := ReadSeqIntegerWhile(cond);
end;

function ReadSeqRealWhile(const prompt: string; cond: real -> boolean): sequence of real;
begin
  Print(prompt);
  Result := ReadSeqRealWhile(cond);
end;

function ReadSeqStringWhile(const prompt: string; cond: string -> boolean): sequence of string;
begin
  Print(prompt);
  Result := ReadSeqStringWhile(cond);
end;

// -----------------------------------------------------------------------------
//                Функции Rec для создания кортежей
// -----------------------------------------------------------------------------
function Rec<T1,T2>(x1: T1; x2: T2): System.Tuple<T1,T2>;
begin
  Result := Tuple.Create(x1,x2);
end;

function Rec<T1,T2,T3>(x1: T1; x2: T2; x3: T3): (T1,T2,T3);
begin
  Result := Tuple.Create(x1,x2,x3);
end;

function Rec<T1,T2,T3,T4>(x1: T1; x2: T2; x3: T3; x4: T4): (T1,T2,T3,T4);
begin
  Result := Tuple.Create(x1,x2,x3,x4);
end;

function Rec<T1,T2,T3,T4,T5>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5): (T1,T2,T3,T4,T5);
begin
  Result := Tuple.Create(x1,x2,x3,x4,x5);
end;

function Rec<T1,T2,T3,T4,T5,T6>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5; x6: T6): (T1,T2,T3,T4,T5,T6);
begin
  Result := Tuple.Create(x1,x2,x3,x4,x5,x6);
end;

function Rec<T1,T2,T3,T4,T5,T6,T7>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5; x6: T6; x7: T7): (T1,T2,T3,T4,T5,T6,T7);
begin
  Result := Tuple.Create(x1,x2,x3,x4,x5,x6,x7);
end;

// -----------------------------------------------------------------------------
//                Функции Lst, LLst, Dict, KV, HSet, SSet
// -----------------------------------------------------------------------------
function Lst<T>(params a: array of T): List<T>;
begin
  Result := new List<T>(a);
end;

function Lst<T>(a: sequence of T): List<T>;
begin
  Result := new List<T>(a);
end;

function LLst<T>(params a: array of T): LinkedList<T>;
begin
  Result := new LinkedList<T>(a);
end;

function LLst<T>(a: sequence of T): LinkedList<T>;
begin
  Result := new LinkedList<T>(a);
end;

function HSet<T>(params a: array of T): HashSet<T>;
begin
  Result := new HashSet<T>(a);
end;

function SSet<T>(params a: array of T): SortedSet<T>;
begin
  Result := new SortedSet<T>(a);
end;

function HSet<T>(a: sequence of T): HashSet<T>;
begin
  Result := new HashSet<T>(a);
end;

function SSet<T>(a: sequence of T): SortedSet<T>;
begin
  Result := new SortedSet<T>(a);
end;

function Dict<TKey, TVal>(params pairs: array of KeyValuePair<TKey, TVal>): Dictionary<TKey, TVal>;
begin
  Result := new Dictionary<TKey, TVal>();
  for var i := 0 to pairs.Length - 1 do
    Result.Add(pairs[i].Key, pairs[i].Value);
end;

function Dict<TKey, TVal>(params pairs: array of (TKey, TVal)): Dictionary<TKey, TVal>;
begin
  Result := new Dictionary<TKey, TVal>();
  for var i := 0 to pairs.Length - 1 do
    Result.Add(pairs[i][0], pairs[i][1]);
end;

function KV<TKey, TVal>(key: TKey; value: TVal): KeyValuePair<TKey, TVal>;
begin
  Result := new KeyValuePair<TKey, TVal>(key, value);
end;

{function read_lexem: string; 
var
  c: char;
  sb: System.Text.StringBuilder;
begin
  repeat
    c := CurrentIOSystem.read_symbol;
  until not char.IsWhiteSpace(c); // pass spaces
  sb := new System.Text.StringBuilder;
  repeat
    sb.Append(c);
    c := CurrentIOSystem.read_symbol;
  until char.IsWhiteSpace(c) or (c = char(-1)); // accumulate nonspaces
  Result := sb.ToString;
end;}

// -----------------------------------------------------------------------------
//                read_lexem
// -----------------------------------------------------------------------------
function read_lexem: string;// SSM 08.03.11 - пытаюсь исправить с peekом ситуацию с вводом '1 hello'. Должно работать
var
  c: char;
  sb: System.Text.StringBuilder;
begin
  repeat
    c := CurrentIOSystem.read_symbol;
  until not char.IsWhiteSpace(c);
  sb := new System.Text.StringBuilder;
  repeat
    sb.Append(c);
    c := char(CurrentIOSystem.peek);
    if char.IsWhiteSpace(c) or (c = char(-1)) then // char(-1) - Ctrl-Z во входном потоке
      break;
    c := CurrentIOSystem.read_symbol;
  until False; // accumulate nonspaces
  Result := sb.ToString;
end;

function read_lexem(f: Text): string;
var
  c: char;
  i: integer;
  sb: System.Text.StringBuilder;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_READING));
  repeat
    i := f.sr.Read();
  until not char.IsWhiteSpace(char(i)); // pass spaces
  c := char(i);
  sb := System.Text.StringBuilder.Create;
  repeat
    sb.Append(c);
    i := f.sr.Peek();
    if i = -1 then
      break;
    c := char(i);
    if char.IsWhiteSpace(c) then
      break;
    f.sr.Read();
  until False; // accumulate nonspaces
  Result := sb.ToString;
end;

// -----------------------------------------------------
//          IOStandardSystem: implementation
// -----------------------------------------------------
function IOStandardSystem.peek: integer;
begin
  if not console_alloc then
    AllocConsole;
// SSM 29.11.14  
  if state = 1 then // в sym - символ, считанный предыдущим Peek
    Result := sym
  else // в sym ничего нет
  begin 
    state := 1;
    sym := Console.Read(); // считываение в буфер
    Result := sym;
  end;   
end;

function IOStandardSystem.read_symbol: char;
begin
  if not console_alloc then
    AllocConsole;
// SSM 29.11.14  
  if state = 1 then // в sym - символ, считанный предыдущим Peek
  begin
    state := 0;
    Result := char(sym);
    sym := -1;
  end
  else // в sym ничего нет
    Result := char(Console.Read());
end;

procedure IOStandardSystem.read(var x: integer);
begin
  x := Convert.ToInt32(read_lexem);
end;

procedure IOStandardSystem.read(var x: real);
begin
  x := Convert.ToDouble(read_lexem, nfi);
end;

procedure IOStandardSystem.read(var x: char);
begin
  x := CurrentIOSystem.read_symbol;
end;

{procedure IOStandardSystem.read(var x: string);
begin
  var sb := new System.Text.StringBuilder;
  var c := read_symbol;
  while c <> #13 do 
  begin
    sb.Append(c);
    c := read_symbol;
  end;
  x := sb.ToString;
end;}

procedure IOStandardSystem.read(var x: string);
begin
  if IsWDE then
  begin
    var sb := new System.Text.StringBuilder;
    var c := read_symbol;
    if (c <> #13) and (c <> #10) then
        sb.Append(c);
    while (c <> #13) and (c <> #10) do
    begin
        c := read_symbol;
        sb.Append(c);
        c := char(peek());
    end;
    x := sb.ToString;
  end
  else
  begin
    var sb := new System.Text.StringBuilder;
    // SSM 8.04.10
    var c := char(peek()); // первый раз может быть char(-1) - это значит, что в потоке ввода ничего нет, тогда мы читаем символ
    while (c <> #13) and (c <> #10) do 
    begin
      c := read_symbol;
      if (c <> #13) and (c <> #10) then // SSM 13.12.13
        sb.Append(c);
      c := char(peek());
    end;
    x := sb.ToString;
  end;
end;

procedure IOStandardSystem.read(var x: byte);
begin
  x := Convert.ToByte(read_lexem);
end;

procedure IOStandardSystem.read(var x: shortint);
begin
  x := Convert.ToSByte(read_lexem);
end;

procedure IOStandardSystem.read(var x: smallint);
begin
  x := Convert.ToInt16(read_lexem);
end;

procedure IOStandardSystem.read(var x: word);
begin
  x := Convert.ToUInt16(read_lexem);
end;

procedure IOStandardSystem.read(var x: longword);
begin
  x := Convert.ToUInt32(read_lexem);
end;

procedure IOStandardSystem.read(var x: int64);
begin
  x := Convert.ToInt64(read_lexem);
end;

procedure IOStandardSystem.read(var x: uint64);
begin
  x := Convert.ToUInt64(read_lexem);
end;

procedure IOStandardSystem.read(var x: single);
begin
  x := Convert.ToSingle(read_lexem, nfi);
end;

procedure IOStandardSystem.read(var x: boolean);
begin
  var s := read_lexem.ToLower;
  if s = 'true' then
    x := True
  else if s = 'false' then
    x := False  
  else raise new System.FormatException('Входная строка имела неверный формат');
end;

procedure IOStandardSystem.readln;
begin
  while CurrentIOSystem.read_symbol <> END_OF_LINE_SYMBOL do;
end;

procedure IOStandardSystem.write(obj: object);
begin
  if not console_alloc then
    AllocConsole;
  Console.Write(StructuredObjectToString(obj));  
end;

procedure IOStandardSystem.write(p: pointer);
begin
  Write(PointerToString(p));
end;

procedure IOStandardSystem.writeln;
begin
  if not console_alloc then
    AllocConsole;
  Console.WriteLine;
  System.Diagnostics.Debug.WriteLine('');
end;

function _ObjectToString(o: object): string;
begin
  Result := StructuredObjectToString(o);
end;

// -----------------------------------------------------
//     Read - Readln
// -----------------------------------------------------
procedure read;
begin
end;

procedure readln;
begin
  if input.sr <> nil then
    input.sr.ReadLine
  else 
    try
      CurrentIOSystem.readln
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: integer);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: real);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: char);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: string);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: byte);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: shortint);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: smallint);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: word);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: longword);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: int64);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: uint64);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: single);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

procedure read(var x: boolean);
begin
  if input.sr <> nil then
    read(input, x)
  else 
    try
      CurrentIOSystem.read(x)
    except
      on e: Exception do
        raise e;
    end;
end;

function ReadInteger: integer;
begin
  var x: integer;
  read(x);
  Result := x;
end;

function ReadReal: real;
begin
  var x: real;
  read(x);
  Result := x;
end;

function ReadChar: char;
begin
  var x: char;
  read(x);
  Result := x;
end;

function ReadString: string;
begin
  var x: string;
  read(x);
  readln();
  Result := x;
end;

function ReadBoolean: boolean;
begin
  var x: boolean;
  read(x);
  Result := x;
end;

function ReadlnInteger: integer;
begin
  Result := ReadInteger;
  readln();
end;

function ReadlnReal: real;
begin
  Result := ReadReal;
  readln();
end;

function ReadlnChar: char;
begin
  Result := ReadChar;
  readln();
end;

function ReadlnString: string;
begin
  Result := ReadString;
end;

function ReadlnBoolean: boolean;
begin
  Result := ReadBoolean;
  readln();
end;

// Read with prompt

function ReadInteger(prompt: string): integer;
begin
  Print(prompt);
  Result := ReadInteger;
end;

function ReadReal(prompt: string): real;
begin
  Print(prompt);
  Result := ReadReal;
end;

function ReadChar(prompt: string): char;
begin
  Print(prompt);
  Result := ReadChar;
end;

function ReadString(prompt: string): string;
begin
  Print(prompt);
  Result := ReadString;
end;

function ReadBoolean(prompt: string): boolean;
begin
  Print(prompt);
  Result := ReadBoolean;
end;

function ReadlnInteger(prompt: string): integer;
begin
  Print(prompt);
  Result := ReadlnInteger;
end;

function ReadlnReal(prompt: string): real;
begin
  Print(prompt);
  Result := ReadlnReal;
end;

function ReadlnChar(prompt: string): char;
begin
  Print(prompt);
  Result := ReadlnChar;
end;

function ReadlnString(prompt: string): string;
begin
  Print(prompt);
  Result := ReadlnString;
end;

function ReadlnBoolean(prompt: string): boolean;
begin
  Print(prompt);
  Result := ReadlnBoolean;
end;

procedure ReadShortStringFromFile(f: Text; var s: string; n: integer);
begin
  //x := f.sr.ReadLine;//если конец файла то вернет nil  
  //Нельзя эти пользоваться т.к. считывает и конец строки
  var i := 1;
  var sb := new System.Text.StringBuilder;
  repeat
    if f.sr.EndOfStream then
      break;
    if f.sr.Peek = 13 then
      break;
    if f.sr.Peek = 10 then
      break;
    if i > n then break;
    sb.Append(Convert.ToChar(f.sr.Read));
    i := i + 1;
  until False;
  s := sb.ToString; {}
  if s = nil then 
    s := string.Empty;
  if s.Length > n then s := s.Substring(0, n);
end;

procedure ReadShortString(var s: string; n: integer);// Снова сделал peek. В прошлый раз была ошибка
begin
  if (input.fi <> nil) and (input.sr <> nil) then
  begin
    ReadShortStringFromFile(input, s, n);
    exit;
  end;
  
  {  var sb := new System.Text.StringBuilder;
    var c := CurrentIOSystem.read_symbol;
    while c <> #13 do 
    begin
      sb.Append(c);
      c := CurrentIOSystem.read_symbol;
    end;
    s := sb.ToString;
    if s.Length > n then s := s.Substring(0, n);
  }  
  
  // SSM 8.04.10
  var sb := new System.Text.StringBuilder;
  var c := char(CurrentIOSystem.peek());
  var i := 0;
  while (c <> #13) and (c <> #10) and (i < n) do 
  begin
    c := CurrentIOSystem.read_symbol;
    i += 1;
    sb.Append(c);
    c := char(CurrentIOSystem.peek());
  end;
  s := sb.ToString;
end;

//--------------------------------
procedure read(f: Text);
begin
end;

procedure readln(f: Text);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_READING));
  f.sr.ReadLine;
end;

procedure read(f: Text; var x: integer);
begin
  try
    x := Convert.ToInt32(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: real);
begin
  try
    x := Convert.ToDouble(read_lexem(f), nfi);
  except
    on e: Exception do
      raise e;
  end;
end;

procedure readln(f: Text; var x: string);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_READING));

  x := f.sr.ReadLine;
  if x = nil then
    x := '';
end;

procedure read(f: Text; var x: string);
begin
  //x := f.sr.ReadLine;//если конец файла то вернет nil  
  //Нельзя этим пользоваться т.к. считывает и конец строки
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_READING));
  
  var s := new System.Text.StringBuilder;
  repeat
    if f.sr.EndOfStream then
      break;
    if f.sr.Peek = 13 then
      break;
    if f.sr.Peek = 10 then
      break;
    s.Append(Convert.ToChar(f.sr.Read));
  until False;
  x := s.ToString; {}
  if x = nil then 
    x := string.Empty; 
end;

procedure read(f: Text; var x: char);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_READING));
  try
    x := Convert.ToChar(f.sr.Read());
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: byte);
begin
  try
    x := Convert.ToByte(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: shortint);
begin
  try
    x := Convert.ToSByte(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: smallint);
begin
  try
    x := Convert.ToInt16(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: word);
begin
  try
    x := Convert.ToUInt16(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: longword);
begin
  try
    x := Convert.ToUInt32(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: int64);
begin
  try
    x := Convert.ToInt64(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: uint64);
begin
  try
    x := Convert.ToUInt64(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: single);
begin
  try
    x := Convert.ToSingle(read_lexem(f));
  except
    on e: Exception do
      raise e;
  end;
end;

procedure read(f: Text; var x: boolean);
begin
  var s := read_lexem(f).ToLower;
  if s = 'true' then
    x := True
  else if s = 'false' then
    x := False  
  else raise new System.FormatException('Входная строка имела неверный формат');
end;

function ReadInteger(f: Text): integer;
begin
  var x: integer;
  read(f, x);
  Result := x;
end;

function ReadReal(f: Text): real;
begin
  var x: real;
  read(f, x);
  Result := x;
end;

function ReadChar(f: Text): char;
begin
  var x: char;
  read(f, x);
  Result := x;
end;

function ReadString(f: Text): string;
begin
  var x: string;
  read(f, x);
  readln(f);
  Result := x;
end;

function ReadBoolean(f: Text): boolean;
begin
  var x: boolean;
  read(f, x);
  Result := x;
end;

function ReadlnInteger(f: Text): integer;
begin
  Result := ReadInteger(f);
  readln(f);
end;

function ReadlnReal(f: Text): real;
begin
  Result := ReadReal(f);
  readln(f);
end;

function ReadlnChar(f: Text): char;
begin
  Result := ReadChar(f);
  readln(f);
end;

function ReadlnString(f: Text): string;
begin
  Result := ReadString(f);
end;

function ReadlnBoolean(f: Text): boolean;
begin
  Result := ReadBoolean(f);
  readln(f);
end;
// -----------------------------------------------------
//                   TextFile methods
// -----------------------------------------------------
function Text.ReadInteger: integer;
begin
  Result := PABCSystem.ReadInteger(Self);
end;

function Text.ReadReal: real;
begin
  Result := PABCSystem.ReadReal(Self);
end;

function Text.ReadChar: char;
begin
  Result := PABCSystem.ReadChar(Self);
end;

function Text.ReadString: string;
begin
  Result := PABCSystem.ReadString(Self);
end;

function Text.ReadBoolean: boolean;
begin
  Result := PABCSystem.ReadBoolean(Self);
end;

function Text.ReadlnInteger: integer;
begin
  Result := PABCSystem.ReadlnInteger(Self);
end;

function Text.ReadlnReal: real;
begin
  Result := PABCSystem.ReadlnReal(Self);
end;

function Text.ReadlnChar: char;
begin
  Result := PABCSystem.ReadlnChar(Self);
end;

function Text.ReadlnString: string;
begin
  Result := PABCSystem.ReadlnString(Self);
end;

function Text.ReadlnBoolean: boolean;
begin
  Result := PABCSystem.ReadlnBoolean(Self);
end;

procedure Text.Write(params o: array of Object);
begin
  PABCSystem.Write(Self,o);
end;

procedure Text.Writeln(params o: array of Object);
begin
  PABCSystem.Writeln(Self,o);
end;

function Text.Eof: boolean;
begin
  Result := PABCSystem.Eof(Self);
end;

function Text.Eoln: boolean;
begin
  Result := PABCSystem.Eoln(Self);
end;

procedure Text.Close;
begin
  PABCSystem.Close(Self);
end;

function Text.SeekEof: boolean;
begin
  Result := PABCSystem.SeekEof(Self);
end;

function Text.SeekEoln: boolean;
begin
  Result := PABCSystem.SeekEoln(Self);
end;

procedure Text.Flush;
begin
  PABCSystem.Flush(Self);
end;

procedure Text.Erase;
begin
  PABCSystem.Erase(Self);
end;

procedure Text.Rename(newname: string);
begin
  PABCSystem.Rename(Self, newname);
end;

function Text.Name: string;
begin
  Result := fi.Name  
end;

function Text.FullName: string;
begin
  Result := fi.FullName  
end;

function Text.ReadToEnd: string;
begin
  Result := sr.ReadToEnd  
end;

procedure Text.Reset;
begin
  PABCSystem.Reset(Self);
end;


// -----------------------------------------------------
//                AbstractBinaryFile methods
// -----------------------------------------------------
procedure AbstractBinaryFile.Close;
begin
  PABCSystem.Close(Self);
end;

procedure AbstractBinaryFile.Truncate;
begin
  PABCSystem.Truncate(Self);
end;

function AbstractBinaryFile.Eof: boolean;
begin
  Result := PABCSystem.Eof(Self);
end;

procedure AbstractBinaryFile.Erase;
begin
  PABCSystem.Erase(Self);
end;

procedure AbstractBinaryFile.Rename(newname: string);
begin
  PABCSystem.Rename(Self, newname);
end;

procedure AbstractBinaryFile.Write(params vals: array of object);
begin
  PABCSystem.Write(Self, vals);
end;

procedure AbstractBinaryFile.Reset;
begin
  PABCSystem.Reset(Self);
end;

// -----------------------------------------------------
//                TypedFile & BinaryFile methods
// -----------------------------------------------------
function TypedFile.FilePos: int64;
begin
  Result := PABCSystem.FilePos(Self);
end;

function TypedFile.FileSize: int64;
begin
  Result := PABCSystem.FileSize(Self);
end;

procedure TypedFile.Seek(n: int64);
begin
  PABCSystem.Seek(Self, n);
end;

function BinaryFile.FilePos: int64;
begin
  Result := PABCSystem.FilePos(Self);
end;

function BinaryFile.FileSize: int64;
begin
  Result := PABCSystem.FileSize(Self);
end;

procedure BinaryFile.Seek(n: int64);
begin
  PABCSystem.Seek(Self, n);
end;

// -----------------------------------------------------
//                  Eoln - Eof
// -----------------------------------------------------
function Eoln: boolean;
begin
  if not console_alloc then
    AllocConsole;
  Result := CurrentIOSystem.peek = 13
end;

function Eof: boolean;
begin
  if not console_alloc then
    AllocConsole;
  Result := CurrentIOSystem.peek = -1
end;

// -----------------------------------------------------
//                  Write - Writeln # Write subroutines
// -----------------------------------------------------
function PointerOutput.ToString: string;
begin
  result := PointerToString(p);
end;

constructor PointerOutput.Create(ptr: pointer);
begin
  p := ptr;
end;

procedure write;
begin
end;

procedure write_in_output(obj: object);
begin
  write(output, obj);
end;

procedure writeln_in_output;
begin
  writeln(output);
end;

procedure write(obj: object);
begin
  if output.sw <> nil then
    write_in_output(obj)
  else CurrentIOSystem.Write(obj);
end;

//procedure write(ptr: pointer);
//begin
//  CurrentIOSystem.Write(ptr);
//end;

procedure write(obj1, obj2: object);
begin
  if output.sw <> nil then
  begin
    write_in_output(obj1);
    write_in_output(obj2);
  end
  else
  begin
    CurrentIOSystem.Write(obj1);
    CurrentIOSystem.Write(obj2);
  end;
end;

procedure write(params args: array of object);
begin
  for var i := 0 to args.length - 1 do
    if output.sw <> nil then
      write_in_output(args[i])
    else
      CurrentIOSystem.Write(args[i]);
end;

procedure writeln(obj: object);
begin
  if output.sw <> nil then
  begin
    write_in_output(obj);
    writeln_in_output;
  end
  else
  begin
    CurrentIOSystem.Write(obj);
    CurrentIOSystem.Writeln;
  end
end;

//procedure writeln(ptr: pointer);
//begin
//  CurrentIOSystem.Write(PointerToString(ptr));
//  CurrentIOSystem.Writeln;
//end;

procedure writeln(obj1, obj2: object);
begin
  if output.sw <> nil then
  begin
    write_in_output(obj1);
    write_in_output(obj2);
    writeln_in_output;
  end
  else
  begin
    CurrentIOSystem.Write(obj1);
    CurrentIOSystem.Write(obj2);
    CurrentIOSystem.Writeln;
  end
end;

procedure writeln;
begin
  if output.sw <> nil then
    writeln_in_output
  else CurrentIOSystem.Writeln;
end;

procedure writeln(params args: array of object);
begin
  if output.sw <> nil then
  begin
    for var i := 0 to args.length - 1 do
      write_in_output(args[i]);
    writeln_in_output;
  end
  else
  begin
    for var i := 0 to args.length - 1 do
      CurrentIOSystem.Write(args[i]);
    CurrentIOSystem.Writeln;
  end;
end;

procedure write(f: Text);
begin
end;

procedure write(f: Text; val: object);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sw = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_WRITING));

  f.sw.Write(StructuredObjectToString(val));
  {if val = nil then
  begin
    f.sw.Write('nil');
    exit;
  end;
  case System.Type.GetTypeCode(val.GetType) of
    TypeCode.Double, 
    TypeCode.Single,  
    TypeCode.Decimal: 
    f.sw.Write(FormatFloatNumber(val.ToString));
  else
    f.sw.Write(val)
  end;}
end;

procedure write(f: Text; params args: array of object);
begin
  for var i := 0 to args.length - 1 do
    write(f, args[i]);
end;

procedure writeln(f: Text);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sw = nil then 
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED_FOR_WRITING));

  f.sw.WriteLine;
end;

procedure writeln(f: Text; val: object);
begin
  write(f, val);
  writeln(f);
end;

procedure writeln(f: Text; params args: array of object);
begin
  for var i := 0 to args.length - 1 do
    write(f, args[i]);
  writeln(f);  
end;

procedure WriteFormat(formatstr: string; params args: array of object);
begin
  var s := Format(formatstr, args);
  write(s);
end;

procedure WritelnFormat(formatstr: string; params args: array of object);
begin
  var s := Format(formatstr, args);
  writeln(s);
end;

procedure WriteFormat(f: Text; formatstr: string; params args: array of object);
begin
  var s := Format(formatstr, args);
  write(f, s);
end;

procedure WritelnFormat(f: Text; formatstr: string; params args: array of object);
begin
  var s := Format(formatstr, args);
  writeln(f, s);
end;

// -----------------------------------------------------
//                  Print - Println
// -----------------------------------------------------
procedure Print(s: string);
begin
  write(s, ' ');
end;

procedure Print(params args: array of object);
begin
  if args.Length = 0 then
    exit;
  for var i := 0 to args.length - 1 do
    write(args[i], ' ');
end;

procedure Println(params args: array of object);
begin
  Print(args);
  writeln;
end;

procedure Print(f: Text; params args: array of object);
begin
  if args.Length = 0 then
    exit;
  for var i := 0 to args.length - 1 do
    write(f, args[i], ' ');
end;

procedure Println(f: Text; params args: array of object);
begin
  Print(f, args);
  writeln(f);
end;
// -----------------------------------------------------
//                  Text files
// -----------------------------------------------------
procedure Assign(f: Text; name: string);
begin
  try
    f.fi := System.IO.FileInfo.Create(name);
  except
    on e: Exception do
      raise e;
  end;
  if f = output then
    f.sw := new StreamWriter(f.fi.FullName);
  if f = input then
    f.sr := new StreamReader(f.fi.FullName, DefaultEncoding);
end;

procedure AssignFile(f: Text; name: string);
begin
  Assign(f, name);
end;

procedure Close(f: Text);
begin
  if f.fi = nil then
    raise new IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr <> nil then 
  begin
    f.sr.Close; 
    f.sr := nil; 
    f.sw := nil;
    //    f.fi := nil;
  end
  else if f.sw <> nil then 
  begin
    f.sw.Close;
    f.sr := nil;
    f.sw := nil;
    //    f.fi := nil;
  end
  else raise new IOException(GetTranslation(FILE_NOT_OPENED));
end;

procedure CloseFile(f: Text);
begin
  Close(f);
end;

procedure Reset(f: Text);
begin
  Reset(f,DefaultEncoding)
end;

procedure Reset(f: Text; en: Encoding);
begin
  if f.fi = nil then
    raise new IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sr = nil then
  begin
    f.sr := new StreamReader(f.fi.FullName, en);
    if f.sw <> nil then
    begin
      f.sw.Close;
      f.sw := nil;
    end;  
  end
  else 
  begin
    f.sr.BaseStream.Position := 0;
    f.sr.DiscardBufferedData;
  end;
end;

procedure Reset(f: Text; name: string);
begin
  Reset(f,name,DefaultEncoding)
end;

procedure Reset(f: Text; name: string; en: Encoding);
begin
  assign(f, name);
  reset(f,en);
end;

procedure Rewrite(f: Text);
begin
  Rewrite(f,DefaultEncoding)
end;

procedure Rewrite(f: Text; en: Encoding);
begin
  if f.fi = nil then
    raise new IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.sw = nil then
  begin
    f.sw := new StreamWriter(f.fi.FullName, False, en);
    if f.sr <> nil then
    begin
      f.sr.Close;
      f.sr := nil;
    end;  
  end
  else 
  begin
    f.sw.BaseStream.Position := 0;
  end;
end;

procedure Rewrite(f: Text; name: string);
begin
  Rewrite(f,name,DefaultEncoding)
end;

procedure Rewrite(f: Text; name: string; en: Encoding);
begin
  Assign(f, name);
  Rewrite(f,en);
end;

procedure Append(f: Text);
begin
  Append(f,DefaultEncoding)
end;

procedure Append(f: Text; en: Encoding);
begin
  if f.fi = nil then
    raise new IOException(GetTranslation(FILE_NOT_ASSIGNED));
  f.sw := new StreamWriter(f.fi.FullName, True, en);
end;

procedure Append(f: Text; name: string);
begin
  Append(f,name,DefaultEncoding)
end;

procedure Append(f: Text; name: string; en: Encoding);
begin
  Assign(f, name);
  Append(f,en);
end;

function OpenRead(fname: string): Text;
begin
  Result := OpenRead(fname,DefaultEncoding)
end;

function OpenRead(fname: string; en: Encoding): Text;
begin
  var f: Text := new Text;
  Reset(f,fname,en);
  Result := f;
end;

function OpenWrite(fname: string): Text;
begin
  Result := OpenWrite(fname,DefaultEncoding)
end;

function OpenWrite(fname: string; en: Encoding): Text;
begin
  var f: Text := new Text;
  Rewrite(f,fname,en);
  Result := f;
end;

function OpenAppend(fname: string): Text;
begin
  Result := OpenAppend(fname,DefaultEncoding)
end;

function OpenAppend(fname: string; en: Encoding): Text;
begin
  var f: Text := new Text;
  Append(f,fname,en);
  Result := f;
end;

function Eof(f: Text): boolean;
begin
  if f.sr <> nil then
    Result := f.sr.EndOfStream
  else if f.sw <> nil then
    raise new IOException(GetTranslation(EOF_FOR_TEXT_WRITEOPENED))
  else raise new IOException(GetTranslation(FILE_NOT_OPENED));
end;

function Eoln(f: Text): boolean;
begin
  if f.sr <> nil then
    Result := f.sr.EndOfStream or (f.sr.Peek = 13) or (f.sr.Peek = 10) 
  else if f.sw <> nil then
    raise new IOException(GetTranslation(EOLN_FOR_TEXT_WRITEOPENED))
  else raise new IOException(GetTranslation(FILE_NOT_OPENED));
end;

function SeekEof(f: Text): boolean;
var
  i: integer;
begin
  if f.sw <> nil then
    raise new IOException(GetTranslation(SEEKEOF_FOR_TEXT_WRITEOPENED));
  if f.sr = nil then  
    raise new IOException(GetTranslation(FILE_NOT_OPENED));
  repeat
    if f.sr.EndOfStream then
      break;
    i := f.sr.Peek;
    if not char.IsWhiteSpace(char(i)) then
      break;
    f.sr.Read;
  until False;  
  Result := f.sr.EndOfStream;
end;

function SeekEoln(f: Text): boolean;
begin
  if f.sw <> nil then
    raise new IOException(GetTranslation(SEEKEOLN_FOR_TEXT_WRITEOPENED));
  if f.sr = nil then  
    raise new IOException(GetTranslation(FILE_NOT_OPENED));
  repeat
    if f.sr.EndOfStream then
      break;
    var i := f.sr.Peek;
    //    if not char.IsWhiteSpace(char(i)) then
    if (i <> 32) and (i <> 9) then // Если это не пробел и не табуляция
      break;
    f.sr.Read;
  until False;
  Result := f.sr.EndOfStream or (f.sr.Peek = 13) or (f.sr.Peek = 10); // Концом строки 
end;

procedure Flush(f: Text);
begin
  if f.sw <> nil then
    f.sw.Flush
end;

procedure Erase(f: Text);
begin
  if f.fi = nil then
    raise new IOException(GetTranslation(FILE_NOT_ASSIGNED));
  f.fi.Delete;
end;

procedure Rename(f: Text; newname: string);
begin
  if f.fi = nil then
    raise new IOException(GetTranslation(FILE_NOT_ASSIGNED));
  System.IO.File.Move(f.fi.FullName, newname);
end;

procedure TextFileInit(var f: Text);
begin
  f := new Text;  
end;

// -----------------------------------------------------
// ReadLines, ReadAllText, WriteLines, WriteAllText
// -----------------------------------------------------
function ReadLines(path: string): sequence of string;
begin
  Result := ReadLines(path,DefaultEncoding);
end;

function ReadLines(path: string; en: Encoding): sequence of string;
begin
  Result := System.IO.File.ReadLines(path,en);
end;

function ReadAllLines(path: string): array of string;
begin
  Result := ReadAllLines(path,DefaultEncoding);
end;

function ReadAllLines(path: string; en: Encoding): array of string;
begin
  Result := System.IO.File.ReadAllLines(path,en);
end;

function ReadAllText(path: string): string;
begin
  Result := ReadAllText(path,DefaultEncoding);
end;

function ReadAllText(path: string; en: Encoding): string;
begin
  Result := System.IO.File.ReadAllText(path,en);
end;

procedure WriteLines(path: string; ss: sequence of string);
begin
  WriteLines(path,ss,DefaultEncoding);
end;

procedure WriteLines(path: string; ss: sequence of string; en: Encoding);
begin
  System.IO.File.WriteAllLines(path,ss,en);
end;

procedure WriteAllLines(path: string; ss: array of string);
begin
  WriteAllLines(path,ss,DefaultEncoding);
end;

procedure WriteAllLines(path: string; ss: array of string; en: Encoding);
begin
  System.IO.File.WriteAllLines(path,ss,en);
end;

procedure WriteAllText(path: string; s: string);
begin
  System.IO.File.WriteAllText(path,s,DefaultEncoding);
end;

procedure WriteAllText(path: string; s: string; en: Encoding);
begin
  System.IO.File.WriteAllText(path,s,en);
end;

// -----------------------------------------------------
//                  Abstract binary files
// -----------------------------------------------------
procedure Assign(f: AbstractBinaryFile; name: string);
begin
  f.fi := System.IO.FileInfo.Create(name);
end;

procedure AssignFile(f: AbstractBinaryFile; name: string);
begin
  Assign(f, name);
end;

procedure Close(f: AbstractBinaryFile);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  if f.fs <> nil then
  begin
    f.br.Close; 
    f.bw.Close;
    f.fs := nil; 
    f.br := nil;
    f.bw := nil;
  end;
end;

procedure CloseFile(f: AbstractBinaryFile);
begin
  Close(f);
end;

procedure Reset(f: AbstractBinaryFile);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then 
  begin
    f.fs := new FileStream(f.fi.FullName, FileMode.Open);
    f.br := new BinaryReader(f.fs, DefaultEncoding);
    f.bw := new BinaryWriter(f.fs, DefaultEncoding);
  end 
  else
    f.fs.Position := 0;
end;

procedure Reset(f: AbstractBinaryFile; name: string);
begin
  Assign(f, name);
  Reset(f);
end;

procedure Rewrite(f: AbstractBinaryFile);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
  begin
    f.fs := new FileStream(f.fi.FullName, FileMode.Create);
    f.bw := new BinaryWriter(f.fs, DefaultEncoding);
    f.br := new BinaryReader(f.fs, DefaultEncoding);
  end
  else 
  begin
    f.fs.Position := 0;
    Truncate(f);
  end;  
end;

procedure Rewrite(f: AbstractBinaryFile; name: string);
begin
  Assign(f, name);
  Rewrite(f);
end;

procedure Truncate(f: AbstractBinaryFile);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  f.fs.SetLength(f.fs.Position);
end;

function Eof(f: AbstractBinaryFile): boolean;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  if f.fs <> nil then
    Result := f.fs.Position = f.fs.Length;
end;

procedure Erase(f: AbstractBinaryFile);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  f.fi.Delete;
end;

procedure Rename(f: AbstractBinaryFile; newname: string);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  System.IO.File.Move(f.fi.FullName, newname);
end;

function AbstractBinaryFileReadT(f: AbstractBinaryFile; t: System.Type; var ind: integer; in_arr: boolean): object;
var
  t1: System.Type;
  elem: object;
  fa: array of System.Reflection.FieldInfo;
  NullBasedArray: System.Array;
begin
  if t.IsPrimitive then 
  begin
    with f.br do
      case System.Type.GetTypeCode(t) of
        TypeCode.Boolean: Result := ReadBoolean;
        TypeCode.Byte:    Result := ReadByte;
        TypeCode.Char:    Result := ReadChar;
        TypeCode.Decimal: Result := ReadDecimal;
        TypeCode.Double:  Result := ReadDouble;
        TypeCode.Int16:   Result := ReadInt16;
        TypeCode.Int32:   Result := ReadInt32;
        TypeCode.Int64:   Result := ReadInt64;
        TypeCode.UInt16:  Result := ReadUInt16;
        TypeCode.UInt32:  Result := ReadUInt32;
        TypeCode.UInt64:  Result := ReadUInt64;
        TypeCode.SByte:   Result := ReadSByte;
        TypeCode.Single:  Result := ReadSingle;
      
      end;
  end 
  else if t.IsEnum then Result := f.br.ReadInt32
  else if t.IsValueType then
  begin
    elem := Activator.CreateInstance(t);
    fa := t.GetFields;
    for var i := 0 to fa.Length - 1 do
      if not fa[i].IsStatic then
        fa[i].SetValue(elem, AbstractBinaryFileReadT(f, fa[i].FieldType, ind, in_arr));
    Result := elem;
  end
  else
  if t = typeof(string) then
  begin
    Result := f.br.ReadString();
    if (f is TypedFile) and ((f as TypedFile).offsets <> nil) and ((f as TypedFile).offsets.Length > 0) then
    begin
      f.br.BaseStream.Seek((f as TypedFile).offsets[ind] - (Result as string).Length, SeekOrigin.Current);
    end;
    if not in_arr then
      Inc(ind);
    //if f is TypedFile then
    //f.br.BaseStream.Seek(255-(Result as string).Length,SeekOrigin.Current);
  end
  else if t = typeof(TypedSet) then
  begin
    Result := f.br.ReadBytes(256 div 8);
    elem := Activator.CreateInstance(t, Result);
    Result := elem;
  end
  else
  begin
    elem := Activator.CreateInstance(t);
    NullBasedArray := GetNullBasedArray(elem);
    if NullBasedArray <> nil then 
    begin
      t1 := NullBasedArray.GetType.GetElementType;
      var tmp := ind;
      var tmp2 := 0;
      for var i := 0 to NullBasedArray.Length - 1 do
      begin
        NullBasedArray.SetValue(AbstractBinaryFileReadT(f, t1, ind, i = 0 ? false : true), i);
        if i = 0 then
          tmp2 := ind;
        ind := tmp;
      end;
      ind := tmp2;
    end;
    result := elem;
  end;
end;

procedure Write(f: AbstractBinaryFile; val: object; arr: boolean; var ind: integer; in_arr: boolean);
var
  t: System.Type;
  fa: array of System.Reflection.FieldInfo;
  NullBasedArray: System.Array;
begin
  t := val.GetType;
  if f is TypedFile and not arr then
  begin
    t := (f as TypedFile).ElementType;
  end;
  if t.IsPrimitive or t.IsEnum then 
  begin
    if t = typeof(integer) then
      f.bw.Write(Convert.ToInt32(val))
    else if t = typeof(real) then
      f.bw.Write(Convert.ToDouble(val))
    else if t = typeof(char) then
      f.bw.Write(Convert.ToChar(val))
    else if t = typeof(boolean) then
      f.bw.Write(Convert.ToBoolean(val))
    else if t = typeof(byte) then
      f.bw.Write(Convert.ToByte(val))
    else if t = typeof(shortint) then
      f.bw.Write(Convert.ToSByte(val))
    else if t = typeof(smallint) then
      f.bw.Write(Convert.ToInt16(val))
    else if t = typeof(word) then
      f.bw.Write(Convert.ToUInt16(val))
    else if t = typeof(longword) then
      f.bw.Write(Convert.ToUInt32(val))
    else if t = typeof(int64) then
      f.bw.Write(Convert.ToInt64(val))
    else if t = typeof(uint64) then
      f.bw.Write(Convert.ToUInt64(val))
    else if t = typeof(single) then
      f.bw.Write(Convert.ToSingle(val))
    else if t.IsEnum then
      f.bw.Write(Convert.ToInt32(val));
  end  
  else if t.IsValueType then
  begin
    fa := t.GetFields;
    for var i := 0 to fa.Length - 1 do
    begin
      if not fa[i].IsStatic then
        Write(f, fa[i].GetValue(val), true, ind, in_arr);
    end;
  end
  else if t = typeof(string) then
  begin
    //var tmp := f.bw.BaseStream.Position;
    //f.bw.Write(byte(string(val).Length));
    f.bw.Write(string(val));
    if (f is TypedFile) and ((f as TypedFile).offsets <> nil) and ((f as TypedFile).offsets.Length > 0) then
    begin
      f.bw.Write(new byte[(f as TypedFile).offsets[ind] - (val as string).Length]);
    end;
    if not in_arr then
      Inc(ind);
  end
  else if t = typeof(TypedSet) then
  begin
    f.bw.Write((val as TypedSet).GetBytes());
  end
  else
  begin
    NullBasedArray := GetNullBasedArray(val);
    if NullBasedArray <> nil then
    begin
      var tmp := ind;
      var tmp2 := 0;
      for var i := 0 to NullBasedArray.Length - 1 do
      begin
        Write(f, NullBasedArray.GetValue(i), true, ind, i = 0 ? false : true);
        if i = 0 then
          tmp2 := ind;
        ind := tmp;
      end;
      ind := tmp2;
    end;
  end;
end;

procedure Write(f: AbstractBinaryFile; params vals: array of object);
begin
  if f.fi = nil then raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  for var i := 0 to vals.Length - 1 do
  begin
    var NullBasedArray := GetNullBasedArray(vals[i]);
    var ind := 0;
    Write(f, vals[i], NullBasedArray <> nil, ind, false);
  end;
end;

procedure Writeln(f: AbstractBinaryFile);
begin
  raise new System.IO.IOException(GetTranslation(WRITELN_IN_BINARYFILE_ERROR_MESSAGE));
end;

procedure Writeln(f: AbstractBinaryFile; val: object);
begin
  raise new System.IO.IOException(GetTranslation(WRITELN_IN_BINARYFILE_ERROR_MESSAGE));
end;

procedure Writeln(f: AbstractBinaryFile; params vals: array of object);
begin
  raise new System.IO.IOException(GetTranslation(WRITELN_IN_BINARYFILE_ERROR_MESSAGE));
end;

// -----------------------------------------------------
//                  Typed files
// -----------------------------------------------------

function FilePos(f: TypedFile): int64;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  Result := f.fs.Position div f.ElementSize;
end;

function FileSize(f: TypedFile): int64;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  if f.fs.Length mod f.ElementSize <> 0 then
    raise new System.IO.IOException('Bad typed file size');
  Result := f.fs.Length div f.ElementSize;
end;

procedure Seek(f: TypedFile; n: int64);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  f.fs.Position := n * f.ElementSize
end;

procedure TypedFileInit(var f: TypedFile; ElementType: System.Type);
begin
  f := new TypedFile(ElementType, 0, new integer[0]);
end;

procedure TypedFileInit(var f: TypedFile; ElementType: System.Type; off: integer; params offs: array of integer);
begin
  f := new TypedFile(ElementType, off, offs);
end;

procedure TypedFileInitWithShortString(var f: TypedFile; ElementType: System.Type; off: integer; params offs: array of integer);
begin
  f := new TypedFile(ElementType, off, offs);
end;

function TypedFileRead(f: TypedFile): object;
begin
  var ind := 0;
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  Result := AbstractBinaryFileReadT(f, f.ElementType, ind, false);
  //if f.offset > 0 then
  //f.fs.Seek(tmp+(f as TypedFile).ElementSize-f.fs.Position,SeekOrigin.Current);
end;

// -----------------------------------------------------
//                  Binary files
// -----------------------------------------------------
function FilePos(f: BinaryFile): int64;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  Result := f.fs.Position;
end;

function FileSize(f: BinaryFile): int64;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  Result := f.fs.Length;
end;

procedure Seek(f: BinaryFile; n: int64);
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  f.fs.Position := n;
end;

procedure BinaryFileInit(var f: BinaryFile);
begin
  f := new BinaryFile();
end;

function BinaryFileRead(var f: BinaryFile; ElementType: System.Type): object;
begin
  if f.fi = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_ASSIGNED));
  if f.fs = nil then
    raise new System.IO.IOException(GetTranslation(FILE_NOT_OPENED));
  var ind := 0;
  Result := AbstractBinaryFileReadT(f, ElementType, ind, false);
end;

// -----------------------------------------------------
// Operating System subroutines: implementation
// -----------------------------------------------------
function PascalABCVersion: string;
begin
  Result := '3.2.0.1364';
end;

function ParamCount: integer;
begin
  if (Environment.GetCommandLineArgs.Length > 1) and ((Environment.GetCommandLineArgs[1] = '[REDIRECTIOMODE]') or (Environment.GetCommandLineArgs[1] = '[RUNMODE]')) then
    Result := Environment.GetCommandLineArgs.Length - 2
  else
    Result := Environment.GetCommandLineArgs.Length - 1;
end;

function ParamStr(i: integer): string;
begin
  if (Environment.GetCommandLineArgs.Length > 1) and ((Environment.GetCommandLineArgs[1] = '[REDIRECTIOMODE]') or (Environment.GetCommandLineArgs[1] = '[RUNMODE]')) then
    Result := Environment.GetCommandLineArgs[i + 1]
  else
    Result := Environment.GetCommandLineArgs[i];
end;

function GetDir: string;
begin
  Result := Environment.CurrentDirectory;
end;

procedure ChDir(s: string);
begin
  Environment.CurrentDirectory := s;
end;

procedure MkDir(s: string);
begin
  Directory.CreateDirectory(s);
end;

procedure RmDir(s: string);
begin
  Directory.Delete(s);
end;

function CreateDir(s: string): boolean;
begin
  try
    Result := True;
    Directory.CreateDirectory(s);
  except
    Result := False;
  end;
end;

function DeleteFile(s: string): boolean;
begin
  try
    Result := True;
    &File.Delete(s);
  except
    Result := False;
  end;
end;

function GetCurrentDir: string;
begin
  Result := Environment.CurrentDirectory;
end;

function RemoveDir(s: string): boolean;
begin
  try
    Result := True;
    Directory.Delete(s);
  except
    Result := False;
  end;
end;

function RenameFile(name, newname: string): boolean;
begin
  try
    Result := True;
    &File.Move(name, newname);
  except
    Result := False;
  end;
end;

function SetCurrentDir(s: string): boolean;
begin
  try
    Result := True;
    Environment.CurrentDirectory := s;
  except
    Result := False;
  end;
end;

function ChangeFileNameExtension(name, newext: string): string;
begin
  Result := System.IO.Path.ChangeExtension(name, newext);
end;

function FileExists(name: string): boolean;
begin
  Result := System.IO.File.Exists(name);
end;

procedure Assert(cond: boolean; sourceFile: string; line: integer);
begin
  if (Environment.OSVersion.Platform = PlatformID.Unix) or (Environment.OSVersion.Platform = PlatformID.MacOSX) or IsWDE then
  begin
    //var stackTrace := new System.Diagnostics.StackTrace(true);
    //var ind := 1;
    //if stackTrace.GetFrame(0).GetMethod().Name <> 'Assert' then
      //ind := 0;
    //var currentLine := stackTrace.GetFrame(ind).GetFileLineNumber();
    //var currentFile := stackTrace.GetFrame(ind).GetFileName();
    if not IsWDE then
      System.Diagnostics.Debug.Assert(cond,'Файл '+sourceFile+', строка '+line.ToString())
    else if not cond then
    begin
      var err := 'Сбой подтверждения: '+Environment.NewLine+'Файл '+sourceFile+', строка '+line.ToString();
      writeln(err);
      System.Threading.Thread.Sleep(500);
      raise new Exception();
    end;
  end
  else
    System.Diagnostics.Debug.Assert(cond);
end;

procedure Assert(cond: boolean; message: string; sourceFile: string; line: integer);
begin
  if (Environment.OSVersion.Platform = PlatformID.Unix) or (Environment.OSVersion.Platform = PlatformID.MacOSX) or IsWDE then
  begin
    //var stackTrace := new System.Diagnostics.StackTrace(true);
    //var ind := 1;
    //if stackTrace.GetFrame(0).GetMethod().Name <> 'Assert' then
    //  ind := 0;
    //var currentLine := stackTrace.GetFrame(ind).GetFileLineNumber();
    //var currentFile := stackTrace.GetFrame(ind).GetFileName();
    if not IsWDE then
      System.Diagnostics.Debug.Assert(cond,'Файл '+sourceFile+', строка '+line.ToString()+': '+message)
    else if not cond then
    begin
      var err := 'Сбой подтверждения: '+message+Environment.NewLine+'Файл '+sourceFile+', строка '+line.ToString();
      writeln(err);
      System.Threading.Thread.Sleep(500);
      raise new Exception();
    end;
  end
  else
    System.Diagnostics.Debug.Assert(cond, message);
end;

function DiskFree(diskname: string): int64;
begin
  try
    var d := new System.IO.DriveInfo(diskname);
    Result := d.TotalFreeSpace;
  except
    Result := -1;
  end;
end;

function DiskSize(diskname: string): int64;
begin
  try
    var d := new System.IO.DriveInfo(diskname);
    Result := d.TotalSize;
  except
    Result := -1;
  end;
end;

function ConvertDiskToDiskName(disk: integer): string;
begin
  if disk = 0 then
  begin
    var s := Paramstr(0);
    var p := Pos(':', s);
    if p > 0 then 
      Result := Copy(s, 1, p)
    else Result := 'C:';  
  end
  else
  begin
    if disk < 0 then disk := 0;
    if disk > 26 then disk := 26;
    var ch := 'A';
    Inc(ch, disk - 1);
    Result := ch + ':';
  end;
end;

function DiskFree(disk: integer): int64;
begin
  Result := DiskFree(ConvertDiskToDiskName(disk));
end;

function DiskSize(disk: integer): int64;
begin
  Result := DiskSize(ConvertDiskToDiskName(disk));
end;

var curr_time := DateTime.Now;

function Milliseconds: integer;
begin
  curr_time := DateTime.Now;
  Milliseconds := Convert.ToInt32((curr_time - StartTime).TotalMilliseconds);
end;

function MillisecondsDelta: integer;
begin
  var t := DateTime.Now;  
  Result := Convert.ToInt32((t - curr_time).TotalMilliseconds);
  curr_time := DateTime.Now;
end;

procedure Halt;
begin
  Halt(ExitCode);
end;

procedure Halt(exitCode: integer);
begin
  //System.Diagnostics.Process.GetCurrentProcess.Kill;
  //WINAPI_TerminateProcess(System.Diagnostics.Process.GetCurrentProcess.Handle, exitCode);
  System.Environment.Exit(exitCode);
end;

procedure Sleep(ms: integer);
begin
  System.Threading.Thread.Sleep(ms);
end;

function GetEXEFileName: string;
begin
  Result := System.Reflection.Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;
end;

function PointerToString(p: pointer): string;
begin
  //result:= Convert.ToString(integer(p), 16);
  if p = nil then
    result := 'nil'
  else result := '$' + integer(p).ToString('X');
end;

procedure Exec(filename: string);
begin
  System.Diagnostics.Process.Start(filename)
end;

procedure Exec(filename: string; args: string);
begin
  System.Diagnostics.Process.Start(filename, args)
end;

procedure Execute(filename: string);
begin
  System.Diagnostics.Process.Start(filename)
end;

procedure Execute(filename: string; args: string);
begin
  System.Diagnostics.Process.Start(filename, args)
end;

// -----------------------------------------------------
// EnumerateFiles, EnumerateDirectories
// -----------------------------------------------------
function EnumerateFiles(path: string; searchPattern: string): sequence of string;
begin
  Result := System.IO.Directory.EnumerateFiles(path,searchPattern,System.IO.SearchOption.TopDirectoryOnly)
end;

function EnumerateAllFiles(path: string; searchPattern: string): sequence of string;
begin
  Result := System.IO.Directory.EnumerateFiles(path,searchPattern,System.IO.SearchOption.AllDirectories)
end;

function EnumerateDirectories(path: string): sequence of string;
begin
  Result := System.IO.Directory.EnumerateDirectories(path,'*.*',System.IO.SearchOption.TopDirectoryOnly)
end;

function EnumerateAllDirectories(path: string): sequence of string;
begin
  Result := System.IO.Directory.EnumerateDirectories(path,'*.*',System.IO.SearchOption.AllDirectories)
end;

// -----------------------------------------------------
//                File name functions: implementation
// -----------------------------------------------------
function ExtractFileName(fname: string): string;
begin
  var fi := new System.IO.FileInfo(fname);
  Result := fi.Name;
end;

function ExtractFileExt(fname: string): string;
begin
  var fi := new System.IO.FileInfo(fname);
  Result := fi.Extension;
end;

function ExtractFilePath(fname: string): string;
begin
  var fi := new System.IO.FileInfo(fname);
  Result := fi.DirectoryName;
  if (Result.Length > 0) and (Result[Result.Length] <> '\') and  (Result[Result.Length] <> '/') then
    Result += '\';
end;

function ExtractFileDir(fname: string): string;
begin
  var fi := new System.IO.FileInfo(fname);
  Result := fi.DirectoryName;
end;

function ExtractFileDrive(fname: string): string;
begin
  try
    var fi := new System.IO.FileInfo(fname);
    Result := fi.DirectoryName;
    var p := Pos(':', Result);
    if p > 0 then
      Result := Copy(Result, 1, p)
    else Result := '';
  except
    on e: Exception do
      raise e;  
  end;
end;

function ExpandFileName(fname: string): string;
begin
  var fi := new System.IO.FileInfo(fname);
  Result := fi.FullName;
end;

// -----------------------------------------------------
//                Mathematical functions: implementation
// -----------------------------------------------------
function Sign(x: shortint): integer;
begin
  Result := Math.Sign(x);
end;

function Sign(x: smallint): integer;
begin
  Result := Math.Sign(x);
end;

function Sign(x: integer): integer;
begin
  Result := Math.Sign(x);
end;

function Sign(x: BigInteger): integer;
begin
  Result := x.Sign;
end;

function Sign(x: int64): integer;
begin
  Result := Math.Sign(x);
end;

function Sign(x: longword): integer;
begin
  Result := Math.Sign(int64(x));
end;

function Sign(x: uint64): integer;
begin
  Result := Math.Sign(int64(x));
end;

function Sign(x: real): integer;
begin
  Result := Math.Sign(x);
end;

function Abs(x: shortint): shortint;
begin
  Result := Math.Abs(x);
end;

function Abs(x: smallint): smallint;
begin
  Result := Math.Abs(x);
end;

function Abs(x: integer): integer;
begin
  Result := Math.Abs(x);
end;

function Abs(x: BigInteger): BigInteger;
begin
  Result := BigInteger.Abs(x)
end;

function Abs(x: int64): int64;
begin
  Result := Math.Abs(x);
end;

function Abs(x: longword): longword;
begin
  Result := Math.Abs(int64(x));
end;

function Abs(x: uint64): uint64;
begin
  Result := Math.Abs(int64(x));
end;

function Abs(x: real): real;
begin
  Result := Math.Abs(x);
end;

function Sin(x: real): real;
begin
  Result := Math.Sin(x);
end;

function Sinh(x: real): real;
begin
  Result := Math.Sinh(x);
end;

function Cos(x: real): real;
begin
  Result := Math.Cos(x);
end;

function Cosh(x: real): real;
begin
  Result := Math.Cosh(x);
end;

function Tan(x: real): real;
begin
  Result := Math.Tan(x);
end;

function Tanh(x: real): real;
begin
  Result := Math.Tanh(x);
end;

function ArcSin(x: real): real;
begin
  Result := Math.Asin(x);
end;

function ArcCos(x: real): real;
begin
  Result := Math.Acos(x);
end;

function ArcTan(x: real): real;
begin
  Result := Math.Atan(x);
end;

function Exp(x: real): real;
begin
  Result := Math.Exp(x);
end;

function Ln(x: real): real;
begin
  Result := Math.Log(x);
end;

function Log2(x: real): real;
begin
  Result := LogN(2, x);
end;

function Log10(x: real): real;
begin
  Result := Math.Log10(x);
end;

function LogN(base, x: real): real;
begin
  Result := Math.Log(x) / Math.Log(base);
end;

function Sqrt(x: real): real;
begin
  Result := Math.Sqrt(x);
end;

function Sqr(x: integer): int64;
begin
  Result := x * x;
end;

function Sqr(x: shortint): integer;
begin
  Result := x * x;
end;

function Sqr(x: smallint): integer;
begin
  Result := x * x;
end;

function Sqr(x: BigInteger): BigInteger;
begin
  Result := x * x;
end;

function Sqr(x: longword): uint64;
begin
  Result := x * x;
end;

function Sqr(x: int64): int64;
begin
  Result := x * x;
end;

function Sqr(x: uint64): uint64;
begin
  Result := x * x;
end;

function Sqr(x: real): real;
begin
  Result := x * x;
end;

function Power(x, y: real): real;
begin
  Result := Math.Pow(x, y);
end;

function Power(x, y: integer): real;
begin
  Result := Math.Pow(x, y);
end;

function Power(x: BigInteger; y: integer): BigInteger;
begin
  Result := BigInteger.Pow(x, y)
end;

function Round(x: real): integer;
begin
  Result := Convert.ToInt32(Math.Round(x));
end;

function RoundBigInteger(x: real): BigInteger;
begin
  Result := BigInteger.Create(Math.Round(x));
end;

function Trunc(x: real): integer;
begin
  Result := Convert.ToInt32(Math.Truncate(x));
end;

function TruncBigInteger(x: real): BigInteger;
begin
  Result := BigInteger.Create(Math.Truncate(x));
end;

function Int(x: real): real;
begin
  //if x>=0 then
  //  Result := Math.Floor(x)
  //else Result := Math.Ceiling(x);
  Result := x >= 0 ? Math.Floor(x) : Math.Ceiling(x);
end;

function Frac(x: real): real;
begin
  Result := x - Int(x);
end;

function Floor(x: real): integer;
begin
  Result := Convert.ToInt32(Math.Floor(x));
end;

function Ceil(x: real): integer;
begin
  Result := Convert.ToInt32(Math.Ceiling(x));
end;

function RadToDeg(x: real): real;
begin
  Result := x * 180 / Pi;
end;

function DegToRad(x: real): real;
begin
  Result := x * Pi / 180;
end;

procedure Randomize;
begin
  rnd := new System.Random;
end;

procedure Randomize(seed: integer);
begin
  rnd := new System.Random(seed);
end;

function Random(MaxValue: integer): integer;
begin
  Result := rnd.Next(MaxValue);
end;

function Random(a, b: integer): integer;
begin
  if a > b then Swap(a, b);
  Result := rnd.Next(a, b + 1);
end;

function Random: real;
begin
  Result := rnd.NextDouble;
end;

function Max(a, b: byte): byte;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: shortint): shortint;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: word): word;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: smallint): smallint;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: integer): integer;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: BigInteger): BigInteger;
begin
  Result := BigInteger.Max(a,b);
end;

function Max(a, b: longword): longword;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: int64): int64;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: uint64): uint64;
begin
  Result := Math.Max(a, b);
end;

function Max(a, b: real): real;
begin
  Result := Math.Max(a, b);
end;

function Min(a, b: byte): byte;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: shortint): shortint;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: word): word;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: smallint): smallint;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: integer): integer;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: BigInteger): BigInteger;
begin
  Result := BigInteger.Min(a,b);
end;

function Min(a, b: longword): longword;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: int64): int64;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: uint64): uint64;
begin
  Result := Math.Min(a, b);
end;

function Min(a, b: real): real;
begin
  Result := Math.Min(a, b);
end;

function Odd(i: byte): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: shortint): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: word): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: smallint): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: integer): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: BigInteger): boolean;
begin
  Result := not i.IsEven;
end;

function Odd(i: longword): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: int64): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Odd(i: uint64): boolean;
begin
  result := (i mod 2) <> 0;
end;

function Cplx(re,im: real) := new Complex(re,im);

function CplxFromPolar(magnitude,phase: real) := Complex.FromPolarCoordinates(magnitude,phase);

function Sqrt(c: Complex) := Complex.Sqrt(c);

function Abs(c: Complex) := Complex.Abs(c);

function Conjugate(c: Complex) := Complex.Conjugate(c);

function Cos(c: Complex) := Complex.Cos(c);

function Exp(c: Complex) := Complex.Exp(c);

function Log(c: Complex) := Complex.Log(c);

function Log10(c: Complex) := Complex.Log10(c);

function Power(c,power: Complex) := Complex.Pow(c,power);

function Sin(c: Complex) := Complex.Sin(c);

// -----------------------------------------------------
//                Dynamic arrays: implementation
// -----------------------------------------------------
function Low(i: System.Array): integer;
begin
  if i <> nil then 
    Result := i.GetLowerBound(0)
  else Result := 0;
end;

function High(i: System.Array): integer;
begin
  if i <> nil then 
    Result := i.GetUpperBound(0)
  else Result := -1;
end;

function Length(a: &Array): integer;
begin
  if a = nil then
    Result := 0
  else Result := a.Length;
end;

function Length(a: &Array; dim: integer): integer;
begin
  if a = nil then
    Result := 0
  else Result := a.GetLength(dim);
end;

function Copy(a: &Array): &Array;
begin
  Result := &Array(a.Clone());
end;

procedure Sort<T>(a: array of T);
begin
  System.Array.Sort(a);
end;

procedure Sort<T>(a: array of T; cmp: (T,T)->integer);
begin
  System.Array.Sort(a,cmp);
end;

procedure Sort<T>(a: array of T; less: (T,T)->boolean);
begin
  System.Array.Sort(a,(x,y)->less(x,y)?-1:(less(y,x)?1:0));
end;

procedure Sort<T>(l: List<T>);
begin
  l.Sort();
end;

procedure Sort<T>(l: List<T>; cmp: (T,T)->integer);
begin
  l.Sort(cmp);
end;

procedure Sort<T>(l: List<T>; less: (T,T)->boolean);
begin
  l.Sort((x,y)->less(x,y)?-1:(less(y,x)?1:0));
end;

procedure Reverse<T>(a: array of T);
begin
  System.Array.Reverse(a);
end;

procedure Reverse<T>(a: array of T; index,count: integer);
begin
  System.Array.Reverse(a,index,count);
end;

procedure Reverse<T>(a: List<T>);
begin
  a.Reverse
end;

procedure Reverse<T>(a: List<T>; index,count: integer);
begin
  a.Reverse(index,count)
end;


procedure Shuffle<T>(a: array of T);
begin
  var n := a.Length;
	for var i:=0 to n-1 do
	  Swap(a[i],a[Random(n)]);
end;

procedure Shuffle<T>(l: List<T>);
begin
  var n := l.Count;
	for var i:=0 to n-1 do
	begin
    var v := l[i];
    l[i] := l[Random(n)];
    l[Random(n)] := v;
  end;
end;

// -----------------------------------------------------
//                Char and String: implementation
// -----------------------------------------------------
function ChrAnsi(a: Byte): char;
begin
  if a < 128 then
    Result := char(a)
  else
  begin
    __one_byte[0] := a;
    Result := AnsiOrdChrEncoding.GetChars(__one_byte)[0];
  end;
end;

function OrdAnsi(a: char): byte;
begin
  if a < #128 then
    Result := byte(a)
  else
  begin
    __one_char[0] := a;
    Result := AnsiOrdChrEncoding.GetBytes(__one_char)[0];
  end;
end;

function Ord(a: integer): integer;
begin
  Result := a;
end;

function Ord(a: longword): longword;
begin
  Result := a;
end;

function Ord(a: int64): int64;
begin
  Result := a;
end;

function Ord(a: uint64): uint64;
begin
  Result := a;
end;

function Ord(a: boolean): integer;
begin
  Result := integer(a);
end;

function Chr(a: word): char;
begin
  Result := Convert.ToChar(a);
end;

function Ord(a: char): word;
begin
  Result := word(a);
end;

function ChrUnicode(a: word): char;
begin
  Result := Convert.ToChar(a);
end;

function OrdUnicode(a: char): word;
begin
  Result := word(a);
end;

function UpperCase(ch: char): char;
begin
  Result := char.ToUpper(ch);
end;

function LowerCase(ch: char): char;
begin
  Result := char.ToLower(ch);
end;

function UpCase(ch: char): char;
begin
  Result := char.ToUpper(ch);
end;

function LowCase(ch: char): char;
begin
  Result := char.ToLower(ch);
end;

procedure Str(i: integer; var s: string);
begin
  s := i.ToString;
end;

procedure Str(i: longword; var s: string);
begin
  s := i.ToString;
end;

procedure Str(i: int64; var s: string);
begin
  s := i.ToString;
end;

procedure Str(i: uint64; var s: string);
begin
  s := i.ToString;
end;

procedure Str(s1: string; var s: string);
begin
  s := s1;
end;

procedure Str(r: single; var s: string);
begin
  s := Convert.ToString(r, nfi);
end;

procedure Str(r: real; var s: string);
begin
  s := Convert.ToString(r, nfi);
end;

function Pos(subs, s: string; from: integer): integer;
begin
  if (subs = nil) or (subs.Length = 0) then
    Result := 0
  else Result := s.IndexOf(subs, from - 1,System.StringComparison.Ordinal) + 1;
end;

function PosEx(subs, s: string; from: integer): integer;
begin
  if (subs = nil) or (subs.Length = 0) then
    Result := 0
  else Result := s.IndexOf(subs, from - 1,System.StringComparison.Ordinal) + 1;
end;

function LastPos(subs, s: string): integer;
begin
  if (subs = nil) or (subs.Length = 0) then
    Result := 0
  else Result := s.LastIndexOf(subs, s.Length - 1,System.StringComparison.Ordinal) + 1;
end;

function LastPos(subs, s: string; from: integer): integer;
begin
  if (subs = nil) or (subs.Length = 0) then
    Result := 0
  else Result := s.LastIndexOf(subs, from - 1,System.StringComparison.Ordinal) + 1;
end;

function Pos(c: char; s: string; from: integer): integer;
begin
  Result := s.IndexOf(c, from - 1,System.StringComparison.Ordinal) + 1;
end;

function PosEx(c: char; s: string; from: integer): integer;
begin
  Result := s.IndexOf(c, from - 1,System.StringComparison.Ordinal) + 1;
end;

function LastPos(c: char; s: string): integer;
begin
  Result := s.LastIndexOf(c, s.Length - 1,System.StringComparison.Ordinal) + 1;
end;

function LastPos(c: char; s: string; from: integer): integer;
begin
  Result := s.LastIndexOf(c, from - 1,System.StringComparison.Ordinal) + 1;
end;

function Length(s: string): integer;
begin
  if s <> nil then 
    Result := s.Length
  else Result := 0;
end;

procedure SetLength(var s: string; n: integer);
begin
  if n < 0 then
    raise new System.ArgumentOutOfRangeException('n');
  if n = 0 then
    s := String.Empty  
  else if s.Length > n then
    s := s.Substring(0, n)
  else if s.Length < n then
    s += new string(' ', n - s.Length);
end;

procedure SetLengthForShortString(var s: string; n, sz: integer);
begin
  if n < 0 then
    raise new System.ArgumentOutOfRangeException('n');
  if n = 0 then
    s := String.Empty  
  else if s.Length > n then
    s := s.Substring(0, n)
  else if s.Length < n then
    if n <= sz then
      s += new string(' ', n - s.Length )
    else 
      s += new String(' ', sz - s.Length)
end;

procedure Insert(source: string; var s: string; index: integer);
// Insert никогда не возвращает исключения
begin
  if index < 1 then 
    index := 1;
  if index > s.Length + 1 then
    index := s.Length + 1;
  s := s.Insert(index - 1, source);
  {  try
  s := s.Insert(index - 1, source);
  except 
  on e: System.Exception do
  s := s.Insert(s.Length, source);
  end;}
end;

procedure InsertInShortString(source: string; var s: string; index, n: integer);
begin
  if index < 1 then 
    index := 1;
  if index > n then
    exit;
  try
    s := s.Insert(index - 1, source);
    if s.Length > n then s := s.Substring(0, n);
  except
    s := s.Insert(s.Length, source);
    if s.Length > n then s := s.Substring(0, n);
  end;
end;

procedure Delete(var s: string; index, count: integer);
// Delete никогда не возвращает исключения
begin
  if (index < 1) or (index > s.Length) or (count <= 0) then
    Exit;
  if index + count - 1 > s.Length then
    count := s.Length - index + 1;
  s := s.Remove(index - 1, count);
end;

function Copy(s: string; index, count: integer): string;
// Copy никогда не возвращает исключения
begin
  if index < 1 then
    index := 1;
  if (index > s.Length) or (count <= 0) then
  begin
    Result := '';
    exit;
  end;
  if index + count - 1 > s.Length then
    count := s.Length - index + 1;
  Result := s.SubString(index - 1, count);
  {  try
  if index - 1 >= s.Length then 
  Result := ''
  else Result := s.SubString(index - 1, count);
  except 
  on e: System.Exception do
  Result := s.Substring(index - 1, s.Length - index + 1);
  end;}
end;

function Concat(s1, s2: string): string;
begin
  Result := s1 + s2;
end;

function Concat(params strs: array of string): string;
begin
  var sb := new System.Text.StringBuilder;
  for var i := 0 to strs.length - 1 do
    sb.Append(strs[i]);
  concat := sb.ToString;
end;

function LowerCase(s: string): string;
begin
  Result := s.ToLower;
end;

function UpperCase(s: string): string;
begin
  Result := s.ToUpper;
end;

function StringOfChar(ch: char; count: integer): string;
begin
  Result := new string(ch, count);
end;

function ReverseString(s: string): string;
begin
  var ca := s.ToCharArray;
  &Array.Reverse(ca);
  Result := new string(ca);
end;

function CompareStr(s1, s2: string): Integer;
begin
  Result := string.CompareOrdinal(s1, s2);
end;

function LeftStr(s: string; count: integer): string;
begin
  if count > s.Length then
    count := s.Length;
  Result := s.Substring(0, count)
end;

function RightStr(s: string; count: integer): string;
begin
  if count > s.Length then
    count := s.Length;
  Result := s.Substring(s.Length - count, count);
end;

function Trim(s: string): string;
begin
  Result := s.Trim;
end;

function TrimLeft(s: string): string;
begin
  Result := s.TrimStart(' ');
end;

function TrimRight(s: string): string;
begin
  Result := s.TrimEnd(' ');
end;

function ErrorStringFromResource(s: string): string;
begin
  var _rm := new System.Resources.ResourceManager('mscorlib', typeof(object).Assembly);
  Result := _rm.GetString(s);
end;

function StrToInt(s: string): integer;
begin
  var j := 1;
  while (j <= s.Length) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if (j > s.Length) then 
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
  var sign := 0;  
  if s[j] = '-' then
  begin
    sign := -1;
    j += 1;
  end  
  else if s[j] = '+' then
  begin
    sign := 1;
    j += 1;
  end;
  if (j > s.Length) then 
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
  var c := integer(s[j]);
  if (c < 48) or (c > 57) then
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
  Result := c - 48;
  j += 1;  
  while j <= s.Length do
  begin
    c := integer(s[j]);
    if c > 57 then
      break;
    if c < 48 then
      break;
    if Result > 214748364 then
      raise new System.OverflowException(ErrorStringFromResource('Overflow_Int32'));
    Result := Result * 10 + (c - 48);
    j += 1;
  end;
  if Result < 0 then 
    if (Result = -2147483648) and (sign = -1) then
      exit
    else raise new System.OverflowException(ErrorStringFromResource('Overflow_Int32'));
  if sign = -1 then
    Result := -Result;
  while (j <= s.Length) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j <= s.Length then  
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
end;

function TryStrToInt(s: string; var value: integer): boolean;
begin
  Result := True;
  var Res := 0;
  var j := 1;
  while (j <= s.Length) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if (j > s.Length) then 
  begin
    Result := False;
    exit
  end;  
  var sign := 0;  
  if s[j] = '-' then
  begin
    sign := -1;
    j += 1;
  end  
  else if s[j] = '+' then
  begin
    sign := 1;
    j += 1;
  end;
  if (j > s.Length) then 
  begin
    Result := False;
    exit
  end;  
  var c := integer(s[j]);
  if (c < 48) or (c > 57) then
  begin
    Result := False;
    exit
  end;  
  Res := c - 48;
  j += 1;  
  while j <= s.Length do
  begin
    c := integer(s[j]);
    if c > 57 then
      break;
    if c < 48 then
      break;
    if Res > 214748364 then
    begin
      Result := False;
      exit
    end;  
    Res := Res * 10 + (c - 48);
    j += 1;
  end;
  if Res < 0 then 
    if (Res = -2147483648) and (sign = -1) then
      exit
    else 
    begin
      Result := False;
      exit
    end;  
  if sign = -1 then
    Res := -Res;
  while (j <= s.Length) and char.IsWhiteSpace(s[j]) do
    j += 1;
  if j <= s.Length then  
  begin
    Result := False;
    exit
  end;  
  value := Res;
end;

function StrToInt64(s: string): int64;
begin
  Result := Convert.ToInt64(s); 
end;

function StrToFloat(s: string): real;
begin
  Result := Convert.ToDouble(s, nfi);
end;

function TryStrToInt64(s: string; var value: int64): boolean;
begin
  Result := int64.TryParse(s, value);
end;

function TryStrToFloat(s: string; var value: real): boolean;
begin
  try  
    Result := True;
    value := Convert.ToDouble(s, nfi);
  except
    value := 0;
    Result := False;
  end;
end;

function TryStrToFloat(s: string; var value: single): boolean;
begin
  try  
    Result := True;
    value := Convert.ToSingle(s, nfi);
  except
    value := 0;
    Result := False;
  end;
end;

function ReadIntegerFromString(s: string; var from: integer): integer;
begin
  while (from <= s.Length) and char.IsWhiteSpace(s[from]) do
    from += 1;
  if (from > s.Length) then 
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
  var sign := 0;  
  if s[from] = '-' then
  begin
    sign := -1;
    from += 1;
  end  
  else if s[from] = '+' then
  begin
    sign := 1;
    from += 1;
  end;
  if (from > s.Length) then 
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
  var c := integer(s[from]);
  if (c < 48) or (c > 57) then
    raise new System.FormatException(ErrorStringFromResource('Format_InvalidString'));
  Result := c - 48;
  from += 1;  
  while from <= s.Length do
  begin
    c := integer(s[from]);
    if c > 57 then
      break;
    if c < 48 then
      break;
    if Result > 214748364 then
      raise new System.OverflowException(ErrorStringFromResource('Overflow_Int32'));
    Result := Result * 10 + (c - 48);
    from += 1;
  end;
  if Result < 0 then 
    if (Result = -2147483648) and (sign = -1) then
      exit
    else raise new System.OverflowException(ErrorStringFromResource('Overflow_Int32'));
  if sign = -1 then
    Result := -Result;
end;

function ReadWordFromString(s: string; var from: integer): string;
begin
  while (from <= s.Length) and char.IsWhiteSpace(s[from]) do
    from += 1;
  var res := new System.Text.StringBuilder();
  while (from <= s.Length) and not char.IsWhiteSpace(s[from]) do
  begin
    res.Append(s[from]);
    from += 1;
  end;  
  Result := res.ToString;
end;

function ReadRealFromString(s: string; var from: integer): real;
begin
  Result := real.Parse(ReadWordFromString(s, from));  
end;

function TryReadRealFromString(s: string; var from: integer; var res: real): boolean;
begin
  Result := real.TryParse(ReadWordFromString(s, from), res);
end;

function StringIsEmpty(s: string; var from: integer): boolean;
begin
  while (from <= s.Length) and char.IsWhiteSpace(s[from]) do
    from += 1;
  Result := from > s.Length;
end;

function TryReadIntegerFromString(s: string; var from: integer; var res: integer): boolean;
begin
  Result := TryStrToInt(ReadWordFromString(s, from), res);
end;

procedure Val(s: string; var value: integer; var err: integer);
begin
  if TryStrToInt(s, value) then
    err := 0
  else err := 1;  
end;

procedure Val(s: string; var value: real; var err: integer);
begin
  if TryStrToFloat(s, value) then
    err := 0
  else err := 1;  
end;

procedure Val(s: string; var value: single; var err: integer);
begin
  try  
    err := 0;
    value := Convert.ToSingle(s, nfi);
  except
    value := 0;
    err := 1;
  end;
end;

procedure Val(s: string; var value: shortint; var err: integer);
begin
  if shortint.TryParse(s, value) then
    err := 0
  else err := 1;
end;

procedure Val(s: string; var value: smallint; var err: integer);
begin
  if smallint.TryParse(s, value) then
    err := 0
  else err := 1;
end;

procedure Val(s: string; var value: int64; var err: integer);
begin
  if int64.TryParse(s, value) then
    err := 0
  else err := 1;
end;

procedure Val(s: string; var value: byte; var err: integer);
begin
  if byte.TryParse(s, value) then
    err := 0
  else err := 1;
end;

procedure Val(s: string; var value: word; var err: integer);
begin
  if word.TryParse(s, value) then
    err := 0
  else err := 1;
end;

procedure Val(s: string; var value: longword; var err: integer);
begin
  if longword.TryParse(s, value) then
    err := 0
  else err := 1;
end;

procedure Val(s: string; var value: uint64; var err: integer);
begin
  if uint64.TryParse(s, value) then
    err := 0
  else err := 1;
end;

function IntToStr(a: integer): string;
begin
  Result := a.ToString;
end;

function IntToStr(a: int64): string;
begin
  Result := a.ToString;
end;

function FloatToStr(a: real): string;
begin
  Result := a.ToString(nfi);
end;

function Format(formatstring: string; params pars: array of object): string;
begin
  try
    Result := string.Format(nfi, formatstring, pars);
  except
    on e: Exception do
      raise e;
  end;
end;

// -----------------------------------------------------
//                Общие подпрограммы
// -----------------------------------------------------
procedure Inc(var i: integer);
begin
  i += 1;
end;

procedure Inc(var i: integer; n: integer);
begin
  i += n;
end;

procedure Dec(var i: integer);
begin
  i -= 1;
end;

procedure Dec(var i: integer; n: integer);
begin
  i -= n;
end;

procedure Inc(var c: char);
begin
  c := ChrUnicode(word(c) + 1);
end;

procedure Inc(var c: char; n: integer);
begin
  c := ChrUnicode(word(c) + n);
end;

procedure Dec(var c: char);
begin
  c := ChrUnicode(word(c) - 1);
end;

procedure Dec(var c: char; n: integer);
begin
  c := ChrUnicode(word(c) - n);
end;

procedure Inc(var b: byte);
begin
  b += 1;
end;

procedure Inc(var b: byte; n: integer);
begin
  b += n;
end;

procedure Dec(var b: byte);
begin
  b -= 1;
end;

procedure Dec(var b: byte; n: integer);
begin
  b -= n;
end;

procedure Inc(var f: boolean);
begin
  f := not f;
end;

procedure Dec(var f: boolean);
begin
  f := not f;
end;

//------------------------------------------------------------------------------
//PRED-SUCC
function succ(x:  boolean): boolean;
begin
  Result := not x;
end;

function succ(x: byte): byte;
begin
  Result := x + 1;
end;

function succ(x: shortint): shortint;
begin
  Result := x + 1;
end;

function succ(x: smallint): smallint;
begin
  Result := x + 1;
end;

function succ(x: word): word;
begin
  Result := x + 1;
end;

function succ(x: integer): integer;
begin
  Result := x + 1;
end;

function succ(x: longword): longword;
begin
  Result := x + 1;
end;

function succ(x: int64): int64;
begin
  Result := x + 1;
end;

function succ(x: uint64): uint64;
begin
  Result := x + 1;
end;

function succ(x: char): char;
begin
  Result := System.Convert.ToChar(System.Convert.ToUInt16(x) + 1);
end;

function pred(x: boolean): boolean;
begin
  Result := not x;
end;

function pred(x: byte): byte;
begin
  Result := x - 1;
end;

function pred(x: shortint): shortint;
begin
  Result := x - 1;
end;

function pred(x: smallint): smallint;
begin
  Result := x - 1;
end;

function pred(x: word): word;
begin
  Result := x - 1;
end;

function pred(x:  integer): integer;
begin
  Result := x - 1;
end;

function pred(x: longword): longword;
begin
  Result := x - 1;
end;

function pred(x: int64): int64;
begin
  Result := x - 1;
end;

function pred(x: uint64): uint64;
begin
  Result := x - 1;
end;

function pred(x: char): char;
begin
  Result := System.Convert.ToChar(System.Convert.ToUInt16(x) - 1);
end;

procedure Swap<T>(var a, b: T);
begin
  var v := a;
  a := b;
  b := v;
end;

//{{{doc: Начало методов расширения }}}

// -----------------------------------------------------
//>>     Генерация бесконечных последовательностей # Infinite sequences
// -----------------------------------------------------
// Дополнения февраль 2016: Iterate, Step, &Repeat, Cycle

// Возвращает бесконечную рекуррентную последовательность элементов, задаваемую начальным элементом и функцией next
///--
function Iterate<T>(Self: T; next: T -> T): sequence of T; extensionmethod;
begin
  Result := Iterate&<T>(Self,next);
end;

// Возвращает бесконечную рекуррентную последовательность элементов, задаваемую начальным элементом, следующим за ним элементом и функцией next
///--
function Iterate<T>(Self,second: T; next: (T,T) -> T): sequence of T; extensionmethod;
begin
  Result := Iterate&<T>(Self,second,next);
end;

/// Возвращает бесконечную последовательность целых от текущего значения с шагом 1
function Step(Self: integer): sequence of integer; extensionmethod;
begin
  while True do
  begin
    yield Self;
    Self += 1;
  end;
end;

/// Возвращает бесконечную последовательность целых от текущего значения с шагом step
function Step(Self: integer; step: integer): sequence of integer; extensionmethod;
begin
  while True do
  begin
    yield Self;
    Self += step;
  end;
end;

/// Возвращает бесконечную последовательность вещественных от текущего значения с шагом step
function Step(Self: real; step: real): sequence of real; extensionmethod;
begin
  while True do
  begin
    yield Self;
    Self += step;
  end;
end;

// Возвращает бесконечную последовательность элементов, совпадающих с данным
///--
function &Repeat<T>(Self: T): sequence of T; extensionmethod;
begin
  while True do
    yield Self;
end;

/// Повторяет последовательность бесконечное число раз
function Cycle<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  while True do
  begin
    foreach var x in Self do
      yield x;
  end;
end;

//------------------------------------------------------------------------------
//>>     Методы расширения для sequence of T # Extension methods for sequence of T
//------------------------------------------------------------------------------
/// Выводит последовательность на экран, используя delim в качестве разделителя
function Print<T>(Self: sequence of T; delim: string): sequence of T; extensionmethod;
begin
  var g := Self.GetEnumerator();
  if g.MoveNext() then
    write(g.Current);
  while g.MoveNext() do
    if delim<>'' then
      write(delim, g.Current)
    else write(g.Current);
  Result := Self;  
end;

/// Выводит последовательность на экран, используя пробел в качестве разделителя
function Print<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  if typeof(T)=typeof(char) then 
    Result := Self.Print('')
  else  
    Result := Self.Print(PrintDelimDefault);  
end;

/// Выводит последовательность на экран, используя delim в качестве разделителя, и переходит на новую строку
function Println<T>(Self: sequence of T; delim: string): sequence of T; extensionmethod;
begin
  Self.Print(delim);
  Writeln;
  Result := Self;  
end;

/// Выводит последовательность на экран, используя пробел качестве разделителя, и переходит на новую строку
function Println<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  if typeof(T)=typeof(char) then 
    Result := Self.Println('')
  else  
    Result := Self.Println(PrintDelimDefault);  
end;

/// Выводит последовательность строк в файл
function WriteLines(Self: sequence of string; fname: string): sequence of string; extensionmethod;
begin
  WriteLines(fname,Self);
  Result := Self
end;

/// Преобразует элементы последовательности в строковое представление, после чего объединяет их в строку, используя delim в качестве разделителя
function JoinIntoString<T>(Self: sequence of T; delim: string): string; extensionmethod;
begin
  var g := Self.GetEnumerator();
  var sb := new System.Text.StringBuilder('');
  if g.MoveNext() then
    sb.Append(g.Current.ToString());
  while g.MoveNext() do 
    sb.Append(delim + g.Current.ToString());
  Result := sb.ToString;  
end;

/// Преобразует элементы последовательности в строковое представление, после чего объединяет их в строку, используя пробел в качестве разделителя
function JoinIntoString<T>(Self: sequence of T): string; extensionmethod;
begin
  if typeof(T) = typeof(char) then
    Result := Self.JoinIntoString('') 
  else Result := Self.JoinIntoString(' ');  
end;

/// Применяет действие к каждому элементу последовательности
procedure &ForEach<T>(Self: sequence of T; action: T -> ()); extensionmethod;
begin
  foreach x: T in Self do
    action(x);
end;

/// Применяет действие к каждому элементу последовательности, зависящее от номера элемента
procedure &ForEach<T>(Self: sequence of T; action: (T,integer) -> ()); extensionmethod;
begin
  var i := 0;
  foreach x: T in Self do
  begin
    action(x,i);
    i += 1;
  end;
end;

/// Возвращает отсортированную по возрастанию последовательность
function Sorted<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(x -> x);
end;

/// Возвращает отсортированную по убыванию последовательность
function SortedDescending<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Result := Self.OrderByDescending(x -> x);
end;

/// Возвращает множество HashSet по данной последовательности
function ToHashSet<T>(Self: sequence of T): HashSet<T>; extensionmethod;
begin
  Result := new HashSet<T>(Self);
end;

/// Возвращает множество SortedSet по данной последовательности
function ToSortedSet<T>(Self: sequence of T): SortedSet<T>; extensionmethod;
begin
  Result := new SortedSet<T>(Self);
end;

/// Возвращает LinkedList по данной последовательности
function ToLinkedList<T>(Self: sequence of T): LinkedList<T>; extensionmethod;
begin
  Result := new LinkedList<T>(Self);
end;

// Дополнения февраль 2016: MinBy, MaxBy, TakeLast, Slice, Cartesian, SplitAt, 
//   Partition, ZipTuple, UnZipTuple, Interleave, Numerate, Tabulate, Pairwise, Batch 

/// Возвращает первый элемент последовательности с минимальным значением ключа
function MinBy<T, TKey>(Self: sequence of T; selector: T -> TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');

  var comp := Comparer&<TKey>.Default;
  Result := Self.Aggregate((min,x)-> comp.Compare(selector(x),selector(min))<0 ? x : min);
end;

/// Возвращает первый элемент последовательности с максимальным значением ключа
function MaxBy<T, TKey>(Self: sequence of T; selector: T -> TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');
 
  var comp := Comparer&<TKey>.Default;
  Result := Self.Aggregate((max,x)-> comp.Compare(selector(x),selector(max))>0 ? x : max);
end;

/// Возвращает последний элемент последовательности с минимальным значением ключа
function LastMinBy<T, TKey>(Self: sequence of T; selector: T -> TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');

  var comp := Comparer&<TKey>.Default;
  Result := Self.Aggregate((min,x)-> comp.Compare(selector(x),selector(min))<=0 ? x : min);
end;

/// Возвращает последний элемент последовательности с максимальным значением ключа
function LastMaxBy<T, TKey>(Self: sequence of T; selector: T -> TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');
 
  var comp := Comparer&<TKey>.Default;
  Result := Self.Aggregate((max,x)-> comp.Compare(selector(x),selector(max))>=0 ? x : max);
end;

/// Возвращает последние count элементов последовательности
function TakeLast<T>(Self: sequence of T; count: integer): sequence of T; extensionmethod;
begin
  Result := Self.Reverse.Take(count).Reverse;
end;

/// Возвращает последовательность без последних count элементов 
function SkipLast<T>(self: sequence of T; count: integer := 1): sequence of T; extensionmethod;
begin
  Result := Self.Reverse.Skip(count).Reverse;
end;

/// Декартово произведение последовательностей
function Cartesian<T,T1>(Self: sequence of T; b: sequence of T1): sequence of (T,T1); extensionmethod;
begin
  if b=nil then
    raise new System.ArgumentNullException('b');

  foreach var x in Self do
  foreach var y in b do
    yield (x,y)
  
  //Result := Self.Select(x->b.Select(y->(x,y))).SelectMany(x->x);
end;

/// Декартово произведение последовательностей
function Cartesian<T,T1,T2>(Self: sequence of T; b: sequence of T1; func: (T,T1)->T2): sequence of T2; extensionmethod;
begin
  if b=nil then
    raise new System.ArgumentNullException('b');

  foreach var x in Self do
  foreach var y in b do
    yield func(x,y)

//  Result := Self.Select(x->b.Select(y->(x,y))).SelectMany(x->x).Select(x->func(x[0],x[1]));
end;

/// Разбивает последовательности на две в позиции ind
function SplitAt<T>(Self: sequence of T; ind: integer): (sequence of T,sequence of T); extensionmethod;
begin
  Result := (Self.Take(ind),Self.Skip(ind));
end;

// ToDo: то же для TakeWhile

// ToDo: SequenceCompare

/// Разделяет последовательности на две по заданному условию
function Partition<T>(Self: sequence of T; cond: T->boolean): (sequence of T,sequence of T); extensionmethod;
begin
  Result := (Self.Where(cond),Self.Where(x->not cond(x)));
end;

/// Разделяет последовательности на две по заданному условию, в котором участвует индекс
function Partition<T>(Self: sequence of T; cond: (T,integer)->boolean): (sequence of T,sequence of T); extensionmethod;
begin
  Result := (Self.Where(cond),Self.Where((x,i)->not cond(x,i)));
end;

/// Объединяет две последовательности в последовательность двухэлементных кортежей
function ZipTuple<T,T1>(Self: sequence of T; a: sequence of T1): sequence of (T,T1); extensionmethod;
begin
  if a=nil then
    raise new System.ArgumentNullException('a');
  Result := Self.Zip(a,(x,y)->(x,y));
end;

/// Объединяет три последовательности в последовательность трехэлементных кортежей
function ZipTuple<T,T1,T2>(Self: sequence of T; a: sequence of T1; b: sequence of T2): sequence of (T,T1,T2); extensionmethod;
begin
  if a=nil then
    raise new System.ArgumentNullException('a');
  if b=nil then
    raise new System.ArgumentNullException('b');
  Result := Self.Zip(a,(x,y)->(x,y)).Zip(b,(p,z)->(p[0],p[1],z));
end;

/// Объединяет четыре последовательности в последовательность четырехэлементных кортежей
function ZipTuple<T,T1,T2,T3>(Self: sequence of T; a: sequence of T1; b: sequence of T2; c: sequence of T3): sequence of (T,T1,T2,T3); extensionmethod;
begin
  if a=nil then
    raise new System.ArgumentNullException('a');
  if b=nil then
    raise new System.ArgumentNullException('b');
  if c=nil then
    raise new System.ArgumentNullException('c');
  Result := Self.Zip(a,(x,y)->(x,y)).Zip(b,(p,z)->(p[0],p[1],z)).Zip(c,(p,z)->(p[0],p[1],p[2],z));
end;

/// Разъединяет последовательность двухэлементных кортежей на две последовательности
function UnZipTuple<T,T1>(Self: sequence of (T,T1)): (sequence of T,sequence of T1); extensionmethod;
begin
  Result := (Self.Select(x->x[0]),Self.Select(x->x[1]))
end;

/// Разъединяет последовательность трехэлементных кортежей на три последовательности
function UnZipTuple<T,T1,T2>(Self: sequence of (T,T1,T2)): (sequence of T,sequence of T1,sequence of T2); extensionmethod;
begin
  Result := (Self.Select(x->x[0]),Self.Select(x->x[1]),Self.Select(x->x[2]))
end;

/// Разъединяет последовательность четырехэлементных кортежей на четыре последовательности
function UnZipTuple<T,T1,T2,T3>(Self: sequence of (T,T1,T2,T3)): (sequence of T,sequence of T1,sequence of T2,sequence of T3); extensionmethod;
begin
  Result := (Self.Select(x->x[0]),Self.Select(x->x[1]),Self.Select(x->x[2]),Self.Select(x->x[3]))
end;

// ToDo - сделать UnZipTuple с функцией-проекцией

/// Чередует элементы двух последовательностей
function Interleave<T>(Self: sequence of T; a: sequence of T): sequence of T; extensionmethod;
begin
  if a=nil then
    raise new System.ArgumentNullException('a');
  Result := Self.ZipTuple(a).SelectMany(x->Seq(x[0],x[1]))
end;

/// Чередует элементы трех последовательностей
function Interleave<T>(Self: sequence of T; a,b: sequence of T): sequence of T; extensionmethod;
begin
  if a=nil then
    raise new System.ArgumentNullException('a');
  if b=nil then
    raise new System.ArgumentNullException('b');
  Result := Self.ZipTuple(a,b).SelectMany(x->Seq(x[0],x[1],x[2]))
end;

/// Чередует элементы четырех последовательностей
function Interleave<T>(Self: sequence of T; a,b,c: sequence of T): sequence of T; extensionmethod;
begin
  if a=nil then
    raise new System.ArgumentNullException('a');
  if b=nil then
    raise new System.ArgumentNullException('b');
  if c=nil then
    raise new System.ArgumentNullException('c');
  Result := Self.ZipTuple(a,b,c).SelectMany(x->Seq(x[0],x[1],x[2],x[3]))
end;

/// Нумерует последовательность с единицы
function Numerate<T>(Self: sequence of T): sequence of (integer,T); extensionmethod;
begin
  Result := 1.Step.ZipTuple(Self);
end;

/// Нумерует последовательность с номера from
function Numerate<T>(Self: sequence of T; from: integer): sequence of (integer,T); extensionmethod;
begin
  Result := from.Step.ZipTuple(Self);
end;

/// Табулирует функцию последовательностью
function Tabulate<T,T1>(Self: sequence of T; F: T->T1): sequence of (T,T1); extensionmethod;
begin
  Result := Self.Select(x->(x,f(x)));
end;

/// Превращает последовательность в последовательность пар соседних элементов
function Pairwise<T>(Self: sequence of T): sequence of (T,T); extensionmethod;
begin
  var previous: T;
  var it := Self.GetEnumerator();
  if (it.MoveNext()) then
      previous := it.Current;

  while (it.MoveNext()) do
  begin
    yield (previous,it.Current);
    previous := it.Current;
  end
end;

/// Превращает последовательность в последовательность пар соседних элементов, применяет func к каждой паре полученных элементов и получает новую последовательность 
function Pairwise<T,Res>(Self: sequence of T; func:(T,T)->Res): sequence of Res; extensionmethod;
begin
  var previous: T;
  var it := Self.GetEnumerator();
  if (it.MoveNext()) then
      previous := it.Current;

  while (it.MoveNext()) do
  begin
    yield func(previous,it.Current);
    previous := it.Current;
  end
//  Result := Self.ZipTuple(Self.Skip(1)).Select(x->func(x[0],x[1]));
end;

/// Разбивает последовательность на серии длины size
function Batch<T>(Self: sequence of T; size: integer): sequence of sequence of T; extensionmethod;
begin
  Result := SeqWhile(Self,v->v.Skip(size),v->v.Count>0).Select(v->v.Take(size))
end;

/// Разбивает последовательность на серии длины size и применяет проекцию к каждой серии
function Batch<T,Res>(Self: sequence of T; size: integer; proj: Func<IEnumerable<T>,Res>): sequence of Res; extensionmethod;
begin
  Result := SeqWhile(Self,v->v.Skip(size),v->v.Count>0).Select(v->v.Take(size)).Select(ss->proj(ss));
end;

///--
function SliceSeqImpl<T>(Self: sequence of T; from,step,count: integer): sequence of T;
begin
  if step <= 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_GREATER_0));

  if from < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  Result := Self.Skip(from).Where((x,i)->i mod step = 0)
end;

/// Возвращает срез последовательности от номера from с шагом step > 0
function Slice<T>(Self: sequence of T; from,step: integer): sequence of T; extensionmethod;
begin
  if step <= 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_GREATER_0));

  if from < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  Result := Self.Skip(from).Where((x,i)->i mod step = 0)
end;

/// Возвращает срез последовательности от номера from с шагом step > 0 длины не более count
function Slice<T>(Self: sequence of T; from,step,count: integer): sequence of T; extensionmethod;
begin
  if step <= 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_GREATER_0));

  if from < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  Result := Self.Skip(from).Where((x,i)->i mod step = 0).Take(count)
end;

// Дополнения июль 2016: Incremental
///--
{function IncrementalSeq(Self: sequence of integer): sequence of integer; 
begin
  var iter := Self.GetEnumerator();
  if iter.MoveNext() then
  begin
    var prevItem := iter.Current;
    while iter.MoveNext() do
    begin
      var nextItem := iter.Current;
      yield nextItem - prevItem;
      prevItem := nextItem;
    end
  end
end;

///--
function IncrementalSeq(Self: sequence of real): sequence of real;
begin
  var iter := Self.GetEnumerator();
  if iter.MoveNext() then
  begin
    var prevItem := iter.Current;
    while iter.MoveNext() do
    begin
      var nextItem := iter.Current;
      yield nextItem - prevItem;
      prevItem := nextItem;
    end
  end
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: sequence of integer): sequence of integer; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: array of integer): sequence of integer; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: List<integer>): sequence of integer; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: LinkedList<integer>): sequence of integer; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: sequence of real): sequence of real; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: array of real): sequence of real; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: List<real>): sequence of real; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: LinkedList<real>): sequence of real; extensionmethod;
begin
  Result := IncrementalSeq(Self);
end;}

/// Возвращает последовательность разностей соседних элементов исходной последовательности. В качестве функции разности используется func
function Incremental<T,T1>(Self: sequence of T; func: (T,T)->T1): sequence of T1; extensionmethod;
begin
  var iter := Self.GetEnumerator();
  if iter.MoveNext() then
  begin
    var prevItem := iter.Current;
    while iter.MoveNext() do
    begin
      var nextItem := iter.Current;
      yield func(prevItem,nextItem);
      prevItem := nextItem;
    end
  end
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности. В качестве функции разности используется func
function Incremental<T,T1>(Self: sequence of T; func: (T,T,integer)->T1): sequence of T1; extensionmethod;
begin
  var iter := Self.GetEnumerator();
  if iter.MoveNext() then
  begin
    var ind := 0;
    var prevItem := iter.Current;
    while iter.MoveNext() do
    begin
      var nextItem := iter.Current;
      ind += 1;
      yield func(prevItem,nextItem,ind);
      prevItem := nextItem;
    end
  end
end;

// -----------------------------------------------------
//>>     Методы расширения типа List<T> # Extension methods for List T
// -----------------------------------------------------

/// Перемешивает элементы списка случайным образом
function Shuffle<T>(Self: List<T>): List<T>; extensionmethod;
begin
  var n := Self.Count;
	for var i:=0 to n-1 do
	begin
	  var r := PABCSystem.Random(n);
    var v := Self[i];
    Self[i] := Self[r];
    Self[r] := v;
  end;
	Result := Self;  
end;

///-- 
function CreateSliceFromListInternal<T>(Self: List<T>; from,step,count: integer): List<T>;
begin
  Result := new List<T>(count);
  
  var f := from;
  for var i:=0 to count-1 do
  begin
    Result.Add(Self[f]);
    f += step;
  end;
end;

///-- 
procedure CorrectCountForSlice(Len,from,step: integer; var count: integer);
begin
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));

  if count < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_COUNT_MUST_BE_GREATER_0));

  if (from < 0) or (from > Len - 1) then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  var cnt := step > 0 ? Len - from : from + 1; 
  var cntstep := (cnt-1) div abs(step) + 1;
  if count > cntstep then 
    count := cntstep;
end;

///-- 
function SliceListImpl<T>(Self: List<T>; from,step,count: integer): List<T>;
begin
  CorrectCountForSlice(Self.Count,from,step,count);
  Result := CreateSliceFromListInternal(Self,from,step,count);
end;

/// Возвращает срез списка от индекса from с шагом step
function Slice<T>(Self: List<T>; from,step: integer): List<T>; extensionmethod;
begin
  Result := SliceListImpl(Self,from,step,integer.MaxValue);
end;

/// Возвращает срез списка от индекса from с шагом step длины не более count
function Slice<T>(Self: List<T>; from,step,count: integer): List<T>; extensionmethod;
begin
  Result := SliceListImpl(Self,from,step,count);
end;

/// Удаляет последний элемент. Если элементов нет, генерирует исключение
function RemoveLast<T>(Self: List<T>): List<T>; extensionmethod;
begin
  Self.RemoveAt(Self.Count - 1);
  Result := Self;
end;

procedure CorrectFromTo(situation: integer; Len: integer; var from,&to: integer; step: integer);
begin
  if step>0 then
  begin
    case situation of
  1: from := 0;
  2: &to := Len;
  3: begin
       from := 0;
       &to := Len;
     end;
    end;  
  end
  else
  begin
    case situation of
  1: from := Len - 1;
  2: &to := -1;
  3: begin
       from := Len - 1;
       &to := -1;
     end;
    end;
  end;
end;

///--
function CorrectFromToAndCalcCountForSystemSliceQuestion(situation: integer; Len: integer; var from,&to: integer; step: integer): integer;
begin
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));

  CorrectFromTo(situation,Len,from,&to,step);

  if step>0 then
  begin
    if from<0 then
      from += (step - from - 1) div step * step;
    // from может оказаться > Len - 1
    var m := min(Len,&to);
    if from >= m then 
      Result := 0
    else Result := (m - from - 1) div step + 1  
  end
  else
  begin
    if from > Len - 1 then
      from -= (from - Len - step) div step * step;
    // from может оказаться < 0   
    var m := max(&to,-1);
    if from <= m then
      Result := 0
    else Result := (from - m - 1) div (-step) + 1
  end;
end;

///--
function CheckAndCorrectFromToAndCalcCountForSystemSlice(situation: integer; Len: integer; var from,&to: integer; step: integer): integer;
begin
// situation = 0 - все параметры присутствуют
// situation = 1 - from отсутствует
// situation = 2 - to отсутствует
// situation = 3 - from и to отсутствуют
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));

  if (situation=0) or (situation=2) then
    if (from < 0) or (from > Len - 1) then
      raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  if (situation=0) or (situation=1) then
    if (&to < -1) or (&to > Len) then
      raise new ArgumentException(GetTranslation(PARAMETER_TO_OUT_OF_RANGE));

  CorrectFromTo(situation,Len,from,&to,step);

  var count: integer;
  
  if step>0 then
  begin
    var cnt := &to - from;
    if cnt<=0 then 
      count := 0
    else count := (cnt-1) div step + 1;
  end
  else
  begin
    var cnt := from - &to;
    if cnt<=0 then 
      count := 0
    else count := (cnt-1) div (-step) + 1;
  end;

  Result := count;
end;

///--
procedure CheckStepAndCorrectFromTo(situation: integer; Len: integer; var from,&to: integer; step: integer);
begin
// situation = 0 - все параметры присутствуют
// situation = 1 - from отсутствует
// situation = 2 - to отсутствует
// situation = 3 - from и to отсутствуют
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));

  {if (situation=0) or (situation=2) then
    if (from < 0) or (from > Len - 1) then
      raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  if (situation=0) or (situation=1) then
    if (&to < -1) or (&to > Len) then
      raise new ArgumentException(GetTranslation(PARAMETER_TO_OUT_OF_RANGE));}

  CorrectFromTo(situation,Len,from,&to,step);
end;

///-- 
function SystemSliceListImpl<T>(Self: List<T>; situation: integer; from,&to: integer; step: integer := 1): List<T>;
begin
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation,Self.Count,from,&to,step);

  Result := CreateSliceFromListInternal(Self,from,step,count);
end;

///--
function SystemSlice<T>(Self: List<T>; situation: integer; from,&to: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImpl(Self,situation,from,&to,1);
end;

///--
function SystemSlice<T>(Self: List<T>; situation: integer; from,&to,step: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImpl(Self,situation,from,&to,step);
end;

///-- 
function SystemSliceListImplQuestion<T>(Self: List<T>; situation: integer; from,&to: integer; step: integer := 1): List<T>;
begin
  var count := CorrectFromToAndCalcCountForSystemSliceQuestion(situation,Self.Count,from,&to,step);
  
  Result := CreateSliceFromListInternal(Self,from,step,count);
end;

///--
function SystemSliceQuestion<T>(Self: List<T>; situation: integer; from,&to: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImplQuestion(Self,situation,from,&to,1);
end;

///--
function SystemSliceQuestion<T>(Self: List<T>; situation: integer; from,&to,step: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImplQuestion(Self,situation,from,&to,step);
end;
// -----------------------------------------------------
//>>     Методы расширения типа array [,] of T # Extension methods for array [,] of T
// -----------------------------------------------------
/// Количество строк в двумерном массиве
function RowCount<T>(Self: array [,] of T): integer; extensionmethod;
begin
  Result := Self.GetLength(0);
end;

/// Количество столбцов в двумерном массиве
function ColCount<T>(Self: array [,] of T): integer; extensionmethod;
begin
  Result := Self.GetLength(1);
end;

function Print<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod;
begin
	for var i:=0 to Self.RowCount-1 do
	begin
    for var j:=0 to Self.ColCount-1 do
    begin
      var elem := Self[i,j];
      var s := StructuredObjectToString(elem);
      Write(s.PadLeft(w));
    end;
    Writeln;  
  end;
	Result := Self;  
end;

function Print(Self: array [,] of real; w: integer := 7; f: integer := 2): array [,] of real; extensionmethod;
begin
	for var i:=0 to Self.RowCount-1 do
	begin
    for var j:=0 to Self.ColCount-1 do
      Write(FormatValue(Self[i,j],w,f));
    Writeln;  
  end;
	Result := Self;  
end;

function Println<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod;
begin
  Self.Print(w);
	Result := Self;  
end;

function Println(Self: array [,] of real; w: integer := 7; f: integer := 2): array [,] of real; extensionmethod;
begin
  Self.Print(w,f);
	Result := Self;  
end;

/// k-тая строка двумерного массива
function Row<T>(Self: array [,] of T; k: integer): array of T; extensionmethod;
begin
  var n := Self.ColCount;
  var res := new T[n];
  for var j:=0 to n-1 do
    res[j] := Self[k,j];
  Result := res;
end;

/// k-тый столбец двумерного массива
function Col<T>(Self: array [,] of T; k: integer): array of T; extensionmethod;
begin
  var m := Self.RowCount;
  var res := new T[m];
  for var i:=0 to m-1 do
    res[i] := Self[i,k];
  Result := res;
end;

/// k-тая строка двумерного массива как последовательность
function RowSeq<T>(Self: array [,] of T; k: integer): sequence of T; extensionmethod;
begin
  for var j:=0 to Self.ColCount-1 do
    yield Self[k,j];
end;

/// k-тый столбец двумерного массива как последовательность
function ColSeq<T>(Self: array [,] of T; k: integer): sequence of T; extensionmethod;
begin
  for var i:=0 to Self.RowCount-1 do
    yield Self[i,k];
end;

/// Возвращает последовательность строк двумерного массива 
function Rows<T>(Self: array [,] of T): sequence of sequence of T; extensionmethod;
begin
  for var i:=0 to Self.RowCount-1 do
    yield Self.RowSeq(i);
end;

/// Возвращает последовательность столбцов двумерного массива 
function Cols<T>(Self: array [,] of T): sequence of sequence of T; extensionmethod;
begin
  for var j:=0 to Self.ColCount-1 do
    yield Self.ColSeq(j);
end;

/// Меняет местами две строки двумерного массива с номерами k1 и k2
procedure SwapRows<T>(Self: array [,] of T; k1,k2: integer); extensionmethod;
begin
  for var j:=0 to Self.ColCount-1 do
    Swap(Self[k1,j],Self[k2,j])
end;

/// Меняет местами два столбца двумерного массива с номерами k1 и k2
procedure SwapCols<T>(Self: array [,] of T; k1,k2: integer); extensionmethod;
begin
  for var i:=0 to Self.RowCount-1 do
    Swap(Self[i,k1],Self[i,k2])
end;


// Реализация операций с матрицами - только после введения RowCount и ColCount
function MatrRandom(m: integer; n: integer; a,b: integer): array [,] of integer;
begin
  Result := new integer[m,n];
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
    Result[i,j] := Random(a,b);
end;

function MatrRandomInteger(m: integer; n: integer; a,b: integer): array [,] of integer;
begin
  Result := new integer[m,n];
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
    Result[i,j] := Random(a,b);
end;

function MatrRandomReal(m: integer; n: integer; a,b: real): array [,] of real;
begin
  Result := new real[m,n];
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
    Result[i,j] := Random()*(b-a) + a;
end;

function MatrFill<T>(m,n: integer; x: T): array [,] of T;
begin
  Result := new T[m,n];
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
    Result[i,j] := x;
end;

function MatrGen<T>(m,n: integer; gen: (integer,integer)->T): array [,] of T;
begin
  Result := new T[m,n];
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
    Result[i,j] := gen(i,j);
end;

function Transpose<T>(a: array [,] of T): array [,] of T;
begin
  var m := a.RowCount;
  var n := a.ColCount;
  Result := new T[n,m];
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
    Result[i,j] := a[j,i]
end;

// -----------------------------------------------------
//>>     Методы расширения типа array of T # Extension methods for array of T
// -----------------------------------------------------

// Дополнения февраль 2016: Shuffle, AdjacentFind, IndexMin, IndexMax, Replace, Transform
//   Статические методы - в методы расширения: BinarySearch, ConvertAll, Find, FindIndex, FindAll,  
//   FindLast, FindLastIndex, IndexOf, Contains, LastIndexOf, Reverse, Sort

/// Перемешивает элементы массива случайным образом
function Shuffle<T>(Self: array of T): array of T; extensionmethod;
begin
  var n := Self.Length;
	for var i:=0 to n-1 do
	  Swap(Self[i],Self[Random(n)]);
	Result := Self;  
end;

/// Находит первую пару подряд идущих одинаковых элементов и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: array of T; start: integer := 0): integer; extensionmethod;
begin
  Result := -1;
  for var i:=start to Self.Length-2 do
    if Self[i]=Self[i+1] then 
    begin
      Result := i;
      exit;
    end;
end;

/// Находит первую пару подряд идущих одинаковых элементов, используя функцию сравнения eq, и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: array of T; eq: (T,T)->boolean; start: integer := 0): integer; extensionmethod;
begin
  Result := -1;
  for var i:=start to Self.Length-2 do
    if eq(Self[i],Self[i+1]) then 
    begin
      Result := i;
      exit;
    end;
end;

///--
type
  AdjGroupClass<T> = class
  private
    cur: T;
    enm: IEnumerator<T>;
    fin: boolean;
  public
    constructor Create(a: sequence of T);
    begin
      enm := a.GetEnumerator();
      fin := enm.MoveNext;
      if fin then
        cur := enm.Current;
    end;
    
    function TakeGroup: sequence of T;
    begin
      yield cur;
      fin := enm.movenext;
      while fin do
      begin
        if enm.current = cur then
          yield enm.current
        else
        begin
          cur := enm.Current;
          break;
        end;
        fin := enm.movenext;
      end;  
    end;
  end;

/// Группирует одинаковые подряд идущие элементы, получая последовательность последовательностей 
function AdjacentGroup<T>(Self: sequence of T): sequence of sequence of T; extensionmethod;
begin
  var c := new AdjGroupClass<T>(Self);
  while c.fin do
    yield c.TakeGroup();
end;

//ToDo Сделать AdjacentGroup с функцией сравнения

/// Возвращает минимальный элемент 
function Min<T>(Self: array of T): T; extensionmethod; where T: System.IComparable<T>;
begin
  Result := Self[0];
  for var i:=1 to Self.Length-1 do
    if Self[i].CompareTo(Result)<0 then 
      Result := Self[i];
end;

/// Возвращает максинимальный элемент 
function Max<T>(Self: array of T): T; extensionmethod; where T: System.IComparable<T>;
begin
  Result := Self[0];
  for var i:=1 to Self.Length-1 do
    if Self[i].CompareTo(Result)>0 then 
      Result := Self[i];
end;

/// Возвращает минимальный элемент 
function Min(Self: array of integer): integer; extensionmethod; 
begin
  Result := Self[0];
  for var i:=1 to Self.Length-1 do
    if Self[i] < Result then 
      Result := Self[i];
end;

/// Возвращает минимальный элемент 
function Min(Self: array of real): real; extensionmethod; 
begin
  Result := Self[0];
  for var i:=1 to Self.Length-1 do
    if Self[i] < Result then 
      Result := Self[i];
end;

/// Возвращает максимальный элемент 
function Max(Self: array of integer): integer; extensionmethod; 
begin
  Result := Self[0];
  for var i:=1 to Self.Length-1 do
    if Self[i] > Result then 
      Result := Self[i];
end;

/// Возвращает максимальный элемент 
function Max(Self: array of real): real; extensionmethod; 
begin
  Result := Self[0];
  for var i:=1 to Self.Length-1 do
    if Self[i] > Result then 
      Result := Self[i];
end;


/// Возвращает индекс первого минимального элемента начиная с позиции start
function IndexMin<T>(Self: array of T; start: integer := 0): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var min := Self[start];
  Result := start;
  for var i:=start+1 to Self.Length-1 do
    if Self[i].CompareTo(min)<0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс первого максимального элемента начиная с позиции start
function IndexMax<T>(self: array of T; start: integer := 0): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var max := Self[start];
  Result := start;
  for var i:=start+1 to Self.Length-1 do
    if Self[i].CompareTo(max)>0 then 
    begin
      Result := i;
      max := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента
function LastIndexMin<T>(Self: array of T): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var min := Self[Self.Length-1];
  Result := Self.Length-1;
  for var i:=Self.Length-2 downto 0 do
    if Self[i].CompareTo(min)<0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента начиная с позиции start
function LastIndexMin<T>(Self: array of T; start: integer): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var min := Self[start];
  Result := start;
  for var i:=start-1 downto 0 do
    if Self[i].CompareTo(min)<0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента
function LastIndexMax<T>(Self: array of T): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var max := Self[Self.Length-1];
  Result := Self.Length-1;
  for var i:=Self.Length-2 downto 0 do
    if Self[i].CompareTo(max)>0 then 
    begin
      Result := i;
      max := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента начиная с позиции start
function LastIndexMax<T>(Self: array of T; start: integer): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var max := Self[start];
  Result := start;
  for var i:=start-1 downto 0 do
    if Self[i].CompareTo(max)>0 then 
    begin
      Result := i;
      max := Self[i];
    end;
end;

/// Заменяет в массиве все вхождения одного значения на другое
procedure Replace<T>(Self: array of T; oldValue,newValue: T); extensionmethod;
begin
  for var i:=0 to Self.Length-1 do
    if Self[i] = oldValue then
      Self[i] := newValue;
end;

/// Преобразует элементы массива по заданному правилу
procedure Transform<T>(self: array of T; f: T -> T); extensionmethod;
begin
  for var i:=0 to self.Length-1 do
    self[i] := f(self[i]);
end;

/// Выполняет бинарный поиск в отсортированном массиве
function BinarySearch<T>(self: array of T; x: T): integer; extensionmethod;
begin
  Result := System.Array.BinarySearch(self,x);  
end;

/// Преобразует массив одного типа в массив другого типа
function ConvertAll<T,T1>(self: array of T; converter: System.Converter<T,T1>): array of T1; extensionmethod;
begin
  Result := System.Array.ConvertAll(self,converter);  
end;

/// Выполняет поиск первого элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function Find<T>(self: array of T; p: T->boolean): T; extensionmethod;
begin
  Result := System.Array.Find(self,p);  
end;

/// Выполняет поиск индекса первого элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается -1
function FindIndex<T>(self: array of T; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindIndex(self,p);  
end;

/// Выполняет поиск индекса первого элемента в массиве, удовлетворяющего предикату, начиная с индекса start. Если не найден, возвращается -1
function FindIndex<T>(self: array of T; start: integer; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindIndex(self,start,p);  
end;

/// Возвращает в виде массива все элементы, удовлетворяющие предикату
function FindAll<T>(self: array of T; p: T->boolean): array of T; extensionmethod;
begin
  Result := System.Array.FindAll(self,p);  
end;

/// Выполняет поиск последнего элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function FindLast<T>(self: array of T; p: T->boolean): T; extensionmethod;
begin
  Result := System.Array.FindLast(self,p);  
end;

/// Выполняет поиск индекса последнего элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function FindLastIndex<T>(self: array of T; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindLastIndex(self,p);  
end;

/// Выполняет поиск индекса последнего элемента в массиве, удовлетворяющего предикату, начиная с индекса start. Если не найден, возвращается нулевое значение соответствующего типа
function FindLastIndex<T>(self: array of T; start: integer; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindLastIndex(self,start,p);  
end;

/// Возвращает индекс первого вхождения элемента или -1 если элемент не найден
function IndexOf<T>(self: array of T; x: T): integer; extensionmethod;
begin
  Result := System.Array.IndexOf(self,x);  
end;

/// Возвращает индекс первого вхождения элемента начиная с индекса start или -1 если элемент не найден
function IndexOf<T>(self: array of T; x: T; start: integer): integer; extensionmethod;
begin
  Result := System.Array.IndexOf(self,x,start);  
end;

/// Возвращает индекс последнего вхождения элемента или -1 если элемент не найден
function LastIndexOf<T>(self: array of T; x: T): integer; extensionmethod;
begin
  Result := System.Array.LastIndexOf(self,x);  
end;

/// Возвращает индекс последнего вхождения элемента начиная с индекса start или -1 если элемент не найден
function LastIndexOf<T>(self: array of T; x: T; start: integer): integer; extensionmethod;
begin
  Result := System.Array.LastIndexOf(self,x,start);  
end;

/// Сортирует массив по возрастанию
procedure Sort<T>(self: array of T); extensionmethod;
begin
  System.Array.Sort(self);  
end;

/// Сортирует массив по возрастанию, используя cmp в качестве функции сравнения элементов
procedure Sort<T>(self: array of T; cmp: (T,T) -> integer); extensionmethod;
begin
  System.Array.Sort(self,cmp);  
end;

/// Возвращает индекс последнего элемента массива
function High(self: System.Array); extensionmethod := High(Self);

/// Возвращает индекс первого элемента массива
function Low(self: System.Array); extensionmethod := Low(Self);

///-- 
function CreateSliceFromArrayInternal<T>(Self: array of T; from,step,count: integer): array of T;
begin
  Result := new T[count];
  
  var f := from;
  for var i:=0 to count-1 do
  begin
    Result[i] := Self[f];
    f += step;
  end;
end;

///-- 
function SliceArrayImpl<T>(Self: array of T; from,step,count: integer): array of T;
begin
  {if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));

  if (from < 0) or (from > Self.Length - 1) then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  var cnt := step > 0 ? Self.Length - from : from + 1; 
  var cntstep := (cnt-1) div abs(step) + 1;
  if count > cntstep then 
    count := cntstep;}
    
  CorrectCountForSlice(Self.Length,from,step,count);  
    
  Result := CreateSliceFromArrayInternal(Self,from,step,count)
end;

/// Возвращает срез массива от индекса from с шагом step
function Slice<T>(Self: array of T; from,step: integer): array of T; extensionmethod;
begin
  Result := SliceArrayImpl(Self,from,step,integer.MaxValue);
end;

/// Возвращает срез массива от индекса from с шагом step длины не более count
function Slice<T>(Self: array of T; from,step,count: integer): array of T; extensionmethod;
begin
  Result := SliceArrayImpl(Self,from,step,count);
end;

///-- 
function SystemSliceArrayImpl<T>(Self: array of T; situation: integer; from,&to: integer; step: integer := 1): array of T;
begin
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation,Self.Length,from,&to,step);

  Result := CreateSliceFromArrayInternal(Self,from,step,count)
end;

///--
function SystemSlice<T>(Self: array of T; situation: integer; from,&to: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImpl(Self,situation,from,&to,1);
end;

///--
function SystemSlice<T>(Self: array of T; situation: integer; from,&to,step: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImpl(Self,situation,from,&to,step);
end;

///-- 
function SystemSliceArrayImplQuestion<T>(Self: array of T; situation: integer; from,&to: integer; step: integer := 1): array of T;
begin
  var count := CorrectFromToAndCalcCountForSystemSliceQuestion(situation,Self.Length,from,&to,step);
  
  Result := CreateSliceFromArrayInternal(Self,from,step,count);
end;

///--
function SystemSliceQuestion<T>(Self: array of T; situation: integer; from,&to: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImplQuestion(Self,situation,from,&to,1);
end;

///--
function SystemSliceQuestion<T>(Self: array of T; situation: integer; from,&to,step: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImplQuestion(Self,situation,from,&to,step);
end;

// -----------------------------------------------------
//>>     Методы расширения типа integer # Extension methods for integer
// -----------------------------------------------------
/// Возвращает квадратный корень числа
function Sqrt(Self: integer): real; extensionmethod;
begin
  Result := Sqrt(Self);
end;

/// Возвращает квадрат числа
function Sqr(Self: integer): integer; extensionmethod;
begin
  Result := Sqr(Self);
end;

// Дополнения февраль 2016: IsEven, IsOdd

/// Возвращает, является ли целое четным
function IsEven(Self: integer): boolean; extensionmethod;
begin
	Result := Self mod 2 = 0;
end;

/// Возвращает, является ли целое нечетным
function IsOdd(Self: integer): boolean; extensionmethod;
begin
	Result := Self mod 2 <> 0;
end;

/// Возвращает последовательность чисел от 1 до данного
function Range(Self: integer): sequence of integer; extensionmethod;
begin
  Result := Range(1,Self);  
end;

// Дополнения февраль 2016: &To, &Downto, Times

/// Генерирует последовательность целых от текущего значения до n
function &To(Self: integer; n: integer): sequence of integer; extensionmethod;
begin
  Result := Range(Self, n);
end;

/// Генерирует последовательность целых от текущего значения до n в убывающем порядке
function &Downto(Self: integer; n: integer): sequence of integer; extensionmethod;
begin
  Result := Range(Self, n, -1); 
end;

/// Возвращает последовательность целых 0,1,...n-1
function Times(Self: integer): sequence of integer; extensionmethod;
begin
  Result := Range(0,Self-1);
end;

// -----------------------------------------------------
//>>     Методы расширения типа BigInteger # Extension methods for BigInteger
// -----------------------------------------------------
/// Возвращает квадратный корень числа
function Sqrt(Self: BigInteger): real; extensionmethod;
begin
  Result := Sqrt(real(Self));
end;

// -----------------------------------------------------
//>>     Методы расширения типа real # Extension methods for real
// -----------------------------------------------------
/// Возвращает квадратный корень числа
function Sqrt(Self: real): real; extensionmethod;
begin
  Result := Sqrt(Self);
end;

/// Возвращает квадрат числа
function Sqr(Self: real): real; extensionmethod;
begin
  Result := Sqr(Self);
end;

/// Возвращает число, округленное до ближайшего целого
function Round(Self: real): integer; extensionmethod;
begin
  Result := Round(Self);
end;

/// Возвращает число, округленное до ближайшего длинного целого
function RoundBigInteger(Self: real): BigInteger; extensionmethod;
begin
  Result := RoundBigInteger(Self);
end;

/// Возвращает целую часть вещественного числа
function Trunc(Self: real): integer; extensionmethod;
begin
  Result := Trunc(Self);
end;

/// Возвращает целую часть вещественного числа как длинное целое
function TruncBigInteger(Self: real): BigInteger; extensionmethod;
begin
  Result := TruncBigInteger(Self);
end;

/// Возвращает вещественное, отформатированное к строке с frac цифрами после десятичной точки
function ToString(Self: real; frac: integer): string; extensionmethod;
begin
  if frac<0 then
    raise new System.ArgumentOutOfRangeException('frac','frac<0');
  if frac>=100 then
    raise new System.ArgumentOutOfRangeException('frac','frac>=100');
  Result := Format('{0:f'+frac+'}',Self)
end;


//------------------------------------------------------------------------------
//>>     Методы расширения типа char # Extension methods for char
//------------------------------------------------------------------------------
/// Предыдущий символ
function Pred(Self: char): char; extensionmethod;
begin
  Result := PABCSystem.Pred(Self);
end;

/// Следующий символ
function Succ(Self: char): char; extensionmethod;
begin
  Result := PABCSystem.Succ(Self);
end;

/// Код символа в кодировке Unicode
function Code(Self: char): integer; extensionmethod;
begin
  Result := word(Self);
end;

/// Является ли символ цифрой
function IsDigit(Self: char): boolean; extensionmethod;
begin
  Result := char.IsDigit(Self);
end;

/// Является ли символ буквой
function IsLetter(Self: char): boolean; extensionmethod;
begin
  Result := char.IsLetter(Self);
end;

/// Принадлежит ли символ к категории букв нижнего регистра
function IsLower(Self: char): boolean; extensionmethod;
begin
  Result := char.IsLower(Self);
end;

/// Принадлежит ли символ к категории букв верхнего регистра
function IsUpper(Self: char): boolean; extensionmethod;
begin
  Result := char.IsUpper(Self);
end;

/// Преобразует символ в цифру
function ToDigit(Self: char): integer; extensionmethod;
begin
  Result := OrdUnicode(Self) - OrdUnicode('0');
  if (Result<0) or (Result>=10) then
    raise new System.FormatException('not a Digit');
end;

/// Преобразует символ в нижний регистр
function ToLower(Self: char): char; extensionmethod;
begin
  Result := char.ToLower(Self);
end;

/// Преобразует символ в верхний регистр
function ToUpper(Self: char): char; extensionmethod;
begin
  Result := char.ToUpper(Self);
end;

//------------------------------------------------------------------------------
//>>     Методы расширения типа string # Extension methods for string
//------------------------------------------------------------------------------
/// Считывает целое из строки начиная с позиции from и устанавливает from за считанным значением
function ReadInteger(Self: string; var from: integer): integer; extensionmethod;
begin
  Result := ReadIntegerFromString(Self, from);
end;

/// Считывает вещественное из строки начиная с позиции from и устанавливает from за считанным значением
function ReadReal(Self: string; var from: integer): real; extensionmethod;
begin
  Result := ReadRealFromString(Self, from);
end;

/// Считывает слово из строки начиная с позиции from и устанавливает from за считанным значением
function ReadWord(Self: string; var from: integer): string; extensionmethod;
begin
  Result := ReadwordFromString(Self, from);
end;

/// Преобразует строку в целое
function ToInteger(Self: string): integer; extensionmethod;
begin
  Result := integer.Parse(Self);
end;

/// Преобразует строку в BigInteger
function ToBigInteger(Self: string): BigInteger; extensionmethod;
begin
  Result := BigInteger.Parse(Self);
end;

/// Преобразует строку в вещественное
function ToReal(Self: string): real; extensionmethod;
begin
  Result := real.Parse(Self, nfi);
end;

/// Преобразует строку в массив слов
function ToWords(Self: string; params delim: array of char): array of string; extensionmethod;
begin
  Result := Self.Split(delim, System.StringSplitOptions.RemoveEmptyEntries);
end;

/// Преобразует строку в массив целых
function ToIntegers(Self: string): array of integer; extensionmethod;
begin
  Result := Self.ToWords().Select(s -> StrToInt(s)).ToArray();
end;

/// Преобразует строку в массив вещественных
function ToReals(Self: string): array of real; extensionmethod;
begin
  Result := Self.ToWords().Select(s -> StrToFloat(s)).ToArray();
end;

/// Возвращает инверсию строки
function Inverse(Self: string): string; extensionmethod;
begin
  var sb := new System.Text.StringBuilder(Self.Length);
  for var i:= Self.Length downto 1 do
    sb.Append(Self[i]);
  Result := sb.ToString;
end;

// Дополнения февраль 2016: Matches, MatchValues, Replace, Remove, Right, Left

/// Заменяет в указанной строке все вхождения регулярного выражения указанной строкой замены и возвращает преобразованную строку
function Replace(Self: string; reg,repl: string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
begin
	Result := Regex.Replace(Self,reg,repl,options)
end;

/// Заменяет в указанной строке все вхождения регулярного выражения указанным преобразованием замены и возвращает преобразованную строку
function Replace(Self: string; reg: string; repl: Match -> string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
begin
	Result := Regex.Replace(Self,reg,repl,options)
end;

/// Ищет в указанной строке все вхождения регулярного выражения и возвращает их в виде последовательности элементов типа Match
function Matches(Self: string; reg: string; options: RegexOptions := RegexOptions.None): sequence of Match; extensionmethod;
begin
	Result := (new Regex(reg, options)).Matches(Self).Cast&<Match>();
end;

/// Ищет в указанной строке первое вхождение регулярного выражения и возвращает его в виде строки
function MatchValue(Self: string; reg: string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
begin
	Result := (new Regex(reg, options)).Match(Self).Value;
end;

/// Ищет в указанной строке все вхождения регулярного выражения и возвращает их в виде последовательности строк
function MatchValues(Self: string; reg: string; options: RegexOptions := RegexOptions.None): sequence of string; extensionmethod;
begin
	Result := Self.Matches(reg,options).Select(m->m.Value);
end;

/// Удаляет в строке все вхождения указанных строк
function Remove(Self: string; params targets: array of string): string; extensionmethod;
begin
	var builder := new StringBuilder (Self);

	for var i := 0 to targets.Length-1 do
		builder.Replace (targets[i], String.Empty);

	Result := builder.ToString();
end;

/// Возвращает подстроку, полученную вырезанием из строки length самых правых символов
function Right(Self: string; length: integer): string; extensionmethod;
begin
  length := Max(length, 0);

  if Self.Length > length then
      Result := Self.Substring(Self.Length - length, length)
  else Result := Self;
end;

/// Возвращает подстроку, полученную вырезанием из строки length самых левых символов
function Left(Self: string; length: integer): string; extensionmethod;
begin
  length := Max(length, 0);

  if Self.Length > length then
      Result := Self.Substring(0, length)
  else Result := Self;
end;

///-- 
function CreateSliceFromStringInternal(Self: string; from,step,count: integer): string;
begin
  var res := new StringBuilder(count);
  
  for var i:=0 to count-1 do
  begin
    res.Append(Self[from]);
    from += step;
  end;
  Result := res.ToString;
end;

///-- 
function SliceStringImpl(Self: string; from,step,count: integer): string;
begin
  {if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));

  if (from < 0) or (from > Self.Length - 1) then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));

  var cnt := step > 0 ? Self.Length - from : from + 1; 
  var cntstep := (cnt-1) div abs(step) + 1;
  if count > cntstep then 
    count := cntstep;}
    
  CorrectCountForSlice(Self.Length,from,step,count);
  
  Result := CreateSliceFromStringInternal(Self,from+1,step,count);
end;

/// Возвращает срез строки от индекса from с шагом step
function Slice(Self: string; from,step: integer): string; extensionmethod;
begin
  Result := SliceStringImpl(Self,from,step,integer.MaxValue);
end;

/// Возвращает срез строки от индекса from с шагом step длины не более count
function Slice(Self: string; from,step,count: integer): string; extensionmethod;
begin
  Result := SliceStringImpl(Self,from,step,count);
end;

///-- 
function SystemSliceStringImpl(Self: string; situation: integer; from,&to: integer; step: integer := 1): string;
begin
  var fromv := from-1;
  var tov := &to-1;
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation,Self.Length,fromv,tov,step);

  Result := CreateSliceFromStringInternal(Self,fromv+1,step,count)
end;

///--
function SystemSlice(Self: string; situation: integer; from,&to: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImpl(Self,situation,from,&to,1);
end;

///--
function SystemSlice(Self: string; situation: integer; from,&to,step: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImpl(Self,situation,from,&to,step);
end;

///-- 
function SystemSliceStringImplQuestion(Self: string; situation: integer; from,&to: integer; step: integer := 1): string;
begin
  var fromv := from-1;
  var tov := &to-1;
  
  var count := CorrectFromToAndCalcCountForSystemSliceQuestion(situation,Self.Length,fromv,tov,step);
  
  Result := CreateSliceFromStringInternal(Self,fromv+1,step,count);
end;

///--
function SystemSliceQuestion(Self: string; situation: integer; from,&to: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImplQuestion(Self,situation,from,&to,1);
end;

///--
function SystemSliceQuestion(Self: string; situation: integer; from,&to,step: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImplQuestion(Self,situation,from,&to,step);
end;
//--------------------------------------------
//>>     Методы расширения типа Func # Extension methods for Func
//--------------------------------------------
/// Суперпозиция функций
function Compose<T1, T2, TResult> (Self: T2 -> TResult; composer: T1 -> T2): T1 -> TResult; extensionmethod;
begin
  if composer=nil then
    raise new System.ArgumentNullException('composer');
  var Slf := Self;
  Result := x -> Slf(composer(x));
end;

//------------------------------------------------------------------------------
//>>     Методы расширения типа Complex # Extension methods for Complex
//------------------------------------------------------------------------------
/// Возвращает комплексно сопряженное значение
function Conjugate(Self: Complex): Complex; extensionmethod;
begin
  Result := Complex.Conjugate(Self);
end;

// -----------------------------------------------------------------------------
//>>     Методы расширения IDictionary # Extension methods for IDictionary
// -----------------------------------------------------------------------------
/// Возвращает в словаре значение, связанное с указанным ключом, а если такого ключа нет, то значение по умолчанию
function Get<Key,Value>(Self: IDictionary<Key,Value>; K: Key): Value; extensionmethod;
begin
  var b := Self.TryGetValue(K,Result);
  if not b then 
    Result := default(Value);
end;


//{{{--doc: Конец методов расширения }}}

//------------------------------------------------------------------------------
//          Операции для Func
//------------------------------------------------------------------------------
///--
function operator*<T1, T2, TResult> (Self: T2 -> TResult; composer: T1 -> T2): T1 -> TResult; extensionmethod;
begin
  if composer=nil then
    raise new System.ArgumentNullException('composer');
  Result := Self.Compose(composer);
end;

//------------------------------------------------------------------------------
//          Операции для Tuple
//------------------------------------------------------------------------------
///--
function operator+<T1, T2, T3> (Self: (T1,T2); v: T3): (T1,T2,T3); extensionmethod;
begin
  Result := (Self[0],Self[1],v);
end;

///--
function operator+<T1, T2, T3, T4> (Self: (T1,T2,T3); v: T4): (T1,T2,T3,T4); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],v);
end;

///--
function operator+<T1, T2, T3, T4, T5> (Self: (T1,T2,T3,T4); v: T5): (T1,T2,T3,T4,T5); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],Self[3],v);
end;

///--
function operator+<T1, T2, T3, T4, T5, T6> (Self: (T1,T2,T3,T4,T5); v: T6): (T1,T2,T3,T4,T5,T6); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],Self[3],Self[4],v);
end;

///--
function operator+<T1, T2, T3, T4, T5, T6, T7> (Self: (T1,T2,T3,T4,T5,T6); v: T7): (T1,T2,T3,T4,T5,T6,T7); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],Self[3],Self[4],Self[5],v);
end;

///--
function operator=<T1, T2> (Self: (T1,T2); v: (T1,T2)); extensionmethod := Self.Equals( v ) ;
///--
function operator<><T1, T2> (Self: (T1,T2); v: (T1,T2)); extensionmethod := not Self.Equals( v );
///--
function CompareToTup2<T1,T2>(v1: (T1,T2); v2: (T1,T2)) := (v1 as System.IComparable).CompareTo(v2);
///--
function operator<<T1, T2> (Self: (T1,T2); v: (T1,T2)); extensionmethod := CompareToTup2(Self,v) < 0;
///--
function operator<=<T1, T2> (Self: (T1,T2); v: (T1,T2)); extensionmethod := CompareToTup2(Self,v) <= 0;
///--
function operator><T1, T2> (Self: (T1,T2); v: (T1,T2)); extensionmethod := CompareToTup2(Self,v) > 0;
///--
function operator>=<T1, T2> (Self: (T1,T2); v: (T1,T2)); extensionmethod := CompareToTup2(Self,v) >= 0;

///--
function operator=<T1, T2, T3> (Self: (T1,T2,T3); v: (T1,T2,T3)); extensionmethod := Self.Equals( v ) ;
///--
function operator<><T1, T2, T3> (Self: (T1,T2,T3); v: (T1,T2,T3)); extensionmethod := not Self.Equals( v );
///--
function CompareToTup3<T1,T2,T3>(v1: (T1,T2,T3); v2: (T1,T2,T3)) := (v1 as System.IComparable).CompareTo(v2);
///--
function operator<<T1,T2,T3> (Self: (T1,T2,T3); v: (T1,T2,T3)); extensionmethod := CompareToTup3(Self,v) < 0;
///--
function operator<=<T1,T2,T3> (Self: (T1,T2,T3); v: (T1,T2,T3)); extensionmethod := CompareToTup3(Self,v) <= 0;
///--
function operator><T1,T2,T3> (Self: (T1,T2,T3); v: (T1,T2,T3)); extensionmethod := CompareToTup3(Self,v) > 0;
///--
function operator>=<T1,T2,T3> (Self: (T1,T2,T3); v: (T1,T2,T3)); extensionmethod := CompareToTup3(Self,v) >= 0;

///--
function operator=<T1, T2, T3, T4> (Self: (T1,T2,T3,T4); v: (T1,T2,T3,T4)); extensionmethod := Self.Equals( v ) ;
///--
function operator<><T1, T2, T3, T4> (Self: (T1,T2,T3,T4); v: (T1,T2,T3,T4)); extensionmethod := not Self.Equals( v );
///--
function CompareToTup4<T1,T2,T3,T4>(v1: (T1,T2,T3,T4); v2: (T1,T2,T3,T4)) := (v1 as System.IComparable).CompareTo(v2);
///--
function operator<<T1,T2,T3,T4> (Self: (T1,T2,T3,T4); v: (T1,T2,T3,T4)); extensionmethod := CompareToTup4(Self,v) < 0;
///--
function operator<=<T1,T2,T3,T4> (Self: (T1,T2,T3,T4); v: (T1,T2,T3,T4)); extensionmethod := CompareToTup4(Self,v) <= 0;
///--
function operator><T1,T2,T3,T4> (Self: (T1,T2,T3,T4); v: (T1,T2,T3,T4)); extensionmethod := CompareToTup4(Self,v) > 0;
///--
function operator>=<T1,T2,T3,T4> (Self: (T1,T2,T3,T4); v: (T1,T2,T3,T4)); extensionmethod := CompareToTup4(Self,v) >= 0;


// --------------------------------------------
//      Методы расширения типа Tuple # Extension methods for Tuple
// -------------------------------------------
// Дополнения февраль 2016

// Добавляет поле к кортежу
function Add<T1, T2, T3> (Self: (T1,T2); v: T3): (T1,T2,T3); extensionmethod;
begin
  Result := (Self[0],Self[1],v);
end;

// Добавляет поле к кортежу
function Add<T1, T2, T3, T4> (Self: (T1,T2,T3); v: T4): (T1,T2,T3,T4); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],v);
end;

// Добавляет поле к кортежу
function Add<T1, T2, T3, T4, T5> (Self: (T1,T2,T3,T4); v: T5): (T1,T2,T3,T4,T5); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],Self[3],v);
end;

// Добавляет поле к кортежу
function Add<T1, T2, T3, T4, T5, T6> (Self: (T1,T2,T3,T4,T5); v: T6): (T1,T2,T3,T4,T5,T6); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],Self[3],Self[4],v);
end;

// Добавляет поле к кортежу
function Add<T1, T2, T3, T4, T5, T6, T7> (Self: (T1,T2,T3,T4,T5,T6); v: T7): (T1,T2,T3,T4,T5,T6,T7); extensionmethod;
begin
  Result := (Self[0],Self[1],Self[2],Self[3],Self[4],Self[5],v);
end;

{// Определяет, есть ли указанный элемент в массиве
 function Contains<T>(self: array of T; x: T): boolean; extensionmethod;
 begin
   Result := System.Array.IndexOf(self,x)<>-1;  
 end;}

{// Изменяет порядок элементов в массиве на обратный
 procedure Reverse<T>(self: array of T); extensionmethod;
 begin
   System.Array.Reverse(self);  
 end;

// Изменяет порядок элементов на обратный в диапазоне массива, заданном началом и длиной 
 procedure Reverse<T>(self: array of T; index,len: integer); extensionmethod;
 begin
   System.Array.Reverse(self,index,len);  
 end;}


//{{{ Конец секции реализации прикладных методов }}}

//{{{ Начало секции реализации внутренних системных методов }}}
// -----------------------------------------------------
//                  Standard Exceptions: implementation
// -----------------------------------------------------
constructor BadGenericInstanceParameterException.Create(ActualParameterType: System.Type);
begin
  InstanceType := ActualParameterType;
end;

function CanNotUseTypeForPointersException.ToString: string;
begin
  result := InstanceType.FullName + ' непригоден для указателей.';
end;

function CanNotUseTypeForTypedFilesException.ToString: string;
begin
  result := InstanceType.FullName + ' непригоден для типизированных файлов.';
end;

function CanNotUseTypeForFilesException.ToString: string;
begin
  result := InstanceType.FullName + ' непригоден для файлов.';
end;

// -----------------------------------------------------
// Internal System subprograms: implementation
// -----------------------------------------------------
function check_in_range(val: int64; low, up: int64): int64;
begin
  if (val < low) or (val > up) then
    raise new RangeException(GetTranslation(RANGE_ERROR_MESSAGE));
  Result := val;
end;

function check_in_range_char(val: char; low, up: char): char;
begin
  if (val < low) or (val > up) then
    raise new RangeException(GetTranslation(RANGE_ERROR_MESSAGE));
  Result := val;
end;

function RunTimeSizeOf(t: System.Type): integer;
var
  t1: System.Type;
  elem: object;
  fa: array of System.Reflection.FieldInfo;
  NullBasedArray: System.Array;
  fi: System.Reflection.FieldInfo;
begin
  if t.IsPrimitive or t.IsEnum then 
    //begin
    case System.Type.GetTypeCode(t) of
      TypeCode.Boolean: Result := sizeof(Boolean);
      TypeCode.Byte:    Result := sizeof(Byte);
      TypeCode.Char:    Result := 1;//sizeof(Char);
      TypeCode.Decimal: Result := sizeof(Decimal);
      TypeCode.Double:  Result := sizeof(Double);
      TypeCode.Int16:   Result := sizeof(Int16);
      TypeCode.Int32:   Result := sizeof(Int32);
      TypeCode.Int64:   Result := sizeof(Int64);
      TypeCode.UInt16:  Result := sizeof(UInt16);
      TypeCode.UInt32:  Result := sizeof(UInt32);
      TypeCode.UInt64:  Result := sizeof(UInt64);
      TypeCode.SByte:   Result := sizeof(SByte);
      TypeCode.Single:  Result := sizeof(Single);
    else if t.IsEnum then result := sizeof(integer);
    end//;
  //end 
  else
  if t.IsValueType then // it is a record
  begin
    //elem := Activator.CreateInstance(t); //ssyy commented
    fa := t.GetFields;
    Result := 0;
    for var i := 0 to fa.Length - 1 do
      if not fa[i].IsStatic then 
        Result := Result + RunTimeSizeOf(fa[i].FieldType)
  end
  else if t = typeof(string) then
    Result := 0
  else
  if t = typeof(TypedSet) then
  begin
    //elem := Activator.CreateInstance(t); //ssyy commented
    Result := 256 div 8;
  end
  else
  begin
    fi := t.GetField(InternalNullBasedArrayName);
    if fi = nil then 
      raise new SystemException(GetTranslation(BAD_TYPE_IN_RUNTIMESIZEOF));
    elem := Activator.CreateInstance(t);
    NullBasedArray := GetNullBasedArray(elem);
    
    t1 := NullBasedArray.GetType.GetElementType;
    Result := RunTimeSizeOf(t1) * NullBasedArray.Length;
  end;
end;

// -----------------------------------------------------------------------------
//                Внутренние функции для работы с короткими строками 
// -----------------------------------------------------------------------------
[System.Diagnostics.DebuggerStepThrough]
function GetCharInShortString(s: string; ind, n: integer): char;
begin
  if ind < 0 then 
    raise new IndexOutOfRangeException;
  if ind = 0 then 
    Result := char(s.Length)
  else 
    try
      Result := s[ind];
    except 
      on e: Exception do
        if ind > n then raise;
    end;
end;

[System.Diagnostics.DebuggerStepThrough]
function SetCharInShortString(s: string; ind, n: integer; c: char): string;
begin
  if ind < 0 then 
    raise new IndexOutOfRangeException;
  if ind <> 0 then
  begin
    var sb := new System.Text.StringBuilder();
    sb.Append(s);
    if ind - 1 < sb.Length then
      sb[ind - 1] := c;
    if ind > n then raise new IndexOutOfRangeException;
    Result := sb.ToString;
  end
  else
  begin
    //Result := s.PadRight(integer(c));
    raise new IndexOutOfRangeException;
  end;
end;

[System.Diagnostics.DebuggerStepThrough]
function ClipShortString(s: string; len: integer): string;
begin
  if s.Length <= len then 
    Result := s
  else Result := s.Substring(0, len);
end;

function GetResourceStream(ResourceFileName: string): Stream;
begin
  result := System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(ResourceFileName);
end;

function FormatValue(value: object; NumOfChars: integer): string;
begin
  if value <> nil then
    Result := StructuredObjectToString(value)
  else
    Result := 'nil';
  Result := Result.PadLeft(NumOfChars);
end;

function FormatValue(value: integer; NumOfChars: integer): string;
begin
  Result := value.ToString;
  Result := Result.PadLeft(NumOfChars);
end;

function FormatValue(value: int64; NumOfChars: integer): string;
begin
  Result := value.ToString;
  Result := Result.PadLeft(NumOfChars);
end;

function FormatValue(value: real; NumOfChars: integer): string;
begin
  Result := value.ToString(nfi);
  Result := Result.PadLeft(NumOfChars);
end;

function FormatValue(value: real; NumOfChars, NumOfSignesAfterDot: integer): string;
begin
  // SSM 31.03.09
  var FmtStr := '{0,' + NumOfChars.ToString + ':f' + abs(NumOfSignesAfterDot).ToString + '}';
  Result := Format(FmtStr, value);
end;

procedure StringDefaultPropertySet(var s: string; index: integer; c: char);
begin
  //s := string.Concat( s.Copy(0,index-1), c, s.Copy(index, s.Length - index));
  //s := string.Concat(s.Substring(0, index), c, s.Substring(index + 1));
  //ChangeChar(s[index + 1], c);
  var chars := s.ToCharArray;
  chars[index] := c;
  s := new String(chars);
end;

procedure ChangeChar(var c: char; val: char);
begin
  c := val;
end;

procedure CheckCanUsePointerOnType(T: System.Type);
var
  fields: array of System.Reflection.FieldInfo;
begin
  if T.IsPointer then
  begin
    CheckCanUsePointerOnType(T.GetElementType());
    exit;
  end;
  if T.IsValueType then
  begin
    fields := T.GetFields();
    foreach var fi in fields do
      if not fi.IsStatic then
        CheckCanUsePointerOnType(fi.FieldType);
    exit;
  end;
  raise new CanNotUseTypeForPointersException(T);
end;

procedure CheckCanUseTypeForFiles(T: System.Type; FileIsBinary: boolean);
var
  fields: array of System.Reflection.FieldInfo;
begin
  if T.IsPrimitive or T.IsEnum then
    exit;
  if T = typeof(TypedSet) then
  begin
    //TODO: обработать множества
    raise new CanNotUseTypeForFilesException(T);
  end;
  if T = typeof(string) then
    if FileIsBinary then
      exit
    else
      raise new CanNotUseTypeForTypedFilesException(T);
  if T.IsValueType then
  begin
    fields := T.GetFields();
    foreach var fi in fields do
      if not fi.IsStatic then
        CheckCanUseTypeForFiles(fi.FieldType, FileIsBinary);
    exit;
  end;
  raise new CanNotUseTypeForFilesException(T);
end;

procedure CheckCanUseTypeForBinaryFiles(T: System.Type);
begin
  CheckCanUseTypeForFiles(T, true);
end;

procedure CheckCanUseTypeForTypedFiles(T: System.Type);
begin
  CheckCanUseTypeForFiles(T, false);
end;

function RuntimeDetermineType(T: System.Type): byte;
begin
  result := 0;
  if T.IsValueType and (T.GetMethod('$Init$') <> nil) then
  begin
    result := 1;
    exit;
  end;
  if T = typeof(string) then
  begin
    result := 2;
    exit;
  end;
  if T = typeof(TypedSet) then
  begin
    result := 3;
    exit;
  end;
  if T = typeof(Text) then
  begin
    result := 4;
    exit;
  end;
  if T = typeof(BinaryFile) then
  begin
    result := 5;
    exit;
  end;
end;

function RuntimeInitialize(kind: byte; variable: object): object;
begin
  case kind of
    1: 
      begin
        variable.GetType.InvokeMember('$Init$',
        System.Reflection.BindingFlags.InvokeMethod or
        System.Reflection.BindingFlags.Instance or
        System.Reflection.BindingFlags.Public, nil, variable, nil);
        result := variable;
      end;
    2: result := '';
    3: result := new TypedSet;
    4: result := new Text;
    5: result := new BinaryFile;
  end;
end;

function GetRuntimeSize<T>: integer;
var val: T;
begin
  result := System.Runtime.InteropServices.Marshal.SizeOf(val);
end;

// -----------------------------------------------------------------------------
//                Внутренние вспомогательные функции 
// -----------------------------------------------------------------------------
function get_sizes(a: &Array): array of integer;
begin
  var rank := a.Rank;
  Result := Result;
  SetLength(Result, rank);
  for var i := 0 to rank - 1 do
    Result[i] := a.GetLength(i);  
end;

procedure internal_copy(source, dest: &Array; source_sizes, dest_sizes: array of integer; i: integer; var src_ind, dest_ind: integer; flag: byte);
begin
  if i <> source_sizes.Length - 1 then
  begin
    for var j := 0 to min(source_sizes[i], dest_sizes[i]) - 1 do
      internal_copy(source, dest, source_sizes, dest_sizes, i + 1, src_ind, dest_ind, flag);
    if dest_sizes[i] > source_sizes[i] then
      for var j := source_sizes[i] to dest_sizes[i] - 1 do
      begin
        internal_copy(source, dest, source_sizes, dest_sizes, i + 1, src_ind, dest_ind, 1);
      end
    else
    if dest_sizes[i] < source_sizes[i] then
      for var j := dest_sizes[i] to source_sizes[i] - 1 do
      begin
        internal_copy(source, dest, source_sizes, dest_sizes, i + 1, src_ind, dest_ind, 2);
      end
  end
  else
  begin
    if flag = 0 then
    begin
      System.Array.Copy(source, src_ind, dest, dest_ind, min(source_sizes[source_sizes.Length - 1], dest_sizes[source_sizes.Length - 1]));
      src_ind += source_sizes[source_sizes.Length - 1];
      dest_ind += dest_sizes[source_sizes.Length - 1];
    end
    else if flag = 1 then
      dest_ind += dest_sizes[source_sizes.Length - 1]
    else
      src_ind += source_sizes[source_sizes.Length - 1];
  end;
end;

function CopyWithSize(source, dest: &Array): &Array;
begin
  if source <> nil then
  begin
    //System.Array.Copy(source,dest,min(source.Length,dest.Length));
    var source_sizes := get_sizes(source);
    var dest_sizes := get_sizes(dest);
    var src_ind := 0;
    var dest_ind := 0;
    internal_copy(source, dest, source_sizes, dest_sizes, 0, src_ind, dest_ind, 0);
    Result := dest;
  end
  else
    Result := dest;
end;

{function TypedSetComparer.Equals(x: System.Object; y: System.Object): boolean;
begin
  //Result := object.Equals(x,y);
  Result := object.Equals(x, y);
  if not Result then
  begin
    var left_type := x.GetType;
    var right_type := y.GetType;
    case System.Type.GetTypeCode(left_type) of
      TypeCode.Byte:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.SByte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int32: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.SByte:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.SByte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int32: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.UInt16:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.SByte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int32: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.Int16:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.SByte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int32: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.Int32:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.SByte: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt16: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.Int32: Result := Convert.ToInt32(x) = Convert.ToInt32(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.UInt32:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.SByte: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int16: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt16: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.Int64:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.SByte: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int16: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt16: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt32: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
      TypeCode.UInt64:
        begin
          case System.Type.GetTypeCode(right_type) of
            TypeCode.Byte: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.SByte: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.Int16: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt16: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.Int32: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt32: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.Int64: Result := Convert.ToUInt64(x) = Convert.ToInt64(y);
            TypeCode.UInt64: Result := Convert.ToUInt64(x) = Convert.ToUInt64(y);
          end;
        end;
    end;
  end;
end;

function TypedSetComparer.GetHashCode(obj: System.Object): integer;
begin
  case System.Type.GetTypeCode(obj.GetType) of
    TypeCode.Byte: Result := Convert.ToByte(obj);
    TypeCode.SByte: Result := Convert.ToSByte(obj);
    TypeCode.UInt16: Result := Convert.ToUInt16(obj);
    TypeCode.Int16: Result := Convert.ToInt16(obj);
    TypeCode.Int32: Result := Convert.ToInt32(obj);
    TypeCode.UInt32: Result := Convert.ToUInt32(obj).GetHashCode();
    TypeCode.Int64: Result := Convert.ToInt64(obj).GetHashCode();
    TypeCode.UInt64: Result := Convert.ToUInt64(obj).GetHashCode();
  else
    Result := obj.GetHashCode();
  end;
end;}

var
  __from_dll := false;

function ExecuteAssemlyIsDll: boolean;
begin
  Result := not __from_dll and (IO.Path.GetExtension(System.Reflection.Assembly.GetExecutingAssembly.ManifestModule.FullyQualifiedName).ToLower = '.dll');
end;

function IsUnix: boolean;
begin
  Result := (Environment.OSVersion.Platform = PlatformID.Unix) or (Environment.OSVersion.Platform = PlatformID.MacOSX);
end;
//------------------------------------------------------------------------------
//OMP

procedure omp_set_nested(nested: integer);
begin
  OMP_NESTED := nested <> 0;
end;

function omp_get_nested: integer;
begin
  if OMP_NESTED then
    result := 1
  else
    result := 0;
end;

// -----------------------------------------------------
// Internal procedures for PABCRTL.dll: implementation
// -----------------------------------------------------
var
  __initialized := false;

procedure __InitModule;
begin
  DefaultEncoding := Encoding.GetEncoding(1251);
  var arg := Environment.GetCommandLineArgs();
  if arg.Length > 1 then begin
    CommandLineArgs := new string[arg.Length - 1];
    for var i := 1 to arg.Length - 1 do
      CommandLineArgs[i - 1] := arg[i];
  end else
    CommandLineArgs := new string[0];
  CurrentIOSystem := new IOStandardSystem;
  //  ENCultureInfo := new System.Globalization.CultureInfo('en-US');
  var locale: object;
  var locale_str := 'ru-RU';
  if __CONFIG__.TryGetValue('full_locale', locale) then
    locale_str := string(locale);
  System.Threading.Thread.CurrentThread.CurrentUICulture := System.Globalization.CultureInfo.GetCultureInfo(locale_str);
  nfi := new System.Globalization.NumberFormatInfo();
  nfi.NumberGroupSeparator := '.';
  //  System.Threading.Thread.CurrentThread.CurrentCulture := new System.Globalization.CultureInfo('en-US');
  rnd := new System.Random;
  StartTime := DateTime.Now;
  output := new TextFile();
  input := new TextFile();
  //var tmp := __CONFIG__;
  if (Environment.OSVersion.Platform = PlatformID.Unix) or (Environment.OSVersion.Platform = PlatformID.MacOSX) then
    foreach listener: System.Diagnostics.TraceListener in System.Diagnostics.Trace.Listeners do
      if listener is System.Diagnostics.DefaultTraceListener then
        (listener as System.Diagnostics.DefaultTraceListener).AssertUiEnabled := true; 
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __from_dll := true;
    __InitModule;
  end;
end;

procedure __InitPABCSystem;
begin
  __InitModule__;
end;

procedure __FinalizeModule__;
begin
  if (output.sw <> nil) and (output.sw.BaseStream <> nil) then
    output.sw.Close;
  if (input.sr <> nil) and (input.sr.BaseStream <> nil) then
    input.sr.Close;
end;

initialization
  __InitModule;
finalization
  __FinalizeModule__;
end.