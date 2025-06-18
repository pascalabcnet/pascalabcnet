// Вывод английских согласных
begin
  var vowel: string := 'aeiouy';

  var all := ('a'..'z').JoinToString;
  all.Except(vowel).Println;
end.