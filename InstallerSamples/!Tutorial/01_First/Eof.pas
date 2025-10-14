// Демонстрация работы функции eof. Запускать по Shift-F9, завершать по Ctrl-Z
// Ctrl-Z не работает с 3.9
begin
  while not Eof do
  begin
    var s := ReadChar;
    Println('-', Ord(s), '-',CurrentIOSystem.peek);
  end;
end.