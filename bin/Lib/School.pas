/// Учебный модуль, реализующий базовые алгоритмы информатики (07.02.2025)
unit School;

interface

type
  Addr32 = class
  private
    addr: string;         // адрес в десятичном виде с октетами
    addr2: string;        // двоичный адрес
    addr10: longword;     // адрес в виде десятичного числа
    
    /// Проверка правильности записи адреса в десятичном формате с октетами
    function AddressValid(address: string): boolean;
    
    /// Проверка правильности записи 32-разрядного адреса в бинарном представлении
    function AddressBinValid(address: string): boolean;
     
    /// Перевод ip-адреса/маски из десятичного представления с октетами в 32-битное
    function AddressToBin(address: string): string;
       
    /// Десятичное значение 32-битного строкового представления двоичного числа
    function BinToDec(bits: string): longword;
   
    /// Двоичное представление десятичного числа с дополением слева нулями до длины 32 символа
    static function Bin32(n:integer): string;
   
    /// Преобразование двоичной записи адреса (с октетами или без них)
    /// в десятичное представление с октетами
    static function BinToAddress(pic: string): string;
        
    static procedure SetAddress(a: Addr32);
    
    procedure SetAddress(value: string);
        
    procedure SetAddressBin(value: string);
  
  public
    /// конструктор для адреса, заданного строкой
    /// задать можно для десятичный адрес с октетами или 32-битный двоичный адрес
    constructor(address: string);  // для десятичной записи с октетами
        
    /// конструктор для маски, заданной числовым суффиксом CIDR (1..31)
    constructor(suffix: integer);
    
    static function operator and (a, b: Addr32): Addr32;
        
    static function operator or (a, b: Addr32): Addr32;
        
    static function operator not (a: Addr32): Addr32;
        
    static function operator + (a: Addr32; b: longword): Addr32;
       
    /// Формирование октетов для 32-битного представления адреса
    function AddrFormat(addr: string): string;
        
    function ToString: string; override;
       
    property value: string read addr write SetAddress;
    property value2: string read addr2;
    property value10: longword read addr10;
    
  end;
    
  CalcIP = class
  private
    ip32: Addr32;                // ip-адрес
    mask32: Addr32;              // маска
    bit_mask: byte;              // длина маски адреса сети (количество единиц в маске)
    n_hosts: integer;            // количество хостов
    wildcard32: Addr32;          // маска хостов
    network32: Addr32;           // адрес сети
    broadcast32: Addr32;         // широковещательный адрес
    hostmin32: Addr32;           // адрес первого хоста
    hostmax32: Addr32;           // адрес последнего хоста
 
    /// Двоичное представление десятичного числа
    function Bin(n:int64): string;
    
    /// Двоичное представление десятичного числа с дополением слева нулями до длины 32 символа
    function Bin32(n:int64): string;
    
    /// Десятичное значение 32-битного строкового представления двоичного числа
    function BinToDec(bits: string): integer;
    
    /// Формирование октетов для 32-битного представления адреса
    function AddrFormat(addr: string): string;
        
    /// Формирование октетов для 32-битного представления адреса с разделением
    /// в позиции bit_mask + 1 двумя пробелами адресов сети и хоста 
    function AddrFine(addr: Addr32): string;
        
    /// Проверка корректности десятичной записи ip-адреса с октетами
    function AddressValid(addr: string): boolean;
        
    /// Перевод ip-адреса/маски из десятичного представления с октетами в 32-битное
    function AddressToBin(addr: string): string;
                
    /// Преобразование двоичной записи адреса (с октетами или без них)
    /// в десятичное представление с октетами
    function BinToAddress(pic: string): string;
        
    /// Проверка корректности маски
    function MaskValid(mask: Addr32): boolean;
  
    procedure Calc;
      
    function AddressFormat(pic: string): string;
    
  public
    
    /// конструктор для пары адрес - маска
    constructor(addr: string; mask: string);
       
    /// конструктор для пары адрес - длина маски адреса
    constructor(addr: string; bitmask: byte);
       
    function ToString: string; override;
       
    function GenAddrBin: sequence of string;
        
    function GenAddr: sequence of Addr32;
    
    property Address: Addr32 read ip32;
    property Netmask: Addr32 read mask32;
    property Bitmask: byte read bit_mask;
    property Hosts: integer read n_hosts;
    property Wildcard: Addr32 read wildcard32;
    property Network: Addr32 read network32;
    property Broadcast: Addr32 read broadcast32;
    property Hostmin: Addr32 read hostmin32;
    property Hostmax: Addr32 read hostmax32;

  end;

