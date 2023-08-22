unit SF;

type
  int = integer;
  bool = boolean;
  bi = BigInteger;
  FunI = () -> int;
  FunR = () -> real;
  FunS = () -> string;
  FunII = int -> int;
  FunIR = int -> real;
  FunIS = int -> string;
  FunRI = real -> int;
  FunRR = real -> real;
  FunRS = real -> string;
  FunSI = string -> int;
  FunSR = string -> real;
  FunSS = string -> string;
  Pred = () -> bool;
  PredI = int -> bool;
  PredR = real -> bool;
  PredS = string -> bool;
  Act = procedure;
  ProcI = int -> ();
  ProcR = real -> ();
  ProcS = string -> ();
  HS<T> = HashSet<T>;

procedure Pr(params a: array of object) := Print(a);
procedure Pr(o: object) := Print(o);
procedure Pr(s: string) := Print(s);
procedure Prln(params a: array of object) := Println(a);

function RI := ReadInteger;
function RBI := ReadBigInteger;
function RI64 := ReadInt64;
function RR := ReadReal;
function RC := ReadChar;
function RS := ReadString;
function RL := ReadLexem;

function RlnI := ReadlnInteger;
function RlnBI := ReadlnBigInteger;
function RlnI64 := ReadlnInt64;
function RlnR := ReadlnReal;
function RlnC := ReadlnChar;
function RlnS := ReadlnString;

function RI2 := ReadInteger2;
function RR2 := ReadReal2;
function RC2 := ReadChar2;
function RS2 := ReadString2;

function RlnI2 := ReadlnInteger2;
function RlnR2 := ReadlnReal2;
function RlnC2 := ReadlnChar2;
function RlnS2 := ReadlnString2;

function RI3 := ReadInteger3;
function RR3 := ReadReal3;
function RC3 := ReadChar3;
function RS3 := ReadString3;

function RlnI3 := ReadlnInteger3;
function RlnR3 := ReadlnReal3;
function RlnC3 := ReadlnChar3;
function RlnS3 := ReadlnString3;

function RI4 := ReadInteger4;
function RR4 := ReadReal4;
function RC4 := ReadChar4;
function RS4 := ReadString4;

function RlnI4 := ReadlnInteger4;
function RlnR4 := ReadlnReal4;
function RlnC4 := ReadlnChar4;
function RlnS4 := ReadlnString4;

function RAI(n: integer) := ReadArrInteger(n);
function RAR(n: integer) := ReadArrReal(n);

function Pr(Self: integer): integer; extensionmethod := Self.Print;
function Pr(Self: real): real; extensionmethod := Self.Print;
function Pr(Self: Biginteger): Biginteger; extensionmethod := Self.Print;
function Pr(Self: char): char; extensionmethod := Self.Print;
function Pr(Self: boolean): boolean; extensionmethod := Self.Print;
function Pr(Self: string): string; extensionmethod := Self.Print;

function Pr<T>(Self: array [,] of T; w: integer := 4): array [,] of T; extensionmethod := Self.Print(w);

function Prln(Self: integer): integer; extensionmethod := Self.Println;
function Prln(Self: real): real; extensionmethod := Self.Println;
function Prln(Self: Biginteger): Biginteger; extensionmethod := Self.Println;
function Prln(Self: char): char; extensionmethod := Self.Println;
function Prln(Self: boolean): boolean; extensionmethod := Self.Println;
function Prln(Self: string): string; extensionmethod := Self.Println;


function D(Self,b: integer): boolean; extensionmethod := Self mod b = 0;
function ND(Self,b: integer): boolean; extensionmethod := Self mod b <> 0;

/// Записывает в min значение x если x<min
procedure ReMin(var min: integer; x: integer);
begin
  if x < min then 
    min := x
end;

/// Записывает в max значение x если x>max
procedure ReMax(var max: integer; x: integer);
begin
  if x > max then 
    max := x
end;

/// Записывает в min значение x если x<min
procedure ReMin(var min: real; x: real);
begin
  if x < min then 
    min := x
end;

/// Записывает в max значение x если x>max
procedure ReMax(var max: real; x: real);
begin
  if x > max then 
    max := x
end;

/// Возвращает все перестановки
function Prm<T>(Self: array of T): sequence of array of T; extensionmethod
  := Self.Permutations;

/// Возвращает все сочетания по m элементов
function Cmb<T>(Self: array of T; m: integer): sequence of array of T; extensionmethod 
  := Self.Combinations(m);

