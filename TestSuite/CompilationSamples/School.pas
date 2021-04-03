unit School;

interface

/// Перевод десятичного числа в двоичную систему счисления
function Bin(x: int64): string;

/// Перевод десятичного числа в двоичную систему счисления
function Bin(x: BigInteger): string;

/// Перевод десятичного числа в восьмеричную систему счисления
function Oct(x: int64): string;

/// Перевод десятичного числа в восьмеричную систему счисления
function Oct(x: BigInteger): string;

/// Перевод десятичного числа в шестнадцатиричную систему счисления
function Hex(x: int64): string;

/// Перевод десятичного числа в шестнадцатиричную систему счисления
function Hex(x: BigInteger): string;

/// Перевод из системы по основанию base [2..36] в десятичную
function Dec(s: string; base: integer): int64;

/// Перевод из системы по основанию base [2..36] в десятичную
function DecBig(s: string; base: integer): BigInteger;

/// Перевод BigInteger в систему счисления по основанию base (2..36)
function ToBase(BI: BigInteger; base: integer): string;

/// Перевод десятичного числа в систему счисления по основанию base (2..36)
function ToBase(sDec: string; base: integer): string;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of int64): (int64, int64);

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of integer): (integer, integer);

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of real): (real, real);

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of BigInteger): (BigInteger, BigInteger);

/// Возвращает НОД
function НОД(a, b: int64): int64;

/// Возвращает НОК пары чисел
function НОК(a, b: int64): int64;

/// Возвращает НОД и НОК пары чисел
function НОДНОК(a, b: int64): (int64, int64);

/// Разложение числа на простые множители
function Factorize(n: integer): List<integer>;

/// Простые числа на интервале [2;n] 
function Primes(n: integer): List<integer>;

/// Первые n простых чисел 
function FirstPrimes(n: integer): List<integer>;

/// Возвращает список, содержащий цифры числа
function Digits(n: int64): List<integer>;

/// Возвращает список делителей натурального числа
function Divisors(n: integer): List<integer>;

///--
function Divizors(n: integer): List<integer>;

/// Возвращает список всех k делителей натурального числа n.
/// Если делителей не ровно k, возвращает пустой список
function Divisors(n, k: integer): List<integer>;

/// Возвращает Sin угла, заданного в градусах
function SinDegrees(x: real): real;

/// Возвращает Cos угла, заданного в градусах
function CosDegrees(x: real): real;

/// Возвращает вещественный массив, заполненный случайными значениями
/// на интервале [a; b) с t знаками в дробной части
function ArrRandomReal(n: integer; a, b: real; t: integer): array of real;

/// Возвращает вещественную последовательность, заполненную случайными значениями
/// на интервале [a; b) с t знаками в дробной части
function SeqRandomReal(n: integer; a, b: real; t: integer): sequence of real;

/// Возвращает вещественную матрицу, заполненную случайными значениями
/// на интервале [a; b) с t знаками в дробной части
function MatrRandomReal(m: integer; n: integer; a, b: real; t: integer): array [,] of real;

/// Возвращает таблицу истинности для двух переменных
function TrueTable(f: function(a, b: boolean): boolean):
    array[,] of boolean;

/// Возвращает таблицу истинности для трех переменных
function TrueTable(f: function(a, b, c: boolean): boolean):
    array[,] of boolean;

/// Возвращает таблицу истинности для четырех переменных
function TrueTable(f: function(a, b, c, d: boolean): boolean):
    array[,] of boolean;

/// Возвращает таблицу истинности для пяти переменных
function TrueTable(f: function(a, b, c, d, e: boolean): boolean):
    array[,] of boolean;

/// Выводит на монитор таблицу истинности
/// f = 0 - только для значения функции False
/// f = 1 - только для значения функции True
procedure TrueTablePrint(a: array[,] of boolean; f: integer := -1);

/// Заменяет последнее вхождение подстроки в строку
procedure ReplaceLast(var Строка: string; ЧтоЗаменить, ЧемЗаменить: string);

implementation