/// Перевод десятичного числа в двоичную систему счисления
function Bin(number: int64): string;

/// Перевод десятичного числа в двоичную систему счисления
function Bin(number: BigInteger): string;

/// Перевод десятичного числа в двоичную систему счисления с форматированием.
/// bytes - требуемая длина в байтах, автоматически увеличивается, если недостаточна
/// split - разделитель байтов, можено задать пустое значение ''
function BinFormat(number: BigInteger; bytes: integer := 0; split: string := ' '): string;

/// Перевод десятичного числа в двоичную систему счисления с форматированием.
/// bytes - требуемая длина в байтах, автоматически увеличивается, если недостаточна
/// split - разделитель байтов, можено задать пустое значение ''
function BinFormat(number: int64; bytes: integer := 0; split: string := ' '): string;

/// Перевод десятичного числа в восьмеричную систему счисления
function Oct(number: int64): string;

/// Перевод десятичного числа в восьмеричную систему счисления
function Oct(number: BigInteger): string;

/// Перевод десятичного числа в шестнадцатиричную систему счисления
function Hex(number: int64): string;

/// Перевод десятичного числа в шестнадцатиричную систему счисления
function Hex(number: BigInteger): string;

/// Перевод из системы по основанию base [2..36] в десятичную
function Dec(str: string; base: integer): int64;

/// Перевод из системы по основанию base [2..36] в десятичную
function DecBig(str: string; base: integer): BigInteger;

/// Перевод десятичного int64 в систему счисления по основанию base (2..36)
function ToBase(number: int64; base: integer): string;

/// Перевод десятичного BigInteger в систему счисления по основанию base (2..36)
function ToBase(number: BigInteger; base: integer): string;

/// Перевод десятичного string в систему счисления по основанию base (2..36)
function ToBase(str: string; base: integer): string;

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(seq: sequence of int64): (int64, int64);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(seq: sequence of integer): (integer, integer);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(seq: sequence of real): (real, real);

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(seq: sequence of BigInteger): (BigInteger, BigInteger);

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

/// возвращает True, если число n простое и False в противном случае
function IsPrime(n: integer): boolean;

/// Простые числа на интервале [2;n] 
function Primes(n: integer): List<integer>;

/// Простые числа на интервале [m;n] 
function Primes(m, n: integer): List<integer>;

/// Первые k простых чисел 
function FirstPrimes(k: integer): List<integer>;

/// Разложение числа на простые множители (список всех его простых делителей)
function Factorize(n: integer): List<integer>;

/// Список уникальных простых делителей числа n
/// Для n < 2 возвращается пустой список.
/// Для простого n возвращается список из одного числа n.
/// Для составных чисел при n >= 4 возвращается список уникальных простых делителей.
function PrimeDivisors(n: integer): List<integer>;

/// Количество уникальных простых делителей числа n
/// Для n < 2 возвращается 0.
/// Для простых чисел возвращается 1.
/// Для составных чисел при n >= 4 возвращается количество уникальных простых делителей.
function PrimeDivisorsCount(n: integer): integer;

/// Список простых факторов (простых делителей) числа n
/// Для n < 2 возвращается пустой список.
/// Для простого n возвращается список из одного числа n.
/// Для составных чисел n >= 4 возвращается список всех простых факторов.
function PrimeFactors(n: integer): List<integer>;

/// Количество простых факторов (простых делителей) числа n
/// Для n < 2 возвращается 0.
/// Для простых чисел возвращается 1.
/// Для составных чисел при n >= 4 возвращается количество всех простых факторов.
function PrimeFactorsCount(n: integer): integer;

/// Возвращает целочисленный список расширенного представления
/// десятичного числа n по основанию base.
/// По умолчанию base=10 и возвращается список десятичных цифр числа n.
/// Обратное действие выполняет функция DigitsToInt64. 
function Digits(n: int64; base: integer := 10): List<integer>;

/// Возвращает целочисленный список расширенного представления
/// десятичного числа n по основанию base.
/// По умолчанию base=10 и возвращается список десятичных цифр числа n.
/// Обратное действие выполняют функции DigitsToInt64 и DigitsToBigInteger. 
function Digits(n: BigInteger; base: integer := 10): List<integer>;

/// Возвращает целое десятичное число на основе его расширенного представления
/// по основанию base (по умолчанию base=10).
/// Функция выполняет действие, обратное функции Digits,
function DigitsToInt64(ext: List<integer>; base: integer := 10): int64;

/// Возвращает целое десятичное число на основе его расширенного представления
/// по основанию base (по умолчанию base=10).
/// Функция выполняет действие, обратное функции Digits,
function DigitsToBigInteger(ext: List<integer>; base: integer := 10): BigInteger;

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
    
