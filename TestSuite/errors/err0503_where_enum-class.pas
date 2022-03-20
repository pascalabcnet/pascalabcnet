//!Невозможно инстанцировать, так как тип T не является ссылочным (классом)
##
procedure p0<T>(o: T); where T: class;
begin
  Writeln(o.GetType);
end;

procedure p1<T>(o: T); where T: System.Enum;
begin
  p0(o);
end;

var o: (ME_1, ME_2) := ME_2;
p0(o);