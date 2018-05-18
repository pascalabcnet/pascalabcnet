type
  IEq<T> = interface
    function equal(x, y: T): boolean;
  end;
  
  Ord<T1, EqT> = abstract class
    where EqT: IEq<T1>, constructor;
  public 
    
    function equal(x, y: T1): boolean; virtual;
    begin
      var eqInst := new EqT;
      Result := eqInst.equal(x, y);
    end;
  end;

function equal<T, EqT>(v1, v2: T): boolean; where EqT: IEq<T>, constructor;
begin
  var eqtinstance := new EqT;
  Result := eqtinstance.equal(v1, v2);
end;

begin
end.