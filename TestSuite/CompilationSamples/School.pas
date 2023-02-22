/// Учебный модуль, реализующий базовые алгоритмы информатики. 
unit School;

interface

/// Перевод десятичного числа в двоичную систему счисления
function Bin(Число: int64): string;

/// Перевод десятичного числа в двоичную систему счисления
function Bin(Число: BigInteger): string;

/// Перевод десятичного числа в восьмеричную систему счисления
function Oct(Число: int64): string;

/// Перевод десятичного числа в восьмеричную систему счисления
function Oct(Число: BigInteger): string;

/// Перевод десятичного числа в шестнадцатиричную систему счисления
function Hex(Число: int64): string;

/// Перевод десятичного числа в шестнадцатиричную систему счисления
function Hex(Число: BigInteger): string;

/// Перевод из системы по основанию base [2..36] в десятичную
function Dec(СтроковоеПредставление: string; Основание: integer): int64;

/// Перевод из системы по основанию base [2..36] в десятичную
function DecBig(СтроковоеПредставление: string; Основание: integer): BigInteger;

/// Перевод BigInteger в систему счисления по основанию base (2..36)
function ToBase(Число: BigInteger; Основание: integer): string;

/// Перевод десятичного числа в систему счисления по основанию base (2..36)
function ToBase(СтроковоеПредставление: string; Основание: integer): string;

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Последовательность: sequence of int64): (int64, int64);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Последовательность: sequence of integer): (integer, integer);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Последовательность: sequence of real): (real, real);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Последовательность: sequence of BigInteger): (BigInteger, BigInteger);

/// Возвращает НОД пары чисел
function НОД(a, b: int64): int64;

/// Возвращает НОД пары чисел
function GCD(a, b: int64): int64;

/// Возвращает НОК пары чисел
function НОК(a, b: int64): int64;

/// Возвращает НОК пары чисел
function LCM(a, b: int64): int64;

/// Возвращает НОД и НОК пары чисел
function НОДНОК(a, b: int64): (int64, int64);

/// Разложение числа на простые множители
function Factorize(n: integer): List<integer>;

/// Простые числа на интервале [2;n] 
function Primes(n: integer): List<integer>;

/// Простые числа на интервале [m;n] 
function Primes(m, n: integer): List<integer>;

/// Первые k простых чисел 
function FirstPrimes(k: integer): List<integer>;

/// Количество простых делителей числа n
function PrimeDivisorsCount(n: integer): integer;

/// Возвращает целочисленный список расширенного представления
/// десятичного числа n по основанию base.
/// По умолчанию base=10 и возвращается список десятичных цифр числа n.
/// Обратное действие выполняет функция Refold. 
function Digits(n: int64; base: integer := 10): List<integer>;

/// Возвращает целое десятичное число на основе его расширенного представления
/// по основанию base (по умолчанию base=10).
/// Функция выполняет действие, обратное функции Digits,
function DigitsToInt64(ext: List<integer>; base: integer := 10): int64;

/// Возвращает список делителей натурального числа n
function Divisors(n: integer): List<integer>;

///--
function Divizors(n: integer): List<integer>;

/// Количество всех делителей числа n, включая 1 и само число
function DivisorsCount(n: integer): integer;

/// Возвращает Sin угла, заданного в градусах
function SinDegrees(x: real): real;

/// Возвращает Cos угла, заданного в градусах
function CosDegrees(x: real): real;

/// Возвращает Tg угла, заданного в градусах
function TanDegrees(x: real): real;

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
/// f = 2 (или не 0, 1, или отсутствует) - полная таблица истинности
/// s = 'abcde' использует указанные буквы для именования колонок. Может отсутствовать.
procedure TrueTablePrint(a: array[,] of boolean; f: integer := 2; s: string := 'abcde');

/// Заменяет последнее вхождение подстроки в строку
procedure ReplaceLast(var Строка: string; ЧтоЗаменить, ЧемЗаменить: string);

/// Множественная замена символов в строке s
/// Каждый символ source[i] заменяется во всей строке s символом target[i]
function ReplaceMany(s, source, target: string): string;

/// Множественная замена подстрок в строке
/// Каждая подстрока source[i] заменяется во всей строке s подстрокой target[i]
function ReplaceMany(s: string; source, target: IList<string>): string;

/// В строке s меняет местами подстроки ss1 и ss2
procedure SwapSubstr(var s: string; ss1, ss2: string);

implementation