/// Возвращает таблицу истинности для n переменных
// автор - Александр Хазанчук
function TrueTable(n: integer; f: (array of boolean) -> boolean):
    array[,] of boolean;

/// Выводит на монитор таблицу истинности
/// f = 0 - только для значения функции False
/// f = 1 - только для значения функции True
/// f = 2 (или не 0, 1, или отсутствует) - полная таблица истинности
/// s = 'abcde' использует указанные буквы для именования колонок. Может отсутствовать.
procedure TrueTablePrint(a: array[,] of boolean; f: integer := 2; s: string := 'abcde');

/// Заменяет последнее вхождение подстроки в строку
procedure ReplaceLast(var s: string; source, target: string);

/// Множественная замена символов в строке s
/// Каждый символ source[i] заменяется во всей строке s символом target[i]
function ReplaceMany(s, source, target: string): string;

/// Множественная замена подстрок в строке
/// Каждая подстрока source[i] заменяется во всей строке s подстрокой target[i]
function ReplaceMany(s: string; source, target: IList<string>): string;

/// В строке s меняет местами подстроки ss1 и ss2
procedure SwapSubstr(var s: string; ss1, ss2: string);

/// Множественная замена символов в строке s
/// Каждый символ строки s[i] последовательно ищется в source.
/// Если найден, source[j] заменяется target[j], если не найден - не меняется.
/// Если j больше длины строки target, символ s[i] удаляется.
function Translate(s, source, target: string): string;

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

function Bin(number: int64): string;
begin
  if number < 0 then number := -number;
  Result := '';
  while number >= 2 do
  begin
    Result += number mod 2;
    number := number div 2
  end;  
  Result += number;
  Result := Result.Inverse
end;

function Bin(number: BigInteger): string;
begin
  if number < 0 then number := -number;
  Result := '';
  while number >= 2 do
  begin
    Result += byte(number mod 2);
    number := number div 2
  end;  
  Result += byte(number);
  Result := Result.Inverse
end;

function BinFormat(number: BigInteger; bytes: integer; split: string): string;
begin
  if number < 0 then number := -number;
  var a := number = 0 ? [byte(0)] : number.ToByteArray;
  var len := a.Length;
  if (number > 0) and (a[^1] = 0) then Dec(len);
  SetLength(a, Max(bytes, len));
  Result := a.Reverse.Select(t -> (Convert.ToString(t, 2)).PadLeft(8, '0')).JoinToString(split)
end;

function BinFormat(number: int64; bytes: integer; split: string): string :=
    BinFormat(BigInteger(number), bytes, split);

{$endregion}

{$region Oct}

function Oct(number: int64): string;
begin
  if number < 0 then number := -number;
  Result := '';
  while number >= 8 do
  begin
    Result += number mod 8;
    number := number div 8
  end;  
  Result += number;
  Result := Result.Inverse;
end;

function Oct(number: BigInteger): string;
begin
  if number < 0 then number := -number;
  Result := '';
  while number >= 8 do
  begin
    Result += byte(number mod 8);
    number := number div 8
  end;  
  Result += byte(number);
  Result := Result.Inverse;
end;

{$endregion}

{$region Hex}

function Hex(number: int64): string;
begin
  if number < 0 then number := -number;
  var hex_nums := '0123456789ABCDEF';
  Result := '';
  while number >= 16 do
  begin
    Result += hex_nums[number mod 16 + 1];
    number := number div 16
  end;  
  Result += hex_nums[number + 1];
  Result := Result.Inverse;
end;

function Hex(number: BigInteger): string;
begin
  if number < 0 then number := -number;
  var hex_nums := '0123456789ABCDEF';
  Result := '';
  while number >= 16 do
  begin
    Result += hex_nums[byte(number mod 16 + 1)];
    number := number div 16
  end;  
  Result += hex_nums[byte(number + 1)];
  Result := Result.Inverse;
end;

{$endregion}

const
  valid_chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ';

{$region Dec}

function Dec(str: string; base: integer): int64;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'Dec: Недопустимое основание {base}');
  str := str.ToUpper;
  var invalid_chars := 
    str.Except(valid_chars[:base + 1]).JoinToString;
  if invalid_chars.Length > 0 then
    raise new School_BadCharInString
    ($'Dec: Недопустимые символы "{invalid_chars}"');
  var rank_weight := 1bi;
  var res := 0bi;
  foreach var char in str.Reverse do
  begin
    var rank_value := Pos(char, valid_chars) - 1;
    res += rank_weight * rank_value;
    rank_weight *= base
  end;
  Result := int64(res)
