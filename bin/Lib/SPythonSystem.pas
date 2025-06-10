{$HiddenIdents}
unit SPythonSystem;

{$reference '%GAC%\System.dll'}
{$reference '%GAC%\mscorlib.dll'}
{$reference '%GAC%\System.Core.dll'}
{$reference '%GAC%\System.Numerics.dll'}

interface

// uses PABCSystem;

// Basic IO methods

function input(): string;

function input(s: string): string;

type kwargs_gen<T> = class
      public !kwargs: Dictionary<string, T>
        := new Dictionary<string, T>();
      
      constructor Create(
        keys: array of string;
        params values: array of T
        );
      begin
        for var i := 0 to keys.count() - 1 do
          !kwargs[keys[i]] := values[i];
      end;
      
      constructor Create(
        keys: array of char;
        params values: array of T
        );
      begin
        for var i := 0 to keys.count() - 1 do
          !kwargs[keys[i]] := values[i];
      end;
      
      constructor Create(); begin end;
    end;

function all(s: sequence of boolean): boolean;
function any(s: sequence of boolean): boolean;

function __NewSetCreatorInternal<T>(params a: array of T): NewSet<T>;

procedure clear<T>(var st: set of T);

function int(val: string): integer;

function int(val: real): integer;

function round(val: real): integer;

function split(s: string): sequence of string;

function get_keys<K, V>(dct: Dictionary<K, V>): sequence of K;
function get_values<K, V>(dct: Dictionary<K, V>): sequence of V;

function &type(obj: object): string;

//function int(val: string): integer;
function int(b: boolean): integer;

function str(val: object): string;

function float(val: string): real;

function float(x: integer): real;

// Basic sequence functions

function range(s: integer; e: integer; step: integer): sequence of integer;

function range(e: integer): sequence of integer;

function range(s: integer; e: integer): sequence of integer;

function !format(obj: object; fmt: string): string;

function !format(i: integer; fmt: string): string;

function !format(val: real; fmt: string): string;

//function all<T>(seq: sequence of T): boolean;

//function any<T>(seq: sequence of T): boolean;

// Standard Math functions

function abs(x: integer): integer;

function abs(x: real): real;

// function floor(x: real): real;

//Standard functions with Lists

function len<T>(lst: PABCSystem.List<T>): integer;
function len<T>(st: set of T): integer;
function len<K, V>(dct: PABCSystem.Dictionary<K, V>): integer;
function len<T>(arr: array of T): integer;

function &set<T>(sq: sequence of T): set of T;
function &list<T>(sq: sequence of T): PABCSystem.List<T>;

function sorted<T>(lst: PABCSystem.List<T>): PABCSystem.List<T>;

function sum(lst: sequence of integer): integer;

function sum(lst: sequence of real): real;

function !assign<T>(var a: T; b: T): T;

function !pow(x, n: integer): integer;

function !pow(x, n: biginteger): biginteger;

function !pow(x: integer; y: real): real;

function !pow(x: real; y: real): real;

function bigint(x: integer): biginteger;

function !pow_recursion(x, n: integer): integer;

function !pow_recursion(x, n: biginteger): biginteger;

function CreateTuple<T1, T2>(v1: T1; v2: T2): System.Tuple<T1, T2>;

function Dict<TKey, TVal>(params pairs: array of (TKey, TVal)): Dictionary<TKey, TVal>;

type 
    biginteger = PABCSystem.BigInteger;
    !list<T> = PABCSystem.List<T>;
    !dict<K, V> = PABCSystem.Dictionary<K, V>;
    !set<T> = set of T;
    
    empty_list = class
    class function operator implicit<T>(x: empty_list): !list<T>; 
    begin
      Result := new !list<T>();
    end;
    end;
    
    empty_set = class
    class function operator implicit<T>(x: empty_set): !set<T>; 
    begin
      Result := new !set<T>;
    end;
    end;
    
    empty_dict = class
    class function operator implicit<K, V>(x: empty_dict): !dict<K, V>; 
    begin
      Result := new !dict<K, V>();
    end;
    end;


function !empty_list(): empty_list;
function !empty_dict(): empty_dict;
function &set(): empty_set;

implementation

