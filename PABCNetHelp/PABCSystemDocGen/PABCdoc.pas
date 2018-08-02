//>>     Стандартные константы # Standard constants
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
Pi = 3.141592653589793;
/// Константа E
E = 2.718281828459045;
/// Константа перехода на новую строку
NewLine = System.Environment.NewLine;
//>>     Стандартные типы # Standard types
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
//>>     Подпрограммы ввода # Read subroutines
/// Вводит значения a,b,... с клавиатуры
procedure Read(a,b,...);
/// Вводит значения a,b,... с клавиатуры и осуществляет переход на следующую строку
procedure Readln(a,b,...);
/// Вводит числовое значение x клавиатуры. Возвращает False если при вводе произошла ошибка
function TryRead(var x: integer): boolean;
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
/// Выводит приглашение к вводу и возвращает значение типа integer, введенное с клавиатуры,и осуществляет переход на следующую строку ввода
function ReadlnInteger(prompt: string): integer;
/// Выводит приглашение к вводу и возвращает значение типа real, введенное с клавиатуры,и осуществляет переход на следующую строку ввода
function ReadlnReal(prompt: string): real;
/// Выводит приглашение к вводу и возвращает значение типа char, введенное с клавиатуры,и осуществляет переход на следующую строку ввода
function ReadlnChar(prompt: string): char;
/// Выводит приглашение к вводу и возвращает значение типа string, введенное с клавиатуры,и осуществляет переход на следующую строку ввода
function ReadlnString(prompt: string): string;
/// Выводит приглашение к вводу и возвращает значение типа boolean, введенное с клавиатуры,и осуществляет переход на следующую строку ввода
function ReadlnBoolean(prompt: string): boolean;
/// Вводит значения a,b,... из файла f
procedure Read(f: файл; a,b,...);
/// Вводит значения a,b,... из текстового файла f и осуществляет переход на следующую строку
procedure Readln(f: Text; a,b,...);
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
/// Возвращает значение типа integer, введенное из текстового файла f,и осуществляет переход на следующую строку
function ReadlnInteger(f: Text): integer;
/// Возвращает значение типа real, введенное из текстового файла f,и осуществляет переход на следующую строку
function ReadlnReal(f: Text): real;
/// Возвращает значение типа char, введенное из текстового файла f,и осуществляет переход на следующую строку
function ReadlnChar(f: Text): char;
/// Возвращает значение типа string, введенное из текстового файла f,и осуществляет переход на следующую строку
function ReadlnString(f: Text): string;
/// Возвращает значение типа boolean, введенное из текстового файла f,и осуществляет переход на следующую строку
function ReadlnBoolean(f: Text): boolean;
//>>     Подпрограммы вывода # Write subroutines
/// Выводит значения a,b,... на экран
procedure Write(a,b,...);
/// Выводит значения a,b,... на экран и осуществляет переход на новую строку
procedure Writeln(a,b,...);
/// Выводит значения a,b,... в файл f
procedure Write(f: файл; a,b,...);
/// Выводит значения a,b,... в текстовый файл f и осуществляет переход на новую строку
procedure Writeln(f: Text; a,b,...);
/// Выводит значения args согласно форматной строке formatstr
procedure WriteFormat(formatstr: string; params args: array of object);
/// Выводит значения args согласно форматной строке formatstr и осуществляет переход на новую строку
procedure WritelnFormat(formatstr: string; params args: array of object);
/// Выводит значения args в текстовый файл f согласно форматной строке formatstr
procedure WriteFormat(f: Text; formatstr: string; params args: array of object);
/// Выводит значения args в текстовый файл f согласно форматной строке formatstrи осуществляет переход на новую строку
procedure WritelnFormat(f: Text; formatstr: string; params args: array of object);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел
procedure Print(a,b,...);
/// Выводит значения a,b,... в текстовый файл f, после каждого значения выводит пробел
procedure Print(f: Text; a,b,...);
/// Выводит значения a,b,... на экран, после каждого значения выводит пробел и переходит на новую строку
procedure Println(a,b,...);
/// Выводит значения a,b,... в текстовый файл f, после каждого значения выводит пробел и переходит на новую строку
procedure Println(f: Text; a,b,...);
//>>     Общие подпрограммы для работы с файлами # Common subroutines for files
/// Связывает файловую переменную с файлом на диске
procedure Assign(f: файл; name: string);
/// Связывает файловую переменную с файлом на диске
procedure AssignFile(f: файл; name: string);
/// Закрывает файл
procedure Close(f: файл);
/// Закрывает файл
procedure CloseFile(f: файл);
/// Возвращает True, если достигнут конец файла
function Eof(f: файл): boolean;
/// Удаляет файл, связанный с файловой переменной
procedure Erase(f: файл);
/// Переименовывает файл, связаный с файловой переменной, давая ему имя newname.
procedure Rename(f: файл; newname: string);
//>>     Подпрограммы для работы с текстовыми файлами # Subroutines for text files
/// Открывает текстовый файл на чтение в кодировке Windows
procedure Reset(f: Text);
/// Открывает текстовый файл на чтение в указанной кодировке
procedure Reset(f: Text; en: Encoding);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл на чтение в кодировке Windows
procedure Reset(f: Text; name: string);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл на чтение в указанной кодировке
procedure Reset(f: Text; name: string; en: Encoding);
/// Открывает текстовый файл на запись в кодировке Windows.Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text);
/// Открывает текстовый файл на запись в указанной кодировке.Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text; en: Encoding);
/// Связывает файловую переменную с именем файла name и открывает текстовый файл f на запись в кодировке Windows.Если файл существовал - он обнуляется, если нет - создается пустой
procedure Rewrite(f: Text; name: string);
/// Связывает файловую переменную f с именем файла name и открывает текстовый файл f на запись в указанной кодировке.Если файл существовал - он обнуляется, если нет - создается пустой
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
/// Возвращает True, если в файле достигнут конец строки
function Eoln(f: Text): boolean;
/// Пропускает пробельные символы, после чего возвращает True, если достигнут конец файла
function SeekEof(f: Text): boolean;
/// Пропускает пробельные символы, после чего возвращает True, если в файле достигнут конец строки
function SeekEoln(f: Text): boolean;
/// Записывает содержимое буфера файла на диск
procedure Flush(f: Text);
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
//>>     Подпрограммы для работы с двоичными файлами # Subroutines for binary files
/// Открывает двоичный файл на чтение и запись.Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Reset(f: двоичный файл);
/// Связывает файловую переменную f с файлом name на диске и открывает двоичный файл на чтение и запись.Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Reset(f: двоичный файл; name: string);
/// Открывает двоичный файл на чтение и запись, при этом обнуляя его содержимое. Если файл существовал, он обнуляется.Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Rewrite(f: двоичный файл);
/// Связывает файловую переменную f с файлом name на диске и открывает двоичный файл на чтение и запись, при этом обнуляя его содержимое.Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Rewrite(f: двоичный файл; name: string);
/// Усекает двоичный файл, отбрасывая все элементы с позиции файлового указателя.Двоичный файл - это либо типизированный файл file of T, либо бестиповой файл file
procedure Truncate(f: двоичный файл);
/// Возвращает текущую позицию файлового указателя в двоичном файле
function FilePos(f: двоичный файл): int64;
/// Возвращает количество элементов в двоичном файле
function FileSize(f: двоичный файл): int64;
/// Устанавливает текущую позицию файлового указателя в двоичном файле на элемент с данным номером
procedure Seek(f: двоичный файл; n: int64);
//>>     Cистемные подпрограммы # System subroutines
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
/// Выделяет динамическую память размера sizeof(T) и возвращает в переменной p указатель на нее. Тип T должен быть размерным
procedure New<T>(var p: ^T);
/// Освобождает динамическую память, на которую указывает p
procedure Dispose<T>(var p: ^T);
//>>     Функции для работы с именами файлов # Functions for file names
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
//>>     Математические подпрограммы # Math subroutines
/// Возвращает знак числа x
function Sign(x: число): число;
/// Возвращает модуль числа x
function Abs(x: число): число;
/// Возвращает синус числа x
function Sin(x: real): real;
/// Возвращает гиперболический синус числа x
function Sinh(x: real): real;
/// Возвращает косинус числа x
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
/// Возвращает квадрат числа x
function Sqr(x: число): число;
/// Возвращает x в степени y
function Power(x, y: real): real;
/// Возвращает x в целой степени n
function Power(x: real; n: integer): real;
/// Возвращает x в степени y
function Power(x: BigInteger; y: integer): BigInteger;
/// Возвращает x, округленное до ближайшего целого. Если вещественное находится посередине между двумя целыми,то округление осуществляется к ближайшему четному (банковское округление): Round(2.5)=2, Round(3.5)=4
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
/// Возвращает максимальное из чисел a,b
function Max(a: число, b: число): число;
/// Возвращает минимальное из чисел a,b
function Min(a: число, b: число): число;
/// Возвращает True, если i нечетно, и False в противном случае
function Odd(i: целое): boolean;
//>>     Функции для работы с комплексными числами # Functions for Complex numbers
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
//>>     Процедуры для работы со стандартными множествами # Subroutines for set of T
///Добавляет элемент element во множество s
procedure Include(var s: set of T; element: T);
///Удаляет элемент element из множества s
procedure Exclude(var s: set of T; element: T);
//>>     Подпрограммы для работы с символами # Subroutines for char
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
//>>     Подпрограммы для работы со строками # Subroutines for string
/// Преобразует целое значение i к строковому представлению и записывает результат в s
procedure Str(i: целое; var s: string);
/// Преобразует вещественное значение r к строковому представлению и записывает результат в s
procedure Str(r: real; var s: string);
/// Преобразует вещественное значение r к строковому представлению и записывает результат в s
procedure Str(r: single; var s: string);
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
/// Вставляет подстроку source в строку s с позиции index
procedure Insert(source: string; var s: string; index: integer);
/// Удаляет из строки s count символов с позиции index
procedure Delete(var s: string; index, count: integer);
/// Возвращает подстроку строки s длины count с позиции index
function Copy(s: string; index, count: integer): string;
/// Возвращает строку, являющуюся результатом слияния строк s1,s2,...
function Concat(s1,s2,...): string;
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
/// Преобразует строковое представление s целого числа к числовому значению и записывает его в value.При невозможности преобразования возвращается False
function TryStrToInt(s: string; var value: integer): boolean;
/// Преобразует строковое представление s целого числа к числовому значению и записывает его в value.При невозможности преобразования возвращается False
function TryStrToInt64(s: string; var value: int64): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value.При невозможности преобразования возвращается False
function TryStrToFloat(s: string; var value: real): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value.При невозможности преобразования возвращается False
function TryStrToFloat(s: string; var value: single): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value.При невозможности преобразования возвращается False
function TryStrToReal(s: string; var value: real): boolean;
/// Преобразует строковое представление s вещественного числа к числовому значению и записывает его в value.При невозможности преобразования возвращается False
function TryStrToSingle(s: string; var value: single): boolean;
/// Считывает целое из строки начиная с позиции from и устанавливает from за считанным значением
function ReadIntegerFromString(s: string; var from: integer): integer;
/// Считывает вещественное из строки начиная с позиции from и устанавливает from за считанным значением
function ReadRealFromString(s: string; var from: integer): real;
/// Считывает из строки последовательность символов до пробельного символа начиная с позиции from и устанавливает from за считанным значением
function ReadWordFromString(s: string; var from: integer): string;
/// Возвращает True если достигнут конец строки или в строке остались только пробельные символы и False в противном случае
function StringIsEmpty(s: string; var from: integer): boolean;
/// Считывает целое из строки начиная с позиции from и устанавливает from за считанным значением.Возвращает True если считывание удачно и False в противном случае
function TryReadIntegerFromString(s: string; var from: integer; var res: integer): boolean;
/// Считывает вещественное из строки начиная с позиции from и устанавливает from за считанным значением.Возвращает True если считывание удачно и False в противном случае
function TryReadRealFromString(s: string; var from: integer; var res: real): boolean;
/// Преобразует строковое представление s целого числа к числовому значению и записывает его в переменную value.Если преобразование успешно, то err=0, иначе err>0
procedure Val(s: string; var value: число; var err: integer);
/// Преобразует целое число к строковому представлению
function IntToStr(a: integer): string;
/// Преобразует целое число к строковому представлению
function IntToStr(a: int64): string;
/// Преобразует вещественное число к строковому представлению
function FloatToStr(a: real): string;
/// Возвращает отформатированную строку, построенную по форматной строке и списку форматируемых параметров
function Format(formatstring: string; params pars: array of object): string;
//>>     Общие подпрограммы # Common subroutines
/// Увеличивает значение переменной i на 1
procedure Inc(var i: integer);
/// Увеличивает значение переменной i на n
procedure Inc(var i: integer; n: integer);
/// Уменьшает значение переменной i на 1
procedure Dec(var i: integer);
/// Уменьшает значение переменной i на n
procedure Dec(var i: integer; n: integer);
/// Увеличивает значение перечислимого типа на 1
procedure Inc(var e: перечислимый тип);
/// Увеличивает значение перечислимого типа на n
procedure Inc(var e: перечислимый тип; n: integer);
/// Уменьшает значение перечислимого типа на 1
procedure Dec(var e: перечислимый тип);
/// Уменьшает значение перечислимого типа на n
procedure Dec(var e: перечислимый тип; n: integer);
/// Возвращает порядковый номер значения a
function Ord(a: целое): целое;
/// Возвращает порядковый номер значения a
function Ord(a: перечислимый тип): integer;
/// Возвращает следующее за x значение
function Succ(x: целое): целое;
/// Возвращает следующее за x значение
function Succ(x: перечислимый тип): перечислимый тип;
/// Возвращает предшествующее x значение
function Pred(x: целое): целое;
/// Возвращает предшествующее x значение
function Pred(x: перечислимый тип): перечислимый тип;
/// Меняет местами значения двух переменных
procedure Swap<T>(var a, b: T);
/// Возвращает True, если достигнут конец строки
function Eoln: boolean;
/// Возвращает True, если достигнут конец потока ввода
function Eof: boolean;
//>>     Подпрограммы для работы с динамическими массивами # Subroutines for array of T
/// Возвращает 0
function Low(a: array of T): integer;
/// Возвращает верхнюю границу динамического массива
function High(a: array of T): integer;
/// Возвращает длину динамического массива
function Length(a: array of T): integer;
/// Возвращает длину динамического массива по размерности dim
function Length(a: array of T; dim: integer): integer;
/// Устанавливает длину одномерного динамического массива. Старое содержимое сохраняется
procedure SetLength(var a: array of T);
/// Устанавливает размеры n-мерного динамического массива. Старое содержимое сохраняется
procedure SetLength(var a: array of T; n1,n2,...: integer);
/// Создаёт копию динамического массива
function Copy(a: array of T): array of T;
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
//>>     Подпрограммы для генерации последовательностей # Subroutines for sequence generation
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
/// Возвращает последовательность из count элементов, начинающуюся с first и second,с функцией next перехода от двух предыдущих к следующему
function SeqGen<T>(count: integer; first, second: T; next: (T,T) ->T): sequence of T;
/// Возвращает последовательность элементов с начальным значением first,функцией next перехода от предыдущего к следующему и условием pred продолжения последовательности
function SeqWhile<T>(first: T; next: T->T; pred: T->boolean): sequence of T;
/// Возвращает последовательность элементов, начинающуюся с first и second,с функцией next перехода от двух предыдущих к следующему и условием pred продолжения последовательности
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
//>>     Подпрограммы для генерации динамических массивов # Subroutines for array of T generation
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
//>>     Подпрограммы для матриц # Subroutines for matrixes
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
//>>     Подпрограммы для создания кортежей # Subroutines for tuple generation
/// Возвращает кортеж из элементов разных типов
function Rec(x1: T1, x2: T2,...): (T1,T2,...);
//>>     Короткие функции Lst, LLst, HSet, SSet, Dict, KV # Short functions Lst, HSet, SSet, Dict, KV
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
//>>     Генерация бесконечных последовательностей # Infinite sequences
/// Возвращает бесконечную последовательность целых от текущего значения с шагом 1
function Step(Self: integer): sequence of integer; extensionmethod;
/// Возвращает бесконечную последовательность целых от текущего значения с шагом step
function Step(Self: integer; step: integer): sequence of integer; extensionmethod;
/// Возвращает бесконечную последовательность вещественных от текущего значения с шагом step
function Step(Self: real; step: real): sequence of real; extensionmethod;
/// Повторяет последовательность бесконечное число раз
function Cycle<T>(Self: sequence of T): sequence of T; extensionmethod;
//>>     Фиктивная секция XXX - не удалять! # XXX
AdjGroupClass<T> = class
private
cur: T;
enm: IEnumerator<T>;
fin: boolean;
public
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
//>>     Метод расширения Print для элементарных типов # Print for elementary types
function Print(Self: integer): integer; extensionmethod;
function Print(Self: real): real; extensionmethod;
function Print(Self: char): char; extensionmethod;
function Print(Self: boolean): boolean; extensionmethod;
function Print(Self: BigInteger): BigInteger; extensionmethod;
function Println(Self: integer): integer; extensionmethod;
function Println(Self: real): real; extensionmethod;
function Println(Self: char): char; extensionmethod;
function Println(Self: boolean): boolean; extensionmethod;
function Println(Self: BigInteger): BigInteger; extensionmethod;
//>>     Методы расширения для sequence of T # Extension methods for sequence of T
/// Выводит последовательность на экран, используя delim в качестве разделителя
function Print<T>(Self: sequence of T; delim: string): sequence of T; extensionmethod;
/// Выводит последовательность на экран, используя пробел в качестве разделителя
function Print<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Выводит последовательность на экран, используя delim в качестве разделителя, и переходит на новую строку
function Println<T>(Self: sequence of T; delim: string): sequence of T; extensionmethod;
/// Выводит последовательность на экран, используя пробел качестве разделителя, и переходит на новую строку
function Println<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Выводит последовательность строк в файл
function WriteLines(Self: sequence of string; fname: string): sequence of string; extensionmethod;
/// Выводит последовательность, каждый элемент выводится на новой строке
function PrintLines<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Выводит последовательность, каждый элемент отображается с помощью функции map и выводится на новой строке
function PrintLines<T,T1>(Self: sequence of T; map: T->T1): sequence of T; extensionmethod;
/// Преобразует элементы последовательности в строковое представление, после чего объединяет их в строку, используя delim в качестве разделителя
function JoinIntoString<T>(Self: sequence of T; delim: string): string; extensionmethod;
/// Преобразует элементы последовательности в строковое представление, после чего объединяет их в строку, используя пробел в качестве разделителя
function JoinIntoString<T>(Self: sequence of T): string; extensionmethod;
/// Применяет действие к каждому элементу последовательности
procedure ForEach<T>(Self: sequence of T; action: T -> ()); extensionmethod;
/// Применяет действие к каждому элементу последовательности, зависящее от номера элемента
procedure ForEach<T>(Self: sequence of T; action: (T,integer) -> ()); extensionmethod;
/// Возвращает отсортированную по возрастанию последовательность
function Sorted<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Возвращает отсортированную по убыванию последовательность
function SortedDescending<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Возвращает отсортированную по возрастанию последовательность
function Order<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Возвращает отсортированную по убыванию последовательность
function OrderDescending<T>(Self: sequence of T): sequence of T; extensionmethod;
/// Возвращает множество HashSet по данной последовательности
function ToHashSet<T>(Self: sequence of T): HashSet<T>; extensionmethod;
/// Возвращает множество SortedSet по данной последовательности
function ToSortedSet<T>(Self: sequence of T): SortedSet<T>; extensionmethod;
/// Возвращает LinkedList по данной последовательности
function ToLinkedList<T>(Self: sequence of T): LinkedList<T>; extensionmethod;
/// Возвращает первый элемент последовательности с минимальным значением ключа
function MinBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
/// Возвращает первый элемент последовательности с максимальным значением ключа
function MaxBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
/// Возвращает последний элемент последовательности с минимальным значением ключа
function LastMinBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
/// Возвращает последний элемент последовательности с максимальным значением ключа
function LastMaxBy<T, TKey>(Self: sequence of T; selector: T->TKey): T; extensionmethod;
/// Возвращает последние count элементов последовательности
function TakeLast<T>(Self: sequence of T; count: integer): sequence of T; extensionmethod;
/// Возвращает последовательность без последних count элементов
function SkipLast<T>(self: sequence of T; count: integer := 1): sequence of T; extensionmethod;
/// Декартово произведение последовательностей
function Cartesian<T, T1>(Self: sequence of T; b: sequence of T1): sequence of (T, T1); extensionmethod;
/// Декартово произведение последовательностей
function Cartesian<T, T1, T2>(Self: sequence of T; b: sequence of T1; func: (T,T1)->T2): sequence of T2; extensionmethod;
/// Разбивает последовательности на две в позиции ind
function SplitAt<T>(Self: sequence of T; ind: integer): (sequence of T, sequence of T); extensionmethod;
/// Разделяет последовательности на две по заданному условию
function Partition<T>(Self: sequence of T; cond: T->boolean): (sequence of T, sequence of T); extensionmethod;
/// Разделяет последовательности на две по заданному условию, в котором участвует индекс
function Partition<T>(Self: sequence of T; cond: (T,integer)->boolean): (sequence of T, sequence of T); extensionmethod;
/// Объединяет две последовательности в последовательность двухэлементных кортежей
function ZipTuple<T, T1>(Self: sequence of T; a: sequence of T1): sequence of (T, T1); extensionmethod;
/// Объединяет три последовательности в последовательность трехэлементных кортежей
function ZipTuple<T, T1, T2>(Self: sequence of T; a: sequence of T1; b: sequence of T2): sequence of (T, T1, T2); extensionmethod;
/// Объединяет четыре последовательности в последовательность четырехэлементных кортежей
function ZipTuple<T, T1, T2, T3>(Self: sequence of T; a: sequence of T1; b: sequence of T2; c: sequence of T3): sequence of (T, T1, T2, T3); extensionmethod;
/// Разъединяет последовательность двухэлементных кортежей на две последовательности
function UnZipTuple<T, T1>(Self: sequence of (T, T1)): (sequence of T, sequence of T1); extensionmethod;
/// Разъединяет последовательность трехэлементных кортежей на три последовательности
function UnZipTuple<T, T1, T2>(Self: sequence of (T, T1, T2)): (sequence of T, sequence of T1, sequence of T2); extensionmethod;
/// Разъединяет последовательность четырехэлементных кортежей на четыре последовательности
function UnZipTuple<T, T1, T2, T3>(Self: sequence of (T, T1, T2, T3)): (sequence of T, sequence of T1, sequence of T2, sequence of T3); extensionmethod;
/// Чередует элементы двух последовательностей
function Interleave<T>(Self: sequence of T; a: sequence of T): sequence of T; extensionmethod;
/// Чередует элементы трех последовательностей
function Interleave<T>(Self: sequence of T; a, b: sequence of T): sequence of T; extensionmethod;
/// Чередует элементы четырех последовательностей
function Interleave<T>(Self: sequence of T; a, b, c: sequence of T): sequence of T; extensionmethod;
/// Нумерует последовательность с единицы
function Numerate<T>(Self: sequence of T): sequence of (integer, T); extensionmethod;
/// Нумерует последовательность с номера from
function Numerate<T>(Self: sequence of T; from: integer): sequence of (integer, T); extensionmethod;
/// Табулирует функцию последовательностью
function Tabulate<T, T1>(Self: sequence of T; F: T->T1): sequence of (T, T1); extensionmethod;
/// Превращает последовательность в последовательность пар соседних элементов
function Pairwise<T>(Self: sequence of T): sequence of (T, T); extensionmethod;
/// Превращает последовательность в последовательность пар соседних элементов, применяет func к каждой паре полученных элементов и получает новую последовательность
function Pairwise<T, Res>(Self: sequence of T; func: (T,T)->Res): sequence of Res; extensionmethod;
/// Разбивает последовательность на серии длины size
function Batch<T>(Self: sequence of T; size: integer): sequence of sequence of T; extensionmethod;
/// Разбивает последовательность на серии длины size и применяет проекцию к каждой серии
function Batch<T, Res>(Self: sequence of T; size: integer; proj: Func<IEnumerable<T>, Res>): sequence of Res; extensionmethod;
/// Возвращает срез последовательности от номера from с шагом step > 0
function Slice<T>(Self: sequence of T; from, step: integer): sequence of T; extensionmethod;
/// Возвращает срез последовательности от номера from с шагом step > 0 длины не более count
function Slice<T>(Self: sequence of T; from, step, count: integer): sequence of T; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: sequence of integer): sequence of integer; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: array of integer): sequence of integer; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: List<integer>): sequence of integer; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: LinkedList<integer>): sequence of integer; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: sequence of real): sequence of real; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: array of real): sequence of real; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: List<real>): sequence of real; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности
function Incremental(Self: LinkedList<real>): sequence of real; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности. В качестве функции разности используется func
function Incremental<T, T1>(Self: sequence of T; func: (T,T)->T1): sequence of T1; extensionmethod;
/// Возвращает последовательность разностей соседних элементов исходной последовательности. В качестве функции разности используется func
function Incremental<T, T1>(Self: sequence of T; func: (T,T,integer)->T1): sequence of T1; extensionmethod;
/// Группирует одинаковые подряд идущие элементы, получая последовательность массивов
function AdjacentGroup<T>(Self: sequence of T): sequence of array of T; extensionmethod;
//>>     Методы расширения типа List<T> # Extension methods for List T
/// Перемешивает элементы списка случайным образом
function Shuffle<T>(Self: List<T>): List<T>; extensionmethod;
/// Находит первую пару подряд идущих одинаковых элементов и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: IList<T>; start: integer := 0): integer; extensionmethod;
/// Находит первую пару подряд идущих одинаковых элементов, используя функцию сравнения eq, и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: IList<T>; eq: (T,T)->boolean; start: integer := 0): integer; extensionmethod;
/// Возвращает индекс первого минимального элемента начиная с позиции index
function IndexMin<T>(Self: IList<T>; index: integer := 0): integer; extensionmethod;where T: IComparable<T>;
/// Возвращает индекс первого максимального элемента начиная с позиции index
function IndexMax<T>(self: IList<T>; index: integer := 0): integer; extensionmethod;where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента
function LastIndexMin<T>(Self: IList<T>): integer; extensionmethod;where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента в диапазоне [0,index-1]
function LastIndexMin<T>(Self: IList<T>; index: integer): integer; extensionmethod;where T: System.IComparable<T>;
/// Возвращает индекс последнего максимального элемента
function LastIndexMax<T>(Self: IList<T>): integer; extensionmethod;where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента в диапазоне [0,index-1]
function LastIndexMax<T>(Self: IList<T>; index: integer): integer; extensionmethod;where T: System.IComparable<T>;
/// Заменяет в массиве или списке все вхождения одного значения на другое
procedure Replace<T>(Self: IList<T>; oldValue, newValue: T); extensionmethod;
/// Преобразует элементы массива или списка по заданному правилу
procedure Transform<T>(Self: IList<T>; f: T->T); extensionmethod;
/// Заполняет элементы массива или списка значениями, вычисляемыми по некоторому правилу
procedure Fill<T>(Self: IList<T>; f: integer->T); extensionmethod;
/// Возвращает срез списка от индекса from с шагом step
function Slice<T>(Self: List<T>; from, step: integer): List<T>; extensionmethod;
/// Возвращает срез списка от индекса from с шагом step длины не более count
function Slice<T>(Self: List<T>; from, step, count: integer): List<T>; extensionmethod;
/// Удаляет последний элемент. Если элементов нет, генерирует исключение
function RemoveLast<T>(Self: List<T>): List<T>; extensionmethod;
procedure CorrectFromTo(situation: integer; Len: integer; var from, to: integer; step: integer);
//>>     Методы расширения типа array [,] of T # Extension methods for array [,] of T
/// Количество строк в двумерном массиве
function RowCount<T>(Self: array [,] of T): integer; extensionmethod;
/// Количество столбцов в двумерном массиве
function ColCount<T>(Self: array [,] of T): integer; extensionmethod;
/// Вывод двумерного массива, w - ширина поля вывода
function Print<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod;
/// Вывод двумерного вещественного массива по формату :w:f
function Print(Self: array [,] of real; w: integer := 7; f: integer := 2): array [,] of real; extensionmethod;
/// Вывод двумерного массива и переход на следующую строку, w - ширина поля вывода
function Println<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod;
/// Вывод двумерного вещественного массива по формату :w:f и переход на следующую строку
function Println(Self: array [,] of real; w: integer := 7; f: integer := 2): array [,] of real; extensionmethod;
/// k-тая строка двумерного массива
function Row<T>(Self: array [,] of T; k: integer): array of T; extensionmethod;
/// k-тый столбец двумерного массива
function Col<T>(Self: array [,] of T; k: integer): array of T; extensionmethod;
/// k-тая строка двумерного массива как последовательность
function RowSeq<T>(Self: array [,] of T; k: integer): sequence of T; extensionmethod;
/// k-тый столбец двумерного массива как последовательность
function ColSeq<T>(Self: array [,] of T; k: integer): sequence of T; extensionmethod;
/// Возвращает последовательность строк двумерного массива
function Rows<T>(Self: array [,] of T): sequence of sequence of T; extensionmethod;
/// Возвращает последовательность столбцов двумерного массива
function Cols<T>(Self: array [,] of T): sequence of sequence of T; extensionmethod;
/// Меняет местами две строки двумерного массива с номерами k1 и k2
procedure SwapRows<T>(Self: array [,] of T; k1, k2: integer); extensionmethod;
/// Меняет местами два столбца двумерного массива с номерами k1 и k2
procedure SwapCols<T>(Self: array [,] of T; k1, k2: integer); extensionmethod;
/// Меняет строку k двумерного массива на другую строку
procedure SetRow<T>(Self: array [,] of T; k: integer; a: array of T); extensionmethod;
/// Меняет строку k двумерного массива на другую строку
procedure SetRow<T>(Self: array [,] of T; k: integer; a: sequence of T); extensionmethod := Self.SetRow(k,a.ToArray);
/// Меняет столбец k двумерного массива на другой столбец
procedure SetCol<T>(Self: array [,] of T; k: integer; a: array of T); extensionmethod;
/// Меняет столбец k двумерного массива на другой столбец
procedure SetCol<T>(Self: array [,] of T; k: integer; a: sequence of T); extensionmethod := Self.SetCol(k,a.ToArray);
/// Возвращает по заданному двумерному массиву последовательность (a[i,j],i,j)
function ElementsWithIndexes<T>(Self: array [,] of T): sequence of (T, integer, integer); extensionmethod;
/// Возвращает по заданному двумерному массиву последовательность его элементов по строкам
function ElementsByRow<T>(Self: array [,] of T): sequence of T; extensionmethod;
/// Возвращает по заданному двумерному массиву последовательность его элементов по столбцам
function ElementsByCol<T>(Self: array [,] of T): sequence of T; extensionmethod;
/// Преобразует элементы двумерного массива и возвращает преобразованный массив
function ConvertAll<T, T1>(Self: array [,] of T; converter: T->T1): array [,] of T1; extensionmethod;
/// Преобразует элементы двумерного массива по заданному правилу
procedure Transform<T>(Self: array [,] of T; f: T->T); extensionmethod;
/// Заполняет элементы двумерного массива значениями, вычисляемыми по некоторому правилу
procedure Fill<T>(Self: array [,] of T; f: (integer,integer) ->T); extensionmethod;
//>>     Фиктивная секция YYY - не удалять! # YYY
function Matr<T>(m,n: integer; params data: array of T): array [,] of T;
function MatrRandom(m: integer; n: integer; a, b: integer): array [,] of integer;
function MatrRandomInteger(m: integer; n: integer; a, b: integer): array [,] of integer;
function MatrRandomReal(m: integer; n: integer; a, b: real): array [,] of real;
function MatrFill<T>(m, n: integer; x: T): array [,] of T;
function MatrGen<T>(m, n: integer; gen: (integer,integer)->T): array [,] of T;
function Transpose<T>(a: array [,] of T): array [,] of T;
function ReadMatrInteger(m, n: integer): array [,] of integer;
function ReadMatrReal(m, n: integer): array [,] of real;
//>>     Методы расширения типа array of T # Extension methods for array of T
/// Перемешивает элементы массива случайным образом
function Shuffle<T>(Self: array of T): array of T; extensionmethod;
{/// Находит первую пару подряд идущих одинаковых элементов и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: array of T; start: integer := 0): integer; extensionmethod;
/// Находит первую пару подряд идущих одинаковых элементов, используя функцию сравнения eq, и возвращает индекс первого элемента пары. Если не найден, возвращается -1
function AdjacentFind<T>(Self: array of T; eq: (T,T)->boolean; start: integer := 0): integer; extensionmethod;
/// Возвращает минимальный элемент
function Min<T>(Self: array of T): T; extensionmethod;where T: System.IComparable<T>;
/// Возвращает максимальный элемент
function Max<T>(Self: array of T): T; extensionmethod;where T: System.IComparable<T>;
/// Возвращает минимальный элемент
function Min(Self: array of integer): integer; extensionmethod;
/// Возвращает минимальный элемент
function Min(Self: array of real): real; extensionmethod;
/// Возвращает максимальный элемент
function Max(Self: array of integer): integer; extensionmethod;
/// Возвращает максимальный элемент
function Max(Self: array of real): real; extensionmethod;
{/// Возвращает индекс первого минимального элемента начиная с позиции index
function IndexMin<T>(Self: array of T; index: integer := 0): integer; extensionmethod; where T: System.IComparable<T>;
/// Возвращает индекс первого максимального элемента начиная с позиции index
function IndexMax<T>(self: array of T; index: integer := 0): integer; extensionmethod; where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента
function LastIndexMin<T>(Self: array of T): integer; extensionmethod; where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента в диапазоне [0,index]
function LastIndexMin<T>(Self: array of T; index: integer): integer; extensionmethod; where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента
function LastIndexMax<T>(Self: array of T): integer; extensionmethod; where T: System.IComparable<T>;
/// Возвращает индекс последнего минимального элемента в диапазоне [0,index]
function LastIndexMax<T>(Self: array of T; index: integer): integer; extensionmethod; where T: System.IComparable<T>;
/// Заменяет в массиве все вхождения одного значения на другое
procedure Replace<T>(Self: array of T; oldValue,newValue: T); extensionmethod;
/// Преобразует элементы массива по заданному правилу
procedure Transform<T>(self: array of T; f: T -> T); extensionmethod;
/// Заполняет элементы массива значениями, вычисляемыми по некоторому правилу
procedure Fill<T>(Self: array of T; f: integer -> T); extensionmethod;
/// Выполняет бинарный поиск в отсортированном массиве
function BinarySearch<T>(self: array of T; x: T): integer; extensionmethod;
/// Преобразует элементы массива и возвращает преобразованный массив
function ConvertAll<T, T1>(self: array of T; converter: T->T1): array of T1; extensionmethod;
/// Выполняет поиск первого элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function Find<T>(self: array of T; p: T->boolean): T; extensionmethod;
/// Выполняет поиск индекса первого элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается -1
function FindIndex<T>(self: array of T; p: T->boolean): integer; extensionmethod;
/// Выполняет поиск индекса первого элемента в массиве, удовлетворяющего предикату, начиная с индекса start. Если не найден, возвращается -1
function FindIndex<T>(self: array of T; start: integer; p: T->boolean): integer; extensionmethod;
/// Возвращает в виде массива все элементы, удовлетворяющие предикату
function FindAll<T>(self: array of T; p: T->boolean): array of T; extensionmethod;
/// Выполняет поиск последнего элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function FindLast<T>(self: array of T; p: T->boolean): T; extensionmethod;
/// Выполняет поиск индекса последнего элемента в массиве, удовлетворяющего предикату. Если не найден, возвращается нулевое значение соответствующего типа
function FindLastIndex<T>(self: array of T; p: T->boolean): integer; extensionmethod;
/// Выполняет поиск индекса последнего элемента в массиве, удовлетворяющего предикату, начиная с индекса start. Если не найден, возвращается нулевое значение соответствующего типа
function FindLastIndex<T>(self: array of T; start: integer; p: T->boolean): integer; extensionmethod;
/// Возвращает индекс первого вхождения элемента или -1 если элемент не найден
function IndexOf<T>(self: array of T; x: T): integer; extensionmethod;
/// Возвращает индекс первого вхождения элемента начиная с индекса start или -1 если элемент не найден
function IndexOf<T>(self: array of T; x: T; start: integer): integer; extensionmethod;
/// Возвращает индекс последнего вхождения элемента или -1 если элемент не найден
function LastIndexOf<T>(self: array of T; x: T): integer; extensionmethod;
/// Возвращает индекс последнего вхождения элемента начиная с индекса start или -1 если элемент не найден
function LastIndexOf<T>(self: array of T; x: T; start: integer): integer; extensionmethod;
/// Сортирует массив по возрастанию
procedure Sort<T>(self: array of T); extensionmethod;
/// Сортирует массив по возрастанию, используя cmp в качестве функции сравнения элементов
procedure Sort<T>(self: array of T; cmp: (T,T) ->integer); extensionmethod;
/// Возвращает индекс последнего элемента массива
function High(self: System.Array); extensionmethod := High(Self);
/// Возвращает индекс первого элемента массива
function Low(self: System.Array); extensionmethod := Low(Self);
/// Возвращает последовательность индексов одномерного массива
function Indexes<T>(Self: array of T): sequence of integer; extensionmethod := Range(0, Self.Length - 1);
/// Возвращает последовательность индексов элементов одномерного массива, удовлетворяющих условию
function IndexesOf<T>(Self: array of T; cond: T->boolean): sequence of integer; extensionmethod;
/// Возвращает последовательность индексов элементов одномерного массива, удовлетворяющих условию
function IndexesOf<T>(Self: array of T; cond: (T,integer) ->boolean): sequence of integer; extensionmethod;
/// Возвращает срез массива от индекса from с шагом step
function Slice<T>(Self: array of T; from, step: integer): array of T; extensionmethod;
/// Возвращает срез массива от индекса from с шагом step длины не более count
function Slice<T>(Self: array of T; from, step, count: integer): array of T; extensionmethod;
//>>     Методы расширения типа integer # Extension methods for integer
/// Возвращает квадратный корень числа
function Sqrt(Self: integer): real; extensionmethod;
/// Возвращает квадрат числа
function Sqr(Self: integer): integer; extensionmethod;
/// Возвращает True если значение находится между двумя другими
function Between(Self: integer; a, b: integer): boolean; extensionmethod;
/// Возвращает True если значение находится между двумя другими
function InRange(Self: integer; a,b: integer): boolean; extensionmethod;
/// Возвращает, является ли целое четным
function IsEven(Self: integer): boolean; extensionmethod;
/// Возвращает, является ли целое нечетным
function IsOdd(Self: integer): boolean; extensionmethod;
/// Возвращает последовательность чисел от 1 до данного
function Range(Self: integer): sequence of integer; extensionmethod;
/// Генерирует последовательность целых от текущего значения до n
function To(Self: integer; n: integer): sequence of integer; extensionmethod;
/// Генерирует последовательность целых от текущего значения до n в убывающем порядке
function Downto(Self: integer; n: integer): sequence of integer; extensionmethod;
/// Возвращает последовательность целых 0,1,...n-1
function Times(Self: integer): sequence of integer; extensionmethod;
//>>     Методы расширения типа BigInteger # Extension methods for BigInteger
/// Возвращает квадратный корень числа
function Sqrt(Self: BigInteger): real; extensionmethod;
//>>     Методы расширения типа real # Extension methods for real
/// Возвращает True если значение находится между двумя другими
function Between(Self: real; a, b: real): boolean; extensionmethod;
/// Возвращает True если значение находится между двумя другими
function InRange(Self: real; a,b: real): boolean; extensionmethod;
/// Возвращает квадратный корень числа
function Sqrt(Self: real): real; extensionmethod;
/// Возвращает квадрат числа
function Sqr(Self: real): real; extensionmethod;
/// Возвращает число, округленное до ближайшего целого
function Round(Self: real): integer; extensionmethod;
/// Возвращает x, округленное до ближайшего вещественного с digits знаками после десятичной точки
function Round(Self: real; digits: integer): real; extensionmethod;
/// Возвращает число, округленное до ближайшего длинного целого
function RoundBigInteger(Self: real): BigInteger; extensionmethod;
/// Возвращает целую часть вещественного числа
function Trunc(Self: real): integer; extensionmethod;
/// Возвращает целую часть вещественного числа как длинное целое
function TruncBigInteger(Self: real): BigInteger; extensionmethod;
/// Возвращает вещественное, отформатированное к строке с frac цифрами после десятичной точки
function ToString(Self: real; frac: integer): string; extensionmethod;
//>>     Методы расширения типа char # Extension methods for char
/// Возвращает True если значение находится между двумя другими
function Between(Self: char; a, b: char): boolean; extensionmethod;
/// Возвращает True если значение находится между двумя другими
function InRange(Self: char; a,b: char): boolean; extensionmethod;
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
/// Принадлежит ли символ к категории букв нижнего регистра
function IsLower(Self: char): boolean; extensionmethod;
/// Принадлежит ли символ к категории букв верхнего регистра
function IsUpper(Self: char): boolean; extensionmethod;
/// Преобразует символ в цифру
function ToDigit(Self: char): integer; extensionmethod;
/// Преобразует символ в нижний регистр
function ToLower(Self: char): char; extensionmethod;
/// Преобразует символ в верхний регистр
function ToUpper(Self: char): char; extensionmethod;
//>>     Методы расширения типа string # Extension methods for string
/// Возвращает True если значение находится между двумя другими
function Between(Self: string; a, b: string): boolean; extensionmethod;
/// Возвращает True если значение находится между двумя другими
function InRange(Self: string; a,b: string): boolean; extensionmethod;
/// Считывает целое из строки начиная с позиции from и устанавливает from за считанным значением
function ReadInteger(Self: string; var from: integer): integer; extensionmethod;
/// Считывает вещественное из строки начиная с позиции from и устанавливает from за считанным значением
function ReadReal(Self: string; var from: integer): real; extensionmethod;
/// Считывает слово из строки начиная с позиции from и устанавливает from за считанным значением
function ReadWord(Self: string; var from: integer): string; extensionmethod;
/// Преобразует строку в целое
function ToInteger(Self: string): integer; extensionmethod;
/// Преобразует строку в BigInteger
function ToBigInteger(Self: string): BigInteger; extensionmethod;
/// Преобразует строку в вещественное
function ToReal(Self: string): real; extensionmethod;
/// Преобразует строку в массив слов
function ToWords(Self: string; params delim: array of char): array of string; extensionmethod;
/// Преобразует строку в массив целых
function ToIntegers(Self: string): array of integer; extensionmethod;
/// Преобразует строку в массив вещественных
function ToReals(Self: string): array of real; extensionmethod;
/// Возвращает инверсию строки
function Inverse(Self: string): string; extensionmethod;
/// Заменяет в указанной строке все вхождения регулярного выражения указанной строкой замены и возвращает преобразованную строку
function RegexReplace(Self: string; reg, repl: string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
/// Заменяет в указанной строке все вхождения регулярного выражения указанным преобразованием замены и возвращает преобразованную строку
function RegexReplace(Self: string; reg: string; repl: Match->string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
/// Ищет в указанной строке все вхождения регулярного выражения и возвращает их в виде последовательности элементов типа Match
function Matches(Self: string; reg: string; options: RegexOptions := RegexOptions.None): sequence of Match; extensionmethod;
/// Ищет в указанной строке первое вхождение регулярного выражения и возвращает его в виде строки
function MatchValue(Self: string; reg: string; options: RegexOptions := RegexOptions.None): string; extensionmethod;
/// Ищет в указанной строке все вхождения регулярного выражения и возвращает их в виде последовательности строк
function MatchValues(Self: string; reg: string; options: RegexOptions := RegexOptions.None): sequence of string; extensionmethod;
/// Удовлетворяет ли строка регулярному выражению
function IsMatch(Self: string; reg: string; options: RegexOptions := RegexOptions.None): boolean; extensionmethod := Regex.IsMatch(Self, reg, options);
/// Удаляет в строке все вхождения указанных строк
function Remove(Self: string; params targets: array of string): string; extensionmethod;
/// Возвращает подстроку, полученную вырезанием из строки length самых правых символов
function Right(Self: string; length: integer): string; extensionmethod;
/// Возвращает подстроку, полученную вырезанием из строки length самых левых символов
function Left(Self: string; length: integer): string; extensionmethod;
/// Возвращает срез строки от индекса from с шагом step
function Slice(Self: string; from, step: integer): string; extensionmethod;
/// Возвращает срез строки от индекса from с шагом step длины не более count
function Slice(Self: string; from, step, count: integer): string; extensionmethod;
//>>     Методы расширения типа Func # Extension methods for Func
/// Суперпозиция функций
function Compose<T1, T2, TResult>(Self: T2->TResult; composer: T1->T2): T1->TResult; extensionmethod;
//>>     Методы расширения типа Complex # Extension methods for Complex
/// Возвращает комплексно сопряженное значение
function Conjugate(Self: Complex): Complex; extensionmethod;
//>>     Методы расширения IDictionary # Extension methods for IDictionary
/// Возвращает в словаре значение, связанное с указанным ключом, а если такого ключа нет, то значение по умолчанию
function Get<Key, Value>(Self: IDictionary<Key, Value>; K: Key): Value; extensionmethod;
//>>     Подпрограммы для работы с типизированными файлами # Subroutines for typed files
/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenBinary<T>(fname: string): file of T;
/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateBinary<T>(fname: string): file of T;
/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string): file of T;
/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string): file of T;
/// Открывает типизированный файл целых и возвращает значение для инициализации файловой переменной
function OpenFileInteger(fname: string): file of integer;
/// Открывает типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function OpenFileReal(fname: string): file of real;
/// Создаёт или обнуляет типизированный файл целых и возвращает значение для инициализации файловой переменной
function CreateFileInteger(fname: string): file of integer;
/// Создаёт или обнуляет типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function CreateFileReal(fname: string): file of real;
/// Открывает типизированный файл, возвращает последовательность его элементов и закрывает его
procedure WriteElements<T>(fname: string; ss: sequence of T);
//>>     Методы расширения типизированных файлов # Extension methods for typed files
/// Устанавливает текущую позицию файлового указателя в типизированном файле на элемент с номером n
function Seek<T>(Self: file of T; n: int64): file of T; extensionmethod;
/// Считывает и возвращает следующий элемент типизированного файла
function Read<T>(Self: file of T): T; extensionmethod;
/// Считывает и возвращает два следующих элемента типизированного файла в виде кортежа
function Read2<T>(Self: file of T): (T,T); extensionmethod;
/// Считывает и возвращает три следующих элемента типизированного файла в виде кортежа
function Read3<T>(Self: file of T): (T,T,T); extensionmethod;
/// Возвращает последовательность элементов открытого типизированного файла от текущего элемента до конечного
function ReadElements<T>(Self: file of T): sequence of T; extensionmethod;
/// Открывает типизированный файл, возвращает последовательность его элементов и закрывает его
function ReadElements<T>(fname: string): sequence of T;
/// Записывает данные в типизированный файл
procedure Write<T>(Self: file of T; params vals: array of T); extensionmethod;
