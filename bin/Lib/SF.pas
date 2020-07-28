unit SF;

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

function RlnI3 := ReadInteger3;
function RlnR3 := ReadlnReal3;
function RlnC3 := ReadlnChar3;
function RlnS3 := ReadlnString3;

function RI4 := ReadInteger4;
function RR4 := ReadReal4;
function RC4 := ReadChar4;
function RS4 := ReadString4;

function RlnI4 := ReadInteger4;
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

function Dst<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Distinct;

function Ord<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Order;
function OrdD<T>(Self: sequence of T): sequence of T; extensionmethod := Self.OrderDescending;

/// Преобразует последовательность в строку
function Str<T>(Self: sequence of T): string; extensionmethod := Self.JoinToString;

/// Выводит последовательность, разделяя элементы пробелами
function Pr<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Print;
/// Выводит последовательность, разделяя элементы пробелами, и переходит на новую строку
function Prln<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Println;

/// Возвращает инвертированную последовательность
function Rev<T>(Self: sequence of T): sequence of T; extensionmethod := Self.Reverse;

/// Преобразует последовательность, используя функцию transform
function Sel<T,T1>(Self: sequence of T; transform: T->T1): sequence of T1; extensionmethod := Self.Select(transform);

/// Фильтрует последовательность по условию cond
function Wh<T>(Self: sequence of T; cond: T->boolean): sequence of T; extensionmethod := Self.Where(cond);

/// Преобразует последовательность в массив
function ToA<T>(Self: sequence of T): array of T; extensionmethod := Self.ToArray;

/// Преобразует последовательность в список
function ToL<T>(Self: sequence of T): List<T>; extensionmethod := Self.ToList;

/// Преобразует последовательность в HashSet
function ToHS<T>(Self: sequence of T): HashSet<T>; extensionmethod := Self.ToHashSet;
/// Преобразует последовательность в SortedSet
function ToSS<T>(Self: sequence of T): SortedSet<T>; extensionmethod := Self.ToSortedSet;

/// Преобразует последовательность в HashSet 
function HS<T>(s: sequence of T): HashSet<T> := s.ToHashSet;
/// Преобразует последовательность в SortedSet
function SS<T>(s: sequence of T): SortedSet<T> := s.ToSortedSet;

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


end.