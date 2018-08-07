
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
  E = 2.718281828459045;
  /// Константа перехода на новую строку
  /// !! The newline string defined for this environment.
  NewLine = System.Environment.NewLine;
// -----------------------------------------------------
//>>     Стандартные типы # Standard types
// -----------------------------------------------------
type
  /// Базовый тип объектов
  Object = System.Object;
  
  /// Базовый тип исключений
  Exception = System.Exception;
  
  /// double = real
  double = System.Double;
  
  /// longint = integer
  longint = System.Int32;
  
  /// cardinal = longword
  cardinal = System.UInt32;
  
  /// Представляет 128-битное вещественное число
  /// !! Represents a decimal number
  decimal = System.Decimal;
  
  /// Представляет произвольно большое целое число
  BigInteger = System.Numerics.BigInteger;

  /// Представляет дату и время
  DateTime = System.DateTime;
  
  /// Представляет комплексное число
  Complex = System.Numerics.Complex;
  
  /// Представляет кортеж
  Tuple = System.Tuple;
  
  /// Представляет список на базе динамического массива
  List<T> = System.Collections.Generic.List<T>;
  
  /// Представляет базовый класс для реализации интерфейса IComparer
  Comparer<T> = System.Collections.Generic.Comparer<T>;
  
  /// Представляет базовый класс для реализации интерфейса IComparer
  IComparable<T> = System.IComparable<T>;
  
  /// Представляет множество значений, реализованное на базе хеш-таблицы
  HashSet<T> = System.Collections.Generic.HashSet<T>;
  
  /// Представляет множество значений, реализованное на базе бинарного дерева поиска
  SortedSet<T> = System.Collections.Generic.SortedSet<T>;
  
  /// Представляет ассоциативный массив (набор пар Ключ-Значение), реализованный на базе хеш-таблицы
  Dictionary<Key, Value> = System.Collections.Generic.Dictionary<Key, Value>;
  
  /// Представляет ассоциативный массив, реализованный на базе бинарного дерева поиска
  SortedDictionary<Key, Value> = System.Collections.Generic.SortedDictionary<Key, Value>;
  
  /// Представляет ассоциативный массив (набор пар ключ-значение), реализованный на базе динамического массива пар
  SortedList<Key, Value> = System.Collections.Generic.SortedList<Key, Value>;
  
  /// Представляет пару Ключ-Значение для ассоциативного массива 
  KeyValuePair<Key, Value> = System.Collections.Generic.KeyValuePair<Key, Value>;
  
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
  IDictionary<Key, Value> = System.Collections.Generic.IDictionary<Key, Value>;
  
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
  Action2<T1, T2> = System.Action<T1, T2>;
  
  /// Представляет действие с тремя параметрами
  Action3<T1, T2, T3> = System.Action<T1, T2, T3>;
  
  /// Представляет функцию без параметров
  Func0<Res> = System.Func<Res>;
  
  /// Представляет функцию с одним параметром
  Func<T, Res> = System.Func<T, Res>;
  
  /// Представляет функцию с двумя параметрами
  Func2<T1, T2, Res> = System.Func<T1, T2, Res>;
  
  /// Представляет функцию с тремя параметрами
  Func3<T1, T2, T3, Res> = System.Func<T1, T2, T3, Res>;
  
  /// Представляет функцию с одним параметром целого типа, возвращающую целое
  IntFunc = Func<integer, integer>;
  
  /// Представляет функцию с одним параметром вещественного типа, возвращающую вещественное
  RealFunc = Func<real, real>;
  
  /// Представляет функцию с одним параметром строкового типа, возвращающую строку
  StringFunc = Func<string, string>;
  
  /// Представляет функцию с одним параметром, возвращающую boolean 
  Predicate<T> = System.Predicate<T>;
  
  /// Представляет функцию с двумя параметрами, возвращающую boolean 
  Predicate2<T1, T2> = function(x1: T1; x2: T2): boolean;
  
  /// Представляет функцию с тремя параметрами, возвращающую boolean 
  Predicate3<T1, T2, T3> = function(x1: T1; x2: T2; x3: T3): boolean;
  
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

/// Вводит числовое значение x клавиатуры. Возвращает False если при вводе произошла ошибка
function TryRead(var x: integer): boolean;
///--
function TryRead(var x: real): boolean;
///--
function TryRead(var x: byte): boolean;
///--
function TryRead(var x: shortint): boolean;
///--
function TryRead(var x: smallint): boolean;
///--
function TryRead(var x: word): boolean;
///--
function TryRead(var x: longword): boolean;
///--
function TryRead(var x: int64): boolean;
///--
function TryRead(var x: uint64): boolean;
///--
function TryRead(var x: single): boolean;

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

/// Возвращает кортеж из двух значений типа integer, введенных с клавиатуры
function ReadInteger2: (integer, integer);
/// Возвращает кортеж из двух значений типа real, введенных с клавиатуры
function ReadReal2: (real, real);
/// Возвращает кортеж из двух значений типа char, введенных с клавиатуры
function ReadChar2: (char, char);
/// Возвращает кортеж из двух значений типа string, введенных с клавиатуры
function ReadString2: (string, string);
/// Возвращает кортеж из двух значений типа integer, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnInteger2: (integer, integer);
/// Возвращает кортеж из двух значений типа real, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnReal2: (real, real);
/// Возвращает кортеж из двух значений типа char, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnChar2: (char, char);
/// Возвращает кортеж из двух значений типа string, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnString2: (string, string);

/// Возвращает кортеж из трёх значений типа integer, введенных с клавиатуры
function ReadInteger3: (integer, integer, integer);
/// Возвращает кортеж из трёх значений типа real, введенных с клавиатуры
function ReadReal3: (real, real, real);
/// Возвращает кортеж из трёх значений типа char, введенных с клавиатуры
function ReadChar3: (char, char, char);
/// Возвращает кортеж из трёх значений типа string, введенных с клавиатуры
function ReadString3: (string, string, string);
/// Возвращает кортеж из двух значений типа integer, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnInteger3: (integer, integer, integer);
/// Возвращает кортеж из двух значений типа real, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnReal3: (real, real, real);
/// Возвращает кортеж из двух значений типа char, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnChar3: (char, char, char);
/// Возвращает кортеж из двух значений типа string, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnString3: (string, string, string);

/// Возвращает кортеж из двух значений типа integer, введенных с клавиатуры
function ReadInteger2(prompt: string): (integer, integer);
/// Возвращает кортеж из двух значений типа real, введенных с клавиатуры
function ReadReal2(prompt: string): (real, real);
/// Возвращает кортеж из двух значений типа char, введенных с клавиатуры
function ReadChar2(prompt: string): (char, char);
/// Возвращает кортеж из двух значений типа string, введенных с клавиатуры
function ReadString2(prompt: string): (string, string);
/// Возвращает кортеж из двух значений типа integer, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnInteger2(prompt: string): (integer, integer);
/// Возвращает кортеж из двух значений типа real, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnReal2(prompt: string): (real, real);
/// Возвращает кортеж из двух значений типа char, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnChar2(prompt: string): (char, char);
/// Возвращает кортеж из двух значений типа string, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnString2(prompt: string): (string, string);

/// Возвращает кортеж из трёх значений типа integer, введенных с клавиатуры
function ReadInteger3(prompt: string): (integer, integer, integer);
/// Возвращает кортеж из трёх значений типа real, введенных с клавиатуры
function ReadReal3(prompt: string): (real, real, real);
/// Возвращает кортеж из трёх значений типа char, введенных с клавиатуры
function ReadChar3(prompt: string): (char, char, char);
/// Возвращает кортеж из трёх значений типа string, введенных с клавиатуры
function ReadString3(prompt: string): (string, string, string);
/// Возвращает кортеж из двух значений типа integer, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnInteger3(prompt: string): (integer, integer, integer);
/// Возвращает кортеж из двух значений типа real, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnReal3(prompt: string): (real, real, real);
/// Возвращает кортеж из двух значений типа char, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnChar3(prompt: string): (char, char, char);
/// Возвращает кортеж из двух значений типа string, введенных с клавиатуры, и переходит на следующую строку ввода
function ReadlnString3(prompt: string): (string, string, string);


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
/// Устанавливает текущий каталог. Возвращает True, если каталог успешно удален
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
//>>     Подпрограммы для работы с именами файлов # Functions for file names
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
///--
function Abs(x: single): single;
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
/// Возвращает натуральный логарифм числа x
function Log(x: real): real;
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
/// Возвращает x в целой степени n
function Power(x: real; n: integer): real;
/// Возвращает x в степени y
function Power(x: BigInteger; y: integer): BigInteger;
/// Возвращает x, округленное до ближайшего целого. Если вещественное находится посередине между двумя целыми, 
///то округление осуществляется к ближайшему четному (банковское округление): Round(2.5)=2, Round(3.5)=4
function Round(x: real): integer;
/// Возвращает x, округленное до ближайшего вещественного с digits знаками после десятичной точки
function Round(x: real; digits: integer): real;
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
/// Возвращает кортеж из двух случайных целых в диапазоне от 0 до maxValue-1
function Random2(maxValue: integer): (integer, integer);
/// Возвращает кортеж из двух случайных целых в диапазоне от a до b
function Random2(a, b: integer): (integer, integer);
/// Возвращает кортеж из двух случайных вещественных в диапазоне [0..1)
function Random2: (real, real);
/// Возвращает кортеж из трех случайных целых в диапазоне от 0 до maxValue-1
function Random3(maxValue: integer): (integer, integer, integer);
/// Возвращает кортеж из трех случайных целых в диапазоне от a до b
function Random3(a, b: integer): (integer, integer, integer);
/// Возвращает кортеж из трех случайных вещественных в диапазоне [0..1)
function Random3: (real, real, real);

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
//>>     Подпрограммы для работы с комплексными числами # Functions for Complex numbers
// -----------------------------------------------------
/// Конструирует комплексное число с вещественной частью re и мнимой частью im
function Cplx(re, im: real): Complex;
/// Конструирует комплексное число по полярным координатам
function CplxFromPolar(magnitude, phase: real): Complex;
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
function Ln(c: Complex): Complex;
/// Возвращает натуральный логарифм комплексного числа
function Log(c: Complex): Complex;
/// Возвращает десятичный логарифм комплексного числа
function Log10(c: Complex): Complex;
/// Возвращает степень комплексного числа
function Power(c, power: Complex): Complex;
/// Возвращает синус комплексного числа
function Sin(c: Complex): Complex;

