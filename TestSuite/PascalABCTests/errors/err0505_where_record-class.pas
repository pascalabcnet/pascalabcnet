//!Невозможно инстанцировать, так как тип T не является ссылочным (классом)
##
procedure p0<T>(o: T); where T: class;
begin
  Writeln(o.GetType);
end;

procedure p1<T>(o: T); where T: record;
begin
  p0(o);
end;

p1(0);