/// Преобразует объект в строку
function ToS(Self: Object): string; extensionmethod := Self.ToString;

/// Преобразует строку в массив слов
function ToW(Self: string): array of string; extensionmethod := Self.ToWords;

/// Возвращает элементы последовательности без повторяющихся
function Dst<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Distinct;

/// Преобразует последовательность в строку
function ToS<T>(Self: sequence of T; delim: string): string; extensionmethod := Self.JoinToString(delim);

/// Преобразует последовательность в строку
function JtS<T>(Self: sequence of T; delim: string): string; extensionmethod := Self.JoinToString(delim);

/// Преобразует последовательность в строку
function JtS<T>(Self: sequence of T): string; extensionmethod := Self.JoinToString();

/// Сливает последовательнсть строк в одну строку 
function Concat(Self: sequence of string): string; extensionmethod := Self.JoinToString('');

/// Преобразует строку в целое
function ToI(Self: string); extensionmethod := Self.ToInteger;

/// Преобразует строку в вещественное
function ToR(Self: string); extensionmethod := Self.ToReal;

/// Преобразует строку в массив целых
function ToIs(Self: string); extensionmethod := Self.ToIntegers;

/// Преобразует строку в массив вещественных
function ToRs(Self: string); extensionmethod := Self.ToReals;

function operator-(c,c1: char): integer; extensionmethod := Ord(c) - Ord(c1);

/// Возвращает длину строки
function Len(Self: string): integer; extensionmethod := Self.Length;

/// Возвращает длину массива
function Len<T>(Self: array of T): integer; extensionmethod := Self.Length;

/// Возвращает среднее элементов последовательности
function Av(Self: sequence of integer): real; extensionmethod := Self.Average;
/// Возвращает среднее элементов последовательности
function Av(Self: sequence of real): real; extensionmethod := Self.Average;

/// Возвращает количество элементов последовательности
function Cnt<T>(Self: sequence of T): integer; extensionmethod := Self.Count;
/// Возвращает количество элементов последовательности, удовлетворяющих условию
function Cnt<T>(Self: sequence of T; pred: T->boolean): integer; extensionmethod := Self.Count(pred);
/// Возвращает количество элементов последовательности, равных заданному значению
function Cnt<T>(Self: sequence of T; elem: T): integer; extensionmethod := Self.CountOf(elem);

function Ord<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Order;
function OrdD<T>(Self: sequence of T): sequence of T; extensionmethod := Self.OrderDescending;

/// Преобразует последовательность в строку
function Str<T>(Self: sequence of T): string; extensionmethod := Self.JoinToString;

/// Выводит последовательность, разделяя элементы пробелами
function Pr<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Print;
/// Выводит последовательность, разделяя элементы пробелами, и переходит на новую строку
function Prln<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Println;

/// Выводит последовательность, каждый элемент выводится на новой строке
function PL<T>(Self: sequence of T); extensionmethod := Self.PrintLines;
/// Выводит последовательность, каждый элемент отображается с помощью функции map и выводится на новой строке
function PL<T,T1>(Self: sequence of T; map: T->T1); extensionmethod := Self.PrintLines(map);

/// Возвращает инвертированную последовательность
function Rev<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Reverse;

/// Преобразует каждый элемент последовательности в последовательность и сливает полученные последовательности в одну
function SelM<T,T1>(Self: sequence of T; transform: T->sequence of T1): sequence of T1; extensionmethod := Self.SelectMany(transform);

/// Преобразует последовательность, используя функцию transform
function Sel<T,T1>(Self: sequence of T; transform: T->T1): sequence of T1; extensionmethod := Self.Select(transform);

/// Преобразует последовательность, используя функцию transform с индексом
function Sel<T,T1>(Self: sequence of T; transform: (T,int)->T1): sequence of T1; extensionmethod := Self.Select(transform);

/// Создаёт словарь в ссответствии с заданными функциями проекции на ключ и значение
function ToD<T,TKey,TValue>(Self: sequence of T; keySelector: T->TKey; valueSelector: T->TValue): Dictionary<TKey,TValue>; extensionmethod := 
  Self.ToDictionary(keySelector,valueSelector);

/// Создаёт словарь в ссответствии с заданной функцией проекции на ключ
function ToD<T,TKey>(Self: sequence of T; keySelector: T->TKey): Dictionary<TKey,T>; extensionmethod := 
  Self.ToDictionary(keySelector);

