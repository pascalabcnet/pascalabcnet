// Таблица символов и их кодов
begin
  for var i:=32 to 255 do
  begin
    write(i:4,chr(i):2,'  ');
    if i mod 8 = 7 then writeln;
  end;
end.
