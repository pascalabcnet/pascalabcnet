var a: integer;
procedure Test(p: integer);
begin
  a := 1;
end;
procedure Test<T>(p: T);
begin
  a := 2;
end;
procedure Test(params p: array of integer);
begin
  a := 3;
end;
procedure Test<T>(params p: array of T);
begin
  a := 4;
end;
begin
  Test(3);
  assert(a = 1);
  Test('a');
  assert(a = 2);
  Test(byte(3));
  assert(a = 2);
  Test(1,2);
  assert(a = 3);
  Test('a','b');
  assert(a = 4);
end.