var s: string;
    i: integer;
begin
  s := 'ABCDEFGH';
  s := s + 'IJK';
  for i:=1 to Length(s) do
    write(s[i],' ');
  writeln;
  Str(12345,s);
  
  s := '';
  for i:=1 to 10 do
    s := s + 'a';
  writeln(s);
end.