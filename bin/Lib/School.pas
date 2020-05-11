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

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of int64): (int64, int64);

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of integer): (integer, integer);

/// Ввозвращает НОД и НОК пары чисел
function НОДНОК(a, b: int64): (int64, int64);

/// Разложение числа на простые множители
function Factorize(n: int64): array of int64;

/// Рразложение числа на простые множители
function Factorize(n: integer): array of integer;

/// Простые числа на интервале [2;n] 
function Primes(n: integer): array of integer;

/// Первые n простых чисел 
function FirstPrimes(n: integer): array of integer;

/// Возвращает массив, содержащий цифры числа
function Digits(n: int64): array of integer;

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

{$region}

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

{$region}


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

{$region}

{$region Dec}

function Dec(s: string; base: integer): int64;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToDecimal: Недопустимое основание {base}');
  var sb := '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
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
  var sb := '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';
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

{$region}

{$region MinMax}

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of int64): (int64, int64);
begin
  var (min, max) := (int64.MaxValue, int64.MinValue);
  foreach var m in s do
    if m < min then
      min := m
    else if m > max then
      max := m;
  Result := (min, max)  
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(s: sequence of integer): (integer, integer);
begin
  var (min, max) := (integer.MaxValue, integer.MinValue);
  foreach var m in s do
    if m < min then
      min := m
    else if m > max then
      max := m;
  Result := (min, max)  
end;

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Self: sequence of int64): (int64, int64); extensionmethod :=
  MinMax(Self);
  
/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(Self: sequence of integer): (integer, integer);
    extensionmethod := MinMax(Self);  

{$region}

{$region НОДНОК}

/// Возвращает НОД и НОК пары чисел
function НОДНОК(a, b: int64): (int64, int64);
begin
  (a, b) := (Abs(a), Abs(b));
  var (a1, b1) := (a, b);
  while b <> 0 do
    (a, b) := (b, a mod b);
  Result := (a, a1 div a * b1)
end;

{$region}

{$region Factorize}

/// Разложение числа на простые множители
function Factorize(n: int64): array of int64;
begin
  n := Abs(n);
  var i: int64 := 2;
  var L := new List<int64>;
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
  Result := L.ToArray
end;

/// Разложение числа на простые множители
function Factorize(n: integer): array of integer;
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
  Result := L.ToArray
end;

/// разложение числа на простые множители
function Factorize(Self: int64): array of int64; extensionmethod :=
    Factorize(Self);
    
/// Разложение числа на простые множители
function Factorize(Self: integer): array of integer; extensionmethod :=
    Factorize(Self);    

{$region}

{$region Primes}

/// Простые числа на интервале [2;n] 
function Primes(n: integer): array of integer;
// Модифицированное решето Эратосфена на [2;n] 
begin 
  var Mas := ArrFill(n, True);
  var i := 2;
  while i * i <= n do
  begin 
    if Mas[i-1] then
    begin 
      var k := i*i; 
      while k <= n do
      begin 
        Mas[k-1] := False; 
        k += i
      end
    end; 
    Inc(i) 
  end;
  n := Mas.Count(t -> t);
  Result := new integer[n-1];
  i := 0;
  for var j := 1 to Mas.High do
    if Mas[j] then
    begin
      Result[i] := j+1;
      Inc(i)
    end
end;

/// Первые n простых чисел 
function FirstPrimes(n: integer): array of integer;
// Модифицированное решето Эратосфена 
begin 
  var n1 := Trunc(Exp((Ln(n)+1.088)/0.8832));
  var Mas := ArrFill(n1, True);
  var i := 2;
  while i * i <= n1 do
  begin 
    if Mas[i-1] then
    begin 
      var k := i*i; 
      while k <= n1 do
      begin 
        Mas[k-1] := False; 
        k += i
      end
    end; 
    Inc(i) 
  end;
  //n := Mas.Count(t -> t);
  Result := new integer[n];
  i := 0;
  for var j := 1 to Mas.High do
    if Mas[j] then
    begin
      Result[i] := j+1;
      Inc(i);
      if i = n then
        break
    end
end;

/// возвращает True, если число простое и False в противном случае
function IsPrime(Self: integer): boolean; extensionmethod;
begin
  if Self < 2 then
  begin
    Result := False;
    exit
  end;
  var i := 2;
  while i * i <= Self do  
    if Self mod i = 0 then
    begin
      Result := False;
      exit
    end
    else
      i += if i = 2 then 1 else 2;
  Result := True
end;

/// возвращает True, если число простое и False в противном случае
function IsPrime(Self: int64): boolean; extensionmethod;
begin
  if Self < 2 then
  begin
    Result := False;
    exit
  end;
  var i := int64(2);
  while i * i <= Self do  
    if Self mod i = 0 then
    begin
      Result := False;
      exit
    end
    else
      i += if i = 2 then 1 else 2;
  Result := True
end;

{$region}

{$region Digits}

/// Возвращает массив, содержащий цифры числа
function Digits(n: int64): array of integer;
begin
  var St := new Stack<integer>;
  n := Abs(n);
  if n = 0 then
    Result := Arr(0)
  else
  begin
    while n > 0 do
    begin
      St.Push(n mod 10);
      n := n div 10
    end;
    Result := St.ToArray
  end
end;

/// Возвращает массив, содержащий цифры числа
function Digits(Self: integer): array of integer;
    extensionmethod := Digits(Self);

/// возвращает массив, содержащий цифры числа
function Digits(Self: int64): array of integer;
    extensionmethod := Digits(Self);
    
{$region}

{$region Trig}

function SinDegrees(x: real): real := Sin(DegToRad(x));

function CosDegrees(x: real): real := Cos(DegToRad(x));
    
{$region}

{$refion Random}

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

{$region}

end.