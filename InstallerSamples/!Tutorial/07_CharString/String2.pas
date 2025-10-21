// Строки. Процедура SetLength, функция Length и свойство Length 

begin
  var s := '';
  Println('Length(s) =', s.Length);
  SetLength(s, 3);
  s[1] := 'N';
  s[2] := 'E';
  s[3] := 'T';
  Println(s, '     Length(s) =', s.Length);
  for var i := 1 to s.Length do
    Print(s[i]);
  Println;
end.