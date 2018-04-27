type
  Ordering = (_EQ, _LT, _GT);
  
  Eq<T> = abstract class
  public
  
    function equal(x, y: T): boolean; virtual;
    begin
      Result := not notEqual(x, y);
    end;
    
    function notEqual(x, y: T) :boolean; virtual;
    begin
      Result := not equal(x, y);
    end;
  end;
  
  Ord<T, EqT> = abstract class
    where EqT: Eq<T>;
  public

    
    function compare(x, y: T): Ordering; virtual;
    begin
      var eqInst := __ConceptSingleton&<EqT>.&Instance;
      if eqInst.equal(x, y) then
        Result := _EQ
      else if less(x, y) then
        Result := _LT
      else
        Result := _GT;
    end;
    function less(x, y: T): boolean; virtual;
    begin
      Result := compare(x, y) = _LT;
    end;
    function lessEqual(x, y: T): boolean; virtual;
    begin
      Result := compare(x, y) <> _GT;
    end;
    function greater(x, y: T): boolean; virtual;
    begin
      Result := compare(x, y) = _GT;
    end;
    function greaterEqual(x, y: T): boolean; virtual;
    begin
      Result := compare(x, y) <> _LT;
    end;
    function min(x, y: T): T; virtual;
    begin
      Result := lessEqual(x, y) ? x : y;
    end;
    function max(x, y: T): T; virtual;
    begin
      Result := lessEqual(x, y) ? y : x;
    end;
  end;
  
  
  Eq_integer = class(Eq<integer>)
  public  
  
    function equal(x, y: integer): boolean; override;
    begin
       Result := x = y;
    end;
  end;
  

  Ord_integer<EqT> = class(Ord<integer, EqT>)
    where EqT: Eq<integer>, constructor;
  public
  
    function compare(x, y:integer): Ordering; override;
    begin
      var eqInst := __ConceptSingleton&<EqT>.&Instance;
      if eqInst.equal(x, y) then
        Result := _EQ
      else if x < y then
        Result := _LT
      else
        Result := _GT;
    end;
  end;
  

function Max3<T, EqT, OrdT>(v1, v2, v3: T): T;
  where EqT: Eq<T>, constructor;
  where OrdT: Ord<T, EqT>, constructor;
begin
  var eqInst := __ConceptSingleton&<EqT>.&Instance;
  var ordInst := __ConceptSingleton&<OrdT>.&Instance;

  Result := ordInst.max(v1, ordInst.max(v2, v3));
end;


begin
  writeln(Max3&<integer, Eq_integer, Ord_integer<Eq_integer>>(1, 2, 3))
end.