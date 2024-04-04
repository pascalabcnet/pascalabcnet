procedure p(s: string; x: integer := 1; y: integer := 2; z: integer := 3);
begin
  Print(s,x,y,z);
end;

begin
  p('Hello', z := 33, y := 22);
end.
