uses PointerTools;

var
  v: integer;
  
begin
  write('Введите целое число: ');
  readln(v);
  WriteMemoryToScreen(@v, sizeof(integer));
  readln;
end.

