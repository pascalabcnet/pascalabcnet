var i: integer;
procedure p1<T>(a: array of T);
begin
  i := 1;  
end;
procedure p1<T>(a: T);
begin
  i := 2;
end;

procedure p2<T>(a: array[,] of T);
begin
  i := 1;  
end;
procedure p2<T>(a: T);
begin
  i := 2;
end;


begin
  var a := new byte[5];
  p1(a);
  assert(i = 1);
  i := 0;
  p2(a);
  assert(i = 2);
end.