// -----------------------------------------------------
//>>     Подпрограммы для работы со стандартными множествами # Subroutines for set of T
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
/// Возвращает инвертированную строку в диапазоне длины length начиная с индекса index
function ReverseString(s: string; index,length: integer): string;
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
/// Преобразует строковое представление вещественного числа к числовому значению
function StrToReal(s: string): real;
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
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value. 
///При невозможности преобразования возвращается False
function TryStrToReal(s: string; var value: real): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value. 
///При невозможности преобразования возвращается False
function TryStrToSingle(s: string; var value: single): boolean;
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
/// Изменяет порядок элементов на противоположный в диапазоне динамического массива длины count, начиная с индекса index
procedure Reverse<T>(a: array of T; index, count: integer);
/// Изменяет порядок элементов в списке на противоположный
procedure Reverse<T>(a: List<T>);
/// Изменяет порядок элементов на противоположный в диапазоне списка длины count, начиная с индекса index
procedure Reverse<T>(a: List<T>; index, count: integer);
/// Перемешивает динамический массив случайным образом
procedure Shuffle<T>(a: array of T);
/// Перемешивает список случайным образом
procedure Shuffle<T>(l: List<T>);

// -----------------------------------------------------
//>>     Подпрограммы для генерации последовательностей # Subroutines for sequence generation
// -----------------------------------------------------
/// Возвращает последовательность целых от a до b
function Range(a, b: integer): sequence of integer;
/// Возвращает последовательность символов от c1 до c2
function Range(c1, c2: char): sequence of char;
/// Возвращает последовательность вещественных в точках разбиения отрезка [a,b] на n равных частей (Используйте Partition)
function Range(a, b: real; n: integer): sequence of real;
/// Возвращает последовательность вещественных в точках разбиения отрезка [a,b] на n равных частей
function Partition(a, b: real; n: integer): sequence of real;
/// Возвращает последовательность целых от a до b с шагом step
function Range(a, b, step: integer): sequence of integer;
/// Возвращает последовательность указанных элементов
function Seq<T>(params a: array of T): sequence of T;
/// Возвращает последовательность из n случайных целых элементов
function SeqRandom(n: integer := 10; a: integer := 0; b: integer := 100): sequence of integer;
/// Возвращает последовательность из n случайных целых элементов
function SeqRandomInteger(n: integer := 10; a: integer := 0; b: integer := 100): sequence of integer;
/// Возвращает последовательность из n случайных вещественных элементов
function SeqRandomReal(n: integer := 10; a: real := 0; b: real := 10): sequence of real;
/// Возвращает последовательность из count элементов, заполненных значениями f(i)
function SeqGen<T>(count: integer; f: integer->T): sequence of T;
/// Возвращает последовательность из count элементов, заполненных значениями f(i), начиная с i=from
function SeqGen<T>(count: integer; f: integer->T; from: integer): sequence of T;
/// Возвращает последовательность из count элементов, начинающуюся с first, с функцией next перехода от предыдущего к следующему 
function SeqGen<T>(count: integer; first: T; next: T->T): sequence of T;
/// Возвращает последовательность из count элементов, начинающуюся с first и second, 
///с функцией next перехода от двух предыдущих к следующему 
function SeqGen<T>(count: integer; first, second: T; next: (T,T) ->T): sequence of T;
/// Возвращает последовательность элементов с начальным значением first, 
///функцией next перехода от предыдущего к следующему и условием pred продолжения последовательности 
function SeqWhile<T>(first: T; next: T->T; pred: T->boolean): sequence of T;
/// Возвращает последовательность элементов, начинающуюся с first и second, 
///с функцией next перехода от двух предыдущих к следующему и условием pred продолжения последовательности 
function SeqWhile<T>(first, second: T; next: (T,T) ->T; pred: T->boolean): sequence of T;
/// Возвращает последовательность из count элементов x 
function SeqFill<T>(count: integer; x: T): sequence of T;

/// Возвращает последовательность из n целых, введенных с клавиатуры
function ReadSeqInteger(n: integer): sequence of integer;
/// Возвращает последовательность из n вещественных, введенных с клавиатуры
function ReadSeqReal(n: integer): sequence of real;
/// Возвращает последовательность из n строк, введенных с клавиатуры
function ReadSeqString(n: integer): sequence of string;

/// Выводит приглашение к вводу и возвращает последовательность из n целых, введенных с клавиатуры
function ReadSeqInteger(prompt: string; n: integer): sequence of integer;
/// Выводит приглашение к вводу и возвращает последовательность из n вещественных, введенных с клавиатуры
function ReadSeqReal(prompt: string; n: integer): sequence of real;
/// Выводит приглашение к вводу и возвращает последовательность из n строк, введенных с клавиатуры
function ReadSeqString(prompt: string; n: integer): sequence of string;

/// Возвращает последовательность целых, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqIntegerWhile(cond: integer->boolean): sequence of integer;
/// Возвращает последовательность вещественных, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqRealWhile(cond: real->boolean): sequence of real;
/// Возвращает последовательность строк, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqStringWhile(cond: string->boolean): sequence of string;

/// Выводит приглашение к вводу и возвращает последовательность целых, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqIntegerWhile(prompt: string; cond: integer->boolean): sequence of integer;
/// Выводит приглашение к вводу и возвращает последовательность вещественных, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqRealWhile(prompt: string; cond: real->boolean): sequence of real;
/// Выводит приглашение к вводу и возвращает последовательность строк, вводимых с клавиатуры пока выполняется определенное условие
function ReadSeqStringWhile(prompt: string; cond: string->boolean): sequence of string;

// -----------------------------------------------------
//>>     Подпрограммы для создания динамических массивов # Subroutines for array of T generation
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
function ArrGen<T>(count: integer; gen: integer->T): array of T;
/// Возвращает массив из count элементов, заполненных значениями gen(i), начиная с i=from
function ArrGen<T>(count: integer; gen: integer->T; from: integer): array of T;
/// Возвращает массив из count элементов, начинающихся с first, с функцией next перехода от предыдущего к следующему 
function ArrGen<T>(count: integer; first: T; next: T->T): array of T;
/// Возвращает массив из count элементов, начинающихся с first и second, с функцией next перехода от двух предыдущих к следующему 
function ArrGen<T>(count: integer; first, second: T; next: (T,T) ->T): array of T;
/// Возвращает массив из count элементов x 
function ArrFill<T>(count: integer; x: T): array of T;

/// Возвращает массив из n целых, введенных с клавиатуры
function ReadArrInteger(n: integer): array of integer;
/// Возвращает массив из n вещественных, введенных с клавиатуры
function ReadArrReal(n: integer): array of real;
/// Возвращает массив из n строк, введенных с клавиатуры
function ReadArrString(n: integer): array of string;

/// Выводит приглашение к вводу и возвращает массив из n целых, введенных с клавиатуры
function ReadArrInteger(prompt: string; n: integer): array of integer;
/// Выводит приглашение к вводу и возвращает массив из n вещественных, введенных с клавиатуры
function ReadArrReal(prompt: string; n: integer): array of real;
/// Выводит приглашение к вводу и возвращает массив из n строк, введенных с клавиатуры
function ReadArrString(prompt: string; n: integer): array of string;

// -----------------------------------------------------
//>>     Подпрограммы для создания двумерных динамических массивов # Subroutines for matrixes 
// -----------------------------------------------------
/// Возвращает двумерный массив размера m x n, заполненный указанными значениями по строкам
function Matr<T>(m,n: integer; params data: array of T): array [,] of T;
/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandom(m: integer := 5; n: integer := 5; a: integer := 0; b: integer := 100): array [,] of integer;
/// Возвращает двумерный массив размера m x n, заполненный случайными целыми значениями
function MatrRandomInteger(m: integer := 5; n: integer := 5; a: integer := 0; b: integer := 100): array [,] of integer;
/// Возвращает двумерный массив размера m x n, заполненный случайными вещественными значениями
function MatrRandomReal(m: integer := 5; n: integer := 5; a: real := 0; b: real := 10): array [,] of real;
/// Возвращает двумерный массив размера m x n, заполненный элементами x 
function MatrFill<T>(m, n: integer; x: T): array [,] of T;
/// Возвращает двумерный массив размера m x n, заполненный элементами x 
function MatrGen<T>(m, n: integer; gen: (integer,integer)->T): array [,] of T;
/// Транспонирует двумерный массив 
function Transpose<T>(a: array [,] of T): array [,] of T;
/// Возвращает матрицу m на n целых, введенных с клавиатуры
function ReadMatrInteger(m, n: integer): array [,] of integer;
/// Возвращает матрицу m на n вещественных, введенных с клавиатуры
function ReadMatrReal(m, n: integer): array [,] of real;


