{$HiddenIdents}
unit SPythonSystem;

// В SPython данной версии нет возможности использовать пространства имен
// {$reference '%GAC%\System.dll'}
// {$reference '%GAC%\mscorlib.dll'}
// {$reference '%GAC%\System.Core.dll'}
// {$reference '%GAC%\System.Numerics.dll'}

interface

// uses PABCSystem;

// Basic IO methods

function input(): string;

function input(s: string): string;

///--
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
function all(s: sequence of integer): boolean;
function any(s: sequence of boolean): boolean;
function any(s: sequence of integer): boolean;

function enumerate<T>(s: sequence of T; start: integer := 0): sequence of (integer,T);
function filter<T>(cond: T -> boolean; s: sequence of T): sequence of T;
function sorted<T>(s: sequence of T; reverse: boolean := false): sequence of T;
function sorted<T,T1>(s: sequence of T; key: T -> T1; reverse: boolean := false): sequence of T;

function int(val: string): integer;

function int(val: real): integer;

function int(b: boolean): integer;

function str(val: object): string;

function float(val: string): real;

function float(x: integer): real;

function bool(val: integer): boolean;

function round(val: real): integer;

function split(s: string): sequence of string;

function get_keys<K, V>(dct: Dictionary<K, V>): sequence of K;
function get_values<K, V>(dct: Dictionary<K, V>): sequence of V;

function &type(obj: object): string;

// Basic sequence functions

function range(s: integer; e: integer; step: integer): sequence of integer;

function range(e: integer): sequence of integer;

function range(s: integer; e: integer): sequence of integer;

function !format(obj: object; fmt: string): string;

function !format(i: integer; fmt: string): string;

function !format(val: real; fmt: string): string;

//------------------------------------
//     Standard Math functions
//------------------------------------

/// Возвращает абсолютное значение числа
function abs(x: integer): integer;
/// Возвращает абсолютное значение числа
function abs(x: real): real;

/// Возвращает x в степени y
function pow(x,y: real): real;
/// Возвращает x в целой степени n
function pow(x: real; n: integer): real;
/// Возвращает x в целой степени n
function pow(x: BigInteger; n: integer): BigInteger;

{$region STANDARD CONTAINERS}

type list<T> = class(IEnumerable<T>)
    private
      wrappee : PABCSystem.List<T>;
    
      constructor Create(l : PABCSystem.List<T>) := wrappee := l;
    
      function GetItem(ind : integer) : T := wrappee[ind];
      
      procedure SetItem(ind : integer; newItem : T) := wrappee[ind] := newItem;
    
    public
    
      static function operator implicit(l : PABCSystem.List<T>) : list<T> := new list<T>(l);
      
      constructor Create() := wrappee := new PABCSystem.List<T>();
      
      constructor Create(capacity : integer) := wrappee := new PABCSystem.List<T>(capacity);
    
      constructor Create(collection : sequence of T) := wrappee := new PABCSystem.List<T>(collection);
    
      property ByIndex[ind : integer] : T read GetItem write SetItem; default;
    
      ///--
      property !count : integer read wrappee.Count;
    
      procedure append(item : T) := wrappee.Add(item);
      
      procedure extend(seq : sequence of T) := wrappee.AddRange(seq);
      
      procedure clear() := wrappee.Clear();
      
      procedure insert(index : integer; item : T) := wrappee.Insert(index, item);
      
      procedure remove(item : T);
      begin
         if not wrappee.Remove(item) then
           raise new System.ArgumentException($'ValueError: {item} is not in list');
      end;
      
      function pop() : T;
      begin
        var lastIndex := wrappee.Count - 1;
        Result := wrappee[lastIndex];
        wrappee.RemoveAt(lastIndex);
      end;
      
      function pop(index : integer) : T;
      begin
        Result := wrappee[index];
        wrappee.RemoveAt(index);
      end;
    
      function index(item : T) : integer := index(item, 0, wrappee.Count);
      
      function index(item : T; start : integer) : integer := index(item, start, wrappee.Count);
      
      function index(item : T; start : integer; &end : integer) : integer;
      begin
        Result := wrappee.IndexOf(item, start, &end - start);
        if Result = -1 then
          raise new System.ArgumentException($'ValueError: {item} is not in list');
      end;
      
      function count(item : T) : integer;
      begin
        var comparer := System.Collections.Generic.EqualityComparer&<T>.Default;
        
        Result := 0;
        for var i := 0 to wrappee.Count - 1 do
        begin
          if comparer.Equals(wrappee[i], item) then
            Result += 1;
        end;
      end;
    
      procedure sort() := wrappee.Sort();
    
      procedure sort(reverse : boolean) := wrappee.OrderByDescending(x -> x);
      
      procedure sort<TKey>(key : T -> TKey) := wrappee.OrderBy(key);
      
      procedure sort<TKey>(key : T -> TKey; reverse : boolean) := wrappee.OrderByDescending(key);
      
      procedure reverse() := wrappee.Reverse();
    
      function copy() : list<T> := new list<T>(wrappee.ToList());
    
      function GetEnumerator() : IEnumerator<T> := wrappee.GetEnumerator();

      function System.Collections.IEnumerable.GetEnumerator() : System.Collections.IEnumerator := GetEnumerator();
    end;

