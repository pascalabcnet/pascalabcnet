function f1(self:array of byte; a:byte):byte; extensionmethod := a+1;

procedure p1<T>(self:T; f:()->byte); extensionmethod;
begin
  assert(f = 1);
  assert(f() = 1);
end;

begin
  var a:array of byte;
  SetLength(a, 2);
  var b:byte;
  b.p1(()->a.f1(0));
  
end.