/// Применяет к каждому элементы последовательности агрегатную функцию. Последнее значение агрегатной функции является результатом
function Agr<T,TAcc>(Self: sequence of T; seed: TAcc; func: (TAcc,T)->TAcc): TAcc; extensionmethod 
  := Self.Aggregate(seed,func);

/// Фильтрует последовательность по условию cond
function Wh<T>(Self: sequence of T; cond: T->boolean): sequence of T; extensionmethod := Self.Where(cond);

/// Фильтрует последовательность по условию cond с индексом
function Wh<T>(Self: sequence of T; cond: (T,integer)->boolean); extensionmethod := Self.Where(cond);

/// Группирует элементы последовательности в соответствии с заданной функцией проекции на ключ группы
function GrBy<T,TKey>(Self: sequence of T; selector: T->TKey): sequence of System.Linq.IGrouping<TKey,T>; extensionmethod := Self.GroupBy(selector);


/// Преобразует последовательность в массив
function ToA<T>(Self: sequence of T): array of T; extensionmethod := Self.ToArray;

/// Преобразует последовательность в список
function ToL<T>(Self: sequence of T): List<T>; extensionmethod := Self.ToList;

/// Преобразует последовательность в HashSet
function ToHS<T>(Self: sequence of T): HashSet<T>; extensionmethod := Self.ToHashSet;
/// Преобразует последовательность в SortedSet
function ToSS<T>(Self: sequence of T): SortedSet<T>; extensionmethod := Self.ToSortedSet;


/// Инвертирует последовательность
procedure Rev<T>(a: array of T) := Reverse(a);

/// Возвращает индекс минимального элемента
function IMin<T>(Self: array of T): integer; extensionmethod; where T: IComparable<T>;
begin
  Result := Self.IndexMin;
end;

/// Возвращает индекс максимального элемента
function IMax<T>(Self: array of T): integer; extensionmethod; where T: IComparable<T>;
begin
  Result := Self.IndexMax;
end;

/// Возвращает True если массивы равны
function Eq<T>(Self,b: array of T): boolean; extensionmethod := Self.ArrEqual(b);

/// Возвращает в виде массива все элементы, удовлетворяющие cond
function Filt<T>(Self: array of T; cond: T->boolean): array of T; extensionmethod := Self.FindAll(cond);

/// Возвращает массив с элементами, преобразованными по правилу transform
function Tr<T>(Self: array of T; transform: T->T): array of T; extensionmethod := Self.ConvertAll(transform);

/// Преобразует массив по правилу transform
procedure Tr<T>(a: array of T; transform: T->T) := a.Transform(transform);

/// Возвращает индекс последнего элемента массива
function H<T>(Self: array of T): integer; extensionmethod := Self.High;

/// Возвращает все перестановки множества элементов, заданного массивом
function Prm<T>(Self: array of T): sequence of array of T; extensionmethod := Self.Permutations;

/// Возвращает все перестановки множества элементов, заданного массивом
function Prm<T>(Self: sequence of T): sequence of array of T; extensionmethod := Self.Permutations;

/// Возвращает все частичные перестановки из n элементов по m 
function Prm<T>(Self: array of T; m: integer): sequence of array of T; extensionmethod := Self.Permutations(m);

/// Возвращает все частичные перестановки из n элементов по m 
function Prm<T>(Self: sequence of T; m: integer): sequence of array of T; extensionmethod := Self.Permutations(m);

/// Возвращает n-тую декартову степень множества элементов, заданного массивом
function Cart<T>(Self: array of T; n: integer): sequence of array of T; extensionmethod := Self.Cartesian(n);

/// Возвращает n-тую декартову степень множества элементов, заданного массивом
function Cart<T>(Self: sequence of T; n: integer): sequence of array of T; extensionmethod := Self.Cartesian(n);

/// Возвращает n-тую декартову степень множества символов, заданного строкой
function Cart(Self: string; n: integer): sequence of string; extensionmethod := Self.CartesianPower(n);

/// Возвращает декартово произведение последовательностей в виде последовательности пар
function Cart<T, T1>(Self: sequence of T; b: sequence of T1): sequence of (T, T1); extensionmethod := Self.Cartesian(b);

/// Возвращает декартово произведение последовательностей, проектируя каждую пару на значение
function Cart<T, T1, T2>(Self: sequence of T; b: sequence of T1; func: (T,T1)->T2): sequence of T2; extensionmethod := Self.Cartesian(b,func);