type
  School_BadCharInString = class(Exception)
  end;
  
  School_InvalidBase = class(Exception)
  end;

{$region Bin}

function Bin(x: int64): string;
begin
  x := Abs(x);
  var r := '';
  while x >= 2 do
    (r, x) := (x mod 2 + r, x Shr 1);
  Result := x + r
end;

function Bin(x: BigInteger): string;
begin
  x := Abs(x);
  var r := '';
  while x >= 2 do
    (r, x) := (byte(x mod 2) + r, x div 2);
  Result := byte(x) + r
end;

{$endregion}

{$region Oct}

function Oct(x: int64): string;
begin
  x := Abs(x);
  var r := '';
  while x >= 8 do
    (r, x) := (x mod 8 + r, x Shr 3);
  Result := x + r
end;

function Oct(x: BigInteger): string;
begin
  x := Abs(x);
  var r := '';
  while x >= 8 do
    (r, x) := (byte(x mod 8) + r, x div 8);
  Result := byte(x) + r
end;

{$endregion}

{$region Hex}
function Hex(x: int64): string;
begin
  x := Abs(x);
  var s := '0123456789ABCDEF';
  var r := '';
  while x >= 16 do
    (r, x) := (s[x mod 16 + 1] + r, x Shr 4);
  Result := s[x + 1] + r
end;

function Hex(x: BigInteger): string;
begin
  x := Abs(x);
  var s := '0123456789ABCDEF';
  var r := '';
  while x >= 16 do
    (r, x) := (s[byte(x mod 16) + 1] + r, x div 16);
  Result := s[byte(x) + 1] + r
end;

{$endregion}

const sb = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';

{$region Dec}

function Dec(s: string; base: integer): int64;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {base}');
  s := s.ToUpper;
  var r := s.Except(sb[:base + 1]).JoinToString;
  if r.Length > 0 then
    raise new School_BadCharInString
    ($'ToDecimal: Недопустимый символ "{r}" в строке {s}');
  var (pa, p) := (BigInteger.One, BigInteger.Zero);
  foreach var c in s.Reverse do
  begin
    var i := Pos(c, sb) - 1;
    p += pa * i;
    pa *= base
  end;
  Result := int64(p)
end;

function DecBig(s: string; base: integer): BigInteger;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {base}');
  s := s.ToUpper;
  var r := s.Except(sb[:base + 1]).JoinToString;
  if r.Length > 0 then
    raise new School_BadCharInString
    ($'ToDecimal: Недопустимый символ "{r}" в строке {s}');
  var pa := BigInteger.One;
  Result := BigInteger.Zero;
  foreach var c in s.Reverse do
  begin
    var i := Pos(c, sb) - 1;
    Result += pa * i;
    pa *= base
  end
end;
{$endregion}

{$region ToBase}

/// Перевод BigInteger в систему счисления по основанию base (2..36)
function ToBase(BI: BigInteger; base: integer): string;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {base}');
  var s := new System.Text.StringBuilder('');
  while BI > 0 do 
  begin
    s.Insert(0,sb[integer(BI mod base) + 1]);
    BI := BI div base
  end;
  Result := if s.Length = 0 then '0' else s.ToString
end;
  
/// Перевод BigInteger в систему счисления по основанию base (2..36)
function ToBase(Self: BigInteger; base: integer): string; extensionmethod :=
  ToBase(Self, base);
  
/// Перевод десятичного числа в систему счисления по основанию base (2..36)
function ToBase(sDec: string; base: integer): string;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {base}');
  var n: BigInteger;
  if BigInteger.TryParse(sDec, n) then
    Result := ToBase(n, base)
end;

/// Перевод десятичного числа в систему счисления по основанию base (2..36)
function ToBase(Self: string; base: integer): string; extensionmethod :=
  ToBase(Self, base);

{$endregion}

