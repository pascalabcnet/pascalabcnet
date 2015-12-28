begin
  var dct := Dict(KV('цвет','color'));
  foreach var s in ReadLines('translations.txt') do
  begin
    var ss := s.ToWords;
    dct.Add(ss[0],ss[1]);
  end;
  writeln(dct);
  
  var word := ReadString('Введите слово:');
  if dct.ContainsKey(word) then
    Println('Перевод:',dct[word])
  else Println('Нет такого слова!')  
end.