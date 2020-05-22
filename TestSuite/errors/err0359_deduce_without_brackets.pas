type
  t0<T> = class
    procedure p1 := exit;
  end;
  
  t1 = class
    function f1<T>: t0<T> := nil;
  end;
  
begin
  var b := new t1;
  b.f1.p1;
  
end.