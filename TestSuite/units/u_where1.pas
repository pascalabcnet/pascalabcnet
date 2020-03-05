unit u_where1;

var i: integer;

type
  t1<T> = class end;
  
procedure p0<T,TEl>(a: T); where T: t1<TEl>;
begin 
  i := 1;
end;

procedure p1;
begin
  p0&<t1<byte>,byte>(new t1<byte>);
end;
begin
  p1;
  assert(i = 1);
end.