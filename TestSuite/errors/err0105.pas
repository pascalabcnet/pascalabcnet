type TClass = class
procedure Test;
begin
end;
end;

type TDer = class(TClass)
procedure Test; override;
begin
end;
end;

begin
end.