type &set<T> = record(IEnumerable<T>)
  private
    wrappee : PABCSystem.HashSet<T>;
    
    constructor Create(h : PABCSystem.HashSet<T>) := wrappee := h;
    
  public
    
    constructor Create() := wrappee := new PABCSystem.HashSet<T>();
    
    constructor Create(collection : sequence of T) := wrappee := new PABCSystem.HashSet<T>(collection);
    
    constructor Create(collection : sequence of T; comparer : PABCSystem.IEqualityComparer<T>) := wrappee := new PABCSystem.HashSet<T>(collection, comparer);
    
    constructor Create(comparer : PABCSystem.IEqualityComparer<T>) := wrappee := new PABCSystem.HashSet<T>(comparer);
    
    ///--
    property !count : integer read wrappee.Count;
    
    procedure add(elem : T) := wrappee.Add(elem);
    
    procedure remove(elem : T);
    begin
      if not wrappee.Remove(elem) then
        raise new System.Collections.Generic.KeyNotFoundException('KeyError: ' + elem.ToString());
    end;
    
    procedure discard(elem : T) := wrappee.Remove(elem);
    
    function pop() : T;
    begin 
      if wrappee.Count = 0 then
        raise new System.Collections.Generic.KeyNotFoundException('KeyError: ''pop from an empty set''');
      
      Result := wrappee.First();
      wrappee.Remove(Result);
    end;
    
    procedure clear() := wrappee.Clear();
    
    function copy() : &set<T> := new &set<T>(wrappee.ToHashSet());
    
    static function operator in(elem: T; Self: &set<T>): boolean;
    begin
      Result := Self.wrappee.Contains(elem);
    end;
    
  private 
    function setOperation1(op : (HashSet<T>, sequence of T) -> (); params others : array of sequence of T) : &set<T>;
    begin
      Result.wrappee := wrappee.ToHashSet();
      foreach var seq in others do
        op(Result.wrappee, seq);
    end;
    
    procedure setOperation2(op : (HashSet<T>, sequence of T) -> (); params others : array of sequence of T);
    begin
      foreach var seq in others do
        op(wrappee, seq);
    end;
    
  public
    function intersection(params others : array of sequence of T) : &set<T> := setOperation1((h1, h2) -> h1.IntersectWith(h2), others);
    
    function union(params others : array of sequence of T) : &set<T> := setOperation1((h1, h2) -> h1.UnionWith(h2), others);
    
    function difference(params others : array of sequence of T) : &set<T> := setOperation1((h1, h2) -> h1.ExceptWith(h2), others);
    
    function symmetric_difference(params others : array of sequence of T) : &set<T> := setOperation1((h1, h2) -> h1.SymmetricExceptWith(h2), others);
    
    procedure update(params others : array of sequence of T) := setOperation2((h1, h2) -> h1.UnionWith(h2), others);
    
    procedure update(other : sequence of T) := wrappee.UnionWith(other);
    
    procedure intersection_update(params others : array of sequence of T) := setOperation2((h1, h2) -> h1.IntersectWith(h2), others);
    
    procedure intersection_update(other : sequence of T) := wrappee.IntersectWith(other);
    
    procedure difference_update(params others : array of sequence of T) := setOperation2((h1, h2) -> h1.ExceptWith(h2), others);
    
    procedure difference_update(other : sequence of T) := wrappee.ExceptWith(other);
    
    procedure symmetric_difference_update(params others : array of sequence of T) := setOperation2((h1, h2) -> h1.SymmetricExceptWith(h2), others);
    
    procedure symmetric_difference_update(other : sequence of T) := wrappee.SymmetricExceptWith(other);
    
    function isdisjoint(other : sequence of T) : boolean;
    
    function issubset(other : sequence of T) : boolean := wrappee.IsSubsetOf(other);
    
    function issuperset(other : sequence of T) : boolean := wrappee.IsSupersetOf(other);
    
    static function operator=(s1 : &set<T>; s2 : &set<T>) : boolean := s1.wrappee.SetEquals(s2.wrappee);
    
    static function operator<>(s1 : &set<T>; s2 : &set<T>) : boolean := not (s1 = s2);
    
    static function operator<=(s1 : &set<T>; s2 : &set<T>) : boolean := s1.issubset(s2);
    
    static function operator<(s1 : &set<T>; s2 : &set<T>) : boolean := (s1 <= s2) and (s1 <> s2);
    
    static function operator>=(s1 : &set<T>; s2 : &set<T>) : boolean := s1.issuperset(s2);
    
    static function operator>(s1 : &set<T>; s2 : &set<T>) : boolean := (s1 >= s2) and (s1 <> s2);
    
    static function operator or(s1 : &set<T>; s2 : &set<T>) : &set<T> := s1.union(s2);
    
    ///-
    function !orEqual(other : &set<T>) : &set<T>;
    begin
      update(other);
      Result := Self;
    end;
    
    static function operator and(s1 : &set<T>; s2 : &set<T>) : &set<T> := s1.intersection(s2);
    
    ///-
    function !andEqual(other : &set<T>) : &set<T>;
    begin
      intersection_update(other);
      Result := Self;
    end;
    
    static function operator-(s1 : &set<T>; s2 : &set<T>) : &set<T> := s1.difference(s2);
    
    static function operator -=(var s1 : &set<T>; s2 : &set<T>) : &set<T>;
    begin
      s1.difference_update(s2);
    end;
    
    static function operator xor(s1 : &set<T>; s2 : &set<T>) : &set<T> := s1.symmetric_difference(s2);
    
    ///-
    function !xorEqual(other : &set<T>) : &set<T>;
    begin
      symmetric_difference_update(other);
      Result := Self;
    end;
    