{$region MinMax}

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of int64): (int64, int64);
begin
  var (min, max) := (int64.MaxValue, int64.MinValue);
  foreach var m in s do
  begin  
    if m < min then
      min := m;
    if m > max then
      max := m
  end;  
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of integer): (integer, integer);
begin
  var (min, max) := (integer.MaxValue, integer.MinValue);
  foreach var m in s do
  begin  
    if m < min then
      min := m;
    if m > max then
      max := m
  end;  
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of real): (real, real);
begin
  var (min, max) := (real.MaxValue, -real.MaxValue);
  foreach var m in s do
  begin  
    if m < min then
      min := m;
    if m > max then
      max := m
  end;  
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of BigInteger): (BigInteger, BigInteger);
begin
  var min, max: BigInteger;
  var FirstValue := True;
  foreach var m in s do
    if FirstValue then
      (min, max, FirstValue) := (m, m, False)
    else
    begin  
      if m < min then
        min := m;
      if m > max then
        max := m
    end;  
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Self: sequence of int64): (int64, int64); extensionmethod :=
MinMax(Self);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Self: sequence of integer): (integer, integer);
    extensionmethod :=
MinMax(Self);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Self: sequence of real): (real, real); extensionmethod :=
MinMax(Self);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Self: sequence of BigInteger): (BigInteger, BigInteger);
extensionmethod := MinMax(Self);

{$endregion}

{$region GCDLCM}

/// Возвращает НОД пары чисел
function НОД(a, b: int64): int64;
begin
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := Abs(a)
end;

/// Возвращает НОК пары чисел
function НОК(a, b: int64): int64;
begin
  var (a1, b1) := (a, b);
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := Abs(a1 div a * b1)
end;

/// Возвращает НОД и НОК пары чисел
function НОДНОК(a, b: int64): (int64, int64);
begin
  var (a1, b1) := (a, b);
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := (a, Abs(a1 div a * b1))
end;

/// Возвращает НОД кортежа двух чисел типа int64
function НОД(Self: (int64, int64)): integer; extensionmethod;
begin
  var (a, b) := Self;
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := Abs(a)
end;

/// Возвращает НОД кортежа двух чисел типа integer
function НОД(Self: (integer, integer)): integer; extensionmethod;
begin
  var (a, b) := Self;
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := Abs(a)
end;

{$endregion}

{$region Factorize}

/// Разложение числа на простые множители
function Factorize(n: integer): List<integer>;
begin
  n := Abs(n);
  var i := 2;
  var L := new List<integer>;
  while i * i <= n do  
    if n mod i = 0 then
    begin
      L.Add(i);
      n := n div i;
      if n < i then
        break
    end
    else
      i += i = 2 ? 1 : 2;
  if n > 1 then
    L.Add(n);
  Result := L
end;

/// Разложение числа на простые множители
function Factorize(Self: integer): List<integer>; extensionmethod :=
Factorize(Self);    

{$endregion}

{$region Primes}

/// Простые числа на интервале [2;n] 
function Primes(n: integer): List<integer>;
// Модифицированное решето Эратосфена на [2;n] 
begin
  var Mas := ArrFill(n, True);
  var i := 2;
  while i * i <= n do
  begin
    if Mas[i - 1] then
    begin
      var k := i * i; 
      while k <= n do
      begin
        Mas[k - 1] := False; 
        k += i
      end
    end; 
    Inc(i)
  end;
  n := Mas.Count(t -> t);
  Result := new List<integer>;
  for var j := 1 to Mas.High do
    if Mas[j] then Result.Add(j + 1)
end;

/// Первые n простых чисел 
function FirstPrimes(n: integer): List<integer>;
// Модифицированное решето Эратосфена 
begin
  var n1 := Trunc(Exp((Ln(n) + 1.088) / 0.8832));
  var Mas := ArrFill(n1, True);
  var i := 2;
  while i * i <= n1 do
  begin
    if Mas[i - 1] then
    begin
      var k := i * i; 
      while k <= n1 do
      begin
        Mas[k - 1] := False; 
        k += i
      end
    end; 
    Inc(i)
  end;
  //n := Mas.Count(t -> t);
  Result := new List<integer>;
  i := 0;
  for var j := 1 to Mas.High do
    if Mas[j] then
    begin
      Result.Add(j + 1);
      Inc(i);
      if i = n then break
    end
end;

