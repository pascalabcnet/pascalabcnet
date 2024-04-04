var i: integer;

procedure p0(p: byte->());
begin
  Inc(i);
end;
procedure p1<T>(o: T);
begin
  Inc(i);
end;

begin
  p0(p1&<byte>);
  //Ошибка: Нельзя преобразовать тип procedure(o: T) к Action<byte>
  p0(p1);
  assert(i = 2);
end.