type
  School_BadCharInString = class(Exception)
  end;
  
  School_InvalidBase = class(Exception)
  end;

var
  LPrimes: List<integer>; // на отрезке [2;46349]
  ubPrimeDivs := 46349;
  nPrimeDivs: integer; // 4792
  MaxPrimeDiv: integer; // 46349, его квадрат <int64>, получать посредством Sqr

{$region Bin}

function Bin(Число: int64): string;
begin
  Число := Abs(Число);
  Result := '';
  while Число >= 2 do
  begin
    Result += Число mod 2;
    Число := Число div 2
  end;  
  Result += Число;
  Result := Result.Inverse
end;

function Bin(Число: BigInteger): string;
begin
  Число := Abs(Число);
  Result := '';
  while Число >= 2 do
  begin
    Result += byte(Число mod 2);
    Число := Число div 2
  end;  
  Result += byte(Число);
  Result := Result.Inverse
end;

{$endregion}

{$region Oct}

function Oct(Число: int64): string;
begin
  Число := Abs(Число);
  Result := '';
  while Число >= 8 do
  begin
    Result += Число mod 8;
    Число := Число div 8
  end;  
  Result += Число;
  Result := Result.Inverse
end;

function Oct(Число: BigInteger): string;
begin
  Число := Abs(Число);
  Result := '';
  while Число >= 8 do
  begin
    Result += byte(Число mod 8);
    Число := Число div 8
  end;  
  Result += byte(Число);
  Result := Result.Inverse
end;

{$endregion}

{$region Hex}
function Hex(Число: int64): string;
begin
  Число := Abs(Число);
  var ШестнадцатиричныеЦифры := '0123456789ABCDEF';
  Result := '';
  while Число >= 16 do
  begin
    Result += ШестнадцатиричныеЦифры[Число mod 16 + 1];
    Число := Число div 16
  end;  
  Result += ШестнадцатиричныеЦифры[Число + 1];
  Result := Result.Inverse
end;

function Hex(Число: BigInteger): string;
begin
  Число := Abs(Число);
  var ШестнадцатиричныеЦифры := '0123456789ABCDEF';
  Result := '';
  while Число >= 16 do
  begin
    Result += ШестнадцатиричныеЦифры[byte(Число mod 16) + 1];
    Число := Число div 16
  end;  
  Result += ШестнадцатиричныеЦифры[byte(Число) + 1];
  Result := Result.Inverse
end;

{$endregion}

const
  ДопустимыеСимволы = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';

{$region Dec}

