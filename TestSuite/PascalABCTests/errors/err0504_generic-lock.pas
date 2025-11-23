//!Выражение должно возвращать ссылочный тип, как того требует оператор lock
##
procedure p1<T>(o: T);
begin
  lock o do
  begin
    Writeln(o);
  end;
end;

p1(0);