// -----------------------------------------------------
//>>     Подпрограммы для создания кортежей # Subroutines for tuple generation
// -----------------------------------------------------
///- function Rec(x1: T1, x2: T2,...): (T1,T2,...);
/// Возвращает кортеж из элементов разных типов
function Rec<T1, T2>(x1: T1; x2: T2): System.Tuple<T1, T2>;
///--
function Rec<T1, T2, T3>(x1: T1; x2: T2; x3: T3): (T1, T2, T3);
///--
function Rec<T1, T2, T3, T4>(x1: T1; x2: T2; x3: T3; x4: T4): (T1, T2, T3, T4);
///--
function Rec<T1, T2, T3, T4, T5>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5): (T1, T2, T3, T4, T5);
///--
function Rec<T1, T2, T3, T4, T5, T6>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5; x6: T6): (T1, T2, T3, T4, T5, T6);
///--
function Rec<T1, T2, T3, T4, T5, T6, T7>(x1: T1; x2: T2; x3: T3; x4: T4; x5: T5; x6: T6; x7: T7): (T1, T2, T3, T4, T5, T6, T7);

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


// -----------------------------------------------------
//>>     Генерация бесконечных последовательностей # Infinite sequences
// -----------------------------------------------------
// Дополнения февраль 2016: Iterate, Step, Repeat, Cycle

// Возвращает бесконечную рекуррентную последовательность элементов, задаваемую начальным элементом и функцией next
///--
function Iterate<T>(Self: T; next: T->T): sequence of T; extensionmethod;
begin
  Result := Iterate<T>(Self, next);
end;

// Возвращает бесконечную рекуррентную последовательность элементов, задаваемую начальным элементом, следующим за ним элементом и функцией next
///--
function Iterate<T>(Self, second: T; next: (T,T) ->T): sequence of T; extensionmethod;
begin
  Result := Iterate<T>(Self, second, next);
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
function Repeat<T>(Self: T): sequence of T; extensionmethod;
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

// -----------------------------------------------------
//>>     Фиктивная секция XXX - не удалять! # XXX
// -----------------------------------------------------
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

//------------------------------------------------------------------------------
//>>     Метод расширения Print для элементарных типов # Print for elementary types
//------------------------------------------------------------------------------
function Print(Self: integer): integer; extensionmethod;
begin
  PABCSystem.Print(Self);
  Result := Self;
end;

function Print(Self: real): real; extensionmethod;
begin
  PABCSystem.Print(Self);
  Result := Self;
end;

function Print(Self: char): char; extensionmethod;
begin
  PABCSystem.Print(Self);
  Result := Self;
end;

function Print(Self: boolean): boolean; extensionmethod;
begin
  PABCSystem.Print(Self);
  Result := Self;
end;

function Print(Self: BigInteger): BigInteger; extensionmethod;
begin
  PABCSystem.Print(Self);
  Result := Self;
end;

function Println(Self: integer): integer; extensionmethod;
begin
  PABCSystem.Println(Self);
  Result := Self;
end;

function Println(Self: real): real; extensionmethod;
begin
  PABCSystem.Println(Self);
  Result := Self;
end;

function Println(Self: char): char; extensionmethod;
begin
  PABCSystem.Println(Self);
  Result := Self;
end;

function Println(Self: boolean): boolean; extensionmethod;
begin
  PABCSystem.Println(Self);
  Result := Self;
end;

function Println(Self: BigInteger): BigInteger; extensionmethod;
begin
  PABCSystem.Println(Self);
  Result := Self;
end;



//------------------------------------------------------------------------------
//>>     Методы расширения последовательностей # Extension methods for sequence of T
//------------------------------------------------------------------------------
/// Выводит последовательность на экран, используя delim в качестве разделителя
function Print<T>(Self: sequence of T; delim: string): sequence of T; extensionmethod;
begin
  var g := Self.GetEnumerator();
  if g.MoveNext() then
    Write(g.Current);
  while g.MoveNext() do
    if delim <> '' then
      Write(delim, g.Current)
    else Write(g.Current);
  Result := Self; 
end;

/// Выводит последовательность на экран, используя пробел в качестве разделителя
function Print<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  if typeof(T) = typeof(char) then 
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
  if typeof(T) = typeof(char) then 
    Result := Self.Println('')
  else  
    Result := Self.Println(PrintDelimDefault);  
end;

/// Выводит последовательность строк в файл
function WriteLines(Self: sequence of string; fname: string): sequence of string; extensionmethod;
begin
  WriteLines(fname, Self);
  Result := Self
end;

/// Выводит последовательность, каждый элемент выводится на новой строке
function PrintLines<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Self.Println(NewLine);
  Result := Self
end;

/// Выводит последовательность, каждый элемент отображается с помощью функции map и выводится на новой строке
function PrintLines<T,T1>(Self: sequence of T; map: T->T1): sequence of T; extensionmethod;
begin
  Self.Select(map).Println(NewLine);
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
procedure ForEach<T>(Self: sequence of T; action: T -> ()); extensionmethod;
begin
  foreach x: T in Self do
    action(x);
end;

/// Применяет действие к каждому элементу последовательности, зависящее от номера элемента
procedure ForEach<T>(Self: sequence of T; action: (T,integer) -> ()); extensionmethod;
begin
  var i := 0;
  foreach x: T in Self do
  begin
    action(x, i);
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

/// Возвращает отсортированную по возрастанию последовательность
function Order<T>(Self: sequence of T): sequence of T; extensionmethod;
begin
  Result := Self.OrderBy(x -> x);
end;

/// Возвращает отсортированную по убыванию последовательность
function OrderDescending<T>(Self: sequence of T): sequence of T; extensionmethod;
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
function MinBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');
  
  var comp := Comparer<TKey>.Default;
  Result := Self.Aggregate((min, x)-> comp.Compare(selector(x), selector(min)) < 0 ? x : min);
end;

/// Возвращает первый элемент последовательности с максимальным значением ключа
function MaxBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');
  
  var comp := Comparer<TKey>.Default;
  Result := Self.Aggregate((max, x)-> comp.Compare(selector(x), selector(max)) > 0 ? x : max);
end;

/// Возвращает последний элемент последовательности с минимальным значением ключа
function LastMinBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');
  
  var comp := Comparer<TKey>.Default;
  Result := Self.Aggregate((min, x)-> comp.Compare(selector(x), selector(min)) <= 0 ? x : min);
end;

/// Возвращает последний элемент последовательности с максимальным значением ключа
function LastMaxBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
begin
  if selector = nil then
    raise new ArgumentNullException('selector');
  if not Self.Any() then
    raise new InvalidOperationException('Empty sequence');
  
  var comp := Comparer<TKey>.Default;
  Result := Self.Aggregate((max, x)-> comp.Compare(selector(x), selector(max)) >= 0 ? x : max);
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
function Cartesian<T, T1>(Self: sequence of T; b: sequence of T1): sequence of (T, T1); extensionmethod;
begin
  if b = nil then
    raise new System.ArgumentNullException('b');
  
  foreach var x in Self do
    foreach var y in b do
      yield (x, y)
end;

/// Декартово произведение последовательностей
function Cartesian<T, T1, T2>(Self: sequence of T; b: sequence of T1; func: (T,T1)->T2): sequence of T2; extensionmethod;
begin
  if b = nil then
    raise new System.ArgumentNullException('b');
  
  foreach var x in Self do
    foreach var y in b do
      yield func(x, y)
end;

/// Разбивает последовательности на две в позиции ind
function SplitAt<T>(Self: sequence of T; ind: integer): (sequence of T, sequence of T); extensionmethod;
begin
  Result := (Self.Take(ind), Self.Skip(ind));
end;

// ToDo: то же для TakeWhile

// ToDo: SequenceCompare

/// Разделяет последовательности на две по заданному условию
function Partition<T>(Self: sequence of T; cond: T->boolean): (sequence of T, sequence of T); extensionmethod;
begin
  Result := (Self.Where(cond), Self.Where(x -> not cond(x)));
end;

/// Разделяет последовательности на две по заданному условию, в котором участвует индекс
function Partition<T>(Self: sequence of T; cond: (T,integer)->boolean): (sequence of T, sequence of T); extensionmethod;
begin
  Result := (Self.Where(cond), Self.Where((x, i)-> not cond(x, i)));
end;

/// Объединяет две последовательности в последовательность двухэлементных кортежей
function ZipTuple<T, T1>(Self: sequence of T; a: sequence of T1): sequence of (T, T1); extensionmethod;
begin
  if a = nil then
    raise new System.ArgumentNullException('a');
  Result := Self.Zip(a, (x, y)-> (x, y));
end;

/// Объединяет три последовательности в последовательность трехэлементных кортежей
function ZipTuple<T, T1, T2>(Self: sequence of T; a: sequence of T1; b: sequence of T2): sequence of (T, T1, T2); extensionmethod;
begin
  if a = nil then
    raise new System.ArgumentNullException('a');
  if b = nil then
    raise new System.ArgumentNullException('b');
  Result := Self.Zip(a, (x, y)-> (x, y)).Zip(b, (p, z)-> (p[0], p[1], z));