//    static function operator:=(var s1: &set<T>; s2: &set<T>): &set<T>;
//    begin
//      s1.wrappee := s2.wrappee.ToHashSet();
//    end;
    
    static function operator implicit(s : HashSet<T>) : &set<T> := new &set<T>(s);
    
    function GetEnumerator() : IEnumerator<T> := wrappee.GetEnumerator();

    function System.Collections.IEnumerable.GetEnumerator() : System.Collections.IEnumerator := GetEnumerator();
end;

type dict<K, V> = class(IEnumerable<PABCSystem.KeyValuePair<K, V>>)
  private
    wrappee : PABCSystem.Dictionary<K, V>;
    
    constructor Create(d : PABCSystem.Dictionary<K, V>) := wrappee := d;
    
    function GetValue(key : K) : V := wrappee[key];
    
    procedure SetValue(key : K; val : V) := wrappee[key] := val;
    
  public
    
    constructor Create() := wrappee := new PABCSystem.Dictionary<K, V>();
    
    constructor Create(capacity : integer) := wrappee := new PABCSystem.Dictionary<K, V>(capacity);
    
    constructor Create(comparer : PABCSystem.IEqualityComparer<K>) := wrappee := new PABCSystem.Dictionary<K, V>(comparer);
    
    constructor Create(capacity : integer; comparer : PABCSystem.IEqualityComparer<K>) := wrappee := new PABCSystem.Dictionary<K, V>(capacity, comparer);
  
    constructor Create(params pairs: array of (K, V));
    begin
      wrappee := new PABCSystem.Dictionary<K, V>();
      
      for var i := 0 to pairs.Length - 1 do
        wrappee[pairs[i].Item1] := pairs[i].Item2;
    end;
  
    constructor Create(seqOfPairs : sequence of (K, V));
    begin
      wrappee := new PABCSystem.Dictionary<K, V>();
      
      foreach var p in seqOfPairs do
        wrappee[p.Item1] := p.Item2;
    end;
  
    ///--
    property !count : integer read wrappee.Count;
  
    property ByKey[key : K] : V read GetValue write SetValue; default;
  
    static function operator in(key: K; Self: dict<K, V>): boolean;
    begin
      Result := Self.wrappee.ContainsKey(key);
    end;
  
    procedure clear() := wrappee.Clear();
    
    function copy() : dict<K, V> := new dict<K, V>(new Dictionary<K,V>(wrappee));
  
    static function fromkeys(sq : sequence of K; val : V := default(V)) : dict<K, V>;
    begin
      Result := new dict<K, V>();
      
      foreach var key in sq do
        Result[key] := val;
    end;
  
    function get(key : K; &default : V := default(V)) : V;
    begin
      var val : V;
      if wrappee.TryGetValue(key, val) then
        Result := val
      else
        Result := &default;
    end;
  
    function items() : sequence of KeyValuePair<K, V> := wrappee;
  
    function keys() := wrappee.Keys;
    
    function pop(key : K) : V;
    begin
      Result := wrappee[key];
      wrappee.Remove(key);
    end;
    
    function pop(key : K; &default : V) : V;
    begin
      var val : V;
      if wrappee.TryGetValue(key, val) then
      begin
        Result := val;
        wrappee.Remove(key);
      end
      else
        Result := &default;
    end;
    
    function popitem() : KeyValuePair<K, V>;
    begin
      Result := wrappee.Last();
      wrappee.Remove(Result.Key);
    end;
    
    function setdefault(key : K; &default : V := default(V)) : V;
    begin
      var val : V;
      if wrappee.TryGetValue(key, val) then
        Result := val
      else
      begin
        wrappee[key] := &default;
        Result := &default;
      end;
    end;
  
    procedure update(params pairs: array of (K, V));
    begin
      for var i := 0 to pairs.Length - 1 do
        wrappee[pairs[i].Item1] := pairs[i].Item2;
    end;
    
    procedure update(seqOfPairs : sequence of (K, V));
    begin
      foreach var p in seqOfPairs do
        wrappee[p.Item1] := p.Item2;
    end;
  
    function values() := wrappee.Values;
    
    static function operator or(d1 : dict<K, V>; d2 : dict<K, V>) : dict<K, V>;
    begin
      Result := new dict<K, V>(d1.wrappee);
      
      foreach var p in d2.wrappee do
        Result[p.Key] := p.Value;
    end;
  
    static function operator implicit(d : PABCSystem.Dictionary<K, V>) : dict<K, V> := new dict<K, V>(d);
  
    function GetEnumerator() : IEnumerator<KeyValuePair<K, V>> := wrappee.GetEnumerator();

    function System.Collections.IEnumerable.GetEnumerator() : System.Collections.IEnumerator := GetEnumerator();
