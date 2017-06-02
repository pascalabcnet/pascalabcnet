// Создание кортежей
uses System;

begin
  var a := Tuple.Create('Иванов',24);
  writeln(a.Item1,' ',a.Item2);
  
  var b := Tuple.Create('Петров',17,1,11);
  writeln(b.Item1,' ',b.Item2,' ',b.Item3,' ',b.Item4);
end.