function input(): string;
begin
  PABCSystem.Print();
  Result := PABCSystem.ReadlnString();
end;

function input(s: string): string;
begin
  PABCSystem.Print(s);
  Result := PABCSystem.ReadlnString();
end;

function int(val: string): integer := integer.Parse(val);

function int(val: real): integer := round(val);

function int(b: boolean): integer;
begin
  if b then
    Result := 1
  else
    Result := 0;
end;

function &type(obj: object): string;
begin
    Result := TypeName(obj)
    .Replace('<', '[')
    .Replace('>', ']')
    .Replace('List', 'list')
    .Replace('Dictionary', 'dict')
    .Replace('empty_list', 'list[anytype]')
    .Replace('empty_set', 'set[anytype]')
    .Replace('empty_dict', 'dict[anytype]')
    .Replace('integer', 'int')
    .Replace('string', 'str')
    .Replace('real', 'float')
    .Replace('boolean', 'bool')
    .Replace('System.Numerics.BigInteger', 'bigint');
end;

function int(obj: object): integer := Convert.ToInt32(obj);

function str(val: object): string := val.ToString(); 

function float(val: string): real := real.Parse(val);

function float(x: integer): real := PABCSystem.Floor(x);

function range(s: integer; e: integer; step: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e - 1, step);
end;

function range(s: integer; e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e - 1);
end;