end;

{$endregion STANDARD CONTAINERS}

//Standard functions with Lists

function len<T>(lst: list<T>): integer;
function len<T>(st: &set<T>): integer;
function len<K, V>(dct: dict<K, V>): integer;
function len<T>(arr: array of T): integer;
function len(s: string): integer;

function sorted<T>(lst: list<T>): list<T>;

function sum(s: sequence of boolean): integer;
function sum(s: sequence of integer): integer;
function sum(s: sequence of real): real;

function !assign<T>(var a: T; b: T): T;

function !pow(x, n: integer): integer;

function !pow(x, n: biginteger): biginteger;

function !pow(x: integer; y: real): real;

function !pow(x: real; y: real): real;

function bigint(x: integer): biginteger;

function !pow_recursion(x, n: integer): integer;

function !pow_recursion(x, n: biginteger): biginteger;

// TUPLES BEGIN
  
function !CreateTuple<T>(v: T): System.Tuple<T>;  

function !CreateTuple<T1, T2>(
    v1: T1; v2: T2
    ): System.Tuple<T1, T2>;

function !CreateTuple<T1, T2, T3>(
    v1: T1; v2: T2; v3: T3
    ): System.Tuple<T1, T2, T3>;

function !CreateTuple<T1, T2, T3, T4>(
    v1: T1; v2: T2; v3: T3; v4: T4
    ): System.Tuple<T1, T2, T3, T4>;
 