end;

/// Объединяет четыре последовательности в последовательность четырехэлементных кортежей
function ZipTuple<T, T1, T2, T3>(Self: sequence of T; a: sequence of T1; b: sequence of T2; c: sequence of T3): sequence of (T, T1, T2, T3); extensionmethod;
begin
  if a = nil then
    raise new System.ArgumentNullException('a');
  if b = nil then
    raise new System.ArgumentNullException('b');
  if c = nil then
    raise new System.ArgumentNullException('c');
  Result := Self.Zip(a, (x, y)-> (x, y)).Zip(b, (p, z)-> (p[0], p[1], z)).Zip(c, (p, z)-> (p[0], p[1], p[2], z));
end;

/// Разъединяет последовательность двухэлементных кортежей на две последовательности
function UnZipTuple<T, T1>(Self: sequence of (T, T1)): (sequence of T, sequence of T1); extensionmethod;
begin
  Result := (Self.Select(x -> x[0]), Self.Select(x -> x[1]))
end;

/// Разъединяет последовательность трехэлементных кортежей на три последовательности
function UnZipTuple<T, T1, T2>(Self: sequence of (T, T1, T2)): (sequence of T, sequence of T1, sequence of T2); extensionmethod;
begin
  Result := (Self.Select(x -> x[0]), Self.Select(x -> x[1]), Self.Select(x -> x[2]))
end;

/// Разъединяет последовательность четырехэлементных кортежей на четыре последовательности
function UnZipTuple<T, T1, T2, T3>(Self: sequence of (T, T1, T2, T3)): (sequence of T, sequence of T1, sequence of T2, sequence of T3); extensionmethod;
begin
  Result := (Self.Select(x -> x[0]), Self.Select(x -> x[1]), Self.Select(x -> x[2]), Self.Select(x -> x[3]))
end;

// ToDo - сделать UnZipTuple с функцией-проекцией

/// Чередует элементы двух последовательностей
function Interleave<T>(Self: sequence of T; a: sequence of T): sequence of T; extensionmethod;
begin
  if a = nil then
    raise new System.ArgumentNullException('a');
  Result := Self.ZipTuple(a).SelectMany(x -> Seq(x[0], x[1]))
end;

/// Чередует элементы трех последовательностей
function Interleave<T>(Self: sequence of T; a, b: sequence of T): sequence of T; extensionmethod;
begin
  if a = nil then
    raise new System.ArgumentNullException('a');
  if b = nil then
    raise new System.ArgumentNullException('b');
  Result := Self.ZipTuple(a, b).SelectMany(x -> Seq(x[0], x[1], x[2]))
end;

/// Чередует элементы четырех последовательностей
function Interleave<T>(Self: sequence of T; a, b, c: sequence of T): sequence of T; extensionmethod;
begin
  if a = nil then
    raise new System.ArgumentNullException('a');
  if b = nil then
    raise new System.ArgumentNullException('b');
  if c = nil then
    raise new System.ArgumentNullException('c');
  Result := Self.ZipTuple(a, b, c).SelectMany(x -> Seq(x[0], x[1], x[2], x[3]))
end;

/// Нумерует последовательность с единицы
function Numerate<T>(Self: sequence of T): sequence of (integer, T); extensionmethod;
begin
  Result := 1.Step.ZipTuple(Self);
end;

/// Нумерует последовательность с номера from
function Numerate<T>(Self: sequence of T; from: integer): sequence of (integer, T); extensionmethod;
begin
  Result := from.Step.ZipTuple(Self);
end;

/// Табулирует функцию последовательностью
function Tabulate<T, T1>(Self: sequence of T; F: T->T1): sequence of (T, T1); extensionmethod;
begin
  Result := Self.Select(x -> (x, f(x)));
end;

/// Превращает последовательность в последовательность пар соседних элементов
function Pairwise<T>(Self: sequence of T): sequence of (T, T); extensionmethod;
begin
  var previous: T;
  var it := Self.GetEnumerator();
  if (it.MoveNext()) then
    previous := it.Current;
  
  while (it.MoveNext()) do
  begin
    yield (previous, it.Current);
    previous := it.Current;
  end
end;

/// Превращает последовательность в последовательность пар соседних элементов, применяет func к каждой паре полученных элементов и получает новую последовательность 
function Pairwise<T, Res>(Self: sequence of T; func: (T,T)->Res): sequence of Res; extensionmethod;
begin
  var previous: T;
  var it := Self.GetEnumerator();
  if (it.MoveNext()) then
    previous := it.Current;
  
  while (it.MoveNext()) do
  begin
    yield func(previous, it.Current);
    previous := it.Current;
  end
  //  Result := Self.ZipTuple(Self.Skip(1)).Select(x->func(x[0],x[1]));
end;

/// Разбивает последовательность на серии длины size
function Batch<T>(Self: sequence of T; size: integer): sequence of sequence of T; extensionmethod;
begin
  Result := SeqWhile(Self, v -> v.Skip(size), v -> v.Count > 0).Select(v -> v.Take(size))
end;

/// Разбивает последовательность на серии длины size и применяет проекцию к каждой серии
function Batch<T, Res>(Self: sequence of T; size: integer; proj: Func<IEnumerable<T>, Res>): sequence of Res; extensionmethod;
begin
  Result := SeqWhile(Self, v -> v.Skip(size), v -> v.Count > 0).Select(v -> v.Take(size)).Select(ss -> proj(ss));
end;

///--
function SliceSeqImpl<T>(Self: sequence of T; from, step, count: integer): sequence of T;
begin
  if step <= 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_GREATER_0));
  
  if from < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  Result := Self.Skip(from).Where((x, i)-> i mod step = 0)
end;

/// Возвращает срез последовательности от номера from с шагом step > 0
function Slice<T>(Self: sequence of T; from, step: integer): sequence of T; extensionmethod;
begin
  if step <= 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_GREATER_0));
  
  if from < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  Result := Self.Skip(from).Where((x, i)-> i mod step = 0)
end;

/// Возвращает срез последовательности от номера from с шагом step > 0 длины не более count
function Slice<T>(Self: sequence of T; from, step, count: integer): sequence of T; extensionmethod;
begin
  if step <= 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_GREATER_0));
  
  if from < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  Result := Self.Skip(from).Where((x, i)-> i mod step = 0).Take(count)
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
function Incremental<T, T1>(Self: sequence of T; func: (T,T)->T1): sequence of T1; extensionmethod;
begin
  var iter := Self.GetEnumerator();
  if iter.MoveNext() then
  begin
    var prevItem := iter.Current;
    while iter.MoveNext() do
    begin
      var nextItem := iter.Current;
      yield func(prevItem, nextItem);
      prevItem := nextItem;
    end
  end
end;

/// Возвращает последовательность разностей соседних элементов исходной последовательности. В качестве функции разности используется func
function Incremental<T, T1>(Self: sequence of T; func: (T,T,integer)->T1): sequence of T1; extensionmethod;
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
      yield func(prevItem, nextItem, ind);
      prevItem := nextItem;
    end
  end
end;

/// Группирует одинаковые подряд идущие элементы, получая последовательность массивов 
function AdjacentGroup<T>(Self: sequence of T): sequence of array of T; extensionmethod;
begin
  var c := new AdjGroupClass<T>(Self);
  while c.fin do
    yield c.TakeGroup().ToArray;
end;

// ToDo Сделать AdjacentGroup с функцией сравнения

// -----------------------------------------------------
//>>     Методы расширения списков # Extension methods for List T
// -----------------------------------------------------

/// Перемешивает элементы списка случайным образом
function Shuffle<T>(Self: List<T>): List<T>; extensionmethod;
begin
  var n := Self.Count;
  for var i := 0 to n - 1 do
  begin
    var r := PABCSystem.Random(n);
    var v := Self[i];
    Self[i] := Self[r];
    Self[r] := v;
  end;
  Result := Self;  
end;

/// Находит первую пару подряд идущих одинаковых элементов и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: IList<T>; start: integer := 0): integer; extensionmethod;
begin
  Result := -1;
  for var i := start to Self.Count - 2 do
    if Self[i] = Self[i + 1] then 
    begin
      Result := i;
      exit;
    end;
end;

/// Находит первую пару подряд идущих одинаковых элементов, используя функцию сравнения eq, и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: IList<T>; eq: (T,T)->boolean; start: integer := 0): integer; extensionmethod;
begin
  Result := -1;
  for var i := start to Self.Count - 2 do
    if eq(Self[i], Self[i + 1]) then 
    begin
      Result := i;
      exit;
    end;
end;

/// Возвращает индекс первого минимального элемента начиная с позиции index
function IndexMin<T>(Self: IList<T>; index: integer := 0): integer; extensionmethod;where T: IComparable<T>;
begin
  var min := Self[index];
  Result := index;
  for var i := index + 1 to Self.Count - 1 do
    if Self[i].CompareTo(min) < 0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс первого максимального элемента начиная с позиции index
function IndexMax<T>(self: IList<T>; index: integer := 0): integer; extensionmethod;where T: System.IComparable<T>;
begin
  var max := Self[index];
  Result := index;
  for var i := index + 1 to Self.Count - 1 do
    if Self[i].CompareTo(max) > 0 then 
    begin
      Result := i;
      max := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента
