type
  t0<T> = class end;
  
  t1 = class 
    function f1<T>(q: t0<T>) := default(T);
    
    procedure p1<T>;
    begin
      var a: t0<T>;
      var f: Func0<T> := ()->self.f1(a);
    end;
  end;

begin 
end.