end;

function DecBig(str: string; base: integer): BigInteger;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'DecBig: Недопустимое основание {base}');
  str := str.ToUpper;
  var invalid_chars := 
    str.Except(valid_chars[:base + 1]).JoinToString;
  if invalid_chars.Length > 0 then
    raise new School_BadCharInString
    ($'DecBig: Недопустимые символы "{invalid_chars}"');
  var rank_weight := 1bi;
  Result := 0bi;
  foreach var char in str.Reverse do
  begin
    var rank_value := Pos(char, valid_chars) - 1;
    Result += rank_weight * rank_value;
    rank_weight *= base
  end
end;

{$endregion}

{$region ToBase}

/// Перевод десятичного int64 в систему счисления по основанию base (2..36)
function ToBase(number: int64; base: integer): string;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToBase: Недопустимое основание {base}');
  var sb := new System.Text.StringBuilder('');
  while number > 0 do 
  begin
    sb.Insert(0, valid_chars[number mod base + 1]);
    number := number div base
  end;
  Result := if sb.Length = 0 then '0'
    else sb.ToString
end;

/// Перевод десятичного int64 в систему счисления по основанию base (2..36)
function ToBase(Self: int64; base: integer): string; extensionmethod := ToBase(Self, base);

/// Перевод десятичного BigInteger в систему счисления по основанию base (2..36)
function ToBase(number: BigInteger; base: integer): string;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToBase: Недопустимое основание {base}');
  var sb := new System.Text.StringBuilder('');
  while number > 0 do 
  begin
    sb.Insert(0, valid_chars[integer(number mod base) + 1]);
    number := number div base
  end;
  Result := if sb.Length = 0 then '0'
    else sb.ToString
end;

/// Перевод десятичного BigInteger в систему счисления по основанию base (2..36)
function ToBase(Self: BigInteger; base: integer): string; extensionmethod := ToBase(Self, base);

/// Перевод десятичного string в систему счисления по основанию base (2..36)
function ToBase(str: string; base: integer): string;
begin
  if not (base in 2..36) then
    raise new School_InvalidBase
    ($'ToBase: Недопустимое основание {base}');
  var number: BigInteger;
  if BigInteger.TryParse(str, number) then
    Result := ToBase(number, base)
end;

/// Перевод десятичного string в систему счисления по основанию base (2..36)
function ToBase(Self: string; base: integer): string; extensionmethod := ToBase(Self, base);

{$endregion}

{$region MinMax}

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(seq: sequence of int64): (int64, int64);
begin
  var min := int64.MaxValue;
  var max := int64.MinValue;
  foreach var elem in seq do
  begin
    if elem < min then
      min := elem;
    if elem > max then
      max := elem
  end;
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности
function MinMax(seq: sequence of integer): (integer, integer);
begin
  var min := integer.MaxValue;
  var max := integer.MinValue;
  foreach var elem in seq do
  begin
    if elem < min then
      min := elem;
    if elem > max then
      max := elem
  end;
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(seq: sequence of real): (real, real);
begin
  var min := real.MaxValue;
  var max := -real.MaxValue;
  foreach var elem in seq do
  begin
    if elem < min then
      min := elem;
    if elem > max then
      max := elem
  end; 
  Result := (min, max)
end;

/// Возвращает кортеж из минимума и максимума последовательности s
function MinMax(seq: sequence of BigInteger): (BigInteger, BigInteger);
begin
  var min, max: BigInteger;
  var is_first := True;
  foreach var elem in seq do
    if is_first then
    begin
      (min, max) := (elem, elem);
      is_first := False
    end  
    else
    begin
      if elem < min then
        min := elem;
      if elem > max then
        max := elem
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

{$region Primes}

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

/// возвращает True, если число n простое и False в противном случае
function IsPrime(n: integer): boolean;
begin
  if n < 2 then
    Result := False
  else
  begin
    Result := True;
    foreach var prime in LPrimes do
      if Sqr(prime) > n then
        break
      else if n mod prime = 0 then
      begin
        Result := False;
        break
      end
  end
end;

/// возвращает True, если число простое и False в противном случае
function IsPrime(Self: integer): boolean; extensionmethod := IsPrime(Self);

/// Простые числа на интервале [2;n]
function Primes(n: integer): List<integer>;
// Решето Эратосфена
begin
  Result := [];
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

