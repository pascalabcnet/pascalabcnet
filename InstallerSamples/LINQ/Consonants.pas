// Вывод английских согласных
var vowel: string := 'aeiouy';

begin
  var all := Range('a','z').Aggregate('',(s,x)->s+x);
  all.Except(vowel).Println;
end.