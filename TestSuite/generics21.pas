type
  t1<T>=class end;
  t2=class(t1<byte>) end;
  t3 = class(t2) end;
  
var i: integer;

procedure p1<T>(self:t1<T>);
begin
  i := 1;
end;
procedure p2<T>(self:t1<T>); extensionmethod;
begin
  i := 2;
end;
procedure p3<T>(self:t1<T>); extensionmethod;
begin
  i := 2;
end;
procedure p3(self:t3); extensionmethod;
begin
  i := 3;
end;
begin
  var a := new t2;
  p1(a);
  assert(i = 1);
  a.p2;
  assert(i = 2);
  var b := new t3;
  i := 0;
  p1(b);
  assert(i = 1);
  b.p2;
  assert(i = 2);
  b.p3;
  assert(i = 3);
  a.p3;
  assert(i = 2);
end.