/// Простые числа на интервале [m;n] 
function Primes(m, n: integer): List<integer>;
begin
  var ls := ubPrimeDivs; // длина сегмента
  if m > n then Swap(m, n);
  Result := [];
  if m <= LPrimes[^1] then
  begin
    foreach var prime in LPrimes do
    begin
      if prime > n then break;
      if prime >= m then Result.Add(prime)
    end;
    if Result.Count = 0 then exit;
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
  // k := Min(k,integer.MaxValue); // на всякий случай
  if k <= nPrimeDivs then Result := LPrimes[:k]
  else
  begin
    Result := LPrimes;
    var kprimes := nPrimeDivs; // количество найденных простых чисел
    var n := maxPrimeDiv + 2;  // очередной кандидат на простое число
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

/// Список уникальных простых делителей числа n
/// Для n < 2 возвращается пустой список.
/// Для простого n возвращается список из одного числа n.
/// Для составных чисел при n >= 4 возвращается список уникальных простых делителей.
function PrimeDivisors(n: integer): List<integer>;
begin
  Result := [];
  if n < 2 then exit;
  var (i, prime, m) := (0, LPrimes[0], n);
  while Sqr(prime) <= n do
  begin
    if n mod prime = 0 then
    begin
      Result.Add(prime);
      repeat
        n := n div prime
      until n mod prime <> 0
    end;
    Inc(i);
    prime := LPrimes[i];
  end;
  if Result.Count = 0 then Result.Add(m)
  else if n > 1 then Result.Add(n)
end;

/// Список уникальных простых делителей числа n
/// Для n < 2 возвращается пустой список.
/// Для простого n возвращается список из одного числа n.
/// Для составных чисел при n >= 4 возвращается список уникальных простых делителей.
function PrimeDivisors(Self: integer): List<integer>; extensionmethod := PrimeDivisors(Self);

/// Количество уникальных простых делителей числа n
/// Для n < 2 возвращается 0.
/// Для простых чисел возвращается 1.
/// Для составных чисел при n >= 4 возвращается количество уникальных простых делителей.
function PrimeDivisorsCount(n: integer): integer;
begin
  Result := 0;
  if n < 2 then Exit;
  var (i, prime, DivIsPrime) := (0, LPrimes[0], True);
  while Sqr(prime) <= n do
  begin
    if n mod prime = 0 then
    begin
      Inc(Result);
      DivIsPrime := False;
      repeat
        n := n div prime
      until n mod prime <> 0
    end;
    Inc(i);
    prime := LPrimes[i];
  end;
  if DivIsPrime then Result := 1
  else if n > 1 then Result += 1
end;

/// Количество уникальных простых делителей числа n
/// Для n < 2 возвращается 0.
/// Для простых чисел возвращается 1.
/// Для составных чисел при n >= 4 возвращается количество уникальных простых делителей.
function PrimeDivisorsCount(Self: integer): integer; extensionmethod := PrimeDivisorsCount(Self);

/// Список простых факторов (простых делителей) числа n
/// Для n < 2 возвращается пустой список.
/// Для простого n возвращается список из одного числа n.
/// Для составных чисел n >= 4 возвращается список всех простых факторов.
function PrimeFactors(n: integer): List<integer>;
begin
  Result := [];
  if n < 2 then exit;
  var (i, prime, m) := (0, LPrimes[0], n);
  while Sqr(prime) <= n do
  begin
    while n mod prime = 0 do
    begin
      Result.Add(prime);
      n := n div prime
    end;
    Inc(i);
    prime := LPrimes[i];
  end;
  if Result.Count = 0 then Result.Add(m)
  else if n > 1 then Result.Add(n)
end;

/// Список простых факторов (простых делителей) числа n
/// Для n < 2 возвращается пустой список.
/// Для простого n возвращается список из одного числа n.
/// Для составных чисел n >= 4 возвращается список всех простых факторов.
function PrimeFactors(Self: integer): List<integer>; extensionmethod := PrimeFactors(Self);

/// Количество простых факторов (простых делителей) числа n
/// Для n < 2 возвращается 0.
/// Для простых чисел возвращается 1.
/// Для составных чисел при n >= 4 возвращается количество всех простых факторов.
function PrimeFactorsCount(n: integer): integer;
begin
  Result := 0;
  if n < 2 then exit;
  var (i, prime, DivIsPrime) := (0, LPrimes[0], True);
  while Sqr(prime) <= n do
  begin
    while n mod prime = 0 do
    begin
      Inc(Result);
      DivIsPrime := False;
      n := n div prime
    end;
    Inc(i);
    prime := LPrimes[i];
  end;
  if DivIsPrime then Result := 1
  else if n > 1 then Result += 1
end;

