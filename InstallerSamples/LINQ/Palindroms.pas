// Вывести все палиндромы в строке, упорядоченные по длине

begin
  var s := ' hello  aha paap   zz ';
  s.ToWords.Where(w -> w.Inverse = w).OrderBy(s->s.Length).Println(',');
end.