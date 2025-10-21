// Строки. Операция + и оператор +=. Доступ по индексу

begin
  var s := 'Pascal';
  Println(s);
  s := s + '.';
  Println(s);
  s += 'NET';
  Println(s);
  Println(s[1], s[2], s[3], s[4], s[5], s[6]);
end.