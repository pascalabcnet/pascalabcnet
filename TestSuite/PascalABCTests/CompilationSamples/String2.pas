// Строки. Процедура SetLength, функция Length и свойство Length 

var s: string;

begin
  writeln('Length(s) = ',Length(s));
  SetLength(s,3);
  s[1] := 'N';
  s[2] := 'E';
  s[3] := 'T';
  writeln(s,'     Length(s) = ',Length(s));
  for var i:= 1 to s.Length do
    write(s[i],' ');
end.