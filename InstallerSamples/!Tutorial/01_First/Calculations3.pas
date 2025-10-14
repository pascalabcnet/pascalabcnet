// Вывод результатов вычислений. Используются переменные и процедура ввода

begin
  var (a,b) := ReadlnInteger2('Введите a и b:');
  Println;
  Println(a,'+',b,'=',a+b);
  Println(a,'-',b,'=',a-b);
  Println(a,'*',b,'=',a*b);
  Println(a,'/',b,'=',a/b);
end.
