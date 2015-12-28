// Стандартные CRT-цвета
// Работа с модулем CRT

uses CRT;

var i,j: integer;
  
begin
  for i:=0 to 15 do
  begin
    TextBackground(i);
    for j:=0 to 15 do
    begin
      TextColor(j);
      write(' CRT');
    end;
    writeln;
  end;
  readkey;
end.
