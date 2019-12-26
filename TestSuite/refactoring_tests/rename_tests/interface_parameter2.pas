unit interface_parameter2;

interface

procedure p1({@}a: byte);
procedure p1(a: byte; b: real);

implementation

procedure p1({!}a: byte);
begin
  
end;

procedure p1(a: byte; b: real);
begin
  
end;

end.