unit interface_function1;

interface

procedure {@}p1(a: byte);
procedure {!}p1(a: byte; b: real);

implementation

procedure {!}p1(a: byte);
begin
  
end;

procedure {!}p1(a: byte; b: real);
begin
  {!}p1(2);
end;

end.