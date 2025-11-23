var i: integer;

procedure p(b: integer; params a:array of integer);
begin
  i := b;
  if a.Length > 0 then
    i := a[0];
end;

procedure p4(b: integer; b2: integer; params a:array of integer);
begin
  i := b;
  if a.Length > 0 then
    i := a[0];
end;

procedure p2(self: object; b: integer; params a:array of integer); extensionmethod;
begin
  i := b;
  if a.Length > 0 then
    i := a[0];
end;

procedure p3(self: object; params a:array of integer); extensionmethod;
begin
  i := 4;
end;

begin
  p(2);
  assert(i = 2);
  p(2,1,3);
  assert(i = 1);
  var o := new object;
  o.p2(2);
  assert(i = 2);
  o.p2(2,1,3);
  assert(i = 1);
  o.p3;
  assert(i = 4);
  p4(2,2);
  assert(i = 2);
  p4(2,1,3);
  assert(i = 3);
end.