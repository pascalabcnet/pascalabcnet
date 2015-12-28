var s: string;
  
begin
  s := 'ABCDEFGH';
  s += 'IJK';
  foreach var c in s do
    Print(c);
  Println;
  s := ''+12345; // число преобразуется в строку
  writeln('a'*10); // строка повторяется 10 раз
end.