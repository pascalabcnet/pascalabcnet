var byte: byte; // работает

procedure p(integer: integer); // не работает.
begin
end;

type T<x> = class
end;

procedure p(t: t<integer>); // работает
begin
end;

procedure p(list: list<integer>); // не работает
begin 
end;

begin
  
end.