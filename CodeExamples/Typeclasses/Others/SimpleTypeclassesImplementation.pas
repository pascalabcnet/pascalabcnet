﻿uses System, PABCSystem;

type
  TMonthType = (January, February, March, April, May, June, July, August, September, October, November, December);
  
  
  Ordering = (_EQ, _LT, _GT);


type
  // ---Typeclasses---
  
  
  Eq[T] = typeclass
    function operator=(x, y: T): boolean;
    begin
      Result := not (x <> y);
    end;
    
    function operator<>(x, y: T): boolean;
    begin
      Result := not (x = y);
    end;
  end;
  
  
  Ord[T] = typeclass(Eq[T])
    function compare(x, y: T): Ordering;
    begin
      if x = y then
        Result := _EQ
      else if less(x, y) then
        Result := _LT
      else
        Result := _GT;
    end;
    
    function less(x, y: T): boolean;
    begin
      Result := compare(x, y) = _LT;
    end;
    
    function lessEqual(x, y: T): boolean;
    begin
      Result := compare(x, y) <> _GT;
    end;
    
    function operator>(x, y: T): boolean;
    begin
      Result := compare(x, y) = _GT;
    end;
    
    function greaterEqual(x, y: T): boolean;
    begin
      Result := compare(x, y) <> _LT;
    end;
    
    function min(x, y: T): T;
    begin
      Result := lessEqual(x, y) ? x : y;
    end;
    
    function max(x, y: T): T;
    begin
      Result := lessEqual(x, y) ? y : x;
    end;
  end;

  
  Show[T] = typeclass
    function show(x: T): string;
  end;
  
  
  Read[T] = typeclass
    function read(s:string): T;
  end;
  
  
  Bounded[T] = typeclass
    function minBound: T;
    function maxBound: T;
  end;
  

  Num[T] = typeclass(Eq[T], Show[T])
    function operator+(x, y: T): T;
    function operator-(x, y: T): T;
    begin
      Result := x + negate(y);
    end;
    function operator*(x, y: T): T;
    function negate(x: T): T;
    begin
      Result := fromInteger(0) - x;
    end;
    function myabs(x: T): T;
    function mysign(x: T): T;
    function fromInteger(x: integer): T;
  end;
  // ---Instances---
  
  
  Eq[integer] = instance
    function operator=(x, y: integer):boolean := integer.Equals(x, y);
  end;
  
  
  Ord[integer] = instance
    function compare(x, y: integer): Ordering;
    begin
      if x = y then
        Result := _EQ
      else if x < y then
        Result := _LT
      else
        Result := _GT;
    end;
  end;   
  
  
  Show[boolean] = instance
    function show(x: boolean): string := x ? 'Правда': 'Ложь';
  end;
  
  
  Read[TMonthType] = instance
    function read(s:string): TMonthType;
    begin
      var t := typeof(TMonthType);
      Result := TMonthType(Enum.Parse(t, s));
    end;
  end;

  
  Bounded[integer] = instance
    function minBound: integer := integer.MinValue;
    function maxBound: integer := integer.MaxValue;
  end;
  
  
  Show[integer] = instance
    function show(x: integer): string := x.ToString();
  end;
  
  
  Num[integer] = instance
    function operator+(x, y: integer): integer;
    begin
      Result := x + y;
    end;
    function operator-(x, y: integer): integer;
    begin
      Result := x - y;
    end;
    function operator*(x, y: integer): integer;
    begin
      Result := x * y;
    end;
    function negate(x: integer): integer;
    begin
      Result := -x;
    end;
    function myabs(x: integer): integer;
    begin
      Result := abs(x);
    end;
    function mysign(x: integer): integer;
    begin
      Result := Sign(x);
    end;
    function fromInteger(x: integer): integer;
    begin
      Result := x;
    end;
  end;
// ---Functions---


function ArrayEq<T>(l1, l2: array of T): boolean; where Eq[T];
begin
  Result := true;
  for var i :=0 to l1.Length - 1 do
    if l1[i] <> l2[i] then
    begin
      Result := false;
      break;
    end;
end;


procedure ShowNTimes<T>(x: T; n: integer); where Show[T];
begin
  for var i:= 1 to n do
    writeln(Show&[T].show(x));
end;


function ReadListOfMonths<T>(x: array of string): array of T; where Read[T];
begin
  Result := new T[x.Length];
  for var i := 0 to x.Length - 1 do
  begin
    Result[i] := Read&[T].read(x[i]);
  end;
end;


procedure MySort<T>(var a: array of T); where Ord[T];
begin
  for var i := 1 to a.Length - 1 do
  begin
    var sorted := true;
    for var j := 0 to a.Length - 1 - i do
    begin
      if a[j] > a[j + 1] then
      begin
        swap(a[j], a[j + 1]);
        sorted := false;
      end;
    end;
    
    if sorted then
      break;
  end;
end;


function NormL2<T>(a, b: T): T; where Num[T];
begin
  Result := myabs(a * a + b * b);
end;
// ---Test Functions---


procedure TestEq();
begin
  writeln('TestEq');
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,7,8); 
  
    writeln(ArrayEq(l1, l2));
  end;
  
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,1,8); 
  
    writeln(ArrayEq(l1, l2));
  end;
  
end;


procedure TestShow();
begin
  writeln('TestShow');
  ShowNTimes(false, 2);
  ShowNTimes(true, 4);
end;


procedure TestRead();
begin
  writeln('TestRead');
  var a := Arr(January, February, March, April);
  var b := Arr('January', 'February', 'March', 'April');
  
  var c := ReadListOfMonths&[TMonthType](b);
  var isCorrect := true;
  for var i := 0 to a.Length - 1 do
  begin
    if a[i] <> c[i] then
    begin
      isCorrect := false;
      break;
    end;
  end;
  writeln(isCorrect);
end;


procedure TestOrd();
begin

  writeln('TestOrd');
  var a := Arr(3, 1, 4, 2, 5, 0);
  var res := Arr(a);
  
  Sort(res);  
  MySort(a);
  
  var isCorrect := a.SequenceEqual(res);

  writeln(isCorrect);  
  
end;


procedure TestNum();
begin

  writeln('TestNum');
  var a := -3;
  var b := -4;
  var res := NormL2(a, b);
  writeln(res = 25);
end;


begin

  TestEq();
  TestShow();
  TestRead();
  TestOrd();
  TestNum();
  
end.