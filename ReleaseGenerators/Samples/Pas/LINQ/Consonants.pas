// Вывод английских согласных
var vowel: string := 'aeiouy';

begin
  var all := Range('a','z').JoinIntoString('');
  all.Except(vowel).Println;
end.