/// Возвращает исходную последовательность или одноэлементную последовательность если исходная последовательность пуста
function DefIfE<T>(Self: sequence of T; def: T): sequence of T; extensionmethod := Self.DefaultIfEmpty(def);

/// Возвращает все сочетания по m элементов
function Cmb<T>(Self: array of T; m: integer): sequence of array of T; extensionmethod := Self.Combinations(m);

/// Возвращает все сочетания по m элементов
function Cmb<T>(Self: sequence of T; m: integer): sequence of array of T; extensionmethod := Self.Combinations(m);

/// Выводит кортеж
function Pr<T1,T2>(Self: (T1,T2)): (T1,T2); extensionmethod;
begin 
  Result := Self; 
  Self.Print; 
end;

/// Выводит кортеж
function Pr<T1,T2,T3>(Self: (T1,T2,T3)): (T1,T2,T3); extensionmethod;
begin 
  Result := Self; 
  Self.Print; 
end;

/// Выводит кортеж
function Pr<T1,T2,T3,T4>(Self: (T1,T2,T3,T4)): (T1,T2,T3,T4); extensionmethod;
begin 
  Result := Self; 
  Self.Print; 
end;

/// Выводит кортеж
function Pr<T1,T2,T3,T4,T5>(Self: (T1,T2,T3,T4,T5)): (T1,T2,T3,T4,T5); extensionmethod;
begin 
  Result := Self; 
  Self.Print; 
end;

/// Выводит кортеж
function Prln<T1,T2>(Self: (T1,T2)): (T1,T2); extensionmethod;
begin 
  Result := Self; 
  Self.Println; 
end;

/// Выводит кортеж
function Prln<T1,T2,T3>(Self: (T1,T2,T3)): (T1,T2,T3); extensionmethod;
begin 
  Result := Self; 
  Self.Println; 
end;

/// Выводит кортеж
function Prln<T1,T2,T3,T4>(Self: (T1,T2,T3,T4)): (T1,T2,T3,T4); extensionmethod;
begin 
  Result := Self; 
  Self.Println; 
end;

/// Выводит кортеж
function Prln<T1,T2,T3,T4,T5>(Self: (T1,T2,T3,T4,T5)): (T1,T2,T3,T4,T5); extensionmethod;
begin 
  Result := Self; 
  Self.Println; 
end;

/// Возвращает минимальный элемент кортежа
function Min<T>(Self: (T,T)): T; extensionmethod;
where T: IComparable<T>;
begin
  Result := Min(Self[0],Self[1]);
end;

/// Возвращает максимальный элемент кортежа
function Max<T>(Self: (T,T)): T; extensionmethod;
where T: IComparable<T>;
begin
  Result := Max(Self[0],Self[1]);
end;

/// Возвращает минимальный элемент кортежа
function Min<T>(Self: (T,T,T)): T; extensionmethod;
where T: IComparable<T>; 
begin
  Result := Min(Self[0],Self[1],Self[2]);
end;

/// Возвращает максимальный элемент кортежа
function Max<T>(Self: (T,T,T)): T; extensionmethod;
where T: IComparable<T>;
begin
  Result := Max(Self[0],Self[1],Self[2]);
end;

/// Возвращает минимальный элемент кортежа
function Min<T>(Self: (T,T,T,T)): T; extensionmethod;
where T: IComparable<T>;
begin
  Result := Min(Self[0],Self[1],Self[2],Self[3]);
end;

/// Возвращает максимальный элемент кортежа
function Max<T>(Self: (T,T,T,T)): T; extensionmethod;
where T: IComparable<T>; 
begin
  Result := Max(Self[0],Self[1],Self[2],Self[3]);
end;

/// Возвращает минимальный элемент кортежа
function Min<T>(Self: (T,T,T,T,T)): T; extensionmethod;
where T: IComparable<T>; 
begin
  Result := Min(Self[0],Self[1],Self[2],Self[3],Self[4]);
end;

/// Возвращает максимальный элемент кортежа
function Max<T>(Self: (T,T,T,T,T)): T; extensionmethod;
where T: IComparable<T>; 
begin
  Result := Max(Self[0],Self[1],Self[2],Self[3],Self[4]);
end;

///--
procedure __InitModule__;
begin
end;

///--
procedure __FinalizeModule__;
begin
end;


end.