function LastIndexMin<T>(Self: IList<T>): integer; extensionmethod;where T: System.IComparable<T>;
begin
  var min := Self[Self.Count - 1];
  Result := Self.Count - 1;
  for var i := Self.Count - 2 downto 0 do
    if Self[i].CompareTo(min) < 0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента в диапазоне [0,index-1] 
function LastIndexMin<T>(Self: IList<T>; index: integer): integer; extensionmethod;where T: System.IComparable<T>;
begin
  var min := Self[index];
  Result := index;
  for var i := index - 1 downto 0 do
    if Self[i].CompareTo(min) < 0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс последнего максимального элемента
function LastIndexMax<T>(Self: IList<T>): integer; extensionmethod;where T: System.IComparable<T>;
begin
  var max := Self[Self.Count - 1];
  Result := Self.Count - 1;
  for var i := Self.Count - 2 downto 0 do
    if Self[i].CompareTo(max) > 0 then 
    begin
      Result := i;
      max := Self[i];
    end;
end;

/// Возвращает индекс последнего минимального элемента в диапазоне [0,index-1]
function LastIndexMax<T>(Self: IList<T>; index: integer): integer; extensionmethod;where T: System.IComparable<T>;
begin
  var max := Self[index];
  Result := index;
  for var i := index - 1 downto 0 do
    if Self[i].CompareTo(max) > 0 then 
    begin
      Result := i;
      max := Self[i];
    end;
end;

/// Заменяет в массиве или списке все вхождения одного значения на другое
procedure Replace<T>(Self: IList<T>; oldValue, newValue: T); extensionmethod;
begin
  for var i := 0 to Self.Count - 1 do
    if Self[i] = oldValue then
      Self[i] := newValue;
end;

/// Преобразует элементы массива или списка по заданному правилу
procedure Transform<T>(Self: IList<T>; f: T->T); extensionmethod;
begin
  for var i := 0 to Self.Count - 1 do
    Self[i] := f(Self[i]);
end;

/// Заполняет элементы массива или списка значениями, вычисляемыми по некоторому правилу
procedure Fill<T>(Self: IList<T>; f: integer->T); extensionmethod;
begin
  for var i := 0 to Self.Count - 1 do
    Self[i] := f(i);
end;

///-- 
function CreateSliceFromListInternal<T>(Self: List<T>; from, step, count: integer): List<T>;
begin
  Result := new List<T>(count);
  
  var f := from;
  loop count do
  begin
    Result.Add(Self[f]);
    f += step;
  end;
end;

///-- 
procedure CorrectCountForSlice(Len, from, step: integer; var count: integer);
begin
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));
  
  if count < 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_COUNT_MUST_BE_GREATER_0));
  
  if (from < 0) or (from > Len - 1) then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  var cnt := step > 0 ? Len - from : from + 1; 
  var cntstep := (cnt - 1) div abs(step) + 1;
  if count > cntstep then 
    count := cntstep;
end;

///-- 
function SliceListImpl<T>(Self: List<T>; from, step, count: integer): List<T>;
begin
  CorrectCountForSlice(Self.Count, from, step, count);
  Result := CreateSliceFromListInternal(Self, from, step, count);
end;

/// Возвращает срез списка от индекса from с шагом step
function Slice<T>(Self: List<T>; from, step: integer): List<T>; extensionmethod;
begin
  Result := SliceListImpl(Self, from, step, integer.MaxValue);
end;

/// Возвращает срез списка от индекса from с шагом step длины не более count
function Slice<T>(Self: List<T>; from, step, count: integer): List<T>; extensionmethod;
begin
  Result := SliceListImpl(Self, from, step, count);
end;

/// Удаляет последний элемент. Если элементов нет, генерирует исключение
function RemoveLast<T>(Self: List<T>): List<T>; extensionmethod;
begin
  Self.RemoveAt(Self.Count - 1);
  Result := Self;
end;

procedure CorrectFromTo(situation: integer; Len: integer; var from, to: integer; step: integer);
begin
  if step > 0 then
  begin
    case situation of
      1: from := 0;
      2: to := Len;
      3: (from, to) := (0, Len)
    end;  
  end
  else
  begin
    case situation of
      1: from := Len - 1;
      2: to := -1;
      3: (from, to) := (Len - 1, -1);
    end;
  end;
end;

///--
function CorrectFromToAndCalcCountForSystemSliceQuestion(situation: integer; Len: integer; var from, to: integer; step: integer): integer;
begin
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));
  
  CorrectFromTo(situation, Len, from, to, step);
  
  if step > 0 then
  begin
    if from < 0 then
      from += (step - from - 1) div step * step;
    // from может оказаться > Len - 1
    var m := Min(Len,to);
    if from >= m then 
      Result := 0
    else Result := (m - from - 1) div step + 1
  end
  else
  begin
    if from > Len - 1 then
      from -= (from - Len - step) div step * step;
    // from может оказаться < 0   
    var m := Max(to,-1);
    if from <= m then
      Result := 0
    else Result := (from - m - 1) div (-step) + 1
  end;
end;

///--
function CheckAndCorrectFromToAndCalcCountForSystemSlice(situation: integer; Len: integer; var from, to: integer; step: integer): integer;
begin
  // situation = 0 - все параметры присутствуют
  // situation = 1 - from отсутствует
  // situation = 2 - to отсутствует
  // situation = 3 - from и to отсутствуют
  if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));
  
  if (situation = 0) or (situation = 2) then
    if (from < 0) or (from > Len - 1) then
      raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  if (situation = 0) or (situation = 1) then
    if (to < -1) or (to > Len) then
      raise new ArgumentException(GetTranslation(PARAMETER_TO_OUT_OF_RANGE));
  
  CorrectFromTo(situation, Len, from, to, step);
  
  var count: integer;
  
  if step > 0 then
  begin
    var cnt := to - from;
    if cnt <= 0 then 
      count := 0
    else count := (cnt - 1) div step + 1;
  end
  else
  begin
    var cnt := from - to;
    if cnt <= 0 then 
      count := 0
    else count := (cnt - 1) div (-step) + 1;
  end;
  
  Result := count;
end;

///--
procedure CheckStepAndCorrectFromTo(situation: integer; Len: integer; var from, to: integer; step: integer);
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
    if (to < -1) or (to > Len) then
      raise new ArgumentException(GetTranslation(PARAMETER_TO_OUT_OF_RANGE));}
  
  CorrectFromTo(situation, Len, from, to, step);
end;

///-- 
function SystemSliceListImpl<T>(Self: List<T>; situation: integer; from, to: integer; step: integer := 1): List<T>;
begin
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation, Self.Count, from, to, step);
  
  Result := CreateSliceFromListInternal(Self, from, step, count);
end;

