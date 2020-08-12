type
  t1 = class end;
  
procedure p1<T>; where T: class;
begin
  var a := new t1;
  var b := a as T;
  Assert(b <> nil);
end;

begin 
  p1&<t1>();
end.