// Разделитель AllDelimiters содержит все основные символы, 
// которые могут служить разделителями между словами
begin
  var s := '123@!45  @#$678\/|?90~;';
  s.ToWords(AllDelimiters).Println;
end.

