begin
  var s := 'красный оранжевый желтый зеленый голубой синий фиолетовый';
  var ss: array of string := s.ToWords;
  ss.Println;
  Reverse(ss);
  ss.Println;
  Sort(ss);
  ss.Println;
  s := string.Join('-',ss);
  writeln(s);
end.