// Демонстрация работы функции eof. Запускать по Shift-F9
var s: char;
begin
  while not eof do
  begin
    read(s);
    writeln('- ',Ord(s),' -');
  end;
end.
