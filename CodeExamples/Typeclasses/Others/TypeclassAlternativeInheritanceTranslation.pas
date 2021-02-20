type
  Ordering = (_EQ, _LT, _GT);
  
  Eq<T> = interface
    function equal(x, y: T): boolean;    
    function notEqual(x, y: T) :boolean;
  end;
  
  Ord<T> = interface(Eq<T>)
    function compare(x, y: T): Ordering;
    function less(x, y: T): boolean;
    function lessEqual(x, y: T): boolean;
    function greater(x, y: T): boolean;
    function greaterEqual(x, y: T): boolean;
    function min(x, y: T): T;
    function max(x, y: T): T;
  end;
  
  
  Eq_integer = class(Eq<integer>)
  public  
  
    function equal(x, y: integer): boolean;
    begin
      Result := x = y;
    end; 
    function notEqual(x, y: integer) :boolean;
    begin
      Result := x <> y;
    end;
  end;
  

  Ord_integer = class(Eq_integer, Ord<integer>)
  public
  
    function compare(x, y:integer): Ordering;
    begin
      if self.equal(x, y) then
        Result := _EQ
      else if x < y then
        Result := _LT
      else
        Result := _GT;
    end;
    function less(x, y: integer): boolean;
    begin
      Result := compare(x, y) = _LT;
    end;
    function lessEqual(x, y: integer): boolean;
    begin
      Result := compare(x, y) <> _GT;
    end;
    function greater(x, y: integer): boolean;
    begin
      Result := compare(x, y) = _GT;
    end;
    function greaterEqual(x, y: integer): boolean;
    begin
      Result := compare(x, y) <> _LT;
    end;
    function min(x, y: integer): integer;
    begin
      Result := lessEqual(x, y) ? x : y;
    end;
    function max(x, y: integer): integer;
    begin
      Result := lessEqual(x, y) ? y : x;
    end;
  end;
  

function Max3<T, OrdT>(v1, v2, v3: T): T;
  where OrdT: Ord<T>, constructor;
begin
  var ordInst := __ConceptSingleton&<OrdT>.&Instance;
  
  Result := ordInst.max(v1, ordInst.max(v2, v3));
end;


begin
  writeln(Max3&<integer, Ord_integer>(1, 2, 3))
end.