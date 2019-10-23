function f: integer;
begin
  result := 1;
end;

begin
  var a: integer;
  a := f + nil;
  Print(a);
end.