function Dec(СтроковоеПредставление: string; Основание: integer): int64;
begin
  if not (Основание in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {Основание}');
  СтроковоеПредставление := СтроковоеПредставление.ToUpper;
  var НедопустимыеСимволы := 
  СтроковоеПредставление.Except(ДопустимыеСимволы[:Основание + 1]).JoinToString;
  if НедопустимыеСимволы.Length > 0 then
    raise new School_BadCharInString
    ($'ToDecimal: Недопустимые символы "{НедопустимыеСимволы}"');
  var ВесРазряда := 1bi;
  var Результат := 0bi;
  foreach var Символ in СтроковоеПредставление.Reverse do
  begin
    var ЗначениеРазряда := Pos(Символ, ДопустимыеСимволы) - 1;
    Результат += ВесРазряда * ЗначениеРазряда;
    ВесРазряда *= Основание
  end;
  Result := int64(Результат)
end;

function DecBig(СтроковоеПредставление: string; Основание: integer): BigInteger;
begin
  if not (Основание in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {Основание}');
  СтроковоеПредставление := СтроковоеПредставление.ToUpper;
  var НедопустимыеСимволы := 
  СтроковоеПредставление.Except(ДопустимыеСимволы[:Основание + 1]).JoinToString;
  if НедопустимыеСимволы.Length > 0 then
    raise new School_BadCharInString
    ($'ToDecimal: Недопустимые символы "{НедопустимыеСимволы}"');
  var ВесРазряда := 1bi;
  Result := 0bi;
  foreach var Символ in СтроковоеПредставление.Reverse do
  begin
    var ЗначениеРазряда := Pos(Символ, ДопустимыеСимволы) - 1;
    Result += ВесРазряда * ЗначениеРазряда;
    ВесРазряда *= Основание
  end
end;
{$endregion}

{$region ToBase}

/// Перевод BigInteger в систему счисления по основанию base (2..36)
function ToBase(Число: BigInteger; Основание: integer): string;
begin
  if not (Основание in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {Основание}');
  var СтроковоеПредставление := new System.Text.StringBuilder('');
  while Число > 0 do 
  begin
    СтроковоеПредставление.Insert(0, ДопустимыеСимволы[integer(Число mod Основание) + 1]);
    Число := Число div Основание
  end;
  Result := if СтроковоеПредставление.Length = 0 then '0'
    else СтроковоеПредставление.ToString
end;

/// Перевод BigInteger в систему счисления по основанию base (2..36)
function ToBase(Self: BigInteger; Основание: integer): string; extensionmethod :=
ToBase(Self, Основание);

/// Перевод десятичного числа в систему счисления по основанию base (2..36)
function ToBase(СтроковоеПредставление: string; Основание: integer): string;
begin
  if not (Основание in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {Основание}');
  var Число: BigInteger;
  if BigInteger.TryParse(СтроковоеПредставление, Число) then
    Result := ToBase(Число, Основание)
end;

/// Перевод десятичного числа в систему счисления по основанию base (2..36)
function ToBase(Self: string; Основание: integer): string; extensionmethod :=
ToBase(Self, Основание);

{$endregion}

{$region MinMax}

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Последовательность: sequence of int64): (int64, int64);
begin
  var min := int64.MaxValue;
  var max := int64.MinValue;
  foreach var Элемент in Последовательность do
  begin
    if Элемент < min then
      min := Элемент;
    if Элемент > max then
      max := Элемент
  end;
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Последовательность: sequence of integer): (integer, integer);
begin
  var min := integer.MaxValue;
  var max := integer.MinValue;
  foreach var Элемент in Последовательность do
  begin
    if Элемент < min then
      min := Элемент;
    if Элемент > max then
      max := Элемент
  end;
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(Последовательность: sequence of real): (real, real);
begin
  var min := real.MaxValue;
  var max := real.MinValue;
  foreach var Элемент in Последовательность do
  begin
    if Элемент < min then
      min := Элемент;
    if Элемент > max then
      max := Элемент
  end; 
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(Последовательность: sequence of BigInteger): (BigInteger, BigInteger);
begin
  var min, max: BigInteger;
  var ЭтоПервыйЭлемент := True;
  foreach var Элемент in Последовательность do
    if ЭтоПервыйЭлемент then
    begin
      min := Элемент;
      max := Элемент;
      ЭтоПервыйЭлемент := False
    end  
    else
    begin
      if Элемент < min then
        min := Элемент;
      if Элемент > max then
        max := Элемент
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

/// Возвращает НОД пары чисел
function GCD(a, b: int64) := НОД(a, b);

/// Возвращает НОК пары чисел
function НОК(a, b: int64): int64;
begin
  var (a1, b1) := (a, b);
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := Abs(a1 div a * b1)
end;

/// Возвращает НОК пары чисел
function LCM(a, b: int64) := НОК(a, b);

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

/// Разложение числа n на простые множители
function Factorize(n: integer): List<integer>;
begin
  n := Abs(n);
  Result := new List<integer>;
  foreach var Divisor in LPrimes do
  begin
    while (Sqr(Divisor) <= n) and (n >= Divisor) do  
      if n mod Divisor = 0 then
      begin
        Result.Add(Divisor);
        n := n div Divisor;
      end
      else
        break;
    if n < Divisor then
      break
  end;  
  Result.Add(n)
end;

/// Разложение числа на простые множители
function Factorize(Self: integer): List<integer>; extensionmethod :=
Factorize(Self);

{$endregion}

{$region Primes}

/// Простые числа на интервале [2;n]
function Primes(n: integer): List<integer>;
// Решето Эратосфена
begin
  Result := new List<integer>;
  if n < 2 then
    exit;
  var Sieve := new boolean[n];
  var p := 2;
  while Sqr(p) <= n do
  begin
    if not Sieve[p - 1] then
    begin
      var notPrime := p * p;
      while notPrime <= n do
      begin
        Sieve[notPrime - 1] := True;
        notPrime += p
      end
    end;
    p += 1
  end;
  for var i := 2 to n do
    if not Sieve[i - 1] then
      Result.Add(i)
end;

// Внутреняя процедура для заполнения LPrimes
// Решето Эратосфена
procedure PrimesInternal;
begin
  LPrimes := new List<integer>;
  var Sieve := new boolean[ubPrimeDivs + 1];
  for var i := 2 to ubPrimeDivs do
    if not Sieve[i] then
    begin
      LPrimes.Add(i);
      var k := i;
      while k <= ubPrimeDivs - i do
      begin
        k += i;
        Sieve[k] := True
      end
    end
end;

/// возвращает True, если число простое и False в противном случае
function IsPrime(Self: integer): boolean; extensionmethod;
begin
  if Self < 2 then
    Result := False
  else
  begin
    Result := True;
    foreach var Простое in LPrimes do
      if Sqr(Простое) > Self then
        break
      else if Self mod Простое = 0 then
      begin
        Result := False;
        break
      end
  end
end;

/// Простые числа на интервале [m;n] 
function Primes(m, n: integer): List<integer>;
begin
  var ls := ubPrimeDivs; // длина сегмента
  if m > n then Swap(m, n);
  Result := new List<integer>;
  if m <= LPrimes[^1] then
  begin
    foreach var prime in LPrimes do
    begin
      if prime > n then break;
      if prime >= m then Result.Add(prime)
    end;
    m := Result[^1] + 1;
    if m >= n then exit
  end;  
  var ls64: int64 := ls;
  var ln := Ceil(m / ls); // номер левого сегмента
  var rn := Ceil(n / ls); // номер правого сегмента
  for var sn := ln to rn do // решето на каждом сегменте
  begin
    var a := Max((sn - 1) * ls64 + 1, 2); // int64
    var b := sn * ls64; // int64
    var Sieve := new boolean[ls];
    for var i := 0 to LPrimes.Count - 1 do // перебор простых
    begin
      var prime := LPrimes[i];
      if Sqr(prime) > b then break;
      var k := a; // int64
      var d := k mod prime;
      if d <> 0 then k += prime - d;
      while k <= b do
      begin
        Sieve[integer(k - a)] := True; // индекс типа int64 недопустим
        k += prime
      end
    end;
    for var i := 0 to Sieve.High do
      if not Sieve[i] then
        if (a + i >= m) and (a + i <= n) then
          Result.Add(a + i)
  end
end;

/// Первые k простых чисел 
function FirstPrimes(k: integer): List<integer>;
begin
  k := k.ClampTop(integer.MaxValue); // на всякий случай
  if k <= nPrimeDivs then Result := LPrimes[:k]
  else
  begin
    Result := LPrimes;
    var kprimes := nPrimeDivs; // количество найденных простых чисел
    var n := maxPrimeDiv + 2; // очередной кандидат на простое число
    repeat
      if n.IsPrime then
      begin
        Result.Add(n);
        Inc(kprimes)
      end;  
      n += 2
    until kprimes = k
  end
end;

/// Количество простых делителей числа n
function PrimeDivisorsCount(n: integer): integer;
begin
  Result := 0;
  foreach var prime in LPrimes do
  begin
    while (Sqr(prime) <= n) and (n >= prime) do  
      if n mod prime = 0 then
      begin
        Result += 1;
        n := n div prime;
      end
      else break;
    if n < prime then break
  end;  
  Result += 1
end;

/// Количество простых делителей числа
function PrimeDivisorsCount(Self: integer): integer; extensionmethod := PrimeDivisorsCount(Self);

{$endregion}

{$region Digits}

/// Возвращает целочисленный список расширенного представления
/// десятичного числа n по основанию base.
/// По умолчанию base=10 и возвращается список десятичных цифр числа n.
/// Обратное действие выполняет функция Refold.
function Digits(n: int64; base: integer): List<integer>;
begin
  Result := new List<integer>;
  if (n < 0) or (base < 2) then exit;
  if n = 0 then Result := Lst(0)
  else
    while n > 0 do
    begin
      var rem: integer := n mod base;
      Result.Add(rem);
      n := n div base
    end;
    Result.Reverse
end;

function Digits(Self: integer; base: integer := 10): List<integer>;
    extensionmethod := Digits(Self, base);

function Digits(Self: int64; base: integer := 10): List<integer>;
    extensionmethod := Digits(Self, base);
    
/// Возвращает целое десятичное число на основе его расширенного представления
/// по основанию base (по умолчанию base=10).
/// Функция выполняет действие, обратное функции Digits
function DigitsToInt64(ext: List<integer>; base: integer): int64;
begin
  Result := 0;
  var p := int64(1);
  for var i := ext.Count -1 downto 0 do
  begin  
    Result += ext[i] * p;
    p *= base
  end
end;

function DigitsToInt64(Self: List<integer>; base: integer := 10): int64;
    extensionmethod := DigitsToInt64(Self, base);    

{$endregion}

{$region Divisors}

/// возвращает список всех делителей натурального числа n
// на основе идеи А. Богданова
function Divisors(n: integer): List<integer>;
begin
  Result := new List<integer>;
  Result.Add(1);
  foreach var prime in LPrimes do
  begin
    if Sqr(prime) > n then break;
    var ndivs := Result.Count;
    while n mod prime = 0 do
    begin
      for var i := Result.Count - ndivs to Result.Count - 1 do
        Result.Add(Result[i] * prime);
      n := n div prime
    end
  end;
  if n > 1 then
    for var i := 0 to Result.Count - 1 do
      Result.Add(Result[i] * n);
  Result.Sort;
end;

///--
function Divizors(n: integer) := Divisors(n);

/// Возвращает список делителей натурального числа
function Divisors(Self: integer): List<integer>; extensionmethod :=
Divisors(Self);

///--
function Divizors(Self: integer): List<integer>; extensionmethod :=
Divisors(Self);

/// Количество всех делителей числа n, включая 1 и само число
function DivisorsCount(n: integer): integer;
begin
  Result := 1;
  foreach var prime in LPrimes do
  begin
    if Sqr(prime) > n then break;
    var ndivs := Result;
    while n mod prime = 0 do
    begin
      for var i := Result - ndivs to Result - 1 do
        Result += 1;
      n := n div prime;
    end
  end;  
  if n > 1 then
    for var i := 0 to Result - 1 do
      Result += 1;
end;

/// Количество всех делителей числа, включая 1 и само число
function DivisorsCount(Self: integer): integer; extensionmethod := DivisorsCount(Self);

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
            Result[i, 4] := e;
            Result[i, 5] := f(a, b, c, d, e);
            i += 1
          end;
end;

/// Выводит на монитор таблицу истинности
/// f = 0 - только для значения функции False
/// f = 1 - только для значения функции True
/// f = 2 (или не 0, 1, или отсутствует) - полная таблица истинности
/// s = 'abcde' использует указанные буквы для именования колонок. Может отсутствовать.
procedure TrueTablePrint(a: array[,] of boolean; f: integer; s: string);
begin
  var n := a.ColCount;
  s := s?[:n];
  s := s.PadRight(n - 1, ' ');
  Write(' ');
  for var i := 1 to s.Length do
    Print(s[i]);
  Writeln('F');
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

/// Множественная замена символов в строке s
/// Каждый символ source[i] заменяется во всей строке s символом target[i]
function ReplaceMany(s, source, target: string): string;
begin
  var SB := new StringBuilder(s);
  var m := target.Length;
  for var i := 1 to source.Length do
    if i <= m then SB.Replace(source[i], target[i])
    else SB.Replace(source[i],'');
  Result := SB.ToString
end;

/// Множественная замена символов в строке s
/// Каждый символ source[i] заменяется во всей строке s символом target[i]
function ReplaceMany(Self, source, target: string): string;
extensionmethod := ReplaceMany(Self, source, target);

/// Множественная замена подстрок в строке
/// Каждая подстрока source[i] заменяется во всей строке s подстрокой target[i]
function ReplaceMany(s: string; source, target: IList<string>): string;
begin
  var SB := new StringBuilder(s);
  var tl := target.Count - 1;
  for var i := 0 to source.Count - 1 do
    if i <= tl then SB.Replace(source[i], target[i])
    else SB.Replace(source[i], '');
  Result := SB.ToString
end;

/// Множественная замена подстрок в строке
/// Каждая подстрока source[i] заменяется во всей строке s подстрокой target[i]
function ReplaceMany(Self: string; source, target: IList<string>): string;
extensionmethod := ReplaceMany(Self, source, target);

/// В строке s меняет местами подстроки ss1 и ss2
procedure SwapSubstr(var s: string; ss1, ss2: string);
begin
  if not ((ss1 in s) and (ss2 in s)) then exit;
  var D := Dict((ss1, ss2), (ss2, ss1));
  s := Regex.Replace(s, $'({ss1}|{ss2})', m -> D[m.Value])
end;

/// В строке s меняет местами подстроки ss1 и ss2
procedure SwapSubstr(var Self: string; ss1, ss2: string);
  extensionmethod := SwapSubstr(Self, ss1, ss2);

{$endregion}

begin
  PrimesInternal;
  nPrimeDivs := LPrimes.Count;
  maxPrimeDiv := LPrimes[^1];
end.