/// Количество простых факторов (простых делителей) числа n
/// Для n < 2 возвращается 0.
/// Для простых чисел возвращается 1.
/// Для составных чисел при n >= 4 возвращается количество всех простых факторов.
function PrimeFactorsCount(Self: integer): integer; extensionmethod := PrimeFactorsCount(Self);

/// Разложение числа на простые множители
function Factorize(n: integer): List<integer> := PrimeFactors(n);

/// Разложение числа на простые множители
function Factorize(Self: integer): List<integer>; extensionmethod := PrimeFactors(Self);

{$endregion}

{$region Digits}

/// Возвращает целочисленный список расширенного представления
/// десятичного числа n по основанию base.
/// По умолчанию base=10 и возвращается список десятичных цифр числа n.
/// Обратное действие выполняет функция DigitsToInt64.
function Digits(n: int64; base: integer): List<integer>;
begin
  Result := [];
  if (n < 0) or (base < 2) or (base > integer.MaxValue) then exit;
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
    
/// Возвращает целочисленный список расширенного представления
/// десятичного числа n по основанию base.
/// По умолчанию base=10 и возвращается список десятичных цифр числа n.
/// Обратное действие выполняют функции DigitsToInt64 и DigitsToBigInteger. 
function Digits(n: BigInteger; base: integer): List<integer>;
begin
  Result := [];
  if (n < 0) or (base < 2) or (base > integer.MaxValue) then exit;
  if n = 0 then Result := Lst(0)
  else
    while n > 0 do
    begin
      var rem := integer(n mod base);
      Result.Add(rem);
      n := n div base
    end;
    Result.Reverse
end;

function Digits(Self: Biginteger; base: integer := 10): List<integer>;
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
    
/// Возвращает целое десятичное число на основе его расширенного представления
/// по основанию base (по умолчанию base=10).
/// Функция выполняет действие, обратное функции Digits,
function DigitsToBigInteger(ext: List<integer>; base: integer): BigInteger;
begin
  Result := BigInteger.Zero;
  var p := BigInteger.One;
  for var i := ext.Count -1 downto 0 do
  begin  
    Result += ext[i] * p;
    p *= base
  end
end;

function DigitsToBigInteger(Self: List<integer>; base: integer := 10): BigInteger;
    extensionmethod := DigitsToBigInteger(Self, base);

{$endregion}

{$region Divisors}

/// возвращает список всех делителей натурального числа n
// на основе идеи А. Богданова
function Divisors(n: integer): List<integer>;
begin
  Result := [];
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

/// Возвращает таблицу истинности для n переменных
// автор - Александр Хазанчук
function TrueTable(n: integer; f: (array of boolean) -> boolean):
    array[,] of boolean;
begin
  Result := new boolean[1 shl n, n + 1];
  foreach var values in [False, True].CartesianPower(n) index i do
  begin
    foreach var val in values index j do 
    begin
      Result[i, j] := val
    end;
    Result[i, ^1] := f(values)
  end
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
procedure ReplaceLast(var s: string; source, target: string);
begin
  var position := LastPos(source, s);
  if position > 0 then
  begin
    Delete(s, position, source.Length);
    Insert(target, s, position)
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
  
/// Множественная замена символов в строке s
/// Каждый символ строки s[i] последовательно ищется в source.
/// Если найден, source[j] заменяется target[j], если не найден - не меняется.
/// Если j больше длины строки target, символ s[i] удаляется.
function Translate(s, source, target: string): string;
begin
  var sb := new StringBuilder(s.Length);
  foreach var x in s do 
  begin
    var ind := source.IndexOf(x); 
    if ind >= 0 then
    begin
      if ind <= target.Length - 1 then
        sb.Append(target[ind + 1])
    end
    else sb.Append(x)
  end;
  Result := sb.ToString
end;

/// Множественная замена символов в строке s
/// Каждый символ строки s[i] последовательно ищется в source.
/// Если найден, source[j] заменяется target[j], если не найден - не меняется.
/// Если j больше длины строки target, символ s[i] удаляется.
function Translate(Self, source, target: string): string;
  extensionmethod := Translate(Self, source, target);

{$endregion}

{$region ipCalc}
/// 
function Addr32.AddressValid(address: string) := 
    (address = address.MatchValue('(\d{1,3}\.){3}\d{1,3}')) and
    address.Split('.').All(t -> t.ToInteger <= 255);
        
function Addr32.AddressBinValid(address: string) :=
    address.MatchValue('[0|1]*').Length = 32;
      
