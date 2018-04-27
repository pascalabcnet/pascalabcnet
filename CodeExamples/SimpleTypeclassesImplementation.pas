uses System;

type
  TMonthType = (January, February, March, April, May, June, July, August, September, October, November, December);
  
  
  Ordering = (_EQ, _LT, _GL);


type
  // ---Typeclasses---
  
  
  Eq[T] = typeclass
    function equal(x, y: T): boolean;
    begin
      Result := not notEqual(x, y);
    end;
    
    function notEqual(x, y: T) := not equal(x, y);
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
  

  // ---Instances---
  
  
  Eq[integer] = instance
    function equal(x, y: integer):boolean := x = y;
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
  
  
// ---Functions---


function ArrayEq<T>(l1, l2: array of T): boolean; where Eq[T];
begin
  Result := true;
  for var i :=0 to l1.Length - 1 do
    if Eq&[T].notEqual(l1[i], l2[i]) then
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


// ---Test Functions---


procedure TestEq();
begin
  writeln('TestEq');
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,7,8); 
  
    writeln(ArrayEq&[integer](l1, l2));
  end;
  
  begin
    var l1 := Arr(1,3,5,7,8); 
    var l2 := Arr(1,3,5,1,8); 
  
    writeln(ArrayEq&[integer](l1, l2));
  end;
  
end;


procedure TestShow();
begin
  writeln('TestShow');
  ShowNTimes&[boolean](false, 2);
  ShowNTimes&[boolean](true, 4);
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


begin

  TestEq();
  TestShow();
  TestRead();
  
end.