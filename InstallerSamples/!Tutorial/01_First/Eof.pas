// Демонстрация работы функции eof. Запускать по Shift-F9, завершать по Ctrl-Z
begin
  var s: char;
  while not Eof do
  begin
    Read(s);
    Println('-',Ord(s),' -');
  end;
end.