function !CreateTuple<T1, T2, T3, T4, T5>(
    v1: T1; v2: T2; v3: T3; v4: T4; v5: T5
    ): System.Tuple<T1, T2, T3, T4, T5>;

function !CreateTuple<T1, T2, T3, T4, T5, T6>(
    v1: T1; v2: T2; v3: T3; v4: T4; v5: T5; v6: T6
    ): System.Tuple<T1, T2, T3, T4, T5, T6>;

function !CreateTuple<T1, T2, T3, T4, T5, T6, T7>(
    v1: T1; v2: T2; v3: T3; v4: T4; v5: T5; v6: T6; v7: T7
    ): System.Tuple<T1, T2, T3, T4, T5, T6, T7>;

// TUPLES END

type 
    biginteger = PABCSystem.BigInteger;
    // tuple = System.Tuple;
    tuple<T> = System.Tuple<T>;
    !tuple2<T1, T2> = System.Tuple<T1, T2>;
    !tuple3<T1, T2, T3> = System.Tuple<T1, T2, T3>;
    !tuple4<T1, T2, T3, T4> = System.Tuple<T1, T2, T3, T4>;
    !tuple5<T1, T2, T3, T4, T5> = System.Tuple<T1, T2, T3, T4, T5>;
    !tuple6<T1, T2, T3, T4, T5, T6> = System.Tuple<T1, T2, T3, T4, T5, T6>;
    !tuple7<T1, T2, T3, T4, T5, T6, T7> = System.Tuple<T1, T2, T3, T4, T5, T6, T7>;
    
    ///--
    empty_list = class
    class function operator implicit<T>(x: empty_list): list<T>; 
    begin
      Result := new list<T>();
    end;
    end;
    
    ///--
    empty_set = class
    class function operator implicit<T>(x: empty_set): &set<T>; 
    begin
      Result := new &set<T>();
    end;
    end;
    
    ///--
    empty_dict = class
    class function operator implicit<K, V>(x: empty_dict): dict<K, V>; 
    begin
      Result := new dict<K, V>();
    end;
    end;


function !empty_list(): empty_list;
function !empty_dict(): empty_dict;


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

function enumerate<T>(s: sequence of T; start: integer): sequence of (integer,T)
  := s.Numerate(start);
  
function filter<T>(cond: T -> boolean; s: sequence of T): sequence of T
  := s.Where(cond);

function sorted<T>(s: sequence of T; reverse: boolean): sequence of T
  := reverse ? s.OrderDescending : s.Order;

function sorted<T,T1>(s: sequence of T; key: T -> T1; reverse: boolean): sequence of T
  := reverse ? s.OrderByDescending(key) : s.OrderBy(key);

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

function bool(val: integer): boolean := Convert.ToBoolean(val);

function range(s: integer; e: integer; step: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e - PABCSystem.Sign(step), step);
end;