/// возвращает True, если число простое и False в противном случае
function IsPrime(Self: integer): boolean; extensionmethod;
begin  
  if Self = 2 then
    Result := True
  else if Self.IsEven or (Self <= 1) then
    Result := False
  else begin
    var i := int64(3);
    Result := True; 
    while i * i <= Self do 
      if Self mod i = 0 then begin
        Result := False;
        exit
      end  
      else
        i += 2
  end
end;

{$endregion}

{$region Digits}

/// Возвращает список, содержащий цифры числа
function Digits(n: int64): List<integer>;
begin
  var St := new Stack<integer>;
  n := Abs(n);
  if n = 0 then
    Result := |0|.ToList
  else
  begin
    while n > 0 do
    begin
      St.Push(n mod 10);
      n := n div 10
    end;
    Result := St.ToList
  end
end;

/// Возвращает список, содержащий цифры числа
function Digits(Self: integer): List<integer>;
    extensionmethod :=
Digits(Self);

/// возвращает список, содержащий цифры числа
function Digits(Self: int64): List<integer>;
    extensionmethod :=
Digits(Self);

{$endregion}

{$region Divisors}

/// возвращает список всех делителей натурального числа
function Divisors(n: integer): List<integer>;
begin
  n := Abs(n); // foolproof
  var L := new List<integer>;
  L.Add(1);
  L.Add(n);
  if n > 3 then
  begin
    var k := 2;
    while (k * k <= n) and (k < 46341) do
    begin
      if n mod k = 0 then
      begin
        var t := n div k;
        L.Add(k);
        if k < t then L.Add(t)
        else break
      end;  
      Inc(k)
    end;
    L.Sort;
  end;
  Result := L
end;

///--
function Divizors(n: integer) := Divisors(n);


/// Возвращает список делителей натурального числа
function Divisors(Self: integer): List<integer>; extensionmethod :=
  Divisors(Self);

///--
function Divizors(Self: integer): List<integer>; extensionmethod :=
  Divisors(Self);
  
/// Возвращает список всех k делителей натурального числа n.
/// Если делителей не ровно k, возвращает пустой список
function Divisors(n, k: integer): List<integer>;
begin
  n := Abs(n); // foolproof
  var L := new List<integer>;
  case n of
    0, 1: if k = 1 then L.Add(n);
    2: if k = 2 then L.AddRange(|1, n|);
  else
    begin
      var t := Trunc(Sqrt(n));
      if (t * t <> n) and k.IsOdd or (t * t = n) and k.IsEven then
      begin
        Result := new List<integer>;
        exit
      end;
      L.AddRange(|1, n|);
      var (p, m) := (2, 2);
      while m <= t do
      begin
        if n mod m = 0 then
        begin
          var r := n div m;
          L.Add(m);
          Inc(p);
          if m < r then
          begin
            L.Add(r);
            Inc(p)
          end  
          else break
        end;
        if p > k then
        begin
          Result := new List<integer>;
          exit
        end;
        Inc(m)
      end;
      if k = p then L.Sort
      else L.Clear
    end
  end;
  Result := L
end;

/// Возвращает список всех k делителей натурального числа n.
/// Если делителей не ровно k, возвращает пустой список
function Divisors(Self, k: integer): List<integer>; extensionmethod :=
  Divisors(Self, k);  

{$endregion}

{$region Trig}

function SinDegrees(x: real): real := Sin(DegToRad(x));

function CosDegrees(x: real): real := Cos(DegToRad(x));

function TanDegrees(x: real): real := Tan(DegToRad(x));

{$endregion}

{$region Random}

/// Возвращает вещественный массив, заполненный случайными значениями
/// на интервале [a; b) с t знаками в дробной части
function ArrRandomReal(n: integer; a, b: real; t: integer): array of real;
begin
  Result := new real[n];
  for var i := 0 to Result.Length - 1 do
    Result[i] := Round(Random * (b - a) + a, t);
end;

