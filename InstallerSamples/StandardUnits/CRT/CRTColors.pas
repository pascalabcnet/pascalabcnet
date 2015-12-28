// Стандартные CRT-цвета
// Для запуска программы используйте Shift+F9 !!!
uses CRT;

begin
  SetWindowTitle('Стандартные CRT-цвета');
  for var i:=0 to 15 do
  begin
    TextBackground(i);
    for var j:=0 to 15 do
    begin
      TextColor(j);
      write(' CRT');
    end;
    writeln;
  end;
  HideCursor;
  TextBackground(0);
  TextColor(0);
end.
