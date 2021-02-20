unit TypeClasses;

type
  TMonth = (January, February, March, April, May, June, July, August, September, October, November, December);
  Ordering = (_EQ, _LT, _GT);

type
  // ---Typeclasses---
  Eq[T] = typeclass
    function operator=(x, y: T): boolean := not (x <> y);
    function operator<>(x, y: T): boolean := not (x = y);
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
    
    function Less(x, y: T): boolean := compare(x, y) = _LT;
    function Greater(x, y: T): boolean := compare(x, y) = _GT;
    function LessEqual(x, y: T): boolean := compare(x, y) <> _GT;
    function GreaterEqual(x, y: T): boolean := compare(x, y) <> _LT;

    function operator>(x, y: T): boolean := greater(x, y);
    function operator<(x, y: T): boolean := less(x, y);
    function operator>=(x, y: T): boolean := greaterEqual(x, y);
    function operator<=(x, y: T): boolean := lessEqual(x, y);
    
    function Min(x, y: T): T := x < y ? x : y;
    function Max(x, y: T): T := x > y ? y : x;
  end;

  Show[T] = typeclass
    function Show(x: T): string;
  end;
  
  Read[T] = typeclass
    function Read(s:string): T;
  end;
    
  Bounded[T] = typeclass
    function MinBound: T;
    function MaxBound: T;
  end;
  
  Num[T] = typeclass(Eq[T], Show[T])
    function operator+(x, y: T): T;
    function operator-(x, y: T): T := x + negate(y);
    function operator*(x, y: T): T;
    function negate(x: T): T := FromInteger(0) - x;
    function Abs(x: T): T;
    function Sign(x: T): T;
    function fromInteger(x: integer): T;
  end;
  
// ---Instances---
   
  Eq[integer] = instance
    function operator=(x, y: integer):boolean := integer.Equals(x, y);
  end;
  
  Ord[integer] = instance
    {function compare(x, y: integer): Ordering;
    begin
      if x = y then
        Result := _EQ
      else if x < y then
        Result := _LT
      else
        Result := _GT;
    end;}
    function Less(x, y: integer): boolean := x < y;
  end;   
    
  Show[boolean] = instance
    function show(x: boolean): string := x ? 'Правда': 'Ложь';
  end;
  
  Read[TMonth] = instance
    function read(s: string): TMonth;
    begin
      var t := typeof(TMonth);
      Result := TMonth(System.Enum.Parse(t, s));
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
    function operator+(x, y: integer): integer := x + y;
    function operator-(x, y: integer): integer := x - y;
    function operator*(x, y: integer): integer := x * y;
    function Negate(x: integer): integer := -x;
    function Abs(x: integer): integer := PABCSystem.Abs(x);
    function Sign(x: integer): integer := PABCSystem.Sign(x);
    function FromInteger(x: integer): integer := x;
  end;


begin
end.