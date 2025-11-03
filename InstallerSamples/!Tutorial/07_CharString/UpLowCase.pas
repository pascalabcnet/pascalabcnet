// Использование стандартных функций UpperCase, LowerCase

begin
  for var c:='a' to 'z' do
    Write(UpperCase(c));
  Writeln;
  for var c:='A' to 'Z' do
    Write(LowerCase(c));
  Writeln;
  for var c:='А' to 'Я' do
    Write(UpperCase(c));
  Writeln;
  for var c:='а' to 'я' do
    Write(LowerCase(c));
  Writeln;
  var s := 'Папа у Васи силён в математике';
  s := UpperCase(s);
  Writeln(s);
  s := LowerCase(s);
  Writeln(s);
end.
