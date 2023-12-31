type
  c1<T1> = class
  where T1: IComparable<T1>;
    
    function f1<T2>(o: T2): integer;
      where T2: T1;
    begin
      Result := {T1}(o).CompareTo({T1}(o));
    end;
    
  end;
  
begin
  var a := new c1<string>;
  assert(a.f1('abc') = 0);
end.