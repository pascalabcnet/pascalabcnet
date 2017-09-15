procedure Test;
begin
  abc{@@} := 2;  
end;
type TClass = class
procedure Test;
begin
  a{@var TClass.a: integer;@} := 2;
end;
public a: integer;
end;
var abc: integer;
begin
  abc := 3;
end.