///--
function SystemSlice<T>(Self: List<T>; situation: integer; from, to: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImpl(Self, situation, from, to, 1);
end;

///--
function SystemSlice<T>(Self: List<T>; situation: integer; from, to, step: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImpl(Self, situation, from, to, step);
end;

///-- 
function SystemSliceListImplQuestion<T>(Self: List<T>; situation: integer; from, to: integer; step: integer := 1): List<T>;
begin
  var count := CorrectFromToAndCalcCountForSystemSliceQuestion(situation, Self.Count, from, to, step);
  
  Result := CreateSliceFromListInternal(Self, from, step, count);
end;

///--
function SystemSliceQuestion<T>(Self: List<T>; situation: integer; from, to: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImplQuestion(Self, situation, from, to, 1);
end;

///--
function SystemSliceQuestion<T>(Self: List<T>; situation: integer; from, to, step: integer): List<T>; extensionmethod;
begin
  Result := SystemSliceListImplQuestion(Self, situation, from, to, step);
end;

// -----------------------------------------------------
//>>     Методы расширения двумерных динамических массивов # Extension methods for array [,] of T
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

/// Вывод двумерного массива, w - ширина поля вывода
function Print<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
  begin
    for var j := 0 to Self.ColCount - 1 do
    begin
      var elem := Self[i, j];
      var s := StructuredObjectToString(elem);
      Write(s.PadLeft(w));
    end;
    Writeln;  
  end;
  Result := Self;  
end;

/// Вывод двумерного вещественного массива по формату :w:f
function Print(Self: array [,] of real; w: integer := 7; f: integer := 2): array [,] of real; extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
  begin
    for var j := 0 to Self.ColCount - 1 do
      Write(FormatValue(Self[i, j], w, f));
    Writeln;  
  end;
  Result := Self;  
end;

/// Вывод двумерного массива и переход на следующую строку, w - ширина поля вывода
function Println<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod;
begin
  Self.Print(w);
  Result := Self;  
end;

/// Вывод двумерного вещественного массива по формату :w:f и переход на следующую строку 
function Println(Self: array [,] of real; w: integer := 7; f: integer := 2): array [,] of real; extensionmethod;
begin
  Self.Print(w, f);
  Result := Self;  
end;

/// k-тая строка двумерного массива
function Row<T>(Self: array [,] of T; k: integer): array of T; extensionmethod;
begin
  var n := Self.ColCount;
  var res := new T[n];
  for var j := 0 to n - 1 do
    res[j] := Self[k, j];
  Result := res;
end;

/// k-тый столбец двумерного массива
function Col<T>(Self: array [,] of T; k: integer): array of T; extensionmethod;
begin
  var m := Self.RowCount;
  var res := new T[m];
  for var i := 0 to m - 1 do
    res[i] := Self[i, k];
  Result := res;
end;

/// k-тая строка двумерного массива как последовательность
function RowSeq<T>(Self: array [,] of T; k: integer): sequence of T; extensionmethod;
begin
  for var j := 0 to Self.ColCount - 1 do
    yield Self[k, j];
end;

/// k-тый столбец двумерного массива как последовательность
function ColSeq<T>(Self: array [,] of T; k: integer): sequence of T; extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    yield Self[i, k];
end;

/// Возвращает последовательность строк двумерного массива 
function Rows<T>(Self: array [,] of T): sequence of sequence of T; extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    yield Self.RowSeq(i);
end;

/// Возвращает последовательность столбцов двумерного массива 
function Cols<T>(Self: array [,] of T): sequence of sequence of T; extensionmethod;
begin
  for var j := 0 to Self.ColCount - 1 do
    yield Self.ColSeq(j);
end;

/// Меняет местами две строки двумерного массива с номерами k1 и k2
procedure SwapRows<T>(Self: array [,] of T; k1, k2: integer); extensionmethod;
begin
  for var j := 0 to Self.ColCount - 1 do
    Swap(Self[k1, j], Self[k2, j])
end;

/// Меняет местами два столбца двумерного массива с номерами k1 и k2
procedure SwapCols<T>(Self: array [,] of T; k1, k2: integer); extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    Swap(Self[i, k1], Self[i, k2])
end;

/// Меняет строку k двумерного массива на другую строку
procedure SetRow<T>(Self: array [,] of T; k: integer; a: array of T); extensionmethod;
begin
  if a.Length <> Self.ColCount then
    raise new System.ArgumentException(GetTranslation(ARR_LENGTH_MUST_BE_MATCH_TO_MATR_SIZE));
  for var j := 0 to Self.ColCount - 1 do
    Self[k, j] := a[j]
end;

/// Меняет строку k двумерного массива на другую строку
procedure SetRow<T>(Self: array [,] of T; k: integer; a: sequence of T); extensionmethod := Self.SetRow(k,a.ToArray);

/// Меняет столбец k двумерного массива на другой столбец
procedure SetCol<T>(Self: array [,] of T; k: integer; a: array of T); extensionmethod;
begin
  if a.Length <> Self.RowCount then
    raise new System.ArgumentException(GetTranslation(ARR_LENGTH_MUST_BE_MATCH_TO_MATR_SIZE));
  for var i := 0 to Self.RowCount - 1 do
    Self[i, k] := a[i]
end;

/// Меняет столбец k двумерного массива на другой столбец
procedure SetCol<T>(Self: array [,] of T; k: integer; a: sequence of T); extensionmethod := Self.SetCol(k,a.ToArray);

/// Возвращает по заданному двумерному массиву последовательность (a[i,j],i,j)
function ElementsWithIndexes<T>(Self: array [,] of T): sequence of (T, integer, integer); extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    for var j := 0 to Self.ColCount - 1 do
      yield (Self[i, j], i, j)
end;

/// Возвращает по заданному двумерному массиву последовательность его элементов по строкам
function ElementsByRow<T>(Self: array [,] of T): sequence of T; extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    for var j := 0 to Self.ColCount - 1 do
      yield Self[i, j]
end;

/// Возвращает по заданному двумерному массиву последовательность его элементов по столбцам
function ElementsByCol<T>(Self: array [,] of T): sequence of T; extensionmethod;
begin
  for var j := 0 to Self.ColCount - 1 do
    for var i := 0 to Self.RowCount - 1 do
      yield Self[i, j]
end;

/// Преобразует элементы двумерного массива и возвращает преобразованный массив
function ConvertAll<T, T1>(Self: array [,] of T; converter: T->T1): array [,] of T1; extensionmethod;
begin
  Result := new T1[Self.RowCount, Self.ColCount];
  for var i := 0 to Self.RowCount - 1 do
    for var j := 0 to Self.ColCount - 1 do
      Result[i, j] := converter(Self[i, j]);  
end;

/// Преобразует элементы двумерного массива по заданному правилу
procedure Transform<T>(Self: array [,] of T; f: T->T); extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    for var j := 0 to Self.ColCount - 1 do
      Self[i, j] := f(Self[i, j]);
end;

/// Заполняет элементы двумерного массива значениями, вычисляемыми по некоторому правилу
procedure Fill<T>(Self: array [,] of T; f: (integer,integer) ->T); extensionmethod;
begin
  for var i := 0 to Self.RowCount - 1 do
    for var j := 0 to Self.ColCount - 1 do
      Self[i, j] := f(i, j);
end;

// -----------------------------------------------------
//>>     Фиктивная секция YYY - не удалять! # YYY
// -----------------------------------------------------

function Matr<T>(m,n: integer; params data: array of T): array [,] of T;
begin
  if data.Length<>m*n then
    raise new System.ArgumentException(GetTranslation(INITELEM_COUNT_MUST_BE_EQUAL_TO_MATRIX_ELEMS_COUNT));
  
  Result := new T[m, n];
  var k := 0;
  for var i:=0 to Result.RowCount-1 do
  for var j:=0 to Result.ColCount-1 do
  begin
    Result[i,j] := data[k];
    k += 1;
  end;
end;

// Реализация операций с матрицами - только после введения RowCount и ColCount
function MatrRandom(m: integer; n: integer; a, b: integer): array [,] of integer;
begin
  Result := new integer[m, n];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := Random(a, b);
end;

function MatrRandomInteger(m: integer; n: integer; a, b: integer): array [,] of integer;
begin
  Result := new integer[m, n];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := Random(a, b);
end;

function MatrRandomReal(m: integer; n: integer; a, b: real): array [,] of real;
begin
  Result := new real[m, n];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := Random() * (b - a) + a;
end;

function MatrFill<T>(m, n: integer; x: T): array [,] of T;
begin
  Result := new T[m, n];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := x;
end;

function MatrGen<T>(m, n: integer; gen: (integer,integer)->T): array [,] of T;
begin
  Result := new T[m, n];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := gen(i, j);
end;

function Transpose<T>(a: array [,] of T): array [,] of T;
begin
  var m := a.RowCount;
  var n := a.ColCount;
  Result := new T[n, m];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := a[j, i]
end;

function ReadMatrInteger(m, n: integer): array [,] of integer;
begin
  Result := new integer[m, n];
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      Result[i, j] := ReadInteger;
end;

function ReadMatrReal(m, n: integer): array [,] of real;
begin
  Result := new real[m, n];
  for var i := 0 to m - 1 do
    for var j := 0 to n - 1 do
      Result[i, j] := ReadReal;
end;

// -----------------------------------------------------
//>>     Методы расширения одномерных динамических массивов # Extension methods for array of T
// -----------------------------------------------------

// Дополнения февраль 2016: Shuffle, AdjacentFind, IndexMin, IndexMax, Replace, Transform
//   Статические методы - в методы расширения: BinarySearch, ConvertAll, Find, FindIndex, FindAll,  
//   FindLast, FindLastIndex, IndexOf, Contains, LastIndexOf, Reverse, Sort

/// Перемешивает элементы массива случайным образом
function Shuffle<T>(Self: array of T): array of T; extensionmethod;
begin
  var n := Self.Length;
  for var i := 0 to n - 1 do
    Swap(Self[i], Self[Random(n)]);
  Result := Self;  
end;

{/// Находит первую пару подряд идущих одинаковых элементов и возвращает индекс первого элемента пары. Если не найден, возвращается -1
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
end;}

/// Возвращает минимальный элемент 
function Min<T>(Self: array of T): T; extensionmethod;where T: System.IComparable<T>;
begin
  Result := Self[0];
  for var i := 1 to Self.Length - 1 do
    if Self[i].CompareTo(Result) < 0 then 
      Result := Self[i];
end;

/// Возвращает максимальный элемент 
function Max<T>(Self: array of T): T; extensionmethod;where T: System.IComparable<T>;
begin
  Result := Self[0];
  for var i := 1 to Self.Length - 1 do
    if Self[i].CompareTo(Result) > 0 then 
      Result := Self[i];
end;

/// Возвращает минимальный элемент 
function Min(Self: array of integer): integer; extensionmethod;
begin
  Result := Self[0];
  for var i := 1 to Self.Length - 1 do
    if Self[i] < Result then 
      Result := Self[i];
end;

/// Возвращает минимальный элемент 
function Min(Self: array of real): real; extensionmethod;
begin
  Result := Self[0];
  for var i := 1 to Self.Length - 1 do
    if Self[i] < Result then 
      Result := Self[i];
end;

/// Возвращает максимальный элемент 
function Max(Self: array of integer): integer; extensionmethod;
begin
  Result := Self[0];
  for var i := 1 to Self.Length - 1 do
    if Self[i] > Result then 
      Result := Self[i];
end;

/// Возвращает максимальный элемент 
function Max(Self: array of real): real; extensionmethod;
begin
  Result := Self[0];
  for var i := 1 to Self.Length - 1 do
    if Self[i] > Result then 
      Result := Self[i];
end;

{/// Возвращает индекс первого минимального элемента начиная с позиции index
function IndexMin<T>(Self: array of T; index: integer := 0): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var min := Self[index];
  Result := index;
  for var i:=index+1 to Self.Length-1 do
    if Self[i].CompareTo(min)<0 then 
    begin
      Result := i;
      min := Self[i];
    end;
end;

/// Возвращает индекс первого максимального элемента начиная с позиции index
function IndexMax<T>(self: array of T; index: integer := 0): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var max := Self[index];
  Result := index;
  for var i:=index+1 to Self.Length-1 do
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

/// Возвращает индекс последнего минимального элемента в диапазоне [0,index] 
function LastIndexMin<T>(Self: array of T; index: integer): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var min := Self[index];
  Result := index;
  for var i:=index-1 downto 0 do
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

/// Возвращает индекс последнего минимального элемента в диапазоне [0,index]
function LastIndexMax<T>(Self: array of T; index: integer): integer; extensionmethod; where T: System.IComparable<T>;
begin
  var max := Self[index];
  Result := index;
  for var i:=index-1 downto 0 do
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
  for var i:=0 to Self.Length-1 do
    Self[i] := f(Self[i]);
end;

/// Заполняет элементы массива значениями, вычисляемыми по некоторому правилу
procedure Fill<T>(Self: array of T; f: integer -> T); extensionmethod;
begin
  for var i:=0 to Self.Length-1 do
    Self[i] := f(i);
end;}

/// Выполняет бинарный поиск в отсортированном массиве
function BinarySearch<T>(Self: array of T; x: T): integer; extensionmethod;
begin
  Result := System.Array.BinarySearch(self, x);  
end;

/// Преобразует элементы массива и возвращает преобразованный массив
function ConvertAll<T, T1>(Self: array of T; converter: T->T1): array of T1; extensionmethod;
begin
  Result := System.Array.ConvertAll(self, t -> converter(t));  
end;

/// Выполняет поиск первого элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function Find<T>(Self: array of T; p: T->boolean): T; extensionmethod;
begin
  Result := System.Array.Find(self, p);  
end;

/// Выполняет поиск индекса первого элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается -1
function FindIndex<T>(Self: array of T; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindIndex(self, p);  
end;

/// Выполняет поиск индекса первого элемента в массиве, удовлетворяющего предикату, начиная с индекса start. Если не найден, возвращается -1
function FindIndex<T>(Self: array of T; start: integer; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindIndex(self, start, p);  
end;

/// Возвращает в виде массива все элементы, удовлетворяющие предикату
function FindAll<T>(Self: array of T; p: T->boolean): array of T; extensionmethod;
begin
  Result := System.Array.FindAll(self, p);  
end;

/// Выполняет поиск последнего элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function FindLast<T>(Self: array of T; p: T->boolean): T; extensionmethod;
begin
  Result := System.Array.FindLast(self, p);  
end;

/// Выполняет поиск индекса последнего элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function FindLastIndex<T>(Self: array of T; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindLastIndex(self, p);  
end;

/// Выполняет поиск индекса последнего элемента в массиве, удовлетворяющего предикату, начиная с индекса start. Если не найден, возвращается нулевое значение соответствующего типа
function FindLastIndex<T>(self: array of T; start: integer; p: T->boolean): integer; extensionmethod;
begin
  Result := System.Array.FindLastIndex(Self, start, p);  
end;

/// Возвращает индекс первого вхождения элемента или -1 если элемент не найден
function IndexOf<T>(Self: array of T; x: T): integer; extensionmethod;
begin
  Result := System.Array.IndexOf(Self, x);  
end;

/// Возвращает индекс первого вхождения элемента начиная с индекса start или -1 если элемент не найден
function IndexOf<T>(Self: array of T; x: T; start: integer): integer; extensionmethod;
begin
  Result := System.Array.IndexOf(Self, x, start);  
end;

/// Возвращает индекс последнего вхождения элемента или -1 если элемент не найден
function LastIndexOf<T>(Self: array of T; x: T): integer; extensionmethod;
begin
  Result := System.Array.LastIndexOf(Self, x);  
end;

/// Возвращает индекс последнего вхождения элемента начиная с индекса start или -1 если элемент не найден
function LastIndexOf<T>(Self: array of T; x: T; start: integer): integer; extensionmethod;
begin
  Result := System.Array.LastIndexOf(Self, x, start);  
end;

/// Сортирует массив по возрастанию
procedure Sort<T>(Self: array of T); extensionmethod;
begin
  System.Array.Sort(Self);  
end;

/// Сортирует массив по возрастанию, используя cmp в качестве функции сравнения элементов
procedure Sort<T>(Self: array of T; cmp: (T,T) ->integer); extensionmethod;
begin
  System.Array.Sort(Self, cmp);  
end;

/// Возвращает индекс последнего элемента массива
function High(Self: System.Array); extensionmethod := High(Self);

/// Возвращает индекс первого элемента массива
function Low(Self: System.Array); extensionmethod := Low(Self);

/// Возвращает последовательность индексов одномерного массива
function Indexes<T>(Self: array of T): sequence of integer; extensionmethod := Range(0, Self.Length - 1);

/// Возвращает последовательность индексов элементов одномерного массива, удовлетворяющих условию
function IndexesOf<T>(Self: array of T; cond: T->boolean): sequence of integer; extensionmethod;
begin
  for var i := 0 to Self.High do
    if cond(Self[i]) then
      yield i;
end;

/// Возвращает последовательность индексов элементов одномерного массива, удовлетворяющих условию
function IndexesOf<T>(Self: array of T; cond: (T,integer) ->boolean): sequence of integer; extensionmethod;
begin
  for var i := 0 to Self.High do
    if cond(Self[i], i) then
      yield i;
end;

///-- 
function CreateSliceFromArrayInternal<T>(Self: array of T; from, step, count: integer): array of T;
begin
  Result := new T[count];
  
  var f := from;
  for var i := 0 to count - 1 do
  begin
    Result[i] := Self[f];
    f += step;
  end;
end;

///-- 
function SliceArrayImpl<T>(Self: array of T; from, step, count: integer): array of T;
begin
  {if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));
  
  if (from < 0) or (from > Self.Length - 1) then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  var cnt := step > 0 ? Self.Length - from : from + 1; 
  var cntstep := (cnt-1) div abs(step) + 1;
  if count > cntstep then 
    count := cntstep;}
  
  CorrectCountForSlice(Self.Length, from, step, count);  
  
  Result := CreateSliceFromArrayInternal(Self, from, step, count)
end;

/// Возвращает срез массива от индекса from с шагом step
function Slice<T>(Self: array of T; from, step: integer): array of T; extensionmethod;
begin
  Result := SliceArrayImpl(Self, from, step, integer.MaxValue);
end;

/// Возвращает срез массива от индекса from с шагом step длины не более count
function Slice<T>(Self: array of T; from, step, count: integer): array of T; extensionmethod;
begin
  Result := SliceArrayImpl(Self, from, step, count);
end;

///-- 
function SystemSliceArrayImpl<T>(Self: array of T; situation: integer; from, to: integer; step: integer := 1): array of T;
begin
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation, Self.Length, from, to, step);
  
  Result := CreateSliceFromArrayInternal(Self, from, step, count)
end;

///--
function SystemSlice<T>(Self: array of T; situation: integer; from, to: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImpl(Self, situation, from, to, 1);
end;

///--
function SystemSlice<T>(Self: array of T; situation: integer; from, to, step: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImpl(Self, situation, from, to, step);
end;

///-- 
function SystemSliceArrayImplQuestion<T>(Self: array of T; situation: integer; from, to: integer; step: integer := 1): array of T;
begin
  var count := CorrectFromToAndCalcCountForSystemSliceQuestion(situation, Self.Length, from, to, step);
  
  Result := CreateSliceFromArrayInternal(Self, from, step, count);
end;

///--
function SystemSliceQuestion<T>(Self: array of T; situation: integer; from, to: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImplQuestion(Self, situation, from, to, 1);
end;

///--
function SystemSliceQuestion<T>(Self: array of T; situation: integer; from, to, step: integer): array of T; extensionmethod;
begin
  Result := SystemSliceArrayImplQuestion(Self, situation, from, to, step);
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

/// Возвращает True если значение находится между двумя другими
function Between(Self: integer; a, b: integer): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

/// Возвращает True если значение находится между двумя другими
function InRange(Self: integer; a,b: integer): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
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
  Result := Range(1, Self);  
end;

// Дополнения февраль 2016: To, Downto, Times

/// Генерирует последовательность целых от текущего значения до n
function To(Self: integer; n: integer): sequence of integer; extensionmethod;
begin
  Result := Range(Self, n);
end;

/// Генерирует последовательность целых от текущего значения до n в убывающем порядке
function Downto(Self: integer; n: integer): sequence of integer; extensionmethod;
begin
  Result := Range(Self, n, -1); 
end;

/// Возвращает последовательность целых 0,1,...n-1
function Times(Self: integer): sequence of integer; extensionmethod;
begin
  Result := Range(0, Self - 1);
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
/// Возвращает True если значение находится между двумя другими
function Between(Self: real; a, b: real): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

/// Возвращает True если значение находится между двумя другими
function InRange(Self: real; a,b: real): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

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

/// Возвращает x, округленное до ближайшего вещественного с digits знаками после десятичной точки
function Round(Self: real; digits: integer): real; extensionmethod;
begin
  Result := Round(Self,digits);
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
  if frac < 0 then
    raise new System.ArgumentOutOfRangeException('frac', 'frac<0');
  if frac >= 100 then
    raise new System.ArgumentOutOfRangeException('frac', 'frac>=100');
  Result := Format('{0:f' + frac + '}', Self)
end;


//------------------------------------------------------------------------------
//>>     Методы расширения типа char # Extension methods for char
//------------------------------------------------------------------------------
/// Возвращает True если значение находится между двумя другими
function Between(Self: char; a, b: char): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

/// Возвращает True если значение находится между двумя другими
function InRange(Self: char; a,b: char): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

/// Предыдущий символ
function Pred(Self: char); extensionmethod := PABCSystem.Pred(Self);

/// Следующий символ
function Succ(Self: char); extensionmethod := PABCSystem.Succ(Self);

/// Код символа в кодировке Unicode
function Code(Self: char): integer; extensionmethod := word(Self);

/// Является ли символ цифрой
function IsDigit(Self: char); extensionmethod := char.IsDigit(Self);

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
  if (Result < 0) or (Result >= 10) then
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
/// Возвращает True если значение находится между двумя другими
function Between(Self: string; a, b: string): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

/// Возвращает True если значение находится между двумя другими
function InRange(Self: string; a,b: string): boolean; extensionmethod;
begin
  Result := (a <= Self) and (Self <= b) or (b <= Self) and (Self <= a);
end;

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
function ToInteger(Self: string): integer; extensionmethod := integer.Parse(Self);

/// Преобразует строку в BigInteger
function ToBigInteger(Self: string): BigInteger; extensionmethod := BigInteger.Parse(Self);

/// Преобразует строку в вещественное
function ToReal(Self: string): real; extensionmethod := real.Parse(Self, nfi);

/// Преобразует строку в целое и записывает его в value. 
///При невозможности преобразования возвращается False
function TryToInteger(Self: string; var value: integer): boolean; extensionmethod := TryStrToInt(Self,value);

/// Преобразует строку в вещественное и записывает его в value. 
///При невозможности преобразования возвращается False
function TryToReal(Self: string; var value: real): boolean; extensionmethod := TryStrToReal(Self,value);

/// Преобразует строку в целое
///При невозможности преобразования возвращается defaultvalue
function ToInteger(Self: string; defaultvalue: integer): integer; extensionmethod;
begin
  var b := TryStrToInt(Self,Result);
  if not b then
    Result := defaultvalue
end;

/// Преобразует строку в вещественное
///При невозможности преобразования возвращается defaultvalue
function ToReal(Self: string; defaultvalue: real): real; extensionmethod;
begin
  var b := TryStrToReal(Self,Result);
  if not b then
    Result := defaultvalue
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
  for var i := Self.Length downto 1 do
    sb.Append(Self[i]);
  Result := sb.ToString;
end;

// Дополнения февраль 2016: Matches, MatchValue, MatchValues, IsMatch, RegexReplace, Remove, Right, Left

/// Заменяет в указанной строке все вхождения регулярного выражения указанной строкой замены и возвращает преобразованную строку
function RegexReplace(Self: string; reg, repl: string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
begin
  Result := Regex.Replace(Self, reg, repl, options)
end;

/// Заменяет в указанной строке все вхождения регулярного выражения указанным преобразованием замены и возвращает преобразованную строку
function RegexReplace(Self: string; reg: string; repl: Match->string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
begin
  Result := Regex.Replace(Self, reg, repl, options)
end;

/// Ищет в указанной строке все вхождения регулярного выражения и возвращает их в виде последовательности элементов типа Match
function Matches(Self: string; reg: string; options: RegexOptions := RegexOptions.None): sequence of Match; extensionmethod;
begin
  Result := (new Regex(reg, options)).Matches(Self).Cast<Match>();
end;

/// Ищет в указанной строке первое вхождение регулярного выражения и возвращает его в виде строки
function MatchValue(Self: string; reg: string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
begin
  Result := (new Regex(reg, options)).Match(Self).Value;
end;

/// Ищет в указанной строке все вхождения регулярного выражения и возвращает их в виде последовательности строк
function MatchValues(Self: string; reg: string; options: RegexOptions := RegexOptions.None): sequence of string; extensionmethod;
begin
  Result := Self.Matches(reg, options).Select(m -> m.Value);
end;

/// Удовлетворяет ли строка регулярному выражению
function IsMatch(Self: string; reg: string; options: RegexOptions := RegexOptions.None): boolean; extensionmethod := Regex.IsMatch(Self, reg, options);

/// Удаляет в строке все вхождения указанных строк
function Remove(Self: string; params targets: array of string): string; extensionmethod;
begin
  var builder := new StringBuilder(Self);
  
  for var i := 0 to targets.Length - 1 do
    builder.Replace(targets[i], String.Empty);
  
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
function CreateSliceFromStringInternal(Self: string; from, step, count: integer): string;
begin
  var res := new StringBuilder(count);
  
  loop count do
  begin
    res.Append(Self[from]);
    from += step;
  end;
  Result := res.ToString;
end;

///-- 
function SliceStringImpl(Self: string; from, step, count: integer): string;
begin
  {if step = 0 then
    raise new ArgumentException(GetTranslation(PARAMETER_STEP_MUST_BE_NOT_EQUAL_0));
  
  if (from < 0) or (from > Self.Length - 1) then
    raise new ArgumentException(GetTranslation(PARAMETER_FROM_OUT_OF_RANGE));
  
  var cnt := step > 0 ? Self.Length - from : from + 1; 
  var cntstep := (cnt-1) div abs(step) + 1;
  if count > cntstep then 
    count := cntstep;}
  
  CorrectCountForSlice(Self.Length, from, step, count);
  
  Result := CreateSliceFromStringInternal(Self, from + 1, step, count);
end;

/// Возвращает срез строки от индекса from с шагом step
function Slice(Self: string; from, step: integer): string; extensionmethod;
begin
  Result := SliceStringImpl(Self, from, step, integer.MaxValue);
end;

/// Возвращает срез строки от индекса from с шагом step длины не более count
function Slice(Self: string; from, step, count: integer): string; extensionmethod;
begin
  Result := SliceStringImpl(Self, from, step, count);
end;

///-- 
function SystemSliceStringImpl(Self: string; situation: integer; from, to: integer; step: integer := 1): string;
begin
  var fromv := from - 1;
  var tov := to - 1;
  var count := CheckAndCorrectFromToAndCalcCountForSystemSlice(situation, Self.Length, fromv, tov, step);
  
  Result := CreateSliceFromStringInternal(Self, fromv + 1, step, count)
end;

///--
function SystemSlice(Self: string; situation: integer; from, to: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImpl(Self, situation, from, to, 1);
end;

///--
function SystemSlice(Self: string; situation: integer; from, to, step: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImpl(Self, situation, from, to, step);
end;

///-- 
function SystemSliceStringImplQuestion(Self: string; situation: integer; from, to: integer; step: integer := 1): string;
begin
  var fromv := from - 1;
  var tov := to - 1;
  
  var count := CorrectFromToAndCalcCountForSystemSliceQuestion(situation, Self.Length, fromv, tov, step);
  
  Result := CreateSliceFromStringInternal(Self, fromv + 1, step, count);
end;

///--
function SystemSliceQuestion(Self: string; situation: integer; from, to: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImplQuestion(Self, situation, from, to, 1);
end;

///--
function SystemSliceQuestion(Self: string; situation: integer; from, to, step: integer): string; extensionmethod;
begin
  Result := SystemSliceStringImplQuestion(Self, situation, from, to, step);
end;
//--------------------------------------------
//>>     Методы расширения типа Func # Extension methods for Func
//--------------------------------------------
/// Суперпозиция функций
function Compose<T1, T2, TResult>(Self: T2->TResult; composer: T1->T2): T1->TResult; extensionmethod;
begin
  if composer = nil then
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
//>>     Методы расширения словарей # Extension methods for IDictionary
// -----------------------------------------------------------------------------
/// Возвращает в словаре значение, связанное с указанным ключом, а если такого ключа нет, то значение по умолчанию
function Get<Key, Value>(Self: IDictionary<Key, Value>; K: Key): Value; extensionmethod;
begin
  var b := Self.TryGetValue(K, Result);
  if not b then 
    Result := default(Value);
end;



// -----------------------------------------------------
//>>     Подпрограммы для работы с типизированными файлами # Subroutines for typed files
// -----------------------------------------------------

/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenBinary<T>(fname: string): file of T;
begin
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateBinary<T>(fname: string): file of T;
begin
  PABCSystem.Rewrite(Result, fname);
end;

/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string): file of T;
begin
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string): file of T;
begin
  PABCSystem.Rewrite(Result, fname);
end;

/// Открывает типизированный файл целых и возвращает значение для инициализации файловой переменной
function OpenFileInteger(fname: string): file of integer;
begin
  Result := OpenFile<integer>(fname);
end;

/// Открывает типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function OpenFileReal(fname: string): file of real;
begin
  Result := OpenFile<real>(fname);
end;

/// Создаёт или обнуляет типизированный файл целых и возвращает значение для инициализации файловой переменной
function CreateFileInteger(fname: string): file of integer;
begin
  Result := CreateFile<integer>(fname);
end;

/// Создаёт или обнуляет типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function CreateFileReal(fname: string): file of real;
begin
  Result := CreateFile<real>(fname);
end;

/// Открывает типизированный файл, возвращает последовательность его элементов и закрывает его
procedure WriteElements<T>(fname: string; ss: sequence of T);
begin
  var f := CreateBinary<T>(fname);
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

/// Открывает типизированный файл, возвращает последовательность его элементов и закрывает его
function ReadElements<T>(fname: string): sequence of T;
begin
  var f := OpenBinary<T>(fname);
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

