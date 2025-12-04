type
  t1 = class end;
  t2 = class(t1) end;
  
procedure p1<T>(a: t1); where T: class;
begin
  var x: T;
  if x is t1 then
    Print(1);
  
  if a is T(var b) then
    if b is t2(var c) then
      Assert(c=c);
end;

begin
  var a: t1 := new t2;
  p1&<t2>(a);
end.