/// Перевод ip-адреса/маски из десятичного представления с октетами в 32-битное
function Addr32.AddressToBin(address: string) :=
    address.Split('.')
    .Select(t -> Convert.ToString(Convert.ToByte(t), 2).PadLeft(8, '0'))
    .JoinToString('');
       
/// Десятичное значение 32-битного строкового представления двоичного числа
function Addr32.BinToDec(bits: string) := Convert.ToUInt32(bits, 2);
   
/// Двоичное представление десятичного числа с дополением слева нулями до длины 32 символа
static function Addr32.Bin32(n:integer) := (Convert.ToString(n, 2)).PadLeft(32, '0');
   
/// Преобразование двоичной записи адреса (с октетами или без них)
/// в десятичное представление с октетами
static function Addr32.BinToAddress(pic: string): string;
begin
  var s := if '.' in pic then pic.Split('.') else pic.Batch(8).Select(t -> t.JoinToString(''));
  Result := s.Select(t -> Convert.ToByte(t, 2)).JoinToString('.')
end;
    
static procedure Addr32.SetAddress(a: Addr32);   // addr10 задано
begin
  a.addr2 := Bin32(a.addr10);
  a.addr := BinToAddress(a.addr2)
end;
    
procedure Addr32.SetAddress(value: string);
begin
  if AddressValid(value) then
  begin
    addr := value;
    addr2 := AddressToBin(value);
    addr10 := BinToDec(addr2)
  end
  else
  begin
    Println('Некорректная запись адреса:', value);
    Halt
  end
end;
    
procedure Addr32.SetAddressBin(value: string);
begin
  if AddressBinValid(value) then
  begin
    addr := Addr32.BinToAddress(value);
    addr2 := value;
    addr10 := BinToDec(addr2)
  end
  else
  begin
    Println('Некорректная запись адреса:', value);
    Halt
  end
 
end;
  
/// конструктор для адреса, заданного строкой
/// задать можно для десятичный адрес с октетами или 32-битный двоичный адрес
constructor Addr32.Create(address: string);
begin
  if '.' in address then SetAddress(address)
  else SetAddressBin(address);
end;
    
/// конструктор для маски, заданной числовым суффиксом CIDR (1..31)
constructor Addr32.Create(suffix: integer);
begin
  if suffix.Between(1, 31) then SetAddressBin('1' * suffix + (32 - suffix) * '0')
  else
  begin
    Println('Недопустимое значение суффикса:', suffix);
    Halt
  end
end;
    
static function Addr32.operator and (a, b: Addr32): Addr32;
begin
  Result := new Addr32;
  Result.addr10 := a.addr10 and b.addr10;
  SetAddress(Result) 
end;
    
static function Addr32.operator or (a, b: Addr32): Addr32;
begin
  Result := new Addr32;
  Result.addr10 := a.addr10 or b.addr10;
  SetAddress(Result)
end;
    
static function Addr32.operator not (a: Addr32): Addr32;
begin
  Result := new Addr32;
  Result.addr10 := not a.addr10;
  SetAddress(Result)
end;
    
static function Addr32.operator + (a: Addr32; b: longword): Addr32;
begin
  Result := new Addr32;
  Result.addr10 := a.addr10 + b;
  SetAddress(Result)
end;
   
/// Формирование октетов для 32-битного представления адреса
function Addr32.AddrFormat(addr: string): string;
begin
  Result := addr.Substring(0, 8);
  for var i := 1 to 3 do
    Result += '.' + addr.Substring(8 * i, 8);
end;
    
function Addr32.ToString: string := $'{addr + '','', -15}{AddrFormat(addr2)}';

/// Двоичное представление десятичного числа
function CalcIP.Bin(n:int64) := Convert.ToString(n, 2);
    
/// Двоичное представление десятичного числа с дополением слева нулями до длины 32 символа
function CalcIP.Bin32(n:int64) := Convert.ToString(n, 2).PadLeft(32, '0');
    
/// Десятичное значение 32-битного строкового представления двоичного числа
function CalcIP.BinToDec(bits: string) := integer(Convert.ToInt64(bits, 2));
    
/// Формирование октетов для 32-битного представления адреса
function CalcIP.AddrFormat(addr: string): string;
begin
  Result := addr.Substring(0, 8);
  for var i := 1 to 3 do
    Result += '.' + addr.Substring(8 * i, 8);
end;
    
/// Формирование октетов для 32-битного представления адреса с разделением
/// в позиции bit_mask + 1 двумя пробелами адресов сети и хоста 
function CalcIP.AddrFine(addr: Addr32): string;
begin
  Result := AddrFormat(addr.value2);
  var d := bit_mask + bit_mask div 8 + 1;
  if bit_mask > 0 then Result := Result[:d] + '  ' + Result[d:]