/// Возвращает вещественную последовательность, заполненную случайными значениями
/// на интервале [a; b) с t знаками в дробной части
function SeqRandomReal(n: integer; a, b: real; t: integer): sequence of real;
begin
  loop n do
    yield Round(Random * (b - a) + a, t)
end;

/// Возвращает вещественную матрицу, заполненную случайными значениями
/// на интервале [a; b) с t знаками в дробной части
function MatrRandomReal(m: integer; n: integer; a, b: real; t: integer): array [,] of real;
begin
  Result := new real[m, n];
  for var i := 0 to Result.RowCount - 1 do
    for var j := 0 to Result.ColCount - 1 do
      Result[i, j] := Round(Random * (b - a) + a, t);
end;

{$endregion}

{$region BooleanLogic}

/// Возвращает логическое значение операции импликации a -> b
function Imp(Self, b: boolean): boolean; extensionmethod := not Self or b;

/// Возвращает таблицу истинности для двух переменных
function TrueTable(f: function(a, b: boolean): boolean):
    array[,] of boolean;
begin
  Result := new boolean[4, 3];
  var i := 0;
  for var a := False to True do
    for var b := False to True do
    begin
      Result[i, 0] := a;
      Result[i, 1] := b;
      Result[i, 2] := f(a, b);
      i += 1
    end;
end;

/// Возвращает таблицу истинности для трех переменных
function TrueTable(f: function(a, b, c: boolean): boolean):
    array[,] of boolean;
begin
  Result := new boolean[8, 4];
  var i := 0;
  for var a := False to True do
    for var b := False to True do
      for var c := False to True do
      begin
        Result[i, 0] := a;
        Result[i, 1] := b;
        Result[i, 2] := c;
        Result[i, 3] := f(a, b, c);
        i += 1
      end;
end;

/// Возвращает таблицу истинности для четырех переменных
function TrueTable(f: function(a, b, c, d: boolean): boolean):
    array[,] of boolean;
begin
  Result := new boolean[16, 5];
  var i := 0;
  for var a := False to True do
    for var b := False to True do
      for var c := False to True do
        for var d := False to True do
        begin
          Result[i, 0] := a;
          Result[i, 1] := b;
          Result[i, 2] := c;
          Result[i, 3] := d;
          Result[i, 4] := f(a, b, c, d);
          i += 1
        end;
end;

/// Возвращает таблицу истинности для пяти переменных
function TrueTable(f: function(a, b, c, d, e: boolean): boolean):
    array[,] of boolean;
begin
  Result := new boolean[32, 6];
  var i := 0;
  for var a := False to True do
    for var b := False to True do
      for var c := False to True do
        for var d := False to True do
          for var e := False to True do
          begin
            Result[i, 0] := a;
            Result[i, 1] := b;
            Result[i, 2] := c;
            Result[i, 3] := d;
            Result[i, 4] := d;
            Result[i, 5] := f(a, b, c, d, e);
            i += 1
          end;
end;

/// Выводит на монитор таблицу истинности
/// f = 0 - только для значения функции False
/// f = 1 - только для значения функции True
procedure TrueTablePrint(a: array[,] of boolean; f: integer);
begin
  var (n, c) := (a.ColCount, 'a');
  for var i := 0 to n - 2 do
  begin
    Write(' ' + c);
    Inc(c)
  end;
  Writeln(' F');
  Writeln(' ' + (2 * n - 1) * '-');
  for var i := 0 to a.RowCount - 1 do
    if not (((f = 0) and a[i, n - 1]) or ((f = 1) and not a[i, n - 1])) then
    begin
      for var j := 0 to n - 1 do
        Write(if a[i, j] then ' 1' else ' 0');
      Writeln
    end
end;

{$endregion}

{$region String}

/// Заменяет последнее вхождение подстроки в строку
procedure ReplaceLast(var Строка: string; ЧтоЗаменить, ЧемЗаменить: string);
begin
  var Позиция := LastPos(ЧтоЗаменить, Строка);
  if Позиция > 0 then
  begin  
    Delete(Строка, Позиция, ЧтоЗаменить.Length);
    Insert(ЧемЗаменить, Строка, Позиция)
  end  
end;

{$endregion}

end.