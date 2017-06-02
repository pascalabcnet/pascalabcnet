// Строки. Операция + и оператор +=. Доступ по индексу

var s: string := 'Pascal';

begin
  writeln(s);
  s := s + '.';
  writeln(s);
  s += 'NET';
  writeln(s);
  writeln(s[1],' ',s[2],' ',s[3],' ',s[4],' ',s[5],' ',s[6]);
end.