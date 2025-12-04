type
  t1<T> = class
    
    function f<T2>(o: T2)  := default(T);
    
  end;
  
function f<T>: T;
begin
  var o: t1<T> := new t1<T>;
  Result := o.f(0);
end;

begin 
  assert(f&<integer> = 0);
end.