// Использование стандартных функций UpperCase, LowerCase
var c: char;

begin
  for c:='a' to 'z' do
    write(UpperCase(c));
  writeln;
  for c:='A' to 'Z' do
    write(LowerCase(c));
  writeln;
  for c:='А' to 'Я' do
    write(UpperCase(c));
  writeln;
  for c:='а' to 'я' do
    write(LowerCase(c));
  writeln;
  var s := 'Папа у Васи силён в математике';
  s := UpperCase(s);
  writeln(s);
  s := LowerCase(s);
  writeln(s);
end.
