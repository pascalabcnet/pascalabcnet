uses PointerTools;

var
  v: integer;
  
begin
  write('¬ведите целое число: ');
  readln(v);
  WriteMemoryToScreen(@v, sizeof(integer));
  readln;
end.

