  	
type
  c = class
    procedure p(v: integer);
    procedure p(var v: integer);
  end;

procedure c.p(v: integer);
begin
end;
procedure c.p(var v: integer);
begin
end;

begin

end.