function range(e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(0, e - 1);
end;

//function all<T>(seq: sequence of T): boolean := seq.All(x -> x);

//function any<T>(seq: sequence of T): boolean := seq.Any(x -> x);

function abs(x: integer): integer := if x >= 0 then x else -x;

function abs(x: real): real := PABCSystem.Abs(x);

function len<T>(lst: PABCSystem.List<T>): integer := lst.Count();
function len<T>(st: set of T): integer := st.Count();
function len<K, V>(dct: PABCSystem.Dictionary<K, V>): integer := dct.Count();
function len<T>(arr: array of T): integer := arr.Count();

function sorted<T>(lst: PABCSystem.List<T>): PABCSystem.List<T>;
begin
  var new_list := new PABCSystem.List<T>(lst);
  new_list.sort();
  Result := new_list;
end;

function sum(lst: sequence of integer): integer := lst.sum();
function sum(lst: sequence of real): real := lst.sum();

function !assign<T>(var a: T; b: T): T;
begin
  a := b;
  Result := a;
end;

function !pow(x, n: biginteger): biginteger;
begin
  if (n < 0) then
    raise new System.ArgumentException('возведение в степень не работает для целой отрицательной степени типа bigint.');
  Result := !pow_recursion(x, n);
end;

function !pow_recursion(x, n: biginteger): biginteger;
begin
  
  if (n = 0) then
    Result := 1
  else begin
    Result := !pow_recursion(x, n div 2);
    Result *= Result;
    if ((n mod 2) = 1) then
      Result *= x;
  end;
end;

function !pow(x, n: integer): integer;
begin
  if (n < 0) then
    raise new System.ArgumentException('возведение в степень не работает для целой отрицательной степени, используйте привидение к типу с плавающей точкой.');
  Result := !pow_recursion(x, n);
end;

function !pow(x: integer; y: real): real := Power(x, y);

function !pow(x: real; y: real): real := Power(x, y);

function !pow_recursion(x, n: integer): integer;
begin
  
  if (n = 0) then
    Result := 1
  else begin
    Result := !pow_recursion(x, n div 2);
    Result *= Result;
    if ((n mod 2) = 1) then
      Result *= x;
  end;
end;

function bigint(x: integer): biginteger;
begin
  Result := x;
end;

function !count<T>(Self: PABCSystem.List<T>; val: T): integer; extensionmethod;
begin
  Result := 0;
  for var i := 0 to Self.Count - 1 do
       if val = Self[i] then
         Result += 1;
end;

function pop<T>(Self: PABCSystem.List<T>; ind: integer): T; extensionmethod;
begin
  Result := Self[ind];
  Self.RemoveAt(ind);
end;

procedure add<T>(Self: set of T; val: T); extensionmethod;
begin
  Self += val;
end;

procedure remove<T>(Self: set of T; val: T); extensionmethod;
begin
  Self -= val;
end;

function get_keys<K, V>(dct: Dictionary<K, V>):= dct.keys;
function get_values<K, V>(dct: Dictionary<K, V>):= dct.values;

function copy<T>(Self: set of T): set of T; extensionmethod;
begin
  Result := Self;
end;

procedure clear<T>(var st: set of T);
begin
  st := [];
end;

function __NewSetCreatorInternal<T>(params a: array of T): NewSet<T>;
begin
  //Result._hs := new HashSet<T>;
  Result._hs.UnionWith(a);
end; 

function CreateTuple<T1, T2>(v1: T1; v2: T2): System.Tuple<T1, T2> := (v1, v2);

function Dict<TKey, TVal>(params pairs: array of (TKey, TVal)): Dictionary<TKey, TVal>;
begin
  Result := new Dictionary<TKey, TVal>();
  for var i := 0 to pairs.Length - 1 do
    Result.Add(pairs[i][0], pairs[i][1]);
end;

function &set<T>(sq: sequence of T): set of T;
begin
  foreach var e in sq do
    Result.Add(e);
end;

function &list<T>(sq: sequence of T): PABCSystem.List<T> := sq.ToList();

function all(s: sequence of boolean): boolean;
begin
  Result := true;
  foreach var elem in s do
    if (not elem) then
    begin
      Result := false;
      break;
    end;
end;

function any(s: sequence of boolean): boolean;
begin
  Result := false;
  foreach var elem in s do
    if (elem) then
    begin
      Result := true;
      break;
    end;
end;

function ToDictionary<T, U>(Self: sequence of System.tuple<T, U>): Dictionary<T, U>; extensionmethod;
begin
  Result := Self.ToDictionary(x->x[0],x->x[1]);
end;

function !empty_list(): empty_list := new empty_list();
function !empty_dict(): empty_dict := new empty_dict();
function &set(): empty_set := new empty_set;

function round(val: real): integer := PABCSystem.round(val);

function split(s: string): sequence of string;
begin
  var temp := '';
  var i := 0;
  
  while i <= s.Length - 1 do
  begin
    if (i <= s.Length - 1) and (s.Substring(i, 1) = ' ') then
    begin
      yield temp;
      temp := '';
      i += 1;
    end
    else
    begin
      temp += s[i];
      i += 1;
    end;
  end;
  
  yield temp;
end;

function !format(i: integer; fmt: string): string;
begin
  if ((fmt.ToLower() = 'x') or (fmt = 'b') or (fmt = 'd')) then
  begin
      var HexChars := '0123456789abcdef';
      if (fmt = 'X') then
        HexChars := HexChars.ToUpper();
      var value := Cardinal(i);
      var radix := 10;
      if (fmt.ToLower() = 'x') then radix := 16;
      if (fmt = 'b') then radix := 2;
      if value = 0 then
      begin
        Result := '0';
        Exit;
      end;
    
      while value > 0 do
      begin
        var digit := value mod radix;
        Result := HexChars[digit + 1] + Result;
        value := value div radix;
      end;
      Exit;
  end;
  if (fmt[1] = '.') then
  begin
    Result := !format(i + 0.0, fmt);
    Exit;
  end;
  raise new System.ArgumentException('Неверный формат для целочисленного аргумента');
end;

function !format(val: real; fmt: string): string;
begin
  var digits: integer;
  if (fmt.Length >= 3) and (fmt[1] = '.') and (fmt.EndsWith('f')) then
  begin
    var numStr := fmt.Substring(1, fmt.Length - 2);
    if TryStrToInt(numStr, digits) then
    begin
      var intPart := Trunc(val);
      if (digits = 0) then
      begin
        Result := intPart.ToString;
        Exit;
      end;
      var frac := Abs(val - intPart);

      var fracPart := Round(frac * Power(10, digits));

      var fracStr := fracPart.ToString;
      while fracStr.Length < digits do
        fracStr := '0' + fracStr;

      Result := intPart.ToString + '.' + fracStr;
      Exit;
    end;
  end;

  raise new System.ArgumentException('Неверный формат для вещественного аргумента');
end;

function !format(obj: object; fmt: string): string;
begin
  Result := '';
  raise new System.ArgumentException('Формат не соответствует типу данных выражения в f-строке');
end;

end.