end;
    
/// Проверка корректности десятичной записи ip-адреса с октетами
function CalcIP.AddressValid(addr: string) :=
    (addr = addr.MatchValue('(\d{1,3}\.){3}\d{1,3}')) and
    addr.Split('.').All(t -> t.ToInteger <= 255);
        
/// Перевод ip-адреса/маски из десятичного представления с октетами в 32-битное
function CalcIP.AddressToBin(addr: string) :=
    addr.Split('.')
    .Select(t -> Convert.ToString(Convert.ToByte(t), 2).PadLeft(8, '0'))
    .JoinToString('');
        
/// Преобразование двоичной записи адреса (с октетами или без них)
/// в десятичное представление с октетами
function CalcIP.BinToAddress(pic: string): string;
begin
  var s := if '.' in pic then pic.Split('.') else pic.Batch(8).Select(t -> t.JoinToString(''));
  Result := s.Select(t -> Convert.ToByte(t, 2)).JoinToString('.')
end;
    
/// Проверка корректности маски
function CalcIP.MaskValid(mask: Addr32) :=
    mask.value2.MatchValue('1+0+').Length = 32;
 
procedure CalcIP.Calc;
begin
  bit_mask := Pos('0', mask32.value2) - 1;
  n_hosts := integer(2 ** (32 - bit_mask)) - 2;
  var addr := mask32.value.Split('.').Select(t -> (255 - Convert.ToByte(t)).ToString).JoinToString('.');
  wildcard32 := new Addr32(addr);
  network32 := ip32 and mask32;
  broadcast32 := network32 or wildcard32;
  hostmin32 := network32 + 1;
  hostmax32 := network32 + n_hosts
end;
  
function CalcIP.AddressFormat(pic: string) := pic[:bit_mask + 1] + '  ' + pic[bit_mask + 1:];

/// конструктор для пары адрес - маска
constructor CalcIP.Create(addr: string; mask: string);
begin
  ip32 := new Addr32(addr);
  mask32 := new Addr32(mask);
  if not MaskValid(mask32) then
  begin
    Println($'Запись маски неверна: {mask32.value} ({mask32.value2})');
    Halt
  end;
  Calc
end;

/// конструктор для пары адрес - длина маски адреса
constructor CalcIP.Create(addr: string; bitmask: byte);
begin
  if (bitmask < 1) or (bitmask > 31) then
  begin
    Println($'Длина маски адреса неверна: {bitmask}');
    Halt
  end;
  ip32 := new Addr32(addr);
  var mask := bitmask * '1' + (32 - bitmask) * '0';
  mask32 := new Addr32(BinToAddress(mask));
  Calc
end;
    
function CalcIP.ToString: string;
begin
  var s := 
       $'IP адрес (Address)                  | {ip32.value, -15} | {AddrFine(ip32)}{NewLine}';
  s += $'Префикс маски подсети (Bitmask)     | /{bit_mask}{NewLine}';
  s += $'Маска подсети (Netmask)             | {mask32.value, -15} | {AddrFine(mask32)}{NewLine}';
  s += $'Маска хостов (Wildcard)             | {Wildcard32.value, -15} | {AddrFine(wildcard32)}{NewLine}';
  s += $'IP адрес сети (Network)             | {network32.value, -15} | {AddrFine(network32)}{NewLine}';
  s += $'Широковещательный адрес (Broadcast) | {broadcast32.value, -15} | {AddrFine(broadcast32)}{NewLine}';
  s += $'Доступно адресов для хостов (Hosts) | {n_hosts}{NewLine}';
  s += $'Адрес первого хоста (Hostmin)       | {hostmin32.value, -15} | {AddrFine(hostmin32)}{NewLine}';
  s += $'Адрес последнего хоста (Hostmax)    | {hostmax32.value, -15} | {AddrFine(hostmax32)}{NewLine}';
  Result := s
end;
   
function CalcIP.GenAddrBin: sequence of string;
begin
  var n := n_hosts;
  if n > 0 then
  begin  
    for var i := network32.addr10 to broadcast32.addr10 do
      yield Bin32(i)
  end
  else yield sequence Seq&<string>;
end;
    
function CalcIP.GenAddr: sequence of Addr32;
begin
  var n := n_hosts;
  if n > 0 then
    for var i := 1 to n_hosts do
      yield network32 + i
  else yield sequence Seq&<Addr32>;
end;

{$endregion}

begin
  PrimesInternal;
  nPrimeDivs := LPrimes.Count;
  maxPrimeDiv := LPrimes[^1];
end.