function range(s: integer; e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(s, e - 1);
end;

function range(e: integer): sequence of integer;
begin
  Result := PABCSystem.Range(0, e - 1);
end;

//------------------------------------
//     Standard Math functions
//------------------------------------

function abs(x: integer): integer := if x >= 0 then x else -x;
function abs(x: real): real := PABCSystem.Abs(x);

function pow(x,y: real): real := PABCSystem.Power(x,y);

function pow(x: real; n: integer): real := PABCSystem.Power(x,n);

function pow(x: BigInteger; n: integer): BigInteger := PABCSystem.Power(x,n);

function len<T>(lst: list<T>): integer := lst.!count;
function len<T>(st: &set<T>): integer := st.!count;
function len<K, V>(dct: dict<K, V>): integer := dct.!count;
function len<T>(arr: array of T): integer := arr.Length;
function len(s: string): integer := s.Length;

function sorted<T>(lst: list<T>): list<T>;
begin
  var newList := lst.copy();
  newList.sort();
  Result := newList;
end;

function sum(s: sequence of boolean): integer := s.Select(x -> Convert.ToInt32(x)).Sum();
function sum(s: sequence of integer): integer := s.Sum();
function sum(s: sequence of real): real := s.Sum();

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

function &set<T>.isdisjoint(other : sequence of T) : boolean;
begin
  if other = nil then
    raise new System.ArgumentNullException('other', 'Null object is not iterable');
  
  var c1 := wrappee.Count;
  var c2 := other.Count();
  
  if (c1 = 0) or (c2 = 0) then
    Result := true
  else
  begin
    if c1 >= c2 then
      Result := other.All(elem -> not wrappee.Contains(elem))
    else
    begin
      var otherSet := new HashSet<T>(other);
      Result := wrappee.All(elem -> not otherSet.Contains(elem));
    end;
  end;
  
end;

function get_keys<K, V>(dct: Dictionary<K, V>):= dct.keys;
function get_values<K, V>(dct: Dictionary<K, V>):= dct.values;

// TUPLES BEGIN

function !CreateTuple<T>(v: T): System.Tuple<T> := System.Tuple.Create(v);

function !CreateTuple<T1, T2>(
    v1: T1; v2: T2
    ): System.Tuple<T1, T2> 
      := (v1, v2);

function !CreateTuple<T1, T2, T3>(
    v1: T1; v2: T2; v3: T3
    ): System.Tuple<T1, T2, T3> 
      := (v1, v2, v3);

function !CreateTuple<T1, T2, T3, T4>(
    v1: T1; v2: T2; v3: T3; v4: T4
    ): System.Tuple<T1, T2, T3, T4> 
      := (v1, v2, v3, v4);
 
function !CreateTuple<T1, T2, T3, T4, T5>(
    v1: T1; v2: T2; v3: T3; v4: T4; v5: T5
    ): System.Tuple<T1, T2, T3, T4, T5> 
      := (v1, v2, v3, v4, v5);

function !CreateTuple<T1, T2, T3, T4, T5, T6>(
    v1: T1; v2: T2; v3: T3; v4: T4; v5: T5; v6: T6
    ): System.Tuple<T1, T2, T3, T4, T5, T6> 
      := (v1, v2, v3, v4, v5, v6);

function !CreateTuple<T1, T2, T3, T4, T5, T6, T7>(
    v1: T1; v2: T2; v3: T3; v4: T4; v5: T5; v6: T6; v7: T7
    ): System.Tuple<T1, T2, T3, T4, T5, T6, T7> 
      := (v1, v2, v3, v4, v5, v6, v7);

// TUPLES END

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

function all(s: sequence of integer): boolean := all(s.Select(x -> Convert.ToBoolean(x)));

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

function any(s: sequence of integer): boolean := any(s.Select(x -> Convert.ToBoolean(x)));

///-
function ToDictionary<T, U>(Self: sequence of System.Tuple<T, U>): dict<T, U>; extensionmethod;
begin
  Result := Self.ToDictionary(x->x[0],x->x[1]);
end;

function !empty_list(): empty_list := new empty_list();
function !empty_dict(): empty_dict := new empty_dict();


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