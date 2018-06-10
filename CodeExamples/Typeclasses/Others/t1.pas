type
  Ordering = (_EQ, _LT, _GT);
  
  IEq<T> = interface
    function eequal(x, y: T): boolean;
    function notEqual(x, y: T): boolean;
  end;
  
  
  Eq<T> = abstract class(IEq<T>)
  public 
    
    function eequal(x, y: T): boolean; virtual;
    begin
      Result := not notEqual(x, y);
    end;
    
    function notEqual(x, y: T): boolean; virtual;
    begin
      Result := not eequal(x, y);
    end;
  end;
  
  
  IOrd<T> = interface(IEq<T>)
    function compare(x, y: T): Ordering;
    function less(x, y: T): boolean;
    function lessEqual(x, y: T): boolean;
    function greater(x, y: T): boolean;
    function greaterEqual(x, y: T): boolean;
    function mmin(x, y: T): T;
    function mmax(x, y: T): T;
  end;
  
  
  Ord<T1, EqT> = abstract class
    where EqT: IEq<T1>, constructor;
  public 
    
    function eequal(x, y: T1): boolean; virtual;
    begin
      var eqInst := __ConceptSingleton&<EqT>.&Instance;
      Result := eqInst.eequal(x, y);
    end;
    
    function notEqual(x, y: T1): boolean; virtual;
    begin
      var eqInst := __ConceptSingleton&<EqT>.&Instance;
      Result := eqInst.notEqual(x, y);
    end;
  end;

function ArrayEq<T, EqT>(l1, l2: array of T): boolean; where EqT: IEq<T>, constructor;
begin
  var eqtinstance := __ConceptSingleton&<EqT>.&Instance;
  Result := true;
  for var i :=0 to l1.Length - 1 do
    if eqtinstance.notEqual(l1[i], l2[i]) then
    begin
      Result := false;
      break;
    end;
end;


function Max3<T, OrdT>(v1, v2, v3: T): T;
  where OrdT: IOrd<T>, constructor;
begin
  var ordInst := __ConceptSingleton&<OrdT>.&Instance;
  
  Result := ordInst.mmax(v1, ordInst.mmax(v2, v3));
end;


begin
 // writeln(Max3&<integer, Ord_integer<Eq_integer>>(1, 2, 3))
end.