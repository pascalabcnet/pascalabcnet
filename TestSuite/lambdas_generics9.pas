type
  t0 = abstract class end;
  
  t1<T> = class
  where T: t0, constructor;
    i: byte;
    function MakeT := new T;
    
    function f1(x: byte): ()->T := ()->
    begin
      
      i := x;
      
      Result := MakeT;
    end;
    
  end;
  
  t2 = class(t0) end;
  
begin
  var a := new t1<t2>;
  var obj := a.f1(5)();
  assert(a.i = 5);
end.